using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    [SerializeField] float rotatingSpeed = 1f;
    [SerializeField] float radius = 1f;

    Rigidbody rigidBody;

    Vector3 rotationPivotPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        GetRotationPivotPoint();
    }

    private void GetRotationPivotPoint()
    {
        Vector3 normal = new Vector3(1, -1, 0).normalized;
        rotationPivotPoint = transform.position + normal * radius;
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.transform.RotateAround(rotationPivotPoint, Vector3.forward, rotatingSpeed * Time.deltaTime);
    }
}
