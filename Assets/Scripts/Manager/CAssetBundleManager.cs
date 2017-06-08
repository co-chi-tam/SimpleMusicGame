using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleGameMusic {
	public class CAssetBundleManager {

		public static AssetBundle currentAssetBundle;
		public static bool loaded;
		public static Dictionary<string, UnityEngine.Object> assetCached = new Dictionary<string, UnityEngine.Object> ();

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

		public static T LoadResourceOrBundle<T>(string name, bool cached = false) where T : UnityEngine.Object {
			T resource = default(T);
			if (assetCached.ContainsKey (name) && cached == true) {
				return assetCached [name] as T;
			}
			var allResources = Resources.LoadAll<T> ("");
			for (int i = 0; i < allResources.Length; i++) {
				if (allResources [i].name == name) {
					resource = allResources [i];
				}
			}
			if (resource == null) {
				resource = currentAssetBundle.LoadAsset<T> (name);
			}
			if (cached == true) {
				assetCached [name] = resource;
			}
			return resource;
		}
		
	}
}
