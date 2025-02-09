// Decompiled with JetBrains decompiler
// Type: RemoveUAC.SelectionForm
// Assembly: RemoveUAC, Version=4.5.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 77B208E8-E0D8-4A0C-958C-E5CF190AB691
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RemoveUAC.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace RemoveUAC
{
  public class SelectionForm : Form
  {
    private string uacHashFolder;
    private string appDataHashFolder;
    private IContainer components;
    private CheckBox chkBoxUAC;
    private CheckBox chkBoxInstallDir;
    private CheckBox chkBoxConfigFiles;
    private CheckBox chkBoxRegistry;
    private Label label1;
    private Button btnRemove;
    private Button btnCancel;
    private Button btnOpenUAC;
    private Button btnOpenEncData;

    public SelectionForm(
      string uacHashFolder,
      string appDataHashFolder,
      string appStartupPath,
      string regKeyPath)
    {
      this.InitializeComponent();
      this.uacHashFolder = uacHashFolder;
      this.appDataHashFolder = appDataHashFolder;
      this.chkBoxUAC.Text = "UAC cache folder '" + uacHashFolder + "' (safe).";
      this.chkBoxInstallDir.Text = "Files under folder '" + appStartupPath + "'.";
      this.chkBoxConfigFiles.Text = "Configuration files under folder '" + appStartupPath + "'.";
      this.chkBoxRegistry.Text = "Registry key '" + regKeyPath + "' (safe).";
      int num = 0;
      string[] files = Directory.GetFiles(appStartupPath);
      if (files != null)
      {
        foreach (string path in files)
        {
          string lower = Path.GetFileName(path).Trim().ToLower();
          if (lower == "applauncher.exe" || lower == "elliemae.encompass.asmresolver.dll")
            ++num;
        }
      }
      if (num >= 2)
        return;
      this.chkBoxInstallDir.Checked = this.chkBoxInstallDir.Enabled = false;
      this.chkBoxConfigFiles.Checked = this.chkBoxConfigFiles.Enabled = false;
      this.btnOpenEncData.Visible = false;
    }

    public bool RemoveUAC => this.chkBoxUAC.Checked;

    public bool RemoveInstallDir => this.chkBoxInstallDir.Checked;

    public bool RemoveConfigFiles => this.chkBoxConfigFiles.Checked;

    public bool RemoveRegistry => this.chkBoxRegistry.Checked;

    private void chkBoxInstallDir_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.chkBoxInstallDir.Checked)
      {
        this.chkBoxConfigFiles.Enabled = false;
        this.chkBoxConfigFiles.Checked = false;
      }
      else
        this.chkBoxConfigFiles.Enabled = true;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (!this.RemoveUAC && !this.RemoveInstallDir && !this.RemoveConfigFiles && !this.RemoveRegistry || MessageBox.Show("Are you sure you want to remove the selected components?", "Smart Client", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.No)
        return;
      this.DialogResult = DialogResult.None;
    }

    private void btnOpenUAC_Click(object sender, EventArgs e)
    {
      try
      {
        new Process()
        {
          StartInfo = {
            FileName = this.uacHashFolder
          }
        }.Start();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnOpenEncData_Click(object sender, EventArgs e)
    {
      string path1 = this.appDataHashFolder;
      if (Directory.Exists(Path.Combine(path1, "EncompassData")))
        path1 = Path.Combine(path1, "EncompassData");
      try
      {
        new Process() { StartInfo = { FileName = path1 } }.Start();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectionForm));
      this.chkBoxUAC = new CheckBox();
      this.chkBoxInstallDir = new CheckBox();
      this.chkBoxConfigFiles = new CheckBox();
      this.chkBoxRegistry = new CheckBox();
      this.label1 = new Label();
      this.btnRemove = new Button();
      this.btnCancel = new Button();
      this.btnOpenUAC = new Button();
      this.btnOpenEncData = new Button();
      this.SuspendLayout();
      this.chkBoxUAC.AutoSize = true;
      this.chkBoxUAC.Checked = true;
      this.chkBoxUAC.CheckState = CheckState.Checked;
      this.chkBoxUAC.Location = new Point(16, 40);
      this.chkBoxUAC.Name = "chkBoxUAC";
      this.chkBoxUAC.Size = new Size(60, 17);
      this.chkBoxUAC.TabIndex = 0;
      this.chkBoxUAC.Text = "<UAC>";
      this.chkBoxUAC.UseVisualStyleBackColor = true;
      this.chkBoxInstallDir.AutoSize = true;
      this.chkBoxInstallDir.Checked = true;
      this.chkBoxInstallDir.CheckState = CheckState.Checked;
      this.chkBoxInstallDir.Location = new Point(16, 63);
      this.chkBoxInstallDir.Name = "chkBoxInstallDir";
      this.chkBoxInstallDir.Size = new Size(78, 17);
      this.chkBoxInstallDir.TabIndex = 1;
      this.chkBoxInstallDir.Text = "<InstallDir>";
      this.chkBoxInstallDir.UseVisualStyleBackColor = true;
      this.chkBoxInstallDir.CheckedChanged += new EventHandler(this.chkBoxInstallDir_CheckedChanged);
      this.chkBoxConfigFiles.AutoSize = true;
      this.chkBoxConfigFiles.Location = new Point(34, 86);
      this.chkBoxConfigFiles.Name = "chkBoxConfigFiles";
      this.chkBoxConfigFiles.Size = new Size(92, 17);
      this.chkBoxConfigFiles.TabIndex = 2;
      this.chkBoxConfigFiles.Text = "<Config Files>";
      this.chkBoxConfigFiles.UseVisualStyleBackColor = true;
      this.chkBoxRegistry.AutoSize = true;
      this.chkBoxRegistry.Checked = true;
      this.chkBoxRegistry.CheckState = CheckState.Checked;
      this.chkBoxRegistry.Location = new Point(16, 109);
      this.chkBoxRegistry.Name = "chkBoxRegistry";
      this.chkBoxRegistry.Size = new Size(76, 17);
      this.chkBoxRegistry.TabIndex = 3;
      this.chkBoxRegistry.Text = "<Registry>";
      this.chkBoxRegistry.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(151, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Select components to remove:";
      this.btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemove.DialogResult = DialogResult.OK;
      this.btnRemove.Location = new Point(447, 143);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(75, 23);
      this.btnRemove.TabIndex = 5;
      this.btnRemove.Text = "Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(528, 143);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOpenUAC.Location = new Point(16, 143);
      this.btnOpenUAC.Name = "btnOpenUAC";
      this.btnOpenUAC.Size = new Size((int) sbyte.MaxValue, 23);
      this.btnOpenUAC.TabIndex = 7;
      this.btnOpenUAC.Text = "Open UAC Folder";
      this.btnOpenUAC.UseVisualStyleBackColor = true;
      this.btnOpenUAC.Click += new EventHandler(this.btnOpenUAC_Click);
      this.btnOpenEncData.Location = new Point(149, 143);
      this.btnOpenEncData.Name = "btnOpenEncData";
      this.btnOpenEncData.Size = new Size(162, 23);
      this.btnOpenEncData.TabIndex = 8;
      this.btnOpenEncData.Text = "Open EncompassData Folder";
      this.btnOpenEncData.UseVisualStyleBackColor = true;
      this.btnOpenEncData.Click += new EventHandler(this.btnOpenEncData_Click);
      this.AcceptButton = (IButtonControl) this.btnRemove;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(626, 178);
      this.Controls.Add((Control) this.btnOpenEncData);
      this.Controls.Add((Control) this.btnOpenUAC);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkBoxRegistry);
      this.Controls.Add((Control) this.chkBoxConfigFiles);
      this.Controls.Add((Control) this.chkBoxInstallDir);
      this.Controls.Add((Control) this.chkBoxUAC);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SelectionForm);
      this.ShowIcon = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Smart Client";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
