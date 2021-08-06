using Android.Content;
using BrightSign.Core.Utility.Interface;
using BrightSign.Droid.Utility.Interface;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Email;
using MvvmCross.Plugins.Messenger;

namespace BrightSign.Droid
{
    public class Setup : MvxAppCompatSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
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
            Mvx.RegisterSingleton<IMvxMessenger>(() => new MvxMessengerHub());
            Mvx.RegisterSingleton<IUserPreferences>(() => new AndroidUserPreferences());
            Mvx.RegisterSingleton<ISQLite>(() => new SQLiteAndroid());
            Mvx.RegisterSingleton<ICustomAlert>(() => new CustomAlert());
            Mvx.RegisterSingleton<IFileManager>(() => new FileManager());
            Mvx.RegisterSingleton<StoragePath>(() => new StoragePathHelper());
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            MvvmCross.Plugins.Email.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();

        }
    }
}
