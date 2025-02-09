// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ConcurrentEditing.LoanMergeTool
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Xml3WayMerge;
using EllieMae.Encompass.Xml3WayMerge.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ConcurrentEditing
{
  public class LoanMergeTool
  {
    private const string className = "ConcurrentEditingMerger�";
    protected static string sw = Tracing.SwOutsideLoan;
    private LoanMergeTool.MergedResult loanMergedResult;
    private LoanMergeTool.MergedResult linkedLoanMergedResult;

    public LoanMergeTool.MergedResult LoanMergedResult => this.loanMergedResult;

    public LoanMergeTool.MergedResult LinkedLoanMergedResult => this.linkedLoanMergedResult;

    public void Merge(LoanDataMgr loanDataMgr, bool isExternalOrganization)
    {
      if (loanDataMgr != null)
        this.loanMergedResult = this.merge(loanDataMgr, isExternalOrganization);
      if (loanDataMgr.LinkedLoan == null)
        return;
      this.linkedLoanMergedResult = this.merge(loanDataMgr.LinkedLoan, isExternalOrganization);
    }

    private LoanMergeTool.MergedResult merge(LoanDataMgr loanDataMgr, bool isExternalOrganization)
    {
      if (loanDataMgr.LoanData == null)
        return (LoanMergeTool.MergedResult) null;
      LoanData loanData1 = loanDataMgr.LoanData;
      string guid = loanData1.GUID;
      if (!FieldDefUtil.Initialized)
        FieldDefUtil.Init(StandardFields.MapFilePath);
      LoanData loanData2 = loanDataMgr.SessionObjects.LoanManager.OpenLoan(guid).GetLoanData(isExternalOrganization);
      DateTime baseLastModified = loanData2.BaseLastModified;
      bool flag = true;
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass\\ConcurrentEditing"))
      {
        if (registryKey != null)
          flag = string.Concat(registryKey.GetValue("Debug")).Trim() != "1";
      }
      string x3wmBaseLoanDataXml = loanDataMgr.X3wmBaseLoanDataXml;
      string xml1 = loanData2.ToXml(LoanContentAccess.FullAccess, true);
      string xml2 = loanData1.ToXml(LoanContentAccess.FullAccess, true);
      LoanDataMergeTool loanDataMergeTool = new LoanDataMergeTool(x3wmBaseLoanDataXml, xml1, xml2, false);
      return new LoanMergeTool.MergedResult(loanDataMergeTool.MergedXmlDocument.OuterXml, loanDataMergeTool.GetMergeReport(FieldDefUtil.MergeFieldDefs, flag, flag, flag), xml1, baseLastModified, loanDataMergeTool.BaseXmlDocument.OuterXml, loanDataMergeTool.FirstXmlDocument.OuterXml, loanDataMergeTool.SecondXmlDocument.OuterXml);
    }

    public class MergedResult
    {
      public readonly string MergedLoanDataXml;
      public readonly List<MergedObject> MergedReport;
      public readonly string NewBaseLoanDataXml;
      public readonly DateTime NewBaseLastModTime = DateTime.MinValue;
      public readonly string BaseLoanDataXml;
      public readonly string FirstLoanDataXml;
      public readonly string SecondLoanDataXml;

      public MergedResult(
        string mergedLoanDataXml,
        List<MergedObject> mergedReport,
        string newBaseLoanDataXml,
        DateTime newBaseLastModTime,
        string baseLoanDataXml,
        string firstLoanDataXml,
        string secondLoanDataXml)
      {
        this.MergedLoanDataXml = mergedLoanDataXml;
        this.MergedReport = mergedReport;
        this.NewBaseLoanDataXml = newBaseLoanDataXml;
        this.NewBaseLastModTime = newBaseLastModTime;
        this.BaseLoanDataXml = baseLoanDataXml;
        this.FirstLoanDataXml = firstLoanDataXml;
        this.SecondLoanDataXml = secondLoanDataXml;
      }
    }
  }
}
