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
		SimpleNode 	= 0,
		HoldNode 	= 1,
		SlideNode 	= 2
	}

	public class CGameManager : CMonoSingleton<CGameManager> {

		#region Properties

		[Header("Audio")]
		[SerializeField]	private AudioSource m_AudioSource;
		[Header("Root nodes")]
		[SerializeField]	private GameObject m_RootNodes;
		[SerializeField]	private GameObject m_RootBackgroundImage;
		[Header("Game info")]
		[SerializeField]	private string m_AudioName;
		[SerializeField]	private CSongData m_SongData;
		[SerializeField]	private int m_PlayerScore;

		private AudioClip m_AudioClip;
		private TextAsset m_AudioTextAsset;
		private GameObject m_AudioBackground;
		private float m_WaitingTime = 2f;
		private int m_NodeIndex = 0;
		private int m_PreviousTime = -1;
		private List<CNodeData> m_ListNodeData;
		private bool m_IsAssetsAlready = false;
		private bool m_IsPlaying = false;
		private CUIManager m_UIManager;

		[HideInInspector]
		public CRootTask root;

		#endregion

		#region Implementation MonoBehavious

		protected override void Awake ()
		{
			base.Awake ();
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}

		protected virtual void Start() {
			this.root = CRootTask.GetInstance ();
			this.m_UIManager = CUIManager.GetInstance ();
			this.m_SongData = CTaskUtil.REFERENCES [CTaskUtil.SELECTED_SONG] as CSongData;
			this.m_AudioName = m_SongData.songName;
			var currentTask = this.root.GetCurrentTask ();
			if (string.IsNullOrEmpty (this.m_AudioName) == false) {
				StartCoroutine (LoadAssetsAsyn (this.m_AudioName, () => {
					this.StartGame();
				}, () => {
					this.EndGame();
					throw new Exception ("ERROR: Can not load assets.");	
				}));
			}
		}

		protected virtual void LateUpdate() {
			if (this.m_IsAssetsAlready) {
				if (this.m_AudioSource.isPlaying) {
					this.UpdateGame ();
				} else {
					if (this.m_IsPlaying) {
						this.root.GetCurrentTask ().OnTaskCompleted ();
						this.m_IsPlaying = false;
					}
				}
			}
		}

		protected virtual void OnDestroy() {
			
		}

		#endregion

		#region Game State

		protected virtual void StartGame() {
			this.m_AudioSource.clip = this.m_AudioClip;
			this.m_AudioBackground.transform.SetParent (this.m_RootBackgroundImage.transform);
			var rectAudioBG = this.m_AudioBackground.transform as RectTransform;
			rectAudioBG.anchoredPosition = Vector2.zero;
			rectAudioBG.sizeDelta = Vector2.zero;
			this.m_AudioSource.Play ();	
			this.m_ListNodeData = CSVUtil.ToObject<CNodeData> (this.m_AudioTextAsset.text);
			this.m_IsAssetsAlready = true;
		}

		protected virtual void UpdateGame() {
			this.m_IsPlaying = true;
			var audioTime = (int)this.m_AudioSource.time;
			var step = 100;
			while (step > 0 && m_NodeIndex < m_ListNodeData.Count) {
				var currentNodeData = m_ListNodeData [m_NodeIndex];
				if (audioTime == currentNodeData.audioTime && audioTime != m_PreviousTime) {
					var nodeType = (ENodeType)currentNodeData.nodeType;
					var node = SpawnNode (nodeType.ToString());
					this.SetUpNode (node);
					node.SetPosition2D (currentNodeData.nodePosition.ToVector2 ());
					node.SetScale (currentNodeData.nodeScale);
					node.SetNodeType (nodeType);
					m_NodeIndex++;
				} else {
					m_PreviousTime = audioTime;
					break;
				}
				step--;
			}
		}

		protected virtual void EndGame() {
			this.m_IsAssetsAlready = false;
		}

		#endregion

		#region Main methods

		protected virtual void SetUpNode(INode node) {
			node.GetTransform().SetParent (this.m_RootNodes.transform);
			node.SetActive (true);
			node.SetComplete (false);
			node.SetValue (0f);
			var simpleNode = node as CSimpleNode;
			if (simpleNode.OnDeactive.GetPersistentEventCount () == 1) {
				simpleNode.OnDeactive.AddListener (() => {
					var score = 0;
					switch (simpleNode.GetNodeType()) {
					case ENodeType.SimpleNode:
						score = this.m_SongData.simpleNodeScore;
						break;
					case ENodeType.HoldNode:
						score = this.m_SongData.holdNodeScore;
						break;
					default:
						score = this.m_SongData.simpleNodeScore;
						break;
					}
					var totalScore = (int) (score * simpleNode.GetValue());
					this.m_PlayerScore += totalScore;
					this.m_UIManager.SetPlayerScore (this.m_PlayerScore.ToString());
					Destroy (simpleNode.gameObject);
				});
			}
		}

		protected virtual INode SpawnNode(string nodeName) {
			INode node = null;
			var nodePrefab = CAssetBundleManager.LoadResourceOrBundle<GameObject> (nodeName, true);
			var nodeInstantiate = Instantiate (nodePrefab);
			node = nodeInstantiate.GetComponent<INode> ();
			return node;
		}

		private IEnumerator LoadAssetsAsyn(string name, Action complete, Action error) {
			this.m_AudioClip = CAssetBundleManager.LoadResourceOrBundle <AudioClip> (name);
			this.m_AudioTextAsset = CAssetBundleManager.LoadResourceOrBundle <TextAsset> (name);
			var background = CAssetBundleManager.LoadResourceOrBundle <GameObject> (name);
			var already = this.m_AudioClip != null 
							&& this.m_AudioTextAsset != null 
							&& background != null;
			yield return new WaitForSeconds (1f);
			if (already) {
				this.m_AudioBackground = Instantiate <GameObject> (background); 
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

		#endregion

	}
}
