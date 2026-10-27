using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public static HpBar Instance { get; private set; }
    private Image _fillImage;
    private float _currentHp = 1f;
    [SerializeField] private float growSpeed = 0.001f;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if (_fillImage == null)
            _fillImage = GetComponent<Image>();
    }

    public void ChangeHp(float current, float maximum)
    {
        _currentHp = current;
        StopAllCoroutines();
        StartCoroutine(ReduceHp(maximum));
    }

    IEnumerator ReduceHp(float maximum)
    {
        float value = 0;
        if (_currentHp < _fillImage.fillAmount * maximum)
        {
            for (value = _fillImage.fillAmount; value > _currentHp / maximum; value -= growSpeed)
            {
                _fillImage.fillAmount = value;
                yield return null;
            }
        }
        else
        {
            for (value = _fillImage.fillAmount; value < _currentHp / maximum; value += growSpeed)
            {
                _fillImage.fillAmount = value;
                yield return null;
            }
        }
    }
}
