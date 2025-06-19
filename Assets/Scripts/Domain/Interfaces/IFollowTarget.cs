using UnityEngine;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstracción para calcular la posición de un seguidor
    /// basándose en la posición de un objetivo.
    /// </summary>
    public interface IFollowTarget
    {
        /// <summary>
        /// Dada la posición actual del seguidor y la posición del objetivo,
        /// devuelve la nueva posición que debe adoptar el seguidor.
        /// </summary>
        Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition);
    }
}