using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NumericNook.Core.Runtime.Gameplay;
using NumericNook.Core.Runtime.Audio;
using System.Collections;

namespace NumericNook.Core.Runtime.UI
{
    public class MainMenuController : MonoBehaviour
    {

        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject levelSelectionPanel;
        [SerializeField] private GameObject creditsText;


        [SerializeField] private string interfaceSceneName;
        [SerializeField] private string level_01SceneName;
        [SerializeField] private string level_02SceneName;

        [SerializeField] private Image level_02Icon;

        [SerializeField] private Image level_01BorderImage;
        [SerializeField] private Image level_02BorderImage;


        [SerializeField] private Color blockedColor = new Color(0.0f, 0.0f, 0f);
        [SerializeField] private Color defaultColor = new Color(0.2f, 0.5f, 1f);
        [SerializeField] private Color selectedColor = new Color(0.2f, 0.8f, 0.3f);


        [SerializeField] AudioClip mainMenuBackgroundMusic;
        [SerializeField] AudioClip menuButtonSFX;


        private int selectedLevelIndex = 0;

        private Coroutine creditsDelayRoutine;


        public void OnLevel_01ButtonClick() { level_02BorderImage.color = defaultColor; level_01BorderImage.color = selectedColor; SetSelectedLevel(1); }
        public void OnLevel_02ButtonClick() { if (LevelRemember.GetLastCompletedLevelIndex() <= 0) return;  level_01BorderImage.color = defaultColor; level_02BorderImage.color = selectedColor;  SetSelectedLevel(2); }
        public void LeaveAppliction() { Application.Quit();  }


        private void Start()
        {
            if (LevelRemember.GetLastCompletedLevelIndex() <= 0)
            {
                level_02Icon.color = blockedColor;
            }

            if (mainMenuBackgroundMusic == null) return;

            AudioManager.Instance?.StopMusic();
            AudioManager.Instance?.PlayMusic(mainMenuBackgroundMusic);


        }

        public void OnCreditsButtonClicked()
        {
            if (creditsText == null) return;

            creditsText.SetActive(true);

            if (creditsDelayRoutine != null)
                creditsDelayRoutine = null;

            creditsDelayRoutine = StartCoroutine(CreditsDisableDelay());
        }

        private IEnumerator CreditsDisableDelay()
        {


            yield return new WaitForSeconds(1f);
            creditsText.SetActive(false);

        }


        private int SetSelectedLevel(int levelIndex)
        {
            selectedLevelIndex = levelIndex;
            return selectedLevelIndex;
        }

        public void OnClickMenuButton()
        {

            if (menuButtonSFX == null) return;

            AudioManager.Instance?.PlaySFX(menuButtonSFX);

        }

        public void OpenLevelSelection()
        {
            mainMenuPanel.SetActive(false);
            levelSelectionPanel.SetActive(true);
        }

        public void OpenMainMenu()
        {
            levelSelectionPanel.SetActive(false);
            mainMenuPanel.SetActive(true);

        }

        public void StartSelectedLevelScene()
        {
            if (selectedLevelIndex == 0) return;

            if (selectedLevelIndex == 1)
            {
                SceneManager.LoadScene(level_01SceneName);
                SceneManager.LoadScene(interfaceSceneName, LoadSceneMode.Additive);
            }

            else if (selectedLevelIndex == 2)
            {
                SceneManager.LoadScene(level_02SceneName);
                SceneManager.LoadScene(interfaceSceneName, LoadSceneMode.Additive);

            }
        }
    }
}
