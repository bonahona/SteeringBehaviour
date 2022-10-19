using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestController : MonoBehaviour
{
    public float MovementSpeed = 10f;

    private Vector3 CurrentMovementSpeed;
    private Rigidbody Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();    
    }

    private void Update()
    {
        var movementDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            movementDirection.z = 1f;
        } else if (Input.GetKey(KeyCode.S)) {
            movementDirection.z = -1f;
        }

        if (Input.GetKey(KeyCode.A)) {
            movementDirection.x = -1f;
        } else if (Input.GetKey(KeyCode.D)) {
            movementDirection.x = 1f;
        }
        movementDirection.Normalize();

        CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
        Rigidbody.MovePosition(transform.position + CurrentMovementSpeed * Time.fixedDeltaTime);

    }
}
