using Common;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace Components;

public interface ITestComponent : INukeBuild, ICompileComponent, IHaveSolution, IHaveConfiguration
{
    Target Test => _ => _
        .DependsOn(Compile)
        .Requires(() => Configuration)
        .Executes(() =>
        {
            DotNetTest(x => x
                .SetProjectFile(Solution.GetProject(_build.Constants.MediatRStrategiesTestsProjectName))
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });
}
