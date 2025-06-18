namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstraction of a player entity.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Updates player movement given input axes.
        /// </summary>
        void UpdateMovement(float horizontal, float vertical);
    }
}