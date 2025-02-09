// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.StatusOnline.StatusOnlineDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.eFolder.eDelivery;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.StatusOnline;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.StatusOnline
{
  public class StatusOnlineDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private StatusOnlineLoanSetup statusOnlineLoanSetup;
    private GridViewDataManager gvUnpublishedWCMgr;
    private GridViewDataManager gvUnpublishedTPOMgr;
    private GridViewDataManager gvPublishedWCMgr;
    private GridViewDataManager gvPublishedTPOMgr;
    private IContainer components;
    private ToolTip tooltip;
    private GroupContainer gcStatusOnline;
    private CheckBox chkBoxNoPrompt;
    private Button btnClose;
    private Button btnPublishWC;
    private GroupContainer gcPublishedWC;
    private GridView gvPublishedWC;
    private GroupContainer gcUnpublishedWC;
    private GridView gvUnpublishedWC;
    private StandardIconButton btnAddWC;
    private GradientPanel gpHeader;
    private Label lblHeader;
    private EMHelpLink emHelpLink;
    private VerticalSeparator separatorWC;
    private Panel pnlBottom;
    private Panel pnlPortal;
    private Panel pnlSplitWC;
    private TabControl tabPortal;
    private TabPage pageWC;
    private TabPage pageTPO;
    private FlowLayoutPanel pnlToolbarWC;
    private GroupContainer gcPublishedTPO;
    private GridView gvPublishedTPO;
    private Panel pnlSplitTPO;
    private GroupContainer gcUnpublishedTPO;
    private FlowLayoutPanel pnlToolbarTPO;
    private Button btnPublishTPO;
    private VerticalSeparator separatorTPO;
    private StandardIconButton btnAddTPO;
    private GridView gvUnpublishedTPO;

    public StatusOnlineDialog(LoanDataMgr loanDataMgr, StatusOnlineLoanSetup statusOnlineLoanSetup)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.statusOnlineLoanSetup = statusOnlineLoanSetup;
      this.chkBoxNoPrompt.Checked = !statusOnlineLoanSetup.ShowPrompt;
      this.initTriggerLists();
      this.loadTriggerLists();
      this.applySecurity();
      this.loanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.85);
        this.Height = Convert.ToInt32((double) form.Height * 0.85);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.85);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.85);
      }
    }

    private void applySecurity()
    {
      if (!Session.UserInfo.PersonalStatusOnline)
      {
        this.btnAddWC.Visible = false;
        this.separatorWC.Visible = false;
        this.btnAddTPO.Visible = false;
        this.separatorTPO.Visible = false;
      }
      string str = (string) null;
      try
      {
        str = this.loanDataMgr.LoanData.GetField("TPO.X1");
      }
      catch
      {
      }
      if (!string.IsNullOrEmpty(str) || StatusOnlineManager.IsTPOConnectLoan(this.loanDataMgr))
        return;
      this.tabPortal.TabPages.Remove(this.pageTPO);
    }

    private void initTriggerLists()
    {
      this.gvUnpublishedWCMgr = new GridViewDataManager(this.gvUnpublishedWC, this.loanDataMgr);
      this.gvUnpublishedTPOMgr = new GridViewDataManager(this.gvUnpublishedTPO, this.loanDataMgr);
      this.gvPublishedWCMgr = new GridViewDataManager(this.gvPublishedWC, this.loanDataMgr);
      this.gvPublishedTPOMgr = new GridViewDataManager(this.gvPublishedTPO, this.loanDataMgr);
      TableLayout.Column[] columnList1 = new TableLayout.Column[6]
      {
        GridViewDataManager.CheckBoxColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.EmailRequiredColumn,
        GridViewDataManager.UpdateMethodColumn,
        GridViewDataManager.StatusTriggerColumn
      };
      TableLayout.Column[] columnList2 = new TableLayout.Column[6]
      {
        GridViewDataManager.CheckBoxColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.EmailRequiredColumn,
        GridViewDataManager.UpdateMethodColumn,
        GridViewDataManager.StatusTriggerColumn
      };
      TableLayout.Column[] columnList3 = new TableLayout.Column[3]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.EmailRequiredColumn
      };
      TableLayout.Column[] columnList4 = new TableLayout.Column[3]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.EmailRequiredColumn
      };
      this.gvUnpublishedWCMgr.CreateLayout(columnList1);
      this.gvUnpublishedTPOMgr.CreateLayout(columnList2);
      this.gvPublishedWCMgr.CreateLayout(columnList3);
      this.gvPublishedTPOMgr.CreateLayout(columnList4);
      this.gvUnpublishedWC.Columns[2].ActivatedEditorType = GVActivatedEditorType.DatePicker;
      this.gvUnpublishedTPO.Columns[2].ActivatedEditorType = GVActivatedEditorType.DatePicker;
      this.gvPublishedWC.Sort(0, SortOrder.Descending);
      this.gvPublishedTPO.Sort(0, SortOrder.Descending);
    }

    private void loadTriggerLists()
    {
      foreach (StatusOnlineTrigger trigger in (CollectionBase) this.statusOnlineLoanSetup.Triggers)
      {
        if (trigger.PortalType == TriggerPortalType.WebCenter)
        {
          if (trigger.DatePublished == DateTime.MinValue)
          {
            if (!this.gvUnpublishedWC.Items.ContainsTag((object) trigger))
            {
              GVItem gvItem = this.gvUnpublishedWCMgr.AddItem(trigger, (HtmlEmailTemplate) null);
              if (trigger.DateTriggered != DateTime.MinValue || trigger.RequirementType == TriggerRequirementType.None)
                gvItem.Checked = true;
            }
          }
          else if (!this.gvPublishedWC.Items.ContainsTag((object) trigger))
            this.gvPublishedWCMgr.AddItem(trigger, (HtmlEmailTemplate) null);
        }
        else if (trigger.PortalType == TriggerPortalType.TPOWC)
        {
          if (trigger.DatePublished == DateTime.MinValue)
          {
            if (!this.gvUnpublishedTPO.Items.ContainsTag((object) trigger))
            {
              GVItem gvItem = this.gvUnpublishedTPOMgr.AddItem(trigger, (HtmlEmailTemplate) null);
              if (trigger.DateTriggered != DateTime.MinValue || trigger.RequirementType == TriggerRequirementType.None)
                gvItem.Checked = true;
            }
          }
          else if (!this.gvPublishedTPO.Items.ContainsTag((object) trigger))
            this.gvPublishedTPOMgr.AddItem(trigger, (HtmlEmailTemplate) null);
        }
      }
      this.gvPublishedWC.ReSort();
      this.gvPublishedTPO.ReSort();
    }

    private List<StatusOnlineTrigger> getSelectedTriggers(GridView gridview)
    {
      List<StatusOnlineTrigger> selectedTriggers = new List<StatusOnlineTrigger>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) gridview.Items)
      {
        if (gvItem.Checked)
        {
          StatusOnlineTrigger tag = (StatusOnlineTrigger) gvItem.Tag;
          selectedTriggers.Add(tag);
        }
      }
      return selectedTriggers;
    }

    private void btnAddWC_Click(object sender, EventArgs e)
    {
      using (StatusOnlineTriggerDialog onlineTriggerDialog = new StatusOnlineTriggerDialog(this.loanDataMgr, new StatusOnlineTrigger(Session.UserID, TriggerPortalType.WebCenter)))
      {
        if (onlineTriggerDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.statusOnlineLoanSetup = onlineTriggerDialog.LoanTriggerSetup;
        this.loadTriggerLists();
      }
    }

    private void btnAddTPO_Click(object sender, EventArgs e)
    {
      using (StatusOnlineTriggerDialog onlineTriggerDialog = new StatusOnlineTriggerDialog(this.loanDataMgr, new StatusOnlineTrigger(Session.UserID, TriggerPortalType.TPOWC)))
      {
        if (onlineTriggerDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.statusOnlineLoanSetup = onlineTriggerDialog.LoanTriggerSetup;
        this.loadTriggerLists();
      }
    }

    private void btnPublishWC_Click(object sender, EventArgs e)
    {
      List<StatusOnlineTrigger> selectedTriggers = this.getSelectedTriggers(this.gvUnpublishedWC);
      if (selectedTriggers.Count > 0)
      {
        List<StatusOnlineTrigger> publishedTriggers = new List<StatusOnlineTrigger>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPublishedWC.Items)
        {
          StatusOnlineTrigger tag = (StatusOnlineTrigger) gvItem.Tag;
          publishedTriggers.Add(tag);
        }
        bool flag = false;
        string str = "The status online options were successfully published";
        if (this.loanDataMgr.IsPlatformLoan(true, true))
        {
          flag = StatusOnlineManager.PublishCCTriggers(this.loanDataMgr, this.statusOnlineLoanSetup, selectedTriggers, publishedTriggers, !this.chkBoxNoPrompt.Checked);
          str = this.loanDataMgr.SessionObjects.StartupInfo.OtpSupport ? this.GetMessageForThirdPartyUsers(str, selectedTriggers) : str;
        }
        else if (!string.IsNullOrEmpty(this.loanDataMgr.WCNotAllowedMessage))
        {
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, this.loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (!flag)
          return;
        int num1 = (int) Utils.Dialog((IWin32Window) this, str, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select item(s) to Publish", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnPublishTPO_Click(object sender, EventArgs e)
    {
      List<StatusOnlineTrigger> selectedTriggers = this.getSelectedTriggers(this.gvUnpublishedTPO);
      if (selectedTriggers.Count > 0)
      {
        List<StatusOnlineTrigger> publishedTriggers = new List<StatusOnlineTrigger>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPublishedTPO.Items)
        {
          StatusOnlineTrigger tag = (StatusOnlineTrigger) gvItem.Tag;
          publishedTriggers.Add(tag);
        }
        if (!StatusOnlineManager.PublishTriggers(this.loanDataMgr, this.statusOnlineLoanSetup, selectedTriggers, publishedTriggers, !this.chkBoxNoPrompt.Checked))
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "The status online options were successfully published to TPO WebCenter", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select item(s) to Publish", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void gvUnpublishedWC_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      DatePicker editorControl = (DatePicker) e.EditorControl;
      this.gvUnpublishedWC.CancelEditing();
      StatusOnlineTrigger tag = (StatusOnlineTrigger) e.SubItem.Item.Tag;
      tag.DateTriggered = editorControl.Value;
      this.gvUnpublishedWCMgr.RefreshItem(e.SubItem.Item, tag, (HtmlEmailTemplate) null);
    }

    private void gvUnpublishedTPO_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      DatePicker editorControl = (DatePicker) e.EditorControl;
      this.gvUnpublishedTPO.CancelEditing();
      StatusOnlineTrigger tag = (StatusOnlineTrigger) e.SubItem.Item.Tag;
      tag.DateTriggered = editorControl.Value;
      this.gvUnpublishedTPOMgr.RefreshItem(e.SubItem.Item, tag, (HtmlEmailTemplate) null);
    }

    private void pageWC_ClientSizeChanged(object sender, EventArgs e)
    {
      int num = (this.pageWC.ClientSize.Height - this.pnlSplitWC.Height) / 2;
      if (num <= 0)
        return;
      this.gcUnpublishedWC.Height = num;
    }

    private void pageTPO_ClientSizeChanged(object sender, EventArgs e)
    {
      int num = (this.pageTPO.ClientSize.Height - this.pnlSplitTPO.Height) / 2;
      if (num <= 0)
        return;
      this.gcUnpublishedTPO.Height = num;
    }

    private void StatusOnlineDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      bool showPrompt = !this.chkBoxNoPrompt.Checked;
      if (this.statusOnlineLoanSetup.ShowPrompt == showPrompt)
        return;
      Session.LoanManager.SetStatusOnlinePrompt(Session.LoanManager.GetLoanIdentity(Session.LoanData.GUID), showPrompt);
    }

    private void StatusOnlineDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.emHelpLink.HelpTag);
    }

    private string GetMessageForThirdPartyUsers(
      string confirmationMessage,
      List<StatusOnlineTrigger> triggerList)
    {
      if (triggerList.Any<StatusOnlineTrigger>((Func<StatusOnlineTrigger, bool>) (x => x.EmailRecipients != null && ((IEnumerable<string>) x.EmailRecipients).Any<string>((Func<string, bool>) (y => y == "VEND.X141")) && x.UpdateType == TriggerUpdateType.Automatic)) && !this.ThirdpartyUsersAuthenticated())
        confirmationMessage = confirmationMessage + "\n\n" + "The authentication code(s) for 3rd party signer(s) can be viewed in the packages tab of the eFolder.";
      return confirmationMessage;
    }

    private bool ThirdpartyUsersAuthenticated()
    {
      Task<List<RecipientDetails>> recipientDetails = new EBSServiceClient().GetRecipientDetails(this.loanDataMgr.LoanData.GetField("GUID"));
      Task.WaitAll((Task) recipientDetails);
      List<RecipientDetails> result = recipientDetails.Result;
      return result != null && result.Count > 0 && !result.Where<RecipientDetails>((Func<RecipientDetails, bool>) (x => x.contactType.ToLower() == "buyer's agent")).Any<RecipientDetails>((Func<RecipientDetails, bool>) (y => !y.isAuthenticated));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.tooltip = new ToolTip(this.components);
      this.btnAddWC = new StandardIconButton();
      this.btnAddTPO = new StandardIconButton();
      this.pnlBottom = new Panel();
      this.emHelpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.gcStatusOnline = new GroupContainer();
      this.pnlPortal = new Panel();
      this.tabPortal = new TabControl();
      this.pageWC = new TabPage();
      this.gcPublishedWC = new GroupContainer();
      this.gvPublishedWC = new GridView();
      this.pnlSplitWC = new Panel();
      this.gcUnpublishedWC = new GroupContainer();
      this.pnlToolbarWC = new FlowLayoutPanel();
      this.btnPublishWC = new Button();
      this.separatorWC = new VerticalSeparator();
      this.gvUnpublishedWC = new GridView();
      this.pageTPO = new TabPage();
      this.gcPublishedTPO = new GroupContainer();
      this.gvPublishedTPO = new GridView();
      this.pnlSplitTPO = new Panel();
      this.gcUnpublishedTPO = new GroupContainer();
      this.pnlToolbarTPO = new FlowLayoutPanel();
      this.btnPublishTPO = new Button();
      this.separatorTPO = new VerticalSeparator();
      this.gvUnpublishedTPO = new GridView();
      this.chkBoxNoPrompt = new CheckBox();
      this.gpHeader = new GradientPanel();
      this.lblHeader = new Label();
      ((ISupportInitialize) this.btnAddWC).BeginInit();
      ((ISupportInitialize) this.btnAddTPO).BeginInit();
      this.pnlBottom.SuspendLayout();
      this.gcStatusOnline.SuspendLayout();
      this.pnlPortal.SuspendLayout();
      this.tabPortal.SuspendLayout();
      this.pageWC.SuspendLayout();
      this.gcPublishedWC.SuspendLayout();
      this.gcUnpublishedWC.SuspendLayout();
      this.pnlToolbarWC.SuspendLayout();
      this.pageTPO.SuspendLayout();
      this.gcPublishedTPO.SuspendLayout();
      this.gcUnpublishedTPO.SuspendLayout();
      this.pnlToolbarTPO.SuspendLayout();
      this.gpHeader.SuspendLayout();
      this.SuspendLayout();
      this.btnAddWC.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddWC.BackColor = Color.Transparent;
      this.btnAddWC.Location = new Point(15, 3);
      this.btnAddWC.Margin = new Padding(4, 3, 0, 3);
      this.btnAddWC.MouseDownImage = (Image) null;
      this.btnAddWC.Name = "btnAddWC";
      this.btnAddWC.Size = new Size(16, 16);
      this.btnAddWC.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddWC.TabIndex = 2;
      this.btnAddWC.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddWC, "Add Status");
      this.btnAddWC.Click += new EventHandler(this.btnAddWC_Click);
      this.btnAddTPO.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddTPO.BackColor = Color.Transparent;
      this.btnAddTPO.Location = new Point(15, 3);
      this.btnAddTPO.Margin = new Padding(4, 3, 0, 3);
      this.btnAddTPO.MouseDownImage = (Image) null;
      this.btnAddTPO.Name = "btnAddTPO";
      this.btnAddTPO.Size = new Size(16, 16);
      this.btnAddTPO.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddTPO.TabIndex = 2;
      this.btnAddTPO.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddTPO, "Add Status");
      this.btnAddTPO.Click += new EventHandler(this.btnAddTPO_Click);
      this.pnlBottom.Controls.Add((Control) this.emHelpLink);
      this.pnlBottom.Controls.Add((Control) this.btnClose);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 377);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(657, 33);
      this.pnlBottom.TabIndex = 1;
      this.emHelpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink.BackColor = Color.Transparent;
      this.emHelpLink.Cursor = Cursors.Hand;
      this.emHelpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink.HelpTag = "Status Online Tool";
      this.emHelpLink.Location = new Point(8, 8);
      this.emHelpLink.Name = "emHelpLink";
      this.emHelpLink.Size = new Size(90, 16);
      this.emHelpLink.TabIndex = 1;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(579, 4);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(72, 22);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.gcStatusOnline.Controls.Add((Control) this.pnlPortal);
      this.gcStatusOnline.Controls.Add((Control) this.chkBoxNoPrompt);
      this.gcStatusOnline.Controls.Add((Control) this.gpHeader);
      this.gcStatusOnline.Dock = DockStyle.Fill;
      this.gcStatusOnline.HeaderForeColor = SystemColors.ControlText;
      this.gcStatusOnline.Location = new Point(0, 0);
      this.gcStatusOnline.Name = "gcStatusOnline";
      this.gcStatusOnline.Size = new Size(657, 377);
      this.gcStatusOnline.TabIndex = 0;
      this.gcStatusOnline.Text = "Status Online";
      this.pnlPortal.Controls.Add((Control) this.tabPortal);
      this.pnlPortal.Dock = DockStyle.Fill;
      this.pnlPortal.Location = new Point(1, 57);
      this.pnlPortal.Name = "pnlPortal";
      this.pnlPortal.Padding = new Padding(4, 4, 2, 2);
      this.pnlPortal.Size = new Size(655, 319);
      this.pnlPortal.TabIndex = 2;
      this.tabPortal.Controls.Add((Control) this.pageWC);
      this.tabPortal.Controls.Add((Control) this.pageTPO);
      this.tabPortal.Dock = DockStyle.Fill;
      this.tabPortal.HotTrack = true;
      this.tabPortal.ItemSize = new Size(74, 28);
      this.tabPortal.Location = new Point(4, 4);
      this.tabPortal.Margin = new Padding(0);
      this.tabPortal.Name = "tabPortal";
      this.tabPortal.Padding = new Point(11, 3);
      this.tabPortal.SelectedIndex = 0;
      this.tabPortal.Size = new Size(649, 313);
      this.tabPortal.TabIndex = 3;
      this.pageWC.Controls.Add((Control) this.gcPublishedWC);
      this.pageWC.Controls.Add((Control) this.pnlSplitWC);
      this.pageWC.Controls.Add((Control) this.gcUnpublishedWC);
      this.pageWC.Location = new Point(4, 32);
      this.pageWC.Name = "pageWC";
      this.pageWC.Padding = new Padding(0, 2, 2, 2);
      this.pageWC.Size = new Size(641, 277);
      this.pageWC.TabIndex = 0;
      this.pageWC.Text = " WebCenter";
      this.pageWC.UseVisualStyleBackColor = true;
      this.pageWC.ClientSizeChanged += new EventHandler(this.pageWC_ClientSizeChanged);
      this.gcPublishedWC.Controls.Add((Control) this.gvPublishedWC);
      this.gcPublishedWC.Dock = DockStyle.Fill;
      this.gcPublishedWC.HeaderForeColor = SystemColors.ControlText;
      this.gcPublishedWC.Location = new Point(0, 143);
      this.gcPublishedWC.Name = "gcPublishedWC";
      this.gcPublishedWC.Size = new Size(639, 132);
      this.gcPublishedWC.TabIndex = 2;
      this.gcPublishedWC.Text = "Already Published";
      this.gvPublishedWC.BorderStyle = BorderStyle.None;
      this.gvPublishedWC.Dock = DockStyle.Fill;
      this.gvPublishedWC.Location = new Point(1, 26);
      this.gvPublishedWC.Name = "gvPublishedWC";
      this.gvPublishedWC.Size = new Size(637, 105);
      this.gvPublishedWC.TabIndex = 0;
      this.gvPublishedWC.TextTrimming = StringTrimming.EllipsisCharacter;
      this.pnlSplitWC.Dock = DockStyle.Top;
      this.pnlSplitWC.Location = new Point(0, 139);
      this.pnlSplitWC.Name = "pnlSplitWC";
      this.pnlSplitWC.Size = new Size(639, 4);
      this.pnlSplitWC.TabIndex = 1;
      this.gcUnpublishedWC.Controls.Add((Control) this.pnlToolbarWC);
      this.gcUnpublishedWC.Controls.Add((Control) this.gvUnpublishedWC);
      this.gcUnpublishedWC.Dock = DockStyle.Top;
      this.gcUnpublishedWC.HeaderForeColor = SystemColors.ControlText;
      this.gcUnpublishedWC.Location = new Point(0, 2);
      this.gcUnpublishedWC.Name = "gcUnpublishedWC";
      this.gcUnpublishedWC.Size = new Size(639, 137);
      this.gcUnpublishedWC.TabIndex = 0;
      this.gcUnpublishedWC.Text = "Select to Publish";
      this.pnlToolbarWC.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbarWC.BackColor = Color.Transparent;
      this.pnlToolbarWC.Controls.Add((Control) this.btnPublishWC);
      this.pnlToolbarWC.Controls.Add((Control) this.separatorWC);
      this.pnlToolbarWC.Controls.Add((Control) this.btnAddWC);
      this.pnlToolbarWC.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbarWC.Location = new Point(535, 2);
      this.pnlToolbarWC.Name = "pnlToolbarWC";
      this.pnlToolbarWC.Size = new Size(100, 22);
      this.pnlToolbarWC.TabIndex = 3;
      this.btnPublishWC.BackColor = Color.Transparent;
      this.btnPublishWC.Location = new Point(40, 0);
      this.btnPublishWC.Margin = new Padding(0);
      this.btnPublishWC.Name = "btnPublishWC";
      this.btnPublishWC.Size = new Size(60, 22);
      this.btnPublishWC.TabIndex = 0;
      this.btnPublishWC.Text = "Publish";
      this.btnPublishWC.UseVisualStyleBackColor = true;
      this.btnPublishWC.Click += new EventHandler(this.btnPublishWC_Click);
      this.separatorWC.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separatorWC.Location = new Point(35, 3);
      this.separatorWC.Margin = new Padding(4, 3, 3, 3);
      this.separatorWC.MaximumSize = new Size(2, 16);
      this.separatorWC.MinimumSize = new Size(2, 16);
      this.separatorWC.Name = "separatorWC";
      this.separatorWC.Size = new Size(2, 16);
      this.separatorWC.TabIndex = 2;
      this.gvUnpublishedWC.BorderStyle = BorderStyle.None;
      this.gvUnpublishedWC.Dock = DockStyle.Fill;
      this.gvUnpublishedWC.Location = new Point(1, 26);
      this.gvUnpublishedWC.Name = "gvUnpublishedWC";
      this.gvUnpublishedWC.Size = new Size(637, 110);
      this.gvUnpublishedWC.TabIndex = 1;
      this.gvUnpublishedWC.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvUnpublishedWC.EditorClosing += new GVSubItemEditingEventHandler(this.gvUnpublishedWC_EditorClosing);
      this.pageTPO.Controls.Add((Control) this.gcPublishedTPO);
      this.pageTPO.Controls.Add((Control) this.pnlSplitTPO);
      this.pageTPO.Controls.Add((Control) this.gcUnpublishedTPO);
      this.pageTPO.Location = new Point(4, 32);
      this.pageTPO.Name = "pageTPO";
      this.pageTPO.Padding = new Padding(0, 2, 2, 2);
      this.pageTPO.Size = new Size(641, 277);
      this.pageTPO.TabIndex = 1;
      this.pageTPO.Text = "TPO WebCenter";
      this.pageTPO.UseVisualStyleBackColor = true;
      this.pageTPO.ClientSizeChanged += new EventHandler(this.pageTPO_ClientSizeChanged);
      this.gcPublishedTPO.Controls.Add((Control) this.gvPublishedTPO);
      this.gcPublishedTPO.Dock = DockStyle.Fill;
      this.gcPublishedTPO.HeaderForeColor = SystemColors.ControlText;
      this.gcPublishedTPO.Location = new Point(0, 143);
      this.gcPublishedTPO.Name = "gcPublishedTPO";
      this.gcPublishedTPO.Size = new Size(639, 132);
      this.gcPublishedTPO.TabIndex = 5;
      this.gcPublishedTPO.Text = "Already Published";
      this.gvPublishedTPO.BorderStyle = BorderStyle.None;
      this.gvPublishedTPO.Dock = DockStyle.Fill;
      this.gvPublishedTPO.Location = new Point(1, 26);
      this.gvPublishedTPO.Name = "gvPublishedTPO";
      this.gvPublishedTPO.Size = new Size(637, 105);
      this.gvPublishedTPO.TabIndex = 0;
      this.gvPublishedTPO.TextTrimming = StringTrimming.EllipsisCharacter;
      this.pnlSplitTPO.Dock = DockStyle.Top;
      this.pnlSplitTPO.Location = new Point(0, 139);
      this.pnlSplitTPO.Name = "pnlSplitTPO";
      this.pnlSplitTPO.Size = new Size(639, 4);
      this.pnlSplitTPO.TabIndex = 4;
      this.gcUnpublishedTPO.Controls.Add((Control) this.pnlToolbarTPO);
      this.gcUnpublishedTPO.Controls.Add((Control) this.gvUnpublishedTPO);
      this.gcUnpublishedTPO.Dock = DockStyle.Top;
      this.gcUnpublishedTPO.HeaderForeColor = SystemColors.ControlText;
      this.gcUnpublishedTPO.Location = new Point(0, 2);
      this.gcUnpublishedTPO.Name = "gcUnpublishedTPO";
      this.gcUnpublishedTPO.Size = new Size(639, 137);
      this.gcUnpublishedTPO.TabIndex = 3;
      this.gcUnpublishedTPO.Text = "Select to Publish";
      this.pnlToolbarTPO.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbarTPO.BackColor = Color.Transparent;
      this.pnlToolbarTPO.Controls.Add((Control) this.btnPublishTPO);
      this.pnlToolbarTPO.Controls.Add((Control) this.separatorTPO);
      this.pnlToolbarTPO.Controls.Add((Control) this.btnAddTPO);
      this.pnlToolbarTPO.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbarTPO.Location = new Point(535, 2);
      this.pnlToolbarTPO.Name = "pnlToolbarTPO";
      this.pnlToolbarTPO.Size = new Size(100, 22);
      this.pnlToolbarTPO.TabIndex = 3;
      this.btnPublishTPO.BackColor = Color.Transparent;
      this.btnPublishTPO.Location = new Point(40, 0);
      this.btnPublishTPO.Margin = new Padding(0);
      this.btnPublishTPO.Name = "btnPublishTPO";
      this.btnPublishTPO.Size = new Size(60, 22);
      this.btnPublishTPO.TabIndex = 0;
      this.btnPublishTPO.Text = "Publish";
      this.btnPublishTPO.UseVisualStyleBackColor = true;
      this.btnPublishTPO.Click += new EventHandler(this.btnPublishTPO_Click);
      this.separatorTPO.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separatorTPO.Location = new Point(35, 3);
      this.separatorTPO.Margin = new Padding(4, 3, 3, 3);
      this.separatorTPO.MaximumSize = new Size(2, 16);
      this.separatorTPO.MinimumSize = new Size(2, 16);
      this.separatorTPO.Name = "separatorTPO";
      this.separatorTPO.Size = new Size(2, 16);
      this.separatorTPO.TabIndex = 2;
      this.gvUnpublishedTPO.BorderStyle = BorderStyle.None;
      this.gvUnpublishedTPO.Dock = DockStyle.Fill;
      this.gvUnpublishedTPO.Location = new Point(1, 26);
      this.gvUnpublishedTPO.Name = "gvUnpublishedTPO";
      this.gvUnpublishedTPO.Size = new Size(637, 110);
      this.gvUnpublishedTPO.TabIndex = 1;
      this.gvUnpublishedTPO.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvUnpublishedTPO.EditorClosing += new GVSubItemEditingEventHandler(this.gvUnpublishedTPO_EditorClosing);
      this.chkBoxNoPrompt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkBoxNoPrompt.AutoSize = true;
      this.chkBoxNoPrompt.BackColor = Color.Transparent;
      this.chkBoxNoPrompt.Location = new Point(361, 4);
      this.chkBoxNoPrompt.Name = "chkBoxNoPrompt";
      this.chkBoxNoPrompt.Size = new Size(291, 18);
      this.chkBoxNoPrompt.TabIndex = 0;
      this.chkBoxNoPrompt.TabStop = false;
      this.chkBoxNoPrompt.Text = "Turn off automatic updates and reminders for this loan.";
      this.chkBoxNoPrompt.UseVisualStyleBackColor = false;
      this.gpHeader.Borders = AnchorStyles.Bottom;
      this.gpHeader.Controls.Add((Control) this.lblHeader);
      this.gpHeader.Dock = DockStyle.Top;
      this.gpHeader.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpHeader.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpHeader.Location = new Point(1, 26);
      this.gpHeader.Name = "gpHeader";
      this.gpHeader.Size = new Size(655, 31);
      this.gpHeader.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpHeader.TabIndex = 1;
      this.lblHeader.AutoSize = true;
      this.lblHeader.BackColor = Color.Transparent;
      this.lblHeader.ForeColor = SystemColors.ControlText;
      this.lblHeader.Location = new Point(8, 8);
      this.lblHeader.Margin = new Padding(3, 0, 0, 0);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(231, 14);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "Select items and click Publish to send updates.";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(657, 410);
      this.Controls.Add((Control) this.gcStatusOnline);
      this.Controls.Add((Control) this.pnlBottom);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (StatusOnlineDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Status Online Dialog";
      this.FormClosing += new FormClosingEventHandler(this.StatusOnlineDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.StatusOnlineDialog_KeyDown);
      ((ISupportInitialize) this.btnAddWC).EndInit();
      ((ISupportInitialize) this.btnAddTPO).EndInit();
      this.pnlBottom.ResumeLayout(false);
      this.gcStatusOnline.ResumeLayout(false);
      this.gcStatusOnline.PerformLayout();
      this.pnlPortal.ResumeLayout(false);
      this.tabPortal.ResumeLayout(false);
      this.pageWC.ResumeLayout(false);
      this.gcPublishedWC.ResumeLayout(false);
      this.gcUnpublishedWC.ResumeLayout(false);
      this.pnlToolbarWC.ResumeLayout(false);
      this.pageTPO.ResumeLayout(false);
      this.gcPublishedTPO.ResumeLayout(false);
      this.gcUnpublishedTPO.ResumeLayout(false);
      this.pnlToolbarTPO.ResumeLayout(false);
      this.gpHeader.ResumeLayout(false);
      this.gpHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
