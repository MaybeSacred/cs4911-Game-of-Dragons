using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoBuilder : MonoBehaviour {

	void Start () {

	}
	
	void Update () {
	
	}
	static void PerformBuild ()
	{
		
		string[] scenes = {"Scene0.unity"};
		//BuildPipeline.BuildPlayer(scenes,"win.exe",BuildTarget.StandaloneWindows,BuildOptions.None);
		//BuildPipeline.BuildPlayer(scenes,"osx.app",BuildTarget.StandaloneOSXUniversal,BuildOptions.None);
		BuildPipeline.BuildPlayer(scenes,"WebPlayer.unity3d",BuildTarget.WebPlayerStreamed,BuildOptions.Development);
		scenes = null;
		
	}
}
