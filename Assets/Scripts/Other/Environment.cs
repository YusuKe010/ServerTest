using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Environment
{
	public GameObject mySelf;
	public GameObject target;
	private List<IComposite> visit = new();

	public bool Visit(IComposite node)
	{
		if (visit.Where(n => n == node).Count() == 0)
		{
			visit.Add(node);
			return true;
		}

		return false;
	}

	public void Leave(IComposite node)
	{
		var n = visit.Where(n => n == node);
		if (n.Count() == 0) return;

		visit.Remove(n.Single());
	}
}
