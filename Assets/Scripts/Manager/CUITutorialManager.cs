﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSingleton;
using SimpleMusicGame.UICustom;

namespace SimpleMusicGame {
	public class CUITutorialManager : CMonoSingleton<CUITutorialManager> {

		[SerializeField]	private Animator m_TutorialAnimator;
		[SerializeField]	private ETutorialPhase m_TutorialPhase = ETutorialPhase.Idle;

		public enum ETutorialPhase: int {
			Idle 			= 0,
			Phase_1 		= 1,
			SwitchPhase 	= 2,
			Phase_2 		= 3,
			SwitchPhase_2 	= 4,
			Phase_3 		= 5,
			End 			= 6
		}

		public void NextPhase() {
			var currentPhase = (int) m_TutorialPhase;
			m_TutorialPhase = (ETutorialPhase) (currentPhase + 1);
			SetPhaseAnimation ((int) m_TutorialPhase);
		}

		public void PreviousPhase() {
			var currentPhase = (int) m_TutorialPhase;
			m_TutorialPhase = (ETutorialPhase) (currentPhase - 1);
			SetPhaseAnimation ((int) m_TutorialPhase);
		}

		public void SetPhaseAnimation(int value) {
			var fitValue = value < 0 ? 0 : value > (int) ETutorialPhase.End ? (int) ETutorialPhase.End : value;
			m_TutorialAnimator.SetInteger ("AnimParam", fitValue);
			m_TutorialPhase = (ETutorialPhase) fitValue;
		}

		public ETutorialPhase GetTutorialPhase() {
			return m_TutorialPhase;
		}

	}
}
