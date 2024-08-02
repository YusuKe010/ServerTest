using System.Collections.Generic;
using UnityEngine;

public class Selector : IComposite
{
	[SerializeField] [SerializeReference] [SubclassSelector(true)]
	private List<IComposite> _composites = new();

	public void Operation(Environment env)
	{
	}
}
