using NumericNook.Core.Runtime.Gameplay;
using UnityEngine;
using System.Collections.Generic;

namespace NumericNook.Core.Runtime
{
    public class TokenSpawner : MonoBehaviour
    {

        [SerializeField] private LevelSetup levelSetup;
        [SerializeField] private Collider2D spawnAreaCollider;
        [SerializeField] private GameObject tokenPrefab;
        [SerializeField] private Transform tokenParent;

        [SerializeField] private LayerMask obstableLayer;
        [SerializeField] private float minDistanceBetweenTokens = 1.5f;
        [SerializeField] private float tokenCheckRadius = 0.4f;
        [SerializeField] private int maxSpawnAttempts = 10;


        private List<Vector2> spawnedPositions = new();


        private void Start()
        {
            if (levelSetup == null)
            {
                Debug.LogWarning("TOKEN SPAWNER: No LevelSetup assigned!");
                return;
            }

            if (spawnAreaCollider == null)
            {
                Debug.LogWarning("TOKEN SPAWNER: No Spawn Area Collider assigned!");
                return;
            }

            SpawnTokens();

        }

        private void SpawnTokens()
        {

            List<int> pool = levelSetup.TokenPool;

            foreach (int value in pool)
            {

                Vector2 position = GetValidPosition();


                if (position == Vector2.negativeInfinity)
                {
                    Debug.LogWarning($"TOKEN SPAWNER: Could not find valid position for token with value {value}. Skipping spawn.");
                    continue;
                }

                SpawnToken(value, position);
                spawnedPositions.Add(position);

            }
        }


        private void SpawnToken(int value, Vector2 position)
        {

            GameObject instance = Instantiate(tokenPrefab, position, Quaternion.identity);

            if (tokenParent != null)
            instance.transform.SetParent(tokenParent);

            if (instance.TryGetComponent(out NumberToken token))
                token.Initialize(value);
        }

        private Vector2 GetValidPosition()
        {

            Bounds bounds = spawnAreaCollider.bounds;

            for (int i = 0; i < maxSpawnAttempts; i++)
            {

                Vector2 candidate = new Vector2(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y)
                );

                if (!IsPositionValid(candidate))
                    continue;

                return candidate;
            }

            return Vector2.negativeInfinity;
        }

        private bool IsPositionValid(Vector2 position)
        {
            if (Physics2D.OverlapCircle(position, tokenCheckRadius, obstableLayer))
                return false;

            foreach (Vector2 existing in spawnedPositions)
            {
                if (Vector2.Distance(position, existing) < minDistanceBetweenTokens)
                    return false;
            }
            return true;

        }


        private void OnDrawGizmos()
        {
            if (spawnAreaCollider == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(spawnAreaCollider.bounds.center, spawnAreaCollider.bounds.size);
        }
    }
}
