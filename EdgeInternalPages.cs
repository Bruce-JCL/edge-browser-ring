using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Automation;

namespace Loupedeck.EdgeBrowserRingPlugin;

public abstract class EdgeInternalPage(string name, string description, string address)
    : PluginDynamicCommand(name, description, "Browser tools")
{
    protected override void RunCommand(string actionParameter)
    {
        var window = GetForegroundWindow();
        if (window == IntPtr.Zero || GetWindowThreadProcessId(window, out var processId) == 0)
            return;

        try
        {
            using var process = Process.GetProcessById((int)processId);
            if (!process.ProcessName.Equals("msedge", StringComparison.OrdinalIgnoreCase))
                return;
        }
        catch (ArgumentException)
        {
            return;
        }

        Input[] focusAddress = [Key(0x11), Key(0x4C), Key(0x4C, KeyUp), Key(0x11, KeyUp)];
        if (GetForegroundWindow() != window)
            return;

        SendInput((uint)focusAddress.Length, focusAddress, Marshal.SizeOf<Input>());
        if (TrySetAddressBar(window, processId))
        {
            if (GetForegroundWindow() == window)
            {
                Input[] openTab = [Key(0x12), Key(0x0D), Key(0x0D, KeyUp), Key(0x12, KeyUp)];
                SendInput((uint)openTab.Length, openTab, Marshal.SizeOf<Input>());
            }
            return;
        }

        // Fall back to keyboard input if the address bar does not expose ValuePattern.
        var input = new Input[8 + address.Length * 2];
        input[0] = Key(0x11);
        input[1] = Key(0x4C);
        input[2] = Key(0x4C, KeyUp);
        input[3] = Key(0x11, KeyUp);
        for (var i = 0; i < address.Length; i++)
        {
            input[4 + i * 2] = Key(0, Unicode, address[i]);
            input[5 + i * 2] = Key(0, Unicode | KeyUp, address[i]);
        }
        input[^4] = Key(0x12);
        input[^3] = Key(0x0D);
        input[^2] = Key(0x0D, KeyUp);
        input[^1] = Key(0x12, KeyUp);

        if (GetForegroundWindow() == window)
            SendInput((uint)input.Length, input, Marshal.SizeOf<Input>());
    }

    private bool TrySetAddressBar(IntPtr window, uint processId)
    {
        try
        {
            for (var attempt = 0; attempt < 8 && GetForegroundWindow() == window; attempt++)
            {
                var field = AutomationElement.FocusedElement;
                if (field is not null && field.Current.ProcessId == processId &&
                    field.Current.ControlType == ControlType.Edit && IsToolbarEdit(field) &&
                    field.TryGetCurrentPattern(ValuePattern.Pattern, out var pattern) &&
                    pattern is ValuePattern value && !value.Current.IsReadOnly)
                {
                    value.SetValue(address);
                    return value.Current.Value.Equals(address, StringComparison.OrdinalIgnoreCase);
                }

                Thread.Sleep(15);
            }
        }
        catch (Exception exception) when (exception is InvalidOperationException or COMException)
        {
            // Edge can replace the focused element while its address bar opens.
        }

        return false;
    }

    private static bool IsToolbarEdit(AutomationElement field)
    {
        for (var element = TreeWalker.ControlViewWalker.GetParent(field);
             element is not null;
             element = TreeWalker.ControlViewWalker.GetParent(element))
        {
            var controlType = element.Current.ControlType;
            if (controlType == ControlType.ToolBar)
                return true;
            if (controlType == ControlType.Document || controlType == ControlType.Window)
                return false;
        }

        return false;
    }

    private const uint KeyUp = 0x0002;
    private const uint Unicode = 0x0004;

    private static Input Key(ushort key, uint flags = 0, ushort scan = 0) => new()
    {
        Type = 1,
        Data = new InputData { Keyboard = new KeyboardInput { Key = key, Scan = scan, Flags = flags } }
    };

    [StructLayout(LayoutKind.Sequential)]
    private struct Input { public uint Type; public InputData Data; }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputData
    {
        [FieldOffset(0)] public KeyboardInput Keyboard;
        // MOUSEINPUT determines the size of the native INPUT union.
        [FieldOffset(0)] public MouseInput Mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KeyboardInput { public ushort Key, Scan; public uint Flags, Time; public UIntPtr ExtraInfo; }

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseInput { public int X, Y; public uint Data, Flags, Time; public UIntPtr ExtraInfo; }

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr window, out uint processId);

    [DllImport("user32.dll")]
    private static extern uint SendInput(uint count, Input[] inputs, int size);
}

public sealed class OpenEdgeSettings() : EdgeInternalPage(
    "Edge settings", "Open Microsoft Edge settings", "edge://settings/");

public sealed class ManageEdgeExtensions() : EdgeInternalPage(
    "Manage extensions", "Open the Edge extensions manager", "edge://extensions/");

