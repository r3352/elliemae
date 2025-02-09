// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MultiInputDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MultiInputDialog : Form
  {
    private IContainer components;
    private TextBox txtManualInput;
    private Button btnCancel;
    private Label lblHelp;
    private Button btnAdd;

    public MultiInputDialog(FileImportType fileType)
    {
      this.InitializeComponent();
      this.btnAdd.Enabled = false;
      if (fileType == FileImportType.LoanNumberContractNumberProductName)
      {
        this.lblHelp.Text = "Enter loan numbers, commitment contract numbers, and product names separated by commas per row for each loan number.";
      }
      else
      {
        if (fileType != FileImportType.LoanNumberPrice)
          return;
        this.lblHelp.Text = "Enter loan numbers and total prices separated by commas per row for each loan number.";
      }
    }

    public string HeaderText { get; set; }

    public string HelpText { get; set; }

    public List<string> ReturnValue
    {
      get
      {
        return ((IEnumerable<string>) this.txtManualInput.Text.Trim().Split(new char[2]
        {
          '\r',
          '\n'
        }, StringSplitOptions.RemoveEmptyEntries)).ToList<string>();
      }
    }

    private void txtManualInput_TextChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.txtManualInput.Text.Trim().Length > 0;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtManualInput = new TextBox();
      this.btnCancel = new Button();
      this.lblHelp = new Label();
      this.btnAdd = new Button();
      this.SuspendLayout();
      this.txtManualInput.Location = new Point(16, 44);
      this.txtManualInput.Multiline = true;
      this.txtManualInput.Name = "txtManualInput";
      this.txtManualInput.ScrollBars = ScrollBars.Vertical;
      this.txtManualInput.Size = new Size(251, 168);
      this.txtManualInput.TabIndex = 0;
      this.txtManualInput.TextChanged += new EventHandler(this.txtManualInput_TextChanged);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(195, 218);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblHelp.Location = new Point(13, 9);
      this.lblHelp.Name = "lblHelp";
      this.lblHelp.Size = new Size(254, 32);
      this.lblHelp.TabIndex = 2;
      this.lblHelp.Text = "Enter loan numbers separated by commas or line break.";
      this.btnAdd.DialogResult = DialogResult.OK;
      this.btnAdd.Location = new Point(114, 218);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "&Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(282, 253);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.lblHelp);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtManualInput);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MultiInputDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Loan Numbers";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
