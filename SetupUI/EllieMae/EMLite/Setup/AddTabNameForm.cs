// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddTabNameForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddTabNameForm : Form
  {
    private string currentTabName = string.Empty;
    private Sessions.Session session;
    private IContainer components;
    private Button addBtn;
    private Button okBtn;
    private TextBox textBoxName;
    private Label label1;

    public AddTabNameForm(Sessions.Session session, string currentTabName)
    {
      this.session = session;
      this.InitializeComponent();
      this.currentTabName = currentTabName;
      this.textBoxName.Text = this.currentTabName;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Tab name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.textBoxName.Text.Trim().ToLower() == "general")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The Conventional MI tables already contains this Tab Name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.currentTabName != this.textBoxName.Text.Trim() && this.session.ConfigurationManager.HadDuplicateMITab(this.textBoxName.Text.Trim()))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The Conventional MI tables already contains this Tab Name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public string NewTabName => this.textBoxName.Text.Trim();

    private void AddTabNameForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.addBtn.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.addBtn = new Button();
      this.okBtn = new Button();
      this.textBoxName = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.addBtn.DialogResult = DialogResult.Cancel;
      this.addBtn.Location = new Point(240, 59);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(75, 23);
      this.addBtn.TabIndex = 2;
      this.addBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(159, 59);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 1;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.textBoxName.Location = new Point(78, 17);
      this.textBoxName.MaxLength = 30;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(237, 20);
      this.textBoxName.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(57, 13);
      this.label1.TabIndex = 21;
      this.label1.Text = "Tab Name";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(332, 94);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.addBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddTabNameForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add/Edit Tab Name";
      this.KeyDown += new KeyEventHandler(this.AddTabNameForm_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
