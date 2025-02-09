// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AppSuiteInfo
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System.IO;
using System.Reflection;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class AppSuiteInfo
  {
    private string companyName;
    private string appSuiteName;

    public string CompanyName => this.companyName;

    public string AppSuiteName => this.appSuiteName;

    public AppSuiteInfo(string companyName, string appSuiteName)
    {
      this.companyName = companyName;
      this.appSuiteName = appSuiteName;
    }

    public AppSuiteInfo(string executableFilePath)
    {
      Path.GetDirectoryName(executableFilePath);
      this.getAppSuiteInfo(Assembly.LoadFrom(executableFilePath));
    }

    public AppSuiteInfo(Assembly exeAsm) => this.getAppSuiteInfo(exeAsm);

    private void getAppSuiteInfo(Assembly exeAsm)
    {
      object[] customAttributes = exeAsm.GetCustomAttributes(typeof (AssemblyApplicationSuiteAttribute), false);
      if (customAttributes != null && customAttributes.Length == 1)
      {
        this.companyName = ((AssemblyApplicationSuiteAttribute) customAttributes[0]).CompanyName;
        this.appSuiteName = ((AssemblyApplicationSuiteAttribute) customAttributes[0]).AppSuiteName;
      }
      if (!BasicUtils.IsNullOrEmpty(this.appSuiteName))
        return;
      string fullName = exeAsm.FullName;
      this.appSuiteName = fullName.Substring(0, fullName.IndexOf(','));
    }
  }
}
