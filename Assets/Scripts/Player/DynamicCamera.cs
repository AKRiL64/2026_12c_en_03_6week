using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 maxOffset;
    [SerializeField] private float dPlayer = 2;
    [SerializeField] private float dCamera = 8;

    [SerializeField] private float smoothSpeed = 2;
    private float shakeTimeRemaining;
    private float shakeMagnitude;
    private Vector3 shakeOffset;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!player || !mainCamera || Mouse.current == null) return;

        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Mouse.current.position.ReadValue();

        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector2 distance = mousePos - playerPos;
        Vector2 cameraOffset = distance * dPlayer / (dCamera + dPlayer);

        cameraOffset = new Vector2(
            Mathf.Clamp(cameraOffset.x, -maxOffset.x, maxOffset.x),
            Mathf.Clamp(cameraOffset.y, -maxOffset.y, maxOffset.y)
        );

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(playerPos.x + cameraOffset.x,
                        playerPos.y + cameraOffset.y,
                        transform.position.z),
            smoothSpeed * Time.deltaTime
        );

        if (shakeTimeRemaining > 0)
        {
            shakeOffset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        transform.position += shakeOffset;

    }

    public void ShakeCamera(float D, float M)
    {
        shakeTimeRemaining = D;
        shakeMagnitude = M;
    }
}
