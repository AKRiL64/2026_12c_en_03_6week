using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions input;
    
    public Vector2 InputVector {
        get; private set; 
    }

    public bool JumpPressed
    {
        get; private set; 
        
    }
    
    private void Awake() 
    {
        input = new InputSystem_Actions();
    }
    
    private void OnEnable() 
    { 
        input.Enable();
    }
    
    private void OnDisable() 
    {
        input.Disable();
    }

    private void Update()
    {
        InputVector = input.Player.Move.ReadValue<Vector2>();

        JumpPressed = input.Player.Jump.WasPressedThisFrame();
    }   
    
}
