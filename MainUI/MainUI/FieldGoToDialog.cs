// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.FieldGoToDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class FieldGoToDialog : Form
  {
    private Button cancelBtn;
    private Button findBtn;
    private Label label1;
    private TextBox findTxt;
    private System.ComponentModel.Container components;
    private bool findNext;
    private string fieldID = string.Empty;

    public FieldGoToDialog(string fieldID)
    {
      this.fieldID = fieldID;
      this.InitializeComponent();
      this.findTxt.Text = this.fieldID;
      if (fieldID != string.Empty)
        this.findBtn.Text = "Find Next";
      else
        this.findBtn.Text = "Find";
      this.findTxt_TextChanged((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    internal bool FindNext
    {
      get => this.findNext;
      set => this.findNext = value;
    }

    internal string FieldID
    {
      get => this.fieldID;
      set => this.fieldID = value;
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.findBtn = new Button();
      this.findTxt = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(214, 54);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "&Cancel";
      this.findBtn.DialogResult = DialogResult.OK;
      this.findBtn.Location = new Point(130, 54);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(75, 22);
      this.findBtn.TabIndex = 1;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.findTxt.Location = new Point(49, 15);
      this.findTxt.MaxLength = 100;
      this.findTxt.Name = "findTxt";
      this.findTxt.Size = new Size(240, 20);
      this.findTxt.TabIndex = 0;
      this.findTxt.TextChanged += new EventHandler(this.findTxt_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 14);
      this.label1.TabIndex = 9;
      this.label1.Text = "Field";
      this.AcceptButton = (IButtonControl) this.findBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(301, 89);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.findTxt);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.findBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldGoToDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Go to Field";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void findTxt_TextChanged(object sender, EventArgs e)
    {
      if (this.findBtn.Text == "Find Next" && this.findTxt.Text != this.fieldID)
        this.findBtn.Text = "Find";
      if (this.findTxt.Text.Length > 0)
        this.findBtn.Enabled = true;
      else
        this.findBtn.Enabled = false;
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      this.findNext = this.findBtn.Text == "Find Next";
      this.fieldID = this.findTxt.Text.Trim();
    }
  }
}
