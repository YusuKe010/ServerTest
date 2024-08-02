using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionTest : MonoBehaviour
{
	// Start is called before the first frame update
	private void Start()
	{
		StartCoroutine(UPloadFile());
	}

	// Update is called once per frame
	private void Update()
	{
	}

	private IEnumerator UPloadFile()
	{
		var fileName = "Movie.mp4";
		var filePath = @".\Assets\screenshot" + @"\" + fileName;

		// 画像ファイルをbyte配列に格納
		var img = File.ReadAllBytes(filePath);

		// formにバイナリデータを追加
		var form = new WWWForm();
		form.AddBinaryData("image", img, fileName, "image.mp4");

		// HTTPリクエストを送る
		var request = UnityWebRequest.Post("http://192.168.56.1:7000", form);
		yield return request.SendWebRequest();

		//"Save Texture"ダイアログを表示し、選択されたパスを取得する
		var path = EditorUtility.SaveFilePanelInProject("Save Texture", "test",
			"png", "Save Texture");

		//新規の空テクスチャを作成する
		var texture = new Texture2D(1, 1);
		var bytes = Convert.FromBase64String(request.downloadHandler.text);
		texture.LoadImage(bytes);
		var png = texture.EncodeToJPG();

		//PNG形式でエンコード
		File.WriteAllBytes(path, png);

		//オブジェクトに画像を表示
		GetComponent<Renderer>().material.mainTexture = texture;
	}
}
