using UnityEngine;

namespace ColorClone.Domain.Interfaces
{
    // Expone solo lo necesario para los estados
    public interface IPlayerContext
    {
        void PerformJump();
        void ProcessTriggerWhileAlive(Collider2D other);
        void Die();
        void Finish();
    }
}
