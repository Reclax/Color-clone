namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstracci�n para rotar una rueda u objeto circular.
    /// </summary>
    public interface IWheelRotator
    {
        /// <summary>
        /// Ejecuta una rotaci�n dependiente del deltaTime.
        /// </summary>
        void Rotate(float deltaTime);
    }
}