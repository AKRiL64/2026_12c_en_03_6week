using UnityEngine;

public class SwitchPlatform: SwitchObject
{
    [SerializeField] private GameObject onVisual;
    [SerializeField] private GameObject offVisual;
    [SerializeField] private Collider2D cl2D;
    protected override void SwitchOn()
    {
        cl2D.enabled = true;
        onVisual.SetActive(true);
        offVisual.SetActive(false);
    }

    protected override void SwitchOff()
    {
        cl2D.enabled = false;
        onVisual.SetActive(false);
        offVisual.SetActive(true);
    }
}
