using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSingleton;

namespace SimpleGameMusic {
	public class CUIManager : CMonoSingleton<CUIManager> {

		[Header("Root node")]
		[SerializeField]	private GameObject m_RootNode;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected virtual void Start() {
		
		}

	}
}
