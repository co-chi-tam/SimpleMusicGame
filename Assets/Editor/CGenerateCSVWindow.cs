using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleGameMusic {
	public class CGenerateCSVWindow : EditorWindow {

		private float m_SoundTimeLine = 0f;
		private int m_PrevertSoundTimeLine = 0;
		private AudioClip m_SoundAudio;
		private AudioClip m_PrevertAudio;
		private Dictionary<string, List<CNodeEditorData>> m_DictNodeData = new Dictionary<string, List<CNodeEditorData>>(); 
		private bool m_IsPlaying = false;
		private bool m_IsPause = false;
		private Rect m_CreateNodeDataRect = new Rect (0f, 0f, 20f, 20f);
		private Rect m_SoundLineRect;
		private int m_NodeDuplicate = 1;
		private CNodeEditorData m_CurrentSelectedNode;
		private static Texture2D m_SampleTexture;
		private Vector2 m_CurrentUISize = new Vector2 (1280f, 800f);
		private Vector2 m_CurrentKeyframeScrollBar;
		private string m_CSVFileName = "GenerateCSV";
		private string m_CurrentSelectKeyframe;

		[MenuItem ("Simple Music Game/Generate CSV window")]
		static void Init() {
			var window = (CGenerateCSVWindow)EditorWindow.GetWindow (typeof(CGenerateCSVWindow));
			window.Show ();
			m_SampleTexture = new Texture2D (1, 1);
			m_SampleTexture.SetPixels (new Color[]{Color.blue, Color.blue});
			m_SampleTexture.Apply ();
		}

		private void OnDestroy() {
			AudioUtility.StopAllClips ();
		}

		private void OnGUI() {
			GUILayout.BeginVertical ();
			this.DrawSoundHandle ();
			this.DrawKeyframeManager ();
			this.DrawGenerateCSV ();
			GUILayout.EndVertical ();
		}

		private void Update() {
			if (m_IsPlaying = true) {
				Repaint ();
			}
		}

		private void DrawSoundHandle() {
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			// Choose file music.
			GUILayout.Label ("File:");
			this.m_SoundAudio = (AudioClip) EditorGUILayout.ObjectField (this.m_SoundAudio, typeof(AudioClip));
			if (this.m_PrevertAudio != this.m_SoundAudio) {
				this.m_DictNodeData.Clear();
				this.m_PrevertAudio = this.m_SoundAudio;
				this.m_CurrentSelectedNode = null;
			}
//			GUILayout.FlexibleSpace ();
			// Choose sound file
			GUILayout.Label (this.m_SoundAudio == null ? "Please choose file audio clip." : "You select file: " + this.m_SoundAudio.name);
			// Play sound
			if (GUILayout.Button ("Play >>")) {
				if (this.m_SoundAudio != null) {
					AudioUtility.PlayClip (this.m_SoundAudio);
					this.m_PrevertAudio = this.m_SoundAudio;
					m_IsPlaying = true;
					m_IsPause = false;
				}
			}
			// Pause or resume sound
			if (this.m_IsPause == false) {
				if (GUILayout.Button ("Pause ||")) {
					if (this.m_SoundAudio != null) {
						AudioUtility.PauseClip (this.m_SoundAudio);
						m_IsPause = true;
					}
				}
			} else {
				if (GUILayout.Button ("Resume ||")) {
					if (this.m_SoundAudio != null) {
						AudioUtility.ResumeClip (this.m_SoundAudio);
						m_IsPause = false;
					}
				}
			}
			GUILayout.EndHorizontal ();
			GUILayout.BeginVertical ();
			var soundTime = this.m_SoundAudio == null ? 1 : this.m_SoundAudio.length;
			GUILayout.Label ("File length: " + (this.m_SoundAudio == null ? "0s" : (int)this.m_SoundAudio.length + "s") + "\n" +
								"Position: " + (int) this.m_SoundTimeLine + "s");
			// Sound play line
			this.m_SoundTimeLine = GUILayout.HorizontalSlider (this.m_SoundTimeLine, 0, soundTime);
			this.m_SoundLineRect = GUILayoutUtility.GetLastRect ();
			if (this.m_SoundAudio != null) {
				if (this.m_IsPause == false) {
					// Update sound line
					this.m_SoundTimeLine = AudioUtility.GetClipPosition (this.m_SoundAudio);
					if (this.m_PrevertSoundTimeLine != (int) this.m_SoundTimeLine) {
						this.m_PrevertSoundTimeLine = (int) this.m_SoundTimeLine;
						this.m_NodeDuplicate = 1;
					}
				} else {
					// Set sound line
					var clipPosition = this.m_SoundTimeLine * AudioUtility.GetFrequency (this.m_SoundAudio);
					AudioUtility.SetClipSamplePosition (this.m_SoundAudio, (int)clipPosition);
				}
			} 
			GUILayout.EndVertical ();
			GUILayout.BeginHorizontal ();
			var soundRatio = 0f;
			var windowSize = this.position;
			foreach (var item in m_DictNodeData) {
				var keyframeName = item.Key;
				var listNodeInFrame = item.Value;
				var keyframeIndex = keyframeName.Replace ("Keyframe_", "");
				var audioTime = int.Parse (keyframeIndex);
				var ratio = (float) audioTime / this.m_SoundAudio.length;
				soundRatio = this.m_SoundLineRect.width * ratio;
				this.m_CreateNodeDataRect.x = this.m_SoundLineRect.x + soundRatio;
				this.m_CreateNodeDataRect.y = this.m_SoundLineRect.y + 20f;
				if (GUI.Button (this.m_CreateNodeDataRect, keyframeIndex)) {
					this.m_CurrentSelectKeyframe = keyframeName;
					this.m_CurrentSelectedNode = null;
				}
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}

		private void DrawKeyframeManager() {
			if (this.m_SoundAudio == null)
				return;
			GUILayout.Space (30f);
			GUILayout.BeginVertical ("Node info: ");
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add keyframe")) {
				var nodeTimelineIndex = (int) this.m_SoundTimeLine;
				var keyName = "Keyframe_" + nodeTimelineIndex;
				// Create new node
				var nodeData = CreateNewNode(nodeTimelineIndex);
				if (this.m_DictNodeData.ContainsKey (keyName)) {
					// Add new key node in key frame
					this.m_DictNodeData [keyName].Add (nodeData);
				} else {
					// Create new key frame
					this.m_DictNodeData.Add (keyName, new List<CNodeEditorData> ());
					// Add new key node in key frame
					this.m_DictNodeData [keyName].Add (nodeData);
				}
				var listNode = this.m_DictNodeData [keyName];
				nodeData.editorIndex = listNode.Count;
				this.m_CurrentSelectKeyframe = keyName;
			}
			if (GUILayout.Button ("Delete keyframe")) {
				if (string.IsNullOrEmpty (this.m_CurrentSelectKeyframe) == false) {
					if (this.m_DictNodeData.ContainsKey (this.m_CurrentSelectKeyframe)) {
						this.m_DictNodeData [this.m_CurrentSelectKeyframe].Clear ();
						this.m_DictNodeData.Remove (this.m_CurrentSelectKeyframe);
						this.m_CurrentSelectKeyframe = string.Empty;
						this.m_CurrentSelectedNode = null;
					}
				}
			}
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical (GUILayout.Width (300f));
			if (string.IsNullOrEmpty (this.m_CurrentSelectKeyframe) == false) {
				var keyName = this.m_CurrentSelectKeyframe;
				if (GUILayout.Button ("Add node")) {
					var nodeIndex = keyName.Replace ("Keyframe_", "");
					// Create new node
					var nodeData = CreateNewNode(int.Parse (nodeIndex));
					if (this.m_DictNodeData.ContainsKey (keyName)) {
						// Add new key node in key frame
						this.m_DictNodeData [keyName].Add (nodeData);
					} else {
						// Create new key frame
						this.m_DictNodeData.Add (keyName, new List<CNodeEditorData> ());
						// Add new key node in key frame
						this.m_DictNodeData [keyName].Add (nodeData);
					}
					var listNode = this.m_DictNodeData [keyName];
					nodeData.editorIndex = listNode.Count;
					this.m_CurrentSelectedNode = null;
				}
				this.m_CurrentKeyframeScrollBar = GUILayout.BeginScrollView (this.m_CurrentKeyframeScrollBar,false, true);
				var keyframeSelected = this.m_DictNodeData[this.m_CurrentSelectKeyframe];
				for (int i = 0; i < keyframeSelected.Count; i++) {
					if (GUILayout.Button ("Node " + keyframeSelected [i].editorIndex)) {
						this.m_CurrentSelectedNode = keyframeSelected [i];
					}
				}
				GUILayout.EndScrollView ();
			}
			GUILayout.EndVertical ();
			GUILayout.BeginVertical ();
			if (this.m_CurrentSelectedNode != null) {
				GUILayout.Label ("Selected keyframe: " + this.m_CurrentSelectKeyframe);
				GUILayout.Label ("Selected node: " + this.m_CurrentSelectedNode.editorIndex);
				this.m_CurrentSelectedNode.audioTime = EditorGUILayout.IntField ("Samples time: ", this.m_CurrentSelectedNode.audioTime);
				this.m_CurrentSelectedNode.nodeType = (int)((ENodeType) EditorGUILayout.EnumPopup ("Node type: ", (ENodeType) this.m_CurrentSelectedNode.nodeType));
				this.m_CurrentSelectedNode.nodePositionEditor = EditorGUILayout.Vector2Field ("Node position: ", this.m_CurrentSelectedNode.nodePositionEditor);
				this.m_CurrentSelectedNode.nodePosition = "0|0";
				this.m_CurrentSelectedNode.nodePosition = this.m_CurrentSelectedNode.nodePositionEditor.x + "|" + this.m_CurrentSelectedNode.nodePositionEditor.y;
				this.m_CurrentSelectedNode.nodeScale = EditorGUILayout.FloatField ("Node scale: ", this.m_CurrentSelectedNode.nodeScale);
				var lastRect = GUILayoutUtility.GetLastRect ();
				var widthScreen = 500f;
				var heightScreen = widthScreen / 1.6f;
				EditorGUI.DrawPreviewTexture (new Rect (lastRect.x, lastRect.y + 40f, widthScreen, heightScreen), m_SampleTexture);
				if (GUILayout.Button ("Delete node")) {
					var keyframeSelected = this.m_DictNodeData[this.m_CurrentSelectKeyframe];
					keyframeSelected.Remove (this.m_CurrentSelectedNode);
				}
				var widthRatio = widthScreen / this.m_CurrentUISize.x;
				var heightRatio = heightScreen / this.m_CurrentUISize.y;
				var nodePosition = this.m_CurrentSelectedNode.nodePositionEditor;
				var screenNodePosition = Vector2.zero;
				screenNodePosition.x = (lastRect.x) - 15f + (widthScreen / 2f) + (nodePosition.x * widthRatio);//this.m_CurrentUISize.x / 2f;
				screenNodePosition.y = (lastRect.y + 40f) - 15f + (heightScreen / 2f) - (nodePosition.y * heightRatio);//this.m_CurrentUISize.y / 2f;
				if (GUI.Button (new Rect (screenNodePosition.x, screenNodePosition.y, 30f, 30f), this.m_CurrentSelectedNode.editorIndex.ToString())) {
					
				}
			}
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}

		private void DrawGenerateCSV() {
			GUILayout.FlexibleSpace ();
			GUILayout.BeginHorizontal ();
			m_CSVFileName = EditorGUILayout.TextField ("File name: ", m_CSVFileName);
			if (GUILayout.Button ("Generate")) {
				var csvText = "\"audioTime\",\"nodeType\",\"nodePosition\",\"nodeScale\"\n";
				foreach (var item in m_DictNodeData) {
					var listNode = item.Value;
					for (int i = 0; i < listNode.Count; i++) {
						csvText += listNode [i].ToString () + "\n";
					}
				}
				File.WriteAllText(Application.dataPath + "/Editor/Out/" + m_CSVFileName + ".csv", csvText);
				AssetDatabase.Refresh ();
			}
			GUILayout.EndHorizontal ();
		}

		private CNodeEditorData CreateNewNode(int soundTime) {
			var nodeData = new CNodeEditorData ();
			nodeData.audioTime = soundTime;
			nodeData.nodeType = 0;
			nodeData.nodePosition = "0|0";
			nodeData.nodeScale = 2f;
			nodeData.duplicateIndex = this.m_NodeDuplicate;
			return nodeData;
		}

	}
}
