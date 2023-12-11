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
        [SerializeField, ReadOnly]
        private MusicalCharacter musicalCharacter;
        [SerializeField]
        private Animator characterAnimator;
        [SerializeField]
        private List<SpriteRenderer> spriteRenders;
        [SerializeField] 
        private Material spriteMaterial;

        [Header("Reference")] 
        [SerializeField]
        private SpriteRenderer headSpriteRenderer;
        [SerializeField]
        private SpriteRenderer topSpriteRenderer;
        [SerializeField]
        private SpriteRenderer shirtSpriteRenderer;

        [Header("Database")] 
        [SerializeField] 
        private MusicalCharacterPartsSO characterParts;
        
        [Header("FMOD")] 
        [SerializeField, ReadOnly] 
        private StudioEventEmitter eventEmitter;
        [SerializeField] 
        private List<EmitterNamePair> instrumentEmitters;

        private Material internalMaterial;

        private string[] animatorParameters;
        private Coroutine idleCoroutine;
        private static readonly int AnimatorWalk = Animator.StringToHash("Walk");
        private static readonly int AnimatorIdle = Animator.StringToHash("IdleState");
        private static readonly int AnimateTrigger = Animator.StringToHash("Animate");
        private static readonly int AnimatorAnimation = Animator.StringToHash("Animation");

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
        }
        
        public void Initialize(MusicalCharacter musicalCharacter)
        {
            this.musicalCharacter = musicalCharacter;
            StartCoroutine(LazyInitialize());
            idleCoroutine = StartCoroutine(IdleCoroutine());
        }

        private IEnumerator LazyInitialize()
        {
            //Waits for one frame and then sets the character parts
            yield return null;
            ComposeCharacter();
        }

        [Button("Refresh Compose")]
        private void ComposeCharacter()
        {
            var head = musicalCharacter.head;
            var headSprite = characterParts.GetHeadSprite(head);
            headSpriteRenderer.sprite = headSprite;
            var body = musicalCharacter.body;
            var topSprite = characterParts.GetTopSprite(body);
            topSpriteRenderer.sprite = topSprite;
            var skirtSprite = characterParts.GetSkirtSprite(body);
            shirtSpriteRenderer.sprite = skirtSprite;
            
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
            var instrument = this.musicalCharacter.GetInstrument();
            var studioEmitterPair = instrumentEmitters.Find(pair => pair.One.Equals(instrument));
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

        public void AnimateAction(int action)
        {
            characterAnimator.SetTrigger(AnimateTrigger);
            //Clamp to force the animation to be with the 3 possible animations
            characterAnimator.SetFloat(AnimatorAnimation, Mathf.Clamp(action, 0, 2));
        }

        [Button("Animate Randomly")]
        public int AnimateRandomly()
        {
            var animation = Random.Range(0, 3);
            AnimateAction(animation);
            return animation;
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
            SetEvent(emitter, musicalCharacter.GetInstrument(), 1.0f, playlist);
        }

        public void ResetMusicParameters(StudioEventEmitter emitter)
        {
            // eventEmitter.Stop();
            SetEvent(emitter, musicalCharacter.GetInstrument(), 0.0f, 0);
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

        private void Animate(string animationToPlay, bool check)
        {
            if (characterAnimator == null) return;
            if (animatorParameters.Contains(animationToPlay))
            {
                characterAnimator.SetBool(animationToPlay, check);    
            }
        }

        public string UID => musicalCharacter.UID;
    }
}