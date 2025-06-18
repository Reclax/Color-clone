using System;
using UnityEngine;

namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// L�gica de dominio del jugador: salto, trigger, cambio de color y notificaciones de estado.
    /// </summary>
    public interface IPlayerInteractor
    {
        /// <summary>
        /// Se dispara cuando el jugador colisiona con un color incorrecto.
        /// </summary>
        event Action OnDie;

        /// <summary>
        /// Se dispara cuando el jugador alcanza la l�nea de meta.
        /// </summary>
        event Action OnFinish;

        /// <summary>
        /// Ejecuta el salto.
        /// </summary>
        void Jump();

        /// <summary>
        /// Procesa una colisi�n/trigger con un Collider2D.
        /// </summary>
        void HandleTrigger(Collider2D other);

        /// <summary>
        /// Cambia el color del jugador de forma aleatoria.
        /// </summary>
        void ChangeColor();
    }
}