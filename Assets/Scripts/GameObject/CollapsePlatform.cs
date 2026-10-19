using System.Collections;
using UnityEngine;

public class CollapsePlatform: MonoBehaviour
{
    [SerializeField] private GameObject onVisual;
    [SerializeField] private GameObject offVisual;
    [SerializeField] private Collider2D cl2D;
    [SerializeField] private float collapseTime = 2f;
    [SerializeField] private float appearTime = 3f;
    
    private bool _falling = false;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_falling) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CollapseCoroutine());
        }
    }

    private IEnumerator CollapseCoroutine()
    {
        _falling = true;
        yield return new WaitForSeconds(appearTime);
        Disappear();
        
        yield return new WaitForSeconds(appearTime);
        _falling = false;
        Appear();
        
    }
    
    protected void Appear()
    {
        cl2D.enabled = true;
        onVisual.SetActive(true);
        if (offVisual) offVisual.SetActive(false);
    }

    protected void Disappear()
    {
        cl2D.enabled = false;
        onVisual.SetActive(false);
        if (offVisual) offVisual.SetActive(true);
    }
}
