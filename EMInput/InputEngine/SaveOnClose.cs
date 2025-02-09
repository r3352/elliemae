// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SaveOnClose
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
  public class SaveOnClose : Form
  {
    private bool _boolSaveLoan;
    private IContainer components;
    private RadioButton rbtnCloseSave;
    private Panel panel2;
    private Button button2;
    private Button button1;
    private Panel panel1;
    private Label label1;
    private RadioButton rbtnCloseNotSave;
    private Panel panel3;

    public SaveOnClose() => this.InitializeComponent();

    public bool BoolSaveLoan => this._boolSaveLoan;

    private void button1_Click(object sender, EventArgs e)
    {
      this._boolSaveLoan = this.rbtnCloseSave.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SaveOnClose));
      this.rbtnCloseSave = new RadioButton();
      this.panel2 = new Panel();
      this.button2 = new Button();
      this.button1 = new Button();
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.rbtnCloseNotSave = new RadioButton();
      this.panel3 = new Panel();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.rbtnCloseSave.Checked = true;
      this.rbtnCloseSave.Location = new Point(28, 35);
      this.rbtnCloseSave.Name = "rbtnCloseSave";
      this.rbtnCloseSave.Size = new Size(212, 28);
      this.rbtnCloseSave.TabIndex = 0;
      this.rbtnCloseSave.TabStop = true;
      this.rbtnCloseSave.Text = "Save and exit loan";
      this.panel2.Controls.Add((Control) this.button2);
      this.panel2.Controls.Add((Control) this.button1);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(0, 94);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(260, 44);
      this.panel2.TabIndex = 10;
      this.button2.DialogResult = DialogResult.Cancel;
      this.button2.Location = new Point(168, 12);
      this.button2.Name = "button2";
      this.button2.Size = new Size(75, 23);
      this.button2.TabIndex = 5;
      this.button2.Text = "&Cancel";
      this.button1.DialogResult = DialogResult.OK;
      this.button1.Location = new Point(88, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "&OK";
      this.button1.Click += new EventHandler(this.button1_Click);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(260, 33);
      this.panel1.TabIndex = 9;
      this.label1.Location = new Point(6, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(242, 23);
      this.label1.TabIndex = 3;
      this.label1.Text = "Before reviewing this loan, exit the current loan. ";
      this.rbtnCloseNotSave.Location = new Point(28, 63);
      this.rbtnCloseNotSave.Name = "rbtnCloseNotSave";
      this.rbtnCloseNotSave.Size = new Size(212, 28);
      this.rbtnCloseNotSave.TabIndex = 1;
      this.rbtnCloseNotSave.Text = "Exit the loan without saving";
      this.panel3.BackColor = Color.White;
      this.panel3.Controls.Add((Control) this.rbtnCloseSave);
      this.panel3.Controls.Add((Control) this.rbtnCloseNotSave);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(260, 138);
      this.panel3.TabIndex = 11;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(260, 138);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.panel3);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SaveOnClose);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Exit current loan";
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
