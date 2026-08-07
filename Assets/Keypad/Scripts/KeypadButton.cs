using System.Collections;
using UnityEngine;

namespace NavKeypad
{
    public class KeypadButton : MonoBehaviour, global::IInteractable
    {
        [Header("Value")]
        [SerializeField] private string value;

        [Header("Button Animation Settings")]
        [SerializeField] private float bttnspeed = 0.1f;
        [SerializeField] private float moveDist = 0.0025f;
        [SerializeField] private float buttonPressedTime = 0.1f;

        [Header("Component References")]
        [SerializeField] private Keypad keypad;

        private bool _moving;
        public bool CanInteract()
        {
            return true;
        }
        public void Interact()
        {
            PressButton();
        }
        
        public void PressButton()
        {
            if (_moving || keypad == null)
            {
                return;
            }

            keypad.AddInput(value);
            StartCoroutine(MoveSmooth());
        }

        private IEnumerator MoveSmooth()
        {
            _moving = true;

            Vector3 startPos = transform.localPosition;
            Vector3 pressedPos = startPos + new Vector3(0f, 0f, moveDist);

            float elapsedTime = 0f;

            while (elapsedTime < bttnspeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / bttnspeed);
                transform.localPosition = Vector3.Lerp(startPos, pressedPos, t);
                yield return null;
            }

            transform.localPosition = pressedPos;

            yield return new WaitForSeconds(buttonPressedTime);

            elapsedTime = 0f;

            while (elapsedTime < bttnspeed)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / bttnspeed);
                transform.localPosition = Vector3.Lerp(pressedPos, startPos, t);
                yield return null;
            }

            transform.localPosition = startPos;
            _moving = false;
        }
    }
}