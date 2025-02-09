// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BorrowerInRadioControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class BorrowerInRadioControl : UserControl
  {
    public EventHandler RadioButtonChecked;
    private IContainer components;
    private RadioButton rdoBorrower;
    private Label labelName;

    public BorrowerInRadioControl(string borrowerName, bool hiden)
    {
      this.InitializeComponent();
      this.labelName.Text = borrowerName;
      this.labelName.Left = 0;
      this.labelName.Top = 0;
      this.rdoBorrower.Visible = false;
    }

    public BorrowerInRadioControl(string borrowerName)
    {
      this.InitializeComponent();
      this.rdoBorrower.Text = borrowerName;
      this.labelName.Visible = false;
    }

    private void rdoBorrower_CheckedChanged(object sender, EventArgs e)
    {
      if (this.RadioButtonChecked == null)
        return;
      this.RadioButtonChecked((object) this, e);
    }

    public void UncheckRadio(bool donotTriggerEvent)
    {
      if (donotTriggerEvent)
        this.rdoBorrower.CheckedChanged -= new EventHandler(this.rdoBorrower_CheckedChanged);
      this.rdoBorrower.Checked = false;
      if (!donotTriggerEvent)
        return;
      this.rdoBorrower.CheckedChanged += new EventHandler(this.rdoBorrower_CheckedChanged);
    }

    public bool BorrowerIsChecked => this.rdoBorrower.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rdoBorrower = new RadioButton();
      this.labelName = new Label();
      this.SuspendLayout();
      this.rdoBorrower.AutoSize = true;
      this.rdoBorrower.BackColor = SystemColors.Control;
      this.rdoBorrower.Location = new Point(3, -1);
      this.rdoBorrower.Name = "rdoBorrower";
      this.rdoBorrower.Size = new Size(98, 17);
      this.rdoBorrower.TabIndex = 0;
      this.rdoBorrower.TabStop = true;
      this.rdoBorrower.Text = "Borrower Name";
      this.rdoBorrower.UseVisualStyleBackColor = false;
      this.rdoBorrower.CheckedChanged += new EventHandler(this.rdoBorrower_CheckedChanged);
      this.labelName.AutoSize = true;
      this.labelName.Location = new Point(125, 3);
      this.labelName.Name = "labelName";
      this.labelName.Size = new Size(41, 13);
      this.labelName.TabIndex = 1;
      this.labelName.Text = "(Name)";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.labelName);
      this.Controls.Add((Control) this.rdoBorrower);
      this.Name = nameof (BorrowerInRadioControl);
      this.Size = new Size(183, 24);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
