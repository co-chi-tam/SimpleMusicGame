using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SimpleMusicGame {
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

		/// <summary>
		/// Save file the in local.
		/// </summary>
		public static void SaveInLocal(string path, object value) {
			if (File.Exists (path) == false) {
				File.Create (path);
			}
			File.WriteAllBytes (path, value.ToByteArray());
		}

		/// <summary>
		/// Loads the bundle from file.
		/// </summary>
		public static AssetBundle LoadBundleFromFile(string path) {
			if (File.Exists (path)) {
				return AssetBundle.LoadFromFile (path);
			} 
			return null;
		}

		/// <summary>
		/// Loads the bundle from generic.
		/// </summary>
		public static T LoadBundle<T>(string name) where T : UnityEngine.Object {
			T resource = default(T);
			if (currentAssetBundle == null)
				return resource;
			resource = currentAssetBundle.LoadAsset<T> (name);
			return resource;
		}

		/// <summary>
		/// Loads the resource or bundle from generic.
		/// </summary>
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

		/// <summary>
		/// Unload assetbundle.
		/// </summary>
		public static void Unload(bool value) {
			if (currentAssetBundle != null) {
				currentAssetBundle.Unload (value);
			}
		}

		/// <summary>
		/// Combines the byte array.
		/// </summary>
		private byte[] CombineByteArray(params byte[][] arrays)
		{
			byte[] rv = new byte[arrays.Sum(a => a.Length)];
			int offset = 0;
			foreach (byte[] array in arrays) {
				System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
				offset += array.Length;
			}
			return rv;
		}

		#endregion
		
	}
}
