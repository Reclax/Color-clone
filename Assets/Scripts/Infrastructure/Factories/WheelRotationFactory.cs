using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;
using UnityEngine;

namespace ColorClone.Infrastructure.Factories
{
    public class WheelRotationFactory : IWheelRotationFactory
    {
        public IWheelRotator CreateWheelRotation(Transform wheelTransform, float speed)
        {
            return new WheelRotationUseCase(wheelTransform, speed);
        }
    }
}
