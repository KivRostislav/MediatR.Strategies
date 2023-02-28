using Nuke.Common;

namespace Common;

public interface IHaveConfiguration : INukeBuild
{
    [Parameter] Configuration Configuration => TryGetValue(() => Configuration);
}
