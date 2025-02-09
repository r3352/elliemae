// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.Consts
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class Consts
  {
    public static string ManifestVersion = "1.0";
    public static string ZipExt = ".zip";
    public static string DeployFileExt = ".deploy";
    public static string DeployZipExt = Consts.DeployFileExt + Consts.ZipExt;
    public static string B64Ext = ".B64";
    public static string DeployZipB64Ext = Consts.DeployZipExt + Consts.B64Ext;
    public static string[] DeployExts = new string[3]
    {
      Consts.DeployZipExt,
      Consts.DeployFileExt,
      Consts.DeployZipB64Ext.ToLower()
    };
    public static string DeployManifestExt = ".man";
    public static string AppManifestExt = ".exe.man";
    public static string AppUpdater = "AppUpdtr";
    public static string AppUpdaterExe = "AppUpdtr.exe";
    public static string AppLauncher = nameof (AppLauncher);
    public static string AppLauncherExe = "AppLauncher.exe";
    public static string AsmResolver = "EllieMae.Encompass.AsmResolver";
    public static string AsmResolverDll = "EllieMae.Encompass.AsmResolver.dll";
    public static string SharpZipLibDll = "ICSharpCode.SharpZipLib.dll";
    public static string AppCompanyName = "Ellie Mae";
    public static string EventLogSource = "RestoreAppLauncher";
    public static string EncompassSmartClient = "Encompass SmartClient";
    internal static string KB64 = XT.ESB64(BasicUtils.Localization + "@" + BasicUtils.Internationalization, BasicUtils.Internationalization + "@" + BasicUtils.Localization);
    public static readonly string[] AppUpdtrFiles = new string[2]
    {
      "AppUpdtr.exe",
      "AppUpdtr.exe.config"
    };
    public static readonly string[] EncNoUpdtrFiles = new string[14]
    {
      "AppLauncher.exe",
      "AppLauncher.exe.config",
      "EllieMae.Encompass.AsmResolver.dll",
      "Encompass.exe",
      "Encompass.exe.config",
      "RemoveUAC.exe",
      "AdminTools.exe",
      "AdminTools.exe.config",
      "SettingsTool.exe",
      "FormBuilder.exe",
      "EncAdminTools.exe",
      "ClickLoanProxy.exe",
      "CRMTool.exe",
      "SDKConfig.exe"
    };
    public static readonly string[] EncFiles = (string[]) null;
    public static readonly string[] SCAppMgrFiles = new string[4]
    {
      "SCAppMgr.exe",
      "SCAppMgr.exe.config",
      "SCAppMgrInstaller.exe",
      "SCAppMgrInstaller.exe.config"
    };
    public static readonly string[] AllFiles = (string[]) null;
    public static readonly Dictionary<string, string> EncFileMapping = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    public static readonly Dictionary<string, string> SCAppMgrFileMapping = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    static Consts()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) Consts.EncNoUpdtrFiles);
      stringList.AddRange((IEnumerable<string>) Consts.AppUpdtrFiles);
      Consts.EncFiles = stringList.ToArray();
      stringList.AddRange((IEnumerable<string>) Consts.SCAppMgrFiles);
      Consts.AllFiles = stringList.ToArray();
      foreach (string encFile in Consts.EncFiles)
        Consts.EncFileMapping.Add(encFile, encFile);
      Consts.EncFileMapping.Add("AsmResolver.dll", "EllieMae.Encompass.AsmResolver.dll");
      foreach (string scAppMgrFile in Consts.SCAppMgrFiles)
        Consts.SCAppMgrFileMapping.Add(scAppMgrFile, scAppMgrFile);
    }
  }
}
