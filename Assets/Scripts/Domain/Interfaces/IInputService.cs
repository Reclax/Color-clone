namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstracción para leer el input (métodos que necesites, por ejemplo salto).
    /// </summary>
    public interface IInputService
    {
        bool GetJumpDown();
    }
}