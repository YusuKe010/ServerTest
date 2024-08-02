using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class UrlData
{
	[SerializeField] private int _port;
	[SerializeField] private string _ip;
	[SerializeField] private string _url;

	public string Url
	{
		get
		{
			if (_url == "")
				return $"http://{_ip}:{_port}/";
			return _url;
		}
	}

	public int Port => _port;
	public string Ip => _ip;
}

public class HttpListenerDemo : MonoBehaviour
{
	[SerializeField] private UrlData _urlData;
	[SerializeField] private string _movieFileURL = @".\Assets\screenshot\Movie.mp4";

	private byte[] _byteArr;
	private FileStream _fileSteam;

	private HttpListener _listener;
	private HttpListenerResponse _response;
	private Stream _responseOutput;

	/// <summary>
	///     動画のファイルの場所
	/// </summary>
	public string MovieFileURL
	{
		get => _movieFileURL;
		set => _movieFileURL = value;
	}

	private async void Start()
	{
		await FileToByte();
		ListenerStart();
	}

	private void OnDestroy()
	{
		ListenerStop();
		Debug.Log("Listen終了");
	}


	private async Task FileToByte()
	{
		if (_movieFileURL == "")
			return;
		try
		{
			_fileSteam = new FileStream(_movieFileURL, FileMode.Open, FileAccess.Read);
			_byteArr = new byte[_fileSteam.Length];
			await _fileSteam.ReadAsync(_byteArr, 0, _byteArr.Length);
			//Debug.Log(_byteArr);
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
	}

	public void ListenerStart()
	{
		_listener = new HttpListener();
		try
		{
			_listener.Prefixes.Add(_urlData.Url);
			_listener.Start();
			Debug.Log("Listenスタート");
			Debug.Log(_urlData.Url);
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
			return;
		}

		CreateWeb();
	}

	private void ListenerStop()
	{
		_response?.Close();
		_responseOutput?.Close();
		_listener?.Stop();
	}

	private void CreateWeb()
	{
		if (_byteArr == null)
			return;

		Task.Run(async () =>
		{
			//QRコードを読み込むのを待つ
			var context = await _listener.GetContextAsync();

			// 受け取ったリダイレクトURLをログに出力する
			Debug.Log($"redirectUri: {context.Request.Url}");
			// 動画ファイルの更新
			//await FileToByte();

			//サーバーの設定
			_response = context.Response;
			_response.ContentType = "video/mp4";
			_response.ContentLength64 = _byteArr.Length;
			_responseOutput = _response.OutputStream;
			await _responseOutput.WriteAsync(_byteArr, 0, _byteArr.Length);

			_response.Close();
			CreateWeb();
		});
	}
}
