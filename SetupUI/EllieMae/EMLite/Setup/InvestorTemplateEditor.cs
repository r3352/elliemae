// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.InvestorTemplateEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class InvestorTemplateEditor : Form
  {
    private InvestorTemplate template;
    private Sessions.Session session;
    private IContainer components;
    private GroupBox groupBox1;
    private Panel panel2;
    private TextBox txtName;
    private Label label1;
    private Panel panel3;
    private Button btnSave;
    private Button btnCancel;
    private InvestorAddressEditor ctlContactInfo;
    private TextBox txtPairOff;
    private TextBox txtDelivery;
    private RadioButton radBulkNo;
    private RadioButton radBulkYes;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    protected EMHelpLink emHelpLink1;
    private GroupBox groupBox2;
    private Label label7;
    private ComboBox cboTypeOfPurchaser;

    public InvestorTemplateEditor()
      : this(Session.DefaultInstance)
    {
    }

    public InvestorTemplateEditor(Sessions.Session session)
      : this(session, new InvestorTemplate())
    {
    }

    public InvestorTemplateEditor(Sessions.Session session, InvestorTemplate template)
    {
      this.session = session;
      this.initializeControls();
      this.emHelpLink1.AssignSession(session);
      this.template = template;
      this.loadTemplateData();
    }

    private void initializeControls()
    {
      this.ctlContactInfo = new InvestorAddressEditor(this.session);
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtDelivery, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.txtPairOff, TextBoxContentRule.NonNegativeDecimal);
      FieldDefinition field = EncompassFields.GetField("1397");
      this.cboTypeOfPurchaser.DisplayMember = "Text";
      this.cboTypeOfPurchaser.ValueMember = "Value";
      this.cboTypeOfPurchaser.DataSource = (object) field.Options.ToArray();
    }

    private void loadTemplateData()
    {
      this.txtName.Text = this.template.TemplateName;
      this.ctlContactInfo.CurrentInvestor = this.template.CompanyInformation;
      if (this.template.BulkSale)
        this.radBulkYes.Checked = true;
      else
        this.radBulkNo.Checked = true;
      if (this.template.CompanyInformation.DeliveryTimeFrame > 0)
        this.txtDelivery.Text = this.template.CompanyInformation.DeliveryTimeFrame.ToString();
      else
        this.txtDelivery.Text = "";
      if (this.template.CompanyInformation.PairOffFee > 0M)
        this.txtPairOff.Text = this.template.CompanyInformation.PairOffFee.ToString();
      else
        this.txtPairOff.Text = "";
      try
      {
        if (string.IsNullOrEmpty(this.template.CompanyInformation.TypeOfPurchaser))
          return;
        FieldDefinition field = EncompassFields.GetField("1397");
        FieldOption fieldOption = field.Options.GetOptionByValue(this.template.CompanyInformation.TypeOfPurchaser) ?? field.Options.GetOptionByText(this.template.CompanyInformation.TypeOfPurchaser);
        if (fieldOption == null)
          return;
        this.cboTypeOfPurchaser.SelectedValue = (object) fieldOption.Value;
      }
      catch
      {
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.performTemplateValidation())
        return;
      this.commitTemplateData();
      if (!this.saveTemplate())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void commitTemplateData()
    {
      this.ctlContactInfo.CommitChanges();
      this.template.BulkSale = this.radBulkYes.Checked;
      this.template.CompanyInformation.DeliveryTimeFrame = Utils.ParseInt((object) this.txtDelivery.Text, 0);
      this.template.CompanyInformation.PairOffFee = Utils.ParseDecimal((object) this.txtPairOff.Text, 0M);
      if (this.cboTypeOfPurchaser.SelectedIndex <= -1)
        return;
      this.template.CompanyInformation.TypeOfPurchaser = this.cboTypeOfPurchaser.SelectedValue.ToString();
    }

    private bool saveTemplate()
    {
      string templateName = this.template.TemplateName;
      string entryName = this.txtName.Text.Trim();
      if (templateName != "" && templateName != entryName)
        this.session.ConfigurationManager.DeleteTemplateSettingsObject(TemplateSettingsType.Investor, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, templateName, FileSystemEntry.Types.File, (string) null));
      this.template.TemplateName = entryName;
      this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.Investor, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, entryName, FileSystemEntry.Types.File, (string) null), (BinaryObject) (BinaryConvertibleObject) this.template);
      return true;
    }

    private bool performTemplateValidation()
    {
      string str = this.txtName.Text.Trim();
      if (str == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A name must be provided for this template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      FileSystemEntry fileEntry = new FileSystemEntry(FileSystemEntry.PublicRoot.Path, str, FileSystemEntry.Types.File, (string) null);
      if (string.Compare(str, this.template.TemplateName, true) == 0 || !this.session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.Investor, fileEntry))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The name '" + str + "' is already in use. Select a new name and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void InvestorTemplateEditor_Load(object sender, EventArgs e) => this.txtName.Focus();

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.txtPairOff = new TextBox();
      this.txtDelivery = new TextBox();
      this.radBulkNo = new RadioButton();
      this.radBulkYes = new RadioButton();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.panel2 = new Panel();
      this.txtName = new TextBox();
      this.label1 = new Label();
      this.panel3 = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.groupBox2 = new GroupBox();
      this.cboTypeOfPurchaser = new ComboBox();
      this.label7 = new Label();
      this.groupBox1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.txtPairOff);
      this.groupBox1.Controls.Add((Control) this.txtDelivery);
      this.groupBox1.Controls.Add((Control) this.radBulkNo);
      this.groupBox1.Controls.Add((Control) this.radBulkYes);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(16, 313);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(320, 94);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Loan Trade Management";
      this.txtPairOff.Location = new Point(244, 49);
      this.txtPairOff.Name = "txtPairOff";
      this.txtPairOff.Size = new Size(66, 20);
      this.txtPairOff.TabIndex = 4;
      this.txtPairOff.TextAlign = HorizontalAlignment.Right;
      this.txtPairOff.Visible = false;
      this.txtDelivery.Location = new Point(140, 49);
      this.txtDelivery.Name = "txtDelivery";
      this.txtDelivery.Size = new Size(66, 20);
      this.txtDelivery.TabIndex = 3;
      this.txtDelivery.TextAlign = HorizontalAlignment.Right;
      this.radBulkNo.AutoSize = true;
      this.radBulkNo.Checked = true;
      this.radBulkNo.Location = new Point(196, 23);
      this.radBulkNo.Name = "radBulkNo";
      this.radBulkNo.Size = new Size(39, 17);
      this.radBulkNo.TabIndex = 2;
      this.radBulkNo.TabStop = true;
      this.radBulkNo.Text = "No";
      this.radBulkNo.UseVisualStyleBackColor = true;
      this.radBulkYes.AutoSize = true;
      this.radBulkYes.Checked = true;
      this.radBulkYes.Location = new Point(140, 23);
      this.radBulkYes.Name = "radBulkYes";
      this.radBulkYes.Size = new Size(43, 17);
      this.radBulkYes.TabIndex = 1;
      this.radBulkYes.TabStop = true;
      this.radBulkYes.Text = "Yes";
      this.radBulkYes.UseVisualStyleBackColor = true;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(209, 54);
      this.label6.Name = "label6";
      this.label6.Size = new Size(29, 13);
      this.label6.TabIndex = 4;
      this.label6.Text = "days";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(278, 72);
      this.label5.Name = "label5";
      this.label5.Size = new Size(15, 13);
      this.label5.TabIndex = 3;
      this.label5.Text = "%";
      this.label5.Visible = false;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(241, 25);
      this.label4.Name = "label4";
      this.label4.Size = new Size(66, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Pair Off Fee:";
      this.label4.Visible = false;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(22, 54);
      this.label3.Name = "label3";
      this.label3.Size = new Size(106, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Delivery Time Frame:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(22, 27);
      this.label2.Name = "label2";
      this.label2.Size = new Size(55, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Bulk Sale:";
      this.panel2.Controls.Add((Control) this.txtName);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(354, 34);
      this.panel2.TabIndex = 1;
      this.txtName.Location = new Point(102, 11);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(234, 20);
      this.txtName.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(79, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Template Name:";
      this.panel3.Controls.Add((Control) this.emHelpLink1);
      this.panel3.Controls.Add((Control) this.btnSave);
      this.panel3.Controls.Add((Control) this.btnCancel);
      this.panel3.Dock = DockStyle.Bottom;
      this.panel3.Location = new Point(0, 472);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(354, 47);
      this.panel3.TabIndex = 4;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Investor Templates";
      this.emHelpLink1.Location = new Point(16, 14);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 111;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(188, 11);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(268, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.ctlContactInfo.AssigneeNameTextBoxText = "";
      this.ctlContactInfo.BackColor = Color.Transparent;
      this.ctlContactInfo.CurrentAssignee = (ContactInformation) null;
      this.ctlContactInfo.CurrentDealer = (ContactInformation) null;
      this.ctlContactInfo.CurrentInvestor = (Investor) null;
      this.ctlContactInfo.Location = new Point(16, 44);
      this.ctlContactInfo.Name = "ctlContactInfo";
      this.ctlContactInfo.ReadOnly = false;
      this.ctlContactInfo.Size = new Size(320, 260);
      this.ctlContactInfo.TabIndex = 2;
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.cboTypeOfPurchaser);
      this.groupBox2.Location = new Point(16, 413);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(320, 54);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "HMDA Information";
      this.cboTypeOfPurchaser.FormattingEnabled = true;
      this.cboTypeOfPurchaser.Location = new Point(103, 20);
      this.cboTypeOfPurchaser.Name = "comboBox1";
      this.cboTypeOfPurchaser.Size = new Size(207, 21);
      this.cboTypeOfPurchaser.TabIndex = 0;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 23);
      this.label7.Name = "label7";
      this.label7.Size = new Size(97, 13);
      this.label7.TabIndex = 1;
      this.label7.Text = "Type of Purchaser:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(354, 519);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.ctlContactInfo);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel3);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InvestorTemplateEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create/Edit Investor Template";
      this.Load += new EventHandler(this.InvestorTemplateEditor_Load);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
    }

    private class FileSystemEntryWrapper
    {
      private FileSystemEntry fsEntry;

      public FileSystemEntryWrapper(FileSystemEntry e) => this.fsEntry = e;

      public FileSystemEntry FileSystemEntry => this.fsEntry;

      public override string ToString() => this.fsEntry.Name;
    }
  }
}
