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

            // Clean Architecture bindings para PartiesPresenter
            Container.Bind<ISaveGameRepository>()
                     .To<ColorClone.Infrastructure.Adapters.GameManagerSaveGameAdapter>()
                     .AsSingle();
            Container.Bind<ISceneNavigationService>()
                     .To<UnitySceneNavigationService>()
                     .AsSingle()
                     .WithArguments("StartScreen"); // Puedes parametrizar el nombre de la escena si lo necesitas
            Container.Bind<ColorClone.Application.UseCases.SlotSelectionUseCase>()
                     .AsTransient();
            Container.Bind<ColorClone.Presentation.Unity.MenuScreenPresenter>()
                     .AsTransient();
            // Bind para flujo de cierre de juego
            Container.Bind<ColorClone.Domain.Interfaces.IGameFlowService>()
                     .To<ColorClone.Application.UseCases.GameFlowUseCase>()
                     .AsSingle();
        }
    }
}