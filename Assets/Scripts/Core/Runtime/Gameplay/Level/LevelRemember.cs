using UnityEngine;

namespace NumericNook.Core.Runtime.Gameplay
{
    public static class LevelRemember
    {
        private static int lastCompletedLevelIndex;
        public static void SetLastCompletedLevelIndex(int levelIndex) => lastCompletedLevelIndex = levelIndex;
        public static int GetLastCompletedLevelIndex() => lastCompletedLevelIndex;

        public static bool IsLevel_01Completed;


    }
}
