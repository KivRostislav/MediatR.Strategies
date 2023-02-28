using Common;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace Components;

public interface IRestoreComponent : INukeBuild, ICleanComponent, IHaveSolution
{
    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(x => x.SetProjectFile(Solution.GetProject(_build.Constants.MediatRStrategiesProjectName)));
            DotNetRestore(x => x.SetProjectFile(Solution.GetProject(_build.Constants.MediatRStrategiesTestsProjectName)));
        });
}
