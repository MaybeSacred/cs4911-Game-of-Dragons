using UnityEngine;
using System.Collections;
using System;

public interface IResettable
{
	void SaveState();
	void Reset();
}