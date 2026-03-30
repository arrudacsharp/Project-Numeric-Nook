using UnityEngine;

namespace NumericNook.Core.Runtime.Data
{

    public enum MathOperator { Add, Subtract, Multiply, Divide }


    [CreateAssetMenu(fileName = "Expression_", menuName = "NumberNook/Math Expression")]
    public class MathExpression : ScriptableObject
    {


        [SerializeField] private MathOperator mathOperator;
        [SerializeField] private int operandA;
        [SerializeField] private int operandB;


        public int OperandA => operandA;
        public int OperandB => operandB;
        public MathOperator MathOperator => mathOperator;


        // Helpers

        public int Awnser => mathOperator switch
        {


            MathOperator.Add => operandA + operandB,
            MathOperator.Subtract => operandA - operandB,
            MathOperator.Multiply => operandA * operandB,
            MathOperator.Divide => operandA != 0 ? operandB / operandB : 0,
            _ => 0
        };



        public string GetExpressionText() => mathOperator switch
        {


            MathOperator.Add => $"{operandA} + {operandB}",
            MathOperator.Subtract => $"{operandA} - {operandB}",
            MathOperator.Multiply => $"{operandA} x {operandB}",
            MathOperator.Divide => $"{operandA} % {operandB}",
            _ => "?",
        };
    }
}
