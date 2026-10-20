using System.Collections;
using FMODUnity;
using UnityEngine;

public class SwitchCannon: SwitchObject
{
    [SerializeField] private Projectile cannonBallPrefab;
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private float shootForce;
    [SerializeField] private float reloadTimeSeconds;
    [SerializeField] private float startDelaySeconds = 0;
    
    private bool isActive;
    private bool isReloading = false;

    private void Update()
    {
        if (!isActive) return;
        if (isReloading) return;
        Shoot();
        StartCoroutine(Reload());
    }

    private void Shoot()
    {
        Projectile cannonBall = Instantiate(
            cannonBallPrefab,
            shootPoint.transform.position,
            shootPoint.transform.rotation
        );

        Vector2 direction = shootPoint.transform.right;
        cannonBall.Launch(shootForce, direction);
        
        RuntimeManager.PlayOneShot("event:/CANNON_SPIT", transform.position);
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTimeSeconds);
        isReloading = false;
    }

    private IEnumerator initialDelay()
    {
        yield return new WaitForSeconds(startDelaySeconds);
        isActive = true;
    }
    
    protected override void SwitchOn()
    {
        StartCoroutine(initialDelay());
    }

    protected override void SwitchOff()
    {
        isActive = false;
    }
}
