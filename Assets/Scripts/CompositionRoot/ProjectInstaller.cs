using Zenject;
using ColorClone.Domain.Interfaces;
using ColorClone.Infrastructure.Services;
using ColorClone.Infrastructure.Controllers;

namespace ColorClone.CompositionRoot
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // 1) Inyección del servicio de input
            Container.Bind<IInputService>()
                     .To<UnityInputService>()
                     .AsSingle();

            // 2) Localiza en escena el PlayerController y hazlo singleton
            Container.Bind<PlayerController>()
                     .FromComponentInHierarchy()
                     .AsSingle();
        }
    }
}