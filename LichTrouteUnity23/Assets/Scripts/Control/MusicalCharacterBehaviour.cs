using DG.Tweening;
using FMODUnity;
using Model;
using NaughtyAttributes;
using UnityEngine;

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

        private float playInstrumentDelay = 1.0f;
        
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
            emitter.SetParameter($"{parameterPair.One} ON-OFF", parameterPair.Two);
            var paramVol = $"{parameterPair.One} VOL";
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
            emitter.SetParameter($"{parameterPair.One} ON-OFF", 0.0f);
            var paramVol = $"{parameterPair.One} VOL";
            DOTween.To(() =>
                {
                    emitter.EventInstance.getParameterByName(paramVol, out var value);
                    return value;
                },
                value => emitter.SetParameter(paramVol, value),
                0.0f,
                playInstrumentDelay);
        }

        public bool CompareUID(string uid) => musicalCharacter.UID.Equals(uid);

        public string Character => musicalCharacter.character;
    }
}