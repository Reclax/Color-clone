namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Abstracci�n para leer el input (m�todos que necesites, por ejemplo salto).
    /// </summary>
    public interface IInputService
    {
        bool GetJumpDown();
        bool GetPauseDown();
    }
}