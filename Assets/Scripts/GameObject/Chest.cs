using FMODUnity;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int silverCoins = 1;
    [SerializeField] private int goldenCoins = 1;
    [SerializeField] private bool diamond = true;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Key>(out Key key))
        {
            Open();
            Destroy(collision.gameObject);
        }
    }

    private void Open()
    {
        CollectiblesEvents.ChestOpened(silverCoins, goldenCoins, diamond);
        
        RuntimeManager.PlayOneShot("event:/CHEST", transform.position);
    }
}
