using Common;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;

namespace Components;

public interface IPackComponent : INukeBuild, ICompileComponent, ITestComponent, IHaveSolution, IHaveConfiguration
{
    [Parameter] string PackageId => TryGetValue(() => PackageId);

    [Parameter] string PackageVersion => TryGetValue(() => PackageVersion);

    [Parameter] string PackageDescription => TryGetValue(() => PackageDescription);

    [Parameter] string PackageAuthors => TryGetValue(() => PackageAuthors);

    [Parameter] string[] PackageTags => TryGetValue(() => PackageTags);

    [Parameter] string RepositoryUrl => TryGetValue(() => RepositoryUrl);

    [Parameter] string PackageLicenseUrl => TryGetValue(() => PackageLicenseUrl);

    [Parameter] string PackageProjectUrl => TryGetValue(() => PackageProjectUrl);

    Target Pack => _ => _
        .DependsOn(Compile, Test)
        .Requires(() => Configuration, () => PackageId, () => PackageAuthors, () => PackageDescription, () => PackageLicenseUrl, () =>PackageProjectUrl, () => RepositoryUrl)
        .Requires(() => PackageTags)
        .OnlyWhenDynamic(() => Configuration == Configuration.Release)
        .Executes(() =>
        {
            DotNetPack(x => x
                .SetProject(Solution.GetProject(_build.Constants.MediatRStrategiesProjectName))
                .SetConfiguration(Configuration.Release)
                .SetOutputDirectory(RootDirectory / _build.Constants.ArtifactsDirectoryName)
                .SetPackageId(PackageId)
                .SetAuthors(PackageAuthors)
                .SetDescription(PackageDescription)
                .SetVersion(PackageVersion)
                .SetPackageTags(PackageTags)
                .SetPackageLicenseUrl(PackageLicenseUrl)
                .SetPackageProjectUrl(PackageProjectUrl)
                .SetRepositoryUrl(RepositoryUrl)
                .EnableNoRestore()
                .EnableNoBuild());
        });
}
