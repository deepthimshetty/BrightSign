using MvvmCross.Platform.IoC;

namespace BrightSign.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            //RegisterNavigationServiceAppStart<ViewModels.MainViewModel>();
            RegisterAppStart(new AppStart());
        }
    }
}
