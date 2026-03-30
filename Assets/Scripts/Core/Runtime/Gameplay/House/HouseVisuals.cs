using System.Collections;
using UnityEngine;

namespace NumericNook.Core.Runtime.Gameplay
{
    public class HouseVisuals : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer spriteRenderer;


        [SerializeField] private Color defaultColor = new Color(0.2f, 0.5f, 1f);
        [SerializeField] private Color correctColor = new Color(0.2f, 0.8f, 0.3f);
        [SerializeField] private Color wrongColor = new Color(0.9f, 0.2f, 0.2f);

        [SerializeField] private float wrongFlashDuration = 0.8f;

        private Coroutine wrongCoroutine;

        public Color CorrectColor => correctColor;
        public void SetDefaultColor() => spriteRenderer.color = defaultColor;
        public void SetCorrectColor() => spriteRenderer.color = correctColor;

        public void PlayWrongCoroutine()
        {

            if (wrongCoroutine != null) StopCoroutine(wrongCoroutine);
            wrongCoroutine = StartCoroutine(WrongFlashCoroutine());
        }


        private IEnumerator WrongFlashCoroutine()
        {

            spriteRenderer.color = wrongColor;
            yield return new WaitForSeconds(wrongFlashDuration);
            spriteRenderer.color = defaultColor;
            wrongCoroutine = null;
        }
    }
}
