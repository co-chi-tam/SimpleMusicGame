using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleGameMusic {
	public class CAssetBundleManager {

		public static AssetBundle currentAssetBundle;
		public static bool loaded;

		public CAssetBundleManager ()
		{
			
		}

		public static AssetBundle LoadBundleFromFile(string path) {
			return AssetBundle.LoadFromFile (path);
		}

		public static T LoadBundle<T>(string name) where T : UnityEngine.Object {
			var resource = currentAssetBundle.LoadAsset<T> (name);
			return resource;
		}

		public static T LoadResourceOrBundle<T>(string nameOrPath) where T : UnityEngine.Object {
			var resource = Resources.Load<T> (nameOrPath);
			if (resource == null) {
				resource = currentAssetBundle.LoadAsset<T> (nameOrPath);
			}
			return resource;
		}
		
	}
}
