using System.Collections;
using DG.Tweening;
using Model;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
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

        [Header("Components")] 
        [SerializeField]
        private FMODUnity.StudioEventEmitter eventEmitter;
        [SerializeField, ReadOnly]
        private AudioClip instrumentClip;

        [SerializeField, Range(0.0f, 1.0f)]
        private float bassRange;
        [SerializeField, Range(0.0f, 1.0f)]
        private float pianoRange;
        [SerializeField, Range(0.0f, 1.0f)]
        private float drumRange;
        
        private void Awake()
        {
            AssessUtils.CheckRequirement(ref eventEmitter, this, true);
        }

        public void Initialize(MusicalCharacterSO musicalCharacterSo)
        {
            this.musicalCharacterSo = musicalCharacterSo;
        }

        public void Enqueue(MusicalCharacter musicalCharacter)
        {
            this.musicalCharacter = musicalCharacter;
            //TODO
            // instrumentClip = musicalCharacterSo.GetAudioClipFromInstrument(musicalCharacter.instrument);
        }

        public void MoveToStage(Transform stageTransform, Transform stageParent)
        {
            transform.DOMove(stageTransform.position, 1.0f).OnComplete(() =>
            {
                transform.SetParent(stageParent);
            });
        }

        public void PlayMusic()
        {
            StartCoroutine(PlayMusicCoroutine());
        }

        [Button("Test Play")]
        private void TestPlay()
        {
            eventEmitter.SetParameter("Bass", bassRange);
            eventEmitter.SetParameter("Piano", pianoRange);
            eventEmitter.SetParameter("Drums", drumRange);
            eventEmitter.Play();
        }

        [Button("Stop Play")]
        private void StopPlay()
        {
            eventEmitter.Stop();
        }

        private IEnumerator PlayMusicCoroutine()
        {
            // source.PlayOneShot(instrumentClip);
            // yield return new WaitUntil(() => !source.isPlaying);
            //TODO
            yield return null;
        }

        public string Character => musicalCharacter.character;
    }
}