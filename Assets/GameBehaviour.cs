using UnityEngine;
using System.Collections.Generic;

public class GameBehaviour : MonoBehaviour
{
	protected virtual void Start()
	{
		IResettable resettable = this as IResettable;
		if (resettable == null) 
		{
			WorldScript world = (WorldScript)(GameObject.Find("World").GetComponent("WorldScript"));
			world.objectsToReset.Add(resettable);
		}
	}
}