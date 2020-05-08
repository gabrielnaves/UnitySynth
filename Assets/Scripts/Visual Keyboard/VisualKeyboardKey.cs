using UnityEngine;

namespace Synth
{
    public class VisualKeyboardKey : MonoBehaviour
    {
        public Sprite notPressed;
        public Sprite pressed;

        public int index;
        public bool isPressed;

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetNotPressed()
        {
            isPressed = false;
            spriteRenderer.sprite = notPressed;
        }

        public void SetPressed()
        {
            isPressed = true;
            spriteRenderer.sprite = pressed;
        }
    }
}
