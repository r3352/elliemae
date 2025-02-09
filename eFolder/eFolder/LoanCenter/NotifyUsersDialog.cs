// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.NotifyUsersDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.eFolder.EmailNotificationController;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class NotifyUsersDialog : Form
  {
    private const string className = "NotifyUsersDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private List<EmailSetting> existingEmailSettings = new List<EmailSetting>();
    private List<LoanDisplayInfo> loanInfo;
    private bool isModified;
    private IContainer components;
    private GroupContainer gcNewEmails;
    private FlowLayoutPanel pnlEmailBar;
    private StandardIconButton btnDelete;
    private GridView gvEmails;
    private GroupContainer gcEmails;
    private Button btnCancel;
    private Button btnSave;
    private EMHelpLink helpLink;
    private StandardIconButton btnEmailSearch;
    private TextBox txtEmail;
    private DateTimePicker dtNotifyDate;
    private Button btnAdd;
    private Label lblNotifyDate;
    private Label lblEmail;
    private PictureBox imageLoading;
    private GroupBox pnlLoading;
    private Label lblLoading;
    private BackgroundWorker worker;

    public bool IsModified => this.isModified;

    public NotifyUsersDialog(List<LoanDisplayInfo> loanInfo, bool useWorker = true)
    {
      this.InitializeComponent();
      this.loanInfo = loanInfo;
      this.initEmailList();
      if (useWorker)
      {
        Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Starting Thread");
        this.worker.RunWorkerAsync();
      }
      else
      {
        Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Call getEmails");
        this.getEmails();
      }
    }

    private void initEmailList()
    {
      Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Initializing the email list");
      if (this.loanInfo.Count > 1)
        this.initEmailTree();
      else
        this.initEmailGrid();
      this.gvEmails.EditorClosing += new GVSubItemEditingEventHandler(this.gvEmails_EditorClosing);
      this.gvEmails.AllowMultiselect = true;
    }

    private void initEmailGrid()
    {
      this.gvEmails.Columns.Add("Email Address", 225);
      this.gvEmails.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.gvEmails.Columns.Add("Notify Through", 100);
      this.gvEmails.Columns[1].ActivatedEditorType = GVActivatedEditorType.DatePicker;
    }

    private void initEmailTree()
    {
      this.gvEmails.Columns.Add("Email Address", 225);
      this.gvEmails.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.gvEmails.Columns.Add("Notify Through", 100);
      this.gvEmails.Columns[1].ActivatedEditorType = GVActivatedEditorType.DatePicker;
      this.gvEmails.Columns.Add("Loan#", 125);
      this.gvEmails.Columns.Add("Borrower Name", 215);
      this.gvEmails.Columns.Add("Loan Amt", 80, ContentAlignment.MiddleRight);
      this.gvEmails.Columns[4].SpringToFit = true;
      this.gvEmails.EditorOpening += new GVSubItemEditingEventHandler(this.gvEmails_EditorOpening);
      this.gvEmails.ColumnClick += new GVColumnClickEventHandler(this.gvEmails_Sort);
    }

    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Loading the email tree");
      EmailNotificationClient notificationClient = new EmailNotificationClient();
      List<Guid> guidList = new List<Guid>();
      foreach (LoanDisplayInfo loanDisplayInfo in this.loanInfo)
        guidList.Add(loanDisplayInfo.LoanGuid);
      this.existingEmailSettings = ((IEnumerable<EmailSetting>) notificationClient.EmailGet(guidList.ToArray())).ToList<EmailSetting>();
    }

    private void getEmails()
    {
      Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Loading the email tree");
      try
      {
        EmailNotificationClient notificationClient = new EmailNotificationClient();
        List<Guid> guidList = new List<Guid>();
        foreach (LoanDisplayInfo loanDisplayInfo in this.loanInfo)
          guidList.Add(loanDisplayInfo.LoanGuid);
        this.existingEmailSettings = ((IEnumerable<EmailSetting>) notificationClient.EmailGet(guidList.ToArray())).ToList<EmailSetting>();
      }
      catch (Exception ex)
      {
        Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Error, nameof (NotifyUsersDialog), ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to process the request:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.pnlLoading.Visible = false;
      }
      if (this.loanInfo.Count > 1)
        this.loadEmailTree();
      else
        this.loadEmailGrid();
      this.gvEmails.Sort(0, SortOrder.Ascending);
    }

    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.pnlLoading.Visible = false;
      if (e.Error != null)
      {
        Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Error, nameof (NotifyUsersDialog), e.Error.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to process the request:\n\n" + e.Error.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (this.loanInfo.Count > 1)
        this.loadEmailTree();
      else
        this.loadEmailGrid();
      this.gvEmails.Sort(0, SortOrder.Ascending);
    }

    private void loadEmailTree()
    {
      foreach (string str in this.existingEmailSettings.Select<EmailSetting, string>((Func<EmailSetting, string>) (x => x.EmailAddress)).Distinct<string>())
      {
        string email = str;
        this.addGroupGridItem(this.existingEmailSettings.FindAll((Predicate<EmailSetting>) (x => x.EmailAddress == email)).ToArray(), false);
      }
    }

    private void loadEmailGrid()
    {
      foreach (EmailSetting existingEmailSetting in this.existingEmailSettings)
        this.addGridItem(existingEmailSetting, false);
    }

    private void gvEmails_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.EditorControl is DatePicker && e.SubItem.Item.ParentItem == null)
        e.Cancel = true;
      if (!(e.EditorControl is TextBox) || e.SubItem.Item.ParentItem == null)
        return;
      e.Cancel = true;
    }

    private void gvEmails_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (e.EditorControl is DatePicker)
      {
        DatePicker editorControl = e.EditorControl as DatePicker;
        DateTime? validNotifyDate = this.getValidNotifyDate(editorControl.Value, false);
        if (!validNotifyDate.HasValue)
        {
          this.gvEmails.CancelEditing();
          return;
        }
        DateTime? nullable = validNotifyDate;
        DateTime dateTime = editorControl.Value;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != dateTime ? 1 : 0) : 0) : 1) != 0)
          editorControl.Value = validNotifyDate.Value;
        this.setNotifyTillInTag(new DateTime?(validNotifyDate.Value), e.SubItem.Item);
      }
      if (e.EditorControl is TextBox)
      {
        if (this.isValidEmail(e.EditorControl.Text))
          this.setEmailInTag(e.EditorControl.Text.Trim(), e.SubItem.Item);
        else
          this.gvEmails.CancelEditing();
      }
      this.btnSave.Enabled = true;
    }

    private void setEmailInTag(string email, GVItem item)
    {
      if (item.Tag != null)
      {
        (item.Tag as EmailSetting).EmailAddress = email;
      }
      else
      {
        foreach (GVItem groupItem in (IEnumerable<GVItem>) item.GroupItems)
          (groupItem.Tag as EmailSetting).EmailAddress = email;
      }
    }

    private void setNotifyTillInTag(DateTime? date, GVItem item)
    {
      (item.Tag as EmailSetting).ValidTill = date.Value;
    }

    private void gvEmails_Sort(object source, GVColumnClickEventArgs e)
    {
      if (e.Column == 0)
        return;
      this.gvEmails.Sort(0, this.gvEmails.Columns[0].SortOrder);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvEmails.SelectedItems)
      {
        if (selectedItem.ParentItem == null)
          this.gvEmails.Items.Remove(selectedItem);
        else if (this.gvEmails.Items.Contains(selectedItem.ParentItem))
          selectedItem.ParentItem.GroupItems.Remove(selectedItem);
      }
      this.btnSave.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Save event of NotifyUsersDialog");
      EmailNotificationClient notificationClient = new EmailNotificationClient();
      List<EmailSetting> source1 = new List<EmailSetting>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvEmails.Items)
      {
        if (gvItem.Tag != null)
        {
          source1.Add(gvItem.Tag as EmailSetting);
        }
        else
        {
          foreach (GVItem groupItem in (IEnumerable<GVItem>) gvItem.GroupItems)
            source1.Add(groupItem.Tag as EmailSetting);
        }
      }
      if (source1.Count<EmailSetting>() == 0)
      {
        foreach (LoanDisplayInfo loanDisplayInfo in this.loanInfo)
          source1.Add(new EmailSetting()
          {
            LoanId = loanDisplayInfo.LoanGuid
          });
      }
      Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Calling EmailSave");
      FailedEmailSetting[] source2 = notificationClient.EmailSave(source1.ToArray());
      if (source2 != null && ((IEnumerable<FailedEmailSetting>) source2).Count<FailedEmailSetting>() > 0)
      {
        StringBuilder stringBuilder = new StringBuilder("These email addresses could not be saved:");
        foreach (FailedEmailSetting failedEmailSetting in source2)
          stringBuilder.Append("\n\r" + failedEmailSetting.EmailAddress + " - " + failedEmailSetting.Reason);
        int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Changes saved successfully!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.btnSave.Enabled = false;
        this.isModified = true;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnEmailSearch_Click(object sender, EventArgs e)
    {
      using (UserSelectionDialog userSelectionDialog = new UserSelectionDialog())
      {
        if (userSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.txtEmail.Text = userSelectionDialog.SelectedUser.Email;
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      Tracing.Log(NotifyUsersDialog.sw, TraceLevel.Verbose, nameof (NotifyUsersDialog), "Add event of NotifyUsersDialog");
      string email = this.txtEmail.Text.Trim();
      if (!this.isValidEmail(email))
        return;
      DateTime? validNotifyDate = this.getValidNotifyDate(this.dtNotifyDate.Value, true);
      if (!validNotifyDate.HasValue)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvEmails.Items)
      {
        if (gvItem.Value.ToString() == email)
        {
          if (Utils.Dialog((IWin32Window) this, "The email '" + email + "' already exists. Do you want to overwrite? ", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
            return;
          this.gvEmails.Items.Remove(gvItem);
          break;
        }
      }
      this.gvEmails.SelectedItems.Clear();
      if (this.loanInfo.Count > 1)
      {
        List<EmailSetting> emailSettingList = new List<EmailSetting>();
        foreach (LoanDisplayInfo loanDisplayInfo in this.loanInfo)
          emailSettingList.Add(new EmailSetting()
          {
            EmailAddress = email,
            ValidTill = validNotifyDate.Value,
            LoanId = loanDisplayInfo.LoanGuid
          });
        this.addGroupGridItem(emailSettingList.ToArray(), true);
      }
      else
        this.addGridItem(new EmailSetting()
        {
          EmailAddress = email,
          ValidTill = validNotifyDate.Value,
          LoanId = this.loanInfo[0].LoanGuid
        }, true);
      this.btnSave.Enabled = true;
      this.gvEmails.ReSort();
    }

    private void addGroupGridItem(EmailSetting[] emailSettings, bool selected)
    {
      GVItem gvItem1 = new GVItem(emailSettings[0].EmailAddress);
      gvItem1.State = GVItemState.Normal;
      gvItem1.Selected = selected;
      gvItem1.GroupItems.DisableSort = true;
      this.gvEmails.Items.Add(gvItem1);
      foreach (EmailSetting emailSetting1 in emailSettings)
      {
        EmailSetting emailSetting = emailSetting1;
        GVItem gvItem2 = new GVItem();
        gvItem2.Tag = (object) emailSetting;
        gvItem2.Selected = selected;
        gvItem2.SubItems.Add((object) "");
        gvItem2.SubItems.Add((object) emailSetting.ValidTill.ToString("MM/dd/yyyy"));
        LoanDisplayInfo loanDisplayInfo = this.loanInfo.Find((Predicate<LoanDisplayInfo>) (x => x.LoanGuid == emailSetting.LoanId));
        if (loanDisplayInfo != null)
        {
          gvItem2.SubItems.Add((object) loanDisplayInfo.LoanNumber);
          gvItem2.SubItems.Add((object) loanDisplayInfo.BorrowerName);
          gvItem2.SubItems.Add((object) loanDisplayInfo.LoanAmount.ToString("N2"));
        }
        gvItem1.GroupItems.Add(gvItem2);
      }
    }

    private void addGridItem(EmailSetting emailSetting, bool selected)
    {
      this.gvEmails.Items.Add(new GVItem()
      {
        Selected = selected,
        Tag = (object) emailSetting,
        SubItems = {
          (object) emailSetting.EmailAddress,
          (object) emailSetting.ValidTill.ToString("MM/dd/yyyy")
        }
      });
    }

    private DateTime? getValidNotifyDate(DateTime date, bool shouldBeInFuture)
    {
      if (date.Date > DateTime.Now.AddDays(120.0))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid date - " + date.ToString("MM/dd/yyyy") + ". Notify Date cannot be set beyond 120 days.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return new DateTime?();
      }
      if (shouldBeInFuture && date.Date < DateTime.Now.Date)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid date - " + date.ToString("MM/dd/yyyy") + ". Notify Date cannot be set in the past.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return new DateTime?();
      }
      if (!date.Equals(DateTime.MinValue))
        return new DateTime?(date);
      int num1 = (int) Utils.Dialog((IWin32Window) this, " Default date is 120 days from the current date.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return new DateTime?(DateTime.Now.AddDays(120.0));
    }

    private bool isValidEmail(string email)
    {
      if (Utils.ValidateEmail(email))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + email + "' is not a valid email format.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NotifyUsersDialog));
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.helpLink = new EMHelpLink();
      this.gcNewEmails = new GroupContainer();
      this.lblNotifyDate = new Label();
      this.lblEmail = new Label();
      this.dtNotifyDate = new DateTimePicker();
      this.btnAdd = new Button();
      this.btnEmailSearch = new StandardIconButton();
      this.txtEmail = new TextBox();
      this.gcEmails = new GroupContainer();
      this.pnlLoading = new GroupBox();
      this.lblLoading = new Label();
      this.imageLoading = new PictureBox();
      this.pnlEmailBar = new FlowLayoutPanel();
      this.btnDelete = new StandardIconButton();
      this.gvEmails = new GridView();
      this.worker = new BackgroundWorker();
      this.gcNewEmails.SuspendLayout();
      ((ISupportInitialize) this.btnEmailSearch).BeginInit();
      this.gcEmails.SuspendLayout();
      this.pnlLoading.SuspendLayout();
      ((ISupportInitialize) this.imageLoading).BeginInit();
      this.pnlEmailBar.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.OK;
      this.btnCancel.Location = new Point(679, 361);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 18;
      this.btnCancel.TabStop = false;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(586, 361);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 19;
      this.btnSave.TabStop = false;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Notify Users";
      this.helpLink.Location = new Point(3, 365);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 20;
      this.helpLink.TabStop = false;
      this.gcNewEmails.Controls.Add((Control) this.lblNotifyDate);
      this.gcNewEmails.Controls.Add((Control) this.lblEmail);
      this.gcNewEmails.Controls.Add((Control) this.dtNotifyDate);
      this.gcNewEmails.Controls.Add((Control) this.btnAdd);
      this.gcNewEmails.Controls.Add((Control) this.btnEmailSearch);
      this.gcNewEmails.Controls.Add((Control) this.txtEmail);
      this.gcNewEmails.HeaderForeColor = SystemColors.ControlText;
      this.gcNewEmails.Location = new Point(2, 1);
      this.gcNewEmails.Name = "gcNewEmails";
      this.gcNewEmails.Size = new Size(760, 89);
      this.gcNewEmails.TabIndex = 16;
      this.gcNewEmails.Text = "Add users to be notified";
      this.lblNotifyDate.AutoSize = true;
      this.lblNotifyDate.Location = new Point(257, 34);
      this.lblNotifyDate.Name = "lblNotifyDate";
      this.lblNotifyDate.Size = new Size(102, 13);
      this.lblNotifyDate.TabIndex = 442;
      this.lblNotifyDate.Text = "Notify User Through";
      this.lblEmail.AutoSize = true;
      this.lblEmail.Location = new Point(6, 35);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(73, 13);
      this.lblEmail.TabIndex = 441;
      this.lblEmail.Text = "Email Address";
      this.dtNotifyDate.Format = DateTimePickerFormat.Short;
      this.dtNotifyDate.Location = new Point(257, 50);
      this.dtNotifyDate.Name = "dtNotifyDate";
      this.dtNotifyDate.Size = new Size(98, 20);
      this.dtNotifyDate.TabIndex = 440;
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.Location = new Point(427, 47);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(53, 22);
      this.btnAdd.TabIndex = 21;
      this.btnAdd.TabStop = false;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnEmailSearch.BackColor = Color.Transparent;
      this.btnEmailSearch.Location = new Point(197, 53);
      this.btnEmailSearch.MouseDownImage = (Image) null;
      this.btnEmailSearch.Name = "btnEmailSearch";
      this.btnEmailSearch.Size = new Size(16, 16);
      this.btnEmailSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnEmailSearch.TabIndex = 439;
      this.btnEmailSearch.TabStop = false;
      this.btnEmailSearch.Tag = (object) "1";
      this.btnEmailSearch.Click += new EventHandler(this.btnEmailSearch_Click);
      this.txtEmail.BackColor = SystemColors.Window;
      this.txtEmail.CharacterCasing = CharacterCasing.Lower;
      this.txtEmail.Location = new Point(6, 51);
      this.txtEmail.MaxLength = 64;
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(182, 20);
      this.txtEmail.TabIndex = 438;
      this.txtEmail.Tag = (object) "1";
      this.gcEmails.Controls.Add((Control) this.pnlLoading);
      this.gcEmails.Controls.Add((Control) this.pnlEmailBar);
      this.gcEmails.Controls.Add((Control) this.gvEmails);
      this.gcEmails.HeaderForeColor = SystemColors.ControlText;
      this.gcEmails.Location = new Point(2, 84);
      this.gcEmails.Name = "gcEmails";
      this.gcEmails.Size = new Size(760, 271);
      this.gcEmails.TabIndex = 15;
      this.gcEmails.Text = "Users assiged to be notified";
      this.pnlLoading.Controls.Add((Control) this.lblLoading);
      this.pnlLoading.Controls.Add((Control) this.imageLoading);
      this.pnlLoading.Location = new Point(248, 69);
      this.pnlLoading.Name = "pnlLoading";
      this.pnlLoading.Size = new Size(185, 46);
      this.pnlLoading.TabIndex = 14;
      this.pnlLoading.TabStop = false;
      this.lblLoading.AutoSize = true;
      this.lblLoading.Location = new Point(44, 19);
      this.lblLoading.Name = "lblLoading";
      this.lblLoading.Size = new Size(54, 13);
      this.lblLoading.TabIndex = 14;
      this.lblLoading.Text = "Loading...";
      this.imageLoading.Image = (Image) componentResourceManager.GetObject("imageLoading.Image");
      this.imageLoading.Location = new Point(110, 16);
      this.imageLoading.Name = "imageLoading";
      this.imageLoading.Size = new Size(24, 18);
      this.imageLoading.TabIndex = 13;
      this.imageLoading.TabStop = false;
      this.pnlEmailBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlEmailBar.BackColor = Color.Transparent;
      this.pnlEmailBar.Controls.Add((Control) this.btnDelete);
      this.pnlEmailBar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlEmailBar.Location = new Point(609, 2);
      this.pnlEmailBar.Name = "pnlEmailBar";
      this.pnlEmailBar.Size = new Size(147, 22);
      this.pnlEmailBar.TabIndex = 11;
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(129, 3);
      this.btnDelete.Margin = new Padding(3, 3, 2, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 14;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.gvEmails.AllowDrop = true;
      this.gvEmails.AllowMultiselect = false;
      this.gvEmails.BorderStyle = BorderStyle.None;
      this.gvEmails.ClearSelectionsOnEmptyRowClick = false;
      this.gvEmails.Dock = DockStyle.Fill;
      this.gvEmails.ItemGrouping = true;
      this.gvEmails.Location = new Point(1, 26);
      this.gvEmails.Name = "gvEmails";
      this.gvEmails.Size = new Size(758, 244);
      this.gvEmails.TabIndex = 12;
      this.gvEmails.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvEmails.UseCompatibleEditingBehavior = true;
      this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
      this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(762, 390);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.gcNewEmails);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gcEmails);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.icon_allsizes_bug;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NotifyUsersDialog);
      this.Text = "Notify Users";
      this.gcNewEmails.ResumeLayout(false);
      this.gcNewEmails.PerformLayout();
      ((ISupportInitialize) this.btnEmailSearch).EndInit();
      this.gcEmails.ResumeLayout(false);
      this.pnlLoading.ResumeLayout(false);
      this.pnlLoading.PerformLayout();
      ((ISupportInitialize) this.imageLoading).EndInit();
      this.pnlEmailBar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
