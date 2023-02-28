using Components;
using Nuke.Common;

class Build : NukeBuild, ICleanComponent, ICompileComponent, IPackComponent, IRestoreComponent, ITestComponent
{
    public static int Main () => Execute<Build>();
}
