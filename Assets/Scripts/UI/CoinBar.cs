using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    [SerializeField] private int coinsNeeded = 5; // change when the money-collecting is ready
    [SerializeField] private float growSpeed = 0.01f;
    private Image _fillImage;
    private float _endPoint = 0f;
    
    private void Awake()
    {
        if(_fillImage == null)
            _fillImage = GetComponent<Image>();
    }
    
    public void UpdateBar(int coinNumber)
    {
        _endPoint = Mathf.Clamp(coinNumber / (float)coinsNeeded, 0, 1f);
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
