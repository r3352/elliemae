// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard.CampaignStepDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.TaskList;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI.CampaignWizard
{
  public class CampaignStepDialog : Form
  {
    private CampaignStepsPanel.EditMode editMode;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private CampaignStep campaignStep;
    private string documentId = string.Empty;
    private IconButton btnBarColor;
    private ToolTip toolTip1;
    private ActivityTypeNameProvider activityNames = new ActivityTypeNameProvider();
    private IContainer components;
    private GroupBox grpStepDetail;
    private Panel pnlReminder;
    private Panel pnlBarColor;
    private ComboBox cboPriority;
    private TextBox txtComments;
    private Label lblBarColor;
    private Label lblPriority;
    private Label lblComments;
    private Panel pnlDocument;
    private Label lblDocumentId;
    private TextBox txtDocumentId;
    private Panel pnlSubject;
    private TextBox txtSubject;
    private Label lblSubject;
    private Panel pnlTaskCalendar;
    private ComboBox cboActivityType;
    private Label lblActivityType;
    private NumericUpDown nudStepInterval;
    private Label lblStepInterval;
    private TextBox txtDescription;
    private Label lblDescription;
    private TextBox txtStepName;
    private Label lblIntervalDescription;
    private Button btnCancel;
    private ColorDialog dlgColorPicker;
    private Button btnOK;
    private Label lblTaskDescription;
    private FormattedLabel lblStepName;
    private StandardIconButton icnBrowse;
    private Label lblTaskCalendar;

    public CampaignStepDialog(
      CampaignStepsPanel.EditMode editMode,
      EllieMae.EMLite.Campaign.Campaign campaign,
      CampaignStep campaignStep)
    {
      this.editMode = editMode;
      this.campaign = campaign;
      this.campaignStep = campaignStep;
      this.InitializeComponent();
      this.populateControls();
    }

    protected void populateControls()
    {
      this.Text = "Campaign Step and Task";
      this.cboActivityType.Items.Clear();
      this.cboActivityType.Items.AddRange((object[]) this.activityNames.GetNames());
      this.cboPriority.Items.Clear();
      this.cboPriority.Items.AddRange((object[]) TaskInfoUtils.TaskPriorityStrings);
      this.cboActivityType.SelectedItem = (object) this.activityNames.GetName((object) this.campaignStep.ActivityType);
      this.populateStepDetail(true);
      this.setActivityDetail();
    }

    private void populateStepDetail(bool useCampaignStep)
    {
      if (useCampaignStep)
      {
        this.txtStepName.Text = this.campaignStep.StepName;
        this.txtDescription.Text = this.campaignStep.StepDesc;
        this.nudStepInterval.Value = (Decimal) this.campaignStep.StepOffset;
        this.txtSubject.Text = this.campaignStep.Subject;
        this.documentId = this.campaignStep.DocumentId;
        this.txtDocumentId.Text = this.stripPath(this.documentId);
        this.txtComments.Text = this.campaignStep.Comments;
        this.cboPriority.SelectedItem = (object) Enum.GetName(typeof (TaskPriority), (object) this.campaignStep.TaskPriority);
        this.pnlBarColor.BackColor = this.campaignStep.BarColor;
      }
      else
      {
        this.txtSubject.Text = string.Empty;
        this.documentId = string.Empty;
        this.txtDocumentId.Text = string.Empty;
        this.txtComments.Text = string.Empty;
        this.cboPriority.SelectedItem = (object) Enum.GetName(typeof (TaskPriority), (object) TaskPriority.Normal);
        this.pnlBarColor.BackColor = this.pnlReminder.BackColor;
      }
    }

    private string stripPath(string fileName)
    {
      int startIndex = fileName.LastIndexOf("\\") + 1;
      return startIndex != 0 ? fileName.Substring(startIndex) : fileName;
    }

    private void cboActivityType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.campaignStep.ActivityType == (ActivityType) this.activityNames.GetValue(this.cboActivityType.SelectedItem.ToString()))
        return;
      this.campaignStep.ActivityType = (ActivityType) this.activityNames.GetValue(this.cboActivityType.SelectedItem.ToString());
      this.populateStepDetail(false);
      this.setActivityDetail();
    }

    private void setActivityDetail()
    {
      this.pnlDocument.Visible = false;
      this.pnlTaskCalendar.Visible = false;
      this.pnlSubject.Visible = false;
      this.pnlReminder.Visible = false;
      if (this.campaignStep.ActivityType == ActivityType.Email || ActivityType.Fax == this.campaignStep.ActivityType || ActivityType.Letter == this.campaignStep.ActivityType)
      {
        this.lblTaskDescription.Text = "Select a document to " + (this.campaignStep.ActivityType == ActivityType.Email ? "email" : (ActivityType.Fax == this.campaignStep.ActivityType ? "fax" : "print")) + ". Contact information will be merged into this document.";
        this.pnlDocument.BringToFront();
        this.pnlDocument.Visible = true;
      }
      if (ActivityType.PhoneCall == this.campaignStep.ActivityType || ActivityType.Reminder == this.campaignStep.ActivityType)
      {
        this.pnlSubject.BringToFront();
        this.pnlSubject.Visible = true;
      }
      if (this.campaignStep.ActivityType == ActivityType.Email || ActivityType.PhoneCall == this.campaignStep.ActivityType || ActivityType.Reminder == this.campaignStep.ActivityType)
      {
        this.pnlSubject.BringToFront();
        this.pnlSubject.Visible = true;
      }
      if (ActivityType.PhoneCall != this.campaignStep.ActivityType && ActivityType.Reminder != this.campaignStep.ActivityType)
        return;
      this.pnlReminder.BringToFront();
      this.pnlReminder.Visible = true;
    }

    private bool processUserEntry()
    {
      if (this.campaignStep.IsValid)
        return true;
      this.DisplayBrokenRules((BusinessBase) this.campaignStep);
      return false;
    }

    protected void DisplayBrokenRules(BusinessBase businessObject)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (BrokenRules.Rule brokenRules in (CollectionBase) businessObject.BrokenRulesCollection)
        stringBuilder.Append(brokenRules.Description + ".\n");
      int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void icnBrowse_Click(object sender, EventArgs e)
    {
      DocumentExplorerDialog documentExplorerDialog = new DocumentExplorerDialog(this.campaignStep.ActivityType, this.campaign.ContactType, this.campaignStep.DocumentId, (ArrayList) null);
      if (DialogResult.OK != documentExplorerDialog.ShowDialog())
        return;
      this.documentId = documentExplorerDialog.DocumentId;
      this.txtDocumentId.Text = this.stripPath(this.documentId);
    }

    private void btnBarColor_Click(object sender, EventArgs e)
    {
      this.dlgColorPicker.Color = this.pnlBarColor.BackColor;
      if (DialogResult.OK != this.dlgColorPicker.ShowDialog((IWin32Window) this))
        return;
      this.pnlBarColor.BackColor = this.dlgColorPicker.Color;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.campaignStep.StepName = this.txtStepName.Text;
      this.campaignStep.StepDesc = this.txtDescription.Text;
      this.campaignStep.ActivityType = (ActivityType) this.activityNames.GetValue(this.cboActivityType.SelectedItem.ToString());
      this.campaignStep.StepOffset = (int) this.nudStepInterval.Value;
      this.campaignStep.Subject = this.txtSubject.Text;
      this.campaignStep.DocumentId = this.documentId;
      this.campaignStep.Comments = this.txtComments.Text;
      this.campaignStep.TaskPriority = (TaskPriority) Enum.Parse(typeof (TaskPriority), this.cboPriority.SelectedItem.ToString());
      this.campaignStep.BarColor = this.pnlBarColor.BackColor;
      if (!this.campaignStep.IsValid)
      {
        this.DisplayBrokenRules((BusinessBase) this.campaignStep);
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
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
      this.grpStepDetail = new GroupBox();
      this.pnlDocument = new Panel();
      this.icnBrowse = new StandardIconButton();
      this.lblTaskDescription = new Label();
      this.txtDocumentId = new TextBox();
      this.lblDocumentId = new Label();
      this.pnlReminder = new Panel();
      this.pnlBarColor = new Panel();
      this.cboPriority = new ComboBox();
      this.txtComments = new TextBox();
      this.btnBarColor = new IconButton();
      this.lblBarColor = new Label();
      this.lblPriority = new Label();
      this.lblComments = new Label();
      this.pnlTaskCalendar = new Panel();
      this.lblTaskCalendar = new Label();
      this.pnlSubject = new Panel();
      this.txtSubject = new TextBox();
      this.lblSubject = new Label();
      this.cboActivityType = new ComboBox();
      this.lblActivityType = new Label();
      this.nudStepInterval = new NumericUpDown();
      this.lblStepInterval = new Label();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.txtStepName = new TextBox();
      this.lblIntervalDescription = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.dlgColorPicker = new ColorDialog();
      this.lblStepName = new FormattedLabel();
      this.toolTip1 = new ToolTip(this.components);
      this.grpStepDetail.SuspendLayout();
      this.pnlDocument.SuspendLayout();
      ((ISupportInitialize) this.icnBrowse).BeginInit();
      this.pnlReminder.SuspendLayout();
      ((ISupportInitialize) this.btnBarColor).BeginInit();
      this.pnlTaskCalendar.SuspendLayout();
      this.pnlSubject.SuspendLayout();
      this.nudStepInterval.BeginInit();
      this.SuspendLayout();
      this.grpStepDetail.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpStepDetail.Controls.Add((Control) this.pnlDocument);
      this.grpStepDetail.Controls.Add((Control) this.pnlReminder);
      this.grpStepDetail.Controls.Add((Control) this.pnlTaskCalendar);
      this.grpStepDetail.Controls.Add((Control) this.pnlSubject);
      this.grpStepDetail.Font = new Font("Arial", 8.25f);
      this.grpStepDetail.Location = new Point(10, 170);
      this.grpStepDetail.Name = "grpStepDetail";
      this.grpStepDetail.Padding = new Padding(10);
      this.grpStepDetail.Size = new Size(622, 238);
      this.grpStepDetail.TabIndex = 3;
      this.grpStepDetail.TabStop = false;
      this.pnlDocument.Controls.Add((Control) this.icnBrowse);
      this.pnlDocument.Controls.Add((Control) this.lblTaskDescription);
      this.pnlDocument.Controls.Add((Control) this.txtDocumentId);
      this.pnlDocument.Controls.Add((Control) this.lblDocumentId);
      this.pnlDocument.Dock = DockStyle.Top;
      this.pnlDocument.Location = new Point(10, 152);
      this.pnlDocument.Name = "pnlDocument";
      this.pnlDocument.Size = new Size(602, 54);
      this.pnlDocument.TabIndex = 0;
      this.pnlDocument.Visible = false;
      this.icnBrowse.BackColor = Color.Transparent;
      this.icnBrowse.Location = new Point(553, 26);
      this.icnBrowse.Name = "icnBrowse";
      this.icnBrowse.Size = new Size(16, 16);
      this.icnBrowse.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.icnBrowse.TabIndex = 7;
      this.icnBrowse.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.icnBrowse, "Select a Template");
      this.icnBrowse.Click += new EventHandler(this.icnBrowse_Click);
      this.lblTaskDescription.AutoSize = true;
      this.lblTaskDescription.Location = new Point(0, 0);
      this.lblTaskDescription.Name = "lblTaskDescription";
      this.lblTaskDescription.Size = new Size(400, 14);
      this.lblTaskDescription.TabIndex = 6;
      this.lblTaskDescription.Text = "Select a document to email. Contact information will be merged into this document.";
      this.lblTaskDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDocumentId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDocumentId.Enabled = false;
      this.txtDocumentId.Location = new Point(84, 24);
      this.txtDocumentId.Multiline = true;
      this.txtDocumentId.Name = "txtDocumentId";
      this.txtDocumentId.Size = new Size(463, 20);
      this.txtDocumentId.TabIndex = 0;
      this.lblDocumentId.AutoSize = true;
      this.lblDocumentId.Location = new Point(0, 27);
      this.lblDocumentId.Name = "lblDocumentId";
      this.lblDocumentId.Size = new Size(55, 14);
      this.lblDocumentId.TabIndex = 0;
      this.lblDocumentId.Text = "Document";
      this.lblDocumentId.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlReminder.Controls.Add((Control) this.pnlBarColor);
      this.pnlReminder.Controls.Add((Control) this.cboPriority);
      this.pnlReminder.Controls.Add((Control) this.txtComments);
      this.pnlReminder.Controls.Add((Control) this.btnBarColor);
      this.pnlReminder.Controls.Add((Control) this.lblBarColor);
      this.pnlReminder.Controls.Add((Control) this.lblPriority);
      this.pnlReminder.Controls.Add((Control) this.lblComments);
      this.pnlReminder.Dock = DockStyle.Top;
      this.pnlReminder.Location = new Point(10, 77);
      this.pnlReminder.Name = "pnlReminder";
      this.pnlReminder.Size = new Size(602, 75);
      this.pnlReminder.TabIndex = 3;
      this.pnlReminder.Visible = false;
      this.pnlBarColor.BorderStyle = BorderStyle.FixedSingle;
      this.pnlBarColor.Location = new Point(272, 42);
      this.pnlBarColor.Name = "pnlBarColor";
      this.pnlBarColor.Size = new Size(23, 23);
      this.pnlBarColor.TabIndex = 5;
      this.cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriority.Location = new Point(84, 42);
      this.cboPriority.Name = "cboPriority";
      this.cboPriority.Size = new Size(121, 22);
      this.cboPriority.TabIndex = 1;
      this.txtComments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtComments.Location = new Point(84, 0);
      this.txtComments.MaxLength = (int) byte.MaxValue;
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new Size(463, 32);
      this.txtComments.TabIndex = 0;
      this.btnBarColor.DisabledImage = (Image) null;
      this.btnBarColor.Image = (Image) Resources.color_picker;
      this.btnBarColor.Location = new Point(301, 46);
      this.btnBarColor.MouseOverImage = (Image) Resources.color_picker;
      this.btnBarColor.Name = "btnBarColor";
      this.btnBarColor.Size = new Size(16, 16);
      this.btnBarColor.TabIndex = 6;
      this.btnBarColor.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnBarColor, "Select Color");
      this.btnBarColor.Click += new EventHandler(this.btnBarColor_Click);
      this.lblBarColor.AutoSize = true;
      this.lblBarColor.Location = new Point(220, 46);
      this.lblBarColor.Name = "lblBarColor";
      this.lblBarColor.Size = new Size(52, 14);
      this.lblBarColor.TabIndex = 4;
      this.lblBarColor.Text = "Bar Color";
      this.lblBarColor.TextAlign = ContentAlignment.MiddleRight;
      this.lblPriority.AutoSize = true;
      this.lblPriority.Location = new Point(0, 46);
      this.lblPriority.Name = "lblPriority";
      this.lblPriority.Size = new Size(40, 14);
      this.lblPriority.TabIndex = 2;
      this.lblPriority.Text = "Priority";
      this.lblPriority.TextAlign = ContentAlignment.MiddleLeft;
      this.lblComments.AutoSize = true;
      this.lblComments.Location = new Point(0, 2);
      this.lblComments.Name = "lblComments";
      this.lblComments.Size = new Size(57, 14);
      this.lblComments.TabIndex = 0;
      this.lblComments.Text = "Comments";
      this.lblComments.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlTaskCalendar.Controls.Add((Control) this.lblTaskCalendar);
      this.pnlTaskCalendar.Dock = DockStyle.Top;
      this.pnlTaskCalendar.Location = new Point(10, 53);
      this.pnlTaskCalendar.Name = "pnlTaskCalendar";
      this.pnlTaskCalendar.Size = new Size(602, 24);
      this.pnlTaskCalendar.TabIndex = 0;
      this.lblTaskCalendar.AutoSize = true;
      this.lblTaskCalendar.Location = new Point(0, 0);
      this.lblTaskCalendar.Name = "lblTaskCalendar";
      this.lblTaskCalendar.Size = new Size(401, 14);
      this.lblTaskCalendar.TabIndex = 0;
      this.lblTaskCalendar.Text = "Add this task to your Encompass Task List or Calendar with the following options.";
      this.pnlSubject.Controls.Add((Control) this.txtSubject);
      this.pnlSubject.Controls.Add((Control) this.lblSubject);
      this.pnlSubject.Dock = DockStyle.Top;
      this.pnlSubject.Location = new Point(10, 23);
      this.pnlSubject.Name = "pnlSubject";
      this.pnlSubject.Size = new Size(602, 30);
      this.pnlSubject.TabIndex = 1;
      this.pnlSubject.Visible = false;
      this.txtSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSubject.Location = new Point(84, 0);
      this.txtSubject.MaxLength = (int) byte.MaxValue;
      this.txtSubject.Name = "txtSubject";
      this.txtSubject.Size = new Size(463, 20);
      this.txtSubject.TabIndex = 0;
      this.lblSubject.AutoSize = true;
      this.lblSubject.Location = new Point(0, 3);
      this.lblSubject.Name = "lblSubject";
      this.lblSubject.Size = new Size(43, 14);
      this.lblSubject.TabIndex = 0;
      this.lblSubject.Text = "Subject";
      this.lblSubject.TextAlign = ContentAlignment.MiddleLeft;
      this.cboActivityType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActivityType.ItemHeight = 14;
      this.cboActivityType.Location = new Point(104, 142);
      this.cboActivityType.Name = "cboActivityType";
      this.cboActivityType.Size = new Size(104, 22);
      this.cboActivityType.Sorted = true;
      this.cboActivityType.TabIndex = 3;
      this.cboActivityType.SelectedIndexChanged += new EventHandler(this.cboActivityType_SelectedIndexChanged);
      this.lblActivityType.AutoSize = true;
      this.lblActivityType.Location = new Point(10, 146);
      this.lblActivityType.Name = "lblActivityType";
      this.lblActivityType.Size = new Size(57, 14);
      this.lblActivityType.TabIndex = 4;
      this.lblActivityType.Text = "Task Type";
      this.lblActivityType.TextAlign = ContentAlignment.MiddleLeft;
      this.nudStepInterval.Location = new Point(104, 112);
      this.nudStepInterval.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this.nudStepInterval.Name = "nudStepInterval";
      this.nudStepInterval.Size = new Size(48, 20);
      this.nudStepInterval.TabIndex = 2;
      this.lblStepInterval.AutoSize = true;
      this.lblStepInterval.Location = new Point(10, 115);
      this.lblStepInterval.Name = "lblStepInterval";
      this.lblStepInterval.Size = new Size(67, 14);
      this.lblStepInterval.TabIndex = 12;
      this.lblStepInterval.Text = "Step Interval";
      this.lblStepInterval.TextAlign = ContentAlignment.MiddleLeft;
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(104, 40);
      this.txtDescription.MaxLength = 250;
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(528, 62);
      this.txtDescription.TabIndex = 1;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(10, 42);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 10;
      this.lblDescription.Text = "Description";
      this.lblDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.txtStepName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStepName.Location = new Point(104, 10);
      this.txtStepName.MaxLength = 64;
      this.txtStepName.Name = "txtStepName";
      this.txtStepName.Size = new Size(528, 20);
      this.txtStepName.TabIndex = 0;
      this.lblIntervalDescription.AutoSize = true;
      this.lblIntervalDescription.Location = new Point(160, 115);
      this.lblIntervalDescription.Name = "lblIntervalDescription";
      this.lblIntervalDescription.Size = new Size(293, 14);
      this.lblIntervalDescription.TabIndex = 14;
      this.lblIntervalDescription.Text = "(The step will start this many days after the previous step.)";
      this.lblIntervalDescription.TextAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(557, 424);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(472, 424);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblStepName.Location = new Point(10, 12);
      this.lblStepName.Name = "lblStepName";
      this.lblStepName.Size = new Size(70, 16);
      this.lblStepName.TabIndex = 15;
      this.lblStepName.Text = "Step Name<c value=\"Red\">*</c>";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(663, 459);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblStepName);
      this.Controls.Add((Control) this.cboActivityType);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblActivityType);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblIntervalDescription);
      this.Controls.Add((Control) this.nudStepInterval);
      this.Controls.Add((Control) this.lblStepInterval);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtStepName);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.grpStepDetail);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignStepDialog);
      this.Text = "Campaign Step and Task";
      this.grpStepDetail.ResumeLayout(false);
      this.pnlDocument.ResumeLayout(false);
      this.pnlDocument.PerformLayout();
      ((ISupportInitialize) this.icnBrowse).EndInit();
      this.pnlReminder.ResumeLayout(false);
      this.pnlReminder.PerformLayout();
      ((ISupportInitialize) this.btnBarColor).EndInit();
      this.pnlTaskCalendar.ResumeLayout(false);
      this.pnlTaskCalendar.PerformLayout();
      this.pnlSubject.ResumeLayout(false);
      this.pnlSubject.PerformLayout();
      this.nudStepInterval.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
