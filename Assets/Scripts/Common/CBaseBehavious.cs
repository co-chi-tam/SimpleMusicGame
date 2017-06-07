using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleGameMusic {
	public class CBaseBehavious : MonoBehaviour {

		protected Transform m_Transform;

		protected virtual void OnEnable() {
		
		}

		protected virtual void OnDiable() {
		
		}

		protected virtual void Awake() {
			this.m_Transform = this.transform;
		}

		protected virtual void Start() {
			
		}

		protected virtual void Update() {
			
		}

		protected virtual void LateUpdate() {
		
		}
		
	}
}
