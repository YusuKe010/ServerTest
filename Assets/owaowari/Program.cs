using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

internal class Program : MonoBehaviour
{
	private static readonly int _timeOutMiliSec = 1000;

	private void Start()
	{
		Debug.Log("Run Start");
		Run();
	}

	private void Run()
	{
		Task.Run(async () =>
		{
			while (true)
			{
				RunningLogic();
				await Task.Delay(_timeOutMiliSec);
			}
		});
	}

	private void RunningLogic()
	{
		var ping = new Ping();
		for (int i = 0; i <= 255; i++)
		{
			for (int j = 0; j <= 255; j++)
			{
				try
				{
					SearchPingStatus(ping.Send(IPAddress.Parse($"192.168.{i}.{j}"), _timeOutMiliSec));
				}
				catch (Exception exception)
				{
					Debug.Log(exception.Message);
				}
			}
		}
	}

	private void SearchPingStatus(PingReply reply)
	{
		if (reply.Status == IPStatus.Success)
		{
			Debug.Log("Address: " + reply.Address.ToString());
			Debug.Log("RoundTrip time: " + reply.RoundtripTime);
		}
	}
}
