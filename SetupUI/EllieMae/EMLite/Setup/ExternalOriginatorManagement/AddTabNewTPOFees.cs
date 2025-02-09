// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.AddTabNewTPOFees
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class AddTabNewTPOFees : Form
  {
    private IContainer components;
    private RadioButton rdbManual;
    private RadioButton rdbSetting;
    private Button btnCancel;
    private Button btnOK;

    public AddTabNewTPOFees() => this.InitializeComponent();

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    public int Value => !this.rdbManual.Checked ? 1 : 0;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddTabNewTPOFees));
      this.rdbManual = new RadioButton();
      this.rdbSetting = new RadioButton();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.rdbManual.AutoSize = true;
      this.rdbManual.Location = new Point(13, 13);
      this.rdbManual.Name = "rdbManual";
      this.rdbManual.Size = new Size(141, 17);
      this.rdbManual.TabIndex = 0;
      this.rdbManual.TabStop = true;
      this.rdbManual.Text = "Create new fee manually";
      this.rdbManual.UseVisualStyleBackColor = true;
      this.rdbSetting.AutoSize = true;
      this.rdbSetting.Location = new Point(13, 46);
      this.rdbSetting.Name = "rdbSetting";
      this.rdbSetting.Size = new Size(174, 17);
      this.rdbSetting.TabIndex = 1;
      this.rdbSetting.TabStop = true;
      this.rdbSetting.Text = "Add fee from existing TPO Fees";
      this.rdbSetting.UseVisualStyleBackColor = true;
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
      this.Controls.Add((Control) this.rdbSetting);
      this.Controls.Add((Control) this.rdbManual);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddTabNewTPOFees);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add New TPO Fee";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
