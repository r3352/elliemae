// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanErrorDetailsLink
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanErrorDetailsLink : UserControl
  {
    private string details;
    private string stackTrace;
    private string errorType;
    private string source;
    private string errorCode;
    private string errorId;
    private IContainer components;
    private EllieMae.EMLite.UI.LinkLabel description_lbl;

    public LoanErrorDetailsLink(
      string details,
      string stackTrace,
      string errorType,
      string source,
      string errorCode,
      string errorId)
    {
      this.details = details;
      this.stackTrace = stackTrace;
      this.errorType = errorType;
      this.source = source;
      this.errorCode = errorCode;
      this.errorId = errorId;
      this.InitializeComponent();
    }

    private void description_lbl_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Error ID: " + this.errorId + "\nType: " + this.errorType + "\nSource: " + this.source + "\nError Code: " + this.errorCode + "\n\nDescription: " + this.details + "\n\nStack Trace: " + this.stackTrace, "Loan Error Details");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.description_lbl = new EllieMae.EMLite.UI.LinkLabel();
      this.SuspendLayout();
      this.description_lbl.AutoSize = true;
      this.description_lbl.Font = new Font("Microsoft Sans Serif", 7f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.description_lbl.Location = new Point(3, 0);
      this.description_lbl.Name = "description_lbl";
      this.description_lbl.Size = new Size(63, 13);
      this.description_lbl.TabIndex = 2;
      this.description_lbl.Text = "View";
      this.description_lbl.Click += new EventHandler(this.description_lbl_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.description_lbl);
      this.Name = nameof (LoanErrorDetailsLink);
      this.Size = new Size(70, 16);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
