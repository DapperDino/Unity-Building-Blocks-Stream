#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DapperDino.BuildingBlocks
{
    [ExecuteInEditMode]
    public class Goal : MonoBehaviour
    {
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += HandleModeStateChange;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= HandleModeStateChange;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("GoalReacher")) { return; }

            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            PlayerPrefs.SetInt($"Scene_{sceneIndex}", 1);

            EditorApplication.isPlaying = false;
        }

        private void HandleModeStateChange(PlayModeStateChange obj)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            switch (obj)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    PlayerPrefs.SetInt($"Scene_{sceneIndex}", 0);
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    if (!PlayerPrefs.HasKey($"Scene_{sceneIndex}")) { break; }
                    if (PlayerPrefs.GetInt($"Scene_{sceneIndex}") != 1) { break; }

                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene($"Assets/Scenes/Scene_{sceneIndex + 1}.unity");

                    break;
            }
        }
    }
}
#endif
