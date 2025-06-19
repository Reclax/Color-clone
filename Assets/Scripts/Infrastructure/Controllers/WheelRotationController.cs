using UnityEngine;
using Zenject;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;

namespace ColorClone.Infrastructure.Controllers
{
    [RequireComponent(typeof(Transform))]
    public class WheelRotationController : MonoBehaviour
    {
        [Header("Configuraci�n de rotaci�n")]
        [SerializeField]
        private float rotationSpeed = 100f;

        IWheelRotator _rotator;

        [Inject]
        public void Construct()
        {
            // Creamos el caso de uso pas�ndole el transform de este GameObject
            _rotator = new WheelRotationUseCase(transform, rotationSpeed);
        }

        void Update()
        {
            // Delegamos la rotaci�n al caso de uso
            _rotator.Rotate(Time.deltaTime);
        }
    }
}