using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothFollow = 30f;
    public Vector3 offset;


    private void LateUpdate()
    {
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothFollow * Time.deltaTime);

        transform.position = smoothedPos;
        
    }
}
