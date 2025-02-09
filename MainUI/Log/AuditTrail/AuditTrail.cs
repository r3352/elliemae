// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AuditTrail.AuditTrail
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.SkyDrive;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log.AuditTrail
{
  public static class AuditTrail
  {
    private static readonly string sw = Tracing.SwInputEngine;
    private const string className = "AuditTrail";

    public static void ViewAuditTrail(string loanGuid, string packageId)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        DMOSServiceClient dmosServiceClient = new DMOSServiceClient();
        if (string.IsNullOrEmpty(packageId))
        {
          int num1 = (int) MessageBox.Show("Cannot run Audit Trail. No Package Id for this Disclosure Log.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          Task<GetDMOSAuditTrailResponse> auditTrail = dmosServiceClient.GetAuditTrail(loanGuid, packageId);
          Task.WaitAll((Task) auditTrail);
          GetDMOSAuditTrailResponse result = auditTrail.Result;
          if (result == null || result.files == null || ((IEnumerable<AuditFile>) result.files).Count<AuditFile>() == 0)
          {
            int num2 = (int) MessageBox.Show("No files returned from the Audit Trail service.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            List<PdfFile> pdfFileList = new List<PdfFile>();
            foreach (AuditFile file in result.files)
            {
              if (file.url.source.ToLower().Trim() == "skydrive")
              {
                Task<string> task = new SkyDriveStreamingClient(Session.LoanDataMgr).DownloadFile(new SkyDriveUrl((string) null, file.url.location, file.url.signature), file.filename);
                Task.WaitAll((Task) task);
                pdfFileList.Add(new PdfFile()
                {
                  Title = file.title,
                  Filepath = task.Result
                });
              }
              else
              {
                Task<string> task = new EDeliveryServiceClient(Session.LoanDataMgr).DownloadServerFile(file.url.location, file.filename);
                pdfFileList.Add(new PdfFile()
                {
                  Title = file.title,
                  Filepath = task.Result
                });
              }
            }
            if (pdfFileList.Count <= 0)
              return;
            using (PdfListPreviewDialog listPreviewDialog = new PdfListPreviewDialog("Audit Trail Documents", pdfFileList.ToArray()))
            {
              PerformanceMeter.Current.AddCheckpoint("new PdfListPreviewDialog", 88, nameof (ViewAuditTrail), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\AuditTrail\\AuditTrail.cs");
              PerformanceMeter.Current.AddCheckpoint("BEFORE PdfListPreviewDialog ShowDialog", 89, nameof (ViewAuditTrail), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\AuditTrail\\AuditTrail.cs");
              PerformanceMeter.Current.AddCheckpoint("AFTER PdfListPreviewDialog ShowDialog - " + listPreviewDialog.ShowDialog((IWin32Window) Form.ActiveForm).ToString(), 91, nameof (ViewAuditTrail), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\AuditTrail\\AuditTrail.cs");
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(EllieMae.EMLite.Log.AuditTrail.AuditTrail.sw, TraceLevel.Error, nameof (AuditTrail), "Audit Trail Error: " + (object) ex);
        if (EllieMae.EMLite.Log.AuditTrail.AuditTrail.EdeliveryNotFoundException(ex))
        {
          int num3 = (int) MessageBox.Show("Audit Trail is not available for this package.", "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num4 = (int) MessageBox.Show("Something went wrong. Please close this window and relaunch the Audit Trail.\r\nDetails:\r\n" + ex.Message + "\r\n\r\nInner Exception:\r\n" + ex.InnerException.Message, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private static bool EdeliveryNotFoundException(Exception ex)
    {
      bool flag = false;
      if (ex != null && ex.InnerException != null)
      {
        string message = ex.InnerException.Message;
        flag = (message != null ? (message.IndexOf("EDELIVERY4000", StringComparison.OrdinalIgnoreCase) > -1 ? 1 : 0) : 0) != 0;
      }
      return flag;
    }
  }
}
