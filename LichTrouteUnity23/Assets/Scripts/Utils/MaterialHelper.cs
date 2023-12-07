using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public static class MaterialHelper
    {
        public static Tweener AnimateFloat(Material material, string parameter, float tweenTo, float time, UnityAction callback)
        {
            return DOTween.To(() => material.GetFloat(parameter), value => material.SetFloat(parameter, value), tweenTo,
                time).OnComplete(callback.Invoke);
        }
        
        public static Tweener AnimateFloat(Material material, int parameter, float tweenTo, float time, UnityAction callback)
        {
            return DOTween.To(() => material.GetFloat(parameter), value => material.SetFloat(parameter, value), tweenTo,
                time).OnComplete(callback.Invoke);
        }
    }
}