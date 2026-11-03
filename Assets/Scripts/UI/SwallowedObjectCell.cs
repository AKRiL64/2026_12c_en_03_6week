using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SwallowedObjectCell : MonoBehaviour
{
    public static SwallowedObjectCell Instance;
    private Image _image;
    private readonly Color _transparentColor = new Color(0, 0, 0, 0);

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if (_image == null)
            _image = GetComponent<Image>();
        _image.enabled = false;
    }

    public void ChangeSprite([CanBeNull] Sprite sprite)
    {
        _image.enabled = true;
        if (sprite == null)
            _image.color = _transparentColor;
        else
        {
            _image.sprite = sprite;
            _image.color = Color.white;
        }
    }
}
