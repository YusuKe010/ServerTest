using System.IO;
using System.Threading.Tasks;
using UnityEngine;

///<summary>summary</summary>
public class TextFileReader : MonoBehaviour
{
	[SerializeField] private TextAsset _textFile;
	[SerializeField] private string _text;

	public string Text => _text;

	private async void Start()
	{
		await StringRead();
	}

	public Task StringRead()
	{
		return Task.Run(async () =>
		{
			var render = new StringReader(_textFile.text);
			while (render.Peek() != -1)
			{
				var a = await render.ReadLineAsync();
				_text += $"{a}\n";
			}
		});
	}
}
