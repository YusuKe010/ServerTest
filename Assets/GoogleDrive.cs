using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityGoogleDrive;
using File = UnityGoogleDrive.Data.File;

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

	private void OnDestroy()
	{
		_fileStream?.Close();
	}

	private void Access()
	{
		Task.Run(async () =>
		{
			_fileStream = new FileStream(@".\Assets\screenshot\Movie.mp4", FileMode.Open, FileAccess.Read);
			_byteArr = new byte[_fileStream.Length];
			await _fileStream.ReadAsync(_byteArr, 0, _byteArr.Length);

			var file = new File();
			file.Name = _fileName;
			file.Content = _byteArr;

			var request = GoogleDriveFiles.Create(file);

			request.Send().OnDone += responce =>
			{
				if (request.IsError)
					Debug.LogError(request.Error);
				else
					Debug.Log(request.ResponseData.Name);
			};
		});
	}
}
