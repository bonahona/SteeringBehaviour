using Fyrvall.SteeringBehaviour;
using UnityEngine;

[RequireComponent(typeof(SteeringAgent))]
public class TestController : MonoBehaviour
{
    private float TargetTimeScale = 1f;

    private SteeringAgent SteeringAgent;

    private void Start()
    {
        SteeringAgent = GetComponent<SteeringAgent>();
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

        SteeringAgent.TargetMovementSpeed = movementDirection;
    }
}
