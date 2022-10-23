using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    public Transform Target;
    public float Angle = 60;
    public float Distance = 20;

    [HideInInspector]
    public Vector3 DistanceVector;

    void Start()
    {
        transform.rotation = Quaternion.Euler(Angle, 0, 0);
        DistanceVector = transform.rotation * -Vector3.forward * Distance;
    }

    void Update()
    {
        if(Target == null) {
            return;
        }

        transform.position = Target.transform.position + DistanceVector;
    }
}
