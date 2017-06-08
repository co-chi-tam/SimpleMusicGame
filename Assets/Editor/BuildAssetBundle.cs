using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem ("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles ()
    {
		var path = Application.dataPath + "/ExportAssetBundles";
		if (Directory.Exists (path) == false) { 
			Directory.CreateDirectory (path);
		}
		BuildPipeline.BuildAssetBundles ("Assets/ExportAssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
    }
}