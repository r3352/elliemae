// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FieldHelpItem
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
  public class FieldHelpItem : UserControl
  {
    private IContainer components;
    private Label lblText;
    private Label label1;

    public event EventHandler Close;

    public FieldHelpItem(string text)
    {
      this.InitializeComponent();
      this.lblText.Text = text;
    }

    private void pbClose_Click(object sender, EventArgs e)
    {
      if (this.Close == null)
        return;
      this.Close((object) this, EventArgs.Empty);
    }

    private void FieldHelpItem_Resize(object sender, EventArgs e)
    {
      this.lblText.AutoSize = false;
      this.lblText.MaximumSize = new Size(0, 0);
      this.lblText.AutoSize = true;
      this.lblText.MaximumSize = new Size(this.ClientSize.Width - 2 * this.lblText.Left, 0);
      int num = this.lblText.Height + 2 * this.lblText.Top;
      if (this.Height == num)
        return;
      this.Height = num;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblText = new Label();
      this.label1 = new Label();
      this.SuspendLayout();
      this.lblText.AutoSize = true;
      this.lblText.Location = new Point(10, 4);
      this.lblText.Name = "lblText";
      this.lblText.Size = new Size(159, 13);
      this.lblText.TabIndex = 0;
      this.lblText.Text = "This is the text of the help item...";
      this.label1.BackColor = Color.LightGray;
      this.label1.Dock = DockStyle.Bottom;
      this.label1.Location = new Point(0, 34);
      this.label1.Name = "label1";
      this.label1.Size = new Size(440, 1);
      this.label1.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblText);
      this.Name = nameof (FieldHelpItem);
      this.Size = new Size(440, 35);
      this.Resize += new EventHandler(this.FieldHelpItem_Resize);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
