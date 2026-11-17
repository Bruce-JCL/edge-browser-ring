namespace Loupedeck.EdgeBrowserRingPlugin;

public abstract class EdgeShortcut(
    string name, string description, string group, VirtualKeyCode key, ModifierKey modifiers)
    : PluginDynamicCommand(name, description, group)
{
    protected override void RunCommand(string actionParameter)
    {
        Plugin.ClientApplication.SendKeyboardShortcut(key, modifiers);
    }
}

public sealed class NewTab() : EdgeShortcut(
    "New tab", "Open a new tab", "Tabs", VirtualKeyCode.KeyT, ModifierKey.Control);

public sealed class CloseTab() : EdgeShortcut(
    "Close tab", "Close the current tab", "Tabs", VirtualKeyCode.KeyW, ModifierKey.Control);

// Keep the action ID so existing ring layouts switch to tab search in place.
public sealed class RestoreTab() : EdgeShortcut(
    "Search tabs", "List and search open Edge tabs", "Tabs",
    VirtualKeyCode.KeyA, ModifierKey.Control | ModifierKey.Shift);

public sealed class PreviousTab() : EdgeShortcut(
    "Previous tab", "Switch to the previous tab", "Tabs",
    VirtualKeyCode.Tab, ModifierKey.Control | ModifierKey.Shift);

public sealed class NextTab() : EdgeShortcut(
    "Next tab", "Switch to the next tab", "Tabs", VirtualKeyCode.Tab, ModifierKey.Control);

public sealed class GoBack() : EdgeShortcut(
    "Go back", "Go back in the current tab", "Navigation", VirtualKeyCode.ArrowLeft, ModifierKey.Alt);

public sealed class GoForward() : EdgeShortcut(
    "Go forward", "Go forward in the current tab", "Navigation", VirtualKeyCode.ArrowRight, ModifierKey.Alt);

public sealed class ReloadPage() : EdgeShortcut(
    "Reload page", "Reload the current page", "Navigation", VirtualKeyCode.KeyR, ModifierKey.Control);

public sealed class FindOnPage() : EdgeShortcut(
    "Find on page", "Search within the current page", "Page tools", VirtualKeyCode.KeyF, ModifierKey.Control);

public sealed class FocusAddressBar() : EdgeShortcut(
    "Address bar", "Select the address bar", "Page tools", VirtualKeyCode.KeyL, ModifierKey.Control);

public sealed class OpenHistory() : EdgeShortcut(
    "History", "Open browsing history", "Browser tools", VirtualKeyCode.KeyH, ModifierKey.Control);

public sealed class OpenDownloads() : EdgeShortcut(
    "Downloads", "Open downloads", "Browser tools", VirtualKeyCode.KeyJ, ModifierKey.Control);

public sealed class ToggleFullScreen() : EdgeShortcut(
    "Full screen", "Switch full screen mode on or off", "Page tools", VirtualKeyCode.F11, default);

