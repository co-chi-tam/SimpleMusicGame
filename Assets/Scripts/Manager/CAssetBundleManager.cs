using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleGameMusic {
	public class CAssetBundleManager {

		#region Properties

		public static AssetBundle currentAssetBundle;
		public static bool loaded;
		public static Dictionary<string, UnityEngine.Object> assetCached = new Dictionary<string, UnityEngine.Object> ();

		#endregion

		#region Constructor

		public CAssetBundleManager ()
		{
			 
		}

		#endregion

		#region Main methods

		public static AssetBundle LoadBundleFromFile(string path) {
			if (File.Exists (path)) {
				return AssetBundle.LoadFromFile (path);
			} 
			return null;
		}

		public static T LoadBundle<T>(string name) where T : UnityEngine.Object {
			T resource = default(T);
			if (currentAssetBundle == null)
				return resource;
			resource = currentAssetBundle.LoadAsset<T> (name);
			return resource;
		}

		public static T LoadResourceOrBundle<T>(string name, bool cached = false) where T : UnityEngine.Object {
			T resource = default(T);
			if (currentAssetBundle == null)
				return resource;
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

		#endregion
		
	}
}
