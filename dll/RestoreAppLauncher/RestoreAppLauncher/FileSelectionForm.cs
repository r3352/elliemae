// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.FileSelectionForm
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace RestoreAppLauncher
{
  public class FileSelectionForm : Form
  {
    private IContainer components;
    private CheckedListBox chkLstBoxFiles;
    private Button btnCancel;
    private Button btnRestore;
    private Label lblInstallDir;
    private CheckBox chkBoxUseCurrentDir;
    private TextBox txtBoxInstallDir;

    public FileSelectionForm(string[] initFiles, bool useCurrentDirAsInstallDir)
    {
      this.InitializeComponent();
      if (useCurrentDirAsInstallDir)
      {
        this.chkBoxUseCurrentDir.Checked = true;
        this.txtBoxInstallDir.Text = Application.StartupPath;
      }
      else
      {
        this.chkBoxUseCurrentDir.Checked = false;
        this.txtBoxInstallDir.Text = SCUtil.GetInstallDirFromUacFolder();
      }
      this.chkLstBoxFiles.Items.Clear();
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (string allFile in Consts.AllFiles)
      {
        string str = (string) null;
        if (Consts.EncFileMapping.ContainsKey(allFile))
          str = Consts.EncFileMapping[allFile];
        else if (Consts.SCAppMgrFileMapping.ContainsKey(allFile))
          str = Consts.SCAppMgrFileMapping[allFile];
        int index = this.chkLstBoxFiles.Items.Add((object) str);
        foreach (string initFile in initFiles)
        {
          if (string.Compare(initFile, allFile, true) == 0 || string.Compare(initFile, "AsmResolver.dll", true) == 0 && str == "EllieMae.Encompass.AsmResolver.dll")
          {
            this.chkLstBoxFiles.SetItemCheckState(index, CheckState.Checked);
            dictionary.Add(allFile, allFile);
            break;
          }
        }
      }
      foreach (string initFile in initFiles)
      {
        if (!dictionary.ContainsKey(initFile))
          this.chkLstBoxFiles.SetItemCheckState(this.chkLstBoxFiles.Items.Add((object) initFile), CheckState.Checked);
      }
    }

    public string[] SelectedEncFiles
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (string checkedItem in this.chkLstBoxFiles.CheckedItems)
        {
          if (checkedItem.IndexOf("SCAppMgr", StringComparison.OrdinalIgnoreCase) != 0)
            stringList.Add(checkedItem);
        }
        return stringList.ToArray();
      }
    }

    public string[] SelectedSCAppFiles
    {
      get
      {
        List<string> stringList = new List<string>();
        foreach (string checkedItem in this.chkLstBoxFiles.CheckedItems)
        {
          if (checkedItem.IndexOf("SCAppMgr", StringComparison.OrdinalIgnoreCase) == 0)
            stringList.Add(checkedItem);
        }
        return stringList.ToArray();
      }
    }

    public bool UseCurrentDirAsInstallDir => this.chkBoxUseCurrentDir.Checked;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnRestore_Click(object sender, EventArgs e)
    {
      if (this.chkLstBoxFiles.CheckedItems.Count == 0)
      {
        int num = (int) MessageBox.Show("Please select at least one file to restore.", Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.DialogResult = DialogResult.None;
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void chkBoxUseCurrentDir_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkBoxUseCurrentDir.Checked)
        this.txtBoxInstallDir.Text = Application.StartupPath;
      else
        this.txtBoxInstallDir.Text = SCUtil.GetInstallDirFromUacFolder();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkLstBoxFiles = new CheckedListBox();
      this.btnCancel = new Button();
      this.btnRestore = new Button();
      this.lblInstallDir = new Label();
      this.chkBoxUseCurrentDir = new CheckBox();
      this.txtBoxInstallDir = new TextBox();
      this.SuspendLayout();
      this.chkLstBoxFiles.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.chkLstBoxFiles.CheckOnClick = true;
      this.chkLstBoxFiles.FormattingEnabled = true;
      this.chkLstBoxFiles.Location = new Point(0, 0);
      this.chkLstBoxFiles.Name = "chkLstBoxFiles";
      this.chkLstBoxFiles.Size = new Size(335, 334);
      this.chkLstBoxFiles.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(250, 374);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnRestore.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRestore.Location = new Point(169, 374);
      this.btnRestore.Name = "btnRestore";
      this.btnRestore.Size = new Size(75, 23);
      this.btnRestore.TabIndex = 0;
      this.btnRestore.Text = "Restore";
      this.btnRestore.UseVisualStyleBackColor = true;
      this.btnRestore.Click += new EventHandler(this.btnRestore_Click);
      this.lblInstallDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblInstallDir.AutoSize = true;
      this.lblInstallDir.Location = new Point(12, 346);
      this.lblInstallDir.Name = "lblInstallDir";
      this.lblInstallDir.Size = new Size(47, 13);
      this.lblInstallDir.TabIndex = 2;
      this.lblInstallDir.Text = "InstallDir";
      this.chkBoxUseCurrentDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkBoxUseCurrentDir.AutoSize = true;
      this.chkBoxUseCurrentDir.Location = new Point(12, 378);
      this.chkBoxUseCurrentDir.Name = "chkBoxUseCurrentDir";
      this.chkBoxUseCurrentDir.Size = new Size(152, 17);
      this.chkBoxUseCurrentDir.TabIndex = 3;
      this.chkBoxUseCurrentDir.Text = "Use current dir as InstallDir";
      this.chkBoxUseCurrentDir.UseVisualStyleBackColor = true;
      this.chkBoxUseCurrentDir.CheckedChanged += new EventHandler(this.chkBoxUseCurrentDir_CheckedChanged);
      this.txtBoxInstallDir.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxInstallDir.Location = new Point(65, 343);
      this.txtBoxInstallDir.Name = "txtBoxInstallDir";
      this.txtBoxInstallDir.ReadOnly = true;
      this.txtBoxInstallDir.Size = new Size(260, 20);
      this.txtBoxInstallDir.TabIndex = 4;
      this.AcceptButton = (IButtonControl) this.btnRestore;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(337, 404);
      this.Controls.Add((Control) this.txtBoxInstallDir);
      this.Controls.Add((Control) this.chkBoxUseCurrentDir);
      this.Controls.Add((Control) this.lblInstallDir);
      this.Controls.Add((Control) this.btnRestore);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.chkLstBoxFiles);
      this.Name = nameof (FileSelectionForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Files to Restore";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
