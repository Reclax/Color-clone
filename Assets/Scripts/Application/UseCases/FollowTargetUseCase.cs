using UnityEngine;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    /// <summary>
    /// Caso de uso que implementa la l�gica de seguir
    /// un objetivo s�lo cuando �ste sube en el eje Y.
    /// </summary>
    public class FollowTargetUseCase : IFollowTarget
    {
        public Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition)
        {
            // S�lo subimos la posici�n Y si el objetivo est� por encima.
            if (targetPosition.y > currentPosition.y)
            {
                return new Vector3(
                    currentPosition.x,
                    targetPosition.y,
                    currentPosition.z
                );
            }

            // Si no, mantenemos la posici�n actual.
            return currentPosition;
        }
    }
}