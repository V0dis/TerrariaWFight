using UnityEngine;

public class UserInput : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string JumpButton = "Jump";

    private Vector2 _inputCache;

    private void Awake()
    {
        _inputCache = Vector2.zero;
    }

    public Vector2 GetMoveInput()
    {
        _inputCache.x = Input.GetAxis(HorizontalAxis);
        _inputCache.y = 0f;
        
        return _inputCache;
    }

    public bool GetJumpInput() =>
        Input.GetButtonDown(JumpButton);
    

    public bool GetMouseClick() =>
        Input.GetMouseButtonDown(0);
    
}
