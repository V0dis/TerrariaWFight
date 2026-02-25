using UnityEngine;

public class Rotator : MonoBehaviour
{
    private const float RightRotation = 0f;
    private const float LeftRotation = 180f;

    private Quaternion _rightQuaternion = Quaternion.Euler(0f, RightRotation, 0f);
    private Quaternion _leftQuaternion = Quaternion.Euler(0f, LeftRotation, 0f);

    public void SetDirection(float direction)
    {
        if (direction == 0f)
            return;

        transform.rotation = direction > 0f ? _rightQuaternion : _leftQuaternion;
    }
}
