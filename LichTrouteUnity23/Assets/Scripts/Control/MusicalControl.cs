using System.Collections;
using System.Collections.Generic;
using Control;
using Model;
using UnityEngine;
using Utils;

public class MusicalControl : Singleton<MusicalControl>
{
    private List<MusicalCharacterBehaviour> stageCharacters;
    private Queue<MusicalCharacterBehaviour> waitingCharacters;
    private Queue<MusicalCharacter> queuedCharacters;

    [Header("Data")] 
    [SerializeField, Range(1, 20)] 
    private int stageSize;
    [SerializeField]
    private List<Transform> stagePositions;
    
    [Header("References")]
    [SerializeField]
    private Transform musicalPerformanceParent;
    [SerializeField]
    private Transform musicalWaitingQueueParent;

    [Header("Database")] 
    [SerializeField] 
    private List<MusicalCharacterSO> musicalCharacters;

    private const float orchestraDelayCheckTimer = 1.0f;
    private int stageIndex = 0;

    private void Start()
    {
        stageCharacters = new List<MusicalCharacterBehaviour>();
        waitingCharacters = new Queue<MusicalCharacterBehaviour>();
        queuedCharacters = new Queue<MusicalCharacter>();
        
        //Start the coroutines for the Musical Control
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(OrchestraCoroutine());
    }

    public void QueueMusicalCharacterSpawning(MusicalCharacter musicalCharacter)
    {
        DebugUtils.DebugLogMsg($"Queued the Spawn of {musicalCharacter}.");
        queuedCharacters.Enqueue(musicalCharacter);
    }

    private IEnumerator OrchestraCoroutine()
    {
        IEnumerator Perform()
        {
            yield return new WaitForSeconds(1.0f);
            stageCharacters.ForEach(character => character.PlayMusic());
            //TODO have the correct time to wait for the end of the performance
            yield return new WaitForSeconds(1.0f);
            
            //Destroy characters on stage
            stageCharacters.ForEach(character => Destroy(character.gameObject));
            yield return new WaitForSeconds(0.1f);
            //Reset the list
            stageCharacters = new List<MusicalCharacterBehaviour>();
            
            //Reset index
            stageIndex = 0;
        }
        
        while (true)
        {
            if (stageCharacters.Count == stageSize)
            {
                //Perform music
                yield return Perform();
            }
            else
            {
                yield return new WaitForSeconds(orchestraDelayCheckTimer);
                
                //Moves the waiting characters to the stage until either the stage is full or the waiting list is empty.
                while (waitingCharacters.Count > 0 && stageCharacters.Count < stageSize)
                {
                    var waitingCharacter = waitingCharacters.Dequeue();
                    stageCharacters.Add(waitingCharacter);
                    var stagePosition = stagePositions[stageIndex++];
                    waitingCharacter.MoveToStage(stagePosition, musicalPerformanceParent);
                }

                var waitingQueueEmpty = waitingCharacters.Count <= 0;
                var stageIsFull = stageCharacters.Count >= stageSize;

                if (stageIsFull)
                {
                    //Stage is full, then let the band perform
                    yield return Perform();
                }
                else if (waitingQueueEmpty)
                {
                    //Stage is not full, and the waiting queue is empty.
                    //TODO add logic to randomly generate characters in the queue if it remains empty for too long.
                    yield return new WaitUntil(() => waitingCharacters.Count > 0);
                } //else { Stage is not full and there are still characters waiting, then it should just repeat the first while from the start.
            }
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => queuedCharacters.Count > 0);
            while (queuedCharacters.Count > 0)
            {
                var musicalCharacter = queuedCharacters.Dequeue();
                var musicalCharacterSo = musicalCharacters.Find(so => so.character.Equals(musicalCharacter.character));
                if (musicalCharacterSo == null) continue; //Could not find the scriptable object. Proceed to the next one.
                var musicalCharacterBehaviorPrefab = musicalCharacterSo.Prefab;
                if (musicalCharacterBehaviorPrefab == null) continue; //Could not find the prefab. Proceed to the next one. 
                var musicalCharacterBehaviour = Instantiate(musicalCharacterBehaviorPrefab, musicalWaitingQueueParent);
                musicalCharacterBehaviour.Initialize(musicalCharacterSo);
                musicalCharacterBehaviour.Enqueue(musicalCharacter);
                waitingCharacters.Enqueue(musicalCharacterBehaviour);
            }
        }
    }

}