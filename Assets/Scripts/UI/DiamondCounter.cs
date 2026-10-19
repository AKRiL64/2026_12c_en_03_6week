using TMPro;
using UnityEngine;

public class DiamondCounter : MonoBehaviour
{
    public static DiamondCounter Instance { get; private set; };
    
    private TextMeshProUGUI _counter;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if(_counter == null)
            _counter = GetComponent<TextMeshProUGUI>();
    }
    
    public void SetCounter(int value)
    {
        _counter.text = value.ToString();
    }
}
