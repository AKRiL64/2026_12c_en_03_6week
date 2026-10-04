using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FierceTooth : Projectile
{
    private Quaternion _upwardsRotation;
    [SerializeField] private float rotationSpeed = 1f;

    protected override void Awake()
    {
        base.Awake();
        _upwardsRotation = transform.rotation;
    }
    
    protected override void Stop()
    {
        base.Stop();
        StartCoroutine(RotateUpwards());
    }

    private IEnumerator RotateUpwards()
    {
        while(Rb.linearVelocity != Vector2.zero)
            yield return null;
        
        for (float t = 0; t < 1; t += Time.deltaTime * rotationSpeed)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _upwardsRotation, t);
            yield return null;
        }
    }
}
