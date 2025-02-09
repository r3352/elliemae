// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.ExportFREData
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Export
{
  public class ExportFREData
  {
    private const string typeName = "ExportFreddieData";
    private static string traceSW = Tracing.SwImportExport;
    private string selectedFormat = string.Empty;

    public ExportFREData(string format)
    {
      this.selectedFormat = format;
      this.selectedFormat = "Freddie";
    }

    public bool Export(LoanDataMgr loanDataMgr, string[] guids)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string str3 = string.Empty;
      using (ExportToLocalDialog exportToLocalDialog = new ExportToLocalDialog("Export to Freddie Mac", "FreddieExport-" + DateTime.Now.ToString("MMddyyy") + ".xml"))
      {
        if (exportToLocalDialog.ShowDialog((IWin32Window) Session.MainScreen) != DialogResult.OK)
          return false;
        str1 = exportToLocalDialog.SelectedFolder;
        str2 = exportToLocalDialog.SelectedFileName;
      }
      for (int index = 0; index < guids.Length; ++index)
      {
        try
        {
          ILoan loan = Session.LoanManager.OpenLoan(guids[index]);
          str3 = new ExportFREData.ExportFREDataHandler(loanDataMgr, loan.GetLoanData(false), this.selectedFormat, str3).Export(this.selectedFormat);
        }
        catch (Exception ex)
        {
          Tracing.Log(ExportFREData.traceSW, "ExportFreddieData", TraceLevel.Error, "Export Freddie file error: " + ex.Message);
        }
      }
      if (!(str3 != string.Empty))
        return false;
      try
      {
        FileStream fileStream = new FileStream(str1 + "\\" + str2, FileMode.Create, FileAccess.Write, FileShare.None);
        byte[] bytes = Encoding.ASCII.GetBytes(str3);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "You do not have access rights to write Freddie file to " + str1 + " folder for Freddie export. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      int num1 = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "The Freddie file has been saved to " + str1 + "\\" + str2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return true;
    }

    internal sealed class ExportFREDataHandler : ExportData
    {
      private string selectedFormat = string.Empty;
      private string xmlRequest = string.Empty;

      public ExportFREDataHandler(
        LoanDataMgr loanDataMgr,
        LoanData loanData,
        string format,
        string request)
        : base(loanDataMgr, loanData)
      {
        this.selectedFormat = format;
        this.xmlRequest = request;
      }

      protected override string exportDataMethodInvoke(IExport export)
      {
        MethodInfo method = export.GetType().GetMethod("ExportData", new System.Type[2]
        {
          typeof (string),
          typeof (string)
        });
        if (method == (MethodInfo) null)
          throw new NotSupportedException("The export does not support export of the current loan.");
        return string.Concat(method.Invoke((object) export, new object[2]
        {
          (object) this.selectedFormat,
          (object) this.xmlRequest
        }));
      }
    }
  }
}
