using UnityEngine;
using ColorClone.Domain.Interfaces;
using ColorClone.Application.UseCases;

namespace ColorClone.Infrastructure.Controllers
{
    /// <summary>
    /// MonoBehaviour que �pegla� Unity con el caso de uso de seguimiento.
    /// </summary>
    [RequireComponent(typeof(Transform))]
    public class FollowPlayerController : MonoBehaviour
    {
        [Header("Target to follow")]
        [SerializeField]
        private Transform playerTransform;

        private IFollowTarget _followUseCase;

        void Awake()
        {
            // Instanciamos el caso de uso
            _followUseCase = new FollowTargetUseCase();
        }

        private void SetupButton(UnityEngine.UI.Button button, UnityEngine.Events.UnityAction action)
        {
            if (button == null) return;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        void Update()
        {
            // Calculamos la siguiente posici�n y la aplicamos
            transform.position = _followUseCase.CalculateNextPosition(
                transform.position,
                playerTransform.position
            );
        }
    }
}