namespace AspNetCoreBlazor.Core.Data;

public class PlatformDetection(PlatformKind current)
{
    public PlatformKind Current { get; } = current;
}
