using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string JumpButton = "Jump";
    private const int LeftMouseButton = 0;
    private const KeyCode VampirismSkillKey = KeyCode.E;
    
    public Vector2 GetMoveInput() =>
        new Vector2(Input.GetAxis(HorizontalAxis), 0f);

    public bool GetJumpInput() =>
        Input.GetButtonDown(JumpButton);

    public bool GetAttackInput() =>
        Input.GetMouseButtonDown(LeftMouseButton);
    
    public bool GetVampirismSkillInput() => 
        Input.GetKeyDown(VampirismSkillKey);
}
