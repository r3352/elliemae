// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.PreferencesDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class PreferencesDialog : Form
  {
    private IContainer components;
    private GroupBox groupBox1;
    private CheckBox chkNCMLD;
    private CheckBox chkNMLS;
    private CheckBox chkHMDA;
    private Button btnSave;
    private Button btnCancel;

    public bool HMDAoption => this.chkHMDA.Checked;

    public bool NMLSoption => this.chkNMLS.Checked;

    public bool NCMLDoption => this.chkNCMLD.Checked;

    public bool HMDAInitialState { get; private set; }

    public bool NMLSInitialState { get; private set; }

    public bool NCMLDInitialState { get; private set; }

    public PreferencesDialog()
    {
      this.InitializeComponent();
      if ((Session.ConfigurationManager.GetCompanySetting("HMDA", "HideRDBPopup") ?? "") != "1")
      {
        this.chkHMDA.Checked = true;
        this.HMDAInitialState = true;
      }
      if ((Session.ConfigurationManager.GetCompanySetting("NMLS", "HideRDBPopup") ?? "") != "1")
      {
        this.chkNMLS.Checked = true;
        this.NMLSInitialState = true;
      }
      if (!((Session.ConfigurationManager.GetCompanySetting("NCMLD", "HideRDBPopup") ?? "") != "1"))
        return;
      this.chkNCMLD.Checked = true;
      this.NCMLDInitialState = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.chkNCMLD = new CheckBox();
      this.chkNMLS = new CheckBox();
      this.chkHMDA = new CheckBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.chkNCMLD);
      this.groupBox1.Controls.Add((Control) this.chkNMLS);
      this.groupBox1.Controls.Add((Control) this.chkHMDA);
      this.groupBox1.Location = new Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(268, 101);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Select Options to enable notifications";
      this.chkNCMLD.AutoSize = true;
      this.chkNCMLD.Location = new Point(16, 71);
      this.chkNCMLD.Name = "chkNCMLD";
      this.chkNCMLD.Size = new Size(220, 18);
      this.chkNCMLD.TabIndex = 2;
      this.chkNCMLD.Text = "Enable NCMLD field addition notifications";
      this.chkNCMLD.UseVisualStyleBackColor = true;
      this.chkNMLS.AutoSize = true;
      this.chkNMLS.Location = new Point(17, 47);
      this.chkNMLS.Name = "chkNMLS";
      this.chkNMLS.Size = new Size(213, 18);
      this.chkNMLS.TabIndex = 1;
      this.chkNMLS.Text = "Enable NMLS field addition notifications";
      this.chkNMLS.UseVisualStyleBackColor = true;
      this.chkHMDA.AutoSize = true;
      this.chkHMDA.Location = new Point(17, 23);
      this.chkHMDA.Name = "chkHMDA";
      this.chkHMDA.Size = new Size(214, 18);
      this.chkHMDA.TabIndex = 0;
      this.chkHMDA.Text = "Enable HMDA field addition notifications";
      this.chkHMDA.UseVisualStyleBackColor = true;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(28, 124);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(167, 124);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(288, 156);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.groupBox1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PreferencesDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Preferences";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
