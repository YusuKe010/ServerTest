using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
	public class Sample : MonoBehaviour
	{
		private void Start()
		{
			Debug.Log("m1begin");
			M1();
			Debug.Log("m1End");
		}

		void M1()
		{
			Debug.Log("m2begin");
			M2();
			Debug.Log("m2End");
		}

		void M2()
		{
			Debug.Log("m3");
			
		}
	}
}
