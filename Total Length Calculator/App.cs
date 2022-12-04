#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Application = Autodesk.Revit.ApplicationServices.Application;

#endregion

namespace Total_Length_Calculator
{
    class App : IExternalApplication
    {
        // ExternalCommands assembly path
        static string AddInPath =
          typeof(App).Assembly.Location;

        #region IExternalApplication Members

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // create a Ribbon panel which contains three 
                // stackable buttons and one single push button

                string firstPanelName = "TotalLengthCalculator";
                RibbonPanel panel = application.CreateRibbonPanel(firstPanelName);

                // set the information about the command we will 
                // be assigning to the button 

                PushButtonData pushButtonData = new PushButtonData(
                  "Total Length Calculator",
                  "Total Length \n Calculator",
                  AddInPath,
                  "Total_Length_Calculator.Command");

                //' add a button to the panel 

                PushButton pushButton = panel.AddItem(pushButtonData)
                  as PushButton;

                //' add an icon 

                pushButton.LargeImage = LoadPNGImageFromResource(
                "Total_Length_Calculator.Icon.Total Length Calculator.png");

                // add a tooltip 
                pushButton.ToolTip =
                  "This add-in helps you measure the total length of selected ducts, pipes, cable trays and conduits.";

                // long description

                pushButton.LongDescription =
                     "This add-in helps you measure the total length of selected ducts, pipes, cable trays and conduits. Mark elements and check their length. It's so easy!";

                // Context (F1) Help - new in 2013 
                //string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %AppData% 

                //string path;
                
                //path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) 
                //    + @"\Autodesk\ApplicationPlugins\LiksCode Total Length Calculator.bundle\Contents\help.html";

                //ContextualHelp contextHelp = new ContextualHelp(
                //    ContextualHelpType.ChmFile,
                //    path); // hard coding for simplicity. 

                //pushButton.SetContextualHelp(contextHelp);

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Total Length Calculator Ribbon");
                return Autodesk.Revit.UI.Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        private static System.Windows.Media.ImageSource
        LoadPNGImageFromResource(string imageResourceName)
        {
            System.Reflection.Assembly dotNetAssembly =
              System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream iconStream =
              dotNetAssembly.GetManifestResourceStream(imageResourceName);
            System.Windows.Media.Imaging.PngBitmapDecoder bitmapDecoder =
              new System.Windows.Media.Imaging.PngBitmapDecoder(iconStream,
                System.Windows.Media.Imaging.BitmapCreateOptions.
                PreservePixelFormat, System.Windows.Media.Imaging.
                BitmapCacheOption.Default);
            System.Windows.Media.ImageSource imageSource =
              bitmapDecoder.Frames[0];
            return imageSource;
        }
        #endregion
    }





}
