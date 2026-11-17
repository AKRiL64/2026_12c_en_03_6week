using UnityEngine;

public class TutorialCrab : FierceTooth
{
    [SerializeField] private FierceTooth fierceToothToDisable;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D for TutorialCrab");
        if (other.CompareTag("Lever"))
        {
            Debug.Log("its a lever!");
            fierceToothToDisable.enabled = false;
        }
        base.OnTriggerEnter2D(other);
    }
}
