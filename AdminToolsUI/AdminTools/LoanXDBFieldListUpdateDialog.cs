// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanXDBFieldListUpdateDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanXDBFieldListUpdateDialog : Form
  {
    private static IProgressFeedback progressFeedback;
    private static string errMessage;
    private IContainer components;
    private Label label1;
    private Button btnYes;
    private Label label2;
    private Button btnCancel;
    private Button btnNo;

    public LoanXDBFieldListUpdateDialog() => this.InitializeComponent();

    public static bool VerifyServerFieldList(IWin32Window owner)
    {
      if (Session.StartupInfo.RuntimeEnvironment != RuntimeEnvironment.Default || StandardFields.Instance.FileVersion.CompareTo(Session.ServerManager.GetEncompassFieldListVersion()) <= 0)
        return true;
      switch (Utils.Dialog(owner, "The field list on your Encompass Server is out-of-date. Encompass fields that were added due to a recent update may be unavailable to include in your Reporting Database until the server's field list is updated." + Environment.NewLine + Environment.NewLine + "Would you like to update the server's field list now?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          return true;
        default:
          return LoanXDBFieldListUpdateDialog.updateServerFieldList(owner) == DialogResult.OK;
      }
    }

    private static DialogResult updateServerFieldList(IWin32Window owner)
    {
      LoanXDBFieldListUpdateDialog.errMessage = (string) null;
      using (ProgressDialog progressDialog = new ProgressDialog("Encompass", new AsynchronousProcess(LoanXDBFieldListUpdateDialog.uploadFieldList), (object) null, true))
      {
        DialogResult dialogResult = progressDialog.ShowDialog(owner);
        switch (dialogResult)
        {
          case DialogResult.OK:
            int num1 = (int) Utils.Dialog(owner, "The server's field list has been updated successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          case DialogResult.Abort:
            int num2 = (int) Utils.Dialog(owner, "An error occurred while attempting to update the field list: " + LoanXDBFieldListUpdateDialog.errMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
        }
        return dialogResult;
      }
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      if (LoanXDBFieldListUpdateDialog.updateServerFieldList((IWin32Window) this) != DialogResult.OK)
        return;
      this.DialogResult = DialogResult.Yes;
    }

    private static DialogResult uploadFieldList(object notUsed, IProgressFeedback feedback)
    {
      try
      {
        LoanXDBFieldListUpdateDialog.progressFeedback = feedback;
        FileInfo fileInfo = new FileInfo(StandardFields.MapFilePath);
        using (BinaryObject data = new BinaryObject(StandardFields.MapFilePath))
        {
          data.UploadProgress += new DownloadProgressEventHandler(LoanXDBFieldListUpdateDialog.onUploadProgress);
          feedback.Status = "Publishing Field Definitions...";
          feedback.ResetCounter(100);
          Session.ServerManager.ReplaceEncompassFieldList(data, fileInfo.LastWriteTimeUtc, true);
          return DialogResult.OK;
        }
      }
      catch (CanceledOperationException ex)
      {
        return DialogResult.Cancel;
      }
      catch (Exception ex)
      {
        LoanXDBFieldListUpdateDialog.errMessage = ex.ToString();
        return DialogResult.Abort;
      }
    }

    private static void onUploadProgress(object source, DownloadProgressEventArgs e)
    {
      if (LoanXDBFieldListUpdateDialog.progressFeedback == null)
        return;
      LoanXDBFieldListUpdateDialog.progressFeedback.Value = (int) ((double) e.PercentComplete * 100.0);
      if (!LoanXDBFieldListUpdateDialog.progressFeedback.Cancel)
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanXDBFieldListUpdateDialog));
      this.label1 = new Label();
      this.btnYes = new Button();
      this.label2 = new Label();
      this.btnCancel = new Button();
      this.btnNo = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(14, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(486, 50);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.btnYes.Location = new Point(135, 108);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "&Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.label2.Location = new Point(14, 68);
      this.label2.Name = "label2";
      this.label2.Size = new Size(486, 20);
      this.label2.TabIndex = 2;
      this.label2.Text = "Would you like to update the server's field list now?";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(301, 108);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(218, 108);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 4;
      this.btnNo.Text = "&No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(511, 146);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanXDBFieldListUpdateDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass";
      this.ResumeLayout(false);
    }
  }
}
