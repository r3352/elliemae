// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddOrChooseTPODoc
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
  public class AddOrChooseTPODoc : Form
  {
    private IContainer components;
    private RadioButton rdbFromDesktop;
    private RadioButton rdbFromSetting;
    private Button btnCancel;
    private Button btnOK;

    public AddOrChooseTPODoc(bool hasTPODocDisableRight)
    {
      this.InitializeComponent();
      if (!hasTPODocDisableRight)
        return;
      this.rdbFromSetting.Enabled = false;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public int Value => !this.rdbFromDesktop.Checked ? 1 : 0;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddOrChooseTPODoc));
      this.rdbFromDesktop = new RadioButton();
      this.rdbFromSetting = new RadioButton();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.rdbFromDesktop.AutoSize = true;
      this.rdbFromDesktop.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rdbFromDesktop.Location = new Point(13, 13);
      this.rdbFromDesktop.Name = "rdbFromDesktop";
      this.rdbFromDesktop.Size = new Size(181, 17);
      this.rdbFromDesktop.TabIndex = 0;
      this.rdbFromDesktop.TabStop = true;
      this.rdbFromDesktop.Text = "Add document from your desktop";
      this.rdbFromDesktop.UseVisualStyleBackColor = true;
      this.rdbFromSetting.AutoSize = true;
      this.rdbFromSetting.Location = new Point(13, 46);
      this.rdbFromSetting.Name = "rdbFromSetting";
      this.rdbFromSetting.Size = new Size(208, 17);
      this.rdbFromSetting.TabIndex = 1;
      this.rdbFromSetting.TabStop = true;
      this.rdbFromSetting.Text = "Add document from existing TPO Docs";
      this.rdbFromSetting.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(197, 94);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(116, 94);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(284, 129);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdbFromSetting);
      this.Controls.Add((Control) this.rdbFromDesktop);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddOrChooseTPODoc);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Document";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
