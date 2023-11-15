using DG.Tweening;
using FMODUnity;
using Model;
using NaughtyAttributes;
using UnityEngine;
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
            DebugUtils.DebugLogMsg($"Setting {parameterPair.One} with {parameterPair.Two}");
            emitter.SetParameter($"{parameterPair.One} ON-OFF", parameterPair.Two);
            emitter.SetParameter($"{parameterPair.One} VOL", parameterPair.Two);
        }

        public void ResetMusicParameters(StudioEventEmitter emitter)
        {
            emitter.SetParameter($"{parameterPair.One} ON-OFF", 0.0f);
            emitter.SetParameter($"{parameterPair.One} VOL", 0.0f);
        }

        public bool CompareUID(string uid) => musicalCharacter.UID.Equals(uid);

        public string Character => musicalCharacter.character;
    }
}