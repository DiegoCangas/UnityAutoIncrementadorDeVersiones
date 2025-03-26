using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AutoIncrementBuildVersion : IPreprocessBuildWithReport
{
	public int callbackOrder { get { return 0; } }
	public void OnPreprocessBuild (BuildReport buildTarget)
	{
		
	    string currentVersion = PlayerSettings.bundleVersion;

		try
		{
			if (!EditorUtility.DisplayDialog("","¿Desea actualzar la version y el codigo de version?", "Si","No")) return;
			int tipoDeVersion = EditorUtility.DisplayDialogComplex("Version",
				"¿Que tipo de actualización intenta compilar?", "Mayor (X.0.0)", "Parche (0.0.X)", "Menor (0.X.0)");
			int major = Convert.ToInt32 (currentVersion.Split ('.') [0]);
			int minor = Convert.ToInt32 (currentVersion.Split ('.') [1]);
			int build = Convert.ToInt32 (currentVersion.Split ('.') [2]);

			if (tipoDeVersion == 0) major++;
			else if (tipoDeVersion == 1) build++;
			else if (tipoDeVersion == 2) minor++;
			
			PlayerSettings.bundleVersion = major + "." + minor + "." + build;

			if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS) {
				PlayerSettings.iOS.buildNumber = ""+int.Parse(PlayerSettings.iOS.buildNumber)+1;
				UnityEngine.Debug.Log ("BuidNumber:" + PlayerSettings.iOS.buildNumber + " y versión" + PlayerSettings.bundleVersion);

			} else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) {
				++PlayerSettings.Android.bundleVersionCode;
				UnityEngine.Debug.Log ("BundelVersion:" + PlayerSettings.Android.bundleVersionCode + " y versión" + PlayerSettings.bundleVersion);
			}
			else
			{
				UnityEngine.Debug.Log ("Version: " + PlayerSettings.bundleVersion);
			}
			// It's important that you do not chane your project settings during a build in the cloud.
		} catch (Exception e) {
			UnityEngine.Debug.LogError (e);
			UnityEngine.Debug.LogError ("AutoIncrementBuildVersion script failed. Make sure your current bundle version is in the format X.X.X (e.g. 1.0.0) and not X.X (1.0) or X (1).");
		}
	
	}
}



