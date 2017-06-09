using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CUIManager : CMonoSingleton<CUIManager> {

		[Header("Root node")]
		[SerializeField]	private GameObject m_RootNode;

		[Header("Player info")]
		[SerializeField]	private Text m_ScoreText;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected virtual void Start() {
		
		}

		public void SetPlayerScore(string value) {
			this.m_ScoreText.text = "Score: " + value;
		}

	}
}
