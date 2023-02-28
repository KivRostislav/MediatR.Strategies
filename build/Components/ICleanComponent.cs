using Common;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.IO.FileSystemTasks;

namespace Components;

public interface ICleanComponent : INukeBuild, IHaveSolution
{
    Target Clean => _ => _
        .Executes(() =>
        {
            DotNetClean(x => x.SetProject(Solution.GetProject(_build.Constants.MediatRStrategiesProjectName)));
            DotNetClean(x => x.SetProject(Solution.GetProject(_build.Constants.MediatRStrategiesTestsProjectName)));
            EnsureCleanDirectory(RootDirectory / _build.Constants.ArtifactsDirectoryName);
        });
}
