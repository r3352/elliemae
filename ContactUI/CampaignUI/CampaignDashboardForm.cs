// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignDashboardForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignDashboardForm : Form, ICampaigns
  {
    private const bool FORCE_REFRESH = true;
    private CampaignData campaignData;
    private ContactMainForm frmContactMain;
    private CampaignListForm frmCampaignList;
    private CampaignDetailForm frmCampaignDetail;
    private bool refreshPending;
    private System.ComponentModel.Container components;
    private Panel pnlList;

    public CampaignDashboardForm(ContactMainForm frmContactMain)
    {
      this.frmContactMain = frmContactMain;
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.campaignData.TasksDueChangedEvent += new TasksDueChangedEventHandler(this.campaignData_TasksDueChangedEvent);
      Session.Application.RegisterService((object) this, typeof (ICampaigns));
    }

    public void RefreshCampaignData()
    {
      Session.Application.GetService<IStatusDisplay>().DisplayHelpText("Press F1 for Help");
      this.campaignData.Campaigns.RefreshCampaignList();
      if (this.frmCampaignList != null && this.frmCampaignList.Visible)
        this.frmCampaignList.RefreshCampaignListForm();
      if (this.frmCampaignDetail == null || !this.frmCampaignDetail.Visible)
        return;
      this.frmCampaignDetail.PopulateCampaignDetailForm(true);
    }

    public void LoadCampaignList()
    {
      if (this.frmCampaignList == null)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.frmCampaignList = new CampaignListForm();
        this.frmCampaignList.TopLevel = false;
        this.frmCampaignList.Dock = DockStyle.Fill;
        this.pnlList.Controls.Add((Control) this.frmCampaignList);
        this.frmCampaignList.DisplayDetailEvent += new DisplayDetailEventHandler(this.frmCampaignList_DisplayDetailEvent);
        this.frmCampaignList.CampaignActionRequestEvent += new CampaignActionRequestEventHandler(this.frmCampaignList_CampaignActionRequestEvent);
      }
      if (this.frmCampaignDetail != null)
        this.frmCampaignDetail.Visible = false;
      this.frmCampaignList.Visible = true;
      Cursor.Current = Cursors.Default;
    }

    public void LoadCampaignDetail()
    {
      if (this.frmCampaignDetail == null)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.frmCampaignDetail = new CampaignDetailForm(this.frmCampaignList);
        this.frmCampaignDetail.TopLevel = false;
        this.frmCampaignDetail.Dock = DockStyle.Fill;
        this.pnlList.Controls.Add((Control) this.frmCampaignDetail);
        this.frmCampaignDetail.DisplayListEvent += new DisplayListEventHandler(this.frmCampaignDetail_DisplayListEvent);
        this.frmCampaignDetail.PrevCampaignEvent += new PrevCampaignEventHandler(this.frmCampaignDetail_PrevCampaignEvent);
        this.frmCampaignDetail.NextCampaignEvent += new NextCampaignEventHandler(this.frmCampaignDetail_NextCampaignEvent);
        this.frmCampaignDetail.CampaignActionRequestEvent += new CampaignActionRequestEventHandler(this.frmCampaignList_CampaignActionRequestEvent);
      }
      this.frmCampaignDetail.PopulateCampaignDetailForm(false);
      if (this.frmCampaignList != null)
        this.frmCampaignList.Visible = false;
      this.frmCampaignDetail.Visible = true;
      Cursor.Current = Cursors.Default;
    }

    public bool IsMenuItemEnabled(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          flag = this.frmCampaignList.Visible && this.frmCampaignList.IsMenuItemEnabled(action);
          break;
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
          flag = !this.frmCampaignList.Visible || this.frmCampaignList.IsMenuItemEnabled(action);
          break;
      }
      return flag;
    }

    public bool IsMenuItemVisible(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          flag = !this.frmCampaignList.Visible || this.frmCampaignList.IsMenuItemVisible(action);
          break;
      }
      return flag;
    }

    public void TriggerContactAction(ContactMainForm.ContactsActionEnum action)
    {
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Campaign_NewCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_OpenCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DuplicateCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_DeleteCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StartCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_StopCampaign:
        case ContactMainForm.ContactsActionEnum.Campaign_ManageTemplate:
          this.frmCampaignList.TriggerContactAction(action);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void frmCampaignDetail_DisplayListEvent(object sender, EventArgs e)
    {
      this.displayCampaignList();
    }

    private void displayCampaignList()
    {
      if (this.refreshPending)
      {
        this.frmCampaignList.GetCampaignData();
        this.refreshPending = false;
      }
      this.frmCampaignList.RefreshCampaignListForm();
      this.LoadCampaignList();
    }

    private void frmCampaignList_DisplayDetailEvent(object sender, EventArgs e)
    {
      this.LoadCampaignDetail();
    }

    private void campaignData_TasksDueChangedEvent(object sender, TasksDueEventArgs e)
    {
      this.frmContactMain.SetCampaignsButtonText(e.TasksDue);
    }

    private void frmCampaignDetail_PrevCampaignEvent(object sender, EventArgs e)
    {
      if (this.refreshPending)
      {
        this.frmCampaignList.GetCampaignData();
        this.refreshPending = false;
      }
      this.frmCampaignList.MoveToPrevCampaignNode();
      this.frmCampaignDetail.PopulateCampaignDetailForm(false);
    }

    private void frmCampaignDetail_NextCampaignEvent(object sender, EventArgs e)
    {
      if (this.refreshPending)
      {
        this.frmCampaignList.GetCampaignData();
        this.refreshPending = false;
      }
      this.frmCampaignList.MoveToNextCampaignNode();
      this.frmCampaignDetail.PopulateCampaignDetailForm(false);
    }

    private void frmCampaignList_CampaignActionRequestEvent(
      object sender,
      CampaignActionRequestEventArgs e)
    {
      string actionRequest = e.ActionRequest;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(actionRequest))
      {
        case 336665409:
          if (!(actionRequest == "ManageTemplates"))
            break;
          this.manageTemplates();
          break;
        case 1201677025:
          if (!(actionRequest == "StartCampaign"))
            break;
          this.startCampaign();
          break;
        case 1945223699:
          if (!(actionRequest == "StopCampaign"))
            break;
          this.stopCampaign();
          break;
        case 2051173743:
          if (!(actionRequest == "OpenCampaign"))
            break;
          this.openCampaign();
          break;
        case 2140232380:
          if (!(actionRequest == "DuplicateCampaign"))
            break;
          this.duplicateCampaign();
          break;
        case 4049127352:
          if (!(actionRequest == "DeleteCampaign"))
            break;
          this.deleteCampaign();
          break;
        case 4261451079:
          if (!(actionRequest == "NewCampaign"))
            break;
          this.CreateNew();
          break;
      }
    }

    public void CreateNew()
    {
      if (DialogResult.OK != new CampaignWizardForm(true).ShowDialog((IWin32Window) this.ParentForm))
        return;
      this.campaignData.ActiveCampaign = this.campaignData.WizardCampaign;
      this.campaignData.WizardCampaign = (EllieMae.EMLite.Campaign.Campaign) null;
      this.frmCampaignList.GetCampaignData();
    }

    public bool OpenCampaign(int campaignId)
    {
      this.displayCampaignList();
      if (!this.frmCampaignList.MoveToCampaignNode(campaignId))
        return false;
      this.openCampaign();
      return true;
    }

    private void openCampaign()
    {
      if (CampaignStatus.NotStarted == this.campaignData.ActiveCampaign.Status)
      {
        if (DialogResult.OK != new CampaignWizardForm(false).ShowDialog((IWin32Window) this.ParentForm))
          return;
        this.frmCampaignList.GetCampaignData();
      }
      else
        this.LoadCampaignDetail();
    }

    private void duplicateCampaign()
    {
      if (DialogResult.OK != new CopyCampaignDialog(this.campaignData.ActiveCampaign).ShowDialog())
        return;
      this.frmCampaignList.GetCampaignData();
    }

    private void deleteCampaign()
    {
      if (DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected campaign?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      EllieMae.EMLite.Campaign.Campaign.DeleteCampaign(this.campaignData.ActiveCampaign.CampaignId, Session.SessionObjects);
      this.frmCampaignList.GetCampaignData();
    }

    private void startCampaign()
    {
      if (CampaignStatus.NotStarted != this.campaignData.ActiveCampaign.Status && CampaignStatus.Stopped != this.campaignData.ActiveCampaign.Status)
        return;
      if (CampaignStatus.NotStarted == this.campaignData.ActiveCampaign.Status)
      {
        CampaignStartDialog campaignStartDialog = new CampaignStartDialog();
        int num = (int) campaignStartDialog.ShowDialog();
        if (CampaignStartDialog.SelectionTypes.Cancel == campaignStartDialog.UserSelection)
          return;
      }
      this.campaignData.ActiveCampaign.Start();
      this.frmCampaignList.GetCampaignData();
    }

    private void stopCampaign()
    {
      if (this.campaignData.ActiveCampaign.Status != CampaignStatus.Running || DialogResult.Yes != Utils.Dialog((IWin32Window) this, "You will no longer add contacts or schedule activities for this campaign.\nAre you sure you want to stop the campaign?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      this.campaignData.ActiveCampaign.Stop();
      if (this.frmCampaignList.HidingStoppedCampaigns)
        this.refreshPending = true;
      else
        this.frmCampaignList.GetCampaignData();
    }

    private void manageTemplates()
    {
      int num = (int) new CampaignExplorerDialog(FSExplorer.DialogMode.ManageFiles).ShowDialog((IWin32Window) this);
      this.frmCampaignList.GetCampaignData();
    }

    private void InitializeComponent()
    {
      this.pnlList = new Panel();
      this.SuspendLayout();
      this.pnlList.Dock = DockStyle.Fill;
      this.pnlList.Location = new Point(0, 0);
      this.pnlList.Name = "pnlList";
      this.pnlList.Size = new Size(816, 446);
      this.pnlList.TabIndex = 1;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(816, 446);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlList);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignDashboardForm);
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
    }
  }
}
