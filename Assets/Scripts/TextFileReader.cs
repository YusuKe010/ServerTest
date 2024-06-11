using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace name
{
	///<summary>summary</summary>
	public class TextFileReader : MonoBehaviour
	{
		[SerializeField] private TextAsset _textFile;
		[SerializeField] private Text _text;

		private List<string> textData = new List<string>();

		private void Start()
		{
			StringReader render = new StringReader(_textFile.text);
			while (render.Peek() != -1)
			{
				string a = render.ReadLine();
				textData.Add($"{a}\n");
				_text.text += $"{a}\n";
			}
		}
	}
}
