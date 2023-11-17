using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] 
    private float performTime;

    [Header("FMOD")] 
    [SerializeField] 
    private FMODUnity.StudioEventEmitter eventEmitter;
    
    [Header("References")]
    [SerializeField]
    private Transform musicalPerformanceParent;
    [SerializeField]
    private Transform musicalWaitingQueueParent;
    [SerializeField]
    private Transform musicalSpawnPlace;
    
    [Header("Queue Related")]
    [SerializeField] 
    private int visibleQueueSize;
    [SerializeField] 
    private Vector3 queueDirection;
    [SerializeField]
    private float queueDistance;
    [SerializeField]
    private float walkToQueueTime;
    [SerializeField] 
    private float decreaseQueueTime;

    [Header("Database")] 
    [SerializeField] 
    private List<MusicalCharacterSO> musicalCharacters;

    private List<Vector3> queuePositions;
    private const float orchestraDelayCheckTimer = 1.0f;
    private int stageIndex;
    private int queueIndex;

    protected override void Awake()
    {
        base.Awake();
        AssessUtils.CheckRequirement(ref eventEmitter, this);
    }

    private void Start()
    {
        stageCharacters = new List<MusicalCharacterBehaviour>();
        waitingCharacters = new Queue<MusicalCharacterBehaviour>();
        queuedCharacters = new Queue<MusicalCharacter>();
        
        //Start the coroutines for the Musical Control
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(OrchestraCoroutine());

        GenerateQueueSpots();
        
        //Start the music with the game
        eventEmitter.Play();
    }

    private void GenerateQueueSpots()
    {
        queuePositions = new List<Vector3>();
        var position = musicalWaitingQueueParent.position;
        var normalizedDirection = queueDirection.normalized;
        for (var i = 0; i < visibleQueueSize; i++)
        {
            queuePositions.Add(position + normalizedDirection * (i * queueDistance));
        }
    }
    
    public int QueueMusicalCharacterSpawning(MusicalCharacter musicalCharacter)
    {
        DebugUtils.DebugLogMsg($"Queued the Spawn of {musicalCharacter}.");
        queuedCharacters.Enqueue(musicalCharacter);
        //Returns the number of characters currently waiting in the queue
        return waitingCharacters.Count;
    }
    
    private IEnumerator OrchestraCoroutine()
    {
        
        //Inner enumerator to handle playing the music
        IEnumerator Perform()
        {
            eventEmitter.Stop();
            yield return new WaitForSeconds(1.0f);
            //Set all stage character to play
            
            eventEmitter.Play();
            stageCharacters.ForEach(character => character.SetMusicParameters(eventEmitter));
            
            yield return new WaitForSeconds(performTime);
            
            stageCharacters.ForEach(character => character.ResetMusicParameters(eventEmitter));
            
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
                
                //TODO change this to move towards the queue starting place, then walking to the queue
                var musicalCharacterBehaviour = Instantiate(musicalCharacterBehaviorPrefab, musicalSpawnPlace.position, musicalSpawnPlace.rotation);
                musicalCharacterBehaviour.Initialize(musicalCharacterSo);

                void Enqueue(MusicalCharacterBehaviour mCB, MusicalCharacter mC)
                {
                    mCB.Enqueue(mC);
                    waitingCharacters.Enqueue(mCB);
                }
                
                if (queueIndex < visibleQueueSize)
                {
                    var queuePosition = queuePositions[queueIndex];
                    var walkTime = walkToQueueTime - queueIndex * decreaseQueueTime;
                    queueIndex++;

                    musicalCharacterBehaviour.WalkToTheQueue(queuePosition, walkTime, () =>
                    {
                        //TODO make the entire moving to the queue to then get properly queued.
                        Enqueue(musicalCharacterBehaviour, musicalCharacter);
                    });
                }
                else
                {
                    //Queue is too long already, then the characters are placed outside the screen
                    Enqueue(musicalCharacterBehaviour, musicalCharacter);
                }
            }
        }
    }

    public MusicalCharacterInfo GetCharacterInfo(string content)
    {
        var uidResponse = JsonUtility.FromJson<UIDResponse>(content);
        var uid = uidResponse.UID;
        
        var isTheCharacterOnTheStage = stageCharacters.Find(character => character.CompareUID(uid)) != null;
        //The character is on stage, then return it with an invalid queue position and the queue size 
        if(isTheCharacterOnTheStage) return new MusicalCharacterInfo { onStage = true, uid = uid, queuePosition = -1, queueSize = QueueSize()};
        var queuePosition = waitingCharacters.ToList().FindIndex(character => character.CompareUID(uid));
        //If the character is not on stage, then get its position in the queue and return its index with the current queue size
        return new MusicalCharacterInfo { onStage = false, uid = uid, queuePosition = queuePosition, queueSize = QueueSize() };
    }

    public int QueueSize() => waitingCharacters.Count;

    private void OnDrawGizmos()
    {
        var position = musicalWaitingQueueParent.position;
        Gizmos.DrawLine(position, position + queueDirection * queueDistance);

        var queueShowers = position;
        queueShowers.z += 2.0f;

        for (var i = 0; i < visibleQueueSize; i++)
        {
            Gizmos.color = i == queueIndex ? Color.red : Color.white;
            
            Gizmos.DrawWireSphere(queueShowers + queueDirection * i * queueDistance, 3.0f);
        }
    }
}
