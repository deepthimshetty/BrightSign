using Acr.UserDialogs;
using BrightSign.Core.Utility.Interface;
using BrightSign.iOS.Utility.Interface;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;
using UIKit;

namespace BrightSign.iOS
{
    public class Setup : MvxIosSetup
    {
        private MvxApplicationDelegate _applicationDelegate;
        UIWindow _window;

        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            _applicationDelegate = applicationDelegate;
            _window = window;
        }

        public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeIoC()
        {
            base.InitializeIoC();
            Mvx.RegisterSingleton<IDialogService>(() => new DialogService());
            Mvx.RegisterSingleton<IUserPreferences>(() => new iOSUserPreferences());
            Mvx.RegisterSingleton<ISQLite>(() => new SQLiteiOS());
            Mvx.RegisterSingleton<ICustomAlert>(() => new CustomAlert());
            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
        }

    }
}
