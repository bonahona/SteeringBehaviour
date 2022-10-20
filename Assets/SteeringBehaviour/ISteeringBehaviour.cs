namespace Fyrvall.SteeringBehaviour
{
    public interface ISteeringBehaviour
    {
        SteeringData GetSteeringData();
        void UpdateBehaviour();
    }
}