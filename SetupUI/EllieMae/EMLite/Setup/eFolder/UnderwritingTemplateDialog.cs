// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.UnderwritingTemplateDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
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
  public class UnderwritingTemplateDialog : Form
  {
    private ConditionTrackingSetup condSetup;
    private UnderwritingConditionTemplate template;
    private DocumentTrackingSetup docSetup;
    private DocumentTemplate[] docList;
    private DocumentTemplate tpoDefaultDoc;
    private Sessions.Session session;
    private const string TPOCondDefaultDocType = "DefaultDoc";
    private const string TPOCondSameNameAsCondition = "SameNameAsCondition";
    private readonly bool isUpdate;
    private IContainer components;
    private CheckBox chkInternal;
    private EMHelpLink helpLink;
    private CheckBox chkAllowToClear;
    private ComboBox cboPriorTo;
    private Label lblPriorTo;
    private Label lblOwner;
    private Label lblDescription;
    private Label lblCategory;
    private TextBox txtName;
    private TextBox txtDescription;
    private Label lblName;
    private ComboBox cboOwner;
    private ComboBox cboCategory;
    private Button btnCancel;
    private Button btnOK;
    private Button btnDocuments;
    private ListBox lstDocuments;
    private CheckBox chkExternal;
    private Label lblPrint;
    private Label lblDaysTillDue;
    private TextBox txtDaysTillDue;
    private RadioButton docSameNameAsCond;
    private RadioButton defaultDoc;
    private Button selectDocBtn;
    private Label defaultDocLabel;
    private GroupBox groupBox1;

    public UnderwritingTemplateDialog(
      ConditionTrackingSetup condSetup,
      UnderwritingConditionTemplate template,
      Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      if (template != null)
        this.isUpdate = true;
      this.helpLink.AssignSession(session);
      this.condSetup = condSetup;
      this.template = template;
      this.initDocumentList();
      this.initOwnerField();
      this.loadTemplateInfo();
    }

    private void loadTemplateInfo()
    {
      if (this.template == null)
        this.template = new UnderwritingConditionTemplate();
      this.txtName.Text = this.template.Name;
      this.txtDescription.Text = this.template.Description;
      this.docList = this.template.GetDocuments(this.docSetup);
      this.cboCategory.Text = this.template.Category;
      this.chkAllowToClear.Checked = this.template.AllowToClear;
      this.chkInternal.Checked = this.template.IsInternal;
      this.chkExternal.Checked = this.template.IsExternal;
      if (this.template.DaysTillDue > 0)
        this.txtDaysTillDue.Text = this.template.DaysTillDue.ToString();
      switch (this.template.PriorTo)
      {
        case "PTA":
          this.cboPriorTo.Text = "Approval";
          break;
        case "PTD":
          this.cboPriorTo.Text = "Docs";
          break;
        case "PTF":
          this.cboPriorTo.Text = "Funding";
          break;
        case "AC":
          this.cboPriorTo.Text = "Closing";
          break;
        case "PTP":
          this.cboPriorTo.Text = "Purchase";
          break;
      }
      foreach (RoleInfo roleInfo in this.cboOwner.Items)
      {
        if (roleInfo.ID == this.template.ForRoleID)
          this.cboOwner.SelectedItem = (object) roleInfo;
      }
      this.loadDocumentList();
      this.initTPOCondDocType(this.template.TPOCondDocType, this.template.TPOCondDocGuid);
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
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + name + "' name has been used already. Please enter a different one.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.getTPOCondDocType() == "DefaultDoc" && this.tpoDefaultDoc == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select the Default Document to upload TPO attachments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      RoleInfo selectedItem = (RoleInfo) this.cboOwner.SelectedItem;
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid Role specified in the \"Owner\"field.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      this.template.Category = this.cboCategory.Text;
      this.template.ForRoleID = selectedItem.ID;
      this.template.AllowToClear = this.chkAllowToClear.Checked;
      this.template.DaysTillDue = num1;
      this.template.IsInternal = this.chkInternal.Checked;
      this.template.IsExternal = this.chkExternal.Checked;
      this.template.TPOCondDocType = this.getTPOCondDocType();
      this.template.TPOCondDocGuid = this.template.TPOCondDocType == "DefaultDoc" ? this.tpoDefaultDoc.Guid : "";
      switch (this.cboPriorTo.Text)
      {
        case "Approval":
          this.template.PriorTo = "PTA";
          break;
        case "Docs":
          this.template.PriorTo = "PTD";
          break;
        case "Funding":
          this.template.PriorTo = "PTF";
          break;
        case "Closing":
          this.template.PriorTo = "AC";
          break;
        case "Purchase":
          this.template.PriorTo = "PTP";
          break;
      }
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

    private void initOwnerField()
    {
      this.cboOwner.Items.AddRange((object[]) ((WorkflowManager) this.session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions());
    }

    private void initDocumentList()
    {
      this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
    }

    private void initTPOCondDocType(string doctype, string docguid)
    {
      switch (doctype)
      {
        case "SameNameAsCondition":
          this.docSameNameAsCond.Checked = true;
          break;
        case "DefaultDoc":
          this.tpoDefaultDoc = this.template.GetTPODefaultDoc(this.docSetup, this.template.TPOCondDocGuid);
          if (this.tpoDefaultDoc != null)
            this.defaultDocLabel.Text = this.tpoDefaultDoc.Name;
          this.defaultDoc.Checked = true;
          this.selectDocBtn.Enabled = true;
          break;
      }
    }

    private string getTPOCondDocType()
    {
      if (this.docSameNameAsCond.Checked)
        return "SameNameAsCondition";
      return this.defaultDoc.Checked ? "DefaultDoc" : "";
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

    private void selectDocBtn_Click(object sender, EventArgs e)
    {
      DocumentTemplate documentTemplate = (DocumentTemplate) null;
      if (this.tpoDefaultDoc != null)
        documentTemplate = this.tpoDefaultDoc;
      else if (this.docList.Length != 0)
        documentTemplate = this.docList[0];
      using (ConditionDocumentsDialog conditionDocumentsDialog = new ConditionDocumentsDialog(this.session, this.docSetup, this.docList, false, this.tpoDefaultDoc))
      {
        if (conditionDocumentsDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        DocumentTemplate[] documents = conditionDocumentsDialog.Documents;
        if (documents.Length == 0)
          return;
        this.tpoDefaultDoc = documents[0];
        this.defaultDocLabel.Text = this.tpoDefaultDoc.Name;
      }
    }

    private void docSameNameAsCond_CheckedChanged(object sender, EventArgs e)
    {
      if (this.defaultDoc.Checked)
        this.selectDocBtn.Enabled = true;
      else
        this.selectDocBtn.Enabled = false;
    }

    private void defaultDoc_CheckedChanged(object sender, EventArgs e)
    {
      if (this.defaultDoc.Checked)
        this.selectDocBtn.Enabled = true;
      else
        this.selectDocBtn.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkInternal = new CheckBox();
      this.chkAllowToClear = new CheckBox();
      this.cboPriorTo = new ComboBox();
      this.lblPriorTo = new Label();
      this.lblOwner = new Label();
      this.lblDescription = new Label();
      this.lblCategory = new Label();
      this.txtName = new TextBox();
      this.txtDescription = new TextBox();
      this.lblName = new Label();
      this.cboOwner = new ComboBox();
      this.cboCategory = new ComboBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.btnDocuments = new Button();
      this.lstDocuments = new ListBox();
      this.chkExternal = new CheckBox();
      this.lblPrint = new Label();
      this.lblDaysTillDue = new Label();
      this.txtDaysTillDue = new TextBox();
      this.docSameNameAsCond = new RadioButton();
      this.defaultDoc = new RadioButton();
      this.selectDocBtn = new Button();
      this.defaultDocLabel = new Label();
      this.groupBox1 = new GroupBox();
      this.helpLink = new EMHelpLink();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.chkInternal.AutoSize = true;
      this.chkInternal.Location = new Point(114, 280);
      this.chkInternal.Name = "chkInternal";
      this.chkInternal.Size = new Size(69, 18);
      this.chkInternal.TabIndex = 14;
      this.chkInternal.Text = "Internally";
      this.chkAllowToClear.AutoSize = true;
      this.chkAllowToClear.Location = new Point(319, 229);
      this.chkAllowToClear.Name = "chkAllowToClear";
      this.chkAllowToClear.Size = new Size(94, 18);
      this.chkAllowToClear.TabIndex = 12;
      this.chkAllowToClear.Text = "Allow to Clear";
      this.cboPriorTo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriorTo.Items.AddRange(new object[5]
      {
        (object) "Approval",
        (object) "Docs",
        (object) "Funding",
        (object) "Closing",
        (object) "Purchase"
      });
      this.cboPriorTo.Location = new Point(113, 198);
      this.cboPriorTo.MaxDropDownItems = 15;
      this.cboPriorTo.Name = "cboPriorTo";
      this.cboPriorTo.Size = new Size(192, 22);
      this.cboPriorTo.TabIndex = 9;
      this.lblPriorTo.AutoSize = true;
      this.lblPriorTo.Location = new Point(12, 202);
      this.lblPriorTo.Name = "lblPriorTo";
      this.lblPriorTo.Size = new Size(43, 14);
      this.lblPriorTo.TabIndex = 8;
      this.lblPriorTo.Text = "Prior To";
      this.lblOwner.AutoSize = true;
      this.lblOwner.Location = new Point(12, 230);
      this.lblOwner.Name = "lblOwner";
      this.lblOwner.Size = new Size(41, 14);
      this.lblOwner.TabIndex = 10;
      this.lblOwner.Text = "Owner";
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(12, 40);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(61, 14);
      this.lblDescription.TabIndex = 2;
      this.lblDescription.Text = "Description";
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(12, 174);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(51, 14);
      this.lblCategory.TabIndex = 6;
      this.lblCategory.Text = "Category";
      this.txtName.Location = new Point(113, 8);
      this.txtName.MaxLength = 256;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(406, 20);
      this.txtName.TabIndex = 1;
      this.txtDescription.Location = new Point(113, 36);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(406, 48);
      this.txtDescription.TabIndex = 3;
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(12, 12);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(34, 14);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name";
      this.cboOwner.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOwner.Location = new Point(113, 226);
      this.cboOwner.MaxDropDownItems = 15;
      this.cboOwner.Name = "cboOwner";
      this.cboOwner.Size = new Size(192, 22);
      this.cboOwner.Sorted = true;
      this.cboOwner.TabIndex = 11;
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.Items.AddRange(new object[6]
      {
        (object) "Assets",
        (object) "Credit",
        (object) "Income",
        (object) "Legal",
        (object) "Misc",
        (object) "Property"
      });
      this.cboCategory.Location = new Point(113, 170);
      this.cboCategory.MaxDropDownItems = 15;
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(192, 22);
      this.cboCategory.TabIndex = 7;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(430, 429);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 17;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Location = new Point(349, 429);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 16;
      this.btnOK.Text = "Save";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnDocuments.Location = new Point(12, 92);
      this.btnDocuments.Name = "btnDocuments";
      this.btnDocuments.Size = new Size(75, 22);
      this.btnDocuments.TabIndex = 4;
      this.btnDocuments.Text = "Documents";
      this.btnDocuments.Click += new EventHandler(this.btnDocuments_Click);
      this.lstDocuments.FormattingEnabled = true;
      this.lstDocuments.ItemHeight = 14;
      this.lstDocuments.Location = new Point(113, 92);
      this.lstDocuments.Name = "lstDocuments";
      this.lstDocuments.SelectionMode = SelectionMode.None;
      this.lstDocuments.Size = new Size(406, 60);
      this.lstDocuments.TabIndex = 5;
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(186, 280);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(73, 18);
      this.chkExternal.TabIndex = 15;
      this.chkExternal.Text = "Externally";
      this.lblPrint.AutoSize = true;
      this.lblPrint.Location = new Point(13, 281);
      this.lblPrint.Name = "lblPrint";
      this.lblPrint.Size = new Size(28, 14);
      this.lblPrint.TabIndex = 13;
      this.lblPrint.Text = "Print";
      this.lblDaysTillDue.AutoSize = true;
      this.lblDaysTillDue.Location = new Point(12, 257);
      this.lblDaysTillDue.Name = "lblDaysTillDue";
      this.lblDaysTillDue.Size = new Size(86, 14);
      this.lblDaysTillDue.TabIndex = 19;
      this.lblDaysTillDue.Text = "Days to Receive";
      this.txtDaysTillDue.Location = new Point(114, 254);
      this.txtDaysTillDue.Name = "txtDaysTillDue";
      this.txtDaysTillDue.Size = new Size(116, 20);
      this.txtDaysTillDue.TabIndex = 20;
      this.docSameNameAsCond.AutoSize = true;
      this.docSameNameAsCond.Checked = true;
      this.docSameNameAsCond.Location = new Point(8, 18);
      this.docSameNameAsCond.Name = "docSameNameAsCond";
      this.docSameNameAsCond.Size = new Size(216, 18);
      this.docSameNameAsCond.TabIndex = 23;
      this.docSameNameAsCond.TabStop = true;
      this.docSameNameAsCond.Text = "Document with same name as condition";
      this.docSameNameAsCond.UseVisualStyleBackColor = true;
      this.docSameNameAsCond.CheckedChanged += new EventHandler(this.docSameNameAsCond_CheckedChanged);
      this.defaultDoc.AutoSize = true;
      this.defaultDoc.Location = new Point(8, 42);
      this.defaultDoc.Name = "defaultDoc";
      this.defaultDoc.Size = new Size(110, 18);
      this.defaultDoc.TabIndex = 24;
      this.defaultDoc.Text = "Default Document";
      this.defaultDoc.UseVisualStyleBackColor = true;
      this.defaultDoc.CheckedChanged += new EventHandler(this.defaultDoc_CheckedChanged);
      this.selectDocBtn.Enabled = false;
      this.selectDocBtn.Location = new Point(381, 44);
      this.selectDocBtn.Name = "selectDocBtn";
      this.selectDocBtn.Size = new Size(110, 22);
      this.selectDocBtn.TabIndex = 25;
      this.selectDocBtn.Text = "Select Document";
      this.selectDocBtn.Click += new EventHandler(this.selectDocBtn_Click);
      this.defaultDocLabel.AutoSize = true;
      this.defaultDocLabel.ForeColor = SystemColors.InactiveCaptionText;
      this.defaultDocLabel.Location = new Point(126, 44);
      this.defaultDocLabel.Name = "defaultDocLabel";
      this.defaultDocLabel.Size = new Size(0, 14);
      this.defaultDocLabel.TabIndex = 26;
      this.groupBox1.Controls.Add((Control) this.docSameNameAsCond);
      this.groupBox1.Controls.Add((Control) this.selectDocBtn);
      this.groupBox1.Controls.Add((Control) this.defaultDocLabel);
      this.groupBox1.Controls.Add((Control) this.defaultDoc);
      this.groupBox1.Location = new Point(12, 330);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(497, 83);
      this.groupBox1.TabIndex = 27;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Automatically upload TPO attachments to";
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Conditions";
      this.helpLink.Location = new Point(14, 431);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 17);
      this.helpLink.TabIndex = 18;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(524, 470);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.lblDaysTillDue);
      this.Controls.Add((Control) this.txtDaysTillDue);
      this.Controls.Add((Control) this.lblPrint);
      this.Controls.Add((Control) this.chkExternal);
      this.Controls.Add((Control) this.lstDocuments);
      this.Controls.Add((Control) this.btnDocuments);
      this.Controls.Add((Control) this.chkInternal);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.chkAllowToClear);
      this.Controls.Add((Control) this.cboPriorTo);
      this.Controls.Add((Control) this.lblPriorTo);
      this.Controls.Add((Control) this.lblOwner);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.lblCategory);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.cboOwner);
      this.Controls.Add((Control) this.cboCategory);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UnderwritingTemplateDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Underwriting Condition";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
