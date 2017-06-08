using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleGameMusic {
	public class CDownloadResourceManager {

		private int m_Version = 1;
		private string m_ResourceUrl = "http://www.dropbox.com/s/ivrd48us6z2hyts/cactus_leafy_go.go?dl=1";
		private string m_ResourceName = "AssetBundles.bin";
		private string m_StorePath;
		private bool m_SaveOnLocal = false;

		public CDownloadResourceManager (int version, string assetUrl, bool saveOnLocal)
		{
			this.m_Version = version;
			this.m_ResourceUrl = assetUrl;
			this.m_SaveOnLocal = saveOnLocal;
#if UNITY_EDITOR
			this.m_StorePath = Application.dataPath + "/AssetBundles/v" + m_Version + "/";
#else
			this.m_StorePath = Application.persistentDataPath + "/AssetBundles/v" + m_Version + "/";
#endif
			if (Directory.Exists (this.m_StorePath) == false) {
				Directory.CreateDirectory (this.m_StorePath);
			}
		}

		public void LoadResource(Action complete, Action<string> error, Action<float> process) {
			this.LoadResource (this.m_ResourceUrl, complete, error, process);
		}

		public void LoadResource(string url, Action complete, Action<string> error, Action<float> process) {
			CHandleEvent.Instance.AddEvent (this.HandleLoadResource (url, complete, error, process), null);
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
					var processFake = 0f;
					while (processFake < 1f) {
						if (process != null) {
							process (processFake);
						}
						processFake += Time.deltaTime;
						yield return WaitHelper.WaitFixedUpdate;
					}
					CAssetBundleManager.currentAssetBundle = CAssetBundleManager.LoadBundleFromFile (fullPath);
					CAssetBundleManager.loaded = true;
					if (complete != null) {
						complete ();
					}
					yield break;
				}
			}
			while (www.isDone == false) {
				if (process != null) {
					process (www.progress);
				}
				yield return WaitHelper.WaitFixedUpdate;
			}
			yield return www;
			if (string.IsNullOrEmpty (www.error) == false) {
				if (error != null) {
					error (www.error);
				}
				CAssetBundleManager.loaded = false;
			} else {
				if (this.m_SaveOnLocal == false) {
					CAssetBundleManager.loaded = false;
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
			CAssetBundleManager.currentAssetBundle = www.assetBundle;
			CAssetBundleManager.loaded = true;
		}
		
	}
}
