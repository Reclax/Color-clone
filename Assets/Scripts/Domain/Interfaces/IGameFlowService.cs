namespace ColorClone.Domain.Interfaces
{
    /// <summary>
    /// Servicio encargado de gestionar el flujo de la aplicaci�n,
    /// como cerrar el juego o reiniciar niveles.
    /// </summary>
    public interface IGameFlowService
    {
        /// <summary>
        /// Cierra la aplicaci�n o detiene el modo Play en el Editor.
        /// </summary>
        void QuitApplication();
    }
}