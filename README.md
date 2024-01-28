# ricaun.Revit.UI.Tasks

[![Revit 2017](https://img.shields.io/badge/Revit-2017+-blue.svg)](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks/actions/workflows/Build.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks/actions)
[![Release](https://img.shields.io/nuget/v/ricaun.Revit.UI.Tasks?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.Revit.UI.Tasks)

`ricaun.Revit.UI.Tasks` implementation to run Revit code and `await` the code to finish.

[![ricaun.Revit.UI.Tasks](https://raw.githubusercontent.com/ricaun-io/ricaun.Revit.UI.Tasks/develop/assets/ricaun.Revit.UI.Tasks.png)](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks)

This project was generated by the [ricaun.AppLoader](https://ricaun.com/AppLoader/) Revit plugin.

## RevitTaskService

The `RevitTaskService` is a service that manages the creation of the `AsyncExternalEventHandler` inside the `Idling` event.

```C#
UIControlledApplication application;
RevitTaskService revitTaskService = new RevitTaskService(application);
```

### Initialize

Initialize the `RevitTaskService` to start the `Idling` event.

```C#
revitTaskService.Initialize();
```

### Dispose

Dispose the `RevitTaskService` to stop the `Idling` event.

```C#
revitTaskService.Dispose();
```

## IRevitTask

The `RevitTaskService` has the interface `IRevitTask` with the `Run` method to execute code in Revit Context.

```C#
IRevitTask revitTask = revitTaskService;
```

### Run

The `Run` method runs the code inside Revit Context and `await` the code to finish.

```C#
UIApplication uiapp = await revitTask.Run((uiapp) =>
{
    // Code run inside Revit Context
    return uiapp;
});
```

The `IRevitTask` interface has an extension methods for `Run` without `UIApplication` and `return`.

```C#
await revitTask.Run(() => { });
await revitTask.Run(() => { return 1; });
await revitTask.Run((uiapp) => { });
```

## Example

Example sample to implement `RevitTaskService` within `IExternalApplication`.

```C#
public class App : IExternalApplication
{
    private static RevitTaskService revitTaskService;
    public static IRevitTask RevitTask => revitTaskService;
    public Result OnStartup(UIControlledApplication application)
    {
        revitTaskService = new RevitTaskService(application);
        revitTaskService.Initialize();

        Task.Run(async () =>
        {
            await Task.Delay(1000);
            await RevitTask.Run(() =>
            {
                TaskDialog.Show("Revit", "Hello from RevitTask");
            });
        });

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        revitTaskService?.Dispose();

        return Result.Succeeded;
    }
}
```

## Todo

* Run code if in `Revit Context` instead of awaiting Task.
* `CancellationToken` timeout support to prevent infinite await.

## Similar Projects

There are some similar packages/implementations to run Revit API code in an async way using await/Tasks.

* [Revit.Async](https://github.com/KennanChan/Revit.Async)
* [RevitTask](https://github.com/WhiteSharq/RevitTask)

## Release

* [Latest release](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks/releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](https://github.com/ricaun-io/ricaun.Revit.UI.Tasks/stargazers)!