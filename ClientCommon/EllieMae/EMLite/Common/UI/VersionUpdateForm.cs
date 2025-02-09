// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.VersionUpdateForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class VersionUpdateForm : Form
  {
    private Label label1;
    private Label label3;
    private Button btnUpdate;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private IVersionControl remoteVersionControl;
    private Label lblUpdate;
    private bool updateRequired;
    private VersionUpdateResult updateResult;
    private string sw = Tracing.SwVersionControl;
    private const string className = "VersionUpdateForm";

    public VersionUpdateForm(IVersionControl remoteVersionControl)
    {
      this.InitializeComponent();
      this.remoteVersionControl = remoteVersionControl;
      this.updateRequired = !remoteVersionControl.IsCompatibleWithVersion(VersionControl.CurrentVersion.Version);
      if (this.updateRequired)
        this.lblUpdate.Text = "A new version of the Encompass client application is available for download on the server. In order to continue, you must upgrade your software to this latest version.";
      else
        this.lblUpdate.Text = "A new version of the Encompass client application is available for download on the server. It is recommended that you update your software by pressing \"Update Now,\" but you may continue logging in by pressing Cancel.";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.lblUpdate = new Label();
      this.label3 = new Label();
      this.btnUpdate = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(14, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(146, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Version Update Required";
      this.lblUpdate.Location = new Point(14, 40);
      this.lblUpdate.Name = "lblUpdate";
      this.lblUpdate.Size = new Size(294, 54);
      this.lblUpdate.TabIndex = 1;
      this.lblUpdate.Text = "A new version of the Encompass client application is available for download on the server. In order to run Encompass, you must upgrade your software to this latest version.";
      this.label3.Location = new Point(14, 103);
      this.label3.Name = "label3";
      this.label3.Size = new Size(294, 23);
      this.label3.TabIndex = 2;
      this.label3.Text = "To begin the update, press the \"Update Now\" button.";
      this.btnUpdate.Location = new Point(157, 140);
      this.btnUpdate.Name = "btnUpdate";
      this.btnUpdate.TabIndex = 3;
      this.btnUpdate.Text = "&Update Now";
      this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(233, 140);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AcceptButton = (IButtonControl) this.btnUpdate;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(316, 171);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnUpdate);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.lblUpdate);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (VersionUpdateForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Version Update";
      this.TopMost = true;
      this.ResumeLayout(false);
    }

    public VersionUpdateResult StartUpdate()
    {
      int num = (int) this.ShowDialog();
      return this.UpdateResult;
    }

    public VersionUpdateResult UpdateResult => this.updateResult;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.updateRequired && Utils.Dialog((IWin32Window) this, "You will not be able to continue until this update has been completed.  Cancel anyway?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        this.DialogResult = DialogResult.None;
      }
      else
      {
        this.Log(TraceLevel.Info, "User abort from version update process.");
        this.updateResult = VersionUpdateResult.UpdateAborted;
        this.Close();
      }
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
      try
      {
        this.Log(TraceLevel.Info, "Starting Encompass client update process.");
        string updateFilename = this.retrieveUpdateFromServer();
        if (updateFilename == null)
          return;
        DialogResult dialogResult = DialogResult.Ignore;
        if (Utils.Dialog((IWin32Window) this, "This application must now shut down in order to complete the update installation. Please do not restart Encompass or any related applications until the update has been completed.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
        {
          if (this.updateRequired)
            dialogResult = Utils.Dialog((IWin32Window) this, "You are about to cancel the Encompass update. You will not be able to connect to the Encompass server until the update has been completed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          else
            dialogResult = Utils.Dialog((IWin32Window) this, "You are about to cancel the Encompass update. You will be reminded of this update the next time you log in to this server.", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        else
        {
          this.updateResult = !this.startClientVersionUpdate(updateFilename) ? VersionUpdateResult.UpdateFailed : VersionUpdateResult.UpdateStarted;
          this.Close();
        }
      }
      catch (Exception ex)
      {
        ErrorDialog.Display("An unexpected error occurred while updating the Encompass software.", ex);
      }
    }

    private string retrieveUpdateFromServer()
    {
      this.Log(TraceLevel.Info, "Contacting Encompass Server for update file stream.");
      FileStream updateFileStream = this.remoteVersionControl.GetVersionUpdateFileStream(VersionControl.CurrentVersion.Version);
      string targetPath = SystemUtil.NormalizePath(SystemSettings.UpdatesDir) + this.extractFilename(updateFileStream.Name);
      using (updateFileStream)
      {
        DownloadManager downloadManager = new DownloadManager();
        DownloadResult downloadResult = downloadManager.BeginDownload((Stream) updateFileStream, targetPath);
        int num = (int) downloadManager.ShowDialog((IWin32Window) this);
        return downloadResult.DownloadStatus == DownloadStatus.Complete ? targetPath : (string) null;
      }
    }

    private string extractFilename(string path) => path.Substring(path.LastIndexOf("\\") + 1);

    private bool startClientVersionUpdate(string updateFilename)
    {
      try
      {
        SystemUtil.ExecSystemCmd(Path.Combine(EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory, "VersionControl.exe"), "update client \"" + updateFilename + "\"");
        this.Log(TraceLevel.Info, "Lauched VersionController application for update file \"" + updateFilename + "\".");
        return true;
      }
      catch (Exception ex)
      {
        this.Log(TraceLevel.Error, "Failed to launch VersionController: " + ex.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "An unexpected error occurred while launching the version update process.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private void Log(TraceLevel l, string msg)
    {
      Tracing.Log(this.sw, l, nameof (VersionUpdateForm), msg);
    }
  }
}
