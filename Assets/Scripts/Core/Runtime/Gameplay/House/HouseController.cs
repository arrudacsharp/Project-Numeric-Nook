using NumericNook.Core.Runtime.Data;
using NumericNook.Core.Runtime.Audio;
using System;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

namespace NumericNook.Core.Runtime.Gameplay

{
    public class HouseController : MonoBehaviour
    {

        [SerializeField] private HouseVisuals houseVisuals;
        [SerializeField] private TextMeshProUGUI expressionText;

        [Header("Debug")]
        [SerializeField] private bool enableDebugs = false;
        [SerializeField] private int debugTestValue;

        [SerializeField] private AudioClip houseWrongSFX;
        [SerializeField] private AudioClip houseCorrectSFX;


        public static event Action<HouseController> OnHouseSolved;
        public static event Action OnHouseWrongAttempt;


        private MathExpression expression;
        public bool IsSolved { get; private set; }


        private void Awake()
        {

            if (houseVisuals == null || expressionText == null)
            {

                Debug.LogWarning("HOUSE CONTROLLER: The House Controller was disabled because wasn't configured correctly!");
                gameObject.SetActive(false);
                return;
            }
        }


        public void Initialize(MathExpression assignedExpression)
        {

            expression = assignedExpression;
            expressionText.text = expression.GetExpressionText() + " = ?";
            houseVisuals.SetDefaultColor();

        }

        public bool TryDeliver(int value)
        {

            if (value == expression.Awnser)
            {

                if (IsSolved)
                {
                    if (enableDebugs)
                        Debug.Log($"HOUSE CONTROLLER: This house is already correct with the result: {expression.Awnser}");
                    return false;
                }


                if (value == expression.Awnser)
                {

                    IsSolved = true;
                    expressionText.text = expression.GetExpressionText() + $" = {value}";
                    expressionText.color = houseVisuals.CorrectColor;
                    houseVisuals.SetCorrectColor();
                    OnHouseSolved?.Invoke(this);
                    AudioManager.Instance?.PlaySFX(houseCorrectSFX);


                    if (enableDebugs)
                    Debug.Log($"HOUSE CONTROLLER: The value for this expression is RIGHT! Value: {value}");

                    return true;

                }
            }

            if (enableDebugs)
            Debug.Log($"HOUSE CONTROLLER: The value for this expression is WRONG! Value: {value}");

            houseVisuals.PlayWrongCoroutine();
            OnHouseWrongAttempt?.Invoke();
            AudioManager.Instance?.PlaySFX(houseWrongSFX);

            return false;
        }





        //Debug
        [ContextMenu("Test Try Solve")]
        private void DebugTrySolveExpression()
        {

            var value = debugTestValue;
            TryDeliver(value);

        }
    }
}
