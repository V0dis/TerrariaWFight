using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flasher : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private Color _healColor = Color.green;
    [SerializeField] private float _flashDuration = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private bool _isFlashing;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer != null)
            _originalColor = _spriteRenderer.color;
    }

    public void DamageFlash()
    {
        if (_isFlashing)
            return;

        StartCoroutine(FlashRoutine(_damageColor));
    }

    public void HealFlash()
    {
        if (_isFlashing)
            return;

        StartCoroutine(FlashRoutine(_healColor));
    }

    private IEnumerator FlashRoutine(Color color)
    {
        _isFlashing = true;
        _spriteRenderer.color = color;

        yield return new WaitForSeconds(_flashDuration);

        _spriteRenderer.color = _originalColor;
        _isFlashing = false;
    }
}
