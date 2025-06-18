using ColorClone.Domain.Interfaces;

namespace ColorClone.Application.UseCases
{
    /// <summary>
    /// Use case that orchestrates player movement.
    /// </summary>
    public class MovePlayerUseCase
    {
        private readonly IPlayer _player;

        public MovePlayerUseCase(IPlayer player)
        {
            _player = player;
        }

        /// <summary>
        /// Execute movement logic on the player.
        /// </summary>
        public void Execute(float horizontal, float vertical)
        {
            _player.UpdateMovement(horizontal, vertical);
        }
    }
}