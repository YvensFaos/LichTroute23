using System.Collections;
using System.Collections.Generic;
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
        [SerializeField]
        private List<SpriteRenderer> spriteRenders;
        [SerializeField] 
        private Material spriteMaterial;
        
        [Header("Database")]
        [SerializeField] 
        private InstrumentDatabase instrumentDatabase;
        
        [Header("FMOD")] 
        [SerializeField, ReadOnly] 
        private StudioEventEmitter eventEmitter;
        [SerializeField] 
        private List<EmitterNamePair> instrumentEmitters;

        private Material internalMaterial;

        // private readonly float playInstrumentDelay = 1.0f;
        private string[] animatorParameters;
        private Coroutine idleCoroutine;
        private static readonly int AnimatorWalk = Animator.StringToHash("Walk");
        private static readonly int AnimatorIdle = Animator.StringToHash("IdleState");

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref characterAnimator, this);
        }

        private void Start()
        {
            animatorParameters = characterAnimator.parameters.Select(param => param.name).ToArray();

            internalMaterial = new Material(spriteMaterial);
            spriteRenders.ForEach(render =>
            {
                render.material = internalMaterial;
            });
            internalMaterial.SetColor("_Multiply", new Vector4(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), 3.0f));
        }
        
        public void Initialize(MusicalCharacterSO musicalCharacterSo)
        {
            this.musicalCharacterSo = musicalCharacterSo;
            idleCoroutine = StartCoroutine(IdleCoroutine());
        }

        public void WalkTo(Vector3 newPosition, float time, UnityAction callback)
        {
            StopCoroutine(idleCoroutine);
            characterAnimator.SetFloat(AnimatorWalk, 1.0f);
            AnimatorHelper.AnimateFloat(characterAnimator, AnimatorWalk, 1.0f, 0.5f, () => { });
            transform.DOMove(newPosition, time).OnComplete(() =>
            {
                callback();
                AnimatorHelper.AnimateFloat(characterAnimator, AnimatorWalk, 0.0f, 0.5f, () =>
                {
                    idleCoroutine = StartCoroutine(IdleCoroutine());
                });
            });
        }

        public void Enqueue(MusicalCharacter musicalCharacter)
        {
            this.musicalCharacter = musicalCharacter;
            parameterPair = this.musicalCharacter.GetPair();
            var pair = instrumentDatabase.GetPairForInstrument(parameterPair.One);

            var studioEmitterPair = instrumentEmitters.Find(pair => pair.One.Equals(parameterPair.One));
            var studioEmitter = studioEmitterPair.Two;
            studioEmitter.gameObject.SetActive(true);

            eventEmitter = studioEmitter;
        }

        public void MoveToStage(Transform moveOutTransform, Transform stageTransform, Transform stageParent, UnityAction callback)
        {
            transform.DOMove(moveOutTransform.position, 1.0f).OnComplete(() =>
            {
                var alphaPower = "_AlphaPower";
                internalMaterial.SetFloat(alphaPower, 0.0f);
                transform.DOMove(stageTransform.position, 0.15f).OnComplete(() =>
                {
                    MaterialHelper.AnimateFloat(internalMaterial, alphaPower, 1.0f, 0.5f, () =>
                    {
                        transform.SetParent(stageParent);
                        callback();
                    });
                });
            });
        }

        private IEnumerator IdleCoroutine()
        {
            while (true)
            {
                var idleTo = Random.Range(0, 4);
                var repeat = false;
                AnimatorHelper.AnimateFloat(characterAnimator, AnimatorIdle, idleTo, 1.5f, () => { repeat = true; });
                yield return new WaitUntil(() => repeat);
                yield return new WaitForSeconds(Random.Range(1.0f, 8.0f));
            }
        }

        public void SetMusicParameters(StudioEventEmitter emitter, int playlist)
        {
            // eventEmitter.EventReference = eventReference;
            // eventEmitter.Play();
            SetEvent(emitter, parameterPair.One, 1.0f, playlist);
        }

        public void ResetMusicParameters(StudioEventEmitter emitter)
        {
            // eventEmitter.Stop();
            SetEvent(emitter, parameterPair.One, 0.0f, 0);
        }

        private void SetEvent(StudioEventEmitter emitter, string instrument, float value, int playlist)
        {
            //Main
            var instrumentParameter = $"{instrument}_ON-OFF";
            emitter.SetParameter(instrumentParameter, value);
            // emitter.SetParameter($"{instrument} VOL", value);
            
            //Character
            eventEmitter.SetParameter($"{instrument} ON-OFF", value);
            // eventEmitter.SetParameter($"{instrument} VOL", value);
            eventEmitter.SetParameter("PlayList", playlist);
            
            DebugUtils.DebugLogMsg($"{name} -> {instrumentParameter} set to {value}. PlayList: {playlist}");
            Animate(instrument, value > 0);
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