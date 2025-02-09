// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.OtherExpenseDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class OtherExpenseDialog : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Button cancelBtn;
    private System.Windows.Forms.Button okBtn;
    private System.Windows.Forms.CheckBox exFloodChk;
    private System.Windows.Forms.TextBox floodTxt;
    private System.Windows.Forms.CheckBox ex1006Chk;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox user1006Txt;
    private System.Windows.Forms.CheckBox ex1007Chk;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox user1007Txt;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox otherTxt;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox totalTxt;
    private IContainer components;
    private System.Windows.Forms.CheckBox ex1008Chk;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox user1008Txt;
    private ToolTip fieldToolTip;
    private PictureBox pboxAsterisk;
    private LoanData loan;
    private PictureBox pboxDownArrow;
    private System.Windows.Forms.TextBox user1008DescTxt;
    private System.Windows.Forms.TextBox user1007DescTxt;
    private System.Windows.Forms.TextBox user1006DescTxt;
    private System.Windows.Forms.CheckBox chkTax;
    private System.Windows.Forms.CheckBox exUSDAChk;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox usdaText;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.GroupBox groupBoxAll;
    private System.Windows.Forms.Panel panelLine1006;
    private System.Windows.Forms.Panel panelOverrideFlag;
    private PopupBusinessRules popupRules;
    private System.Windows.Forms.Panel panelOthers;
    private System.Windows.Forms.Panel panelLineTotal;
    private System.Windows.Forms.Panel panelLineOther;
    private System.Windows.Forms.Panel panelLine1010;
    private System.Windows.Forms.Panel panelInfo;
    private System.Windows.Forms.Label labelInfo;
    private PictureBox pb1628;
    private string calculatedField = "";
    private PictureBox pb661;
    private PictureBox pb660;
    private Sessions.Session session;
    private FeeManagementPersonaInfo feeManagementPermission;
    private FeeManagementSetting feeManagementSetting;
    private double otherExpense;

    internal OtherExpenseDialog(
      LoanData loan,
      string calculatedField,
      Sessions.Session session,
      FeeManagementSetting feeManagementSetting,
      FeeManagementPersonaInfo feeManagementPermission)
    {
      this.loan = loan;
      this.calculatedField = calculatedField;
      this.session = session;
      this.feeManagementPermission = feeManagementPermission;
      this.feeManagementSetting = feeManagementSetting;
      this.InitializeComponent();
      if (this.calculatedField == "URLA.X144")
      {
        this.Text = "Proposed Supplemental Property Insurance";
        this.labelInfo.Text = "The description must have the word \"insurance\" to be included in the Total Insurance.";
      }
      else
      {
        this.Text = "Proposed Other Expense";
        this.labelInfo.Text = "Entries not designated as Tax or Insurance will be included in Total. Enter Other for non-escrowed expense other than Association/Project Dues.";
      }
      this.panelInfo.Visible = true;
      this.chkTax.Checked = this.loan.GetField("USEGFETAX") == "Y";
      this.floodTxt.Text = this.loan.GetField("235");
      this.user1006Txt.Text = this.loan.GetField("1630");
      this.user1007Txt.Text = this.loan.GetField("253");
      this.user1008Txt.Text = this.loan.GetField("254");
      this.usdaText.Text = this.loan.GetField("NEWHUD.X1707");
      this.totalTxt.Text = this.loan.GetField(this.calculatedField == "234" ? "234" : "URLA.X144");
      this.user1006DescTxt.Text = this.loan.GetField("1628");
      this.user1007DescTxt.Text = this.loan.GetField("660");
      this.user1008DescTxt.Text = this.loan.GetField("661");
      this.exFloodChk.Checked = this.loan.GetField("1801") == "Y";
      this.ex1006Chk.Checked = this.loan.GetField("1802") == "Y";
      this.ex1007Chk.Checked = this.loan.GetField("1803") == "Y";
      this.ex1008Chk.Checked = this.loan.GetField("1804") == "Y";
      this.exUSDAChk.Checked = this.loan.GetField("3357") == "Y";
      this.otherTxt.Text = this.loan.GetField("1799");
      this.totalTxt.Text = this.calculateTotal().ToString("N2");
      this.totalTxt.Tag = (object) this.calculatedField;
      if (this.loan != null)
      {
        if (this.loan.Use2020URLA)
        {
          this.floodTxt.ReadOnly = this.usdaText.ReadOnly = this.user1006DescTxt.ReadOnly = this.user1006Txt.ReadOnly = this.user1007DescTxt.ReadOnly = this.user1007Txt.ReadOnly = this.user1008DescTxt.ReadOnly = this.user1008Txt.ReadOnly = false;
          this.panelOverrideFlag.Visible = false;
          this.panelLine1006.Visible = this.calculatedField != "234";
          this.panelLine1010.Visible = this.calculatedField == "234";
          this.panelLineOther.Visible = this.calculatedField == "234";
          this.groupBoxAll.Height = (this.calculatedField != "234" ? this.panelLine1006.Height : 0) + this.panelOthers.Height + (this.calculatedField == "234" ? this.panelLine1010.Height : 0) + (this.calculatedField == "234" ? this.panelLineOther.Height : 0) + this.panelLineTotal.Height + 20;
        }
        else
          this.floodTxt.ReadOnly = this.usdaText.ReadOnly = this.user1006DescTxt.ReadOnly = this.user1006Txt.ReadOnly = this.user1007DescTxt.ReadOnly = this.user1007Txt.ReadOnly = this.user1008DescTxt.ReadOnly = this.user1008Txt.ReadOnly = true;
        this.panelInfo.Top = this.groupBoxAll.Top + this.groupBoxAll.Height;
        this.Height = this.groupBoxAll.Height + this.panelInfo.Height + 70;
      }
      ResourceManager resources = new ResourceManager(typeof (OtherExpenseDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (System.Drawing.Image) resources.GetObject("pboxAsterisk.Image"), (System.Drawing.Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      System.Windows.Forms.Control[] controlArray = new System.Windows.Forms.Control[16]
      {
        (System.Windows.Forms.Control) this.chkTax,
        (System.Windows.Forms.Control) this.floodTxt,
        (System.Windows.Forms.Control) this.exFloodChk,
        (System.Windows.Forms.Control) this.user1006DescTxt,
        (System.Windows.Forms.Control) this.user1006Txt,
        (System.Windows.Forms.Control) this.ex1006Chk,
        (System.Windows.Forms.Control) this.user1007DescTxt,
        (System.Windows.Forms.Control) this.user1007Txt,
        (System.Windows.Forms.Control) this.ex1007Chk,
        (System.Windows.Forms.Control) this.user1008DescTxt,
        (System.Windows.Forms.Control) this.user1008Txt,
        (System.Windows.Forms.Control) this.ex1008Chk,
        (System.Windows.Forms.Control) this.usdaText,
        (System.Windows.Forms.Control) this.exUSDAChk,
        (System.Windows.Forms.Control) this.otherTxt,
        (System.Windows.Forms.Control) this.totalTxt
      };
      for (int index = 0; index < controlArray.Length; ++index)
      {
        this.fieldToolTip.SetToolTip(controlArray[index], controlArray[index].Tag.ToString());
        this.popupRules.SetBusinessRules((object) controlArray[index], controlArray[index].Tag.ToString());
        if (controlArray[index] is System.Windows.Forms.TextBox && !((TextBoxBase) controlArray[index]).ReadOnly)
        {
          switch (controlArray[index].Name)
          {
            case nameof (user1006DescTxt):
              this.pb1628.Visible = true;
              continue;
            case nameof (user1007DescTxt):
              this.pb660.Visible = true;
              continue;
            case nameof (user1008DescTxt):
              this.pb661.Visible = true;
              continue;
            default:
              continue;
          }
        }
      }
      if (this.feeManagementSetting == null || !this.feeManagementSetting.CompanyOptIn)
      {
        this.pb1628.Visible = this.pb660.Visible = this.pb661.Visible = false;
      }
      else
      {
        bool flag = this.feeManagementPermission == null || this.feeManagementPermission.IsSectionEditable(FeeSectionEnum.For1000);
        if (this.feeManagementPermission != null && !this.user1006DescTxt.ReadOnly)
          this.user1006DescTxt.ReadOnly = !flag;
        if (this.feeManagementPermission != null && !this.user1007DescTxt.ReadOnly)
          this.user1007DescTxt.ReadOnly = !flag;
        if (this.feeManagementPermission == null || this.user1008DescTxt.ReadOnly)
          return;
        this.user1008DescTxt.ReadOnly = !flag;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OtherExpenseDialog));
      this.user1006DescTxt = new System.Windows.Forms.TextBox();
      this.exUSDAChk = new System.Windows.Forms.CheckBox();
      this.user1006Txt = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.usdaText = new System.Windows.Forms.TextBox();
      this.ex1006Chk = new System.Windows.Forms.CheckBox();
      this.user1008DescTxt = new System.Windows.Forms.TextBox();
      this.user1007Txt = new System.Windows.Forms.TextBox();
      this.user1007DescTxt = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.ex1007Chk = new System.Windows.Forms.CheckBox();
      this.otherTxt = new System.Windows.Forms.TextBox();
      this.ex1008Chk = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.totalTxt = new System.Windows.Forms.TextBox();
      this.user1008Txt = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.chkTax = new System.Windows.Forms.CheckBox();
      this.exFloodChk = new System.Windows.Forms.CheckBox();
      this.floodTxt = new System.Windows.Forms.TextBox();
      this.cancelBtn = new System.Windows.Forms.Button();
      this.okBtn = new System.Windows.Forms.Button();
      this.fieldToolTip = new ToolTip(this.components);
      this.pboxAsterisk = new PictureBox();
      this.pboxDownArrow = new PictureBox();
      this.groupBoxAll = new System.Windows.Forms.GroupBox();
      this.panelLineTotal = new System.Windows.Forms.Panel();
      this.panelLineOther = new System.Windows.Forms.Panel();
      this.panelLine1010 = new System.Windows.Forms.Panel();
      this.panelOthers = new System.Windows.Forms.Panel();
      this.pb661 = new PictureBox();
      this.pb660 = new PictureBox();
      this.pb1628 = new PictureBox();
      this.panelLine1006 = new System.Windows.Forms.Panel();
      this.panelOverrideFlag = new System.Windows.Forms.Panel();
      this.panelInfo = new System.Windows.Forms.Panel();
      this.labelInfo = new System.Windows.Forms.Label();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.groupBoxAll.SuspendLayout();
      this.panelLineTotal.SuspendLayout();
      this.panelLineOther.SuspendLayout();
      this.panelLine1010.SuspendLayout();
      this.panelOthers.SuspendLayout();
      ((ISupportInitialize) this.pb661).BeginInit();
      ((ISupportInitialize) this.pb660).BeginInit();
      ((ISupportInitialize) this.pb1628).BeginInit();
      this.panelLine1006.SuspendLayout();
      this.panelOverrideFlag.SuspendLayout();
      this.panelInfo.SuspendLayout();
      this.SuspendLayout();
      this.user1006DescTxt.Location = new Point(43, 0);
      this.user1006DescTxt.Name = "user1006DescTxt";
      this.user1006DescTxt.Size = new Size(151, 20);
      this.user1006DescTxt.TabIndex = 4;
      this.user1006DescTxt.TabStop = false;
      this.user1006DescTxt.Tag = (object) "1628";
      this.user1006DescTxt.Enter += new EventHandler(this.field_Entered);
      this.user1006DescTxt.Leave += new EventHandler(this.leave);
      this.exUSDAChk.Location = new Point(315, 0);
      this.exUSDAChk.Name = "exUSDAChk";
      this.exUSDAChk.Size = new Size(72, 20);
      this.exUSDAChk.TabIndex = 14;
      this.exUSDAChk.Tag = (object) "3357";
      this.exUSDAChk.Text = "Excluded";
      this.exUSDAChk.Click += new EventHandler(this.excluded_Click);
      this.exUSDAChk.Enter += new EventHandler(this.field_Entered);
      this.user1006Txt.Location = new Point(228, 0);
      this.user1006Txt.MaxLength = 10;
      this.user1006Txt.Name = "user1006Txt";
      this.user1006Txt.Size = new Size(76, 20);
      this.user1006Txt.TabIndex = 5;
      this.user1006Txt.TabStop = false;
      this.user1006Txt.Tag = (object) "1630";
      this.user1006Txt.TextAlign = HorizontalAlignment.Right;
      this.user1006Txt.Enter += new EventHandler(this.field_Entered);
      this.user1006Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.user1006Txt.Leave += new EventHandler(this.leave);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(11, 2);
      this.label8.Name = "label8";
      this.label8.Size = new Size(124, 13);
      this.label8.TabIndex = 75;
      this.label8.Text = "1010  USDA Annual Fee";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(11, 4);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "1007";
      this.usdaText.Location = new Point(228, 0);
      this.usdaText.MaxLength = 10;
      this.usdaText.Name = "usdaText";
      this.usdaText.Size = new Size(76, 20);
      this.usdaText.TabIndex = 13;
      this.usdaText.TabStop = false;
      this.usdaText.Tag = (object) "NEWHUD.X1707";
      this.usdaText.TextAlign = HorizontalAlignment.Right;
      this.usdaText.Enter += new EventHandler(this.field_Entered);
      this.usdaText.KeyUp += new KeyEventHandler(this.keyup);
      this.usdaText.Leave += new EventHandler(this.leave);
      this.ex1006Chk.Location = new Point(314, 1);
      this.ex1006Chk.Name = "ex1006Chk";
      this.ex1006Chk.Size = new Size(72, 20);
      this.ex1006Chk.TabIndex = 6;
      this.ex1006Chk.Tag = (object) "1802";
      this.ex1006Chk.Text = "Excluded";
      this.ex1006Chk.Click += new EventHandler(this.excluded_Click);
      this.ex1006Chk.Enter += new EventHandler(this.field_Entered);
      this.user1008DescTxt.Location = new Point(43, 51);
      this.user1008DescTxt.Name = "user1008DescTxt";
      this.user1008DescTxt.Size = new Size(151, 20);
      this.user1008DescTxt.TabIndex = 10;
      this.user1008DescTxt.TabStop = false;
      this.user1008DescTxt.Tag = (object) "661";
      this.user1008DescTxt.Enter += new EventHandler(this.field_Entered);
      this.user1008DescTxt.Leave += new EventHandler(this.leave);
      this.user1007Txt.Location = new Point(228, 25);
      this.user1007Txt.MaxLength = 10;
      this.user1007Txt.Name = "user1007Txt";
      this.user1007Txt.Size = new Size(76, 20);
      this.user1007Txt.TabIndex = 8;
      this.user1007Txt.TabStop = false;
      this.user1007Txt.Tag = (object) "253";
      this.user1007Txt.TextAlign = HorizontalAlignment.Right;
      this.user1007Txt.Enter += new EventHandler(this.field_Entered);
      this.user1007Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.user1007Txt.Leave += new EventHandler(this.leave);
      this.user1007DescTxt.Location = new Point(43, 25);
      this.user1007DescTxt.Name = "user1007DescTxt";
      this.user1007DescTxt.Size = new Size(151, 20);
      this.user1007DescTxt.TabIndex = 7;
      this.user1007DescTxt.TabStop = false;
      this.user1007DescTxt.Tag = (object) "660";
      this.user1007DescTxt.Enter += new EventHandler(this.field_Entered);
      this.user1007DescTxt.Leave += new EventHandler(this.leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 29);
      this.label4.Name = "label4";
      this.label4.Size = new Size(31, 13);
      this.label4.TabIndex = 13;
      this.label4.Text = "1008";
      this.ex1007Chk.Location = new Point(314, 26);
      this.ex1007Chk.Name = "ex1007Chk";
      this.ex1007Chk.Size = new Size(72, 20);
      this.ex1007Chk.TabIndex = 9;
      this.ex1007Chk.Tag = (object) "1803";
      this.ex1007Chk.Text = "Excluded";
      this.ex1007Chk.Click += new EventHandler(this.excluded_Click);
      this.ex1007Chk.Enter += new EventHandler(this.field_Entered);
      this.otherTxt.Location = new Point(228, 0);
      this.otherTxt.MaxLength = 10;
      this.otherTxt.Name = "otherTxt";
      this.otherTxt.Size = new Size(76, 20);
      this.otherTxt.TabIndex = 15;
      this.otherTxt.Tag = (object) "1799";
      this.otherTxt.TextAlign = HorizontalAlignment.Right;
      this.otherTxt.Enter += new EventHandler(this.field_Entered);
      this.otherTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.otherTxt.Leave += new EventHandler(this.leave);
      this.ex1008Chk.Location = new Point(315, 51);
      this.ex1008Chk.Name = "ex1008Chk";
      this.ex1008Chk.Size = new Size(72, 20);
      this.ex1008Chk.TabIndex = 12;
      this.ex1008Chk.Tag = (object) "1804";
      this.ex1008Chk.Text = "Excluded";
      this.ex1008Chk.Click += new EventHandler(this.excluded_Click);
      this.ex1008Chk.Enter += new EventHandler(this.field_Entered);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(11, 4);
      this.label5.Name = "label5";
      this.label5.Size = new Size(33, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Other";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(11, 54);
      this.label7.Name = "label7";
      this.label7.Size = new Size(31, 13);
      this.label7.TabIndex = 22;
      this.label7.Text = "1009";
      this.totalTxt.Location = new Point(228, 0);
      this.totalTxt.MaxLength = 10;
      this.totalTxt.Name = "totalTxt";
      this.totalTxt.ReadOnly = true;
      this.totalTxt.Size = new Size(76, 20);
      this.totalTxt.TabIndex = 16;
      this.totalTxt.TabStop = false;
      this.totalTxt.TextAlign = HorizontalAlignment.Right;
      this.totalTxt.Enter += new EventHandler(this.field_Entered);
      this.user1008Txt.Location = new Point(228, 51);
      this.user1008Txt.MaxLength = 10;
      this.user1008Txt.Name = "user1008Txt";
      this.user1008Txt.Size = new Size(76, 20);
      this.user1008Txt.TabIndex = 11;
      this.user1008Txt.TabStop = false;
      this.user1008Txt.Tag = (object) "254";
      this.user1008Txt.TextAlign = HorizontalAlignment.Right;
      this.user1008Txt.Enter += new EventHandler(this.field_Entered);
      this.user1008Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.user1008Txt.Leave += new EventHandler(this.leave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(11, 4);
      this.label6.Name = "label6";
      this.label6.Size = new Size(31, 13);
      this.label6.TabIndex = 19;
      this.label6.Text = "Total";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(11, 4);
      this.label9.Name = "label9";
      this.label9.Size = new Size(113, 13);
      this.label9.TabIndex = 76;
      this.label9.Text = "1006  Flood Insurance";
      this.chkTax.Location = new Point(17, 3);
      this.chkTax.Name = "chkTax";
      this.chkTax.Size = new Size(232, 16);
      this.chkTax.TabIndex = 1;
      this.chkTax.Tag = (object) "USEGFETAX";
      this.chkTax.Text = "Override with Reserves from Itemization";
      this.chkTax.Click += new EventHandler(this.excluded_Click);
      this.chkTax.Enter += new EventHandler(this.field_Entered);
      this.exFloodChk.Location = new Point(314, 1);
      this.exFloodChk.Name = "exFloodChk";
      this.exFloodChk.Size = new Size(72, 20);
      this.exFloodChk.TabIndex = 3;
      this.exFloodChk.Tag = (object) "1801";
      this.exFloodChk.Text = "Excluded";
      this.exFloodChk.Click += new EventHandler(this.excluded_Click);
      this.exFloodChk.Enter += new EventHandler(this.field_Entered);
      this.floodTxt.Location = new Point(228, 0);
      this.floodTxt.MaxLength = 10;
      this.floodTxt.Name = "floodTxt";
      this.floodTxt.Size = new Size(76, 20);
      this.floodTxt.TabIndex = 2;
      this.floodTxt.TabStop = false;
      this.floodTxt.Tag = (object) "235";
      this.floodTxt.TextAlign = HorizontalAlignment.Right;
      this.floodTxt.Enter += new EventHandler(this.field_Entered);
      this.floodTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.floodTxt.Leave += new EventHandler(this.leave);
      this.cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(437, 44);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 18;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(437, 16);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 17;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.pboxAsterisk.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(429, 104);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 18;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(429, 80);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 68;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.groupBoxAll.Controls.Add((System.Windows.Forms.Control) this.panelLineTotal);
      this.groupBoxAll.Controls.Add((System.Windows.Forms.Control) this.panelLineOther);
      this.groupBoxAll.Controls.Add((System.Windows.Forms.Control) this.panelLine1010);
      this.groupBoxAll.Controls.Add((System.Windows.Forms.Control) this.panelOthers);
      this.groupBoxAll.Controls.Add((System.Windows.Forms.Control) this.panelLine1006);
      this.groupBoxAll.Controls.Add((System.Windows.Forms.Control) this.panelOverrideFlag);
      this.groupBoxAll.Location = new Point(12, 12);
      this.groupBoxAll.Name = "groupBoxAll";
      this.groupBoxAll.Size = new Size(411, 222);
      this.groupBoxAll.TabIndex = 69;
      this.groupBoxAll.TabStop = false;
      this.panelLineTotal.Controls.Add((System.Windows.Forms.Control) this.totalTxt);
      this.panelLineTotal.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.panelLineTotal.Dock = DockStyle.Top;
      this.panelLineTotal.Location = new Point(3, 191);
      this.panelLineTotal.Name = "panelLineTotal";
      this.panelLineTotal.Size = new Size(405, 25);
      this.panelLineTotal.TabIndex = 75;
      this.panelLineOther.Controls.Add((System.Windows.Forms.Control) this.otherTxt);
      this.panelLineOther.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.panelLineOther.Dock = DockStyle.Top;
      this.panelLineOther.Location = new Point(3, 166);
      this.panelLineOther.Name = "panelLineOther";
      this.panelLineOther.Size = new Size(405, 25);
      this.panelLineOther.TabIndex = 74;
      this.panelLine1010.Controls.Add((System.Windows.Forms.Control) this.usdaText);
      this.panelLine1010.Controls.Add((System.Windows.Forms.Control) this.exUSDAChk);
      this.panelLine1010.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.panelLine1010.Dock = DockStyle.Top;
      this.panelLine1010.Location = new Point(3, 141);
      this.panelLine1010.Name = "panelLine1010";
      this.panelLine1010.Size = new Size(405, 25);
      this.panelLine1010.TabIndex = 73;
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.pb661);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.pb660);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.pb1628);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.user1006DescTxt);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.user1006Txt);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.user1007DescTxt);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.user1008Txt);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.ex1007Chk);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.user1007Txt);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.user1008DescTxt);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.ex1008Chk);
      this.panelOthers.Controls.Add((System.Windows.Forms.Control) this.ex1006Chk);
      this.panelOthers.Dock = DockStyle.Top;
      this.panelOthers.Location = new Point(3, 66);
      this.panelOthers.Name = "panelOthers";
      this.panelOthers.Size = new Size(405, 75);
      this.panelOthers.TabIndex = 2;
      this.pb661.Image = (System.Drawing.Image) componentResourceManager.GetObject("pb661.Image");
      this.pb661.Location = new Point(196, 51);
      this.pb661.Name = "pb661";
      this.pb661.Size = new Size(17, 17);
      this.pb661.TabIndex = 71;
      this.pb661.TabStop = false;
      this.pb661.Visible = false;
      this.pb661.Click += new EventHandler(this.pb661_Click);
      this.pb660.Image = (System.Drawing.Image) componentResourceManager.GetObject("pb660.Image");
      this.pb660.Location = new Point(195, 25);
      this.pb660.Name = "pb660";
      this.pb660.Size = new Size(17, 17);
      this.pb660.TabIndex = 70;
      this.pb660.TabStop = false;
      this.pb660.Visible = false;
      this.pb660.Click += new EventHandler(this.pb660_Click);
      this.pb1628.Image = (System.Drawing.Image) componentResourceManager.GetObject("pb1628.Image");
      this.pb1628.Location = new Point(196, 1);
      this.pb1628.Name = "pb1628";
      this.pb1628.Size = new Size(17, 17);
      this.pb1628.TabIndex = 69;
      this.pb1628.TabStop = false;
      this.pb1628.Visible = false;
      this.pb1628.Click += new EventHandler(this.pb1628_Click);
      this.panelLine1006.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.panelLine1006.Controls.Add((System.Windows.Forms.Control) this.floodTxt);
      this.panelLine1006.Controls.Add((System.Windows.Forms.Control) this.exFloodChk);
      this.panelLine1006.Dock = DockStyle.Top;
      this.panelLine1006.Location = new Point(3, 41);
      this.panelLine1006.Name = "panelLine1006";
      this.panelLine1006.Size = new Size(405, 25);
      this.panelLine1006.TabIndex = 1;
      this.panelOverrideFlag.Controls.Add((System.Windows.Forms.Control) this.chkTax);
      this.panelOverrideFlag.Dock = DockStyle.Top;
      this.panelOverrideFlag.Location = new Point(3, 16);
      this.panelOverrideFlag.Name = "panelOverrideFlag";
      this.panelOverrideFlag.Size = new Size(405, 25);
      this.panelOverrideFlag.TabIndex = 0;
      this.panelInfo.Controls.Add((System.Windows.Forms.Control) this.labelInfo);
      this.panelInfo.Location = new Point(15, 235);
      this.panelInfo.Name = "panelInfo";
      this.panelInfo.Size = new Size(493, 40);
      this.panelInfo.TabIndex = 70;
      this.labelInfo.Location = new Point(6, 6);
      this.labelInfo.Name = "labelInfo";
      this.labelInfo.Size = new Size(436, 31);
      this.labelInfo.TabIndex = 76;
      this.labelInfo.Text = "(Description)";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(520, 292);
      this.Controls.Add((System.Windows.Forms.Control) this.panelInfo);
      this.Controls.Add((System.Windows.Forms.Control) this.groupBoxAll);
      this.Controls.Add((System.Windows.Forms.Control) this.pboxDownArrow);
      this.Controls.Add((System.Windows.Forms.Control) this.pboxAsterisk);
      this.Controls.Add((System.Windows.Forms.Control) this.cancelBtn);
      this.Controls.Add((System.Windows.Forms.Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OtherExpenseDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Other Housing Expense";
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.groupBoxAll.ResumeLayout(false);
      this.panelLineTotal.ResumeLayout(false);
      this.panelLineTotal.PerformLayout();
      this.panelLineOther.ResumeLayout(false);
      this.panelLineOther.PerformLayout();
      this.panelLine1010.ResumeLayout(false);
      this.panelLine1010.PerformLayout();
      this.panelOthers.ResumeLayout(false);
      this.panelOthers.PerformLayout();
      ((ISupportInitialize) this.pb661).EndInit();
      ((ISupportInitialize) this.pb660).EndInit();
      ((ISupportInitialize) this.pb1628).EndInit();
      this.panelLine1006.ResumeLayout(false);
      this.panelLine1006.PerformLayout();
      this.panelOverrideFlag.ResumeLayout(false);
      this.panelInfo.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    internal double OtherExpense => this.otherExpense;

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender != null && sender is System.Windows.Forms.TextBox)
      {
        System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
        if (!textBox.ReadOnly)
        {
          double num = Utils.ParseDouble((object) textBox.Text);
          if (num != 0.0)
            textBox.Text = num.ToString("N2");
          textBox.BackColor = SystemColors.Window;
        }
      }
      this.totalTxt.Text = this.calculateTotal().ToString("N2");
    }

    private void excluded_Click(object sender, EventArgs e)
    {
      this.totalTxt.Text = this.calculateTotal().ToString("N2");
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.loan.SetCurrentField("USEGFETAX", this.chkTax.Checked ? "Y" : "");
      if (this.loan.Use2020URLA)
      {
        this.loan.SetCurrentField("235", this.floodTxt.Text);
        this.loan.SetCurrentField("1628", this.user1006DescTxt.Text);
        this.loan.SetCurrentField("1630", this.user1006Txt.Text);
        this.loan.SetCurrentField("660", this.user1007DescTxt.Text);
        this.loan.SetCurrentField("253", this.user1007Txt.Text);
        this.loan.SetCurrentField("661", this.user1008DescTxt.Text);
        this.loan.SetCurrentField("254", this.user1008Txt.Text);
      }
      this.loan.SetCurrentField("1799", this.otherTxt.Text);
      this.loan.SetCurrentField("1801", this.exFloodChk.Checked ? "Y" : "");
      this.loan.SetCurrentField("1802", this.ex1006Chk.Checked ? "Y" : "");
      this.loan.SetCurrentField("1803", this.ex1007Chk.Checked ? "Y" : "");
      this.loan.SetCurrentField("1804", this.ex1008Chk.Checked ? "Y" : "");
      this.loan.SetCurrentField("3357", this.exUSDAChk.Checked ? "Y" : "");
      this.otherExpense = Utils.ParseDouble((object) this.totalTxt.Text);
    }

    private double calculateTotal()
    {
      double total = 0.0;
      bool flag = this.chkTax.Checked;
      if (!this.exFloodChk.Checked && (!this.loan.Use2020URLA || this.calculatedField != "234"))
        total += Utils.ParseDouble((object) this.floodTxt.Text.Trim());
      if (!this.ex1006Chk.Checked && (!flag || !this.loan.Use2020URLA && !SharedCalculations.IsTaxFee(this.loan.GetField("1628")) || this.loan.Use2020URLA && (this.calculatedField == "URLA.X144" && SharedCalculations.IsInsuranceFee(this.user1006DescTxt.Text) || this.calculatedField == "234" && !SharedCalculations.IsInsuranceOrTax(this.user1006DescTxt.Text))))
        total += Utils.ParseDouble((object) this.user1006Txt.Text.Trim());
      if (!this.ex1007Chk.Checked && (!flag || !this.loan.Use2020URLA && !SharedCalculations.IsTaxFee(this.loan.GetField("660")) || this.loan.Use2020URLA && (this.calculatedField == "URLA.X144" && SharedCalculations.IsInsuranceFee(this.user1007DescTxt.Text) || this.calculatedField == "234" && !SharedCalculations.IsInsuranceOrTax(this.user1007DescTxt.Text))))
        total += Utils.ParseDouble((object) this.user1007Txt.Text.Trim());
      if (!this.ex1008Chk.Checked && (!flag || !this.loan.Use2020URLA && !SharedCalculations.IsTaxFee(this.loan.GetField("661")) || this.loan.Use2020URLA && (this.calculatedField == "URLA.X144" && SharedCalculations.IsInsuranceFee(this.user1008DescTxt.Text) || this.calculatedField == "234" && !SharedCalculations.IsInsuranceOrTax(this.user1008DescTxt.Text))))
        total += Utils.ParseDouble((object) this.user1008Txt.Text.Trim());
      if (this.calculatedField == "234")
      {
        if (!this.exUSDAChk.Checked)
          total += Utils.ParseDouble((object) this.usdaText.Text.Trim());
        total += Utils.ParseDouble((object) this.otherTxt.Text.Trim());
      }
      return total;
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void field_Entered(object sender, EventArgs e)
    {
      System.Windows.Forms.Control control = (System.Windows.Forms.Control) sender;
      if (control == null || control.Tag == null || this.loan == null || this.loan.IsTemplate)
        return;
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(control.Tag.ToString());
      if (!(control is System.Windows.Forms.TextBox) || ((TextBoxBase) control).ReadOnly)
        return;
      control.BackColor = Color.LightGoldenrodYellow;
    }

    private void displayCustomCodeRuntimeError(string eventName, Exception ex)
    {
      ErrorDialog.Display("An error has occurred in the '" + eventName + " Error : " + ex.Message, ex);
    }

    private void populatepicklist(System.Windows.Forms.TextBox textBox)
    {
      string[] feeNames = this.session.LoanDataMgr.SystemConfiguration.FeeManagementList.GetFeeNames(FeeSectionEnum.For1000);
      DropdownOption[] options = new DropdownOption[feeNames.Length];
      for (int index = 0; index < feeNames.Length; ++index)
        options[index] = new DropdownOption(feeNames[index]);
      int w = Math.Max(textBox.Width, 260);
      Point position = Cursor.Position;
      int x1 = position.X;
      position = Cursor.Position;
      int y1 = position.Y;
      int x2 = x1 - w;
      int y2 = y1 + 10;
      using (FieldRuleDropdownDialog ruleDropdownDialog = new FieldRuleDropdownDialog(options, "test"))
      {
        if (ruleDropdownDialog.Height + y2 > Screen.PrimaryScreen.Bounds.Height)
          y2 = y1 - ruleDropdownDialog.Height - 40;
        ruleDropdownDialog.SetListBoxWidth(w);
        ruleDropdownDialog.Location = new Point(x2, y2);
        if (ruleDropdownDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        try
        {
          textBox.Text = ruleDropdownDialog.SelectedItem.Text;
        }
        catch (Exception ex)
        {
          this.displayCustomCodeRuntimeError(nameof (populatepicklist), ex);
        }
      }
    }

    private void pb1628_Click(object sender, EventArgs e)
    {
      this.populatepicklist(this.user1006DescTxt);
    }

    private void pb661_Click(object sender, EventArgs e)
    {
      this.populatepicklist(this.user1008DescTxt);
    }

    private void pb660_Click(object sender, EventArgs e)
    {
      this.populatepicklist(this.user1007DescTxt);
    }
  }
}
