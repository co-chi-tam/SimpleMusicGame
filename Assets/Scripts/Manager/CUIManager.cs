using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using SimpleGameMusic.UICustom;

namespace SimpleGameMusic {
	public class CUIManager : CMonoSingleton<CUIManager> {

		#region Properties

		[Header("Root node")]
		[SerializeField]	private GameObject m_RootNode;

		[Header("Player info")]
		[SerializeField]	private Text m_ScoreText;

		#endregion

		#region Implementation MonoBehavious

		protected override void Awake ()
		{
			base.Awake ();
		}

		#endregion

		#region Main methods

		public void PreviousTask() {
			CRootTask.GetInstance ().PreviousTask ();
		}

		public void SetPlayerScore(string value) {
			this.m_ScoreText.text = "Score: " + value;
		}

		#endregion

	}
}
