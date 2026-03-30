using System;
using UnityEngine;
using NumericNook.Core.Runtime.Data;

namespace NumericNook.Core.Runtime.Gameplay
{
    public class LevelController : MonoBehaviour
    {

        [SerializeField] private LevelSetup levelSetup;
        [SerializeField] private string nextLevelSceneName;
        [SerializeField] private bool isFinalLevel = false;
        [SerializeField] private string levelTutorialText;
        [SerializeField] private string levelTutorialOperator;

        public static event Action<LevelResult> OnLevelComplete;
        public static event Action LevelControllerReady;

        private int housesSolved;
        private int wrongCount;

        public string NextLevelSceneName => nextLevelSceneName;
        public string LevelTutorialText => levelTutorialText;
        public string LevelTutorialOperator => levelTutorialOperator;
        public bool IsFinalLevel => isFinalLevel;

        private void OnEnable()
        {
            HouseController.OnHouseSolved += HandleHouseSolved;
            HouseController.OnHouseWrongAttempt += HandleWrongAnswer;
            LevelControllerReady?.Invoke();
        }

        private void OnDisable()
        {
            HouseController.OnHouseSolved -= HandleHouseSolved;
            HouseController.OnHouseWrongAttempt -= HandleWrongAnswer;
        }

        private void HandleWrongAnswer() => wrongCount++;


        private void HandleHouseSolved(HouseController house)
        {

            housesSolved++;

            if (housesSolved < levelSetup.ActiveExpressions.Count) return;

            var result = new LevelResult(housesSolved, housesSolved, wrongCount);
            // Only works for 2 levels, 3 or more levels would require a different approach
            LevelRemember.SetLastCompletedLevelIndex(LevelRemember.GetLastCompletedLevelIndex() + 1);
            OnLevelComplete?.Invoke(result);

        }
    }
}
