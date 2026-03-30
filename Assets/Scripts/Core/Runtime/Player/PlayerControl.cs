using UnityEngine;
using UnityEngine.InputSystem;

namespace NumericNook.Core.Runtime.Player
{
    public class PlayerControl : MonoBehaviour
    {

        #region References

        [Header("References")]
        [SerializeField]
        private Rigidbody2D playerRB;

        [SerializeField]
        private PlayerInput playerInput;

        private InputAction playerMoveAction;

        private Vector2 activeInput;

        #endregion

        [Header("Variables")]
        [SerializeField]
        private float playerMoveSpeed = 5f;


        private void Awake ()
        {

            if (playerRB == null)
            {
                Debug.LogWarning("PLAYER CONTROL: Make sure to reference the player character Rigidbody!");
                return;
            }

            if (playerInput == null)
            {

                Debug.LogWarning("PLAYER CONTROL: Make sure to reference the player Inputs asset");
                return;
            }

            playerMoveAction = playerInput.actions["Move"];

        }

        private void OnEnable()
        {
            playerMoveAction.Enable();
        }

        private void OnDisable()
        {
            playerMoveAction.Disable();
        }

        private void Update()
        {

            activeInput = playerMoveAction.ReadValue<Vector2>();

        }

        private void FixedUpdate()
        {
            playerRB.linearVelocity = activeInput * playerMoveSpeed;
        }

    }
}
