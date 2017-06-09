using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SimpleGameMusic {
	public class CDownloadResourceManager {

		private int m_Version = 1;
		private string m_ResourceUrl = "https://google.com.vn";
		private string m_ResourceName = "AssetBundles.bin";
		private string m_StorePath;
		private bool m_SaveOnLocal = false;
		private WWW m_WWW;

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
			var fullPath = this.m_StorePath + this.m_ResourceName;
			yield return this.DownloadContent (url, fullPath, complete, error, process);
			yield return this.SaveDownloadContent (fullPath, complete, error);
		}

		private IEnumerator DownloadContent(string url, string fullPath, Action complete, Action<string> error, Action<float> process) {
			if (this.m_SaveOnLocal == false) {
				while (!Caching.ready)
					yield return null;
				m_WWW = WWW.LoadFromCacheOrDownload (url, this.m_Version);
			} else {
				m_WWW = new WWW (url);
				if (File.Exists (fullPath) == false) {
					// TODO
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
					CAssetBundleManager.loaded = CAssetBundleManager.currentAssetBundle != null;
					if (complete != null) {
						if (CAssetBundleManager.currentAssetBundle != null) {
							complete ();
						} else {
							if (error != null) {
								error ("Error: AssetBundle is null.");
							}
						}
					}
					yield break;
				}
			}
			while (m_WWW.isDone == false) {
				if (process != null) {
					process (m_WWW.progress);
				}
				yield return WaitHelper.WaitFixedUpdate;
			}
			yield return m_WWW;
		}

		private IEnumerator SaveDownloadContent(string fullPath, Action complete, Action<string> error) {
			yield return m_WWW;
			if (string.IsNullOrEmpty (m_WWW.error) == false) {
				if (error != null) {
					error (m_WWW.error);
				}
				CAssetBundleManager.loaded = false;
			} else {
				if (this.m_SaveOnLocal == false) {
					// TODO
				} else {
					if (m_WWW.bytes.Length > 0) {
						if (File.Exists (fullPath) == false) {
							File.WriteAllBytes (fullPath, m_WWW.bytes);
						}
					}
				}
			}
			if (CAssetBundleManager.currentAssetBundle == null) {
				CAssetBundleManager.currentAssetBundle = m_WWW.assetBundle;
				CAssetBundleManager.loaded = CAssetBundleManager.currentAssetBundle != null;
				if (complete != null) {
					if (CAssetBundleManager.currentAssetBundle != null) {
						complete ();
					} else {
						if (error != null) {
							error ("Error: AssetBundle is null.");
						}
					}
				}
			}
		}
		
	}
}
