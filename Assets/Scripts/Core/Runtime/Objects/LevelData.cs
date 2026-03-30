using System.Collections.Generic;
using UnityEngine;

namespace NumericNook.Core.Runtime.Data
{


    [CreateAssetMenu(fileName = "LevelData_", menuName = "NumberNook/Level Data")]
    public class LevelData : ScriptableObject
    {

        [Header("References & Variables")]
        [SerializeField] private MathExpressionRegistry mathExpressionRegistry;
        [SerializeField] private int houseCount = 3;
        [SerializeField] private int decoyCount = 5;

        [Header("Decoy Number Range")]
        [SerializeField] private int decoyRangeMin = 1;
        [SerializeField] private int decoyRangeMax = 20;




        public MathExpressionRegistry MathExpressionRG => mathExpressionRegistry;
        public int HouseCount => houseCount;
        public int DecoyCount => decoyCount;


        public List<MathExpression> GetLevelExpressions()
        {
            if (mathExpressionRegistry == null)
            {
                Debug.LogWarning("LEVEL DATA: No MathExpressionRegistry assigned");
                return new List<MathExpression>();
            }

            return mathExpressionRegistry.GetRandom(houseCount);

        }

        public List<int> BuildTokenPool(List<MathExpression> selectedExpressions)
        {
            var pool = new HashSet<int>();

            foreach (var expression in selectedExpressions)
                pool.Add(expression.Awnser);

            int attempts = 0;
            while (pool.Count < selectedExpressions.Count + decoyCount && attempts < 100)
            {

                pool.Add(Random.Range(decoyRangeMin, decoyRangeMax));
                attempts++;
            }

            if (attempts >= 100)
                Debug.LogWarning("LEVEL DATA: Could not fill decoy pool without duplicates. Consider incresing the decoy range");


            return new List<int>(pool);

        }
    }
}
