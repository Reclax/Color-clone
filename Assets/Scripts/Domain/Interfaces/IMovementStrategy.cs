namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Strategy for moving an entity based on input.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Moves according to horizontal and vertical values.
        /// </summary>
        void Move(float horizontal, float vertical);
    }
}