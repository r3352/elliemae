// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignStepsTab
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.ContactUI.TaskList;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using Infragistics.Win.UltraWinSchedule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignStepsTab : Form
  {
    private const int LAST_NAME_COLUMN_INDEX = 0;
    private const int FIRST_NAME_COLUMN_INDEX = 1;
    private const int SCHEDULED_DATE_COLUMN_INDEX = 2;
    private const int PHONE_NUMBER_COLUMN_INDEX = 3;
    private const int EMAIL_ADDRESS_COLUMN_INDEX = 4;
    private string lastNameFilterValue = string.Empty;
    private string firstNameFilterValue = string.Empty;
    private DateTime scheduledDateFilterValue = DateTime.MinValue;
    private string phoneNumberFilterValue = string.Empty;
    private string emailAddressFilterValue = string.Empty;
    private CampaignData campaignData;
    private ActivityTypeNameProvider activityNames = new ActivityTypeNameProvider();
    private CampaignStepsTab.YesNoUnknown addInaccessibleContacts = CampaignStepsTab.YesNoUnknown.Unknown;
    private int[] accessibleIds;
    private DatePicker dtpScheduleDate;
    private string accessibleUserId = string.Empty;
    private IContainer components;
    private CollapsibleSplitter ctrStepDetailSplitter;
    private Panel pnlStepDetail;
    private Label lblTotalSteps;
    private Label lblStepDescription;
    private Label lblStepDescriptionHdr;
    private Label lblStepNameHdr;
    private Label lblStepName;
    private ToolTip tipCampaignSteps;
    private BorderPanel pnlStepList;
    private Panel pnlEmailTask;
    private Panel pnlFaxLetterTask;
    private Panel pnlPhoneCallReminderTask;
    private Label lblScheduleDateHdr;
    private Label lblEmailDocumentHdr;
    private Label lblEmailSubjectHdr;
    private TextBox txtEmailSubject;
    private Label lblLetterDocumentHdr;
    private Label lblTaskType;
    private Label lblTaskTypeHdr;
    private UltraCalendarInfo ultraCalendarInfo;
    private GVItem gvTooltipItem;
    private GradientPanel pnlSteps;
    private GridView gvSteps;
    private GroupContainer grpStepDetails;
    private Label lblTaskInformation;
    private TableContainer pnlContactsTable;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton icnDeleteTask;
    private StandardIconButton icnExcelTask;
    private StandardIconButton icnPrintTask;
    private Button btnTaskAction1;
    private Button btnTaskAction2;
    private Button btnTaskAction3;
    private FormattedLabel lblTaskCount;
    private CheckBox cbIncludeFutureTasks;
    private GridView gvContacts;
    private ImageList imgList;
    private SizableTextBox txtEmailAddressFilter;
    private SizableTextBox txtPhoneNumberFilter;
    private SizableTextBox txtFirstNameFilter;
    private SizableTextBox txtLastNameFilter;
    private ContextMenuStrip ctxContacts;
    private ToolStripMenuItem mnuContactsPrint;
    private ToolStripMenuItem mnuContactsRemove;
    private ToolStripMenuItem mnuContactsExport;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem mnuContactsView;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem mnuContactsSelectAll;
    private Panel pnlTaskActions;
    private Label lblLetterDocument;
    private Label lblEmailDocument;
    private DateFilterBox dfScheduledDateFilter;

    public event CampaignDetailForm.UpdateTaskCount UpdateTaskCount;

    public CampaignStepsTab()
    {
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
      this.pnlEmailTask.Visible = false;
      this.pnlFaxLetterTask.Visible = false;
      this.pnlPhoneCallReminderTask.Visible = false;
      this.pnlFaxLetterTask.Location = this.pnlEmailTask.Location;
      this.pnlPhoneCallReminderTask.Location = this.pnlEmailTask.Location;
      this.gvContacts.Columns[0].FilterControl = (Control) this.txtLastNameFilter;
      this.txtLastNameFilter.LostFocus += new EventHandler(this.txtLastNameFilter_LostFocus);
      this.txtLastNameFilter.KeyUp += new KeyEventHandler(this.txtLastNameFilter_KeyUp);
      this.gvContacts.Columns[1].FilterControl = (Control) this.txtFirstNameFilter;
      this.txtFirstNameFilter.LostFocus += new EventHandler(this.txtFirstNameFilter_LostFocus);
      this.txtFirstNameFilter.KeyUp += new KeyEventHandler(this.txtFirstNameFilter_KeyUp);
      this.gvContacts.Columns[2].FilterControl = (Control) this.dfScheduledDateFilter;
      this.dfScheduledDateFilter.FilterChanged += new EventHandler(this.dfScheduledDateFilter_FilterChanged);
      this.dfScheduledDateFilter.KeyUp += new KeyEventHandler(this.dfScheduledDateFilter_KeyUp);
      this.gvContacts.Columns[3].FilterControl = (Control) this.txtPhoneNumberFilter;
      TextBoxFormatter.Attach(this.txtPhoneNumberFilter.TextBox, TextBoxContentRule.PhoneNumber);
      this.txtPhoneNumberFilter.LostFocus += new EventHandler(this.txtPhoneNumberFilter_LostFocus);
      this.txtPhoneNumberFilter.KeyUp += new KeyEventHandler(this.txtPhoneNumberFilter_KeyUp);
      this.gvContacts.Columns[4].FilterControl = (Control) this.txtEmailAddressFilter;
      this.txtEmailAddressFilter.LostFocus += new EventHandler(this.txtEmailAddressFilter_LostFocus);
      this.txtEmailAddressFilter.KeyUp += new KeyEventHandler(this.txtEmailAddressFilter_KeyUp);
    }

    public void PopulateControls()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      this.gvSteps.BeginUpdate();
      this.gvSteps.Items.Clear();
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < activeCampaign.CampaignSteps.Count; ++index)
      {
        CampaignStep campaignStep = activeCampaign.CampaignSteps[index];
        if (campaignStep.CampaignActivities == null)
          this.campaignData.GetActivity(campaignStep);
        num2 += campaignStep.StepOffset;
        this.gvSteps.Items.Add(new GVItem((object) campaignStep.StepNumber)
        {
          SubItems = {
            new GVSubItem() { Value = (object) campaignStep.StepName },
            new GVSubItem()
            {
              Value = (object) this.activityNames.GetName((object) campaignStep.ActivityType)
            },
            new GVSubItem()
            {
              Value = (object) (campaignStep.StepOffset.ToString() + " days")
            },
            new GVSubItem()
            {
              Value = (object) campaignStep.TasksDueCount.ToString(),
              ForeColor = Color.FromArgb(239, 0, 0)
            }
          },
          Tag = (object) campaignStep
        });
        this.gvContacts.Sort(0, SortOrder.Descending);
        if (this.campaignData.ActiveCampaignStep.CampaignStepId == campaignStep.CampaignStepId)
          num1 = index;
      }
      if (0 < this.gvSteps.Items.Count)
      {
        this.gvSteps.Items[num1].Selected = true;
        this.gvSteps.EnsureVisible(num1);
      }
      this.gvSteps.EndUpdate();
      this.lblTotalSteps.Text = "Steps (" + activeCampaign.CampaignStepCount.ToString() + ")";
      if (activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        this.gvContacts.Columns[3].Text = "Home Phone";
        this.gvContacts.Columns[4].Text = "Home Email";
      }
      else
      {
        this.gvContacts.Columns[3].Text = "Business Phone";
        this.gvContacts.Columns[4].Text = "Business Email";
      }
    }

    private void populateContactList()
    {
      this.lblTaskCount.Text = "<b>Tasks </b> <b><c value=\"239, 0, 0\">(0)</c></b>";
      this.gvContacts.Items.Clear();
      this.setActivityState(false);
      if (this.gvSteps.SelectedItems.Count == 0)
        return;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      if (this.cbIncludeFutureTasks.Checked && activeCampaignStep.IsDueItemsOnly || !this.cbIncludeFutureTasks.Checked && !activeCampaignStep.IsDueItemsOnly)
      {
        activeCampaignStep.IsDueItemsOnly = !this.cbIncludeFutureTasks.Checked;
        activeCampaignStep.CampaignActivities = (CampaignActivityCollection) null;
      }
      if (activeCampaignStep.CampaignActivities == null)
        this.campaignData.GetActivity(activeCampaignStep);
      this.gvContacts.BeginUpdate();
      this.OnActivitySelectedEvent(new ActivitySelectedEventArgs(false));
      for (int index = 0; index < activeCampaignStep.CampaignActivities.Count; ++index)
      {
        string contactProperty1 = (string) activeCampaignStep.CampaignActivities[index].ContactProperties["Contact.LastName"];
        if (!(string.Empty != this.lastNameFilterValue) || contactProperty1.StartsWith(this.lastNameFilterValue, StringComparison.OrdinalIgnoreCase))
        {
          string contactProperty2 = (string) activeCampaignStep.CampaignActivities[index].ContactProperties["Contact.FirstName"];
          if (!(string.Empty != this.firstNameFilterValue) || contactProperty2.StartsWith(this.firstNameFilterValue, StringComparison.OrdinalIgnoreCase))
          {
            DateTime date = activeCampaignStep.CampaignActivities[index].ScheduledDate.Date;
            if (!(DateTime.MinValue != this.scheduledDateFilterValue) || !(date != this.scheduledDateFilterValue))
            {
              string key1 = activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "Contact.HomePhone" : "Contact.WorkPhone";
              string contactProperty3 = (string) activeCampaignStep.CampaignActivities[index].ContactProperties[key1];
              if (!(string.Empty != this.phoneNumberFilterValue) || contactProperty3.StartsWith(this.phoneNumberFilterValue, StringComparison.OrdinalIgnoreCase))
              {
                string key2 = activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "Contact.PersonalEmail" : "Contact.BizEmail";
                string contactProperty4 = (string) activeCampaignStep.CampaignActivities[index].ContactProperties[key2];
                if (!(string.Empty != this.emailAddressFilterValue) || contactProperty4.StartsWith(this.emailAddressFilterValue, StringComparison.OrdinalIgnoreCase))
                {
                  GVItem gvItem = new GVItem(contactProperty1);
                  gvItem.SubItems.Add(new GVSubItem()
                  {
                    Value = (object) contactProperty2
                  });
                  GVSubItem gvSubItem1 = new GVSubItem();
                  switch ((date - DateTime.Today).Days)
                  {
                    case -1:
                      gvSubItem1.Value = (object) "Yesterday";
                      break;
                    case 0:
                      gvSubItem1.Value = (object) "Today";
                      break;
                    case 1:
                      gvSubItem1.Value = (object) "Tomorrow";
                      break;
                    default:
                      gvSubItem1.Value = (object) date.ToShortDateString();
                      break;
                  }
                  gvSubItem1.ForeColor = Color.FromArgb(239, 0, 0);
                  gvItem.SubItems.Add(gvSubItem1);
                  GVSubItem gvSubItem2 = new GVSubItem();
                  if (string.Empty != contactProperty3)
                  {
                    ImageLink imageLink = new ImageLink((Element) new TextElement(contactProperty3), this.imgList.Images["Phone"], this.imgList.Images["PhoneMouseOver"], new EventHandler(this.lnkPhoneNumber_Click));
                    imageLink.Tag = (object) activeCampaignStep.CampaignActivities[index];
                    gvSubItem2.Value = (object) imageLink;
                  }
                  gvItem.SubItems.Add(gvSubItem2);
                  GVSubItem gvSubItem3 = new GVSubItem();
                  if (string.Empty != contactProperty4)
                  {
                    ImageLink imageLink = new ImageLink((Element) new TextElement(contactProperty4), this.imgList.Images["Email"], this.imgList.Images["EmailMouseOver"], new EventHandler(this.lnkEmailAddress_Click));
                    imageLink.Tag = (object) activeCampaignStep.CampaignActivities[index];
                    gvSubItem3.Value = (object) imageLink;
                  }
                  gvItem.SubItems.Add(gvSubItem3);
                  gvItem.Tag = (object) activeCampaignStep.CampaignActivities[index];
                  this.gvContacts.Items.Add(gvItem);
                  activeCampaignStep.CampaignActivities[index].IsSelected = false;
                }
              }
            }
          }
        }
      }
      if (0 < this.gvContacts.Items.Count)
      {
        this.gvContacts.EnsureVisible(0);
        this.selectAllContacts();
      }
      this.gvContacts.EndUpdate();
      this.cbIncludeFutureTasks.Enabled = true;
      this.lblTaskCount.Text = "<b>Tasks </b> <c value=\"239, 0, 0\">(" + this.gvContacts.Items.Count.ToString() + ")</c>";
    }

    private void selectAllContacts()
    {
      this.gvContacts.BeginUpdate();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContacts.Items)
        gvItem.Selected = true;
      this.gvContacts.EndUpdate();
    }

    private void populateStepActivity()
    {
      this.btnTaskAction1.Text = string.Empty;
      this.btnTaskAction2.Text = string.Empty;
      this.btnTaskAction3.Text = string.Empty;
      this.pnlTaskActions.Visible = false;
      this.lblStepName.Text = string.Empty;
      this.lblStepDescription.Text = string.Empty;
      this.lblTaskType.Text = string.Empty;
      this.lblTaskInformation.Text = string.Empty;
      this.pnlEmailTask.Visible = false;
      this.pnlFaxLetterTask.Visible = false;
      this.pnlPhoneCallReminderTask.Visible = false;
      if (this.gvSteps.SelectedItems.Count == 0)
        return;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      this.fitLabelText(this.lblStepName, activeCampaignStep.StepName);
      this.fitLabelText(this.lblStepDescription, activeCampaignStep.StepDesc);
      this.lblTaskType.Text = this.activityNames.GetName((object) activeCampaignStep.ActivityType);
      this.pnlTaskActions.Visible = true;
      switch (activeCampaignStep.ActivityType)
      {
        case ActivityType.Email:
          this.populateEmailTaskControls();
          this.pnlEmailTask.Visible = true;
          break;
        case ActivityType.Fax:
          this.populateFaxLetterTaskControls();
          this.pnlFaxLetterTask.Visible = true;
          break;
        case ActivityType.Letter:
          this.populateFaxLetterTaskControls();
          this.pnlFaxLetterTask.Visible = true;
          break;
        case ActivityType.PhoneCall:
          this.populatePhoneCallReminderTaskControls();
          this.pnlPhoneCallReminderTask.Visible = true;
          break;
        case ActivityType.Reminder:
          this.populatePhoneCallReminderTaskControls();
          this.pnlPhoneCallReminderTask.Visible = true;
          break;
      }
    }

    private void populateEmailTaskControls()
    {
      this.setTaskHeader(string.Empty, "Email Preview", "Send Email");
      this.lblTaskInformation.Text = "The following document will be sent to the selected contacts with the subject shown.";
      this.txtEmailSubject.Text = this.campaignData.ActiveCampaignStep.Subject;
      this.lblEmailDocument.Text = this.formatDocumentName(this.campaignData.ActiveCampaignStep.DocumentId);
      this.txtEmailSubject.ReadOnly = this.gvContacts.SelectedItems.Count == 0;
    }

    private void setTaskHeader(string buttonText1, string buttonText2, string buttonText3)
    {
      int controlRight = this.setTaskButton(this.btnTaskAction2, this.setTaskButton(this.btnTaskAction3, this.pnlTaskActions.Width - 5, buttonText3), buttonText2);
      int num1 = this.setTaskButton(this.btnTaskAction1, controlRight, buttonText1);
      int num2 = this.setControlLocation((Control) this.icnDeleteTask, this.setControlLocation((Control) this.icnExcelTask, this.setControlLocation((Control) this.icnPrintTask, this.setControlLocation((Control) this.verticalSeparator1, num1 == controlRight ? num1 - 3 : num1 - 5) - 5) - 5) - 5) - 5;
    }

    private int setTaskButton(Button btnTaskAction, int controlRight, string buttonText)
    {
      if (string.Empty == buttonText)
      {
        btnTaskAction.Visible = false;
        return controlRight;
      }
      btnTaskAction.Visible = true;
      btnTaskAction.Text = buttonText;
      using (Graphics graphics = btnTaskAction.CreateGraphics())
      {
        float width = graphics.MeasureString(buttonText, btnTaskAction.Font).Width;
        btnTaskAction.Width = (int) Math.Ceiling((double) width) + 13;
      }
      return this.setControlLocation((Control) btnTaskAction, controlRight);
    }

    private int setControlLocation(Control control, int controlRight)
    {
      control.Left = controlRight - control.Width;
      return control.Left;
    }

    private string formatDocumentName(string fileName)
    {
      string str = fileName;
      if (str.EndsWith(".DOC", StringComparison.OrdinalIgnoreCase))
        str = str.Substring(0, str.Length - ".DOC".Length);
      return str.Replace("Public:", "Public").Replace("Personal:", "Personal");
    }

    public void populateFaxLetterTaskControls()
    {
      this.setTaskHeader("Print Preview", "Print", "Complete");
      this.lblTaskInformation.Text = "The following document will be printed with the selected contact's information.";
      this.lblLetterDocument.Text = this.formatDocumentName(this.campaignData.ActiveCampaignStep.DocumentId);
    }

    public void populatePhoneCallReminderTaskControls()
    {
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      this.setTaskHeader("Add to Tasks", "Add to Calendar", "Complete");
      this.lblTaskInformation.Text = "The selected contacts will be scheduled on the specified user's Calendar or Tasks list.";
      this.dtpScheduleDate.Enabled = this.gvContacts.SelectedItems.Count != 0;
      this.dtpScheduleDate.MinValue = DateTime.Today;
      this.dtpScheduleDate.Value = DateTime.Today;
    }

    private void emailContactsSelected(bool isEnabled)
    {
      this.btnTaskAction2.Enabled = isEnabled;
      this.btnTaskAction3.Enabled = isEnabled;
      this.txtEmailSubject.Enabled = isEnabled;
    }

    private void letterContactsSelected(bool isEnabled)
    {
      this.btnTaskAction1.Enabled = isEnabled;
      this.btnTaskAction2.Enabled = isEnabled;
      this.btnTaskAction3.Enabled = isEnabled;
    }

    private void scheduleContactsSelected(bool isEnabled)
    {
      this.btnTaskAction1.Enabled = isEnabled;
      this.btnTaskAction2.Enabled = isEnabled;
      this.btnTaskAction3.Enabled = isEnabled;
      this.dtpScheduleDate.Enabled = isEnabled;
    }

    private void viewContact()
    {
      if (1 != this.gvContacts.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      CampaignActivity tag = (CampaignActivity) this.gvContacts.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo();
      attendeeInfo.AssignInfo(tag.ContactId, this.campaignData.ActiveCampaign.ContactType);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
      if (!attendeeInfo.GoToContact)
        return;
      Session.MainScreen.NavigateToContact(attendeeInfo.SelectedContact);
    }

    private void printContacts()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      int[] ids = this.setActivitySelection(ActivityStatus.Expected);
      if (ids.Length == 0)
        return;
      ContactPrintDialog contactPrintDialog = new ContactPrintDialog(activeCampaign.ContactType);
      if (DialogResult.Cancel == contactPrintDialog.ShowDialog((IWin32Window) this))
        return;
      PdfFormFacade pdfFormFacade = new PdfFormFacade();
      if (activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
        pdfFormFacade.ProcessContactPrint(Session.ContactManager.GetBorrowers(ids), contactPrintDialog.PrintSummary, contactPrintDialog.PrintPages, contactPrintDialog.PrintPreview);
      else
        pdfFormFacade.ProcessContactPrint(Session.ContactManager.GetBizPartners(ids), contactPrintDialog.PrintSummary, contactPrintDialog.PrintPages, contactPrintDialog.PrintPreview);
      this.Focus();
    }

    private void removeContacts()
    {
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      if (this.setActivitySelection(ActivityStatus.Removed).Length == 0 || DialogResult.No == Utils.Dialog((IWin32Window) this, "The status for the selected contacts will be set to 'Removed'.\nAre you sure you want to remove the selected contacts from this step?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      this.updateActivityStatus();
      if (this.UpdateTaskCount == null)
        return;
      this.UpdateTaskCount();
    }

    private void updateActivityStatus() => this.updateActivityStatus(string.Empty);

    private void updateActivityStatus(string activityNote)
    {
      this.campaignData.UpdateActivity(this.campaignData.ActiveCampaignStep, activityNote);
      this.campaignData.ActiveCampaignStep.IsDueItemsOnly = !this.cbIncludeFutureTasks.Checked;
      this.PopulateControls();
    }

    private void exportContacts()
    {
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      int[] contactIds = this.setActivitySelection(ActivityStatus.Expected);
      if (contactIds.Length == 0)
        return;
      new ContactExportWizard(this.campaignData.ActiveCampaign.ContactType, contactIds).ShowDialog((IWin32Window) this.ParentForm);
    }

    private int[] setActivitySelection(ActivityStatus activityStatus)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContacts.Items)
      {
        CampaignActivity tag = (CampaignActivity) gvItem.Tag;
        if (gvItem.Selected)
        {
          arrayList.Add((object) tag.ContactId);
          tag.IsSelected = true;
          tag.PendingStatus = activityStatus;
        }
        else
          tag.IsSelected = false;
      }
      if (arrayList.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no contacts selected for processing.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      return (int[]) arrayList.ToArray(typeof (int));
    }

    private void processEmailLetterActivity(bool isPreviewRequest)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      int[] numArray = this.setActivitySelection(ActivityStatus.Expected);
      if (numArray.Length == 0)
        return;
      FileSystemEntry document;
      try
      {
        document = FileSystemEntry.Parse(activeCampaignStep.DocumentId);
      }
      catch (ArgumentException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, string.Format("Document '{0}' {1}, Please fix the problem and try again.", (object) activeCampaignStep.DocumentId, (object) ex.Message), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      if (activeCampaignStep.ActivityType == ActivityType.Email && !isPreviewRequest && string.Empty == this.txtEmailSubject.Text.Trim())
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please specify a 'Subject' for email message.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int[] validContactIds;
        int[] optoutContactIds;
        if (!isPreviewRequest)
        {
          if (!this.screenContacts(out validContactIds, out optoutContactIds, (List<ActivityContactInfo>) null))
            return;
        }
        else
        {
          validContactIds = new int[1]{ numArray[0] };
          optoutContactIds = new int[0];
        }
        bool flag = true;
        if (validContactIds.Length != 0)
        {
          using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Campaign.Activity", "Send email to contacts in campaign", true, 743, nameof (processEmailLetterActivity), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\Campaign\\CampaignStepsTab.cs"))
          {
            performanceMeter.AddVariable("Campaign", (object) this.campaignData.ActiveCampaign.CampaignName);
            performanceMeter.AddVariable("Step", (object) this.campaignData.ActiveCampaignStep.StepName);
            performanceMeter.AddVariable("ContactType", (object) this.campaignData.ActiveCampaign.ContactType);
            performanceMeter.AddVariable("Activity", (object) activeCampaignStep.ActivityType);
            performanceMeter.AddVariable("Count", (object) validContactIds.Length);
            performanceMeter.AddVariable("Document", (object) document.Name);
            performanceMeter.AddVariable("Preview", (object) isPreviewRequest);
            flag = this.sendEmailLetter(isPreviewRequest, validContactIds, document);
          }
        }
        if (optoutContactIds.Length != 0)
          this.updateActivityStatus();
        if (!flag || activeCampaignStep.ActivityType != ActivityType.Email || isPreviewRequest)
          return;
        this.setPendingToComplete();
        this.updateActivityStatus();
      }
    }

    private void setPendingToComplete()
    {
      foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
      {
        CampaignActivity tag = (CampaignActivity) selectedItem.Tag;
        if (tag.IsSelected && ActivityStatus.Expected == tag.PendingStatus)
          tag.PendingStatus = ActivityStatus.Completed;
      }
    }

    private int getMailMergeJobThreshold()
    {
      try
      {
        return int.Parse(EnConfigurationSettings.AppSettings["MailMergeJobThreshold"]);
      }
      catch
      {
        return 5;
      }
    }

    private bool submitMailMergeJob(int[] contactIds, FileSystemEntry document)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      Cursor.Current = Cursors.WaitCursor;
      bool flag = ContactUtils.SubmitMailMergeJob(contactIds, activeCampaign.ContactType, document, this.txtEmailSubject.Text.Trim(), (string[]) null, Session.UserID);
      Cursor.Current = Cursors.Default;
      if (flag)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A Mail Merge request has been submitted for the " + (object) contactIds.Length + " selected contact(s).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      return flag;
    }

    private bool sendEmailLetter(bool isPreviewRequest, int[] contactIds, FileSystemEntry document)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      bool flag = false;
      OutlookServices outlookServices = (OutlookServices) null;
      try
      {
        if (activeCampaignStep.ActivityType == ActivityType.Email)
        {
          if (ContactUtils.GetCurrentMailDeliveryMethod() == EmailDeliveryMethod.Outlook)
            outlookServices = new OutlookServices();
          ContactUtils.ContactIDs = contactIds;
          ContactUtils.TypeOfContacts = activeCampaign.ContactType;
          ContactUtils.LetterEntry = document;
          ContactUtils.IsPrintPreview = isPreviewRequest;
          ContactUtils.IsEmailMerge = !isPreviewRequest;
          ContactUtils.EmailSubject = this.txtEmailSubject.Text.Trim();
          ContactUtils.SenderUserID = Session.UserID;
          flag = DialogResult.OK == new ProgressDialog(!isPreviewRequest ? "Sending Emails to Contacts" : "Generating Preview", new AsynchronousProcess(ContactUtils.DoAsyncMailMerge), (object) null, false).ShowDialog((IWin32Window) this);
          if (!isPreviewRequest & flag)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "Email was sent to selected contacts.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        else
          flag = ContactUtils.DoMailMerge(contactIds, activeCampaign.ContactType, document, isPreviewRequest, false, string.Empty, (string[]) null, Session.UserID);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        outlookServices?.Dispose();
      }
      return flag;
    }

    private bool screenContacts(
      out int[] validContactIds,
      out int[] optoutContactIds,
      List<ActivityContactInfo> validContacts)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      string key = activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "Contact.PersonalEmail" : "Contact.BizEmail";
      foreach (CampaignActivity campaignActivity in (CollectionBase) activeCampaignStep.CampaignActivities)
      {
        if (campaignActivity.IsSelected)
        {
          if (activeCampaignStep.ActivityType == ActivityType.Email && (bool) campaignActivity.ContactProperties["Contact.NoSpam"] || ActivityType.PhoneCall == activeCampaignStep.ActivityType && (bool) campaignActivity.ContactProperties["Contact.NoCall"] || ActivityType.Fax == activeCampaignStep.ActivityType && (bool) campaignActivity.ContactProperties["Contact.NoFax"])
          {
            arrayList2.Add((object) campaignActivity.ContactId);
            campaignActivity.PendingStatus = ActivityStatus.OptedOut;
          }
          else if (activeCampaignStep.ActivityType == ActivityType.Email && string.Empty == campaignActivity.ContactProperties[key].ToString())
          {
            arrayList3.Add((object) campaignActivity.ContactId);
            campaignActivity.PendingStatus = ActivityStatus.Removed;
          }
          else
          {
            arrayList1.Add((object) campaignActivity.ContactId);
            validContacts?.Add(new ActivityContactInfo(campaignActivity, activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? CategoryType.Borrower : CategoryType.BizPartner));
          }
        }
      }
      validContactIds = (int[]) arrayList1.ToArray(typeof (int));
      optoutContactIds = (int[]) arrayList2.ToArray(typeof (int));
      bool skipOptOut = false;
      bool skipNoAddress = false;
      bool flag = false;
      if (0 < arrayList2.Count || 0 < arrayList3.Count)
      {
        ActivityValidationDialog validationDialog = new ActivityValidationDialog(activeCampaignStep.ActivityType, (int[]) arrayList2.ToArray(typeof (int)), (int[]) arrayList3.ToArray(typeof (int)));
        if (DialogResult.Cancel == validationDialog.ShowDialog())
          return false;
        skipOptOut = validationDialog.SkipOptOutContacts;
        skipNoAddress = validationDialog.SkipNoAddressContacts;
        flag = validationDialog.ProceedOptOutContacts;
        if (skipOptOut || skipNoAddress)
          this.skipContacts(skipOptOut, skipNoAddress);
        if (flag)
          this.optOutContacts(true);
      }
      return arrayList1.Count != 0 || !(arrayList2.Count == 0 | skipOptOut) || !(arrayList3.Count == 0 | skipNoAddress) || !(arrayList2.Count == 0 | flag);
    }

    private void skipContacts(bool skipOptOut, bool skipNoAddress)
    {
      foreach (CampaignActivity campaignActivity in (CollectionBase) this.campaignData.ActiveCampaignStep.CampaignActivities)
      {
        if (campaignActivity.IsSelected)
        {
          if (ActivityStatus.OptedOut == campaignActivity.PendingStatus & skipOptOut)
            campaignActivity.IsSelected = false;
          else if (ActivityStatus.Removed == campaignActivity.PendingStatus & skipNoAddress)
            campaignActivity.IsSelected = false;
        }
      }
    }

    private void optOutContacts(bool optOut)
    {
      foreach (CampaignActivity campaignActivity in (CollectionBase) this.campaignData.ActiveCampaignStep.CampaignActivities)
      {
        if (campaignActivity.IsSelected && ActivityStatus.OptedOut == campaignActivity.PendingStatus & optOut)
          campaignActivity.PendingStatus = ActivityStatus.OptedOut;
      }
    }

    private void displayTaskDialog()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      if (this.setActivitySelection(ActivityStatus.Expected).Length == 0 || !this.processTasksIndividually())
        return;
      this.updateActivityStatus();
    }

    private bool processTasksIndividually()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      int[] validContactIds;
      if (!this.screenContacts(out validContactIds, out int[] _, (List<ActivityContactInfo>) null))
        return false;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      this.addInaccessibleContacts = CampaignStepsTab.YesNoUnknown.Unknown;
      TaskInfo taskInfo = (TaskInfo) null;
      UserSelectionMethod userSelectionMethod = UserSelectionMethod.ContactOwner;
      string selectedUserId = string.Empty;
      foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
      {
        CampaignActivity tag = (CampaignActivity) selectedItem.Tag;
        if (tag.IsSelected && ActivityStatus.Expected != tag.PendingStatus)
          flag1 = true;
        if (tag.IsSelected && ActivityStatus.Expected == tag.PendingStatus)
        {
          if (flag3)
          {
            tag.IsSelected = false;
          }
          else
          {
            ContactInfo contactInfo = new ContactInfo(tag.ContactProperties["Contact.FirstName"].ToString() + " " + tag.ContactProperties["Contact.LastName"], tag.ContactId.ToString(), activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? CategoryType.Borrower : CategoryType.BizPartner);
            if (flag2 && taskInfo != null)
            {
              if (this.addTask(taskInfo, contactInfo, userSelectionMethod, selectedUserId, validContactIds) && TaskStatus.Completed == taskInfo.TaskStatus)
              {
                flag1 = true;
                tag.PendingStatus = ActivityStatus.Completed;
              }
            }
            else
            {
              DateTime date = this.dtpScheduleDate.Value.Date;
              taskInfo = new TaskInfo(0, this.campaignData.UserId, TaskType.CampaignTask, TaskStatus.NotStarted, activeCampaignStep.Subject, activeCampaignStep.Comments, date, date, activeCampaignStep.TaskPriority, DateTime.MaxValue, DateTime.MaxValue, new ContactInfo[1]
              {
                contactInfo
              }, tag.CampaignStepId);
              TaskDetailsForm taskDetailsForm = new TaskDetailsForm(taskInfo);
              taskDetailsForm.Behavior = TaskDetailsForm.FormBehavior.DisableContactsButton | TaskDetailsForm.FormBehavior.DisableDeleteButton | TaskDetailsForm.FormBehavior.DisableAddTask | TaskDetailsForm.FormBehavior.EnableUserSelection | TaskDetailsForm.FormBehavior.EnableMultiSelection;
              if (DialogResult.OK == taskDetailsForm.ShowDialog())
              {
                flag2 = taskDetailsForm.AddAllSelected;
                taskInfo = taskDetailsForm.Task;
                userSelectionMethod = taskDetailsForm.UserSelectionMethod;
                selectedUserId = taskDetailsForm.SelectedUser;
                if (this.addTask(taskInfo, contactInfo, userSelectionMethod, selectedUserId, validContactIds))
                {
                  if (TaskStatus.Completed == taskInfo.TaskStatus)
                  {
                    flag1 = true;
                    tag.PendingStatus = ActivityStatus.Completed;
                  }
                }
                else
                  tag.IsSelected = false;
              }
              else
              {
                tag.IsSelected = false;
                flag3 = taskDetailsForm.CancelAllSelected;
              }
            }
          }
        }
      }
      return flag1;
    }

    private bool addTask(
      TaskInfo taskInfo,
      ContactInfo contactInfo,
      UserSelectionMethod userSelectionMethod,
      string selectedUserId,
      int[] validContactIds)
    {
      if (UserSelectionMethod.SelectedUser == userSelectionMethod && Session.UserID != selectedUserId && this.addInaccessibleContacts != CampaignStepsTab.YesNoUnknown.Yes)
      {
        bool flag = this.isContactAccessible(selectedUserId, contactInfo, validContactIds);
        if (!flag && CampaignStepsTab.YesNoUnknown.Unknown == this.addInaccessibleContacts)
          this.addInaccessibleContacts = DialogResult.Yes == Utils.Dialog((IWin32Window) this, "Not all contacts will be accessible by the selected user.\nDo you want to add tasks for these contacts?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) ? CampaignStepsTab.YesNoUnknown.Yes : CampaignStepsTab.YesNoUnknown.No;
        if (!flag && CampaignStepsTab.YesNoUnknown.No == this.addInaccessibleContacts)
          return false;
      }
      TaskInfo task = new TaskInfo(0, this.getUserId(contactInfo, userSelectionMethod, selectedUserId), taskInfo.TaskType, taskInfo.TaskStatus, taskInfo.Subject, taskInfo.Notes, taskInfo.DueDate, taskInfo.StartDate, taskInfo.Priority, taskInfo.CreationTime, taskInfo.LastModified, new ContactInfo[1]
      {
        contactInfo
      }, taskInfo.CampaignStepID);
      bool flag1 = false;
      if (0 <= Session.ContactManager.InsertOrUpdateTask(task))
        flag1 = true;
      return flag1;
    }

    private bool isContactAccessible(string userId, ContactInfo contactInfo, int[] validContactIds)
    {
      if (this.accessibleIds == null || this.accessibleUserId != userId)
      {
        this.accessibleUserId = userId;
        if (contactInfo.ContactType == CategoryType.Borrower)
          this.accessibleIds = Session.ContactManager.QueryBorrowerIds(userId, new QueryCriterion[1]
          {
            (QueryCriterion) new ListValueCriterion("Contact.ContactID", (Array) validContactIds, true)
          }, RelatedLoanMatchType.None);
        else
          this.accessibleIds = Session.ContactManager.QueryBizPartnerIds(userId, new QueryCriterion[1]
          {
            (QueryCriterion) new ListValueCriterion("Contact.ContactID", (Array) validContactIds, true)
          }, RelatedLoanMatchType.None);
        if (this.accessibleIds == null)
          this.accessibleIds = new int[0];
      }
      int num;
      try
      {
        num = int.Parse(contactInfo.ContactID);
      }
      catch (Exception ex)
      {
        return false;
      }
      return 0 <= Array.BinarySearch<int>(this.accessibleIds, num);
    }

    private string getUserId(
      ContactInfo contactInfo,
      UserSelectionMethod userSelectionMethod,
      string selectedUserId)
    {
      string str = string.Empty;
      if (userSelectionMethod == UserSelectionMethod.CurrentUser)
        str = Session.UserID;
      else if (UserSelectionMethod.ContactOwner == userSelectionMethod)
      {
        if (contactInfo.ContactType == CategoryType.Borrower)
        {
          BorrowerInfo borrower = Session.ContactManager.GetBorrower(int.Parse(contactInfo.ContactID));
          if (borrower != null)
            str = borrower.OwnerID;
        }
        else
        {
          BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(int.Parse(contactInfo.ContactID));
          if (bizPartner != null)
            str = bizPartner.OwnerID;
        }
      }
      else
        str = selectedUserId;
      return !(string.Empty != str) ? Session.UserID : str;
    }

    private bool processTaskAsGroup()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      List<ActivityContactInfo> validContacts = new List<ActivityContactInfo>();
      if (!this.screenContacts(out int[] _, out int[] _, validContacts))
        return false;
      bool flag = false;
      if (0 < validContacts.Count)
      {
        DateTime date = this.dtpScheduleDate.Value.Date;
        if (DialogResult.OK == new TaskDetailsForm(new TaskInfo(0, this.campaignData.UserId, TaskType.CampaignTask, TaskStatus.NotStarted, activeCampaignStep.Subject, activeCampaignStep.Comments, date, date, activeCampaignStep.TaskPriority, DateTime.MaxValue, DateTime.MaxValue, (ContactInfo[]) validContacts.ToArray(), activeCampaignStep.CampaignStepId))
        {
          Behavior = (TaskDetailsForm.FormBehavior.DisableContactsButton | TaskDetailsForm.FormBehavior.DisableDeleteButton)
        }.ShowDialog())
        {
          flag = true;
          foreach (ActivityContactInfo activityContactInfo in validContacts)
            activityContactInfo.Activity.Status = ActivityStatus.Completed;
        }
      }
      return flag;
    }

    private void displayAppointmentDialog()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      if (this.setActivitySelection(ActivityStatus.Expected).Length == 0 || !this.processAppointmentsIndividually())
        return;
      this.updateActivityStatus();
    }

    private bool processAppointmentsIndividually()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      int[] validContactIds;
      if (!this.screenContacts(out validContactIds, out int[] _, (List<ActivityContactInfo>) null))
        return false;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      this.addInaccessibleContacts = CampaignStepsTab.YesNoUnknown.Unknown;
      Appointment appointment = (Appointment) null;
      UserSelectionMethod userSelectionMethod = UserSelectionMethod.ContactOwner;
      string selectedUserId = string.Empty;
      foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
      {
        CampaignActivity tag = (CampaignActivity) selectedItem.Tag;
        if (tag.IsSelected && ActivityStatus.Expected != tag.PendingStatus)
          flag1 = true;
        if (tag.IsSelected && ActivityStatus.Expected == tag.PendingStatus)
        {
          if (flag3)
          {
            tag.IsSelected = false;
          }
          else
          {
            ContactInfo contactInfo = new ContactInfo(tag.ContactProperties["Contact.FirstName"].ToString() + " " + tag.ContactProperties["Contact.LastName"], tag.ContactId.ToString(), activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? CategoryType.Borrower : CategoryType.BizPartner);
            if (flag2 && appointment != null)
            {
              if (!this.addAppointment(appointment, contactInfo, userSelectionMethod, selectedUserId, validContactIds))
                tag.IsSelected = false;
            }
            else
            {
              DateTime date = this.dtpScheduleDate.Value.Date;
              appointment = new Appointment(date, date.AddMinutes(15.0));
              appointment.Subject = this.campaignData.ActiveCampaignStep.Subject;
              appointment.Description = this.campaignData.ActiveCampaignStep.Comments;
              appointment.BarColor = this.campaignData.ActiveCampaignStep.BarColor;
              appointment.AllDayEvent = true;
              this.ultraCalendarInfo.Appointments.Add(appointment);
              CustomAppointmentDialog appointmentDialog = new CustomAppointmentDialog(this.ultraCalendarInfo, appointment, false, CSMessage.AccessLevel.Full, false, Session.UserID, RecurrenceEditType.UserSelect);
              appointmentDialog.Behavior = CustomAppointmentDialog.FormBehavior.DisableAddContactsButton | CustomAppointmentDialog.FormBehavior.DisableDeleteContactsButton | CustomAppointmentDialog.FormBehavior.DisableRecurranceButton | CustomAppointmentDialog.FormBehavior.DisableAllDayEventCheckbox | CustomAppointmentDialog.FormBehavior.EnableUserSelection | CustomAppointmentDialog.FormBehavior.EnableMultiSelection;
              appointmentDialog.AppointmentContacts = new ContactInfo[1]
              {
                contactInfo
              };
              appointmentDialog.AppointmentSubject = appointment.Subject;
              appointmentDialog.AppointmentDescription = appointment.Description;
              appointmentDialog.IsAppointmentAllDayEvent = appointment.AllDayEvent;
              appointmentDialog.AppointmentStartTime = appointment.StartDateTime.Date;
              appointmentDialog.AppointmentEndTime = appointment.EndDateTime.Date;
              appointmentDialog.AppointmentBarColor = appointment.BarColor;
              if (appointment.Reminder.Enabled)
              {
                appointmentDialog.IsReminderSet = appointment.Reminder.Enabled;
                appointmentDialog.DisplayInterval = appointment.Reminder.DisplayInterval;
                appointmentDialog.DisplayIntervalUnits = appointment.Reminder.DisplayIntervalUnits;
              }
              if (DialogResult.OK == appointmentDialog.ShowDialog())
              {
                flag2 = appointmentDialog.AddAllSelected;
                appointment.Subject = appointmentDialog.AppointmentSubject;
                appointment.Description = appointmentDialog.AppointmentDescription;
                appointment.AllDayEvent = appointmentDialog.IsAppointmentAllDayEvent;
                appointment.StartDateTime = appointmentDialog.AppointmentStartTime;
                appointment.EndDateTime = appointmentDialog.AppointmentEndTime;
                appointment.Location = appointmentDialog.AppointmentLocation;
                appointment.BarColor = appointmentDialog.AppointmentBarColor;
                appointment.Reminder.Enabled = appointmentDialog.IsReminderSet;
                appointment.Reminder.DisplayInterval = 0 < appointmentDialog.DisplayInterval ? appointmentDialog.DisplayInterval : 0;
                appointment.Reminder.DisplayIntervalUnits = appointmentDialog.DisplayIntervalUnits;
                userSelectionMethod = appointmentDialog.UserSelectionMethod;
                selectedUserId = appointmentDialog.SelectedUser;
                if (!this.addAppointment(appointment, contactInfo, userSelectionMethod, selectedUserId, validContactIds))
                  tag.IsSelected = false;
              }
              else
              {
                tag.IsSelected = false;
                flag3 = appointmentDialog.CancelAllSelected;
              }
            }
          }
        }
      }
      return flag1;
    }

    private bool addAppointment(
      Appointment appointment,
      ContactInfo contactInfo,
      UserSelectionMethod userSelectionMethod,
      string selectedUserId,
      int[] validContactIds)
    {
      if (UserSelectionMethod.SelectedUser == userSelectionMethod && Session.UserID != selectedUserId && this.addInaccessibleContacts != CampaignStepsTab.YesNoUnknown.Yes)
      {
        bool flag = this.isContactAccessible(selectedUserId, contactInfo, validContactIds);
        if (!flag && CampaignStepsTab.YesNoUnknown.Unknown == this.addInaccessibleContacts)
          this.addInaccessibleContacts = DialogResult.Yes == Utils.Dialog((IWin32Window) this, "Not all contacts will be accessible by the selected user.\nDo you want to add appointments for these contacts?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) ? CampaignStepsTab.YesNoUnknown.Yes : CampaignStepsTab.YesNoUnknown.No;
        if (!flag && CampaignStepsTab.YesNoUnknown.No == this.addInaccessibleContacts)
          return false;
      }
      EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo apptInfo = new EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo(appointment.Subject, appointment.StartDateTime, appointment.EndDateTime, appointment.Description, appointment.AllDayEvent, appointment.Reminder.Enabled, appointment.Reminder.DisplayInterval, (int) appointment.Reminder.DisplayIntervalUnits, appointment.IsVariance ? appointment.RecurringAppointmentRoot.DataKey.ToString() : string.Empty, Guid.Empty != appointment.RecurrenceId ? appointment.RecurrenceId.ToString() : string.Empty, appointment.Recurrence != null ? appointment.Recurrence.Save() : (byte[]) null, appointment.IsRemoved, appointment.OriginalStartDateTime, appointment.DataKey != null ? appointment.DataKey.ToString() : string.Empty, (byte[]) null, this.getUserId(contactInfo, userSelectionMethod, selectedUserId), DateTime.MinValue, CSMessage.AccessLevel.Full);
      bool flag1 = false;
      try
      {
        int num = Session.CalendarManager.AddNewBlankRecordForAppointment(string.Empty);
        appointment.DataKey = (object) Convert.ToString(num, 10);
        apptInfo.DataKey = appointment.DataKey.ToString();
        apptInfo.AllProperties = appointment.Save();
        Session.CalendarManager.UpdateAppointment(apptInfo, new ContactInfo[1]
        {
          contactInfo
        });
        flag1 = true;
      }
      catch (Exception ex)
      {
        string message = ex.Message;
      }
      return flag1;
    }

    private bool processAppointmentAsGroup()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      List<ActivityContactInfo> validContacts = new List<ActivityContactInfo>();
      if (!this.screenContacts(out int[] _, out int[] _, validContacts))
        return false;
      bool flag = false;
      if (0 < validContacts.Count)
      {
        DateTime date = this.dtpScheduleDate.Value.Date;
        Appointment appointment = new Appointment(date, date.AddMinutes(15.0));
        appointment.Subject = this.campaignData.ActiveCampaignStep.Subject;
        appointment.Description = this.campaignData.ActiveCampaignStep.Comments;
        appointment.BarColor = this.campaignData.ActiveCampaignStep.BarColor;
        appointment.AllDayEvent = true;
        this.ultraCalendarInfo.Appointments.Add(appointment);
        CustomAppointmentDialog appointmentDialog = new CustomAppointmentDialog(this.ultraCalendarInfo, appointment, false, CSMessage.AccessLevel.Full, false, Session.UserID, RecurrenceEditType.UserSelect);
        appointmentDialog.Behavior = CustomAppointmentDialog.FormBehavior.DisableAddContactsButton | CustomAppointmentDialog.FormBehavior.DisableDeleteContactsButton | CustomAppointmentDialog.FormBehavior.DisableRecurranceButton;
        appointmentDialog.AppointmentContacts = (ContactInfo[]) validContacts.ToArray();
        appointmentDialog.AppointmentSubject = appointment.Subject;
        appointmentDialog.AppointmentDescription = appointment.Description;
        appointmentDialog.AppointmentStartTime = appointment.StartDateTime.Date;
        appointmentDialog.AppointmentEndTime = appointment.EndDateTime.Date;
        appointmentDialog.IsAppointmentAllDayEvent = appointment.AllDayEvent;
        appointmentDialog.AppointmentBarColor = appointment.BarColor;
        if (appointment.Reminder.Enabled)
        {
          appointmentDialog.IsReminderSet = appointment.Reminder.Enabled;
          appointmentDialog.DisplayInterval = appointment.Reminder.DisplayInterval;
          appointmentDialog.DisplayIntervalUnits = appointment.Reminder.DisplayIntervalUnits;
        }
        if (DialogResult.OK == appointmentDialog.ShowDialog())
        {
          int num = Session.CalendarManager.AddNewBlankRecordForAppointment("");
          appointment.DataKey = (object) Convert.ToString(num, 10);
          appointment.Subject = appointmentDialog.AppointmentSubject;
          appointment.Description = appointmentDialog.AppointmentDescription;
          appointment.Location = appointmentDialog.AppointmentLocation;
          appointment.BarColor = appointmentDialog.AppointmentBarColor;
          appointment.AllDayEvent = appointmentDialog.IsAppointmentAllDayEvent;
          appointment.Reminder.Enabled = appointmentDialog.IsReminderSet;
          appointment.Reminder.DisplayInterval = 0 < appointmentDialog.DisplayInterval ? appointmentDialog.DisplayInterval : 0;
          appointment.Reminder.DisplayIntervalUnits = appointmentDialog.DisplayIntervalUnits;
          Session.CalendarManager.UpdateAppointment(new EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo()
          {
            Subject = appointment.Subject,
            Description = appointment.Description,
            StartDateTime = appointment.StartDateTime,
            EndDateTime = appointment.EndDateTime,
            AllDayEvent = appointment.AllDayEvent,
            ReminderEnabled = appointment.Reminder.Enabled,
            ReminderInterval = appointment.Reminder.DisplayInterval,
            ReminderUnits = (int) appointment.Reminder.DisplayIntervalUnits,
            OwnerKey = appointment.IsVariance ? appointment.RecurringAppointmentRoot.DataKey.ToString() : "",
            RecurrenceId = appointment.RecurrenceId == Guid.Empty ? "" : appointment.RecurrenceId.ToString(),
            Recurrence = appointment.Recurrence != null ? appointment.Recurrence.Save() : (byte[]) null,
            IsRemoved = appointment.IsRemoved,
            OriginalStartDateTime = appointment.OriginalStartDateTime,
            DataKey = appointment.DataKey.ToString(),
            AllProperties = appointment.Save(),
            UserID = Session.UserID
          }, (ContactInfo[]) validContacts.ToArray());
          flag = true;
          foreach (ActivityContactInfo activityContactInfo in validContacts)
            activityContactInfo.Activity.Status = ActivityStatus.Completed;
        }
      }
      return flag;
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if (Utils.FitLabelText(graphics, label, text))
          this.tipCampaignSteps.SetToolTip((Control) label, string.Empty);
        else
          this.tipCampaignSteps.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    private void processCompletionActivity()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      CampaignStep activeCampaignStep = this.campaignData.ActiveCampaignStep;
      if (this.setActivitySelection(ActivityStatus.Expected).Length == 0)
        return;
      string activityNote = string.Empty;
      using (CompleteTasksDialog completeTasksDialog = new CompleteTasksDialog())
      {
        if (DialogResult.OK != completeTasksDialog.ShowDialog())
          return;
        activityNote = completeTasksDialog.Note;
      }
      using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Campaign.Activity", "Perform campaign step activity", true, 1751, nameof (processCompletionActivity), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\Campaign\\CampaignStepsTab.cs"))
      {
        performanceMeter.AddVariable("Campaign", (object) this.campaignData.ActiveCampaign.CampaignName);
        performanceMeter.AddVariable("Step", (object) this.campaignData.ActiveCampaignStep.StepName);
        performanceMeter.AddVariable("ContactType", (object) this.campaignData.ActiveCampaign.ContactType);
        performanceMeter.AddVariable("Activity", (object) activeCampaignStep.ActivityType);
        performanceMeter.AddVariable("Count", (object) this.gvContacts.SelectedItems.Count);
        this.setPendingToComplete();
      }
      this.updateActivityStatus(activityNote);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void gvSteps_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvSteps.SelectedItems.Count != 0)
        this.campaignData.ActiveCampaignStep = (CampaignStep) this.gvSteps.SelectedItems[0].Tag;
      this.populateContactList();
      this.populateStepActivity();
    }

    private void cbShowDueItemsOnly_CheckedChanged(object sender, EventArgs e)
    {
      this.campaignData.ActiveCampaignStep = (CampaignStep) this.gvSteps.SelectedItems[0].Tag;
      this.populateContactList();
    }

    private void gvSteps_MouseMove(object sender, MouseEventArgs e)
    {
      GVItem itemAt = this.gvSteps.GetItemAt(e.X, e.Y);
      if (this.gvTooltipItem == itemAt)
        return;
      this.gvTooltipItem = itemAt;
      if (this.gvTooltipItem == null)
        this.tipCampaignSteps.SetToolTip((Control) this.gvSteps, string.Empty);
      else
        this.tipCampaignSteps.SetToolTip((Control) this.gvSteps, ((CampaignStep) itemAt.Tag).StepDesc);
    }

    private void lblStepName_Resize(object sender, EventArgs e)
    {
      if (this.campaignData.ActiveCampaignStep == null)
        return;
      this.fitLabelText((Label) sender, this.campaignData.ActiveCampaignStep.StepName);
    }

    private void lblStepDescription_Resize(object sender, EventArgs e)
    {
      if (this.campaignData.ActiveCampaignStep == null)
        return;
      this.fitLabelText((Label) sender, this.campaignData.ActiveCampaignStep.StepDesc);
    }

    private void txtLastNameFilter_LostFocus(object sender, EventArgs e)
    {
      if (!(this.lastNameFilterValue != ((Control) sender).Text))
        return;
      this.lastNameFilterValue = ((Control) sender).Text;
      this.populateContactList();
    }

    private void txtLastNameFilter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txtLastNameFilter_LostFocus(sender, (EventArgs) null);
    }

    private void txtFirstNameFilter_LostFocus(object sender, EventArgs e)
    {
      if (!(this.firstNameFilterValue != ((Control) sender).Text))
        return;
      this.firstNameFilterValue = ((Control) sender).Text;
      this.populateContactList();
    }

    private void txtEmailAddressFilter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txtEmailAddressFilter_LostFocus(sender, (EventArgs) null);
    }

    private void txtPhoneNumberFilter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txtPhoneNumberFilter_LostFocus(sender, (EventArgs) null);
    }

    private void dfScheduledDateFilter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.dfScheduledDateFilter_FilterChanged(sender, (EventArgs) null);
    }

    private void txtFirstNameFilter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.txtFirstNameFilter_LostFocus(sender, (EventArgs) null);
    }

    private void dfScheduledDateFilter_FilterChanged(object sender, EventArgs e)
    {
      if (!(this.scheduledDateFilterValue != ((DateFilterBox) sender).Value))
        return;
      this.scheduledDateFilterValue = ((DateFilterBox) sender).Value;
      this.populateContactList();
    }

    private void txtPhoneNumberFilter_LostFocus(object sender, EventArgs e)
    {
      if (!(this.phoneNumberFilterValue != ((Control) sender).Text))
        return;
      this.phoneNumberFilterValue = ((Control) sender).Text;
      this.populateContactList();
    }

    private void txtEmailAddressFilter_LostFocus(object sender, EventArgs e)
    {
      if (!(this.emailAddressFilterValue != ((Control) sender).Text))
        return;
      this.emailAddressFilterValue = ((Control) sender).Text;
      this.populateContactList();
    }

    private void gvContacts_DoubleClick(object sender, EventArgs e) => this.viewContact();

    private void gvContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setActivityState(0 < this.gvContacts.SelectedItems.Count && (this.campaignData.ActiveCampaign.Status == CampaignStatus.Running || CampaignStatus.Stopped == this.campaignData.ActiveCampaign.Status));
    }

    private void lnkPhoneNumber_Click(object sender, EventArgs e)
    {
      if (!(sender is ImageLink imageLink) || !(imageLink.Tag is CampaignActivity tag))
        return;
      using (ContactNotesDialog contactNotesDialog = new ContactNotesDialog(this.campaignData.ActiveCampaign.ContactType, tag.ContactId))
      {
        int num = (int) contactNotesDialog.ShowDialog();
      }
    }

    private void lnkEmailAddress_Click(object sender, EventArgs e)
    {
      if (!(sender is ImageLink imageLink) || !(imageLink.Tag is CampaignActivity tag))
        return;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      using (ContactNotesDialog contactNotesDialog = new ContactNotesDialog(activeCampaign.ContactType, tag.ContactId))
      {
        int newNote = contactNotesDialog.CreateNewNote();
        string key = activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "Contact.PersonalEmail" : "Contact.BizEmail";
        SystemUtil.ShellExecute("mailto:" + tag.ContactProperties[key]);
        ContactUtils.addEmailHistory(new int[1]
        {
          tag.ContactId
        }, activeCampaign.ContactType, newNote);
        int num = (int) contactNotesDialog.ShowDialog();
      }
    }

    private void ctxContacts_Opening(object sender, CancelEventArgs e)
    {
      this.mnuContactsView.Enabled = 1 == this.gvContacts.SelectedItems.Count;
      this.mnuContactsPrint.Enabled = 0 < this.gvContacts.SelectedItems.Count;
      this.mnuContactsRemove.Enabled = 0 < this.gvContacts.SelectedItems.Count;
      this.mnuContactsExport.Enabled = 0 < this.gvContacts.SelectedItems.Count;
    }

    private void mnuContactsPrint_Click(object sender, EventArgs e) => this.printContacts();

    private void mnuContactsRemove_Click(object sender, EventArgs e) => this.removeContacts();

    private void mnuContactsExport_Click(object sender, EventArgs e) => this.exportContacts();

    private void mnuContactsView_Click(object sender, EventArgs e) => this.viewContact();

    private void mnuContactsSelectAll_Click(object sender, EventArgs e) => this.selectAllContacts();

    private void setActivityState(bool isEnabled)
    {
      this.icnDeleteTask.Enabled = isEnabled;
      this.icnPrintTask.Enabled = isEnabled;
      this.icnExcelTask.Enabled = isEnabled;
      this.emailContactsSelected(isEnabled);
      this.letterContactsSelected(isEnabled);
      this.scheduleContactsSelected(isEnabled);
    }

    private void btnTaskAction_Click(object sender, EventArgs e)
    {
      switch (((Control) sender).Text)
      {
        case "Add to Calendar":
          this.ultraCalendarInfo.MinDate = frmCalendar.MinSmallDatetime;
          this.ultraCalendarInfo.MaxDate = frmCalendar.MaxSmallDatetime;
          this.ultraCalendarInfo.DisplayAppointmentDialog(this.ultraCalendarInfo.ActiveDay.Date, this.ultraCalendarInfo.ActiveDay.Date, false);
          break;
        case "Add to Tasks":
          this.displayTaskDialog();
          break;
        case "Complete":
          this.processCompletionActivity();
          break;
        case "Email Preview":
          this.processEmailLetterActivity(true);
          break;
        case "Print":
          this.processEmailLetterActivity(false);
          break;
        case "Print Preview":
          this.processEmailLetterActivity(true);
          break;
        case "Send Email":
          this.processEmailLetterActivity(false);
          break;
      }
      if (this.UpdateTaskCount == null)
        return;
      this.UpdateTaskCount();
    }

    private void icnDeleteTask_Click(object sender, EventArgs e) => this.removeContacts();

    private void icnExcelTask_Click(object sender, EventArgs e) => this.exportContacts();

    private void icnPrintTask_Click(object sender, EventArgs e) => this.printContacts();

    private void ultraCalendarInfo_BeforeDisplayAppointmentDialog(
      object sender,
      DisplayAppointmentDialogEventArgs e)
    {
      ((CancelEventArgs) e).Cancel = true;
      this.displayAppointmentDialog();
    }

    public event ActivitySelectedEventHandler ActivitySelectedEvent;

    protected virtual void OnActivitySelectedEvent(ActivitySelectedEventArgs e)
    {
      if (this.ActivitySelectedEvent == null)
        return;
      this.ActivitySelectedEvent((object) this, e);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CampaignStepsTab));
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      this.pnlStepDetail = new Panel();
      this.pnlContactsTable = new TableContainer();
      this.dfScheduledDateFilter = new DateFilterBox();
      this.pnlTaskActions = new Panel();
      this.btnTaskAction3 = new Button();
      this.btnTaskAction2 = new Button();
      this.btnTaskAction1 = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.icnPrintTask = new StandardIconButton();
      this.icnExcelTask = new StandardIconButton();
      this.icnDeleteTask = new StandardIconButton();
      this.txtEmailAddressFilter = new SizableTextBox();
      this.txtPhoneNumberFilter = new SizableTextBox();
      this.txtFirstNameFilter = new SizableTextBox();
      this.txtLastNameFilter = new SizableTextBox();
      this.gvContacts = new GridView();
      this.ctxContacts = new ContextMenuStrip(this.components);
      this.mnuContactsPrint = new ToolStripMenuItem();
      this.mnuContactsRemove = new ToolStripMenuItem();
      this.mnuContactsExport = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.mnuContactsView = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.mnuContactsSelectAll = new ToolStripMenuItem();
      this.lblTaskCount = new FormattedLabel();
      this.cbIncludeFutureTasks = new CheckBox();
      this.grpStepDetails = new GroupContainer();
      this.lblTaskInformation = new Label();
      this.lblStepName = new Label();
      this.pnlPhoneCallReminderTask = new Panel();
      this.dtpScheduleDate = new DatePicker();
      this.lblScheduleDateHdr = new Label();
      this.lblStepDescription = new Label();
      this.pnlFaxLetterTask = new Panel();
      this.lblLetterDocument = new Label();
      this.lblLetterDocumentHdr = new Label();
      this.lblTaskType = new Label();
      this.pnlEmailTask = new Panel();
      this.lblEmailDocument = new Label();
      this.lblEmailDocumentHdr = new Label();
      this.lblEmailSubjectHdr = new Label();
      this.txtEmailSubject = new TextBox();
      this.lblStepNameHdr = new Label();
      this.lblTaskTypeHdr = new Label();
      this.lblStepDescriptionHdr = new Label();
      this.imgList = new ImageList(this.components);
      this.tipCampaignSteps = new ToolTip(this.components);
      this.ultraCalendarInfo = new UltraCalendarInfo(this.components);
      this.ctrStepDetailSplitter = new CollapsibleSplitter();
      this.pnlStepList = new BorderPanel();
      this.gvSteps = new GridView();
      this.pnlSteps = new GradientPanel();
      this.lblTotalSteps = new Label();
      this.pnlStepDetail.SuspendLayout();
      this.pnlContactsTable.SuspendLayout();
      this.pnlTaskActions.SuspendLayout();
      ((ISupportInitialize) this.icnPrintTask).BeginInit();
      ((ISupportInitialize) this.icnExcelTask).BeginInit();
      ((ISupportInitialize) this.icnDeleteTask).BeginInit();
      this.ctxContacts.SuspendLayout();
      this.grpStepDetails.SuspendLayout();
      this.pnlPhoneCallReminderTask.SuspendLayout();
      this.pnlFaxLetterTask.SuspendLayout();
      this.pnlEmailTask.SuspendLayout();
      this.pnlStepList.SuspendLayout();
      this.pnlSteps.SuspendLayout();
      this.SuspendLayout();
      this.pnlStepDetail.Controls.Add((Control) this.pnlContactsTable);
      this.pnlStepDetail.Controls.Add((Control) this.grpStepDetails);
      this.pnlStepDetail.Dock = DockStyle.Fill;
      this.pnlStepDetail.Location = new Point(317, 26);
      this.pnlStepDetail.Name = "pnlStepDetail";
      this.pnlStepDetail.Size = new Size(683, 574);
      this.pnlStepDetail.TabIndex = 3;
      this.pnlContactsTable.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlContactsTable.Controls.Add((Control) this.dfScheduledDateFilter);
      this.pnlContactsTable.Controls.Add((Control) this.pnlTaskActions);
      this.pnlContactsTable.Controls.Add((Control) this.txtEmailAddressFilter);
      this.pnlContactsTable.Controls.Add((Control) this.txtPhoneNumberFilter);
      this.pnlContactsTable.Controls.Add((Control) this.txtFirstNameFilter);
      this.pnlContactsTable.Controls.Add((Control) this.txtLastNameFilter);
      this.pnlContactsTable.Controls.Add((Control) this.gvContacts);
      this.pnlContactsTable.Controls.Add((Control) this.lblTaskCount);
      this.pnlContactsTable.Controls.Add((Control) this.cbIncludeFutureTasks);
      this.pnlContactsTable.Dock = DockStyle.Fill;
      this.pnlContactsTable.Location = new Point(0, 237);
      this.pnlContactsTable.Name = "pnlContactsTable";
      this.pnlContactsTable.Size = new Size(683, 337);
      this.pnlContactsTable.TabIndex = 13;
      this.dfScheduledDateFilter.BorderColor = Color.Black;
      this.dfScheduledDateFilter.ComparisonOperator = ComparisonOperator.Equals;
      this.dfScheduledDateFilter.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dfScheduledDateFilter.Location = new Point(204, 46);
      this.dfScheduledDateFilter.Name = "dfScheduledDateFilter";
      this.dfScheduledDateFilter.Size = new Size(96, 22);
      this.dfScheduledDateFilter.TabIndex = 100;
      this.dfScheduledDateFilter.Value = new DateTime(0L);
      this.pnlTaskActions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlTaskActions.BackColor = Color.Transparent;
      this.pnlTaskActions.Controls.Add((Control) this.btnTaskAction3);
      this.pnlTaskActions.Controls.Add((Control) this.btnTaskAction2);
      this.pnlTaskActions.Controls.Add((Control) this.btnTaskAction1);
      this.pnlTaskActions.Controls.Add((Control) this.verticalSeparator1);
      this.pnlTaskActions.Controls.Add((Control) this.icnPrintTask);
      this.pnlTaskActions.Controls.Add((Control) this.icnExcelTask);
      this.pnlTaskActions.Controls.Add((Control) this.icnDeleteTask);
      this.pnlTaskActions.Location = new Point(283, 1);
      this.pnlTaskActions.Name = "pnlTaskActions";
      this.pnlTaskActions.Size = new Size(400, 22);
      this.pnlTaskActions.TabIndex = 99;
      this.btnTaskAction3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTaskAction3.BackColor = SystemColors.Control;
      this.btnTaskAction3.Location = new Point(313, 0);
      this.btnTaskAction3.Name = "btnTaskAction3";
      this.btnTaskAction3.Size = new Size(83, 22);
      this.btnTaskAction3.TabIndex = 86;
      this.btnTaskAction3.Text = "Task Action 3";
      this.btnTaskAction3.UseVisualStyleBackColor = true;
      this.btnTaskAction3.Click += new EventHandler(this.btnTaskAction_Click);
      this.btnTaskAction2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTaskAction2.BackColor = SystemColors.Control;
      this.btnTaskAction2.Location = new Point(229, 0);
      this.btnTaskAction2.Name = "btnTaskAction2";
      this.btnTaskAction2.Size = new Size(83, 22);
      this.btnTaskAction2.TabIndex = 87;
      this.btnTaskAction2.Text = "Task Action 2";
      this.btnTaskAction2.UseVisualStyleBackColor = true;
      this.btnTaskAction2.Click += new EventHandler(this.btnTaskAction_Click);
      this.btnTaskAction1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTaskAction1.BackColor = SystemColors.Control;
      this.btnTaskAction1.Location = new Point(145, 0);
      this.btnTaskAction1.Name = "btnTaskAction1";
      this.btnTaskAction1.Size = new Size(83, 22);
      this.btnTaskAction1.TabIndex = 88;
      this.btnTaskAction1.Text = "Task Action 1";
      this.btnTaskAction1.UseVisualStyleBackColor = true;
      this.btnTaskAction1.Click += new EventHandler(this.btnTaskAction_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(140, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 92;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.icnPrintTask.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnPrintTask.BackColor = Color.Transparent;
      this.icnPrintTask.Location = new Point(118, 3);
      this.icnPrintTask.MouseDownImage = (Image) null;
      this.icnPrintTask.Name = "icnPrintTask";
      this.icnPrintTask.Size = new Size(16, 16);
      this.icnPrintTask.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.icnPrintTask.TabIndex = 89;
      this.icnPrintTask.TabStop = false;
      this.tipCampaignSteps.SetToolTip((Control) this.icnPrintTask, "Print Details");
      this.icnPrintTask.Click += new EventHandler(this.icnPrintTask_Click);
      this.icnExcelTask.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnExcelTask.BackColor = Color.Transparent;
      this.icnExcelTask.Location = new Point(97, 3);
      this.icnExcelTask.MouseDownImage = (Image) null;
      this.icnExcelTask.Name = "icnExcelTask";
      this.icnExcelTask.Size = new Size(16, 16);
      this.icnExcelTask.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.icnExcelTask.TabIndex = 90;
      this.icnExcelTask.TabStop = false;
      this.tipCampaignSteps.SetToolTip((Control) this.icnExcelTask, "Export to Excel");
      this.icnExcelTask.Click += new EventHandler(this.icnExcelTask_Click);
      this.icnDeleteTask.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnDeleteTask.BackColor = Color.Transparent;
      this.icnDeleteTask.Location = new Point(76, 3);
      this.icnDeleteTask.MouseDownImage = (Image) null;
      this.icnDeleteTask.Name = "icnDeleteTask";
      this.icnDeleteTask.Size = new Size(16, 16);
      this.icnDeleteTask.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.icnDeleteTask.TabIndex = 91;
      this.icnDeleteTask.TabStop = false;
      this.tipCampaignSteps.SetToolTip((Control) this.icnDeleteTask, "Remove from step");
      this.icnDeleteTask.Click += new EventHandler(this.icnDeleteTask_Click);
      this.txtEmailAddressFilter.BackColor = SystemColors.Window;
      this.txtEmailAddressFilter.BorderColor = Color.Black;
      this.txtEmailAddressFilter.Location = new Point(424, 46);
      this.txtEmailAddressFilter.Name = "txtEmailAddressFilter";
      this.txtEmailAddressFilter.Size = new Size(280, 22);
      this.txtEmailAddressFilter.TabIndex = 98;
      this.txtPhoneNumberFilter.BackColor = SystemColors.Window;
      this.txtPhoneNumberFilter.BorderColor = Color.Black;
      this.txtPhoneNumberFilter.Location = new Point(304, 46);
      this.txtPhoneNumberFilter.Name = "txtPhoneNumberFilter";
      this.txtPhoneNumberFilter.Size = new Size(116, 22);
      this.txtPhoneNumberFilter.TabIndex = 97;
      this.txtFirstNameFilter.BackColor = SystemColors.Window;
      this.txtFirstNameFilter.BorderColor = Color.Black;
      this.txtFirstNameFilter.Location = new Point(104, 46);
      this.txtFirstNameFilter.Name = "txtFirstNameFilter";
      this.txtFirstNameFilter.Size = new Size(96, 22);
      this.txtFirstNameFilter.TabIndex = 96;
      this.txtLastNameFilter.BackColor = SystemColors.Window;
      this.txtLastNameFilter.BorderColor = Color.Black;
      this.txtLastNameFilter.Location = new Point(2, 46);
      this.txtLastNameFilter.Name = "txtLastNameFilter";
      this.txtLastNameFilter.Size = new Size(96, 22);
      this.txtLastNameFilter.TabIndex = 95;
      this.gvContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvcolLastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvcolFirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvcolScheduled";
      gvColumn3.SortMethod = GVSortMethod.Date;
      gvColumn3.Text = "Scheduled";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvcolPhone";
      gvColumn4.Text = "Home Phone";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvcolEmail";
      gvColumn5.SpringToFit = true;
      gvColumn5.Text = "Home Email";
      gvColumn5.Width = 261;
      this.gvContacts.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvContacts.ContextMenuStrip = this.ctxContacts;
      this.gvContacts.Dock = DockStyle.Fill;
      this.gvContacts.FilterVisible = true;
      this.gvContacts.Location = new Point(1, 25);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(681, 286);
      this.gvContacts.TabIndex = 93;
      this.gvContacts.SelectedIndexChanged += new EventHandler(this.gvContacts_SelectedIndexChanged);
      this.gvContacts.DoubleClick += new EventHandler(this.gvContacts_DoubleClick);
      this.ctxContacts.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.mnuContactsPrint,
        (ToolStripItem) this.mnuContactsRemove,
        (ToolStripItem) this.mnuContactsExport,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.mnuContactsView,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.mnuContactsSelectAll
      });
      this.ctxContacts.Name = "contextMenuStrip1";
      this.ctxContacts.Size = new Size(208, 126);
      this.ctxContacts.Opening += new CancelEventHandler(this.ctxContacts_Opening);
      this.mnuContactsPrint.Name = "mnuContactsPrint";
      this.mnuContactsPrint.Size = new Size(207, 22);
      this.mnuContactsPrint.Text = "Print Contact Information";
      this.mnuContactsPrint.Click += new EventHandler(this.mnuContactsPrint_Click);
      this.mnuContactsRemove.Name = "mnuContactsRemove";
      this.mnuContactsRemove.Size = new Size(207, 22);
      this.mnuContactsRemove.Text = "Remove from Step";
      this.mnuContactsRemove.Click += new EventHandler(this.mnuContactsRemove_Click);
      this.mnuContactsExport.Name = "mnuContactsExport";
      this.mnuContactsExport.Size = new Size(207, 22);
      this.mnuContactsExport.Text = "Export as CSV";
      this.mnuContactsExport.Click += new EventHandler(this.mnuContactsExport_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(204, 6);
      this.mnuContactsView.Name = "mnuContactsView";
      this.mnuContactsView.Size = new Size(207, 22);
      this.mnuContactsView.Text = "View";
      this.mnuContactsView.Click += new EventHandler(this.mnuContactsView_Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(204, 6);
      this.mnuContactsSelectAll.Name = "mnuContactsSelectAll";
      this.mnuContactsSelectAll.Size = new Size(207, 22);
      this.mnuContactsSelectAll.Text = "Select All";
      this.mnuContactsSelectAll.Click += new EventHandler(this.mnuContactsSelectAll_Click);
      this.lblTaskCount.BackColor = Color.Transparent;
      this.lblTaskCount.Location = new Point(10, 5);
      this.lblTaskCount.Name = "lblTaskCount";
      this.lblTaskCount.Size = new Size(70, 16);
      this.lblTaskCount.TabIndex = 0;
      this.lblTaskCount.Text = "<b>Tasks</b> <b><c value=\"239,0,0\">(999)</c></b>";
      this.cbIncludeFutureTasks.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cbIncludeFutureTasks.AutoSize = true;
      this.cbIncludeFutureTasks.BackColor = Color.Transparent;
      this.cbIncludeFutureTasks.Checked = true;
      this.cbIncludeFutureTasks.CheckState = CheckState.Checked;
      this.cbIncludeFutureTasks.Location = new Point(10, 317);
      this.cbIncludeFutureTasks.Name = "cbIncludeFutureTasks";
      this.cbIncludeFutureTasks.Size = new Size(203, 18);
      this.cbIncludeFutureTasks.TabIndex = 1;
      this.cbIncludeFutureTasks.Text = "Include tasks scheduled in the future";
      this.cbIncludeFutureTasks.UseVisualStyleBackColor = false;
      this.cbIncludeFutureTasks.CheckedChanged += new EventHandler(this.cbShowDueItemsOnly_CheckedChanged);
      this.grpStepDetails.BackColor = SystemColors.Window;
      this.grpStepDetails.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpStepDetails.Controls.Add((Control) this.lblTaskInformation);
      this.grpStepDetails.Controls.Add((Control) this.lblStepName);
      this.grpStepDetails.Controls.Add((Control) this.pnlPhoneCallReminderTask);
      this.grpStepDetails.Controls.Add((Control) this.lblStepDescription);
      this.grpStepDetails.Controls.Add((Control) this.pnlFaxLetterTask);
      this.grpStepDetails.Controls.Add((Control) this.lblTaskType);
      this.grpStepDetails.Controls.Add((Control) this.pnlEmailTask);
      this.grpStepDetails.Controls.Add((Control) this.lblStepNameHdr);
      this.grpStepDetails.Controls.Add((Control) this.lblTaskTypeHdr);
      this.grpStepDetails.Controls.Add((Control) this.lblStepDescriptionHdr);
      this.grpStepDetails.Dock = DockStyle.Top;
      this.grpStepDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpStepDetails.Location = new Point(0, 0);
      this.grpStepDetails.Name = "grpStepDetails";
      this.grpStepDetails.Size = new Size(683, 237);
      this.grpStepDetails.TabIndex = 12;
      this.grpStepDetails.Text = "Step Details";
      this.lblTaskInformation.Location = new Point(10, 94);
      this.lblTaskInformation.Name = "lblTaskInformation";
      this.lblTaskInformation.Size = new Size(671, 22);
      this.lblTaskInformation.TabIndex = 70;
      this.lblTaskInformation.Text = "Task information...";
      this.lblStepName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblStepName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStepName.Location = new Point(98, 30);
      this.lblStepName.Name = "lblStepName";
      this.lblStepName.Size = new Size(582, 16);
      this.lblStepName.TabIndex = 8;
      this.lblStepName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStepName.Resize += new EventHandler(this.lblStepName_Resize);
      this.pnlPhoneCallReminderTask.Controls.Add((Control) this.dtpScheduleDate);
      this.pnlPhoneCallReminderTask.Controls.Add((Control) this.lblScheduleDateHdr);
      this.pnlPhoneCallReminderTask.Location = new Point(10, 190);
      this.pnlPhoneCallReminderTask.Name = "pnlPhoneCallReminderTask";
      this.pnlPhoneCallReminderTask.Size = new Size(662, 30);
      this.pnlPhoneCallReminderTask.TabIndex = 4;
      this.dtpScheduleDate.BackColor = SystemColors.Window;
      this.dtpScheduleDate.Location = new Point(91, 2);
      this.dtpScheduleDate.Margin = new Padding(0);
      this.dtpScheduleDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtpScheduleDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtpScheduleDate.Name = "dtpScheduleDate";
      this.dtpScheduleDate.Size = new Size(192, 22);
      this.dtpScheduleDate.TabIndex = 55;
      this.dtpScheduleDate.ToolTip = "";
      this.dtpScheduleDate.Value = new DateTime(0L);
      this.lblScheduleDateHdr.Location = new Point(1, 4);
      this.lblScheduleDateHdr.Name = "lblScheduleDateHdr";
      this.lblScheduleDateHdr.Size = new Size(87, 18);
      this.lblScheduleDateHdr.TabIndex = 51;
      this.lblScheduleDateHdr.Text = "Task Start Date";
      this.lblScheduleDateHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStepDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblStepDescription.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStepDescription.Location = new Point(98, 52);
      this.lblStepDescription.Name = "lblStepDescription";
      this.lblStepDescription.Size = new Size(570, 16);
      this.lblStepDescription.TabIndex = 11;
      this.lblStepDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStepDescription.Resize += new EventHandler(this.lblStepDescription_Resize);
      this.pnlFaxLetterTask.Controls.Add((Control) this.lblLetterDocument);
      this.pnlFaxLetterTask.Controls.Add((Control) this.lblLetterDocumentHdr);
      this.pnlFaxLetterTask.Location = new Point(10, 164);
      this.pnlFaxLetterTask.Name = "pnlFaxLetterTask";
      this.pnlFaxLetterTask.Size = new Size(671, 20);
      this.pnlFaxLetterTask.TabIndex = 3;
      this.lblLetterDocument.BackColor = Color.Transparent;
      this.lblLetterDocument.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLetterDocument.Location = new Point(95, 2);
      this.lblLetterDocument.Name = "lblLetterDocument";
      this.lblLetterDocument.Size = new Size(564, 16);
      this.lblLetterDocument.TabIndex = 84;
      this.lblLetterDocumentHdr.Location = new Point(0, 2);
      this.lblLetterDocumentHdr.Name = "lblLetterDocumentHdr";
      this.lblLetterDocumentHdr.Size = new Size(60, 16);
      this.lblLetterDocumentHdr.TabIndex = 83;
      this.lblLetterDocumentHdr.Text = "Document:";
      this.lblLetterDocumentHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblTaskType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblTaskType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTaskType.Location = new Point(98, 74);
      this.lblTaskType.Name = "lblTaskType";
      this.lblTaskType.Size = new Size(144, 16);
      this.lblTaskType.TabIndex = 69;
      this.lblTaskType.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlEmailTask.Controls.Add((Control) this.lblEmailDocument);
      this.pnlEmailTask.Controls.Add((Control) this.lblEmailDocumentHdr);
      this.pnlEmailTask.Controls.Add((Control) this.lblEmailSubjectHdr);
      this.pnlEmailTask.Controls.Add((Control) this.txtEmailSubject);
      this.pnlEmailTask.Location = new Point(10, 116);
      this.pnlEmailTask.Name = "pnlEmailTask";
      this.pnlEmailTask.Size = new Size(671, 42);
      this.pnlEmailTask.TabIndex = 2;
      this.lblEmailDocument.BackColor = Color.Transparent;
      this.lblEmailDocument.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblEmailDocument.Location = new Point(93, 23);
      this.lblEmailDocument.Name = "lblEmailDocument";
      this.lblEmailDocument.Size = new Size(577, 17);
      this.lblEmailDocument.TabIndex = 76;
      this.lblEmailDocumentHdr.Location = new Point(0, 24);
      this.lblEmailDocumentHdr.Name = "lblEmailDocumentHdr";
      this.lblEmailDocumentHdr.Size = new Size(60, 16);
      this.lblEmailDocumentHdr.TabIndex = 75;
      this.lblEmailDocumentHdr.Text = "Document:";
      this.lblEmailDocumentHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblEmailSubjectHdr.Location = new Point(0, 2);
      this.lblEmailSubjectHdr.Name = "lblEmailSubjectHdr";
      this.lblEmailSubjectHdr.Size = new Size(60, 16);
      this.lblEmailSubjectHdr.TabIndex = 74;
      this.lblEmailSubjectHdr.Text = "Subject:";
      this.lblEmailSubjectHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.txtEmailSubject.Location = new Point(91, 0);
      this.txtEmailSubject.MaxLength = (int) byte.MaxValue;
      this.txtEmailSubject.Name = "txtEmailSubject";
      this.txtEmailSubject.Size = new Size(396, 20);
      this.txtEmailSubject.TabIndex = 72;
      this.lblStepNameHdr.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblStepNameHdr.Location = new Point(10, 30);
      this.lblStepNameHdr.Name = "lblStepNameHdr";
      this.lblStepNameHdr.Size = new Size(67, 16);
      this.lblStepNameHdr.TabIndex = 9;
      this.lblStepNameHdr.Text = "Step Name:";
      this.lblStepNameHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblTaskTypeHdr.Location = new Point(10, 74);
      this.lblTaskTypeHdr.Name = "lblTaskTypeHdr";
      this.lblTaskTypeHdr.Size = new Size(67, 16);
      this.lblTaskTypeHdr.TabIndex = 68;
      this.lblTaskTypeHdr.Text = "Task Type:";
      this.lblTaskTypeHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStepDescriptionHdr.Font = new Font("Arial", 8.25f);
      this.lblStepDescriptionHdr.Location = new Point(10, 52);
      this.lblStepDescriptionHdr.Name = "lblStepDescriptionHdr";
      this.lblStepDescriptionHdr.Size = new Size(67, 16);
      this.lblStepDescriptionHdr.TabIndex = 10;
      this.lblStepDescriptionHdr.Text = "Description:";
      this.lblStepDescriptionHdr.TextAlign = ContentAlignment.MiddleLeft;
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "Phone");
      this.imgList.Images.SetKeyName(1, "PhoneMouseOver");
      this.imgList.Images.SetKeyName(2, "Email");
      this.imgList.Images.SetKeyName(3, "EmailMouseOver");
      this.ultraCalendarInfo.DataBindingsForAppointments.BindingContextControl = (Control) this;
      this.ultraCalendarInfo.DataBindingsForOwners.BindingContextControl = (Control) this;
      this.ultraCalendarInfo.BeforeDisplayAppointmentDialog += new DisplayAppointmentDialogEventHandler(this.ultraCalendarInfo_BeforeDisplayAppointmentDialog);
      this.ctrStepDetailSplitter.AnimationDelay = 20;
      this.ctrStepDetailSplitter.AnimationStep = 20;
      this.ctrStepDetailSplitter.BorderStyle3D = Border3DStyle.RaisedOuter;
      this.ctrStepDetailSplitter.ControlToHide = (Control) this.pnlStepList;
      this.ctrStepDetailSplitter.ExpandParentForm = false;
      this.ctrStepDetailSplitter.Location = new Point(310, 26);
      this.ctrStepDetailSplitter.Name = "ctrStepDetailSplitter";
      this.ctrStepDetailSplitter.TabIndex = 2;
      this.ctrStepDetailSplitter.TabStop = false;
      this.ctrStepDetailSplitter.UseAnimations = false;
      this.ctrStepDetailSplitter.VisualStyle = VisualStyles.Encompass;
      this.pnlStepList.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlStepList.Controls.Add((Control) this.gvSteps);
      this.pnlStepList.Dock = DockStyle.Left;
      this.pnlStepList.Location = new Point(0, 26);
      this.pnlStepList.Name = "pnlStepList";
      this.pnlStepList.Padding = new Padding(6, 0, 0, 6);
      this.pnlStepList.Size = new Size(310, 574);
      this.pnlStepList.TabIndex = 1;
      this.gvSteps.AllowMultiselect = false;
      this.gvSteps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvSteps.BorderStyle = BorderStyle.None;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "hdrStepNumber";
      gvColumn6.Text = "#";
      gvColumn6.Width = 25;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "hdrStepName";
      gvColumn7.Text = "Step Name";
      gvColumn7.Width = 104;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "hdrTaskType";
      gvColumn8.Text = "Task Type";
      gvColumn8.Width = 63;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "hdrInterval";
      gvColumn9.Text = "Interval";
      gvColumn9.Width = 51;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "hdrTasksDue";
      gvColumn10.Text = "Tasks Due";
      gvColumn10.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn10.Width = 64;
      this.gvSteps.Columns.AddRange(new GVColumn[5]
      {
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.gvSteps.Location = new Point(1, 0);
      this.gvSteps.Name = "gvSteps";
      this.gvSteps.Size = new Size(307, 573);
      this.gvSteps.TabIndex = 1;
      this.gvSteps.SelectedIndexChanged += new EventHandler(this.gvSteps_SelectedIndexChanged);
      this.gvSteps.MouseMove += new MouseEventHandler(this.gvSteps_MouseMove);
      this.pnlSteps.Controls.Add((Control) this.lblTotalSteps);
      this.pnlSteps.Dock = DockStyle.Top;
      this.pnlSteps.GradientPaddingColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this.pnlSteps.Location = new Point(0, 0);
      this.pnlSteps.Name = "pnlSteps";
      this.pnlSteps.Size = new Size(1000, 26);
      this.pnlSteps.Style = GradientPanel.PanelStyle.TableHeader;
      this.pnlSteps.TabIndex = 4;
      this.lblTotalSteps.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblTotalSteps.BackColor = Color.Transparent;
      this.lblTotalSteps.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTotalSteps.Location = new Point(10, 5);
      this.lblTotalSteps.Name = "lblTotalSteps";
      this.lblTotalSteps.Size = new Size(72, 16);
      this.lblTotalSteps.TabIndex = 9;
      this.lblTotalSteps.Text = "Steps (999)";
      this.lblTotalSteps.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(1000, 600);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlStepDetail);
      this.Controls.Add((Control) this.ctrStepDetailSplitter);
      this.Controls.Add((Control) this.pnlStepList);
      this.Controls.Add((Control) this.pnlSteps);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignStepsTab);
      this.pnlStepDetail.ResumeLayout(false);
      this.pnlContactsTable.ResumeLayout(false);
      this.pnlContactsTable.PerformLayout();
      this.pnlTaskActions.ResumeLayout(false);
      ((ISupportInitialize) this.icnPrintTask).EndInit();
      ((ISupportInitialize) this.icnExcelTask).EndInit();
      ((ISupportInitialize) this.icnDeleteTask).EndInit();
      this.ctxContacts.ResumeLayout(false);
      this.grpStepDetails.ResumeLayout(false);
      this.pnlPhoneCallReminderTask.ResumeLayout(false);
      this.pnlFaxLetterTask.ResumeLayout(false);
      this.pnlEmailTask.ResumeLayout(false);
      this.pnlEmailTask.PerformLayout();
      this.pnlStepList.ResumeLayout(false);
      this.pnlSteps.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private enum YesNoUnknown
    {
      Yes,
      No,
      Unknown,
    }
  }
}
