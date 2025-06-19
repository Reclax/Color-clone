using UnityEngine;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstracci�n para calcular la posici�n de un seguidor
    /// bas�ndose en la posici�n de un objetivo.
    /// </summary>
    public interface IFollowTarget
    {
        /// <summary>
        /// Dada la posici�n actual del seguidor y la posici�n del objetivo,
        /// devuelve la nueva posici�n que debe adoptar el seguidor.
        /// </summary>
        Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 targetPosition);
    }
}