using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 maxOffset;
    [SerializeField] private float dPlayer = 2;
    [SerializeField] private float dCamera = 8;

    private Camera mainCamera;
    
    private void Start(){
        mainCamera = Camera.main;
    }
    
    private void Update()
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

        transform.position = new Vector3(playerPos.x + cameraOffset.x, playerPos.y + cameraOffset.y, transform.position.z);
    }
}
