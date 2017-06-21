using System;
using UnityEngine;

namespace SimpleMusicGame {
	public interface INode: INodeObject {

		void OnPressNode ();

		void OnHoldNode ();
		void OnLeaveNode ();

		void OnSlideEndNode();

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
		bool GetComplete();
		void SetComplete(bool value);
		ENodeType GetNodeType();
		void SetNodeType(ENodeType value);

	}
}
