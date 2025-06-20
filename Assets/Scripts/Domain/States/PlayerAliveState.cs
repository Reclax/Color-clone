using UnityEngine;
using ColorClone.Application.UseCases;
using ColorClone.Domain.Interfaces;

namespace ColorClone.Domain.States
{
    public class PlayerAliveState : IPlayerState
    {
        public void Enter(IPlayerContext player)
        {
            // Lógica al entrar en estado vivo
        }

        public void Exit(IPlayerContext player)
        {
            // Lógica al salir del estado vivo
        }

        public void Jump(IPlayerContext player) => player.PerformJump();

        public void HandleTrigger(IPlayerContext player, Collider2D other) => player.ProcessTriggerWhileAlive(other);
    }
}
