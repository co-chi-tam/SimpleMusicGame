using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using ObjectPool;
using Pul;

namespace SimpleGameMusic {
	public enum ENodeType: int {
		SimpleNode = 0,
		HoldNode = 1
	}
	public class CGameManager : CMonoSingleton<CGameManager> {

		[Header("Audio")]
		[SerializeField]	private AudioSource m_AudioSource;
		[Header("Root nodes")]
		[SerializeField]	private GameObject m_RootNodes;
		[SerializeField]	private Image m_RootBackgroundImage;
		[Header("Game info")]
		[SerializeField]	private string m_AudioName;

		private AudioClip m_AudioClip;
		private TextAsset m_AudioTextAsset;
		private Sprite m_AudioBackground;
		private string[] m_PrefabNodes = new string[] {"SimpleNode", "HoldNode"};
		private float m_WaitingTime = 2f;
		private int m_NodeIndex = 0;
		private int m_PrevertTime = -1;
		private List<CNodeData> m_ListNodeData;
		private bool m_IsAssetsAlready = false;

		public CRootTask root;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected virtual void Start() {
			this.root = CRootTask.GetInstance ();
			if (string.IsNullOrEmpty (this.m_AudioName) == false) {
				StartCoroutine (LoadAssetsAsyn (this.m_AudioName, () => {
					this.m_AudioSource.clip = this.m_AudioClip;
					this.m_RootBackgroundImage.sprite = this.m_AudioBackground;
					this.m_AudioSource.Play ();	
					this.m_ListNodeData = CSVUtil.ToObject<CNodeData> (this.m_AudioTextAsset.text);
					this.m_IsAssetsAlready = true;
				}, () => {
					this.m_IsAssetsAlready = false;
					throw new Exception ("ERROR: Can not load assets.");	
				}));
			}
		}

		protected virtual void LateUpdate() {
			if (this.m_IsAssetsAlready) {
				var audioTime = (int)this.m_AudioSource.time;
				var step = 100;
				while (step > 0 && m_NodeIndex < m_ListNodeData.Count) {
					var currentNodeData = m_ListNodeData [m_NodeIndex];
					if (audioTime == currentNodeData.audioTime && audioTime != m_PrevertTime) {
						var node = SpawnNode (m_PrefabNodes [currentNodeData.nodeType]);
						this.SetUpNode (node);
						node.SetPosition2D (currentNodeData.nodePosition.ToVector2 ());
						node.SetScale (currentNodeData.nodeScale);
						m_NodeIndex++;
					} else {
						m_PrevertTime = audioTime;
						break;
					}
					step--;
				}
			}
		}

		protected virtual void SetUpNode(INode node) {
			node.GetTransform().SetParent (this.m_RootNodes.transform);
			node.SetActive (true);
			node.SetComplete (false);
			node.SetValue (0f);
			var simpleNode = node as CSimpleNode;
			if (simpleNode.OnDeactive.GetPersistentEventCount () == 1) {
				simpleNode.OnDeactive.AddListener (() => {
					Destroy (simpleNode.gameObject);
				});
			}
		}

		protected virtual INode SpawnNode(string nodeName) {
			INode node = null;
			var nodePrefab = Resources.Load<GameObject> ("Node/" + nodeName + "/" + nodeName);
			var nodeInstantiate = Instantiate (nodePrefab);
			node = nodeInstantiate.GetComponent<INode> ();
			return node;
		}

		private void LoadAssets(string name) {
			this.m_AudioClip = Resources.Load <AudioClip> ("Sounds/" + name);
			this.m_AudioTextAsset = Resources.Load <TextAsset> ("Data/" + name);
			this.m_AudioBackground = Resources.Load <Sprite> ("Backgrounds/" + name);
			this.m_IsAssetsAlready = this.m_AudioClip != null 
										&& this.m_AudioTextAsset != null 
										&& this.m_AudioBackground != null;
		}

		private IEnumerator LoadAssetsAsyn(string name, Action complete, Action error) {
			this.m_AudioClip = Resources.Load <AudioClip> ("Sounds/" + name);
			this.m_AudioTextAsset = Resources.Load <TextAsset> ("Data/" + name);
			this.m_AudioBackground = Resources.Load <Sprite> ("Backgrounds/" + name);
			var already = this.m_AudioClip != null 
							&& this.m_AudioTextAsset != null 
							&& this.m_AudioBackground != null;
			yield return new WaitForSeconds (3f);
			if (already) {
				if (complete != null) {
					complete ();
				}
			} else {
				if (error != null) {
					error ();
				}
			}
		}

		public void SetMusicFileName(string name) {
			this.m_AudioName = name;
		}

	}
}
