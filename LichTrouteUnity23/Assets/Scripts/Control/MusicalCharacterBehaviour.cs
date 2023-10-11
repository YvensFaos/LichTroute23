using System.Collections;
using DG.Tweening;
using FMODUnity;
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
        [SerializeField, ReadOnly]
        private MusicalInstrumentParameterPair parameterPair;
        
        public void Initialize(MusicalCharacterSO musicalCharacterSo)
        {
            this.musicalCharacterSo = musicalCharacterSo;
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
            emitter.SetParameter(parameterPair.One, parameterPair.Two);
        }

        public void ResetMusicParameters(StudioEventEmitter emitter)
        {
            emitter.SetParameter(parameterPair.One, 0.0f);
        }

        public string Character => musicalCharacter.character;
    }
}