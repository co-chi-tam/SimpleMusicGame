using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleGameMusic {
	public class CResourceManager {

		private int m_Version = 1;
		private string m_ResourceUrl = "http://www.dropbox.com/s/ivrd48us6z2hyts/cactus_leafy_go.go?dl=1";
		private string m_ResourceName = "AssetBundles.bin";
		private string m_StorePath;
		private bool m_SaveOnLocal = false;

		private AssetBundle m_CurrentBundle;

		public CResourceManager (int version, string assetUrl, bool saveOnLocal)
		{
			this.m_Version = version;
			this.m_ResourceUrl = assetUrl;
			this.m_SaveOnLocal = saveOnLocal;
#if UNITY_EDITOR
			this.m_StorePath = Application.dataPath + "/AssetBundle/v" + m_Version + "/";
#else
			this.m_StorePath = Application.persistentDataPath + "/AssetBundle/v" + m_Version + "/";
#endif
			if (Directory.Exists (this.m_StorePath) == false) {
				Directory.CreateDirectory (this.m_StorePath);
			}
			this.LoadResource (this.m_ResourceUrl);
		}

		public void ReloadResouce() {
			this.LoadResource (this.m_ResourceUrl);
		}

		private void LoadResource(string url) {
			CHandleEvent.Instance.AddEvent (this.HandleLoadResource (url, () => {
				Debug.Log ("Download complete !!");
			}, (error) => {
				Debug.LogError ("ERROR: " + error);
			}, (prc) => {
				
			}), null);
		}

		private IEnumerator HandleLoadResource(string url, Action complete, Action<string> error, Action<float> process) {
			WWW www = null;
			var fullPath = this.m_StorePath + this.m_ResourceName;
			if (this.m_SaveOnLocal == false) {
				while (!Caching.ready)
					yield return null;
				www = WWW.LoadFromCacheOrDownload (url, this.m_Version);
			} else {
				if (File.Exists (fullPath) == false) {
					www = new WWW (url);
				} else {
					this.m_CurrentBundle = this.LoadBundleFromFile (fullPath);
					if (complete != null) {
						complete ();
					}
					yield break;
				}
			}
			if (process != null) {
				process (www.progress);
			}
			yield return www;
			if (string.IsNullOrEmpty (www.error) == false) {
				if (error != null) {
					error (www.error);
				}
			} else {
				if (this.m_SaveOnLocal == false) {
					// TODO
				} else {
					if (www.bytes.Length > 0) {
						if (File.Exists (fullPath) == false) {
							File.WriteAllBytes (fullPath, www.bytes);
						}
					}
				}
				if (complete != null) {
					complete ();
				}
			}
			this.m_CurrentBundle = www.assetBundle;
		}

		private AssetBundle LoadBundleFromFile(string path) {
			return AssetBundle.LoadFromFile (path);
		}

		public T LoadBundle<T>(string name) where T : UnityEngine.Object {
			var resource = this.m_CurrentBundle.LoadAsset<T> (name);
			return resource;
		}

		public T LoadResourceOrBundle<T>(string nameOrPath) where T : UnityEngine.Object {
			var resource = Resources.Load<T> (nameOrPath);
			if (resource == null) {
				resource = this.m_CurrentBundle.LoadAsset<T> (nameOrPath);
			}
			return resource;
		}
		
	}
}
