using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class HttpListenerDemo : MonoBehaviour
{
	[SerializeField] private int _port = 7000;
	[SerializeField] private string _ip = "192.168.56.1";
	private string _redirectURL = "";
	private HttpListener _listener = default;
	private Stream responseOutput;
	private HttpListenerResponse _response;

	private void Start()
	{
		_redirectURL = $"http://{_ip}:{_port}/";

		_listener = new();

		try
		{
			_listener.Prefixes.Add(_redirectURL);
			_listener.Start();
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
			return;
		}

		Task.Run(async () =>
		{
			try
			{
				var context = await _listener.GetContextAsync();
				_response = context.Response;

				// 受け取ったリダイレクトURLをログに出力する
				Debug.Log($"redirectUri: {context.Request.Url}");

				// 受け取ったリダイレクトURLのクエリパラメータからcodeを取得する
				var query = context.Request.Url.Query;
				var code = HttpUtility.ParseQueryString(query).Get("code");

				// リダイレクトURLで開くページのレスポンスページに取得したcodeを表示する
				var responseString =
					$"<p><a href=\"./screenshot/ss001.png\" download=\"テスト.jpg\">Download</a></p>";
				var buffer = Encoding.UTF8.GetBytes(responseString);

				_response.ContentLength64 = buffer.Length;
				responseOutput = _response.OutputStream;
				await responseOutput.WriteAsync(buffer, 0, buffer.Length);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				throw;
			}
		});
	}

	private void OnDestroy()
	{
		_response.Close();
		responseOutput?.Close();
		_listener?.Stop();
	}
}
