using System.Collections;
using UnityEngine;

public class PowderBarrel: MonoBehaviour
{
    [SerializeField] Gunpowder childGunpowder;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float reloadTime = 2f;

    private Rigidbody2D childRigidbody;
    private void Start()
    {
        childGunpowder.parentBarrel = this;
        childRigidbody = childGunpowder.GetComponent<Rigidbody2D>();
    }
    
    public void RespawnProjectile()
    {
        StartCoroutine(StartReload());
    }

    private IEnumerator StartReload()
    {
        childGunpowder.gameObject.SetActive(false);
        childRigidbody.gravityScale = 0f;
        childGunpowder.transform.position = spawnPoint.transform.position;
        yield return new WaitForSeconds(reloadTime);
        childGunpowder.gameObject.SetActive(true);
        
    }
}
