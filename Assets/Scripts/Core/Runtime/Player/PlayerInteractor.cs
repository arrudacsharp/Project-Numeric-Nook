using System;
using NumericNook.Core.Runtime.Audio;
using NumericNook.Core.Runtime.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NumericNook.Core.Runtime.Player
{
    public class PlayerInteractor : MonoBehaviour
    {

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private float interactRadius = 1.2f;
        [SerializeField] private LayerMask interactableLayerMask;
        [SerializeField] private LayerMask houseLayerMask;

        [SerializeField] private AudioClip collectSFX;
        [SerializeField] private AudioClip dropSFX;

        [Header("Debug")]
        [SerializeField] private bool enableDebugs = false;

        private InputAction interactAction;
        private InputAction escAction;
        private NumberToken carriedToken;

        public static event Action OnPressedEsc;

        private void Awake()
        {
            if (playerInput == null)
            {
                Debug.LogWarning("PLAYER INTERACTOR: Make sure to reference the player Inputs asset.");
                return;
            }

            interactAction = playerInput.actions["Interact"];
            escAction = playerInput.actions["Esc"];
           
        }


        private void OnEnable() => interactAction?.Enable();
        private void OnDisable() => interactAction?.Disable();




        private void Update()
        {
            if (!interactAction.WasPressedThisFrame()) return;


            if (carriedToken != null)
            {
                TryDeliver();
                return;
            }

            if (carriedToken == null)
            {
                TryPickup();
            }
        }

        public void OnEsc()
        {
            OnPressedEsc?.Invoke();
        }


        private void TryPickup()
        {
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayerMask);

            if (hits.Length == 0) return;

            Collider2D closest = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance >= closestDistance) continue;

                closestDistance = distance;
                closest = hit;
            }

            if (closest == null ) return;

            if (closest.TryGetComponent(out NumberToken token))
            {
                token.Interact(this);
                carriedToken = token;
                AudioManager.Instance?.PlaySFX(collectSFX);


                if (enableDebugs)
                    Debug.Log($"Player Interactor: Picked up token with value {token.TokenValue}.");
            }
        }


        private void TryDeliver()
        {

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius, houseLayerMask);

            if (hits.Length == 0)
            {
                TryDrop();

                if (enableDebugs)
                    Debug.Log("Player Interactor: No house found, trying to drop token.");

                return; 
            }

            Collider2D closest = null;
            float closestDistance = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance >= closestDistance) continue;

                closestDistance = distance;
                closest = hit;
            }

            if (closest == null) return;

            if (closest.TryGetComponent(out HouseController house))
            {
                if (house.IsSolved == true)
                {
                    TryDrop();

                    if (enableDebugs)
                        Debug.Log("Player Interactor: House is already solved, trying to drop token.");

                    return;
                }

                bool correct = house.TryDeliver(carriedToken.TokenValue);

                if (correct)
                {
                    Destroy(carriedToken.gameObject);
                    carriedToken = null;
                }

                if (enableDebugs)
                    Debug.Log("Player Interactor: House founded, trying to deliver token.");

            }
        }

        private void TryDrop()
        {

            // Try to drop only if the player is currently carrying a token
            if (carriedToken != null)
            {
                carriedToken.Drop();
                carriedToken = null;
                AudioManager.Instance?.PlaySFX(dropSFX);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}
