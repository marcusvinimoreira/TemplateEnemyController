using UnityEngine;

public class FixRotate : MonoBehaviour
{
    Quaternion rotation;

    private void Awake()
    {
        rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
