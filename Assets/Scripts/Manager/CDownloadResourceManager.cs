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
		private WWW m_WWW;

		public CDownloadResourceManager (int version, string assetUrl)
		{
			this.m_Version = version;
			this.m_ResourceUrl = assetUrl;
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
			if (Application.internetReachability != NetworkReachability.NotReachable) {
				var fullPath = this.m_StorePath + this.m_ResourceName;
				if (File.Exists (fullPath) == false) {
					yield return this.DownloadContent (url, fullPath, false, error, process);
					yield return this.SaveDownloadContent (fullPath, complete, error);
				} else {
					yield return this.LoadLocalAsset (fullPath, complete, error, process);
				}
			} else {
				if (error != null) {
					error ("Error: Connect error, please check connect internet.");
				}
			}
			yield return WaitHelper.WaitForShortSeconds;
		}

		private IEnumerator DownloadContent(string url, string fullPath, bool cache, Action<string> error, Action<float> process) {
			if (cache) {
				while (!Caching.ready)
					yield return null;
				m_WWW = WWW.LoadFromCacheOrDownload (url, this.m_Version);
			} else {
				m_WWW = new WWW (url);
			}
			while (m_WWW.isDone == false) {
				if (process != null) {
					process (m_WWW.progress);
				}
				yield return WaitHelper.WaitFixedUpdate;
			}
			yield return m_WWW;
			if (string.IsNullOrEmpty (m_WWW.error) == false) {
				if (error != null) {
					error (m_WWW.error);
				}
			} 

		}

		private IEnumerator SaveDownloadContent(string fullPath, Action complete, Action<string> error) {
			if (m_WWW.bytes.Length > 0) {
				File.WriteAllBytes (fullPath, m_WWW.bytes);
				CAssetBundleManager.currentAssetBundle = m_WWW.assetBundle;
				CAssetBundleManager.loaded = m_WWW.assetBundle != null;
				if (complete != null) {
					if (CAssetBundleManager.currentAssetBundle != null) {
						complete ();
					} else {
						if (error != null) {
							error ("Error: AssetBundle is null.");
						}
					}
				}
			} else {
				if (error != null) {
					error ("ERROR: Download not complete.");
				}
				CAssetBundleManager.currentAssetBundle = null;
				CAssetBundleManager.loaded = false;
			}
			yield return WaitHelper.WaitFixedUpdate;
		}

		private IEnumerator LoadLocalAsset(string fullPath, Action complete, Action<string> error, Action<float> process) {
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
			yield return null;
		}
		
	}
}
