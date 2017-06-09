using System;
using UnityEngine;

namespace SimpleGameMusic {
	public interface INode {

		void OnPressNode ();
		void OnHoldNode ();
		void OnLeaveNode ();

		void PlayAudio ();

		void StartNode ();
		void Processing ();
		void Reset ();

		void StartAnimation (string value);
		void EndAnimation (string value);

		Transform GetTransform();
		RectTransform GetRectTransform();
		Vector2 GetPosition2D();
		void SetPosition2D(Vector2 value);
		string GetText();
		void SetText(string value);
		float GetScale();
		void SetScale(float value);
		float GetValue();
		void SetValue(float value);
		bool GetActive();
		void SetActive(bool value);
		bool GetComplete();
		void SetComplete(bool value);
		ENodeType GetNodeType();
		void SetNodeType(ENodeType value);

	}
}
