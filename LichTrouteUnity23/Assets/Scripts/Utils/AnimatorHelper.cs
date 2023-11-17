using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public static class AnimatorHelper
    {
        public static Tweener AnimateFloat(Animator animator, string parameter, float tweenTo, float time, UnityAction callback)
        {
            return AnimateFloat(animator, Animator.StringToHash(parameter), tweenTo, time, callback);
        }
        
        public static Tweener AnimateFloat(Animator animator, int parameter, float tweenTo, float time, UnityAction callback)
        {
            return DOTween.To(() => animator.GetFloat(parameter), value => animator.SetFloat(parameter, value), tweenTo,
                time).OnComplete(callback.Invoke);
        }
    }
}