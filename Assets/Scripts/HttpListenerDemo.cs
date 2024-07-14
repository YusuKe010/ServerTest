using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class HttpListenerDemo : MonoBehaviour
{
	[SerializeField] private int _port = 7000;
	private string _redirectURL = "";
	private HttpListener _listener = default;
	private Stream _responseOutput;
	private HttpListenerResponse _response;
	private byte[] _byteArr;
	private StreamWriter _streamWriter;
	private FileStream fileSteam;
	private TextFileReader _textFile;

	private async void Start()
	{
		fileSteam = new FileStream(@".\Assets\screenshot\Movie.mp4", FileMode.Open, FileAccess.Read);
		_byteArr = new byte[fileSteam.Length];
		await fileSteam.ReadAsync(_byteArr, 0, _byteArr.Length);
		Debug.Log(_byteArr);


		_redirectURL = $"http://*:{_port}/";

		_listener = new();

		try
		{
			_listener.Prefixes.Add(_redirectURL);
			_listener.Start();
			Debug.Log("Listenスタート");
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
			return;
		}

		Access();
		// 	await Access();
	}

	private void OnDestroy()
	{
		_response.Close();
		_responseOutput?.Close();
		_listener?.Stop();
		Debug.Log("終わり");
		//fileSteam.Close();
	}

	void Access()
	{
		Task.Run(async () =>
		{
			try
			{
				var context = await _listener.GetContextAsync();
				_response = context.Response;
				_response.ContentType = "video/mp4";


				// 受け取ったリダイレクトURLをログに出力する
				Debug.Log($"redirectUri: {context.Request.Url}");

				_response.ContentLength64 = _byteArr.Length;
				_responseOutput = _response.OutputStream;
				await _responseOutput.WriteAsync(_byteArr, 0, _byteArr.Length);

				Debug.Log(_responseOutput);

				_response.Close();
				Access();
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				throw;
			}
		});
	}
}
