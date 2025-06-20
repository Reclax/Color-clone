namespace ColorClone.Domain.Interfaces
{
    public interface IPlayerState
    {
        void Enter(IPlayerContext player);
        void Exit(IPlayerContext player);
        void Jump(IPlayerContext player);
        void HandleTrigger(IPlayerContext player, UnityEngine.Collider2D other);
    }
}
