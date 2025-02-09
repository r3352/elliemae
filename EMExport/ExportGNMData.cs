// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.ExportGNMData
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class ExportGNMData
  {
    private const string typeName = "ExportGinnieData";
    private static string traceSW = Tracing.SwImportExport;
    private string selectedFormat = string.Empty;

    public ExportGNMData(string format) => this.selectedFormat = format;

    public bool Export(LoanDataMgr loanDataMgr, string[] guids)
    {
      if (guids == null || guids.Length == 0)
        return false;
      string str1 = string.Empty;
      string str2 = string.Empty;
      string s = string.Empty;
      using (ExportToLocalDialog exportToLocalDialog = new ExportToLocalDialog("Export to Ginnie Mae", "GinnieExport-" + DateTime.Now.ToString("MMddyyy") + ".xml"))
      {
        if (exportToLocalDialog.ShowDialog((IWin32Window) Session.MainScreen) != DialogResult.OK)
          return false;
        str1 = exportToLocalDialog.SelectedFolder;
        str2 = exportToLocalDialog.SelectedFileName;
      }
      if (guids.Length == 1)
      {
        try
        {
          s = this.GetExportXml(loanDataMgr, guids);
        }
        catch (Exception ex)
        {
          Tracing.Log(ExportGNMData.traceSW, "ExportGinnieData", TraceLevel.Error, "Export Ginnie file error: " + ex.Message);
        }
      }
      else
      {
        try
        {
          s = this.GetExportMultipleXml(guids);
        }
        catch (Exception ex)
        {
          Tracing.Log(ExportGNMData.traceSW, "ExportGinnieData", TraceLevel.Error, "Export Ginnie file error: " + ex.Message);
        }
      }
      if (!(s != string.Empty))
        return false;
      try
      {
        FileStream fileStream = new FileStream(str1 + "\\" + str2, FileMode.Create, FileAccess.Write, FileShare.None);
        byte[] bytes = Encoding.ASCII.GetBytes(s);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "You do not have access rights to write Ginnie file to " + str1 + " folder for Ginnie export. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      int num1 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "The Ginnie file has been saved to " + str1 + "\\" + str2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return true;
    }

    private string GetExportXml(LoanDataMgr loanDataMgr, string[] guids)
    {
      ILoan loan = Session.LoanManager.OpenLoan(guids[0]);
      return new ExportGNMData.ExportGNMDataHandler(loanDataMgr, loan.GetLoanData(false)).Export(this.selectedFormat);
    }

    private string GetExportMultipleXml(string[] guids)
    {
      List<LoanData> loanDataCollection = new List<LoanData>();
      using (Tracing.StartTimer(ExportGNMData.traceSW, "ExportGinnieData", TraceLevel.Info, "Opening Loans"))
      {
        for (int index = 0; index < guids.Length; ++index)
        {
          ILoan loan = Session.LoanManager.OpenLoan(guids[index]);
          loanDataCollection.Add(loan.GetLoanData(false));
        }
      }
      Tracing.Log(ExportGNMData.traceSW, TraceLevel.Info, "ExportGinnieData", "Total Loans: " + loanDataCollection.Count.ToString());
      return new ExportGNMData.ExportGNMDataHandler((IEnumerable<LoanData>) loanDataCollection).ExportMultiple(this.selectedFormat);
    }

    internal sealed class ExportGNMDataHandler : ExportData
    {
      public ExportGNMDataHandler(LoanDataMgr loanDataMgr, LoanData loanData)
        : base(loanDataMgr, loanData)
      {
      }

      public ExportGNMDataHandler(IEnumerable<LoanData> loanDataCollection)
        : base(loanDataCollection)
      {
      }
    }
  }
}
