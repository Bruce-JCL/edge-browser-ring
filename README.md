# Edge Browser Ring

An Actions Ring plugin for Microsoft Edge on Windows. It adds 15 browser actions to Logi Options+, including tab search, page navigation, settings, and extension management.

## Use

Requires Logi Options+ with Logi Plugin Service 6.4 or later and a device with Actions Ring, such as the MX Master 4. After installing the plugin, select or create the Edge Browser Ring profile in Logi Options+. It starts with eight default actions. Open Edge and press the device button assigned to Show Actions Ring.

## Support

If a shortcut or profile does not work, [open an issue](https://github.com/Bruce-JCL/edge-browser-ring/issues). Include your Windows, Edge, and Logi Options+ versions and the steps to reproduce the issue.

## Build

Install the .NET 10 SDK and Logi Options+, then build with `dotnet build -c Release`. The project references the Logi Plugin Service assemblies from its standard Windows installation path. Set `PluginApiDir` if they are installed elsewhere.

Edge Browser Ring is independently developed and is not affiliated with Microsoft or Logitech.
