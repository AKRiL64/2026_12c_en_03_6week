using UnityEngine;
using UnityEngine.InputSystem;

public class CannonPositioner : MonoBehaviour
{
    private Vector2 _mousePosition;

    [SerializeField] private float angleOffset = -90;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Vector2 delta = _mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        bool facingLeft = Mathf.Abs(angle) > 90;

        spriteRenderer.flipY = !facingLeft;

        transform.rotation = Quaternion.Euler(
            0, 0, angle + angleOffset
        );
    }
}