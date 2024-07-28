using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityGoogleDrive;

///<summary>summary</summary>
public class GoogleDrive : MonoBehaviour
{
	[SerializeField] private string _fileName = "Movie.mp4";
	private byte[] _byteArr;
	private FileStream _fileStream;

	private void Start()
	{
		Access();
	}

	void Access()
	{
		Task.Run(async () =>
		{
			_fileStream = new FileStream(@".\Assets\screenshot\Movie.mp4", FileMode.Open, FileAccess.Read);
			_byteArr = new byte[_fileStream.Length];
			await _fileStream.ReadAsync(_byteArr, 0, _byteArr.Length);

			UnityGoogleDrive.Data.File file = new UnityGoogleDrive.Data.File();
			file.Name = _fileName;
			file.Content = _byteArr;

			var request = GoogleDriveFiles.Create(file);

			request.Send().OnDone += (responce) =>
			{
				if (request.IsError)
				{
					Debug.LogError(request.Error);
				}
				else
				{
					Debug.Log(request.ResponseData.Name);
				}
			};
		});
	}

	private void OnDestroy()
	{
		_fileStream?.Close();
	}
}
