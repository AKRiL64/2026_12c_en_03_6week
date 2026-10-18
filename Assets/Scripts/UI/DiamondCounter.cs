using TMPro;
using UnityEngine;

public class DiamondCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;

    private void Awake()
    {
        if(_counter == null)
            _counter = GetComponent<TextMeshProUGUI>();
    }
    
    public void SetCounter(int value)
    {
        _counter.text = value.ToString();
    }
}
