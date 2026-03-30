using UnityEngine;

namespace NumericNook.Core.Runtime.Data
{

    public enum LevelGrade { S, A, B, C, D, F }



    public class LevelResult
    {


        public int HouseSolved { get; }
        public int CorrectCount { get; }
        public int WrongCount { get; }
        public LevelGrade Grade { get; }


        public LevelResult(int houseSolved, int correctCount, int wrongCount)
        {
            HouseSolved = houseSolved;
            CorrectCount = correctCount;
            WrongCount = wrongCount;
            Grade = CalculateGrade(wrongCount);
        }

        private static LevelGrade CalculateGrade(int wrongCount) => wrongCount switch
        {

            0 => LevelGrade.S,
            <= 2 => LevelGrade.A,
            <= 5 => LevelGrade.B,
            <= 10 => LevelGrade.C,
            <= 15 => LevelGrade.D,
            _ => LevelGrade.F
        };
    }
}
