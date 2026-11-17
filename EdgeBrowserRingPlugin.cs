namespace Loupedeck.EdgeBrowserRingPlugin;

public sealed class EdgeBrowserRingPlugin : Plugin
{
    public override bool HasNoApplication => false;
    public override bool UsesApplicationApiOnly => false;
}

public sealed class EdgeBrowserRingApplication : ClientApplication
{
    protected override string GetProcessName() => "msedge";
    protected override string GetBundleName() => "";
}
