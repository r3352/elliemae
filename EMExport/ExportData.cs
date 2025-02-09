// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.ExportData
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using Elli.Interface;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class ExportData
  {
    private const string typeName = "ExportData";
    private static string traceSW = Tracing.SwImportExport;
    public const string FORMAT_EDRSXML = "EDRS";
    private LoanDataMgr loanDataMgr;
    private LoanData loanData;
    private IEnumerable<LoanData> loanDataCollection;

    public ExportData()
    {
    }

    public ExportData(LoanDataMgr loanDataMgr, LoanData loanData)
    {
      bool flag = false;
      if (loanDataMgr == null)
        flag = true;
      else if (loanDataMgr.LoanData == null)
        flag = true;
      else if (loanDataMgr.LoanData.GUID != loanData.GUID)
        flag = true;
      if (flag)
        loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, loanData.GUID, false);
      this.loanDataMgr = loanDataMgr;
      this.loanData = loanData;
    }

    public ExportData(IEnumerable<LoanData> loanDataCollection)
    {
      this.loanDataCollection = loanDataCollection;
    }

    public static string GetExportAssemblyPath(string format)
    {
      if (string.Compare(format, "EDRS", true) == 0)
        return (string) null;
      switch (format.ToUpper())
      {
        case "EMXML":
          format = "EMXML10";
          break;
        case "FNMA30":
          format = "FANNIE30";
          break;
        case "FNMA32":
          format = "FANNIE32";
          break;
        case "FANNIE":
          format = "LOANDELIVERY";
          break;
        case "FREDDIE":
          format = "LOANDELIVERY";
          break;
        case "GINNIEMAEPDD12.161":
          format = "GnmaPdd12.161";
          break;
      }
      return SystemSettings.EpassDataDir + "Export." + format + ".dll";
    }

    private Assembly GetExportAssembly(string format)
    {
      string exportAssemblyPath = ExportData.GetExportAssemblyPath(format);
      Tracing.Log(ExportData.traceSW, nameof (ExportData), TraceLevel.Verbose, "Export assembly file: " + exportAssemblyPath);
      if (!File.Exists(exportAssemblyPath))
        return (Assembly) null;
      string fullName = AssemblyName.GetAssemblyName(exportAssemblyPath).FullName;
      Assembly exportAssembly = (Assembly) null;
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (assembly.FullName == fullName)
          exportAssembly = assembly;
      }
      if (exportAssembly == (Assembly) null)
      {
        FileStream fileStream = File.OpenRead(exportAssemblyPath);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        exportAssembly = Assembly.Load(numArray);
      }
      return exportAssembly;
    }

    public string Export(string format) => this.Export(format, false);

    public string Export(string format, bool currentBorPairOnly)
    {
      if (string.Compare(format, "EDRS", true) == 0)
        return this.loanData.ToXml(true);
      Assembly exportAssembly = this.GetExportAssembly(format);
      if (exportAssembly == (Assembly) null)
        return string.Empty;
      string fullName = typeof (IExport).FullName;
      string typeName = (string) null;
      foreach (Type type in exportAssembly.GetTypes())
      {
        if (type.GetInterface(fullName) != (Type) null)
          typeName = type.FullName;
      }
      IExport instance = (IExport) exportAssembly.CreateInstance(typeName);
      if (format.Equals("LEF", StringComparison.OrdinalIgnoreCase))
        this.loanData.AttachSnapshotProvider((ILoanSnapshotProvider) new LoanSnapshotProvider(this.loanDataMgr));
      instance.Bam = (IBam) new Bam(this.loanData);
      string empty = string.Empty;
      string str;
      if (currentBorPairOnly)
      {
        MethodInfo method = instance.GetType().GetMethod(nameof (ExportData), new Type[1]
        {
          typeof (bool)
        });
        str = !(method == (MethodInfo) null) ? string.Concat(method.Invoke((object) instance, new object[1]
        {
          (object) true
        })) : throw new NotSupportedException("The export format '" + format + "' does not support export of the current borrower pair only");
      }
      else
        str = this.exportDataMethodInvoke(instance);
      this.CreateExportLog(format, format);
      return str;
    }

    public string ExportMultiple(string format)
    {
      string str = string.Empty;
      Assembly exportAssembly = this.GetExportAssembly(format);
      if (exportAssembly == (Assembly) null)
        return str;
      string fullName = typeof (IExportMultiple).FullName;
      string typeName = (string) null;
      foreach (Type type in exportAssembly.GetTypes())
      {
        if (type.GetInterface(fullName) != (Type) null)
          typeName = type.FullName;
      }
      IExportMultiple instance = (IExportMultiple) exportAssembly.CreateInstance(typeName);
      using (Tracing.StartTimer(ExportData.traceSW, nameof (ExportData), TraceLevel.Info, "Exporting Loans"))
        str = instance.Export((IEnumerable<IBam>) this.loanDataCollection.Select<LoanData, Bam>((Func<LoanData, Bam>) (l => new Bam(l))));
      using (Tracing.StartTimer(ExportData.traceSW, nameof (ExportData), TraceLevel.Info, "Creating Export Logs"))
        this.CreateExportLog(format, format);
      return str;
    }

    protected virtual string exportDataMethodInvoke(IExport export) => export.ExportData();

    public bool Validate(string format, bool allowContinue)
    {
      string empty = string.Empty;
      string str = !(format == "LEF") ? SystemSettings.EpassDataDir + "Export.Validate.dll" : SystemSettings.EpassDataDir + "Export.LEF.dll";
      if (!File.Exists(str))
        return true;
      string fullName1 = AssemblyName.GetAssemblyName(str).FullName;
      Assembly assembly1 = (Assembly) null;
      foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
      {
        if (assembly2.FullName == fullName1)
          assembly1 = assembly2;
      }
      if (assembly1 == (Assembly) null)
      {
        FileStream fileStream = File.OpenRead(str);
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        fileStream.Close();
        assembly1 = Assembly.Load(numArray);
      }
      string fullName2 = typeof (IValidate).FullName;
      string typeName = (string) null;
      foreach (Type type in assembly1.GetTypes())
      {
        if (type.GetInterface(fullName2) != (Type) null)
          typeName = type.FullName;
      }
      IValidate instance = (IValidate) assembly1.CreateInstance(typeName);
      instance.Bam = (IBam) new Bam(this.loanData);
      return instance.ValidateData(format, allowContinue);
    }

    public void CreateExportLog(string category, string serviceType)
    {
      this.CreateExportLog(category, new string[1]
      {
        serviceType
      });
    }

    public void CreateExportLog(ExportLog.ExportCategory category, string serviceType)
    {
      this.CreateExportLog(category, new string[1]
      {
        serviceType
      });
    }

    public void CreateExportLog(string category, string[] serviceTypes)
    {
      ExportLog.ExportCategory category1 = ExportLog.ExportCategory.None;
      switch (category)
      {
        case "GSE Services":
          category1 = ExportLog.ExportCategory.GSEServices;
          break;
        case "Compliance Services":
          category1 = ExportLog.ExportCategory.ComplianceServices;
          break;
      }
      if (category1 == ExportLog.ExportCategory.None)
      {
        foreach (string category2 in ServicesMapping.Categories)
        {
          foreach (ServiceSetting serviceSetting in ServicesMapping.GetServiceSetting(category2))
          {
            if (serviceSetting.DataServiceID == category)
              this.CreateExportLog(serviceSetting.CategoryName, serviceTypes);
          }
        }
      }
      if (category1 == ExportLog.ExportCategory.None)
        return;
      this.CreateExportLog(category1, serviceTypes);
    }

    public void CreateExportLog(ExportLog.ExportCategory category, string[] serviceTypes)
    {
      ExportLog exportLog = new ExportLog(DateTime.Now);
      exportLog.ExportedBy = Session.UserInfo.Userid;
      exportLog.ExportedByFullName = Session.UserInfo.FullName;
      exportLog.Category = category;
      string category1 = "";
      if (category == ExportLog.ExportCategory.GSEServices)
        category1 = "GSE Services";
      if (category == ExportLog.ExportCategory.ComplianceServices)
        category1 = "Compliance Services";
      foreach (string serviceType in serviceTypes)
      {
        string o = serviceType;
        foreach (ServiceSetting serviceSetting in ServicesMapping.GetServiceSetting(category1))
        {
          if (serviceSetting.DataServiceID == serviceType)
            o = serviceSetting.DisplayName;
        }
        exportLog.ItemList.Add((object) o);
      }
      if (this.loanData != null)
        this.AddExportLog(exportLog, this.loanData, this.loanDataMgr);
      if (this.loanDataCollection == null)
        return;
      foreach (LoanData loanData in this.loanDataCollection)
      {
        LoanDataMgr loanDataMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, loanData.GUID, false);
        this.AddExportLog(exportLog, loanData, loanDataMgr);
      }
    }

    private void AddExportLog(ExportLog exportLog, LoanData loanData, LoanDataMgr loanDataMgr)
    {
      loanData.GetLogList().AddRecord((LogRecordBase) exportLog);
      LoanEventLogList loanEventLog = new LoanEventLogList();
      loanEventLog.InsertNonSystemLog((LogRecordBase) exportLog);
      loanDataMgr.AddLoanEventLogs(loanEventLog);
    }

    public bool EpassExportMultiple(string epassSignature, string[] guids)
    {
      try
      {
        IEPass service = Session.Application.GetService<IEPass>();
        string str = ((IEnumerable<string>) guids).Aggregate<string, string>(string.Empty, (Func<string, string, string>) ((current, guid) => current + guid + "&"));
        epassSignature += str;
        Tracing.Log(ExportData.traceSW, nameof (ExportData), TraceLevel.Info, "Exported Guids to Epass: " + str);
        service.ProcessURL(epassSignature, false);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(ExportData.traceSW, nameof (ExportData), TraceLevel.Error, "EpassMultipleLoanGuids Failed" + ex.Message);
      }
      return false;
    }
  }
}
