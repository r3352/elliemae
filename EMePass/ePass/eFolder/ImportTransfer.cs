// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.eFolder.ImportTransfer
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.eFolder
{
  public class ImportTransfer : Form
  {
    private ComboBox folderCombo;
    private Label label1;
    private Button cancelBtn;
    private Button okBtn;
    private System.ComponentModel.Container components;
    private string transferFile = string.Empty;
    private string targetFolder;

    public ImportTransfer(string transferFile, Sessions.Session session)
    {
      this.InitializeComponent();
      this.transferFile = transferFile;
      string[] allLoanFolderNames = Session.LoanManager.GetAllLoanFolderNames(false);
      string[] foldersForAction = ((LoanFolderRuleManager) session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import);
      if (foldersForAction == null || foldersForAction.Length == 0)
        return;
      List<string> stringList = new List<string>((IEnumerable<string>) foldersForAction);
      for (int index = 0; index < allLoanFolderNames.Length; ++index)
      {
        if (stringList.Contains(allLoanFolderNames[index]))
          this.folderCombo.Items.Add((object) allLoanFolderNames[index]);
      }
      if (stringList.Contains(Session.UserInfo.WorkingFolder))
        this.folderCombo.SelectedItem = (object) Session.UserInfo.WorkingFolder;
      else
        this.folderCombo.SelectedIndex = 0;
    }

    public bool HaveValidFolders => this.folderCombo.Items.Count > 0;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    internal string TargetFolder => this.targetFolder;

    private void InitializeComponent()
    {
      this.folderCombo = new ComboBox();
      this.label1 = new Label();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.SuspendLayout();
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.folderCombo.Location = new Point(12, 32);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(332, 22);
      this.folderCombo.TabIndex = 2;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(284, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Select the loan folder where you want to import the files";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancelBtn.Location = new Point(268, 64);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "Cancel";
      this.okBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.okBtn.Location = new Point(188, 64);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 6;
      this.okBtn.Text = "Import";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(354, 99);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.folderCombo);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportTransfer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Files";
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.folderCombo.SelectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select your target folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.targetFolder = this.folderCombo.SelectedItem.ToString();
        TransferImport transferImport = new TransferImport(this.transferFile, this.targetFolder);
        this.DialogResult = DialogResult.OK;
      }
    }
  }
}
