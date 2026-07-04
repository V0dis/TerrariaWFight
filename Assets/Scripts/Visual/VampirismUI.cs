using UnityEngine;
using UnityEngine.UI;

public class VampirismUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Vampirism _vampirism;
    
    private void Start() => 
        _slider.value = 1;
    
    private void Update()
    { 
        if (_vampirism.IsActive)
            _slider.value = 1 - (_vampirism.CurrentTime / _vampirism.SkillDuration);
        else if (_vampirism.IsDelay)
            _slider.value = _vampirism.CurrentTime / _vampirism.SkillCooldown;
        else
            _slider.value = 1;
    }
}
