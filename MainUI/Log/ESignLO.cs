// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ESignLO
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.eFolder.LoanCenter;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ESignLO
  {
    private IDisclosureTracking2015Log _disclosureLog;
    private ESignResult _eSignResult;
    private readonly EBSServiceClient _ebsClient;
    private readonly string _loanGuid;
    private const string ESignTaskId = "ESign";

    public ESignLO(string loanGuid)
    {
      this._loanGuid = loanGuid.Replace("{", string.Empty).Replace("}", string.Empty);
      this._ebsClient = new EBSServiceClient();
    }

    public async Task<ESignResult> ShowESignDialog(
      IDisclosureTracking2015Log log,
      Form parentForm,
      string userId = "")
    {
      ESignLO esignLo = this;
      using (PerformanceMeter.StartNew("eSgnLOShoweSigDlg", "DOCS eSignLO.ShowESignDialog", 40, nameof (ShowESignDialog), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs"))
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 42, nameof (ShowESignDialog), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs");
        esignLo._disclosureLog = log;
        Request request = new Request()
        {
          caller = new Caller()
          {
            name = EnGlobalSettings.ApplicationName,
            version = VersionInformation.CurrentVersion.DisplayVersion
          },
          applicationGroupId = esignLo._loanGuid,
          packageId = log.eDisclosurePackageID
        };
        EDeliveryServiceClient edeliveryServiceClient = new EDeliveryServiceClient();
        string empty = string.Empty;
        string envelopeSignerUrl;
        if (log is EnhancedDisclosureTracking2015Log)
          envelopeSignerUrl = await edeliveryServiceClient.GenerateEnvelopeSignerURL(request, userId);
        else
          envelopeSignerUrl = await edeliveryServiceClient.GenerateEnvelopeSignerURL(request, log.eDisclosureLOUserId);
        DocuSignSigningDialog signSigningDialog1 = new DocuSignSigningDialog(envelopeSignerUrl);
        PerformanceMeter.Current.AddCheckpoint("new DocuSignSigningDialog", 64, nameof (ShowESignDialog), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs");
        Screen screen = Screen.FromControl((Control) parentForm);
        DocuSignSigningDialog signSigningDialog2 = signSigningDialog1;
        Rectangle workingArea = screen.WorkingArea;
        int width = workingArea.Width;
        workingArea = screen.WorkingArea;
        int height = workingArea.Height;
        Size size = new Size(width, height);
        signSigningDialog2.Size = size;
        DocuSignSigningDialog signSigningDialog3 = signSigningDialog1;
        workingArea = screen.WorkingArea;
        Point location = workingArea.Location;
        signSigningDialog3.Location = location;
        signSigningDialog1.Closing += new CancelEventHandler(esignLo.DocuSignForm_Closing);
        PerformanceMeter.Current.AddCheckpoint("BEFORE ShowDialog", 71, nameof (ShowESignDialog), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs");
        int num = (int) signSigningDialog1.ShowDialog((IWin32Window) parentForm);
        PerformanceMeter.Current.AddCheckpoint("AFTER ShowDialog", 73, nameof (ShowESignDialog), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs");
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 75, nameof (ShowESignDialog), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs");
      }
      return esignLo._eSignResult;
    }

    private void DocuSignForm_Closing(object sender, EventArgs e)
    {
      using (PerformanceMeter.StartNew("eSgnLODcuSnFrmCls", "DOCS eSign_LO.DocuSignForm_Closing", 91, nameof (DocuSignForm_Closing), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\Log\\DisclosureTracking2015\\eSignLO.cs"))
      {
        if (!(sender is DocuSignSigningDialog signSigningDialog))
          return;
        this._eSignResult = signSigningDialog.ESignResult;
        if (this._eSignResult == ESignResult.Complete && this._disclosureLog.eDisclosureLOeSignedDate == DateTime.MinValue)
        {
          this.UpdatePackageESignComplete();
        }
        else
        {
          if (this._eSignResult != ESignResult.Viewed || !(this._disclosureLog.eDisclosureLOViewMessageDate == DateTime.MinValue))
            return;
          this.UpdatePackageViewed();
        }
      }
    }

    private void UpdatePackageESignComplete()
    {
      this._disclosureLog.eDisclosureLOeSignedDate = !(this._disclosureLog is DisclosureTracking2015Log) ? (!(this._disclosureLog is EnhancedDisclosureTracking2015Log) ? DateTime.Now : ((EnhancedDisclosureTracking2015Log) this._disclosureLog).ConvertToDateTimeWithZone(DateTime.UtcNow).DateTime) : ((DisclosureTrackingBase) this._disclosureLog).ConvertToLoanTimeZone(DateTime.UtcNow);
      PackageTask packageTask = this.CreatePackageTask();
      packageTask.CompletedDate = new DateTime?(this._disclosureLog.eDisclosureLOeSignedDate);
      new EDeliveryServiceClient().UpdatePackageTask(packageTask, true);
    }

    private void UpdatePackageViewed()
    {
      this._disclosureLog.eDisclosureLOViewMessageDate = !(this._disclosureLog is DisclosureTracking2015Log) ? (!(this._disclosureLog is EnhancedDisclosureTracking2015Log) ? DateTime.Now : ((EnhancedDisclosureTracking2015Log) this._disclosureLog).ConvertToDateTimeWithZone(DateTime.UtcNow).DateTime) : ((DisclosureTrackingBase) this._disclosureLog).ConvertToLoanTimeZone(DateTime.UtcNow);
      PackageTask packageTask = this.CreatePackageTask();
      packageTask.ViewedDate = new DateTime?(this._disclosureLog.eDisclosureLOViewMessageDate);
      new EDeliveryServiceClient().UpdatePackageTask(packageTask, true);
    }

    private PackageTask CreatePackageTask()
    {
      PackageTask packageTask = new PackageTask();
      packageTask.Caller = new Caller()
      {
        name = EnGlobalSettings.ApplicationName,
        version = VersionInformation.CurrentVersion.DisplayVersion
      };
      packageTask.ApplicationGroupId = this._loanGuid;
      packageTask.PackageId = this._disclosureLog.eDisclosurePackageID;
      packageTask.TaskId = "ESign";
      packageTask.RecipientId = this._disclosureLog.eDisclosureLOUserId;
      return packageTask;
    }
  }
}
