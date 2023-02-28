using Common;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace Components;

public interface ICompileComponent : INukeBuild, IRestoreComponent, IHaveSolution, IHaveConfiguration
{
    Target Compile => _ => _
        .DependsOn(Restore)
        .Requires(() => Configuration)
        .Executes(() =>
        {
            DotNetBuild(x => x
                .SetProjectFile(Solution.GetProject(_build.Constants.MediatRStrategiesProjectName))
                .SetConfiguration(Configuration)
                .EnableNoRestore());

            DotNetBuild(x => x
                .SetProjectFile(Solution.GetProject(_build.Constants.MediatRStrategiesTestsProjectName))
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });
}
