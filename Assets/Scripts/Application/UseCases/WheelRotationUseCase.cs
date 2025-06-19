using UnityEngine;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    /// <summary>
    /// Caso de uso que encapsula la lógica de rotación de una rueda.
    /// </summary>
    public class WheelRotationUseCase : IWheelRotator
    {
        readonly Transform _transform;
        readonly float _speed;

        public WheelRotationUseCase(Transform transform, float speed)
        {
            _transform = transform;
            _speed = speed;
        }

        public void Rotate(float deltaTime)
        {
            // Aplica la rotación en Z con independencia del framerate
            _transform.Rotate(0f, 0f, _speed * deltaTime);
        }
    }
}