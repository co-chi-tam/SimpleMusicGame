﻿using System;
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
		[SerializeField]	private AudioClip m_AudioClip;
		[SerializeField]	private TextAsset m_AudioTextAsset;
		[SerializeField]	private Sprite m_AudioBackground;

		private Queue<INode> m_QueueNodes;
		private string[] m_PrefabNodes = new string[] {"SimpleNode", "HoldNode"};
		private float m_WaitingTime = 2f;

		private int m_NodeIndex = 0;
		private int m_PrevertTime = -1;
		private List<CNodeData> m_ListNodeData;

		protected override void Awake ()
		{
			base.Awake ();
			this.m_QueueNodes = new Queue<INode> ();
		}

		protected virtual void Start() {
			// TEST
			this.m_AudioSource.clip = this.m_AudioClip;
			this.m_RootBackgroundImage.sprite = this.m_AudioBackground;
			this.m_AudioSource.Play ();
			if (m_AudioTextAsset != null) {
				this.m_ListNodeData = CSVUtil.ToObject<CNodeData> (this.m_AudioTextAsset.text);
			}
		}

		protected virtual void LateUpdate() {
			if (this.m_AudioSource.isPlaying) {
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

	}
}
