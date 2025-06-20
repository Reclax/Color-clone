using UnityEngine;
using ColorClone.Application.UseCases;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Domain.States
{
    public class PlayerDeadState : IPlayerState
    {
        public void Enter(IPlayerContext player)
        {
            player.Die();
        }

        public void Exit(IPlayerContext player)
        {
            // Lógica al salir del estado muerto
        }

        public void Jump(IPlayerContext player)
        {
            // No puede saltar estando muerto
        }

        public void HandleTrigger(IPlayerContext player, Collider2D other)
        {
            // No procesa triggers estando muerto
        }
    }
}
