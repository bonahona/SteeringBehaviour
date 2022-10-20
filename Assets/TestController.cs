using UnityEngine;

public class TestController : MonoBehaviour
{
    public float MovementSpeed = 10f;

    private Vector3 CurrentMovementSpeed;
    private float TargetTimeScale = 1f;

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

        if (Input.GetKey(KeyCode.LeftShift)) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = TargetTimeScale;
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(TargetTimeScale == 1f) {
                TargetTimeScale = 0f;
            } else {
                TargetTimeScale = 1f;
            }
        }

        CurrentMovementSpeed = Vector3.Lerp(CurrentMovementSpeed, movementDirection * MovementSpeed, 0.1f);
        transform.Translate(CurrentMovementSpeed * Time.deltaTime);

    }
}
