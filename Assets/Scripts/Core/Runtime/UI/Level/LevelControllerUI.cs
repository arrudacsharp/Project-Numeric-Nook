using NumericNook.Core.Runtime.Audio;
using NumericNook.Core.Runtime.Data;
using NumericNook.Core.Runtime.Gameplay;
using NumericNook.Core.Runtime.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NumericNook.Core.Runtime.UI
{
    public class LevelControllerUI : MonoBehaviour
    {

        [SerializeField] private GameObject completionPanel;

        [SerializeField] private TextMeshProUGUI gradeText;
        [SerializeField] private TextMeshProUGUI summaryText;

        [SerializeField] private string interfaceSceneName;
        [SerializeField] private string mainMenuSceneName;

        [SerializeField] private GameObject nextLevelButton;

        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private TextMeshProUGUI tutorialTextUI;
        [SerializeField] private TextMeshProUGUI operatorTextUI;
        [SerializeField] private string tutorialText;
        [SerializeField] private string operatorText;

        [SerializeField] private GameObject escMenuPanel;

        [SerializeField] private AudioClip menuButtonSFX;

        private LevelController levelController;

        private string NextLevelSceneName;
        private bool isEscMenuActive = false;

        private void OnEnable()
        {

            LevelController.OnLevelComplete += HandleLevelComplete;
            PlayerInteractor.OnPressedEsc += HandleEscPressed;
            LevelController.LevelControllerReady += ResolveLevelControllerReferences;
        }

        private void OnDisable()
        {
            LevelController.OnLevelComplete -= HandleLevelComplete;
            PlayerInteractor.OnPressedEsc -= HandleEscPressed;
            LevelController.LevelControllerReady -= ResolveLevelControllerReferences;
        }



        private void Start()
        {

            completionPanel.SetActive(false);
            ResolveLevelControllerReferences();

            if (tutorialPanel != null)
            {
                Time.timeScale = 0f;
                tutorialPanel.SetActive(true);
                tutorialTextUI.text = levelController.LevelTutorialText;
                operatorTextUI.text = levelController.LevelTutorialOperator;
            }
        }

        public void HandleEscPressed()
        {
            if (isEscMenuActive)
            {
                escMenuPanel.SetActive(false);
                isEscMenuActive = false;
                Time.timeScale = 1f;
                return;
            }

            if (!isEscMenuActive) 
            {
                Time.timeScale = 0f;
                escMenuPanel.SetActive(true);
                isEscMenuActive = true;
            }

        }



        public void OnQuitApplicationPressed()
        {
            Application.Quit();
        }



        public void CloseLevelTutorial()
        {
            Time.timeScale = 1f;
            tutorialPanel.SetActive(false);
        }

        private void ResolveLevelControllerReferences()
        {

            if (levelController == null)
            {
                levelController ??= FindFirstObjectByType<LevelController>();
            }

            NextLevelSceneName = levelController.NextLevelSceneName;
        }


        private void HandleLevelComplete(LevelResult result)
        {

            if (levelController.IsFinalLevel)
            {
                if (nextLevelButton != null)
                nextLevelButton.SetActive(false);
            }

            completionPanel.SetActive(true);
            Time.timeScale = 0f;

            gradeText.text = result.Grade.ToString();
            summaryText.text = $"Correct: {result.CorrectCount}\nMistakes: {result.WrongCount}";
        }

        public void OnNextLevelPressed()
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(NextLevelSceneName);
            SceneManager.LoadScene(interfaceSceneName, LoadSceneMode.Additive); 

        }

        public void OnBackToMenuPressed()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);

        }
        public void OnClickMenuButton()
        {

            if (menuButtonSFX == null) return;

            AudioManager.Instance?.PlaySFX(menuButtonSFX);

        }
    }
}
