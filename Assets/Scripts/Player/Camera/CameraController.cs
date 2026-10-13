
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 5f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player == null || Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 mouseScreen = new Vector3(
            mousePosition.x,
            mousePosition.y,
            -transform.position.z
        );

        Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
        Vector3 midpoint = Vector3.Lerp(player.position, mouseWorld, 1f / 5f);

        midpoint.z = transform.position.z;

        transform.position = Vector3.Lerp(
            transform.position,
            midpoint,
            1f - Mathf.Exp(-smoothSpeed * Time.deltaTime)
        );
    }
}