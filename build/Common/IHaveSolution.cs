using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace Common;

public interface IHaveSolution : INukeBuild
{
    [Solution] Solution Solution => TryGetValue(() => Solution); 
}
