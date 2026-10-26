using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private int coinsNeeded = 5; 
    [SerializeField] private float growSpeed = 0.01f;
    private Image _fillImage;
    private float _endPoint = 0f;
    
    public static CoinBar Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if(_fillImage == null)
            _fillImage = GetComponent<Image>();
    }

    public void SetNeeded(int coins)
    {
        coinsNeeded = coins;
    }
    
    public void UpdateBar(int coinNumber)
    {
        _endPoint = Mathf.Clamp(coinNumber / (float)coinsNeeded, 0, 1f);
        _fillImage.fillAmount = coinNumber / (float)coinsNeeded;
        StopAllCoroutines();
        StartCoroutine(FillBar());
    }

    IEnumerator FillBar()
    {
        for (float i = _fillImage.fillAmount; i < _endPoint; i += growSpeed)
        {
            _fillImage.fillAmount = i;
            yield return null;
        }
    }
}
