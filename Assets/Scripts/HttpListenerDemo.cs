using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class UrlData
{
    [SerializeField] int _port;
    [SerializeField] string _ip;
    [SerializeField] string _url;
    public string Url => $"http://{_ip}:{_port}/";
    public int Port => _port;
    public string Ip => _ip;
}

public class HttpListenerDemo : MonoBehaviour
{
    [SerializeField] UrlData _urlData;
    [SerializeField] string _movieFileURL = @".\Assets\screenshot\Movie.mp4";

    private HttpListener _listener = default;
    private HttpListenerResponse _response;
    private FileStream fileSteam;
    private Stream _responseOutput;

    private byte[] _byteArr;

    private async void Start()
    {
        await FileToByte();

        ListenerStart();
    }

    async Task FileToByte()
    {
        if (_movieFileURL == "")
            return;
        try
        {
            fileSteam = new FileStream(_movieFileURL, FileMode.Open, FileAccess.Read);
            _byteArr = new byte[fileSteam.Length];
            await fileSteam.ReadAsync(_byteArr, 0, _byteArr.Length);
            Debug.Log(_byteArr);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void OnDestroy()
    {
        _response?.Close();
        _responseOutput?.Close();
        _listener?.Stop();
        Debug.Log("Listen終了");
    }

    void ListenerStart()
    {
        _listener = new();
        try
        {
            _listener.Prefixes.Add(_urlData.Url);
            _listener.Start();
            Debug.Log("Listenスタート");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }

        CreateWeb();
    }

    void CreateWeb()
    {
        if(_byteArr == null) 
            return;

        Task.Run(async () =>
        {
            var context = await _listener.GetContextAsync();
            _response = context.Response;
            _response.ContentType = "video/mp4";

            // 受け取ったリダイレクトURLをログに出力する
            Debug.Log($"redirectUri: {context.Request.Url}");

            _response.ContentLength64 = _byteArr.Length;
            _responseOutput = _response.OutputStream;
            await _responseOutput.WriteAsync(_byteArr, 0, _byteArr.Length);

            _response.Close();
            CreateWeb();
        });
    }
}
