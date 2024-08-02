using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreRoot : MonoBehaviour
{
	[SerializeField] private Environment _environment;

	[SerializeField] [SerializeReference] [SubclassSelector(true)]
	private List<IComposite> _composites;
}
