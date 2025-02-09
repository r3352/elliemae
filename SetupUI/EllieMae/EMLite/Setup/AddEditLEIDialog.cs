// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddEditLEIDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddEditLEIDialog : Form, IHelp
  {
    private const string className = "AddEditLEIDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private HMDASettingsForm hmdaSettingsForm;
    private HMDAProfile profile;
    private IContainer components;
    private Panel panelLEI;
    private Panel panel1;
    private Button okBtn;
    private Button cancelBtn;

    public HMDAProfile Profile => this.profile;

    public AddEditLEIDialog(Sessions.Session session, int hmdaProfileId, bool isAddEdit)
    {
      this.InitializeComponent();
      this.session = session;
      if (isAddEdit)
        this.okBtn.Enabled = true;
      else
        this.okBtn.Enabled = false;
      this.hmdaSettingsForm = new HMDASettingsForm(this.session, (SetUpContainer) null, hmdaProfileId);
      this.panelLEI.Controls.Add((Control) this.hmdaSettingsForm);
      this.hmdaSettingsForm.Dock = DockStyle.Fill;
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (AddEditLEIDialog));
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.hmdaSettingsForm.Profile != null)
      {
        this.profile = this.hmdaSettingsForm.Profile;
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an HMDA Profile from the list", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelLEI = new Panel();
      this.panel1 = new Panel();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panelLEI.Dock = DockStyle.Fill;
      this.panelLEI.Location = new Point(0, 0);
      this.panelLEI.Name = "panelLEI";
      this.panelLEI.Size = new Size(1094, 438);
      this.panelLEI.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.okBtn);
      this.panel1.Controls.Add((Control) this.cancelBtn);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 438);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1094, 35);
      this.panel1.TabIndex = 1;
      this.okBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.okBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.okBtn.Location = new Point(932, 6);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(72, 24);
      this.okBtn.TabIndex = 25;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.cancelBtn.Location = new Point(1010, 6);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(72, 24);
      this.cancelBtn.TabIndex = 26;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(1094, 473);
      this.Controls.Add((Control) this.panelLEI);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddEditLEIDialog);
      this.ShowInTaskbar = false;
      this.Text = "HMDA Profiles";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
