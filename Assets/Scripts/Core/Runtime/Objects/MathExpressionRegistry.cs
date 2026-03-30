using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace NumericNook.Core.Runtime.Data
{


    [CreateAssetMenu(fileName = "ExpressionRegistry_", menuName = "NumberNook/Math Expression Registry")]
    public class MathExpressionRegistry : ScriptableObject
    {

        [SerializeField] private List<MathExpression> expressions = new();


        public bool HasEnoughExpresisons( int required ) => expressions.Count > required;

        public List<MathExpression> GetRandom(int count)
        {

            if (!HasEnoughExpresisons(count))
            {
                Debug.LogWarning($"EXPRESSION REGISTRY: Requested {count} expressions but only {expressions.Count} available");
                return new List<MathExpression>();
            }

            var pool = new List<MathExpression>(expressions);
            var result = new List<MathExpression>();


            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, pool.Count);
                result.Add(pool[index]);
                pool.RemoveAt(index);
            }
            return result;
        }
    }
}
