using UnityEngine;
using Zenject;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;

namespace ColorClone.Infrastructure.Controllers
{
    [RequireComponent(typeof(Transform))]
    public class WheelRotationController : MonoBehaviour
    {
        [Header("Configuración de rotación")]
        [SerializeField]
        private float rotationSpeed = 100f;

        IWheelRotator _rotator;

        [Inject]
        public void Construct()
        {
            // Creamos el caso de uso pasándole el transform de este GameObject
            _rotator = new WheelRotationUseCase(transform, rotationSpeed);
        }

        void Update()
        {
            // Delegamos la rotación al caso de uso
            _rotator.Rotate(Time.deltaTime);
        }
    }
}