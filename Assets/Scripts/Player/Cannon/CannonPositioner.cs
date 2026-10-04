using UnityEngine;
using UnityEngine.InputSystem;

public class CannonPositioner : MonoBehaviour
{
    private Vector2 _mousePosition;
    [SerializeField] private float angleOffset = -90;

    // Update is called once per frame
    void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.rotation = LookAtMouse(_mousePosition);
    }
    
    private Quaternion LookAtMouse(Vector2 mousePosition)
    {
        Vector2 delta = mousePosition - (Vector2)transform.position;
        float angle = Mathf.Atan2(delta.y, delta.x);
        return Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + angleOffset);
    }
}
