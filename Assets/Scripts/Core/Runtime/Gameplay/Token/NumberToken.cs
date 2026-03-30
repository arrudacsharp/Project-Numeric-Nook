using UnityEngine;
using TMPro;
using NumericNook.Core.Runtime.Data;
using NumericNook.Core.Runtime.Interfaces;
using NumericNook.Core.Runtime.Player;

namespace NumericNook.Core.Runtime
{
    public class NumberToken : MonoBehaviour, IInteractable
    {

        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Vector3 carryOffset = new Vector3(0f, 1.2f, 0f);
        [SerializeField] private Collider2D tokenCollider;

        [Header("Debug")]
        [SerializeField] private bool enableDebugs = false;


        private int tokenValue;
        private bool isCarried = false;
        private Transform playerTransform;


        public int TokenValue => tokenValue;
        public bool IsCarried => isCarried;


        public void Initialize(int value)
        {
            tokenValue = value;
            valueText.text = value.ToString();

        }

        public void Interact(PlayerInteractor interactor)
        {

            if (isCarried)
            {
                if (enableDebugs)
                    Debug.Log("NUMBER TOKEN: The number is already being carried. Cannot interact.");
                return;
            }


            isCarried = true;
            playerTransform = interactor.transform;

            GetComponent<Collider2D>().enabled = false; // Disable collider to prevent further interactions
            if (tokenCollider)
                tokenCollider.enabled = false; // Disable the token's collider to prevent interactions while being carried

        }

        public void Drop()
        {
            isCarried = false;
            playerTransform = null;

            GetComponent<Collider2D>().enabled = true; // Re-enable collider for interactions
            if (tokenCollider)
                tokenCollider.enabled = true;
        }


        private void LateUpdate()
        {
            if (!isCarried) return;

            transform.position = playerTransform.position + carryOffset;
        }

    }
}
