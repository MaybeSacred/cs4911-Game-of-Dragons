using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Interface for behaviors that need to save
/// and restore some state when the player
/// reaches checkpoints.
/// </summary>
public interface IResettable
{
	/// <summary>
	/// Save the state.
	/// </summary>
	void SaveState();

	/// <summary>
	/// Reset the state.
	/// </summary>
	void Reset();
}