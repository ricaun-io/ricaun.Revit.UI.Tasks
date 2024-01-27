using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ricaun.Revit.UI;
using System.Threading.Tasks;

namespace ricaun.Revit.UI.Tasks.Revit
{
    [AppLoader]
    public class App : IExternalApplication
    {
        private static RibbonPanel ribbonPanel;
        private static RevitTaskService revitTaskService;
        public static IRevitTask RevitTask => revitTaskService;
        public Result OnStartup(UIControlledApplication application)
        {
            revitTaskService = new RevitTaskService(application);
            revitTaskService.Initialize();

            // var revitTask = new RevitTask(revitTaskService);

            ribbonPanel = application.CreatePanel("Tasks");
            var button = ribbonPanel.CreatePushButton<Commands.Command>()
                .SetLargeImage("/UIFrameworkRes;component/ribbon/images/revit.ico");


            var task = Task.Run(async () =>
            {
                await Task.Delay(1000);

                await RevitTask.Run(() =>
                {
                    button.SetText("Tasks");
                });
            });

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            ribbonPanel?.Remove();
            revitTaskService?.Dispose();
            return Result.Succeeded;
        }
    }

}
