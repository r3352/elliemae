// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockRequest.ExtensionRequestForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.LockRequest
{
  public class ExtensionRequestForm : UserControl
  {
    private Sessions.Session session;
    private LockExtensionUtils lockExtensionUtils;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label1;
    private TextBox txtTradeExpDate;
    private Label label2;
    private LockExtensionDropdownBox lockExtDDL;
    private TextBox txtPriceAdjustment;
    private Label label3;
    private TextBox txtCPA;
    private Label label5;
    private TextBox txtReLockFee;
    private Label label4;
    private TextBox txtCustomPriceDesc;
    private Label label6;
    private Label label7;
    private TextBox txtComments;

    internal string TradeExtensionInfo => this.BuildTradeExtensionInfo();

    public ExtensionRequestForm(DateTime dtExpDate, Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.lockExtDDL.ResetSession(this.session, true);
      this.lockExtDDL.DDLTextChanged += new EventHandler(this.lockExtDDL_TextChanged);
      this.lockExtensionUtils = new LockExtensionUtils(this.session.SessionObjects, this.session.LoanData);
      this.txtTradeExpDate.Text = dtExpDate.ToShortDateString();
      this.txtTradeExpDate.Enabled = false;
      this.txtCPA.Enabled = false;
    }

    public bool requestTradeExtension() => this.validateData();

    private bool validateData()
    {
      if (!(this.lockExtDDL.Text.Trim() == ""))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "Days to Extend cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void lockExtDDL_TextChanged(object sender, EventArgs e)
    {
      if (!Utils.IsInt((object) this.lockExtDDL.Text.Trim()))
      {
        this.txtPriceAdjustment.Text = "";
      }
      else
      {
        this.txtPriceAdjustment.Text = this.getPriceAdjustment().ToString("N3");
        this.dataChanged(sender, e);
      }
    }

    private Decimal getPriceAdjustment()
    {
      int daysToExtend = Utils.ParseInt((object) this.lockExtDDL.Text);
      return (bool) this.session.ServerManager.GetServerSettings("Policies")[(object) "Policies.EnableLockExtension"] && this.lockExtensionUtils.IsCompanyControlled ? this.lockExtensionUtils.GetPriceAdjustment(daysToExtend) : 0M;
    }

    private void txtCustomPriceDesc_TextChanged(object sender, EventArgs e)
    {
      this.txtCPA.Enabled = true;
    }

    private void txtCustomPriceDesc_Leave(object sender, EventArgs e)
    {
      if (!(this.txtCustomPriceDesc.Text.Trim() == ""))
        return;
      this.txtCPA.Text = "";
      this.txtCPA.Enabled = false;
    }

    private void dataChanged(object sender, EventArgs e)
    {
      if (!(sender is LockExtensionDropdownBox))
        ;
    }

    private void txtPriceAdjustment_Leave(object sender, EventArgs e)
    {
      this.txtPriceAdjustment.Text = Utils.ParseDecimal((object) this.txtPriceAdjustment.Text.Trim()).ToString("N3");
    }

    private void txtReLockFee_Leave(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtReLockFee.Text.Trim()))
        return;
      this.txtReLockFee.Text = Utils.ParseDecimal((object) this.txtReLockFee.Text.Trim()).ToString("N3");
    }

    private void txtCPA_Leave(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtCPA.Text.Trim()))
        return;
      this.txtCPA.Text = Utils.ParseDecimal((object) this.txtCPA.Text.Trim()).ToString("N3");
    }

    private void lockExtDDL_DDLTextChanged(object sender, EventArgs e)
    {
      if (!Utils.IsInt((object) this.lockExtDDL.Text.Trim()))
      {
        this.txtPriceAdjustment.Text = "";
      }
      else
      {
        if (this.lockExtDDL.Text.Length > 1 && this.lockExtDDL.Text.Substring(0, 1) == "-")
          this.lockExtDDL.Text = "";
        if (this.lockExtDDL.Text.Length > 3)
          this.lockExtDDL.Text = this.lockExtDDL.Text.Substring(0, 3);
        int daysToExtend = Utils.ParseInt((object) this.lockExtDDL.Text);
        if (this.lockExtensionUtils.HasPriceAdjustment(daysToExtend))
          this.txtPriceAdjustment.Text = this.lockExtensionUtils.GetPriceAdjustment(daysToExtend).ToString("N3");
        else
          this.txtPriceAdjustment.Text = "0.000";
        this.dataChanged(sender, e);
      }
    }

    private string BuildTradeExtensionInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("DaysToExtend:" + this.lockExtDDL.Text + ",");
      stringBuilder.AppendLine("PriceAdjustment:" + this.txtPriceAdjustment.Text + ",");
      stringBuilder.AppendLine("ReLockFee:" + this.txtReLockFee.Text + ",");
      stringBuilder.AppendLine("CustomPriceDescription:" + this.txtCustomPriceDesc.Text + ",");
      stringBuilder.AppendLine("CustomPriceAdjustment:" + this.txtCPA.Text + ",");
      stringBuilder.AppendLine("Comment:" + this.txtComments.Text.Trim());
      return stringBuilder.ToString();
    }

    private void txtComments_Leave(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtComments.Text.Trim()) || this.txtComments.Text.Trim().Length <= 1000)
        return;
      this.txtComments.Text = this.txtComments.Text.Substring(0, 1000);
    }

    private void txtPriceAdjustment_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-')
        return;
      e.Handled = true;
    }

    private void txtReLockFee_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-')
        return;
      e.Handled = true;
    }

    private void txtCPA_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '-')
        return;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.label7 = new Label();
      this.txtComments = new TextBox();
      this.txtCustomPriceDesc = new TextBox();
      this.label6 = new Label();
      this.txtCPA = new TextBox();
      this.label5 = new Label();
      this.txtReLockFee = new TextBox();
      this.label4 = new Label();
      this.txtPriceAdjustment = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.txtTradeExpDate = new TextBox();
      this.label1 = new Label();
      this.lockExtDDL = new LockExtensionDropdownBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.txtComments);
      this.groupContainer1.Controls.Add((Control) this.txtCustomPriceDesc);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.txtCPA);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.txtReLockFee);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.txtPriceAdjustment);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtTradeExpDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.lockExtDDL);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(3, 3);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(493, 270);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Extension Information";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 186);
      this.label7.Name = "label7";
      this.label7.Size = new Size(56, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Comments";
      this.txtComments.Location = new Point(198, 180);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new Size(291, 82);
      this.txtComments.TabIndex = 11;
      this.txtComments.Tag = (object) "3370";
      this.txtComments.Leave += new EventHandler(this.txtComments_Leave);
      this.txtCustomPriceDesc.Location = new Point(198, 133);
      this.txtCustomPriceDesc.Name = "txtCustomPriceDesc";
      this.txtCustomPriceDesc.Size = new Size(171, 20);
      this.txtCustomPriceDesc.TabIndex = 4;
      this.txtCustomPriceDesc.TextChanged += new EventHandler(this.txtCustomPriceDesc_TextChanged);
      this.txtCustomPriceDesc.Leave += new EventHandler(this.txtCustomPriceDesc_Leave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 138);
      this.label6.Name = "label6";
      this.label6.Size = new Size(125, 13);
      this.label6.TabIndex = 5;
      this.label6.Text = "Custom Price Description";
      this.txtCPA.Location = new Point(198, 156);
      this.txtCPA.Name = "txtCPA";
      this.txtCPA.Size = new Size(171, 20);
      this.txtCPA.TabIndex = 5;
      this.txtCPA.Leave += new EventHandler(this.txtCPA_Leave);
      this.txtCPA.KeyPress += new KeyPressEventHandler(this.txtCPA_KeyPress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 162);
      this.label5.Name = "label5";
      this.label5.Size = new Size(124, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Custom Price Adjustment";
      this.txtReLockFee.Location = new Point(198, 110);
      this.txtReLockFee.Name = "txtReLockFee";
      this.txtReLockFee.Size = new Size(171, 20);
      this.txtReLockFee.TabIndex = 3;
      this.txtReLockFee.Leave += new EventHandler(this.txtReLockFee_Leave);
      this.txtReLockFee.KeyPress += new KeyPressEventHandler(this.txtReLockFee_KeyPress);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 115);
      this.label4.Name = "label4";
      this.label4.Size = new Size(69, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Re-Lock Fee";
      this.txtPriceAdjustment.Location = new Point(198, 88);
      this.txtPriceAdjustment.Name = "txtPriceAdjustment";
      this.txtPriceAdjustment.Size = new Size(171, 20);
      this.txtPriceAdjustment.TabIndex = 2;
      this.txtPriceAdjustment.Tag = (object) "3362";
      this.txtPriceAdjustment.Leave += new EventHandler(this.txtPriceAdjustment_Leave);
      this.txtPriceAdjustment.KeyPress += new KeyPressEventHandler(this.txtPriceAdjustment_KeyPress);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 92);
      this.label3.Name = "label3";
      this.label3.Size = new Size(86, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Price Adjustment";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 67);
      this.label2.Name = "label2";
      this.label2.Size = new Size(79, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Days to Extend";
      this.txtTradeExpDate.Location = new Point(198, 41);
      this.txtTradeExpDate.Name = "txtTradeExpDate";
      this.txtTradeExpDate.Size = new Size(171, 20);
      this.txtTradeExpDate.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 41);
      this.label1.Name = "label1";
      this.label1.Size = new Size(110, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Trade Expiration Date";
      this.lockExtDDL.BackColor = Color.Transparent;
      this.lockExtDDL.Location = new Point(198, 63);
      this.lockExtDDL.Name = "lockExtDDL";
      this.lockExtDDL.Size = new Size(171, 23);
      this.lockExtDDL.TabIndex = 1;
      this.lockExtDDL.Tag = (object) "3360";
      this.lockExtDDL.DDLTextChanged += new EventHandler(this.lockExtDDL_DDLTextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (ExtensionRequestForm);
      this.Size = new Size(499, 276);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
