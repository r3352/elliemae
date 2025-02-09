// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.RecordingFeeDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class RecordingFeeDialog : Form
  {
    private Button buttonOK;
    private Button buttonCancel;
    private TextBox textBox1;
    private TextBox textBox2;
    private TextBox textBox3;
    private Label label1;
    private Label label2;
    private Label labelTotal;
    private Label labelRel;
    private IContainer components;
    private RecordingFeeDialog.FeeTypes feeType;
    private LoanData loan;
    private ToolTip toolTipField;
    private bool readOnly;
    private double totalFee;
    private Label label3;
    private Label label5;
    private Label labelSign;
    private Label labelTotalDesc;
    private Panel panel10;
    private Panel panel20;
    private Panel panel30;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private IHtmlInput ccTemp;
    private RecordingFeeDialog.FormTypes formType;
    private bool syncGFE;

    public RecordingFeeDialog(
      RecordingFeeDialog.FeeTypes feeType,
      IHtmlInput ccTemp,
      bool readOnly,
      RecordingFeeDialog.FormTypes formType)
    {
      this.feeType = feeType;
      this.loan = (LoanData) null;
      this.readOnly = readOnly;
      this.formType = formType;
      this.ccTemp = ccTemp;
      this.InitializeComponent();
      this.initForm();
    }

    public RecordingFeeDialog(
      RecordingFeeDialog.FeeTypes feeType,
      LoanData loan,
      bool readOnly,
      RecordingFeeDialog.FormTypes formType,
      bool syncGFE)
    {
      this.feeType = feeType;
      this.loan = loan;
      this.readOnly = readOnly;
      this.formType = formType;
      this.ccTemp = (IHtmlInput) null;
      this.syncGFE = syncGFE;
      this.InitializeComponent();
      this.initForm();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RecordingFeeDialog));
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.textBox1 = new TextBox();
      this.textBox2 = new TextBox();
      this.textBox3 = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.labelRel = new Label();
      this.labelTotalDesc = new Label();
      this.labelTotal = new Label();
      this.toolTipField = new ToolTip(this.components);
      this.label3 = new Label();
      this.label5 = new Label();
      this.labelSign = new Label();
      this.panel10 = new Panel();
      this.panel20 = new Panel();
      this.panel30 = new Panel();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.panel10.SuspendLayout();
      this.panel20.SuspendLayout();
      this.panel30.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(42, 31);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(75, 23);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "&OK";
      this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(123, 31);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "&Cancel";
      this.textBox1.Location = new Point(85, 12);
      this.textBox1.MaxLength = 15;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(100, 20);
      this.textBox1.TabIndex = 0;
      this.textBox1.TextAlign = HorizontalAlignment.Right;
      this.textBox1.Leave += new EventHandler(this.field_Leave);
      this.textBox1.KeyUp += new KeyEventHandler(this.num_KeyUp);
      this.textBox1.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
      this.textBox2.Location = new Point(85, 40);
      this.textBox2.MaxLength = 15;
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(100, 20);
      this.textBox2.TabIndex = 1;
      this.textBox2.TextAlign = HorizontalAlignment.Right;
      this.textBox2.Leave += new EventHandler(this.field_Leave);
      this.textBox2.KeyUp += new KeyEventHandler(this.num_KeyUp);
      this.textBox2.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
      this.textBox3.Location = new Point(86, 5);
      this.textBox3.MaxLength = 15;
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(100, 20);
      this.textBox3.TabIndex = 2;
      this.textBox3.TextAlign = HorizontalAlignment.Right;
      this.textBox3.Leave += new EventHandler(this.field_Leave);
      this.textBox3.KeyUp += new KeyEventHandler(this.num_KeyUp);
      this.textBox3.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(17, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(33, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Deed";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(17, 44);
      this.label2.Name = "label2";
      this.label2.Size = new Size(52, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Mortgage";
      this.labelRel.AutoSize = true;
      this.labelRel.Location = new Point(17, 9);
      this.labelRel.Name = "labelRel";
      this.labelRel.Size = new Size(51, 13);
      this.labelRel.TabIndex = 7;
      this.labelRel.Text = "Releases";
      this.labelTotalDesc.AutoSize = true;
      this.labelTotalDesc.Location = new Point(17, 7);
      this.labelTotalDesc.Name = "labelTotalDesc";
      this.labelTotalDesc.Size = new Size(34, 13);
      this.labelTotalDesc.TabIndex = 8;
      this.labelTotalDesc.Text = "Total:";
      this.labelTotal.Location = new Point(57, 7);
      this.labelTotal.Name = "labelTotal";
      this.labelTotal.Size = new Size(125, 16);
      this.labelTotal.TabIndex = 9;
      this.labelTotal.Text = "(Total)";
      this.labelTotal.TextAlign = ContentAlignment.MiddleRight;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(72, 16);
      this.label3.Name = "label3";
      this.label3.Size = new Size(13, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "$";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(72, 44);
      this.label5.Name = "label5";
      this.label5.Size = new Size(13, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "$";
      this.labelSign.AutoSize = true;
      this.labelSign.Location = new Point(73, 9);
      this.labelSign.Name = "labelSign";
      this.labelSign.Size = new Size(13, 13);
      this.labelSign.TabIndex = 12;
      this.labelSign.Text = "$";
      this.panel10.Controls.Add((Control) this.label1);
      this.panel10.Controls.Add((Control) this.label3);
      this.panel10.Controls.Add((Control) this.label5);
      this.panel10.Controls.Add((Control) this.textBox1);
      this.panel10.Controls.Add((Control) this.label2);
      this.panel10.Controls.Add((Control) this.textBox2);
      this.panel10.Dock = DockStyle.Top;
      this.panel10.Location = new Point(0, 0);
      this.panel10.Name = "panel10";
      this.panel10.Size = new Size(211, 66);
      this.panel10.TabIndex = 13;
      this.panel20.Controls.Add((Control) this.labelRel);
      this.panel20.Controls.Add((Control) this.labelSign);
      this.panel20.Controls.Add((Control) this.textBox3);
      this.panel20.Dock = DockStyle.Top;
      this.panel20.Location = new Point(0, 66);
      this.panel20.Name = "panel20";
      this.panel20.Size = new Size(211, 35);
      this.panel20.TabIndex = 14;
      this.panel30.Controls.Add((Control) this.pboxDownArrow);
      this.panel30.Controls.Add((Control) this.pboxAsterisk);
      this.panel30.Controls.Add((Control) this.labelTotalDesc);
      this.panel30.Controls.Add((Control) this.labelTotal);
      this.panel30.Controls.Add((Control) this.buttonOK);
      this.panel30.Controls.Add((Control) this.buttonCancel);
      this.panel30.Dock = DockStyle.Top;
      this.panel30.Location = new Point(0, 101);
      this.panel30.Name = "panel30";
      this.panel30.Size = new Size(211, 69);
      this.panel30.TabIndex = 15;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(12, 43);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 70;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(12, 27);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 69;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(211, 169);
      this.Controls.Add((Control) this.panel30);
      this.Controls.Add((Control) this.panel20);
      this.Controls.Add((Control) this.panel10);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RecordingFeeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Recording Fee";
      this.KeyPress += new KeyPressEventHandler(this.RecordingFeeDialog_KeyPress);
      this.panel10.ResumeLayout(false);
      this.panel10.PerformLayout();
      this.panel20.ResumeLayout(false);
      this.panel20.PerformLayout();
      this.panel30.ResumeLayout(false);
      this.panel30.PerformLayout();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      switch (this.feeType)
      {
        case RecordingFeeDialog.FeeTypes.RecordingFee:
          this.Text = "Recording Fee";
          this.textBox1.Tag = (object) "2402";
          this.textBox2.Tag = (object) "2403";
          this.textBox3.Tag = (object) "2404";
          this.panel20.Visible = true;
          break;
        case RecordingFeeDialog.FeeTypes.LocalTax:
          this.Text = "Local Tax/Stamps";
          this.textBox1.Tag = (object) "2405";
          this.textBox2.Tag = (object) "2406";
          this.panel20.Visible = false;
          this.Height = this.panel10.Height + this.panel30.Height + 30;
          break;
        case RecordingFeeDialog.FeeTypes.StateTax:
          this.Text = "State Tax/Stamps";
          this.textBox1.Tag = (object) "2407";
          this.textBox2.Tag = (object) "2408";
          this.panel20.Visible = false;
          this.Height = this.panel10.Height + this.panel30.Height + 30;
          break;
      }
      this.populateFieldValues(this.Controls);
      this.addTotal();
      if (this.loan == null)
        return;
      ResourceManager resources = new ResourceManager(typeof (RecordingFeeDialog));
      PopupBusinessRules popupBusinessRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      if (this.feeType == RecordingFeeDialog.FeeTypes.RecordingFee)
      {
        popupBusinessRules.SetBusinessRules((object) this.textBox1, "2402");
        popupBusinessRules.SetBusinessRules((object) this.textBox2, "2403");
        popupBusinessRules.SetBusinessRules((object) this.textBox3, "2404");
      }
      else if (this.feeType == RecordingFeeDialog.FeeTypes.LocalTax)
      {
        popupBusinessRules.SetBusinessRules((object) this.textBox1, "2405");
        popupBusinessRules.SetBusinessRules((object) this.textBox2, "2406");
      }
      else
      {
        if (this.feeType != RecordingFeeDialog.FeeTypes.StateTax)
          return;
        popupBusinessRules.SetBusinessRules((object) this.textBox1, "2407");
        popupBusinessRules.SetBusinessRules((object) this.textBox2, "2408");
      }
    }

    private void populateFieldValues(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is TextBox)
        {
          TextBox textBox = (TextBox) c;
          if (textBox != null)
          {
            textBox.Text = "";
            if (textBox.Tag != null)
            {
              string str = textBox.Tag.ToString();
              if (!(str == string.Empty))
              {
                if (this.ccTemp != null)
                  textBox.Text = this.ccTemp.GetField(str);
                else
                  textBox.Text = this.loan.GetField(str);
                this.toolTipField.SetToolTip((Control) textBox, str);
                if (this.readOnly)
                  textBox.ReadOnly = true;
              }
            }
          }
        }
        else
          this.populateFieldValues(c.Controls);
      }
    }

    private void setFieldValues(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is TextBox)
        {
          TextBox textBox = (TextBox) c;
          if (textBox != null && textBox.Tag != null)
          {
            string id = textBox.Tag.ToString();
            if (!(id == string.Empty) && textBox.Visible)
            {
              if (this.ccTemp != null)
                this.ccTemp.SetCurrentField(id, textBox.Text.Trim());
              else
                this.loan.SetCurrentField(id, textBox.Text.Trim());
            }
          }
        }
        else
          this.setFieldValues(c.Controls);
      }
    }

    private void num_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void num_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (needsUpdate)
      {
        textBox.Text = str;
        textBox.SelectionStart = str.Length;
      }
      this.addTotal();
    }

    private void addTotal()
    {
      this.totalFee = Utils.ParseDouble((object) this.textBox1.Text) + Utils.ParseDouble((object) this.textBox2.Text);
      if (this.feeType == RecordingFeeDialog.FeeTypes.RecordingFee)
        this.totalFee += Utils.ParseDouble((object) this.textBox3.Text);
      this.labelTotal.Text = this.totalFee.ToString("N2");
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.setFieldValues(this.Controls);
      string str1 = "Deed $";
      string str2 = (!(this.textBox1.Text.Trim() != string.Empty) ? str1 + "0.00" : str1 + this.textBox1.Text.Trim()) + ";Mortgage $";
      string val = !(this.textBox2.Text.Trim() != string.Empty) ? str2 + "0.00" : str2 + this.textBox2.Text.Trim();
      switch (this.feeType)
      {
        case RecordingFeeDialog.FeeTypes.RecordingFee:
          string str3 = val + ";Releases $";
          this.setFieldValue("1636", !(this.textBox3.Text.Trim() != string.Empty) ? str3 + "0.00" : str3 + this.textBox3.Text.Trim());
          this.setFieldValue("390", (this.totalFee - Utils.ParseDouble(this.loan != null ? (object) this.loan.GetField("587") : (object) this.ccTemp.GetField("587"))).ToString("N2"));
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("390", (string) null, (string) null);
          if (this.loan != null)
          {
            if (this.formType == RecordingFeeDialog.FormTypes.MLDS)
            {
              this.setFieldValue("RE88395.X91", this.totalFee.ToString("N2"));
              this.loan.Calculator.FormCalculation("MLDS", "RE88395.X91", this.totalFee.ToString("N2"));
            }
            else if (this.syncGFE)
              this.loan.Calculator.CopyGFEToMLDS("390");
            this.loan.Calculator.CopyHUD2010ToGFE2010("2402", false);
            this.loan.Calculator.CopyHUD2010ToGFE2010("390", false);
            break;
          }
          break;
        case RecordingFeeDialog.FeeTypes.LocalTax:
          this.setFieldValue("1637", val);
          this.setFieldValue("647", (this.totalFee - Utils.ParseDouble(this.loan != null ? (object) this.loan.GetField("593") : (object) this.ccTemp.GetField("593"))).ToString("N2"));
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("647", (string) null, (string) null);
          if (this.loan != null)
          {
            if (this.formType == RecordingFeeDialog.FormTypes.MLDS)
            {
              this.setFieldValue("RE88395.X94", this.totalFee.ToString("N2"));
              this.loan.Calculator.FormCalculation("MLDS", "RE88395.X94", this.totalFee.ToString("N2"));
            }
            else if (this.syncGFE)
              this.loan.Calculator.CopyGFEToMLDS("647");
            this.loan.Calculator.CopyHUD2010ToGFE2010("2405", false);
            this.loan.Calculator.CopyHUD2010ToGFE2010("647", false);
            break;
          }
          break;
        case RecordingFeeDialog.FeeTypes.StateTax:
          this.setFieldValue("1638", val);
          if (this.formType == RecordingFeeDialog.FormTypes.MLDS)
          {
            this.setFieldValue("RE88395.X89", this.totalFee.ToString("N2"));
            break;
          }
          this.setFieldValue("648", (this.totalFee - Utils.ParseDouble(this.loan != null ? (object) this.loan.GetField("594") : (object) this.ccTemp.GetField("594"))).ToString("N2"));
          if (this.loan != null)
            this.loan.Calculator.FormCalculation("648", (string) null, (string) null);
          if (this.loan != null)
          {
            this.loan.Calculator.CopyHUD2010ToGFE2010("2407", false);
            this.loan.Calculator.CopyHUD2010ToGFE2010("648", false);
            break;
          }
          break;
      }
      if (this.loan == null)
        return;
      if (this.formType == RecordingFeeDialog.FormTypes.MLDS)
      {
        this.loan.Calculator.FormCalculation("MLDS", (string) null, (string) null);
      }
      else
      {
        this.loan.Calculator.FormCalculation("REGZ", (string) null, (string) null);
        if (!this.loan.Use2010RESPA)
          return;
        this.loan.Calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      }
    }

    private void setFieldValue(string id, string val)
    {
      if (this.ccTemp != null)
        this.ccTemp.SetCurrentField(id, val);
      else
        this.loan.SetCurrentField(id, val);
    }

    private void field_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text.Trim());
      textBox.Text = num.ToString("N2");
    }

    private void RecordingFeeDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    public enum FeeTypes
    {
      RecordingFee,
      LocalTax,
      StateTax,
    }

    public enum FormTypes
    {
      GFEItemization,
      MLDS,
    }
  }
}
