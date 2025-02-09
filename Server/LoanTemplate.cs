// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanTemplate
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanTemplate
  {
    private const string className = "LoanTemplate�";
    private string name;

    public LoanTemplate(string name) => this.name = name;

    public LoanData Instantiate()
    {
      PerformanceMeter.Current.AddCheckpoint("Read ILoanSettings object...", 35, nameof (Instantiate), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      return this.Instantiate(LoanConfiguration.GetLoanSettings());
    }

    public LoanData Instantiate(UserInfo userInfo)
    {
      PerformanceMeter.Current.AddCheckpoint("Read ILoanSettings object...", 45, nameof (Instantiate), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      return this.Instantiate(LoanConfiguration.GetLoanSettings(userInfo));
    }

    public LoanData InstantiateFromPlatform()
    {
      PerformanceMeter.Current.AddCheckpoint("Read ILoanSettings object (from platform)...", 55, nameof (InstantiateFromPlatform), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      return this.InstantiateFromPlatform(LoanConfiguration.GetLoanSettings());
    }

    public LoanData Instantiate(ILoanSettings settings)
    {
      PerformanceMeter current = PerformanceMeter.Current;
      LoanFolder templateFolder = LoanTemplate.getTemplateFolder();
      LoanData source = templateFolder.ReadLoanData(this.name, settings, loanFolder: templateFolder.Name);
      current.AddCheckpoint("Deserialized template LoanData object from disk...", 82, nameof (Instantiate), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      LoanData loanData = new LoanData(source, settings, true, enableEnchancedConditions: EnConfigurationSettings.AppSettings["EnableEnchancedConditions"] == "true");
      current.AddCheckpoint("Instantiated template LoanData object...", 85, nameof (Instantiate), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      return loanData;
    }

    public LoanData InstantiateFromPlatform(ILoanSettings settings)
    {
      PerformanceMeter current = PerformanceMeter.Current;
      current.AddCheckpoint("Instantiation starts...", 95, nameof (InstantiateFromPlatform), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      LoanData loanData = LoanTemplate.getTemplateFolder().ReadBlankLoanData(this.name, settings);
      current.AddCheckpoint("Instantiation ends.", 99, nameof (InstantiateFromPlatform), "D:\\ws\\24.3.0.0\\EmLite\\Server\\ServerObjects\\LoanTemplate.cs");
      return loanData;
    }

    public static LoanTemplateInfo[] GetLoanTemplates()
    {
      try
      {
        ArrayList arrayList = new ArrayList();
        foreach (DirectoryInfo directory in new DirectoryInfo(Path.Combine(ClientContext.GetCurrent().Settings.ApplicationDir, "documents\\templates")).GetDirectories())
          arrayList.Add((object) new LoanTemplateInfo(directory.Name, directory.Name));
        return (LoanTemplateInfo[]) arrayList.ToArray(typeof (LoanTemplateInfo));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanTemplate), ex);
        return (LoanTemplateInfo[]) null;
      }
    }

    private static LoanFolder getTemplateFolder()
    {
      return new LoanFolder(Path.Combine(ClientContext.GetCurrent().Settings.ApplicationDir, "documents\\templates"), true);
    }
  }
}
