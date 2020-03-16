using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DapperDino.BuildingBlocks
{
    [ExecuteInEditMode]
    public class TargetHandler : MonoBehaviour
    {
        [SerializeField] private string challengeTitle = string.Empty;
        [SerializeField] private string challengeText = string.Empty;
        [SerializeField] private GameObject wellDonePanel = null;
        [SerializeField] private GameObject tryAgainPanel = null;
        [SerializeField] private GameObject cannon = null;
        [SerializeField] private HealthBehaviour playerHealth = null;

        private List<HealthBehaviour> targets = new List<HealthBehaviour>();

        private void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += HandleModeStateChange;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= HandleModeStateChange;
#endif
        }

        private void Start()
        {
            if (!Application.isPlaying)
            {
#if UNITY_EDITOR
                var challengeWindow = EditorWindow.GetWindow<ChallengeEditorWindow>();
                challengeWindow.Init(challengeTitle, challengeText);
                challengeWindow.Show();
#endif
                return;
            }

            targets = GetComponentsInChildren<HealthBehaviour>().ToList();

            playerHealth.OnDeath += HandlePlayerDeath;

            foreach (var target in targets)
            {
                target.OnDeath += HandleTargetDeath;
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            playerHealth.OnDeath -= HandlePlayerDeath;

            foreach (var target in targets)
            {
                target.OnDeath -= HandleTargetDeath;
            }
        }

        private void HandlePlayerDeath(HealthBehaviour player)
        {
            player.OnDeath -= HandlePlayerDeath;

            tryAgainPanel.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void HandleTargetDeath(HealthBehaviour target)
        {
            target.OnDeath -= HandleTargetDeath;

            targets.Remove(target);

            if (targets.Count != 0) { return; }

#if UNITY_EDITOR
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt($"Scene_{sceneIndex}", 1);
#endif
            cannon.SetActive(false);
            wellDonePanel.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void GoToNextLevel()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
#endif
        }

        public void TryAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

#if UNITY_EDITOR
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
#endif
    }
}
