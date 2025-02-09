// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StatusOnline.StatusOnlineTriggerDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.StatusOnline
{
  public class StatusOnlineTriggerDialog : Form
  {
    private const int TRIGGERTYPE_NONE = 0;
    private const int TRIGGERTYPE_MILESTONE = 1;
    private const int TRIGGERTYPE_DOCUMENT = 2;
    private const int TRIGGERTYPE_FIELDS = 3;
    private const int UPDATEMETHOD_AUTOMATIC = 0;
    private const int UPDATEMETHOD_MANUAL = 1;
    private const int REMINDER_NEVER = 0;
    private const int REMINDER_EXITLOAN = 1;
    private const int EMAILTEMPLATE_NONE = 0;
    private const int EMAILFROM_LOANOFFICER = 0;
    private const int EMAILFROM_FILESTARTER = 1;
    private const int EMAILFROM_CURRENTUSER = 2;
    private const int EMAILFROM_OWNER = 3;
    private Sessions.Session session;
    private StatusOnlineSetup triggerSetup;
    private StatusOnlineLoanSetup loanTriggerSetup;
    private LoanDataMgr loanDataMgr;
    private StatusOnlineTrigger trigger;
    private FieldSettings fieldSettings;
    private GridViewDataManager gvDocumentMgr;
    private GridViewDataManager gvMilestoneMgr;
    private HtmlEmailTemplateType _emailTemplateType;
    private IContainer components;
    private Button btnCancel;
    private Button btnSave;
    private GroupContainer gcRequirement;
    private GroupContainer gcConfigUpdate;
    private GradientPanel pnlRequirementType;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnFindField;
    private StandardIconButton btnAddField;
    private ComboBox cmbRequirementType;
    private Label lblTrigger;
    private GroupContainer gcEmail;
    private Label lblNameInfo;
    private TextBox txtName;
    private Label lblName;
    private ComboBox cmbReminderType;
    private Label lblReminderType;
    private ComboBox cmbUpdateType;
    private Label lblUpdateType;
    private Label lblEmailRecipients;
    private ComboBox cmbEmailFrom;
    private Label lblEmailFrom;
    private ComboBox cmbEmailTemplate;
    private Label lblEmailTemplate;
    private StandardIconButton btnEmailPreview;
    private StandardIconButton btnDeleteField;
    private EMHelpLink helpLink;
    private GroupContainer gcFields;
    private GroupContainer gcDocument;
    private GroupContainer gcMilestone;
    private GridView gvFields;
    private GridView gvDocument;
    private GridView gvMilestone;
    private GridView gvEmailRecipients;
    private ToolTip tooltip;
    private Label lblDescription;
    private TextBox txtDescription;

    public StatusOnlineLoanSetup LoanTriggerSetup => this.loanTriggerSetup;

    public StatusOnlineTriggerDialog(LoanDataMgr loanDataMgr, StatusOnlineTrigger trigger)
      : this(Session.DefaultInstance, (StatusOnlineSetup) null, loanDataMgr, trigger)
    {
    }

    public StatusOnlineTriggerDialog(
      Sessions.Session session,
      StatusOnlineSetup setup,
      StatusOnlineTrigger trigger)
      : this(session, setup, (LoanDataMgr) null, trigger)
    {
    }

    private StatusOnlineTriggerDialog(
      Sessions.Session session,
      StatusOnlineSetup setup,
      LoanDataMgr loanDataMgr,
      StatusOnlineTrigger trigger)
    {
      this.InitializeComponent();
      this.helpLink.AssignSession(session);
      this.session = session;
      this.triggerSetup = setup;
      this.loanDataMgr = loanDataMgr;
      this.trigger = trigger;
      this.initHeader();
      this.initRequirementTypes();
      this.initDocumentList();
      this.initMilestoneList();
      this.initEmailFrom();
      this.initEmailRecipients();
      this.initEmailTemplateType();
      if (this.trigger.OwnerID == null)
        this.helpLink.HelpTag = "Company Status Online";
      this.loadTriggerData();
    }

    private void initEmailTemplateType()
    {
      this._emailTemplateType = HtmlEmailTemplateType.StatusOnline | HtmlEmailTemplateType.ConsumerConnectStatusOnline;
    }

    private void initHeader()
    {
      if (this.trigger.PortalType != TriggerPortalType.TPOWC)
        return;
      this.lblNameInfo.Text = "Your TPO WebCenter users will see this Title and Description online.";
    }

    private void initRequirementTypes()
    {
      this.gcDocument.Dock = DockStyle.Fill;
      this.gcMilestone.Dock = DockStyle.Fill;
      this.gcFields.Dock = DockStyle.Fill;
    }

    private void initDocumentList()
    {
      this.gvDocumentMgr = new GridViewDataManager(this.session, this.gvDocument, this.loanDataMgr);
      if (this.trigger.RequirementType == TriggerRequirementType.DocumentTemplate || this.loanDataMgr == null)
      {
        this.gvDocument.HeaderVisible = false;
        this.gvDocumentMgr.CreateLayout(new TableLayout.Column[1]
        {
          GridViewDataManager.NameColumn
        });
        foreach (DocumentTemplate template in this.session.ConfigurationManager.GetDocumentTrackingSetup())
          this.gvDocumentMgr.AddItem(template);
        this.gvDocument.Sort(0, SortOrder.Ascending);
      }
      else
      {
        this.gvDocumentMgr.CreateLayout(new TableLayout.Column[2]
        {
          GridViewDataManager.NameColumn,
          GridViewDataManager.BorrowerColumn
        });
        this.gvDocument.Columns[1].SpringToFit = true;
        foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
          this.gvDocumentMgr.AddItem(allDocument);
        this.gvDocument.Sort(0, SortOrder.Ascending);
      }
    }

    private void initMilestoneList()
    {
      this.gvMilestoneMgr = new GridViewDataManager(this.session, this.gvMilestone, this.loanDataMgr);
      this.gvMilestoneMgr.CreateLayout(new TableLayout.Column[1]
      {
        GridViewDataManager.NameWithIconColumn
      });
      if (this.loanDataMgr == null)
      {
        List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
        foreach (EllieMae.EMLite.Workflow.Milestone allMilestones in ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllMilestonesList())
        {
          if (allMilestones.Archived)
            milestoneList.Add(allMilestones);
          else
            this.gvMilestoneMgr.AddItem(allMilestones);
        }
        foreach (EllieMae.EMLite.Workflow.Milestone milestone in milestoneList)
          this.gvMilestoneMgr.AddItem(milestone);
      }
      else
      {
        foreach (MilestoneLog allMilestone in this.loanDataMgr.LoanData.GetLogList().GetAllMilestones())
          this.gvMilestoneMgr.AddItem(allMilestone);
      }
    }

    private void initEmailFrom()
    {
      if (this.trigger.OwnerID == null)
        return;
      this.cmbEmailFrom.SelectedIndex = this.cmbEmailFrom.Items.Add((object) "Me");
    }

    private void initEmailRecipients()
    {
      if (this.trigger.PortalType == TriggerPortalType.WebCenter)
      {
        this.gvEmailRecipients.Items.Add("Borrower").Tag = (object) "1240";
        this.gvEmailRecipients.Items.Add("Co-borrower").Tag = (object) "1268";
        this.gvEmailRecipients.Items.Add("Buyer's Agent").Tag = (object) "VEND.X141";
        this.gvEmailRecipients.Items.Add("Non-Borrowing Owner").Tag = (object) "NBO";
      }
      else
      {
        if (this.trigger.PortalType != TriggerPortalType.TPOWC)
          return;
        this.gvEmailRecipients.Items.Add("TPO Loan Officer").Tag = (object) "TPO.X63";
        this.gvEmailRecipients.Items.Add("TPO Loan Processor").Tag = (object) "TPO.X76";
      }
    }

    private void addRequiredField(string field)
    {
      if (this.gvFields.Items.Contains((object) field))
        return;
      GVItem gvItem = this.gvFields.Items.Add(field);
      if (this.fieldSettings == null)
        this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      FieldDefinition field1 = EncompassFields.GetField(field, this.fieldSettings, true);
      if (field1 == null)
        return;
      gvItem.SubItems.Add((object) field1.Description);
    }

    private void loadTriggerData()
    {
      if (this.trigger.RequirementType == TriggerRequirementType.None)
        this.cmbRequirementType.SelectedIndex = 0;
      else if (this.trigger.RequirementType == TriggerRequirementType.Milestone)
      {
        this.cmbRequirementType.SelectedIndex = 1;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        {
          if ((gvItem.Tag as EllieMae.EMLite.Workflow.Milestone).MilestoneID == this.trigger.RequirementData)
            gvItem.Selected = true;
        }
      }
      else if (this.trigger.RequirementType == TriggerRequirementType.MilestoneLog)
      {
        this.cmbRequirementType.SelectedIndex = 1;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvMilestone.Items)
        {
          if ((gvItem.Tag as MilestoneLog).MilestoneID == this.trigger.RequirementData)
            gvItem.Selected = true;
        }
      }
      else if (this.trigger.RequirementType == TriggerRequirementType.DocumentTemplate)
      {
        this.cmbRequirementType.SelectedIndex = 2;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocument.Items)
        {
          if ((gvItem.Tag as DocumentTemplate).Guid == this.trigger.RequirementData)
            gvItem.Selected = true;
        }
      }
      else if (this.trigger.RequirementType == TriggerRequirementType.DocumentLog)
      {
        this.cmbRequirementType.SelectedIndex = 2;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocument.Items)
        {
          if ((gvItem.Tag as DocumentLog).Guid == this.trigger.RequirementData)
            gvItem.Selected = true;
        }
      }
      else if (this.trigger.RequirementType == TriggerRequirementType.DocumentName)
      {
        this.cmbRequirementType.SelectedIndex = 2;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocument.Items)
        {
          if ((gvItem.Tag as DocumentLog).Title == this.trigger.RequirementData)
            gvItem.Selected = true;
        }
      }
      else if (this.trigger.RequirementType == TriggerRequirementType.Fields)
      {
        this.cmbRequirementType.SelectedIndex = 3;
        string requirementData = this.trigger.RequirementData;
        char[] chArray = new char[1]{ ',' };
        foreach (string field in requirementData.Split(chArray))
          this.addRequiredField(field);
      }
      this.txtName.Text = this.trigger.Name;
      this.txtDescription.Text = this.trigger.Description;
      switch (this.trigger.UpdateType)
      {
        case TriggerUpdateType.Manual:
          this.cmbUpdateType.SelectedIndex = 1;
          break;
        case TriggerUpdateType.Automatic:
          this.cmbUpdateType.SelectedIndex = 0;
          break;
      }
      switch (this.trigger.ReminderType)
      {
        case TriggerReminderType.NoReminder:
          this.cmbReminderType.SelectedIndex = 0;
          break;
        case TriggerReminderType.RemindOnExit:
          this.cmbReminderType.SelectedIndex = 1;
          break;
      }
      HtmlEmailTemplate htmlEmailTemplate = (HtmlEmailTemplate) null;
      if (!string.IsNullOrEmpty(this.trigger.EmailTemplate))
        htmlEmailTemplate = this.session.ConfigurationManager.GetHtmlEmailTemplate(this.trigger.EmailTemplateOwner, this.trigger.EmailTemplate);
      if (htmlEmailTemplate != (HtmlEmailTemplate) null)
      {
        this.cmbEmailTemplate.Items.Add((object) htmlEmailTemplate);
        this.cmbEmailTemplate.SelectedItem = (object) htmlEmailTemplate;
        switch (this.trigger.EmailFromType)
        {
          case TriggerEmailFromType.CurrentUser:
            this.cmbEmailFrom.SelectedIndex = 2;
            break;
          case TriggerEmailFromType.LoanOfficer:
            this.cmbEmailFrom.SelectedIndex = 0;
            break;
          case TriggerEmailFromType.FileStarter:
            this.cmbEmailFrom.SelectedIndex = 1;
            break;
          case TriggerEmailFromType.Owner:
            this.cmbEmailFrom.SelectedIndex = 3;
            break;
        }
        foreach (string emailRecipient in this.trigger.EmailRecipients)
        {
          GVItem itemByTag = this.gvEmailRecipients.Items.GetItemByTag((object) emailRecipient);
          if (itemByTag == null)
          {
            switch (emailRecipient.ToUpper())
            {
              case "CX.TPO.LOEMAIL":
              case "TPO.X63":
                itemByTag = this.gvEmailRecipients.Items.GetItemByTag((object) "TPO.X63");
                break;
              case "CX.TPO.LPEMAIL":
              case "TPO.X76":
                itemByTag = this.gvEmailRecipients.Items.GetItemByTag((object) "TPO.X76");
                break;
            }
          }
          if (itemByTag != null)
            itemByTag.Checked = true;
        }
      }
      else
        this.cmbEmailTemplate.SelectedIndex = 0;
    }

    private bool validateTriggerData()
    {
      List<string> stringList = new List<string>();
      if (this.cmbRequirementType.SelectedIndex == -1)
        stringList.Add("You must select a trigger type.");
      else if (this.cmbRequirementType.SelectedIndex == 1)
      {
        if (this.gvMilestone.SelectedItems.Count == 0)
          stringList.Add("You must select a trigger milestone.");
      }
      else if (this.cmbRequirementType.SelectedIndex == 2)
      {
        if (this.gvDocument.SelectedItems.Count == 0)
          stringList.Add("You must select a trigger document.");
      }
      else if (this.cmbRequirementType.SelectedIndex == 3)
      {
        if (this.gvFields.Items.Count == 0)
          stringList.Add("You must have at least one trigger field.");
      }
      else if (this.cmbRequirementType.SelectedIndex == 0 && this.cmbUpdateType.SelectedIndex == 0)
        stringList.Add("Update method must be 'Manual' if selected trigger type is 'No Trigger'");
      if (this.txtName.Text.Trim() == string.Empty)
        stringList.Add("You must enter a trigger description.");
      if (this.cmbUpdateType.SelectedIndex == -1)
        stringList.Add("You must select an update method.");
      else if (this.cmbUpdateType.SelectedIndex == 1 && this.cmbReminderType.SelectedIndex == -1)
        stringList.Add("You must select a reminder type.");
      if (this.cmbEmailTemplate.SelectedIndex == -1)
        stringList.Add("You must select a notification email template.");
      else if ((object) (this.cmbEmailTemplate.SelectedItem as HtmlEmailTemplate) != null)
      {
        if (this.cmbEmailFrom.SelectedIndex == -1)
          stringList.Add("You must select a 'From' for the notification email.");
        bool flag = false;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvEmailRecipients.Items)
        {
          if (gvItem.Checked)
            flag = true;
        }
        if (!flag)
          stringList.Add("You must select at least one 'To' for the notification email.");
      }
      if (stringList.Count == 0)
        return true;
      string text = "You must fix the following items in order to save:";
      foreach (string str in stringList)
        text = text + "\n\n" + str;
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return false;
    }

    private void saveTriggerData()
    {
      if (this.cmbRequirementType.SelectedIndex == 0)
        this.trigger.RequirementType = TriggerRequirementType.None;
      else if (this.cmbRequirementType.SelectedIndex == 1)
      {
        GVItem selectedItem = this.gvMilestone.SelectedItems[0];
        if (selectedItem.Tag is EllieMae.EMLite.Workflow.Milestone)
        {
          this.trigger.RequirementType = TriggerRequirementType.Milestone;
          this.trigger.RequirementData = ((EllieMae.EMLite.Workflow.Milestone) selectedItem.Tag).MilestoneID;
        }
        else if (selectedItem.Tag is MilestoneLog)
        {
          this.trigger.RequirementType = TriggerRequirementType.MilestoneLog;
          this.trigger.RequirementData = ((MilestoneLog) selectedItem.Tag).MilestoneID;
        }
      }
      else if (this.cmbRequirementType.SelectedIndex == 2)
      {
        GVItem selectedItem = this.gvDocument.SelectedItems[0];
        if (selectedItem.Tag is DocumentTemplate)
        {
          this.trigger.RequirementType = TriggerRequirementType.DocumentTemplate;
          this.trigger.RequirementData = ((DocumentTemplate) selectedItem.Tag).Guid;
        }
        else if (selectedItem.Tag is DocumentLog)
        {
          this.trigger.RequirementType = TriggerRequirementType.DocumentLog;
          this.trigger.RequirementData = ((LogRecordBase) selectedItem.Tag).Guid;
        }
      }
      else if (this.cmbRequirementType.SelectedIndex == 3)
      {
        List<string> stringList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
          stringList.Add(gvItem.Text);
        this.trigger.RequirementType = TriggerRequirementType.Fields;
        this.trigger.RequirementData = string.Join(",", stringList.ToArray());
      }
      this.trigger.Name = this.txtName.Text.Trim();
      this.trigger.Description = this.txtDescription.Text.Trim();
      switch (this.cmbUpdateType.SelectedIndex)
      {
        case 0:
          this.trigger.UpdateType = TriggerUpdateType.Automatic;
          break;
        case 1:
          this.trigger.UpdateType = TriggerUpdateType.Manual;
          break;
      }
      switch (this.cmbReminderType.SelectedIndex)
      {
        case 0:
          this.trigger.ReminderType = TriggerReminderType.NoReminder;
          break;
        case 1:
          this.trigger.ReminderType = TriggerReminderType.RemindOnExit;
          break;
      }
      if ((object) (this.cmbEmailTemplate.SelectedItem as HtmlEmailTemplate) != null)
      {
        HtmlEmailTemplate selectedItem = (HtmlEmailTemplate) this.cmbEmailTemplate.SelectedItem;
        this.trigger.EmailTemplate = selectedItem.Guid;
        this.trigger.EmailTemplateOwner = selectedItem.OwnerID;
        switch (this.cmbEmailFrom.SelectedIndex)
        {
          case 0:
            this.trigger.EmailFromType = TriggerEmailFromType.LoanOfficer;
            break;
          case 1:
            this.trigger.EmailFromType = TriggerEmailFromType.FileStarter;
            break;
          case 2:
            this.trigger.EmailFromType = TriggerEmailFromType.CurrentUser;
            break;
          case 3:
            this.trigger.EmailFromType = TriggerEmailFromType.Owner;
            break;
        }
        List<string> stringList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvEmailRecipients.Items)
        {
          if (gvItem.Checked)
            stringList.Add(gvItem.Tag as string);
        }
        this.trigger.EmailRecipients = stringList.ToArray();
      }
      else
      {
        this.trigger.EmailTemplate = (string) null;
        this.trigger.EmailTemplateOwner = (string) null;
      }
      if (this.loanDataMgr == null)
      {
        if (!this.triggerSetup.Triggers.Contains(this.trigger))
          this.triggerSetup.Triggers.Add(this.trigger);
        this.session.ConfigurationManager.SaveStatusOnlineSetup(this.trigger.OwnerID, this.triggerSetup);
      }
      else
      {
        StatusOnlineTrigger[] triggerList = new StatusOnlineTrigger[1]
        {
          this.trigger
        };
        this.loanTriggerSetup = this.session.LoanManager.SaveStatusOnlineTriggers(this.session.LoanManager.GetLoanIdentity(this.loanDataMgr.LoanData.GUID), triggerList);
      }
    }

    private void cmbRequirementType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.gcMilestone.Visible = this.cmbRequirementType.SelectedIndex == 1;
      this.gcDocument.Visible = this.cmbRequirementType.SelectedIndex == 2;
      this.gcFields.Visible = this.cmbRequirementType.SelectedIndex == 3;
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDeleteField.Enabled = this.gvFields.SelectedItems.Count > 0;
    }

    private void btnAddField_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, "Add Fields", AddFieldOptions.AllowAnyField))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.dialog_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (string selectedFieldId in addFields.SelectedFieldIDs)
          this.addRequiredField(selectedFieldId);
        this.gvFields.ReSort();
      }
    }

    private void dialog_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      foreach (string selectedFieldId in ((AddFields) sender).SelectedFieldIDs)
        this.addRequiredField(selectedFieldId);
      this.gvFields.ReSort();
    }

    private void btnFindField_Click(object sender, EventArgs e)
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
        stringList.Add(gvItem.Text);
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, stringList.ToArray(), true, string.Empty, false, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (string selectedRequiredField in ruleFindFieldDialog.SelectedRequiredFields)
          this.addRequiredField(selectedRequiredField);
        this.gvFields.ReSort();
      }
    }

    private void btnDeleteField_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
        gvItemList.Add(selectedItem);
      foreach (GVItem gvItem in gvItemList)
        this.gvFields.Items.Remove(gvItem);
    }

    private void cmbUpdateType_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = this.cmbUpdateType.SelectedIndex == 1;
      this.lblReminderType.Visible = flag;
      this.cmbReminderType.Visible = flag;
    }

    private void cmbEmailTemplate_DropDown(object sender, EventArgs e)
    {
      foreach (object obj in ((IEnumerable<HtmlEmailTemplate>) this.session.ConfigurationManager.GetHtmlEmailTemplates((string) null, this._emailTemplateType)).Where<HtmlEmailTemplate>((Func<HtmlEmailTemplate, bool>) (template => !this.cmbEmailTemplate.Items.Contains((object) template))))
        this.cmbEmailTemplate.Items.Add(obj);
      if (this.trigger.OwnerID != null)
      {
        foreach (object obj in ((IEnumerable<HtmlEmailTemplate>) this.session.ConfigurationManager.GetHtmlEmailTemplates(this.trigger.OwnerID, this._emailTemplateType)).Where<HtmlEmailTemplate>((Func<HtmlEmailTemplate, bool>) (template => !this.cmbEmailTemplate.Items.Contains((object) template))))
          this.cmbEmailTemplate.Items.Add(obj);
      }
      this.cmbEmailTemplate.DropDown -= new EventHandler(this.cmbEmailTemplate_DropDown);
    }

    private void cmbEmailTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = this.cmbEmailTemplate.SelectedItem is HtmlEmailTemplate;
      this.btnEmailPreview.Enabled = flag;
      this.lblEmailFrom.Visible = flag;
      this.cmbEmailFrom.Visible = flag;
      this.lblEmailRecipients.Visible = flag;
      this.gvEmailRecipients.Visible = flag;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateTriggerData())
        return;
      this.saveTriggerData();
      this.DialogResult = DialogResult.OK;
    }

    private void btnEmailPreview_Click(object sender, EventArgs e)
    {
      using (EmailTemplateDialog emailTemplateDialog = new EmailTemplateDialog(this.session, (HtmlEmailTemplate) this.cmbEmailTemplate.SelectedItem, true))
      {
        int num = (int) emailTemplateDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void TriggerDetailsDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.gcConfigUpdate = new GroupContainer();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.cmbReminderType = new ComboBox();
      this.lblReminderType = new Label();
      this.cmbUpdateType = new ComboBox();
      this.lblUpdateType = new Label();
      this.lblNameInfo = new Label();
      this.txtName = new TextBox();
      this.lblName = new Label();
      this.gcRequirement = new GroupContainer();
      this.gcMilestone = new GroupContainer();
      this.gvMilestone = new GridView();
      this.gcFields = new GroupContainer();
      this.gvFields = new GridView();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDeleteField = new StandardIconButton();
      this.btnFindField = new StandardIconButton();
      this.btnAddField = new StandardIconButton();
      this.gcDocument = new GroupContainer();
      this.gvDocument = new GridView();
      this.pnlRequirementType = new GradientPanel();
      this.lblTrigger = new Label();
      this.cmbRequirementType = new ComboBox();
      this.gcEmail = new GroupContainer();
      this.gvEmailRecipients = new GridView();
      this.btnEmailPreview = new StandardIconButton();
      this.lblEmailRecipients = new Label();
      this.cmbEmailFrom = new ComboBox();
      this.lblEmailFrom = new Label();
      this.cmbEmailTemplate = new ComboBox();
      this.lblEmailTemplate = new Label();
      this.helpLink = new EMHelpLink();
      this.tooltip = new ToolTip(this.components);
      this.gcConfigUpdate.SuspendLayout();
      this.gcRequirement.SuspendLayout();
      this.gcMilestone.SuspendLayout();
      this.gcFields.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteField).BeginInit();
      ((ISupportInitialize) this.btnFindField).BeginInit();
      ((ISupportInitialize) this.btnAddField).BeginInit();
      this.gcDocument.SuspendLayout();
      this.pnlRequirementType.SuspendLayout();
      this.gcEmail.SuspendLayout();
      ((ISupportInitialize) this.btnEmailPreview).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(670, 410);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(590, 410);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.gcConfigUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.gcConfigUpdate.Controls.Add((Control) this.txtDescription);
      this.gcConfigUpdate.Controls.Add((Control) this.lblDescription);
      this.gcConfigUpdate.Controls.Add((Control) this.cmbReminderType);
      this.gcConfigUpdate.Controls.Add((Control) this.lblReminderType);
      this.gcConfigUpdate.Controls.Add((Control) this.cmbUpdateType);
      this.gcConfigUpdate.Controls.Add((Control) this.lblUpdateType);
      this.gcConfigUpdate.Controls.Add((Control) this.lblNameInfo);
      this.gcConfigUpdate.Controls.Add((Control) this.txtName);
      this.gcConfigUpdate.Controls.Add((Control) this.lblName);
      this.gcConfigUpdate.HeaderForeColor = SystemColors.ControlText;
      this.gcConfigUpdate.Location = new Point(368, 4);
      this.gcConfigUpdate.Name = "gcConfigUpdate";
      this.gcConfigUpdate.Size = new Size(382, 216);
      this.gcConfigUpdate.TabIndex = 1;
      this.gcConfigUpdate.Text = "2. Configure Status Online Update";
      this.txtDescription.Location = new Point(112, 80);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(256, 76);
      this.txtDescription.TabIndex = 4;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(8, 84);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(102, 14);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.Text = "Detailed Description";
      this.cmbReminderType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbReminderType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbReminderType.Items.AddRange(new object[2]
      {
        (object) "No Reminder",
        (object) "Remind users when they exit the loan"
      });
      this.cmbReminderType.Location = new Point(112, 188);
      this.cmbReminderType.Name = "cmbReminderType";
      this.cmbReminderType.Size = new Size(258, 22);
      this.cmbReminderType.TabIndex = 8;
      this.lblReminderType.AutoSize = true;
      this.lblReminderType.BackColor = Color.Transparent;
      this.lblReminderType.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblReminderType.Location = new Point(8, 192);
      this.lblReminderType.Name = "lblReminderType";
      this.lblReminderType.Size = new Size(56, 14);
      this.lblReminderType.TabIndex = 7;
      this.lblReminderType.Text = "*Reminder";
      this.cmbUpdateType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbUpdateType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbUpdateType.Items.AddRange(new object[2]
      {
        (object) "Automatic Update",
        (object) "Manual Update"
      });
      this.cmbUpdateType.Location = new Point(112, 160);
      this.cmbUpdateType.Name = "cmbUpdateType";
      this.cmbUpdateType.Size = new Size(258, 22);
      this.cmbUpdateType.TabIndex = 6;
      this.cmbUpdateType.SelectedIndexChanged += new EventHandler(this.cmbUpdateType_SelectedIndexChanged);
      this.lblUpdateType.AutoSize = true;
      this.lblUpdateType.BackColor = Color.Transparent;
      this.lblUpdateType.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblUpdateType.Location = new Point(8, 164);
      this.lblUpdateType.Name = "lblUpdateType";
      this.lblUpdateType.Size = new Size(83, 14);
      this.lblUpdateType.TabIndex = 5;
      this.lblUpdateType.Text = "*Update Method";
      this.lblNameInfo.AutoSize = true;
      this.lblNameInfo.BackColor = Color.Transparent;
      this.lblNameInfo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNameInfo.ForeColor = Color.Red;
      this.lblNameInfo.Location = new Point(8, 36);
      this.lblNameInfo.Name = "lblNameInfo";
      this.lblNameInfo.Size = new Size(344, 14);
      this.lblNameInfo.TabIndex = 0;
      this.lblNameInfo.Text = "Your borrowers and partners will see this Title and Description online.";
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(112, 56);
      this.txtName.MaxLength = 50;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(258, 20);
      this.txtName.TabIndex = 2;
      this.lblName.AutoSize = true;
      this.lblName.BackColor = Color.Transparent;
      this.lblName.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblName.Location = new Point(8, 60);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(64, 14);
      this.lblName.TabIndex = 1;
      this.lblName.Text = "*Status Title";
      this.gcRequirement.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcRequirement.Controls.Add((Control) this.gcMilestone);
      this.gcRequirement.Controls.Add((Control) this.gcFields);
      this.gcRequirement.Controls.Add((Control) this.gcDocument);
      this.gcRequirement.Controls.Add((Control) this.pnlRequirementType);
      this.gcRequirement.HeaderForeColor = SystemColors.ControlText;
      this.gcRequirement.Location = new Point(4, 4);
      this.gcRequirement.Name = "gcRequirement";
      this.gcRequirement.Padding = new Padding(3);
      this.gcRequirement.Size = new Size(356, 398);
      this.gcRequirement.TabIndex = 0;
      this.gcRequirement.Text = "1. Select Trigger";
      this.gcMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMilestone.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcMilestone.Controls.Add((Control) this.gvMilestone);
      this.gcMilestone.HeaderForeColor = SystemColors.ControlText;
      this.gcMilestone.Location = new Point(8, 296);
      this.gcMilestone.Name = "gcMilestone";
      this.gcMilestone.Size = new Size(340, 96);
      this.gcMilestone.TabIndex = 3;
      this.gcMilestone.Text = "Select a Trigger Milestone";
      this.gcMilestone.Visible = false;
      this.gvMilestone.AllowMultiselect = false;
      this.gvMilestone.BorderStyle = BorderStyle.None;
      this.gvMilestone.Dock = DockStyle.Fill;
      this.gvMilestone.HeaderHeight = 0;
      this.gvMilestone.HeaderVisible = false;
      this.gvMilestone.Location = new Point(1, 25);
      this.gvMilestone.Name = "gvMilestone";
      this.gvMilestone.Size = new Size(338, 70);
      this.gvMilestone.TabIndex = 0;
      this.gcFields.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcFields.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcFields.Controls.Add((Control) this.gvFields);
      this.gcFields.Controls.Add((Control) this.pnlToolbar);
      this.gcFields.HeaderForeColor = SystemColors.ControlText;
      this.gcFields.Location = new Point(8, 68);
      this.gcFields.Name = "gcFields";
      this.gcFields.Size = new Size(340, 104);
      this.gcFields.TabIndex = 1;
      this.gcFields.Text = "Add Trigger Fields";
      this.gcFields.Visible = false;
      this.gvFields.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colFieldID";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colFieldDesc";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 238;
      this.gvFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvFields.Dock = DockStyle.Fill;
      this.gvFields.Location = new Point(1, 25);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(338, 78);
      this.gvFields.TabIndex = 0;
      this.gvFields.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvFields.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDeleteField);
      this.pnlToolbar.Controls.Add((Control) this.btnFindField);
      this.pnlToolbar.Controls.Add((Control) this.btnAddField);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(236, 1);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(102, 22);
      this.pnlToolbar.TabIndex = 1;
      this.btnDeleteField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteField.BackColor = Color.Transparent;
      this.btnDeleteField.Enabled = false;
      this.btnDeleteField.Location = new Point(83, 3);
      this.btnDeleteField.MouseDownImage = (Image) null;
      this.btnDeleteField.Name = "btnDeleteField";
      this.btnDeleteField.Size = new Size(16, 17);
      this.btnDeleteField.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteField.TabIndex = 29;
      this.btnDeleteField.TabStop = false;
      this.btnDeleteField.Click += new EventHandler(this.btnDeleteField_Click);
      this.btnFindField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFindField.BackColor = Color.Transparent;
      this.btnFindField.Location = new Point(64, 3);
      this.btnFindField.Margin = new Padding(4, 3, 0, 3);
      this.btnFindField.MouseDownImage = (Image) null;
      this.btnFindField.Name = "btnFindField";
      this.btnFindField.Size = new Size(16, 18);
      this.btnFindField.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFindField.TabIndex = 28;
      this.btnFindField.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnFindField, "Find Fields");
      this.btnFindField.Click += new EventHandler(this.btnFindField_Click);
      this.btnAddField.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddField.BackColor = Color.Transparent;
      this.btnAddField.Location = new Point(44, 3);
      this.btnAddField.Margin = new Padding(4, 3, 0, 3);
      this.btnAddField.MouseDownImage = (Image) null;
      this.btnAddField.Name = "btnAddField";
      this.btnAddField.Size = new Size(16, 18);
      this.btnAddField.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddField.TabIndex = 26;
      this.btnAddField.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddField, "Add Fields");
      this.btnAddField.Click += new EventHandler(this.btnAddField_Click);
      this.gcDocument.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDocument.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDocument.Controls.Add((Control) this.gvDocument);
      this.gcDocument.HeaderForeColor = SystemColors.ControlText;
      this.gcDocument.Location = new Point(8, 176);
      this.gcDocument.Name = "gcDocument";
      this.gcDocument.Size = new Size(340, 116);
      this.gcDocument.TabIndex = 2;
      this.gcDocument.Text = "Select a Trigger Document";
      this.gcDocument.Visible = false;
      this.gvDocument.AllowMultiselect = false;
      this.gvDocument.BorderStyle = BorderStyle.None;
      this.gvDocument.Dock = DockStyle.Fill;
      this.gvDocument.Location = new Point(1, 25);
      this.gvDocument.Name = "gvDocument";
      this.gvDocument.Size = new Size(338, 90);
      this.gvDocument.TabIndex = 0;
      this.gvDocument.TextTrimming = StringTrimming.EllipsisCharacter;
      this.pnlRequirementType.Controls.Add((Control) this.lblTrigger);
      this.pnlRequirementType.Controls.Add((Control) this.cmbRequirementType);
      this.pnlRequirementType.Dock = DockStyle.Top;
      this.pnlRequirementType.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlRequirementType.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlRequirementType.Location = new Point(4, 29);
      this.pnlRequirementType.Name = "pnlRequirementType";
      this.pnlRequirementType.Size = new Size(348, 32);
      this.pnlRequirementType.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlRequirementType.TabIndex = 0;
      this.lblTrigger.AutoSize = true;
      this.lblTrigger.BackColor = Color.Transparent;
      this.lblTrigger.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTrigger.Location = new Point(8, 9);
      this.lblTrigger.Name = "lblTrigger";
      this.lblTrigger.Size = new Size(71, 14);
      this.lblTrigger.TabIndex = 0;
      this.lblTrigger.Text = "*Trigger Type";
      this.cmbRequirementType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbRequirementType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRequirementType.Items.AddRange(new object[4]
      {
        (object) "No Trigger",
        (object) "Milestone Finished",
        (object) "Document Received",
        (object) "Field Value Entered"
      });
      this.cmbRequirementType.Location = new Point(80, 5);
      this.cmbRequirementType.Name = "cmbRequirementType";
      this.cmbRequirementType.Size = new Size(260, 22);
      this.cmbRequirementType.TabIndex = 1;
      this.cmbRequirementType.SelectedIndexChanged += new EventHandler(this.cmbRequirementType_SelectedIndexChanged);
      this.gcEmail.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.gcEmail.Controls.Add((Control) this.gvEmailRecipients);
      this.gcEmail.Controls.Add((Control) this.btnEmailPreview);
      this.gcEmail.Controls.Add((Control) this.lblEmailRecipients);
      this.gcEmail.Controls.Add((Control) this.cmbEmailFrom);
      this.gcEmail.Controls.Add((Control) this.lblEmailFrom);
      this.gcEmail.Controls.Add((Control) this.cmbEmailTemplate);
      this.gcEmail.Controls.Add((Control) this.lblEmailTemplate);
      this.gcEmail.HeaderForeColor = SystemColors.ControlText;
      this.gcEmail.Location = new Point(368, 224);
      this.gcEmail.Name = "gcEmail";
      this.gcEmail.Size = new Size(382, 178);
      this.gcEmail.TabIndex = 2;
      this.gcEmail.Text = "3. Send Notification Email (Optional)";
      this.gvEmailRecipients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colContacts";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Contact";
      gvColumn3.Width = 264;
      this.gvEmailRecipients.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvEmailRecipients.HeaderHeight = 0;
      this.gvEmailRecipients.HeaderVisible = false;
      this.gvEmailRecipients.Location = new Point(88, 92);
      this.gvEmailRecipients.Name = "gvEmailRecipients";
      this.gvEmailRecipients.Size = new Size(266, 73);
      this.gvEmailRecipients.TabIndex = 5;
      this.gvEmailRecipients.TextTrimming = StringTrimming.EllipsisCharacter;
      this.btnEmailPreview.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEmailPreview.BackColor = Color.Transparent;
      this.btnEmailPreview.Location = new Point(358, 38);
      this.btnEmailPreview.Margin = new Padding(0, 3, 4, 0);
      this.btnEmailPreview.MouseDownImage = (Image) null;
      this.btnEmailPreview.Name = "btnEmailPreview";
      this.btnEmailPreview.Size = new Size(16, 17);
      this.btnEmailPreview.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnEmailPreview.TabIndex = 60;
      this.btnEmailPreview.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEmailPreview, "Preview Email");
      this.btnEmailPreview.Click += new EventHandler(this.btnEmailPreview_Click);
      this.lblEmailRecipients.AutoSize = true;
      this.lblEmailRecipients.BackColor = Color.Transparent;
      this.lblEmailRecipients.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblEmailRecipients.Location = new Point(8, 96);
      this.lblEmailRecipients.Name = "lblEmailRecipients";
      this.lblEmailRecipients.Size = new Size(18, 14);
      this.lblEmailRecipients.TabIndex = 4;
      this.lblEmailRecipients.Text = "To";
      this.cmbEmailFrom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbEmailFrom.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEmailFrom.Items.AddRange(new object[3]
      {
        (object) "Loan Officer",
        (object) "File Starter",
        (object) "Current User"
      });
      this.cmbEmailFrom.Location = new Point(88, 64);
      this.cmbEmailFrom.Name = "cmbEmailFrom";
      this.cmbEmailFrom.Size = new Size(266, 22);
      this.cmbEmailFrom.TabIndex = 3;
      this.lblEmailFrom.AutoSize = true;
      this.lblEmailFrom.BackColor = Color.Transparent;
      this.lblEmailFrom.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblEmailFrom.Location = new Point(8, 68);
      this.lblEmailFrom.Name = "lblEmailFrom";
      this.lblEmailFrom.Size = new Size(31, 14);
      this.lblEmailFrom.TabIndex = 2;
      this.lblEmailFrom.Text = "From";
      this.cmbEmailTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbEmailTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEmailTemplate.Items.AddRange(new object[1]
      {
        (object) "No Email"
      });
      this.cmbEmailTemplate.Location = new Point(88, 36);
      this.cmbEmailTemplate.Name = "cmbEmailTemplate";
      this.cmbEmailTemplate.Size = new Size(266, 22);
      this.cmbEmailTemplate.Sorted = true;
      this.cmbEmailTemplate.TabIndex = 1;
      this.cmbEmailTemplate.DropDown += new EventHandler(this.cmbEmailTemplate_DropDown);
      this.cmbEmailTemplate.SelectedIndexChanged += new EventHandler(this.cmbEmailTemplate_SelectedIndexChanged);
      this.lblEmailTemplate.AutoSize = true;
      this.lblEmailTemplate.BackColor = Color.Transparent;
      this.lblEmailTemplate.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblEmailTemplate.Location = new Point(8, 40);
      this.lblEmailTemplate.Name = "lblEmailTemplate";
      this.lblEmailTemplate.Size = new Size(76, 14);
      this.lblEmailTemplate.TabIndex = 0;
      this.lblEmailTemplate.Text = "Email Template";
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Personal Status Online";
      this.helpLink.Location = new Point(10, 410);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(755, 440);
      this.Controls.Add((Control) this.gcConfigUpdate);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.gcRequirement);
      this.Controls.Add((Control) this.gcEmail);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (StatusOnlineTriggerDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Status Update Details";
      this.KeyDown += new KeyEventHandler(this.TriggerDetailsDialog_KeyDown);
      this.gcConfigUpdate.ResumeLayout(false);
      this.gcConfigUpdate.PerformLayout();
      this.gcRequirement.ResumeLayout(false);
      this.gcMilestone.ResumeLayout(false);
      this.gcFields.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeleteField).EndInit();
      ((ISupportInitialize) this.btnFindField).EndInit();
      ((ISupportInitialize) this.btnAddField).EndInit();
      this.gcDocument.ResumeLayout(false);
      this.pnlRequirementType.ResumeLayout(false);
      this.pnlRequirementType.PerformLayout();
      this.gcEmail.ResumeLayout(false);
      this.gcEmail.PerformLayout();
      ((ISupportInitialize) this.btnEmailPreview).EndInit();
      this.ResumeLayout(false);
    }
  }
}
