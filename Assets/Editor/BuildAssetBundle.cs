using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem ("Assets/Build AssetBundles/Android")]
    static void BuildAndroidAssetBundles ()
    {
		var path = Application.dataPath + "/ExportAssetBundles/Android";
		if (Directory.Exists (path) == false) { 
			Directory.CreateDirectory (path);
		}
		BuildPipeline.BuildAssetBundles ("Assets/ExportAssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

	[MenuItem ("Assets/Build AssetBundles/iOS")]
	static void BuildIOSAssetBundles ()
	{
		var path = Application.dataPath + "/ExportAssetBundles/iOS";
		if (Directory.Exists (path) == false) { 
			Directory.CreateDirectory (path);
		}
		BuildPipeline.BuildAssetBundles ("Assets/ExportAssetBundles/iOS", BuildAssetBundleOptions.None, BuildTarget.Android);
	}

	[MenuItem ("Assets/Build AssetBundles/Standalone")]
	static void BuildStandaloneAssetBundles ()
	{
		var path = Application.dataPath + "/ExportAssetBundles/Standalone";
		if (Directory.Exists (path) == false) { 
			Directory.CreateDirectory (path);
		}
		BuildPipeline.BuildAssetBundles ("Assets/ExportAssetBundles/Standalone", BuildAssetBundleOptions.None, BuildTarget.Android);
	}
}