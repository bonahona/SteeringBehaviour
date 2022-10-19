namespace Fyrvall.SteeringBehaviour
{
    public interface ISteeringBehaviour
    {
        float GetPriority();
        SteeringData GetSteeringData();
        void DebugDraw();

        void UpdateBehaviour();
    }
}