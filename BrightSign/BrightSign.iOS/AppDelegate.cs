using System.Diagnostics;
using System.Threading.Tasks;
using BrightSign.Core.Utility;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using ObjCRuntime;
using UIKit;

namespace BrightSign.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var setup = new Setup(this, Window);
            setup.Initialize();

            var startup = Mvx.Resolve<IMvxAppStart>();
            startup.Start();
            AppCenter.Start("cf1d8265-b7ef-423d-8e71-d6765e9bf349",
                   typeof(Analytics), typeof(Crashes));

            Window.MakeKeyAndVisible();

            return true;
        }




        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.

            if (Core.Utility.Constants.UdpReceiver != null)
            {
                try
                {
                    BSUtility.Instance.DisconnectSocket();
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }


        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.

        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.


            if (Core.Utility.Constants.UdpReceiver != null)
            {
                Task.Run(async () =>
                    {
                        try
                        {
                            await BSUtility.Instance.InitializeSocketListening();
                        }
                        catch (System.Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }

                    });
            }
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.

            if (Core.Utility.Constants.UdpReceiver != null)
            {
                try
                {
                    BSUtility.Instance.DisconnectSocket();
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, [Transient] UIWindow forWindow)
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                var currentOrientation = UIDevice.CurrentDevice.Orientation;
                //var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;

                if (currentOrientation == UIDeviceOrientation.Portrait)
                {
                    return UIInterfaceOrientationMask.Portrait;
                }
                else if (currentOrientation == UIDeviceOrientation.LandscapeLeft)
                {
                    return UIInterfaceOrientationMask.LandscapeRight;
                }
                else if (currentOrientation == UIDeviceOrientation.LandscapeRight)
                {
                    return UIInterfaceOrientationMask.LandscapeLeft;
                }
                else
                {
                    return UIInterfaceOrientationMask.Portrait;
                }
            }
            else
            {
                return UIInterfaceOrientationMask.Portrait;
            }


        }
    }
}

