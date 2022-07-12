

    using UnityEngine;
    using Zenject;

    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private WindowsManager _windowsManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BackButton>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GlobalSettings.Screen>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WindowsManager>().FromInstance(_windowsManager).AsSingle();
        }
    }

