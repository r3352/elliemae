// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.PostClosingTemplateDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class PostClosingTemplateDialog : Form
  {
    private ConditionTrackingSetup condSetup;
    private PostClosingConditionTemplate template;
    private DocumentTrackingSetup docSetup;
    private DocumentTemplate[] docList;
    private Sessions.Session session;
    private readonly bool isUpdate;
    private IContainer components;
    private EMHelpLink helpLink;
    private Label label2;
    private Label label1;
    private Label typeLbl;
    private TextBox txtName;
    private TextBox txtDescription;
    private Label nameLbl;
    private ComboBox cboSource;
    private Button cancelBtn;
    private Button okBtn;
    private ComboBox cboRecipient;
    private ListBox lstDocuments;
    private Button btnDocuments;
    private Label lblDaysTillDue;
    private TextBox txtDaysTillDue;
    private Label lblPrint;
    private CheckBox chkExternal;
    private CheckBox chkInternal;

    public PostClosingTemplateDialog(
      Sessions.Session session,
      ConditionTrackingSetup condSetup,
      PostClosingConditionTemplate template)
    {
      this.InitializeComponent();
      this.session = session;
      if (template != null)
        this.isUpdate = true;
      this.helpLink.AssignSession(session);
      this.condSetup = condSetup;
      this.template = template;
      this.initDocumentList();
      this.loadTemplateInfo();
    }

    private void loadTemplateInfo()
    {
      if (this.template == null)
        this.template = new PostClosingConditionTemplate();
      this.txtName.Text = this.template.Name;
      this.txtDescription.Text = this.template.Description;
      this.docList = this.template.GetDocuments(this.docSetup);
      this.cboSource.Text = this.template.Source;
      this.cboRecipient.Text = this.template.Recipient;
      this.chkInternal.Checked = this.template.IsInternal;
      this.chkExternal.Checked = this.template.IsExternal;
      if (this.template.DaysTillDue > 0)
        this.txtDaysTillDue.Text = this.template.DaysTillDue.ToString();
      this.loadDocumentList();
    }

    private bool saveTemplateInfo()
    {
      string name = this.txtName.Text.Trim();
      if (name == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Condition Name", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.template.Name != name && this.condSetup.Contains(name))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + name + "' name has been used. Please enter a different one.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      int num1 = 0;
      if (!string.IsNullOrWhiteSpace(this.txtDaysTillDue.Text))
      {
        num1 = Utils.ParseInt((object) this.txtDaysTillDue.Text, -1);
        if (num1 < 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Invalid Days to Receive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      this.template.Name = name.Trim();
      this.template.Description = this.txtDescription.Text.Trim();
      this.template.SetDocuments(this.docList);
      this.template.Source = this.cboSource.Text;
      this.template.Recipient = this.cboRecipient.Text;
      this.template.DaysTillDue = num1;
      this.template.IsInternal = this.chkInternal.Checked;
      this.template.IsExternal = this.chkExternal.Checked;
      try
      {
        if (this.isUpdate)
          this.session.ConfigurationManager.UpdateConditionTrackingSetup((ConditionTemplate) this.template);
        else
          this.session.ConfigurationManager.AddConditionTrackingSetup((ConditionTemplate) this.template);
      }
      catch (Exception ex) when (
      {
        // ISSUE: unable to correctly present filter
        byte? nullable1 = ex.InnerException is SqlException innerException ? new byte?(innerException.Class) : new byte?();
        int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
        int num3 = 15;
        if (nullable2.GetValueOrDefault() == num3 & nullable2.HasValue)
        {
          SuccessfulFiltering;
        }
        else
          throw;
      }
      )
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "Update Failed - This condition was modified by another user. Please refresh and retry to edit condition");
        return false;
      }
      return true;
    }

    private void initDocumentList()
    {
      this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
    }

    private void loadDocumentList()
    {
      this.lstDocuments.Items.Clear();
      foreach (object doc in this.docList)
        this.lstDocuments.Items.Add(doc);
    }

    private void btnDocuments_Click(object sender, EventArgs e)
    {
      using (ConditionDocumentsDialog conditionDocumentsDialog = new ConditionDocumentsDialog(this.session, this.docSetup, this.docList))
      {
        if (conditionDocumentsDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.docList = conditionDocumentsDialog.Documents;
        this.loadDocumentList();
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.saveTemplateInfo())
        return;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.helpLink = new EMHelpLink();
      this.label2 = new Label();
      this.label1 = new Label();
      this.typeLbl = new Label();
      this.txtName = new TextBox();
      this.txtDescription = new TextBox();
      this.nameLbl = new Label();
      this.cboSource = new ComboBox();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.cboRecipient = new ComboBox();
      this.lstDocuments = new ListBox();
      this.btnDocuments = new Button();
      this.lblDaysTillDue = new Label();
      this.txtDaysTillDue = new TextBox();
      this.lblPrint = new Label();
      this.chkExternal = new CheckBox();
      this.chkInternal = new CheckBox();
      this.SuspendLayout();
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Setup\\Post-Closing Conditions";
      this.helpLink.Location = new Point(12, 279);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 17);
      this.helpLink.TabIndex = 14;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 192);
      this.label2.Name = "label2";
      this.label2.Size = new Size(51, 14);
      this.label2.TabIndex = 8;
      this.label2.Text = "Recipient";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(61, 14);
      this.label1.TabIndex = 2;
      this.label1.Text = "Description";
      this.typeLbl.AutoSize = true;
      this.typeLbl.Location = new Point(12, 164);
      this.typeLbl.Name = "typeLbl";
      this.typeLbl.Size = new Size(42, 14);
      this.typeLbl.TabIndex = 6;
      this.typeLbl.Text = "Source";
      this.txtName.Location = new Point(104, 8);
      this.txtName.MaxLength = 256;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(398, 20);
      this.txtName.TabIndex = 1;
      this.txtDescription.Location = new Point(104, 36);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(398, 48);
      this.txtDescription.TabIndex = 3;
      this.nameLbl.AutoSize = true;
      this.nameLbl.Location = new Point(12, 12);
      this.nameLbl.Name = "nameLbl";
      this.nameLbl.Size = new Size(34, 14);
      this.nameLbl.TabIndex = 0;
      this.nameLbl.Text = "Name";
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.Items.AddRange(new object[8]
      {
        (object) "Escrow",
        (object) "Investor",
        (object) "Recorder's Office",
        (object) "Borrowers",
        (object) "FHA",
        (object) "VA",
        (object) "MI Company",
        (object) "Other"
      });
      this.cboSource.Location = new Point(104, 160);
      this.cboSource.MaxDropDownItems = 15;
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new Size(116, 22);
      this.cboSource.TabIndex = 7;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(428, 277);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 13;
      this.cancelBtn.Text = "Cancel";
      this.okBtn.Location = new Point(347, 277);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 12;
      this.okBtn.Text = "Save";
      this.okBtn.Click += new EventHandler(this.btnOK_Click);
      this.cboRecipient.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRecipient.Items.AddRange(new object[2]
      {
        (object) "Investor",
        (object) "MERS"
      });
      this.cboRecipient.Location = new Point(104, 188);
      this.cboRecipient.MaxDropDownItems = 15;
      this.cboRecipient.Name = "cboRecipient";
      this.cboRecipient.Size = new Size(116, 22);
      this.cboRecipient.TabIndex = 9;
      this.lstDocuments.FormattingEnabled = true;
      this.lstDocuments.ItemHeight = 14;
      this.lstDocuments.Location = new Point(104, 92);
      this.lstDocuments.Name = "lstDocuments";
      this.lstDocuments.SelectionMode = SelectionMode.None;
      this.lstDocuments.Size = new Size(398, 60);
      this.lstDocuments.TabIndex = 5;
      this.lstDocuments.TabStop = false;
      this.btnDocuments.Location = new Point(12, 92);
      this.btnDocuments.Name = "btnDocuments";
      this.btnDocuments.Size = new Size(75, 22);
      this.btnDocuments.TabIndex = 4;
      this.btnDocuments.Text = "Documents";
      this.btnDocuments.Click += new EventHandler(this.btnDocuments_Click);
      this.lblDaysTillDue.AutoSize = true;
      this.lblDaysTillDue.Location = new Point(12, 219);
      this.lblDaysTillDue.Name = "lblDaysTillDue";
      this.lblDaysTillDue.Size = new Size(86, 14);
      this.lblDaysTillDue.TabIndex = 10;
      this.lblDaysTillDue.Text = "Days to Receive";
      this.txtDaysTillDue.Location = new Point(104, 216);
      this.txtDaysTillDue.Name = "txtDaysTillDue";
      this.txtDaysTillDue.Size = new Size(116, 20);
      this.txtDaysTillDue.TabIndex = 11;
      this.lblPrint.AutoSize = true;
      this.lblPrint.Location = new Point(13, 247);
      this.lblPrint.Name = "lblPrint";
      this.lblPrint.Size = new Size(28, 14);
      this.lblPrint.TabIndex = 16;
      this.lblPrint.Text = "Print";
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(176, 246);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(73, 18);
      this.chkExternal.TabIndex = 18;
      this.chkExternal.Text = "Externally";
      this.chkInternal.AutoSize = true;
      this.chkInternal.Location = new Point(104, 246);
      this.chkInternal.Name = "chkInternal";
      this.chkInternal.Size = new Size(69, 18);
      this.chkInternal.TabIndex = 17;
      this.chkInternal.Text = "Internally";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(513, 312);
      this.Controls.Add((Control) this.lblPrint);
      this.Controls.Add((Control) this.chkExternal);
      this.Controls.Add((Control) this.chkInternal);
      this.Controls.Add((Control) this.lblDaysTillDue);
      this.Controls.Add((Control) this.txtDaysTillDue);
      this.Controls.Add((Control) this.lstDocuments);
      this.Controls.Add((Control) this.btnDocuments);
      this.Controls.Add((Control) this.cboRecipient);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.typeLbl);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.nameLbl);
      this.Controls.Add((Control) this.cboSource);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PostClosingTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Post-Closing Condition";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
