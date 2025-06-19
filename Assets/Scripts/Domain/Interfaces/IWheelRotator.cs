namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstracción para rotar una rueda u objeto circular.
    /// </summary>
    public interface IWheelRotator
    {
        /// <summary>
        /// Ejecuta una rotación dependiente del deltaTime.
        /// </summary>
        void Rotate(float deltaTime);
    }
}