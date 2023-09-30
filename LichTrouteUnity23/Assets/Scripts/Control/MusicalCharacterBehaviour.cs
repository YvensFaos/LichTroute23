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
        private AudioSource source;
        [SerializeField, ReadOnly]
        private AudioClip instrumentClip;

        private void Awake()
        {
            AssessUtils.CheckRequirement(ref source, this, true);
        }

        public void Initialize(MusicalCharacterSO musicalCharacterSo)
        {
            this.musicalCharacterSo = musicalCharacterSo;
        }

        public void Enqueue(MusicalCharacter musicalCharacter)
        {
            this.musicalCharacter = musicalCharacter;
            instrumentClip = musicalCharacterSo.GetAudioClipFromInstrument(musicalCharacter.instrument);
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

        private IEnumerator PlayMusicCoroutine()
        {
            source.PlayOneShot(instrumentClip);
            yield return new WaitUntil(() => !source.isPlaying);
            
        }

        public string Character => musicalCharacter.character;
    }
}