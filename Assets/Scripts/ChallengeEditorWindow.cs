#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DapperDino.BuildingBlocks
{
    public class ChallengeEditorWindow : EditorWindow
    {
        private string challengeText;

        public void SetText(string challengeText)
        {
            this.challengeText = challengeText;
        }

        private void OnGUI()
        {
            Rect challengeTextRect = GUILayoutUtility.GetRect(new GUIContent(challengeText), "label");
            GUI.Label(challengeTextRect, challengeText);
        }
    }
}
#endif
