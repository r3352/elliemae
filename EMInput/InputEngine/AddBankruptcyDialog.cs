// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddBankruptcyDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddBankruptcyDialog : Form
  {
    private IHtmlInput inputData;
    private PopupBusinessRules popupRules;
    private LoanData loan;
    private Sessions.Session session;
    private List<KeyValuePair<string, CheckBox>> checkBoxControls;
    private IContainer components;
    private GroupContainer borrowergroupContainer;
    private CheckBox chkBorrower1;
    private GroupContainer groupContainer2;
    private Label label6;
    private CheckBox chkBorrower7;
    private Label label5;
    private CheckBox chkBorrower6;
    private Label label4;
    private CheckBox chkBorrower4;
    private Label label3;
    private CheckBox chkBorrower3;
    private Label label2;
    private CheckBox chkBorrower2;
    private Label label1;
    private Label label7;
    private CheckBox chkCoBorrower7;
    private Label label8;
    private CheckBox chkCoBorrower6;
    private Label label9;
    private CheckBox chkCoBorrower4;
    private Label label10;
    private CheckBox chkCoBorrower3;
    private Label label11;
    private CheckBox chkCoBorrower2;
    private Label label12;
    private CheckBox chkCoBorrower1;
    private Label label13;
    private Label label14;
    private CheckBox chkBorrower8;
    private CheckBox chkCoBorrower8;
    private TextBox txtBorrowerOther;
    private TextBox txtCoBorrowerOther;
    private Label label15;
    private CheckBox chkBorrower5;
    private Label label16;
    private CheckBox chkCoBorrower5;
    private Button btnSave;
    private Button btnCancel;
    private ToolTip fieldToolTip;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;

    public AddBankruptcyDialog(IHtmlInput inputData, Sessions.Session session)
    {
      this.inputData = inputData;
      this.session = session;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.InitializeComponent();
      this.checkBoxControls = new List<KeyValuePair<string, CheckBox>>();
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X79", this.chkBorrower1));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X80", this.chkBorrower2));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X81", this.chkBorrower3));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X82", this.chkBorrower4));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X151", this.chkBorrower5));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X152", this.chkBorrower6));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X154", this.chkBorrower7));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X83", this.chkBorrower8));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X85", this.chkCoBorrower1));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X86", this.chkCoBorrower2));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X87", this.chkCoBorrower3));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X88", this.chkCoBorrower4));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X155", this.chkCoBorrower5));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X156", this.chkCoBorrower6));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X153", this.chkCoBorrower7));
      this.checkBoxControls.Add(new KeyValuePair<string, CheckBox>("MORNET.X89", this.chkCoBorrower8));
      this.initForm();
      this.setBusinessRule();
    }

    private void initForm()
    {
      foreach (KeyValuePair<string, CheckBox> checkBoxControl in this.checkBoxControls)
      {
        this.getField(checkBoxControl.Key, checkBoxControl.Value);
        this.fieldToolTip.SetToolTip((Control) checkBoxControl.Value, checkBoxControl.Key);
      }
      this.txtBorrowerOther.Enabled = false;
      if (this.inputData.GetField("MORNET.X83") == "Y")
      {
        this.txtBorrowerOther.Enabled = true;
        this.txtBorrowerOther.Text = this.inputData.GetField("MORNET.X84") != string.Empty ? this.inputData.GetField("MORNET.X84") : string.Empty;
      }
      this.fieldToolTip.SetToolTip((Control) this.txtBorrowerOther, "MORNET.X84");
      this.txtCoBorrowerOther.Enabled = false;
      if (this.inputData.GetField("MORNET.X89") == "Y")
      {
        this.txtCoBorrowerOther.Enabled = true;
        this.txtCoBorrowerOther.Text = this.inputData.GetField("MORNET.X90") != string.Empty ? this.inputData.GetField("MORNET.X90") : string.Empty;
      }
      this.fieldToolTip.SetToolTip((Control) this.txtCoBorrowerOther, "MORNET.X90");
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      foreach (KeyValuePair<string, CheckBox> checkBoxControl in this.checkBoxControls)
      {
        if (!PopupBusinessRules.RequiredFieldRuleCheck(checkBoxControl.Key, this.inputData))
          return;
      }
      if (!PopupBusinessRules.RequiredFieldRuleCheck("MORNET.X84", this.inputData) || !PopupBusinessRules.RequiredFieldRuleCheck("MORNET.X90", this.inputData))
        return;
      foreach (KeyValuePair<string, CheckBox> checkBoxControl in this.checkBoxControls)
        this.inputData.SetCurrentField(checkBoxControl.Key, checkBoxControl.Value.Checked ? "Y" : "N");
      this.inputData.SetCurrentField("MORNET.X84", this.txtBorrowerOther.Text != string.Empty ? this.txtBorrowerOther.Text : string.Empty);
      this.inputData.SetCurrentField("MORNET.X90", this.txtCoBorrowerOther.Text != string.Empty ? this.txtCoBorrowerOther.Text : string.Empty);
      this.DialogResult = DialogResult.OK;
    }

    private void getField(string id, CheckBox checkBox)
    {
      checkBox.Checked = this.inputData.GetField(id) == "Y";
    }

    private void setBusinessRule()
    {
      if (this.loan == null)
        return;
      ResourceManager resources = new ResourceManager(typeof (AddBankruptcyDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      foreach (KeyValuePair<string, CheckBox> checkBoxControl in this.checkBoxControls)
        this.popupRules.SetBusinessRules((object) checkBoxControl.Value, checkBoxControl.Key);
      this.popupRules.SetBusinessRules((object) this.txtBorrowerOther, "MORNET.X84");
      this.popupRules.SetBusinessRules((object) this.txtCoBorrowerOther, "MORNET.X90");
    }

    private void chkBorrower8_CheckedChanged(object sender, EventArgs e)
    {
      this.txtBorrowerOther.Enabled = this.chkBorrower8.Checked;
      if (this.chkBorrower8.Checked)
        return;
      this.txtBorrowerOther.Text = "";
    }

    private void chkCoBorrower8_CheckedChanged(object sender, EventArgs e)
    {
      this.txtCoBorrowerOther.Enabled = this.chkCoBorrower8.Checked;
      if (this.chkCoBorrower8.Checked)
        return;
      this.txtCoBorrowerOther.Text = "";
    }

    private void control_Enter(object sender, EventArgs e)
    {
      string id = (string) null;
      if (sender is CheckBox)
      {
        CheckBox checkBox = (CheckBox) sender;
        foreach (KeyValuePair<string, CheckBox> checkBoxControl in this.checkBoxControls)
        {
          if (checkBoxControl.Value.Name == checkBox.Name)
          {
            id = checkBoxControl.Key;
            break;
          }
        }
      }
      else if (sender is TextBox)
      {
        string str1;
        string str2;
        if (!(((Control) sender).Name == "txtBorrowerOther"))
          str2 = str1 = "MORNET.X90";
        else
          str1 = str2 = "MORNET.X84";
        id = str2;
      }
      if (id == null)
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(id);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddBankruptcyDialog));
      this.borrowergroupContainer = new GroupContainer();
      this.label15 = new Label();
      this.chkBorrower5 = new CheckBox();
      this.txtBorrowerOther = new TextBox();
      this.chkBorrower8 = new CheckBox();
      this.label6 = new Label();
      this.chkBorrower7 = new CheckBox();
      this.label5 = new Label();
      this.chkBorrower6 = new CheckBox();
      this.label4 = new Label();
      this.chkBorrower4 = new CheckBox();
      this.label3 = new Label();
      this.chkBorrower3 = new CheckBox();
      this.label2 = new Label();
      this.chkBorrower2 = new CheckBox();
      this.label1 = new Label();
      this.chkBorrower1 = new CheckBox();
      this.groupContainer2 = new GroupContainer();
      this.label16 = new Label();
      this.chkCoBorrower5 = new CheckBox();
      this.txtCoBorrowerOther = new TextBox();
      this.chkCoBorrower8 = new CheckBox();
      this.label7 = new Label();
      this.chkCoBorrower7 = new CheckBox();
      this.label8 = new Label();
      this.chkCoBorrower6 = new CheckBox();
      this.label9 = new Label();
      this.chkCoBorrower4 = new CheckBox();
      this.label10 = new Label();
      this.chkCoBorrower3 = new CheckBox();
      this.label11 = new Label();
      this.chkCoBorrower2 = new CheckBox();
      this.label12 = new Label();
      this.chkCoBorrower1 = new CheckBox();
      this.label13 = new Label();
      this.label14 = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.fieldToolTip = new ToolTip(this.components);
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.borrowergroupContainer.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.borrowergroupContainer.Controls.Add((Control) this.label15);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower5);
      this.borrowergroupContainer.Controls.Add((Control) this.txtBorrowerOther);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower8);
      this.borrowergroupContainer.Controls.Add((Control) this.label6);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower7);
      this.borrowergroupContainer.Controls.Add((Control) this.label5);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower6);
      this.borrowergroupContainer.Controls.Add((Control) this.label4);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower4);
      this.borrowergroupContainer.Controls.Add((Control) this.label3);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower3);
      this.borrowergroupContainer.Controls.Add((Control) this.label2);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower2);
      this.borrowergroupContainer.Controls.Add((Control) this.label1);
      this.borrowergroupContainer.Controls.Add((Control) this.chkBorrower1);
      this.borrowergroupContainer.HeaderForeColor = SystemColors.ControlText;
      this.borrowergroupContainer.Location = new Point(10, 86);
      this.borrowergroupContainer.Margin = new Padding(2, 3, 2, 3);
      this.borrowergroupContainer.Name = "borrowergroupContainer";
      this.borrowergroupContainer.Size = new Size(326, 363);
      this.borrowergroupContainer.TabIndex = 9;
      this.borrowergroupContainer.Text = "Borrower";
      this.label15.Location = new Point(32, 212);
      this.label15.Margin = new Padding(2, 0, 2, 0);
      this.label15.Name = "label15";
      this.label15.Size = new Size(154, 18);
      this.label15.TabIndex = 15;
      this.label15.Text = "(\"Confirmed CR BK Incorrect\")";
      this.chkBorrower5.AutoSize = true;
      this.chkBorrower5.Location = new Point(12, 197);
      this.chkBorrower5.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower5.Name = "chkBorrower5";
      this.chkBorrower5.Size = new Size(189, 17);
      this.chkBorrower5.TabIndex = 14;
      this.chkBorrower5.Text = "Inaccurate Bankruptcy Information";
      this.chkBorrower5.UseVisualStyleBackColor = true;
      this.chkBorrower5.Enter += new EventHandler(this.control_Enter);
      this.txtBorrowerOther.Location = new Point(74, 329);
      this.txtBorrowerOther.Margin = new Padding(2, 3, 2, 3);
      this.txtBorrowerOther.Name = "txtBorrowerOther";
      this.txtBorrowerOther.Size = new Size(198, 20);
      this.txtBorrowerOther.TabIndex = 13;
      this.txtBorrowerOther.Enter += new EventHandler(this.control_Enter);
      this.chkBorrower8.AutoSize = true;
      this.chkBorrower8.Location = new Point(12, 329);
      this.chkBorrower8.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower8.Name = "chkBorrower8";
      this.chkBorrower8.Size = new Size(52, 17);
      this.chkBorrower8.TabIndex = 12;
      this.chkBorrower8.Text = "Other";
      this.chkBorrower8.UseVisualStyleBackColor = true;
      this.chkBorrower8.CheckedChanged += new EventHandler(this.chkBorrower8_CheckedChanged);
      this.chkBorrower8.Enter += new EventHandler(this.control_Enter);
      this.label6.Location = new Point(32, 301);
      this.label6.Margin = new Padding(2, 0, 2, 0);
      this.label6.Name = "label6";
      this.label6.Size = new Size(172, 18);
      this.label6.TabIndex = 11;
      this.label6.Text = "(\"Confirmed Mtg Del Incorrect\") ";
      this.chkBorrower7.AutoSize = true;
      this.chkBorrower7.Location = new Point(12, 281);
      this.chkBorrower7.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower7.Name = "chkBorrower7";
      this.chkBorrower7.Size = new Size(231, 17);
      this.chkBorrower7.TabIndex = 10;
      this.chkBorrower7.Text = "Confirmed Mortgage Delinquency Incorrect ";
      this.chkBorrower7.UseVisualStyleBackColor = true;
      this.chkBorrower7.Enter += new EventHandler(this.control_Enter);
      this.label5.Location = new Point(32, 257);
      this.label5.Margin = new Padding(2, 0, 2, 0);
      this.label5.Name = "label5";
      this.label5.Size = new Size(154, 18);
      this.label5.TabIndex = 9;
      this.label5.Text = "(\"Confirmed CR BK EC\")";
      this.chkBorrower6.AutoSize = true;
      this.chkBorrower6.Location = new Point(12, 237);
      this.chkBorrower6.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower6.Name = "chkBorrower6";
      this.chkBorrower6.Size = new Size(246, 17);
      this.chkBorrower6.TabIndex = 8;
      this.chkBorrower6.Text = "Bankruptcy Due to Extenuating Circumstances";
      this.chkBorrower6.UseVisualStyleBackColor = true;
      this.chkBorrower6.Enter += new EventHandler(this.control_Enter);
      this.label4.Location = new Point(34, 171);
      this.label4.Margin = new Padding(2, 0, 2, 0);
      this.label4.Name = "label4";
      this.label4.Size = new Size(154, 18);
      this.label4.TabIndex = 7;
      this.label4.Text = "(\"Confirmed CR PFS\")";
      this.chkBorrower4.AutoSize = true;
      this.chkBorrower4.Location = new Point(12, 155);
      this.chkBorrower4.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower4.Name = "chkBorrower4";
      this.chkBorrower4.Size = new Size(192, 17);
      this.chkBorrower4.TabIndex = 6;
      this.chkBorrower4.Text = "Preforeclosure Sales or Short Sales";
      this.chkBorrower4.UseVisualStyleBackColor = true;
      this.chkBorrower4.Enter += new EventHandler(this.control_Enter);
      this.label3.Location = new Point(32, 134);
      this.label3.Margin = new Padding(2, 0, 2, 0);
      this.label3.Name = "label3";
      this.label3.Size = new Size(120, 18);
      this.label3.TabIndex = 5;
      this.label3.Text = "(\"Confirmed CR DIL\")";
      this.chkBorrower3.AutoSize = true;
      this.chkBorrower3.Location = new Point(12, 115);
      this.chkBorrower3.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower3.Name = "chkBorrower3";
      this.chkBorrower3.Size = new Size(156, 17);
      this.chkBorrower3.TabIndex = 4;
      this.chkBorrower3.Text = "Deed-in-Lieu of Foreclosure";
      this.chkBorrower3.UseVisualStyleBackColor = true;
      this.chkBorrower3.Enter += new EventHandler(this.control_Enter);
      this.label2.Location = new Point(32, 97);
      this.label2.Margin = new Padding(2, 0, 2, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(154, 18);
      this.label2.TabIndex = 3;
      this.label2.Text = "(\"Confirmed CR FC EC\")";
      this.chkBorrower2.AutoSize = true;
      this.chkBorrower2.Location = new Point(12, 83);
      this.chkBorrower2.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower2.Name = "chkBorrower2";
      this.chkBorrower2.Size = new Size(243, 17);
      this.chkBorrower2.TabIndex = 2;
      this.chkBorrower2.Text = "Foreclosure due to extenuating circumstances";
      this.chkBorrower2.UseVisualStyleBackColor = true;
      this.chkBorrower2.Enter += new EventHandler(this.control_Enter);
      this.label1.Location = new Point(32, 60);
      this.label1.Margin = new Padding(2, 0, 2, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(154, 18);
      this.label1.TabIndex = 1;
      this.label1.Text = "(\"Confirmed CR FC Incorrect\")";
      this.chkBorrower1.AutoSize = true;
      this.chkBorrower1.Location = new Point(12, 42);
      this.chkBorrower1.Margin = new Padding(2, 3, 2, 3);
      this.chkBorrower1.Name = "chkBorrower1";
      this.chkBorrower1.Size = new Size(189, 17);
      this.chkBorrower1.TabIndex = 0;
      this.chkBorrower1.Text = "Inaccurate Foreclosure information";
      this.chkBorrower1.UseVisualStyleBackColor = true;
      this.chkBorrower1.Enter += new EventHandler(this.control_Enter);
      this.groupContainer2.Controls.Add((Control) this.label16);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower5);
      this.groupContainer2.Controls.Add((Control) this.txtCoBorrowerOther);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower8);
      this.groupContainer2.Controls.Add((Control) this.label7);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower7);
      this.groupContainer2.Controls.Add((Control) this.label8);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower6);
      this.groupContainer2.Controls.Add((Control) this.label9);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower4);
      this.groupContainer2.Controls.Add((Control) this.label10);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower3);
      this.groupContainer2.Controls.Add((Control) this.label11);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower2);
      this.groupContainer2.Controls.Add((Control) this.label12);
      this.groupContainer2.Controls.Add((Control) this.chkCoBorrower1);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(342, 86);
      this.groupContainer2.Margin = new Padding(2, 3, 2, 3);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(318, 363);
      this.groupContainer2.TabIndex = 10;
      this.groupContainer2.Text = "CoBorrower";
      this.label16.Location = new Point(34, 212);
      this.label16.Margin = new Padding(2, 0, 2, 0);
      this.label16.Name = "label16";
      this.label16.Size = new Size(154, 18);
      this.label16.TabIndex = 27;
      this.label16.Text = "(\"Confirmed CR BK Incorrect\")";
      this.chkCoBorrower5.AutoSize = true;
      this.chkCoBorrower5.Location = new Point(14, 192);
      this.chkCoBorrower5.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower5.Name = "chkCoBorrower5";
      this.chkCoBorrower5.Size = new Size(189, 17);
      this.chkCoBorrower5.TabIndex = 26;
      this.chkCoBorrower5.Text = "Inaccurate Bankruptcy Information";
      this.chkCoBorrower5.UseVisualStyleBackColor = true;
      this.chkCoBorrower5.Enter += new EventHandler(this.control_Enter);
      this.txtCoBorrowerOther.Location = new Point(74, 327);
      this.txtCoBorrowerOther.Margin = new Padding(2, 3, 2, 3);
      this.txtCoBorrowerOther.Name = "txtCoBorrowerOther";
      this.txtCoBorrowerOther.Size = new Size(198, 20);
      this.txtCoBorrowerOther.TabIndex = 25;
      this.txtCoBorrowerOther.Enter += new EventHandler(this.control_Enter);
      this.chkCoBorrower8.AutoSize = true;
      this.chkCoBorrower8.Location = new Point(14, 329);
      this.chkCoBorrower8.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower8.Name = "chkCoBorrower8";
      this.chkCoBorrower8.Size = new Size(52, 17);
      this.chkCoBorrower8.TabIndex = 24;
      this.chkCoBorrower8.Text = "Other";
      this.chkCoBorrower8.UseVisualStyleBackColor = true;
      this.chkCoBorrower8.CheckedChanged += new EventHandler(this.chkCoBorrower8_CheckedChanged);
      this.chkCoBorrower8.Enter += new EventHandler(this.control_Enter);
      this.label7.Location = new Point(32, 297);
      this.label7.Margin = new Padding(2, 0, 2, 0);
      this.label7.Name = "label7";
      this.label7.Size = new Size(172, 18);
      this.label7.TabIndex = 23;
      this.label7.Text = "(\"Confirmed Mtg Del Incorrect\") ";
      this.chkCoBorrower7.AutoSize = true;
      this.chkCoBorrower7.Location = new Point(14, 278);
      this.chkCoBorrower7.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower7.Name = "chkCoBorrower7";
      this.chkCoBorrower7.Size = new Size(231, 17);
      this.chkCoBorrower7.TabIndex = 22;
      this.chkCoBorrower7.Text = "Confirmed Mortgage Delinquency Incorrect ";
      this.chkCoBorrower7.UseVisualStyleBackColor = true;
      this.chkCoBorrower7.Enter += new EventHandler(this.control_Enter);
      this.label8.Location = new Point(34, 257);
      this.label8.Margin = new Padding(2, 0, 2, 0);
      this.label8.Name = "label8";
      this.label8.Size = new Size(154, 18);
      this.label8.TabIndex = 21;
      this.label8.Text = "(\"Confirmed CR BK EC\")";
      this.chkCoBorrower6.AutoSize = true;
      this.chkCoBorrower6.Location = new Point(14, 237);
      this.chkCoBorrower6.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower6.Name = "chkCoBorrower6";
      this.chkCoBorrower6.Size = new Size(246, 17);
      this.chkCoBorrower6.TabIndex = 20;
      this.chkCoBorrower6.Text = "Bankruptcy Due to Extenuating Circumstances";
      this.chkCoBorrower6.UseVisualStyleBackColor = true;
      this.chkCoBorrower6.Enter += new EventHandler(this.control_Enter);
      this.label9.Location = new Point(34, 171);
      this.label9.Margin = new Padding(2, 0, 2, 0);
      this.label9.Name = "label9";
      this.label9.Size = new Size(154, 18);
      this.label9.TabIndex = 19;
      this.label9.Text = "(\"Confirmed CR PFS\")";
      this.chkCoBorrower4.AutoSize = true;
      this.chkCoBorrower4.Location = new Point(14, 155);
      this.chkCoBorrower4.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower4.Name = "chkCoBorrower4";
      this.chkCoBorrower4.Size = new Size(192, 17);
      this.chkCoBorrower4.TabIndex = 18;
      this.chkCoBorrower4.Text = "Preforeclosure Sales or Short Sales";
      this.chkCoBorrower4.UseVisualStyleBackColor = true;
      this.chkCoBorrower4.Enter += new EventHandler(this.control_Enter);
      this.label10.Location = new Point(34, 134);
      this.label10.Margin = new Padding(2, 0, 2, 0);
      this.label10.Name = "label10";
      this.label10.Size = new Size(154, 18);
      this.label10.TabIndex = 17;
      this.label10.Text = "(\"Confirmed CR DIL\")";
      this.chkCoBorrower3.AutoSize = true;
      this.chkCoBorrower3.Location = new Point(14, 115);
      this.chkCoBorrower3.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower3.Name = "chkCoBorrower3";
      this.chkCoBorrower3.Size = new Size(156, 17);
      this.chkCoBorrower3.TabIndex = 16;
      this.chkCoBorrower3.Text = "Deed-in-Lieu of Foreclosure";
      this.chkCoBorrower3.UseVisualStyleBackColor = true;
      this.chkCoBorrower3.Enter += new EventHandler(this.control_Enter);
      this.label11.Location = new Point(34, 97);
      this.label11.Margin = new Padding(2, 0, 2, 0);
      this.label11.Name = "label11";
      this.label11.Size = new Size(154, 18);
      this.label11.TabIndex = 15;
      this.label11.Text = "(\"Confirmed CR FC EC\")";
      this.chkCoBorrower2.AutoSize = true;
      this.chkCoBorrower2.Location = new Point(14, 83);
      this.chkCoBorrower2.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower2.Name = "chkCoBorrower2";
      this.chkCoBorrower2.Size = new Size(243, 17);
      this.chkCoBorrower2.TabIndex = 14;
      this.chkCoBorrower2.Text = "Foreclosure due to extenuating circumstances";
      this.chkCoBorrower2.UseVisualStyleBackColor = true;
      this.chkCoBorrower2.Enter += new EventHandler(this.control_Enter);
      this.label12.Location = new Point(34, 60);
      this.label12.Margin = new Padding(2, 0, 2, 0);
      this.label12.Name = "label12";
      this.label12.Size = new Size(154, 18);
      this.label12.TabIndex = 13;
      this.label12.Text = "(\"Confirmed CR FC Incorrect\")";
      this.chkCoBorrower1.AutoSize = true;
      this.chkCoBorrower1.Location = new Point(14, 42);
      this.chkCoBorrower1.Margin = new Padding(2, 3, 2, 3);
      this.chkCoBorrower1.Name = "chkCoBorrower1";
      this.chkCoBorrower1.Size = new Size(189, 17);
      this.chkCoBorrower1.TabIndex = 12;
      this.chkCoBorrower1.Text = "Inaccurate Foreclosure information";
      this.chkCoBorrower1.UseVisualStyleBackColor = true;
      this.chkCoBorrower1.Enter += new EventHandler(this.control_Enter);
      this.label13.Location = new Point(6, 36);
      this.label13.Margin = new Padding(2, 0, 2, 0);
      this.label13.Name = "label13";
      this.label13.Size = new Size(654, 47);
      this.label13.TabIndex = 11;
      this.label13.Text = componentResourceManager.GetString("label13.Text");
      this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label14.Location = new Point(6, 13);
      this.label14.Margin = new Padding(2, 0, 2, 0);
      this.label14.Name = "label14";
      this.label14.Size = new Size(378, 23);
      this.label14.TabIndex = 12;
      this.label14.Text = "Bankruptcy, Foreclosure and Mortgage Delinquency Messages for DU";
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.Location = new Point(462, 462);
      this.btnSave.Margin = new Padding(2, 3, 2, 3);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(74, 24);
      this.btnSave.TabIndex = 13;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(547, 462);
      this.btnCancel.Margin = new Padding(2, 3, 2, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(74, 24);
      this.btnCancel.TabIndex = 14;
      this.btnCancel.Text = "&Cancel";
      this.pboxDownArrow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(336, 582);
      this.pboxDownArrow.Margin = new Padding(2);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(18, 16);
      this.pboxDownArrow.TabIndex = 70;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(298, 583);
      this.pboxAsterisk.Margin = new Padding(2);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(26, 11);
      this.pboxAsterisk.TabIndex = 69;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(668, 493);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.borrowergroupContainer);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label14);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(2, 3, 2, 3);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddBankruptcyDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Bankruptcy,Foreclosure and Mortgage Delinquency Messages for DU";
      this.borrowergroupContainer.ResumeLayout(false);
      this.borrowergroupContainer.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
    }
  }
}
