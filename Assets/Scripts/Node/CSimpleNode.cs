using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SimpleGameMusic.UICustom;

namespace SimpleGameMusic {
	public class CSimpleNode : CBaseBehavious, INode {

		#region Properties

		[Header ("Animator")]
		[SerializeField]	protected Animator m_Animator;

		[Header ("Audio")]
		[SerializeField]	protected AudioSource m_AudioSource;

		[Header ("Text")]
		[SerializeField]	protected Text m_NodeText;

		[Header ("Data")]
		[SerializeField]	protected ENodeType m_NodeType = ENodeType.SimpleNode;
		[Range (1f, 10f)]
		[SerializeField]	protected float m_Scale = 1f;

		[Header ("Event")]
		public UnityEvent OnStart;
		public UnityEvent OnIdle;
		public UnityEvent OnActive;
		public UnityEvent OnDeactive;
		public UnityEvent OnPress;
		public UnityEvent OnLeave;

		protected RectTransform m_RectTransform;
		protected bool m_Processing = false;
		protected bool m_Complete = false;
		protected float m_Value;
		protected bool m_Active;
		protected INodeObject[] m_NodeObjects;

		#endregion

		#region Implementation Monobehavious

		protected override void Awake ()
		{
			base.Awake ();
			this.CheckNode ();
			this.m_RectTransform = this.m_Transform as RectTransform;
			this.m_NodeObjects = this.gameObject.GetComponentsInChildren<INodeObject> ();
		}

		protected override void Start ()
		{
			base.Start ();
			this.OnStart.Invoke ();
			this.LoadAllNodeObject(this.gameObject);
		}

		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			if (this.m_Processing) {
				var deltaValue = Time.deltaTime / this.m_Scale;
				m_Value += deltaValue;
			}
		}

		#endregion

		#region Main methods

		public virtual void OnPressNode() {
			this.OnPress.Invoke ();
		}

		public virtual void OnHoldNode() {
			
		}

		public virtual void OnLeaveNode() {
			this.OnLeave.Invoke ();
		}

		public virtual void OnSlideEndNode() {
		
		}

		public virtual void PlayAudio() {
			this.m_AudioSource.Play ();
		}

		public virtual void StartNode() {
			this.m_Animator.SetTrigger ("TriggerActive");
			this.m_Complete = false;
		}

		public virtual void CompleteNode() {
			this.m_Animator.SetBool ("Complete", true);
			this.m_Complete = true;
			this.m_Processing = false;
			this.m_NodeText.text = string.Empty;
			this.CheckNodeValue ();
			this.DefaultSpeed ();
		}

		public virtual void Processing() {
			this.m_Processing = true;
			this.m_Animator.speed = 1f / this.m_Scale;
		}

		public virtual void Reset() {
			this.m_Processing = false;
			this.m_NodeText.text = string.Empty;
		}

		public virtual void DefaultSpeed() {
			this.m_Animator.speed = 1f;
		}

		public virtual void StartAnimation(string name) {
			switch (name) {
			case "Idle":
				this.OnIdle.Invoke ();
				break;
			case "Active":
				this.OnActive.Invoke ();
				this.Processing ();
				break;
			case "Deactive":
				this.m_Processing = false;
				this.CheckNodeValue ();
				break;
			}
		}

		public virtual void EndAnimation(string name) {
			switch (name) {
			case "Idle":
				// TODO
				break;
			case "Active":
				this.DefaultSpeed ();
				break;
			case "Deactive":
				this.OnDeactive.Invoke ();
				this.Reset ();
				break;
			}
		}

		protected virtual void CheckNode() {
			if (m_Animator == null
			    || m_AudioSource == null
			    || m_NodeText == null) {
				throw new UnityException ("ERROR: Missing component.");
			}
		}

		public virtual void CheckNodeValue() {
			if (this.m_Complete == false) {
				this.m_NodeText.text = CTaskUtil.Translate ("BAD");
			} else {
				var nodeValue = this.GetValue ();
				if (nodeValue >= 0.75f) {
					this.m_NodeText.text = CTaskUtil.Translate ("PERFECT");
				} else if (nodeValue >= 0.5f) {
					this.m_NodeText.text = CTaskUtil.Translate ("GOOD");
				} else {
					this.m_NodeText.text = CTaskUtil.Translate ("BAD");
				}
			}
		}

		protected virtual void LoadAllNodeObject(GameObject root) {
			var childCount = root.transform.childCount;
			for (int i = 0; i < childCount; i++) {
				var child = root.transform.GetChild (i);
				var nodeObj = child.GetComponent<INodeObject> ();
				if (nodeObj != null) {
//					this.m_NodeObjects.Add (nodeObj);
				}
				if (child.transform.childCount > 0) {
					LoadAllNodeObject (child.gameObject);
				}
			}	
		}

		#endregion

		#region Getter && Setter

		public virtual Transform GetTransform() {
			return this.m_Transform;
		}

		public virtual RectTransform GetRectTransform() {
			return this.m_RectTransform;
		}

		public virtual Vector2 GetPosition2D() {
			return this.m_RectTransform.anchoredPosition;
		}

		public virtual void SetPosition2D(Vector2 value) {
			this.m_RectTransform.anchoredPosition = value;
		}

		public virtual string GetText() {
			return this.m_NodeText.text;
		}

		public virtual void SetText(string value) {
			this.m_NodeText.text = value;
		}

		public virtual float GetScale() {
			return this.m_Scale;
		}

		public virtual void SetScale(float value) {
			this.m_Scale = value;
		}
			
		public virtual float GetValue() {
			if (this.m_Complete == false)
				return 0f;
			return 1f - this.m_Value;
		}

		public virtual void SetValue(float value) {
			this.m_Value = value;
		}

		public virtual bool GetActive() {
			return this.m_Active;
		}

		public virtual void SetActive(bool value) {
			this.m_Active = value;
			this.m_Processing = false;
			this.gameObject.SetActive (value);
		}

		public virtual bool GetComplete() {
			return this.m_Complete;
		}

		public virtual void SetComplete(bool value) {
			this.m_Complete = value;
		}

		public virtual ENodeType GetNodeType() {
			return this.m_NodeType;
		}

		public virtual void SetNodeType(ENodeType value) {
			this.m_NodeType = value;
		}

		#endregion
		
	}
}
