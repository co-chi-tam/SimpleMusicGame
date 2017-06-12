using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		private bool m_IsPlayingReview = false;
		private bool m_IsPauseReview = false;
		private Rect m_CreateNodeDataRect = new Rect (0f, 0f, 20f, 20f);
		private Rect m_SoundLineRect;
		private Rect m_FrameManagerRect;
		private CNodeEditorData m_CurrentSelectedNode;
		private static Texture2D m_SampleTexture;
		private Vector2 m_CurrentUISize = new Vector2 (1280f, 800f);
		private Vector2 m_CurrentKeyframeScrollBar;
		private Vector2 m_CurrentNodeScrollBar;
		private string m_CSVFileName = "GenerateCSV";
		private string m_CurrentSelectKeyframe;
		private List<CNodeEditorData> m_CurrentTargetKeyframe;

		[MenuItem ("Simple Music Game/Generate CSV window")]
		static void Init() {
			var window = (CGenerateCSVWindow)EditorWindow.GetWindow (typeof(CGenerateCSVWindow));
			window.Show ();
			m_SampleTexture = new Texture2D (2, 2);
			m_SampleTexture.SetPixels (new Color[]{Color.blue, Color.red, Color.yellow, Color.green});
			m_SampleTexture.filterMode = FilterMode.Point;
			m_SampleTexture.wrapMode = TextureWrapMode.Clamp;
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
			Repaint ();
		}

		private void DrawSoundHandle() {
			GUILayout.BeginVertical ();
			DrawTimeLineMenu ();
			DrawTimeline ();
			DrawTimelineKeyframe ();
			GUILayout.EndVertical ();
		}

		private void DrawTimeLineMenu() {
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
					this.m_IsPlaying = true;
					this.m_IsPause = false;
					this.m_IsPlayingReview = false;
					this.m_IsPauseReview = false;
				}
			}
			// Pause or resume sound
			if (this.m_IsPause == false) {
				if (GUILayout.Button ("Pause ||")) {
					if (this.m_SoundAudio != null) {
						AudioUtility.PauseClip (this.m_SoundAudio);
						this.m_IsPause = true;
					}
				}
			} else {
				if (GUILayout.Button ("Resume ||")) {
					if (this.m_SoundAudio != null) {
						AudioUtility.ResumeClip (this.m_SoundAudio);
						this.m_IsPause = false;
					}
				}
			}
			GUILayout.EndHorizontal ();
		}

		private void DrawTimeline() {
			GUILayout.BeginVertical ();
			var soundTime = this.m_SoundAudio == null ? 1 : this.m_SoundAudio.length;
			GUILayout.Label ("File length: " + (this.m_SoundAudio == null ? "0s" : (int)this.m_SoundAudio.length + "s") + "\n" +
				"Position: " + (int) this.m_SoundTimeLine + "s");
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("<<", GUILayout.Width (30f))) {
				if (this.m_SoundAudio == null)
					return;
				this.m_SoundTimeLine = this.m_SoundTimeLine <= 1 ? 0 : this.m_SoundTimeLine - 1;
				// Set sound line
				var clipPosition = this.m_SoundTimeLine * AudioUtility.GetFrequency (this.m_SoundAudio);
				AudioUtility.SetClipSamplePosition (this.m_SoundAudio, (int)clipPosition);
			}
			// Sound play line
			this.m_SoundTimeLine = GUILayout.HorizontalSlider (this.m_SoundTimeLine, 0, soundTime);
			this.m_SoundLineRect = GUILayoutUtility.GetLastRect ();
			if (this.m_SoundAudio != null) {
				if (this.m_IsPause == false) {
					// Update sound line
					this.m_SoundTimeLine = AudioUtility.GetClipPosition (this.m_SoundAudio);
					if (this.m_PrevertSoundTimeLine != (int) this.m_SoundTimeLine) {
						this.m_PrevertSoundTimeLine = (int) this.m_SoundTimeLine;
					}
				} else {
					// Set sound line
					var clipPosition = this.m_SoundTimeLine * AudioUtility.GetFrequency (this.m_SoundAudio);
					AudioUtility.SetClipSamplePosition (this.m_SoundAudio, (int)clipPosition);
				}
			} 
			if (GUILayout.Button (">>", GUILayout.Width (30f))) {
				if (this.m_SoundAudio == null)
					return;
				this.m_SoundTimeLine = this.m_SoundTimeLine >= this.m_SoundAudio.length - 1 ? this.m_SoundAudio.length : this.m_SoundTimeLine + 1;
				// Set sound line
				var clipPosition = this.m_SoundTimeLine * AudioUtility.GetFrequency (this.m_SoundAudio);
				AudioUtility.SetClipSamplePosition (this.m_SoundAudio, (int)clipPosition);
			}
			GUILayout.EndHorizontal ();
			GUILayout.EndVertical ();
		}

		private void DrawTimelineKeyframe() {
			if (this.m_SoundAudio == null)
				return;
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
		}

		private void DrawKeyframeManager() {
			if (this.m_SoundAudio == null)
				return;
			GUILayout.Space (30f);
			GUILayout.BeginVertical ();
			DrawKeyframeMenu ();
			this.m_FrameManagerRect = GUILayoutUtility.GetLastRect ();
			if (this.m_IsPlayingReview == false) {
				DrawKeyframeInfo ();
			} else {
				DrawKeyframeReview ();
			}
			GUILayout.EndVertical ();
		}

		private void DrawKeyframeMenu() {
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Add keyframe") && this.m_IsPlayingReview == false) {
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
				nodeData.editorIndex = listNode.Count - 1;
				this.m_CurrentSelectKeyframe = keyName;
			}
			if (GUILayout.Button ("Delete keyframe") && this.m_IsPlayingReview == false) {
				if (string.IsNullOrEmpty (this.m_CurrentSelectKeyframe) == false) {
					if (this.m_DictNodeData.ContainsKey (this.m_CurrentSelectKeyframe)) {
						this.m_DictNodeData [this.m_CurrentSelectKeyframe].Clear ();
						this.m_DictNodeData.Remove (this.m_CurrentSelectKeyframe);
						this.m_CurrentSelectKeyframe = string.Empty;
						this.m_CurrentSelectedNode = null;
					}
				}
			}
			if (GUILayout.Button ("Copy keyframe")) {
				if (string.IsNullOrEmpty (this.m_CurrentSelectKeyframe) == false) {
					if (this.m_DictNodeData.ContainsKey (this.m_CurrentSelectKeyframe)) {
						var keyframes = this.m_DictNodeData [this.m_CurrentSelectKeyframe];
						this.m_CurrentTargetKeyframe = keyframes;
					}
				}

			}
			if (GUILayout.Button ("Paste keyframe")) {
				if (this.m_CurrentTargetKeyframe != null && this.m_CurrentTargetKeyframe.Count > 0) {
					var nodeTimelineIndex = (int) this.m_SoundTimeLine;
					var keyName = "Keyframe_" + nodeTimelineIndex;
					if (this.m_DictNodeData.ContainsKey (keyName)) {
						// TODO
					} else {
						// Create new key frame
						this.m_DictNodeData.Add (keyName, new List<CNodeEditorData> ());
					}
					for (int i = 0; i < this.m_CurrentTargetKeyframe.Count; i++) {
						var tmpNode = this.m_CurrentTargetKeyframe [i];
						var newNode = new CNodeEditorData ();
						newNode.audioTime = nodeTimelineIndex;
						newNode.nodeType = tmpNode.nodeType;
						newNode.nodePosition = tmpNode.nodePosition;
						newNode.nodeScale = tmpNode.nodeScale;
						this.m_DictNodeData [keyName].Add (newNode);
						newNode.editorIndex = this.m_DictNodeData [keyName].Count - 1;
						newNode.editorNodePosition = newNode.nodePosition.ToVector2 ();
					}
					this.m_CurrentSelectKeyframe = keyName;
				}
			}
			GUILayout.FlexibleSpace ();
			if (GUILayout.Button (this.m_IsPlayingReview == false ? "Play review" : "Stop review")) {
				this.m_CurrentSelectKeyframe = string.Empty;
				this.m_CurrentSelectedNode = null;
				this.m_IsPlaying = false;
				this.m_IsPause = false;
				this.m_IsPlayingReview = !this.m_IsPlayingReview;
				this.m_IsPauseReview = false;
				if (this.m_IsPlayingReview) {
					AudioUtility.StopAllClips ();
					AudioUtility.PlayClip (this.m_SoundAudio);
				} else {
					AudioUtility.StopClip (this.m_SoundAudio);
				}
			}
			if (GUILayout.Button (this.m_IsPauseReview == false ? "Pause review" : "Resume review") && this.m_IsPlayingReview) {
				this.m_IsPauseReview = !this.m_IsPauseReview;
				if (this.m_IsPauseReview) {
					AudioUtility.PauseClip (this.m_SoundAudio);
				} else {
					AudioUtility.ResumeClip (this.m_SoundAudio);
				}
			}
			GUILayout.EndHorizontal ();
		}

		private void DrawKeyframeInfo() {
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
					nodeData.editorIndex = listNode.Count - 1;
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
			DrawNodeSelectedInfo ();
			GUILayout.EndHorizontal ();
		}

		private void DrawKeyframeReview() {
			GUILayout.Label ("Game review: ");
			GUILayout.BeginVertical ();
			this.m_FrameManagerRect.x = 0;
			this.m_FrameManagerRect.y = this.m_FrameManagerRect.y + this.m_FrameManagerRect.height + 30f;
			this.m_FrameManagerRect.width = this.m_FrameManagerRect.width / 2f;
			this.m_FrameManagerRect.height = this.m_FrameManagerRect.width / 1.6f;
			EditorGUI.DrawPreviewTexture (this.m_FrameManagerRect, m_SampleTexture);
			var keyframeReviewName = "Keyframe_" + ((int) this.m_SoundTimeLine);
			if (this.m_DictNodeData.ContainsKey (keyframeReviewName)) {
				var listNode = this.m_DictNodeData [keyframeReviewName];
				for (int i = 0; i < listNode.Count(); i++) {
					var currentNode = listNode [i];
					var widthScreen = this.m_FrameManagerRect.width;
					var heightScreen = this.m_FrameManagerRect.height;
					var widthRatio = widthScreen / this.m_CurrentUISize.x;
					var heightRatio = heightScreen / this.m_CurrentUISize.y;
					var nodePosition = currentNode.editorNodePosition;
					var screenNodePosition = Vector2.zero;
					screenNodePosition.x = (this.m_FrameManagerRect.x - 15f) + (widthScreen / 2f) + (nodePosition.x * widthRatio);
					screenNodePosition.y = (this.m_FrameManagerRect.y - 15f) + (heightScreen / 2f) - (nodePosition.y * heightRatio);
					if (GUI.Button (new Rect (screenNodePosition.x, screenNodePosition.y, 30f, 30f), currentNode.nodeType.ToString())) {

					}
				}
			}
			GUILayout.EndVertical ();
		}

		private void DrawNodeSelectedInfo() {
			this.m_CurrentNodeScrollBar = GUILayout.BeginScrollView (this.m_CurrentNodeScrollBar);
			GUILayout.BeginVertical ();
			if (this.m_CurrentSelectedNode != null) {
				GUILayout.Label ("Selected keyframe: " + this.m_CurrentSelectKeyframe);
				GUILayout.Label ("Selected node: " + this.m_CurrentSelectedNode.editorIndex);
				this.m_CurrentSelectedNode.audioTime = EditorGUILayout.IntField ("Samples time: ", this.m_CurrentSelectedNode.audioTime);
				this.m_CurrentSelectedNode.nodeType = (int)((ENodeType) EditorGUILayout.EnumPopup ("Node type: ", (ENodeType) this.m_CurrentSelectedNode.nodeType));
				this.m_CurrentSelectedNode.editorNodePosition = EditorGUILayout.Vector2Field ("Node position: ", this.m_CurrentSelectedNode.editorNodePosition);
				this.m_CurrentSelectedNode.nodePosition = "0|0";
				this.m_CurrentSelectedNode.nodePosition = this.m_CurrentSelectedNode.editorNodePosition.x + "|" + this.m_CurrentSelectedNode.editorNodePosition.y;
				this.m_CurrentSelectedNode.nodeScale = EditorGUILayout.FloatField ("Node scale: ", this.m_CurrentSelectedNode.nodeScale);
				var lastRect = GUILayoutUtility.GetLastRect ();
				var widthScreen = 500f;
				var heightScreen = widthScreen / 1.6f;
				EditorGUI.DrawPreviewTexture (new Rect (lastRect.x, lastRect.y + 40f, widthScreen, heightScreen), m_SampleTexture);
				GUILayout.BeginHorizontal ();
				if (GUILayout.Button ("Delete node")) {
					var keyframeSelected = this.m_DictNodeData[this.m_CurrentSelectKeyframe];
					if (keyframeSelected.Count > 1) {
						keyframeSelected.Remove (this.m_CurrentSelectedNode);
						this.m_CurrentSelectedNode = null;
					}
				}
				GUILayout.FlexibleSpace ();
				GUILayout.EndHorizontal ();
				GUILayout.Label ("Game review: ");
				var widthRatio = widthScreen / this.m_CurrentUISize.x;
				var heightRatio = heightScreen / this.m_CurrentUISize.y;
				var nodePosition = this.m_CurrentSelectedNode.editorNodePosition;
				var screenNodePosition = Vector2.zero;
				screenNodePosition.x = (lastRect.x) - 15f + (widthScreen / 2f) + (nodePosition.x * widthRatio);
				screenNodePosition.y = (lastRect.y + 40f) - 15f + (heightScreen / 2f) - (nodePosition.y * heightRatio);
				if (GUI.Button (new Rect (screenNodePosition.x, screenNodePosition.y, 30f, 30f), this.m_CurrentSelectedNode.editorIndex.ToString())) {

				}
				GUILayout.Space (heightScreen);
			}
			GUILayout.EndVertical ();
			GUILayout.EndScrollView ();
		}

		private void DrawGenerateCSV() {
			GUILayout.FlexibleSpace ();
			GUILayout.BeginVertical ();
			GUILayout.BeginHorizontal ();
			m_CSVFileName = EditorGUILayout.TextField ("File name: ", m_CSVFileName);
			var path = Application.dataPath + "/Editor/Out/" + m_CSVFileName + ".csv";
			if (GUILayout.Button ("Generate CSV")) {
				EditorUtility.DisplayProgressBar ("Generate CSV", " " + path, 1f);
				var csvText = "\"audioTime\",\"nodeType\",\"nodePosition\",\"nodeScale\"\n";
				var keys = this.m_DictNodeData.Keys.ToList ();
				keys.Sort (delegate(string x, string y) {
					var intX = int.Parse (x.Replace("Keyframe_", ""));
					var intY = int.Parse (y.Replace("Keyframe_", ""));
					return intX > intY ? 1 : intX == intY ? 0 : -1;
				});
				foreach (var key in keys) {
					var listNode = this.m_DictNodeData [key];
					for (int i = 0; i < listNode.Count; i++) {
						csvText += listNode [i].ToString () + "\n";
					}
				}
				File.WriteAllText(path, csvText);
				AssetDatabase.Refresh ();
				EditorUtility.ClearProgressBar ();
			}
			if (GUILayout.Button ("Load CSV") && this.m_SoundAudio != null) {
				var csvText = File.ReadAllText (path);
				var csvData = Pul.CSVUtil.ToObject <CNodeData> (csvText);
				this.m_DictNodeData.Clear ();
				EditorUtility.DisplayProgressBar ("Load CSV", " " + path, 1f);
				for (int i = 0; i < csvData.Count; i++) {
					var node = csvData [i];
					var nodeKeyframe = "Keyframe_" + node.audioTime;
					if (this.m_DictNodeData.ContainsKey (nodeKeyframe)) {
						// TODO
					} else {
						this.m_DictNodeData.Add (nodeKeyframe, new List<CNodeEditorData> ());
					}
					var nodeEditor = new CNodeEditorData ();
					nodeEditor.audioTime = node.audioTime;
					nodeEditor.nodeType = node.nodeType;
					nodeEditor.nodePosition = node.nodePosition;
					nodeEditor.nodeScale = node.nodeScale;
					nodeEditor.editorIndex = this.m_DictNodeData [nodeKeyframe].Count;
					nodeEditor.editorNodePosition = node.nodePosition.ToVector2 ();
					this.m_DictNodeData [nodeKeyframe].Add (nodeEditor);
				}
				EditorUtility.ClearProgressBar ();
			}
			GUILayout.EndHorizontal ();
			GUILayout.Label (":" + path);
			GUILayout.EndVertical ();	
		}

		private CNodeEditorData CreateNewNode(int soundTime) {
			var nodeData = new CNodeEditorData ();
			nodeData.audioTime = soundTime;
			nodeData.nodeType = 0;
			nodeData.nodePosition = "0|0";
			nodeData.nodeScale = 2f;
			return nodeData;
		}

	}
}
