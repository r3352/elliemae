// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.NonVOLEntryDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class NonVOLEntryDialog : Form
  {
    private const string PRINCIPALREDUCTION = "Principal Reduction";
    private const string PRINCIPALREDUCTIONCURE = "Principal Reduction Cure";
    private FieldDefinition unfl0001Options;
    private int fieldId;
    private bool isAlternate = true;
    private int lastIndex = -1;
    private IContainer components;
    private Panel pnlMainContainer;
    private TextBox txtAdjDecription;
    private ComboBox cbbAdjType;
    private Label label2;
    private Label label1;
    private Panel pnl2;
    private Panel pnlPOC;
    private CheckBox chkPOC;
    private Label label6;
    private Panel pnlcredit;
    private CheckBox chkcredit;
    private Label label5;
    private Button btnCancel;
    private Button btnOk;
    private TextBox txtAdjAmount;
    private Label label4;
    private Panel pnlPaidBy;
    private ComboBox cmdPaidBy;
    private Label label7;
    private TextBox txtOtherAdjDecription;
    private Label label3;

    public string adjustmentType { get; set; }

    public string adjustmentDescription { get; set; }

    public string adjustmentOtherDescription { get; set; }

    public Decimal adjustmentAmount { get; set; }

    public bool pocIndicator { get; set; }

    public string paidBy { get; set; }

    public string principalCureAddendum { get; set; }

    public NonVOLEntryDialog(
      int fieldId = 0,
      string adjType = "",
      string adjDescription = "",
      string adjOtherDescription = "",
      Decimal adjAmount = 0M,
      bool isAlternate = true,
      bool pocIndicator = false,
      string paidBy = "",
      string principalCureAddendum = "")
    {
      this.fieldId = fieldId;
      this.adjustmentType = adjType;
      this.adjustmentOtherDescription = adjOtherDescription;
      this.adjustmentDescription = adjDescription;
      this.adjustmentAmount = adjAmount < 0M ? adjAmount * -1M : adjAmount;
      this.isAlternate = isAlternate;
      this.pocIndicator = pocIndicator;
      this.paidBy = paidBy;
      this.principalCureAddendum = principalCureAddendum;
      this.InitializeComponent();
      this.unfl0001Options = EncompassFields.GetField("UNFL0001");
      if (adjType == "Other")
      {
        this.pnlcredit.Visible = true;
        this.chkcredit.Checked = adjAmount < 0M;
      }
      else
        this.pnlcredit.Visible = false;
      this.pnl2.Location = this.pnlPaidBy.Location;
      this.Size = new Size(373, 196);
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void initForm()
    {
      for (int index = 0; index < this.unfl0001Options.Options.Count; ++index)
        this.cbbAdjType.Items.Add((object) this.unfl0001Options.Options[index].Text);
      this.cbbAdjType.Items.Add((object) "Principal Reduction");
      if (this.adjustmentOtherDescription != "PrincipalReductionCure")
      {
        if (!this.isAlternate || this.adjustmentOtherDescription == "StandardOther")
        {
          this.cbbAdjType.Items.Clear();
          this.cbbAdjType.Items.Add((object) "");
          this.cbbAdjType.Items.Add((object) "Principal Reduction");
          this.cbbAdjType.Items.Add((object) "Other");
          if (this.adjustmentOtherDescription == "PrincipalReduction")
            this.cbbAdjType.SelectedIndex = this.cbbAdjType.FindStringExact("Principal Reduction");
          else
            this.cbbAdjType.SelectedIndex = this.cbbAdjType.FindStringExact(this.adjustmentType);
          if (this.pocIndicator)
            this.parseAdjustmentAmount();
        }
        else if (this.adjustmentType == "Other" && this.adjustmentOtherDescription == "PrincipalReduction")
        {
          if (this.pocIndicator)
            this.parseAdjustmentAmount();
          this.cbbAdjType.SelectedIndex = this.cbbAdjType.FindStringExact("Principal Reduction");
        }
        else
          this.cbbAdjType.Text = this.adjustmentType;
      }
      else
      {
        this.cbbAdjType.Items.Clear();
        this.cbbAdjType.Items.Add((object) "Principal Reduction Cure");
        this.cbbAdjType.SelectedIndex = this.cbbAdjType.FindStringExact("Principal Reduction Cure");
      }
      if (this.fieldId == 0)
        return;
      this.txtAdjDecription.Text = this.adjustmentDescription;
      this.txtOtherAdjDecription.Text = this.adjustmentOtherDescription;
      this.txtAdjAmount.Text = this.adjustmentAmount.ToString("N2");
      this.chkPOC.Checked = this.pocIndicator;
      this.cmdPaidBy.Text = this.paidBy;
    }

    private void parseAdjustmentAmount()
    {
      int num1 = this.adjustmentDescription.IndexOf("$");
      int num2 = this.adjustmentDescription.IndexOf("Principal");
      if (num2 <= -1 || num1 <= -1)
        return;
      this.adjustmentAmount = Utils.ParseDecimal((object) this.adjustmentDescription.Substring(num1 + 1, num2 - num1 - 1));
      this.txtAdjAmount.Text = this.adjustmentAmount.ToString();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      bool flag = true;
      if (!string.IsNullOrEmpty(this.cbbAdjType.Text))
      {
        this.adjustmentType = this.cbbAdjType.SelectedItem.ToString() == "Principal Reduction" || this.cbbAdjType.SelectedItem.ToString() == "Principal Reduction Cure" ? "Other" : this.cbbAdjType.SelectedItem.ToString();
        if (this.chkPOC.Checked && this.cmdPaidBy.SelectedIndex == -1)
        {
          flag = false;
          int num = (int) Utils.Dialog((IWin32Window) this, "POC is checked, so Paid By is required");
        }
        if (!flag)
          return;
        this.adjustmentDescription = this.txtAdjDecription.Text;
        this.adjustmentOtherDescription = this.txtOtherAdjDecription.Text;
        this.adjustmentAmount = this.adjustmentType == "Other" ? (this.chkcredit.Checked ? Utils.ParseDecimal((object) this.txtAdjAmount.Text) * -1M : Utils.ParseDecimal((object) this.txtAdjAmount.Text)) : Utils.ParseDecimal((object) this.txtAdjAmount.Text) * -1M;
        this.pocIndicator = this.chkPOC.Checked;
        this.paidBy = this.cmdPaidBy.Text;
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Adjustment type cannot be blank.");
      }
    }

    private void txtAdjAmount_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void txtAdjAmount_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void NonVOLEntryDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void cbbAdjType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cbbAdjType.SelectedIndex == this.lastIndex)
        return;
      this.lastIndex = this.cbbAdjType.SelectedIndex;
      this.setupDesc();
    }

    private string CalculateDescription()
    {
      if (this.cmdPaidBy.SelectedItem == null)
        return "Principal Reduction to Borrower";
      if ((string) this.cbbAdjType.SelectedItem == "Principal Reduction Cure")
      {
        this.calculatePaidBy();
        return this.adjustmentDescription;
      }
      if (this.txtAdjAmount.Text != string.Empty)
        this.adjustmentAmount = Convert.ToDecimal(this.txtAdjAmount.Text);
      string str = string.Format("{0:C}", (object) this.adjustmentAmount).Replace(",", "");
      if (!this.chkPOC.Checked)
        return "Principal Reduction to Borrower";
      return this.isAlternate ? string.Format("{0} Principal Reduction to Borrower (P.O.C. {1})", (object) str, (object) this.cmdPaidBy.SelectedItem.ToString()) : string.Format("{0} Principal Reduction P.O.C. {1}", (object) str, (object) this.cmdPaidBy.SelectedItem.ToString());
    }

    private void NonVOLEntryDialog_Load(object sender, EventArgs e) => this.initForm();

    private void chkPOC_CheckedChanged(object sender, EventArgs e)
    {
      if ((string) this.cbbAdjType.SelectedItem != "Principal Reduction Cure")
        this.txtAdjDecription.Text = this.CalculateDescription();
      if (this.chkPOC.Checked)
      {
        this.txtAdjAmount.Text = string.Empty;
        this.txtAdjAmount.ReadOnly = true;
      }
      else
      {
        this.txtAdjAmount.ReadOnly = false;
        this.txtAdjAmount.Text = this.adjustmentAmount.ToString();
      }
      this.pocIndicator = this.chkPOC.Checked;
    }

    private void cmdPaidBy_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.txtAdjDecription.Text = this.CalculateDescription();
      this.paidBy = this.cmdPaidBy.Text;
    }

    private void calculatePaidBy()
    {
      string empty = string.Empty;
      int startIndex = this.adjustmentDescription.IndexOf("P.O.C.");
      if (startIndex <= -1)
        return;
      string[] source = this.adjustmentDescription.Substring(startIndex).Split('.');
      string str = source[((IEnumerable<string>) source).Count<string>() - 1];
      if (str.IndexOf(")") > -1)
        str = str.Substring(0, str.Length - 1);
      string result = str.Trim();
      if (string.IsNullOrEmpty(result))
        return;
      this.adjustmentDescription = this.replacePaidBy(this.adjustmentDescription, result);
      this.principalCureAddendum = this.replacePaidBy(this.principalCureAddendum, result);
    }

    private string replacePaidBy(string desc, string result)
    {
      int num1 = desc.IndexOf(result);
      if (num1 <= -1)
        return desc;
      int num2 = desc.IndexOf(result, num1 + 1);
      return num2 <= -1 ? desc.Replace(result, this.cmdPaidBy.Text) : desc.Substring(0, num2) + desc.Substring(num2).Replace(result, this.cmdPaidBy.Text);
    }

    private void setupDesc()
    {
      this.txtOtherAdjDecription.ReadOnly = this.txtAdjDecription.ReadOnly = false;
      if (this.cbbAdjType.SelectedItem != null && this.cbbAdjType.SelectedItem.ToString() != "Principal Reduction Cure")
        this.txtAdjDecription.Text = this.txtOtherAdjDecription.Text = string.Empty;
      this.pnlPOC.Visible = false;
      this.cmdPaidBy.Visible = false;
      this.pnl2.Location = this.pnlPaidBy.Location;
      this.Size = new Size(373, 196);
      if (this.cbbAdjType.SelectedItem == null)
      {
        this.txtAdjDecription.Enabled = !(this.txtOtherAdjDecription.Enabled = false);
        this.txtOtherAdjDecription.Text = "";
        this.pnlcredit.Visible = false;
        this.pnlPaidBy.Visible = false;
      }
      else
      {
        switch (this.cbbAdjType.SelectedItem.ToString())
        {
          case "Other":
            if (!this.isAlternate || this.adjustmentOtherDescription == "StandardOther")
              this.txtAdjAmount.ReadOnly = !(this.txtAdjDecription.Enabled = !(this.txtOtherAdjDecription.Enabled = false));
            else
              this.txtAdjAmount.ReadOnly = !(this.txtAdjDecription.Enabled = this.txtOtherAdjDecription.Enabled = true);
            this.pnlcredit.Visible = true;
            this.pnlPaidBy.Visible = false;
            this.pocIndicator = this.chkPOC.Checked = false;
            if (this.isAlternate)
              break;
            this.txtOtherAdjDecription.Text = "StandardOther";
            break;
          case "Principal Reduction Cure":
            this.txtOtherAdjDecription.ReadOnly = this.txtAdjDecription.ReadOnly = true;
            this.pnlcredit.Enabled = this.pnlPOC.Enabled = false;
            this.pnlcredit.Visible = this.pnlPOC.Visible = this.cmdPaidBy.Visible = this.pnlPaidBy.Visible = true;
            this.txtOtherAdjDecription.Text = "PrincipalReductionCure";
            this.pnl2.Location = new Point(8, 84);
            this.Size = new Size(373, 221);
            break;
          case "Principal Reduction":
            this.txtOtherAdjDecription.ReadOnly = this.txtAdjDecription.ReadOnly = true;
            this.txtAdjAmount.ReadOnly = this.chkPOC.Checked;
            this.pnlcredit.Visible = this.pnlPOC.Visible = this.cmdPaidBy.Visible = this.pnlPaidBy.Visible = true;
            this.txtOtherAdjDecription.Text = "PrincipalReduction";
            if (this.txtAdjDecription.Text == string.Empty)
              this.txtAdjDecription.Text = this.CalculateDescription();
            this.pnl2.Location = new Point(8, 84);
            this.Size = new Size(373, 221);
            break;
          default:
            this.txtAdjDecription.Enabled = !(this.txtOtherAdjDecription.Enabled = false);
            this.txtOtherAdjDecription.Text = "";
            this.pnlcredit.Visible = false;
            this.pnlPaidBy.Visible = false;
            break;
        }
      }
    }

    private void txtAdjAmount_Leave(object sender, EventArgs e)
    {
      this.adjustmentAmount = Utils.ParseDecimal((object) this.txtAdjAmount.Text);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlMainContainer = new Panel();
      this.pnl2 = new Panel();
      this.pnlPOC = new Panel();
      this.chkPOC = new CheckBox();
      this.label6 = new Label();
      this.pnlcredit = new Panel();
      this.chkcredit = new CheckBox();
      this.label5 = new Label();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.txtAdjAmount = new TextBox();
      this.label4 = new Label();
      this.pnlPaidBy = new Panel();
      this.cmdPaidBy = new ComboBox();
      this.label7 = new Label();
      this.txtAdjDecription = new TextBox();
      this.cbbAdjType = new ComboBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label3 = new Label();
      this.txtOtherAdjDecription = new TextBox();
      this.pnlMainContainer.SuspendLayout();
      this.pnl2.SuspendLayout();
      this.pnlPOC.SuspendLayout();
      this.pnlcredit.SuspendLayout();
      this.pnlPaidBy.SuspendLayout();
      this.SuspendLayout();
      this.pnlMainContainer.Controls.Add((Control) this.pnl2);
      this.pnlMainContainer.Controls.Add((Control) this.pnlPaidBy);
      this.pnlMainContainer.Controls.Add((Control) this.txtAdjDecription);
      this.pnlMainContainer.Controls.Add((Control) this.cbbAdjType);
      this.pnlMainContainer.Controls.Add((Control) this.label2);
      this.pnlMainContainer.Controls.Add((Control) this.label1);
      this.pnlMainContainer.Dock = DockStyle.Fill;
      this.pnlMainContainer.Location = new Point(0, 0);
      this.pnlMainContainer.Name = "pnlMainContainer";
      this.pnlMainContainer.Size = new Size(357, 182);
      this.pnlMainContainer.TabIndex = 0;
      this.pnl2.Controls.Add((Control) this.pnlPOC);
      this.pnl2.Controls.Add((Control) this.pnlcredit);
      this.pnl2.Controls.Add((Control) this.btnCancel);
      this.pnl2.Controls.Add((Control) this.btnOk);
      this.pnl2.Controls.Add((Control) this.txtAdjAmount);
      this.pnl2.Controls.Add((Control) this.txtOtherAdjDecription);
      this.pnl2.Controls.Add((Control) this.label4);
      this.pnl2.Controls.Add((Control) this.label3);
      this.pnl2.Location = new Point(8, 84);
      this.pnl2.Name = "pnl2";
      this.pnl2.Size = new Size(343, 92);
      this.pnl2.TabIndex = 15;
      this.pnlPOC.Controls.Add((Control) this.chkPOC);
      this.pnlPOC.Controls.Add((Control) this.label6);
      this.pnlPOC.Location = new Point(80, 57);
      this.pnlPOC.Name = "pnlPOC";
      this.pnlPOC.Size = new Size(71, 30);
      this.pnlPOC.TabIndex = 19;
      this.pnlPOC.Visible = false;
      this.chkPOC.AutoSize = true;
      this.chkPOC.Location = new Point(40, 10);
      this.chkPOC.Name = "chkPOC";
      this.chkPOC.Size = new Size(15, 14);
      this.chkPOC.TabIndex = 1;
      this.chkPOC.UseVisualStyleBackColor = true;
      this.chkPOC.CheckedChanged += new EventHandler(this.chkPOC_CheckedChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 10);
      this.label6.Name = "label6";
      this.label6.Size = new Size(29, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "POC";
      this.pnlcredit.Controls.Add((Control) this.chkcredit);
      this.pnlcredit.Controls.Add((Control) this.label5);
      this.pnlcredit.Location = new Point(3, 57);
      this.pnlcredit.Name = "pnlcredit";
      this.pnlcredit.Size = new Size(71, 30);
      this.pnlcredit.TabIndex = 18;
      this.chkcredit.AutoSize = true;
      this.chkcredit.Location = new Point(40, 10);
      this.chkcredit.Name = "chkcredit";
      this.chkcredit.Size = new Size(15, 14);
      this.chkcredit.TabIndex = 1;
      this.chkcredit.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 10);
      this.label5.Name = "label5";
      this.label5.Size = new Size(34, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Credit";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(260, 62);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 17;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Location = new Point(181, 62);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 16;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.txtAdjAmount.Location = new Point(163, 26);
      this.txtAdjAmount.Name = "txtAdjAmount";
      this.txtAdjAmount.Size = new Size(172, 20);
      this.txtAdjAmount.TabIndex = 15;
      this.txtAdjAmount.KeyPress += new KeyPressEventHandler(this.txtAdjAmount_KeyPress);
      this.txtAdjAmount.KeyUp += new KeyEventHandler(this.txtAdjAmount_KeyUp);
      this.txtAdjAmount.Leave += new EventHandler(this.txtAdjAmount_Leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(2, 27);
      this.label4.Name = "label4";
      this.label4.Size = new Size(101, 13);
      this.label4.TabIndex = 13;
      this.label4.Text = "Adjustment Amount ";
      this.pnlPaidBy.Controls.Add((Control) this.cmdPaidBy);
      this.pnlPaidBy.Controls.Add((Control) this.label7);
      this.pnlPaidBy.Location = new Point(8, 58);
      this.pnlPaidBy.Name = "pnlPaidBy";
      this.pnlPaidBy.Size = new Size(343, 26);
      this.pnlPaidBy.TabIndex = 14;
      this.cmdPaidBy.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmdPaidBy.FormattingEnabled = true;
      this.cmdPaidBy.Items.AddRange(new object[4]
      {
        (object) "Borrower",
        (object) "Seller",
        (object) "Lender",
        (object) "Broker (Realtor)"
      });
      this.cmdPaidBy.Location = new Point(163, 3);
      this.cmdPaidBy.Name = "cmdPaidBy";
      this.cmdPaidBy.Size = new Size(172, 21);
      this.cmdPaidBy.TabIndex = 15;
      this.cmdPaidBy.SelectedIndexChanged += new EventHandler(this.cmdPaidBy_SelectedIndexChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(2, 5);
      this.label7.Name = "label7";
      this.label7.Size = new Size(43, 13);
      this.label7.TabIndex = 14;
      this.label7.Text = "Paid By";
      this.txtAdjDecription.Location = new Point(171, 36);
      this.txtAdjDecription.MaxLength = 100;
      this.txtAdjDecription.Name = "txtAdjDecription";
      this.txtAdjDecription.Size = new Size(172, 20);
      this.txtAdjDecription.TabIndex = 5;
      this.cbbAdjType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbbAdjType.FormattingEnabled = true;
      this.cbbAdjType.Location = new Point(171, 12);
      this.cbbAdjType.Name = "cbbAdjType";
      this.cbbAdjType.Size = new Size(172, 21);
      this.cbbAdjType.TabIndex = 4;
      this.cbbAdjType.SelectedIndexChanged += new EventHandler(this.cbbAdjType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(118, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Adjustment Description ";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(89, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Adjustment Type ";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(2, 3);
      this.label3.Name = "label3";
      this.label3.Size = new Size(147, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Adjustment Other Description ";
      this.txtOtherAdjDecription.Location = new Point(163, 3);
      this.txtOtherAdjDecription.MaxLength = 100;
      this.txtOtherAdjDecription.Name = "txtOtherAdjDecription";
      this.txtOtherAdjDecription.Size = new Size(172, 20);
      this.txtOtherAdjDecription.TabIndex = 14;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(357, 182);
      this.Controls.Add((Control) this.pnlMainContainer);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NonVOLEntryDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Other Adjustment";
      this.Load += new EventHandler(this.NonVOLEntryDialog_Load);
      this.KeyPress += new KeyPressEventHandler(this.NonVOLEntryDialog_KeyPress);
      this.pnlMainContainer.ResumeLayout(false);
      this.pnlMainContainer.PerformLayout();
      this.pnl2.ResumeLayout(false);
      this.pnl2.PerformLayout();
      this.pnlPOC.ResumeLayout(false);
      this.pnlPOC.PerformLayout();
      this.pnlcredit.ResumeLayout(false);
      this.pnlcredit.PerformLayout();
      this.pnlPaidBy.ResumeLayout(false);
      this.pnlPaidBy.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
