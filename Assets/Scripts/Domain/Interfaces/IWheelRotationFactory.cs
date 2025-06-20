namespace ColorClone.Domain.Interfaces
{
    public interface IWheelRotationFactory
    {
        IWheelRotator CreateWheelRotation(UnityEngine.Transform wheelTransform, float speed);
    }
}
