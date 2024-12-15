using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Core.Services;

public class PlatformDetection(PlatformKind current)
{
    public PlatformKind Current { get; } = current;
}
