using UnityEngine;

public class CameControl : MonoBehaviour
{
    [SerializeField]
    Transform focus;

    const float moveSpeed = 1f;
    const float rotateSpeed = 10f;
    KeyCode nearKey = KeyCode.W;
    KeyCode farKey = KeyCode.S;
    KeyCode leftKey = KeyCode.A;
    KeyCode rightKey = KeyCode.D;
    KeyCode rotateLKey = KeyCode.Q;
    KeyCode rotateRKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKey(nearKey))
        {
            transform.position -= moveSpeed * Time.deltaTime * (transform.position - focus.position).normalized;
        }
        else if (Input.GetKey(farKey))
        {
            transform.position += moveSpeed * Time.deltaTime * (transform.position - focus.position).normalized;
        }
        else if (Input.GetKey(leftKey))
        {
            transform.position -= moveSpeed * Time.deltaTime * transform.right;
        }
        else if (Input.GetKey(rightKey))
        {
            transform.position += moveSpeed * Time.deltaTime * transform.right;
        }
        else if (Input.GetKey(rotateLKey))
        {
            transform.RotateAround(focus.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(rotateRKey))
        {
            transform.RotateAround(focus.position, Vector3.up, -rotateSpeed * Time.deltaTime);
        }
    }
}
