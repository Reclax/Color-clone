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

        private IWheelRotationFactory _wheelRotationFactory;
        private IWheelRotator _rotator;

        [Inject]
        public void Construct(IWheelRotationFactory wheelRotationFactory)
        {
            _wheelRotationFactory = wheelRotationFactory;
            // Usar la fábrica para crear el caso de uso de rotación
            _rotator = _wheelRotationFactory.CreateWheelRotation(transform, rotationSpeed);
        }

        private void SetupButton(UnityEngine.UI.Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        void Update()
        {
            // Delegamos la rotación al caso de uso
            _rotator.Rotate(Time.deltaTime);
        }
    }
}