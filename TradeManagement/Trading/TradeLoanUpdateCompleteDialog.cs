// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeLoanUpdateCompleteDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeLoanUpdateCompleteDialog : Form
  {
    private IContainer components;
    private TextBox textBoxErrors;
    private Button buttonOk;
    private Label lblSuccessTitle;
    private Label labelSuccessCount;
    private Label labelErrorCount;
    private Label lblErrorTitle;
    private Label label5;
    private Label lblTotal;
    private Label label3;

    public TradeLoanUpdateCompleteDialog(
      int successCount,
      int errorCount,
      string detailMessage,
      ProcessType processType = ProcessType.Update)
    {
      this.InitializeComponent();
      this.labelSuccessCount.Text = successCount.ToString();
      this.labelErrorCount.Text = errorCount.ToString();
      this.lblTotal.Text = (successCount + errorCount).ToString();
      switch (processType)
      {
        case ProcessType.Assignment:
          this.Text = "Loan Assignment Results";
          this.lblSuccessTitle.Text = "Loans Assigned Successfully:";
          this.lblErrorTitle.Text = "Loans With Errors:";
          break;
        case ProcessType.Void:
          this.Text = "Loan Void Results";
          this.lblSuccessTitle.Text = "Loans Voided Successfully:";
          this.lblErrorTitle.Text = "Loans With Errors:";
          break;
      }
      this.textBoxErrors.Text = detailMessage;
    }

    public TradeLoanUpdateCompleteDialog(
      int failCount,
      string detailMessage,
      ProcessType processType = ProcessType.Update)
    {
      this.InitializeComponent();
      this.labelSuccessCount.Text = "0";
      this.labelErrorCount.Text = failCount.ToString();
      this.lblTotal.Text = failCount.ToString();
      switch (processType)
      {
        case ProcessType.Assignment:
          this.Text = "Loan Assignment Results";
          this.lblSuccessTitle.Text = "Loans Assigned Successfully:";
          this.lblErrorTitle.Text = "Loans With Errors:";
          break;
        case ProcessType.Void:
          this.Text = "Loan Void Results";
          this.lblSuccessTitle.Text = "Loans Voided Successfully:";
          this.lblErrorTitle.Text = "Loans With Errors:";
          break;
      }
      this.textBoxErrors.Text = detailMessage;
    }

    private void buttonOk_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBoxErrors = new TextBox();
      this.buttonOk = new Button();
      this.lblSuccessTitle = new Label();
      this.labelSuccessCount = new Label();
      this.labelErrorCount = new Label();
      this.lblErrorTitle = new Label();
      this.label5 = new Label();
      this.lblTotal = new Label();
      this.label3 = new Label();
      this.SuspendLayout();
      this.textBoxErrors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxErrors.Location = new Point(12, 95);
      this.textBoxErrors.Multiline = true;
      this.textBoxErrors.Name = "textBoxErrors";
      this.textBoxErrors.ScrollBars = ScrollBars.Both;
      this.textBoxErrors.Size = new Size(412, 107);
      this.textBoxErrors.TabIndex = 1;
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.Location = new Point(349, 210);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(75, 23);
      this.buttonOk.TabIndex = 0;
      this.buttonOk.Text = "OK";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
      this.lblSuccessTitle.AutoSize = true;
      this.lblSuccessTitle.Location = new Point(12, 32);
      this.lblSuccessTitle.Name = "lblSuccessTitle";
      this.lblSuccessTitle.Size = new Size(145, 13);
      this.lblSuccessTitle.TabIndex = 2;
      this.lblSuccessTitle.Text = "Loans Updated Successfully:";
      this.labelSuccessCount.AutoSize = true;
      this.labelSuccessCount.Location = new Point(172, 32);
      this.labelSuccessCount.Name = "labelSuccessCount";
      this.labelSuccessCount.Size = new Size(13, 13);
      this.labelSuccessCount.TabIndex = 3;
      this.labelSuccessCount.Text = "0";
      this.labelErrorCount.AutoSize = true;
      this.labelErrorCount.Location = new Point(172, 53);
      this.labelErrorCount.Name = "labelErrorCount";
      this.labelErrorCount.Size = new Size(13, 13);
      this.labelErrorCount.TabIndex = 5;
      this.labelErrorCount.Text = "0";
      this.lblErrorTitle.AutoSize = true;
      this.lblErrorTitle.Location = new Point(12, 53);
      this.lblErrorTitle.Name = "lblErrorTitle";
      this.lblErrorTitle.Size = new Size(94, 13);
      this.lblErrorTitle.TabIndex = 4;
      this.lblErrorTitle.Text = "Loans With Errors:";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 79);
      this.label5.Name = "label5";
      this.label5.Size = new Size(39, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Details";
      this.lblTotal.AutoSize = true;
      this.lblTotal.Location = new Point(172, 11);
      this.lblTotal.Name = "lblTotal";
      this.lblTotal.Size = new Size(13, 13);
      this.lblTotal.TabIndex = 8;
      this.lblTotal.Text = "0";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 11);
      this.label3.Name = "label3";
      this.label3.Size = new Size(119, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Total Loans Processed:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(436, 240);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblTotal);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.labelErrorCount);
      this.Controls.Add((Control) this.lblErrorTitle);
      this.Controls.Add((Control) this.labelSuccessCount);
      this.Controls.Add((Control) this.lblSuccessTitle);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.textBoxErrors);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (TradeLoanUpdateCompleteDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Update Complete";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
