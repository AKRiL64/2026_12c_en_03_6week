using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static int CheckpointCounter { get; private set; } = 0;
    [SerializeField] private int checkpointSequentialNumber = 0;
    [SerializeField] private Transform playerSpawn;

    void Awake()
    {
        if(playerSpawn == null)
            playerSpawn = transform;
        CheckpointManager.AddCheckpoint(checkpointSequentialNumber, playerSpawn.position);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (checkpointSequentialNumber > CheckpointCounter || 
            (checkpointSequentialNumber == 0 && CheckpointCounter == 0))
        {
            CheckpointCounter = checkpointSequentialNumber;
            SaveManager.SetPlayerInventory(other.gameObject.GetComponent<PlayerInventory>());
            SaveManager.UpdateObjectManager(ObjectManager.Instance);
            SaveManager.Save(CheckpointCounter);
        }
    }

    public static void ResetCheckpoint()
    {
        CheckpointCounter = 0;
    }
}
