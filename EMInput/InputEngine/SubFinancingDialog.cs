// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SubFinancingDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SubFinancingDialog : Form
  {
    private Label label3;
    private Label label2;
    private IContainer components;
    private TextBox firstTxt;
    private TextBox secondTxt;
    private ToolTip fieldToolTip;
    private LoanData loan;

    public SubFinancingDialog(double firstLien, double secondLien)
    {
      this.InitializeComponent();
      this.fieldToolTip.SetToolTip((Control) this.firstTxt, "3035");
      this.fieldToolTip.SetToolTip((Control) this.secondTxt, "3036");
      this.firstTxt.Text = firstLien.ToString("N2");
      this.secondTxt.Text = secondLien.ToString("N2");
      this.firstTxt.ReadOnly = true;
      this.secondTxt.ReadOnly = true;
    }

    public SubFinancingDialog(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.fieldToolTip.SetToolTip((Control) this.firstTxt, "3035");
      this.fieldToolTip.SetToolTip((Control) this.secondTxt, "3036");
      loan.GetField("427");
      loan.GetField("428");
      loan.GetField("3035");
      loan.GetField("3036");
      loan.GetField("3043");
      loan.GetSimpleField("2958");
      bool flag1 = this.loan != null && (this.loan.LinkSyncType == LinkSyncType.None || this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.PiggybackLinked);
      bool flag2 = this.loan != null && this.loan.Calculator != null && this.loan.Calculator.IsPiggybackHELOC;
      bool flag3 = this.loan != null && this.loan.Calculator != null && this.loan.Calculator.IsHELOCOrOtherLoan;
      this.firstTxt.Text = loan.GetField("3035");
      this.firstTxt.Tag = (object) "3035";
      this.secondTxt.Text = loan.GetField("3036");
      this.secondTxt.Tag = (object) "3036";
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
      this.label2 = new Label();
      this.secondTxt = new TextBox();
      this.label3 = new Label();
      this.firstTxt = new TextBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(73, 13);
      this.label2.TabIndex = 60;
      this.label2.Text = "2nd Mortgage";
      this.secondTxt.Location = new Point(106, 36);
      this.secondTxt.MaxLength = 10;
      this.secondTxt.Name = "secondTxt";
      this.secondTxt.ReadOnly = true;
      this.secondTxt.Size = new Size(143, 20);
      this.secondTxt.TabIndex = 2;
      this.secondTxt.Tag = (object) "\"3036\"";
      this.secondTxt.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(69, 13);
      this.label3.TabIndex = 57;
      this.label3.Text = "1st Mortgage";
      this.firstTxt.Location = new Point(106, 10);
      this.firstTxt.MaxLength = 10;
      this.firstTxt.Name = "firstTxt";
      this.firstTxt.ReadOnly = true;
      this.firstTxt.Size = new Size(144, 20);
      this.firstTxt.TabIndex = 1;
      this.firstTxt.Tag = (object) "\"3035\"";
      this.firstTxt.TextAlign = HorizontalAlignment.Right;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(270, 62);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.secondTxt);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.firstTxt);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SubFinancingDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Subordinate Mortgage Loan Amounts";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
