using System.Collections.Generic;
using Model;
using UnityEngine;
using Utils;

public class MusicalControl : Singleton<MusicalControl>
{
    [SerializeField]
    private Queue<MusicalCharacter> stageCharacters;
    [SerializeField]
    private Queue<MusicalCharacter> waitingCharacters;
}
