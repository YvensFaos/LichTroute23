using Model;
using UnityEngine;

namespace Control
{
    public class MusicalCharacterBehaviour : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField]
        private MusicalCharacter character;
        
        [Header("Components")]
        [SerializeField]
        private AudioSource source;
    
        public void Enqueue() 
        { }

        public void Dequeue()
        { }

        public void PlayMusic()
        {
            
        }
    }
}