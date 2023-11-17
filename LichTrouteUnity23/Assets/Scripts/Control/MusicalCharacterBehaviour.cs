using System.Linq;
using DG.Tweening;
using FMODUnity;
using Model;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Control
{
    public class MusicalCharacterBehaviour : MonoBehaviour
    {
        [Header("Data")] 
        [ReadOnly, SerializeField] 
        private MusicalCharacterSO musicalCharacterSo;
        [SerializeField, ReadOnly]
        private MusicalCharacter musicalCharacter;
        [SerializeField, ReadOnly]
        private MusicalInstrumentParameterPair parameterPair;
        [SerializeField]
        private Animator characterAnimator;

        private readonly float playInstrumentDelay = 1.0f;
        private string[] animatorParameters;
        private static readonly int AnimatorWalk = Animator.StringToHash("Walk");

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref characterAnimator, this);
        }

        private void Start()
        {
            animatorParameters = characterAnimator.parameters.Select(param => param.name).ToArray();
        }
        
        public void Initialize(MusicalCharacterSO musicalCharacterSo)
        {
            this.musicalCharacterSo = musicalCharacterSo;
        }

        public void WalkToTheQueue(Vector3 newQueuePosition, float time, UnityAction callback)
        {
            characterAnimator.SetFloat(AnimatorWalk, 1.0f);
            AnimatorHelper.AnimateFloat(characterAnimator, AnimatorWalk, 1.0f, 0.5f, () => { });
            transform.DOMove(newQueuePosition, time).OnComplete(() =>
            {
                callback();
                AnimatorHelper.AnimateFloat(characterAnimator, AnimatorWalk, 0.0f, 0.5f, () => { });
            });
        }

        public void Enqueue(MusicalCharacter musicalCharacter)
        {
            this.musicalCharacter = musicalCharacter;
            parameterPair = this.musicalCharacter.GetPair();
        }

        public void MoveToStage(Transform stageTransform, Transform stageParent)
        {
            transform.DOMove(stageTransform.position, 1.0f).OnComplete(() =>
            {
                transform.SetParent(stageParent);
            });
        }

        public void SetMusicParameters(StudioEventEmitter emitter)
        {
            var instrument = parameterPair.One;
            emitter.SetParameter($"{instrument} ON-OFF", parameterPair.Two);
            var paramVol = $"{instrument} VOL";
            Animate(instrument, true);
            DOTween.To(() =>
                {
                    emitter.EventInstance.getParameterByName(paramVol, out var value);
                    return value;
                },
                value => emitter.SetParameter(paramVol, value),
                1.0f,
                playInstrumentDelay);
        }

        public void ResetMusicParameters(StudioEventEmitter emitter)
        {
            var instrument = parameterPair.One;
            emitter.SetParameter($"{instrument} ON-OFF", 0.0f);
            var paramVol = $"{instrument} VOL";
            
            DOTween.To(() =>
                {
                    emitter.EventInstance.getParameterByName(paramVol, out var value);
                    return value;
                },
                value => emitter.SetParameter(paramVol, value),
                0.0f,
                playInstrumentDelay).OnComplete(() =>
            {
                Animate(instrument, false);
            });
        }

        public bool CompareUID(string uid) => musicalCharacter.UID.Equals(uid);

        private void Animate(string animation, bool check)
        {
            if (characterAnimator == null) return;
            if (animatorParameters.Contains(animation))
            {
                characterAnimator.SetBool(animation, check);    
            }
        }

        public string Character => musicalCharacter.character;
    }
}