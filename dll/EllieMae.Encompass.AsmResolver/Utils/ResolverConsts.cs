// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.ResolverConsts
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class ResolverConsts
  {
    public static string ManifestVersion = "1.0";
    public static string ZipExt = ".zip";
    public static string DeployFileExt = ".deploy";
    public static string DeployZipExt = ResolverConsts.DeployFileExt + ResolverConsts.ZipExt;
    public static string B64Ext = ".B64";
    public static string DeployZipB64Ext = ResolverConsts.DeployZipExt + ResolverConsts.B64Ext;
    public static string[] DeployExts = new string[3]
    {
      ResolverConsts.DeployZipExt,
      ResolverConsts.DeployFileExt,
      ResolverConsts.DeployZipB64Ext.ToLower()
    };
    public static string AppFileGroupsDir = "~AppFileGroups~";
    public static string DeployManifestExt = ".man";
    public static string AppManifestExt = ".exe.man";
    public static string HttpPrefix = "http://";
    public static string HttpsPrefix = "https://";
    public static string HashAlgorithmSha1 = "http://www.w3.org/2000/09/xmldsig#sha1";
    public static string Sha1HashFileExt = ".deploy.sha1";
    public static string Md5HashFileExt = ".deploy.md5";
    public static string AppUpdater = "AppUpdtr";
    public static string AppUpdaterExe = "AppUpdtr.exe";
    public static string AppLauncher = nameof (AppLauncher);
    public static string AppLauncherExe = "AppLauncher.exe";
    public static string AsmResolver = "EllieMae.Encompass.AsmResolver";
    public static string AsmResolverDll = "EllieMae.Encompass.AsmResolver.dll";
    public static string SharpZipLibDll = "ICSharpCode.SharpZipLib.dll";
    public static string OfflineReadyFileNamePrefix = "OfflineReady_";
    public static string DeployModuleDir = "~DeploymentModules~";
    public static string DeployModuleConfigFileExt = ".modules.config";
    public static string RestoreAppLauncherExe = "RestoreAppLauncher.exe";
    internal static string KB64 = XT.ESB64(BasicUtils.Localization + "@" + BasicUtils.Internationalization, BasicUtils.Internationalization + "@" + BasicUtils.Localization);
  }
}
