// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.AssemblyApplicationSuiteAttribute
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
  public class AssemblyApplicationSuiteAttribute : Attribute
  {
    private static char[] invalidChars = Path.GetInvalidFileNameChars();
    public readonly string CompanyName;
    public readonly string AppSuiteName;

    public AssemblyApplicationSuiteAttribute(string companyName, string appSuiteName)
    {
      if (appSuiteName != null && appSuiteName.IndexOfAny(AssemblyApplicationSuiteAttribute.invalidChars) >= 0)
        throw new Exception("Application suite name '" + appSuiteName + "' contains some invalid characters");
      this.CompanyName = companyName == null || companyName.IndexOfAny(AssemblyApplicationSuiteAttribute.invalidChars) < 0 ? companyName : throw new Exception("Company name '" + companyName + "' contains some invalid characters");
      this.AppSuiteName = appSuiteName;
    }
  }
}
