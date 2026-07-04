using UnityEngine;

public class VampirismVisualArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public void Initialize(float radius) =>
        _spriteRenderer.enabled = false;
    
    public void Show() =>
        _spriteRenderer.enabled = true;
    

    public void Hide() =>
        _spriteRenderer.enabled = false;
}
