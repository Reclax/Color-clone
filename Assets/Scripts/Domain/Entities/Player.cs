using ColorClone.Domain.Interfaces;

namespace ColorClone.Domain.Entities
{
    /// <summary>
    /// Concrete implementation of IPlayer that delegates to a movement strategy.
    /// </summary>
    public class Player : IPlayer
    {
        private readonly IMovementStrategy _movementStrategy;

        public Player(IMovementStrategy movementStrategy)
        {
            _movementStrategy = movementStrategy;
        }

        public void UpdateMovement(float horizontal, float vertical)
        {
            _movementStrategy.Move(horizontal, vertical);
        }
    }
}