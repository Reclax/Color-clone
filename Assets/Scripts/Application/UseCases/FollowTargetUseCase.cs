using UnityEngine;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    /// <summary>
    /// Caso de uso que implementa la lógica de seguir
    /// un objetivo sólo cuando éste sube en el eje Y.
    /// </summary>
    public class FollowTargetUseCase : IFollowTarget
    {
        public Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition)
        {
            // Sólo subimos la posición Y si el objetivo está por encima.
            if (targetPosition.y > currentPosition.y)
            {
                return new Vector3(
                    currentPosition.x,
                    targetPosition.y,
                    currentPosition.z
                );
            }

            // Si no, mantenemos la posición actual.
            return currentPosition;
        }
    }
}