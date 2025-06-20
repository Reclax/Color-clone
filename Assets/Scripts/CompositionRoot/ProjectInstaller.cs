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
            // 1) Inyecci�n del servicio de input
            Container.Bind<IInputService>()
                     .To<UnityInputService>()
                     .AsSingle();

            // 2) Localiza en escena el PlayerController y hazlo singleton
            Container.Bind<PlayerController>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            // 3) Registrar las fábricas para el patrón Factory
            Container.Bind<IPlayerFactory>()
                     .To<ColorClone.Infrastructure.Factories.PlayerFactory>()
                     .AsSingle();
            Container.Bind<IWheelRotationFactory>()
                     .To<ColorClone.Infrastructure.Factories.WheelRotationFactory>()
                     .AsSingle();
        }
    }
}