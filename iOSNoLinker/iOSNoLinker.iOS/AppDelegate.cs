using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Foundation;
using UIKit;

namespace iOSNoLinker.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            var result = base.FinishedLaunching(app, options);

            Assembly foundAssembly = null;
            Debug.WriteLine("without calling LoadLibrary");

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.Contains("ExternalLibrary"))
                {
                    foundAssembly = assembly;
                    Debug.WriteLine(assembly.FullName);
                }
            }

            Debug.WriteLine("after calling LoadLibrary");
            if (foundAssembly == null)
            {
                Assembly.Load("ExternalLibrary");
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.FullName.Contains("ExternalLibrary"))
                    {
                        foundAssembly = assembly;
                        Debug.WriteLine(assembly.FullName);
                    }
                }
            }

            if (foundAssembly == null)
                throw new Exception("assembly not found");

            var type = foundAssembly.CreateInstance("ExternalLibrary.Class1");

            return result;
        }
    }
}
