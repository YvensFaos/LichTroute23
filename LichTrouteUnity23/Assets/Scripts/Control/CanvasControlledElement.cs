using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    public class CanvasControlledElement : MonoBehaviour
    {
        [SerializeField]
        private Image backImage;
        [SerializeField]
        private RectTransform imageRect;
        [SerializeField]
        private Outline outline;

        private Tweener outlineTween;

        private void Start()
        {
            backImage.enabled = false;
            outline.enabled = false;
        }

        public void AnimateOutline()
        {
            backImage.enabled = true;
            outline.enabled = true;
            var color = outline.effectColor;
            var transparentColor = outline.effectColor;
            transparentColor.a = 0.0f;
            outline.effectColor = transparentColor;

            outlineTween?.Kill();
        
            outlineTween = outline.DOColor(color, 0.4f).OnComplete(() =>
            {
                outline.effectColor = color;
                backImage.enabled = false;
                outline.enabled = false;
            });
        }

        public void Move(Vector2 movement)
        {
            imageRect.anchoredPosition += movement;
        }

        public void Scale(Vector3 scale)
        {
            imageRect.localScale += scale;
        }
    }
}
