#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DapperDino.BuildingBlocks
{
    public class ChallengeEditorWindow : EditorWindow
    {
        private const int width = 900;
        private const int height = 450;

        private int challengeIndex;
        private string challengeTitle;
        private string challengeText;

        public void Init(string challengeTitle, string challengeText)
        {
            Rect windowPosition = position;
            windowPosition.center = new Rect(0f, 0f, Screen.width, Screen.height).center;
            position = windowPosition;

            maxSize = new Vector2(width, height);
            minSize = maxSize;

            challengeIndex = SceneManager.GetActiveScene().buildIndex;

            this.challengeTitle = challengeTitle;
            this.challengeText = challengeText;
        }

        private void OnGUI()
        {
            var titleTextLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                wordWrap = true,
                fontSize = 45,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField($"Challenge {challengeIndex}: {challengeTitle}", titleTextLabelStyle);

            GUILayout.FlexibleSpace();

            var challengeTextLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                wordWrap = true,
                fontSize = 30
            };
            EditorGUILayout.LabelField($"For this challenge you will learn how to:\n\n{challengeText}", challengeTextLabelStyle);

            GUILayout.FlexibleSpace();

            GUI.skin = null;

            GUI.backgroundColor = new Color(0f, 0.8f, 0f);
            var closeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 50f,
            };
            closeButtonStyle.normal.textColor = Color.white;
            closeButtonStyle.hover.textColor = Color.white;
            closeButtonStyle.active.textColor = Color.white;
            closeButtonStyle.fontSize = 20;
            closeButtonStyle.fontStyle = FontStyle.Bold;
            if (GUILayout.Button("Let's Do This!", closeButtonStyle))
            {
                Close();
            }
        }
    }
}
#endif
