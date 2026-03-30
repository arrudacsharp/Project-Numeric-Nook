using UnityEngine;
using System.Collections.Generic;
using NumericNook.Core.Runtime.Data;
using NumericNook.Core.Runtime.Audio;


namespace NumericNook.Core.Runtime.Gameplay
{
    public class LevelSetup : MonoBehaviour
    {

        [SerializeField] private LevelData levelData;
        [SerializeField] Transform housesParent;
        [SerializeField] private List<HouseController> houses;
        [SerializeField] private AudioClip levelBackgroundMusic;

        [Header("Debug")]
        [SerializeField] private bool enableDebugs = false;


        public List<MathExpression> ActiveExpressions { get; private set; }
        public List<int> TokenPool { get; private set; }

        private void Awake()
        {
            if (levelData == null)
            {
                Debug.LogWarning("LEVEL SETUP: No Level Data assigned!");
                return;
            }

            if (housesParent == null)
            {
                Debug.LogWarning("LEVEL SETUP: No Houses Parent assigned!");
                return;
            }


            houses = new List<HouseController>(housesParent.GetComponentsInChildren<HouseController>());

            if (enableDebugs)
            Debug.Log("LEVEL SETUP: Found " + houses.Count + " houses in the scene.");


            if (houses.Count != levelData.HouseCount)
                Debug.LogWarning($"LEVEL SETUP: Expected {levelData.HouseCount} houses but found {houses.Count} in the scene.");


            ActiveExpressions = levelData.GetLevelExpressions();
            TokenPool = levelData.BuildTokenPool(ActiveExpressions);

            for (int i = 0; i < houses.Count; i++)
            {
                if (i >= ActiveExpressions.Count) break;
                houses[i].Initialize(ActiveExpressions[i]);
            }
        }

        private void Start()
        {
            if (AudioManager.Instance == null || levelBackgroundMusic == null) return;

            AudioManager.Instance?.CrossFadeMusic(levelBackgroundMusic);
        }
    }
}
