// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.ImportFannie
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class ImportFannie : Form
  {
    private const string className = "ImportFannie";
    private static readonly string sw = Tracing.SwImportExport;
    private System.ComponentModel.Container components;
    private GroupBox groupBox2;
    private Button importBtn;
    private Button cancelBtn;
    private ComboBox folderCombo;
    private Label label2;
    private Label label1;
    private Button selectAllBtn;
    private Label label3;
    private Button browseBtn;
    private TextBox txtTemplate;
    private GroupBox groupBox1;
    private DriveListBox driveListBox;
    private DirListBox dirListBox;
    private FileListBox fileListBox;
    private ImportProgress progressDialog;
    private LoanTemplateSelection loanTemplate;
    private string loanOfficerId = string.Empty;
    private bool useEMLoanNumbering;
    private bool enforceApplicationDate;
    private List<string> failedValidationFieldIds = new List<string>();
    private LoanImportRequirement loanImportRequirement;
    private bool useFannieMae34;

    public event EventHandler LoanImported;

    public ImportFannie(string loanOfficerId)
      : this(loanOfficerId, false)
    {
    }

    public ImportFannie(string loanOfficerId, bool useFannieMae34)
    {
      this.loanOfficerId = loanOfficerId;
      this.useFannieMae34 = useFannieMae34;
      this.InitializeComponent();
      string[] foldersForAction = ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Import);
      int selectedIndex = -1;
      this.folderCombo.Items.AddRange((object[]) LoanFolderUtil.LoanFolderNames2LoanFolderInfos(foldersForAction, Session.UserInfo.WorkingFolder, out selectedIndex));
      if (selectedIndex >= 0)
        this.folderCombo.SelectedIndex = selectedIndex;
      else if (this.folderCombo.Items.Count > 0)
        this.folderCombo.SelectedIndex = 0;
      this.loanImportRequirement = Session.ConfigurationManager.GetLoanImportRequirements();
      if (this.loanImportRequirement.FannieMaeImportRequirementType == LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany && this.loanImportRequirement.TemplateForFannieMaeImport != string.Empty)
      {
        this.loanTemplate = new LoanTemplateSelection(FileSystemEntry.Parse(this.loanImportRequirement.TemplateForFannieMaeImport), true);
        this.txtTemplate.Text = this.loanTemplate.TemplateEntry.Name;
      }
      else
        this.txtTemplate_TextChanged((object) null, (EventArgs) null);
      this.browseBtn.Enabled = this.loanImportRequirement.FannieMaeImportRequirementType != LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.driveListBox = new DriveListBox();
      this.label1 = new Label();
      this.groupBox2 = new GroupBox();
      this.folderCombo = new ComboBox();
      this.label2 = new Label();
      this.importBtn = new Button();
      this.cancelBtn = new Button();
      this.dirListBox = new DirListBox();
      this.fileListBox = new FileListBox();
      this.selectAllBtn = new Button();
      this.txtTemplate = new TextBox();
      this.browseBtn = new Button();
      this.label3 = new Label();
      this.groupBox1 = new GroupBox();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.driveListBox.FormattingEnabled = true;
      this.driveListBox.Location = new Point(8, 24);
      this.driveListBox.Name = "driveListBox";
      this.driveListBox.Size = new Size(248, 21);
      this.driveListBox.TabIndex = 0;
      this.driveListBox.SelectedIndexChanged += new EventHandler(this.driveListBox_SelectedIndexChanged);
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(152, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = this.useFannieMae34 ? "ULAD / iLAD Folder" : "Fannie Mae Folder";
      this.groupBox2.Controls.Add((Control) this.folderCombo);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox2.Location = new Point(12, 348);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(500, 56);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Import To:";
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Location = new Point(105, 24);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(303, 21);
      this.folderCombo.TabIndex = 7;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(25, 24);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Loan Folder:";
      this.importBtn.Location = new Point(348, 472);
      this.importBtn.Name = "importBtn";
      this.importBtn.Size = new Size(75, 24);
      this.importBtn.TabIndex = 8;
      this.importBtn.Text = "&Import";
      this.importBtn.Click += new EventHandler(this.importBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(428, 472);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 9;
      this.cancelBtn.Text = "&Close";
      this.dirListBox.FormattingEnabled = true;
      this.dirListBox.IntegralHeight = false;
      this.dirListBox.Location = new Point(8, 56);
      this.dirListBox.Name = "dirListBox";
      this.dirListBox.Size = new Size(248, 288);
      this.dirListBox.TabIndex = 1;
      this.dirListBox.Change += new EventHandler(this.dirListBox_Change);
      this.fileListBox.FormattingEnabled = true;
      this.fileListBox.Location = new Point(269, 56);
      this.fileListBox.Name = "fileListBox";
      this.fileListBox.Pattern = "*.*";
      this.fileListBox.SelectionMode = SelectionMode.MultiExtended;
      this.fileListBox.Size = new Size(243, 290);
      this.fileListBox.TabIndex = 2;
      this.selectAllBtn.Location = new Point(428, 24);
      this.selectAllBtn.Name = "selectAllBtn";
      this.selectAllBtn.Size = new Size(84, 24);
      this.selectAllBtn.TabIndex = 3;
      this.selectAllBtn.Text = "&Select All";
      this.selectAllBtn.Click += new EventHandler(this.selectAllBtn_Click);
      this.txtTemplate.Location = new Point(117, 432);
      this.txtTemplate.Name = "txtTemplate";
      this.txtTemplate.ReadOnly = true;
      this.txtTemplate.Size = new Size(303, 20);
      this.txtTemplate.TabIndex = 4;
      this.txtTemplate.Tag = (object) "1862";
      this.txtTemplate.TextChanged += new EventHandler(this.txtTemplate_TextChanged);
      this.browseBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.browseBtn.Location = new Point(428, 432);
      this.browseBtn.Name = "browseBtn";
      this.browseBtn.Size = new Size(75, 23);
      this.browseBtn.TabIndex = 5;
      this.browseBtn.Text = "&Browse...";
      this.browseBtn.Click += new EventHandler(this.browseBtn_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(23, 432);
      this.label3.Name = "label3";
      this.label3.Size = new Size(81, 13);
      this.label3.TabIndex = 18;
      this.label3.Text = "Loan Template:";
      this.groupBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupBox1.Location = new Point(12, 408);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(500, 56);
      this.groupBox1.TabIndex = 19;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Apply:";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(522, 503);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.browseBtn);
      this.Controls.Add((Control) this.txtTemplate);
      this.Controls.Add((Control) this.selectAllBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.importBtn);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.driveListBox);
      this.Controls.Add((Control) this.dirListBox);
      this.Controls.Add((Control) this.fileListBox);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportFannie);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import From " + (this.useFannieMae34 ? "ULAD / iLAD" : "Fannie Mae");
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void driveListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.dirListBox.Path = this.driveListBox.Drive;
      }
      catch (IOException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Device " + this.driveListBox.Drive + " unavailable.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        string path = this.dirListBox.Path;
        this.driveListBox.Drive = this.dirListBox.Path.Substring(0, this.dirListBox.Path.IndexOf("\\"));
        this.dirListBox.Path = path;
      }
    }

    private void dirListBox_Change(object sender, EventArgs e)
    {
      this.fileListBox.Path = this.dirListBox.Path;
    }

    private void selectAllBtn_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.fileListBox.Items.Count; ++index)
        this.fileListBox.SetSelected(index, true);
    }

    private void importBtn_Click(object sender, EventArgs e)
    {
      ContactUtil.InitContactUtil();
      if (this.folderCombo.SelectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to choose a destination folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.fileListBox.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select at least one file in order to import.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.loanTemplate != null && this.loanTemplate.TemplateEntry != null && !Session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.LoanTemplate, this.loanTemplate.TemplateEntry))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The loan template is not available." + (this.browseBtn.Enabled ? "" : " Please check administrator to verify the selected loan template in Loan Import Requirement settings."), MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string path = this.dirListBox.Path;
        string name = ((LoanFolderInfo) this.folderCombo.SelectedItem).Name;
        string[] fileNames = new string[this.fileListBox.SelectedItems.Count];
        int num4 = 0;
        foreach (object selectedItem in this.fileListBox.SelectedItems)
          fileNames[num4++] = selectedItem.ToString();
        this.progressDialog = new ImportProgress(this.fileListBox.SelectedItems.Count);
        this.progressDialog.progressLbl.Text = this.useFannieMae34 ? "ULAD / iLAD (MISMO 3.4)" : "Fannie Mae";
        try
        {
          this.useEMLoanNumbering = (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.LoanNumbering") == EnableDisableSetting.Enabled;
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportFannie.sw, TraceLevel.Error, nameof (ImportFannie), "Problem reading system setting \"Import.LoanNumbering\", exception: " + ex.Message);
        }
        try
        {
          this.enforceApplicationDate = (EnableDisableSetting) Session.ServerManager.GetServerSetting("Import.EnforceApplicationDate") == EnableDisableSetting.Enabled;
        }
        catch (Exception ex)
        {
          Tracing.Log(ImportFannie.sw, TraceLevel.Error, nameof (ImportFannie), "Problem reading system setting \"Import.EnforceApplicationDate\", exception: " + ex.Message);
        }
        ImportFannie.FannieImportParameters parameter = new ImportFannie.FannieImportParameters(path, name, fileNames, this.useFannieMae34);
        Thread thread = new Thread(new ParameterizedThreadStart(this.ImportThread));
        thread.Start((object) parameter);
        Session.ISession.LoanImportInProgress = true;
        if (DialogResult.Cancel == this.progressDialog.ShowDialog((IWin32Window) this))
          thread.Abort();
        Session.ISession.LoanImportInProgress = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void ImportThread(object inputParameter)
    {
      ImportFannie.FannieImportParameters importParameters = inputParameter as ImportFannie.FannieImportParameters;
      long importedCount = 0;
      if (importParameters != null)
      {
        foreach (string fileName in importParameters.FileNames)
        {
          this.updateProgress(fileName);
          LoanDataMgr sender = (LoanDataMgr) null;
          try
          {
            sender = new FannieImport(this.loanTemplate, importParameters.UseFannieMae34)
            {
              UseEMLoanNumbering = this.useEMLoanNumbering,
              EnforceApplicationDate = this.enforceApplicationDate
            }.Convert(importParameters.FannieFolder + "\\" + fileName, Session.SessionObjects, this.loanOfficerId);
            if (sender != null)
            {
              sender.FromLoanImport = true;
              if (this.LoanImported != null)
                this.LoanImported((object) sender, new EventArgs());
              sender.LoanName = sender.LoanData.GUID;
              sender.LoanFolder = importParameters.EmliteFolder;
              if (sender.Save(false, true, false, true))
                ++importedCount;
              if (sender.FailedValidationListFieldIds.Count > 0)
                this.failedValidationFieldIds = sender.FailedValidationListFieldIds;
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(ImportFannie.sw, TraceLevel.Error, nameof (ImportFannie), "Problem importing Fannie Mae file:  " + fileName + ", exception: " + ex.Message + "\r\n");
            Tracing.Log(ImportFannie.sw, TraceLevel.Error, nameof (ImportFannie), ex.StackTrace + "\r\n");
          }
          finally
          {
            sender?.Close();
          }
        }
      }
      this.closeProgress(importedCount);
    }

    private void updateProgress(string fileName)
    {
      if (this.progressDialog.InvokeRequired)
        this.Invoke((Delegate) new ImportFannie.UpdateProgressCallback(this.updateProgress), (object) fileName);
      else
        this.progressDialog.CurrentFile = fileName;
    }

    private void closeProgress(long importedCount)
    {
      if (this.progressDialog.InvokeRequired)
      {
        this.Invoke((Delegate) new ImportFannie.CloseProgressCallback(this.closeProgress), (object) importedCount);
      }
      else
      {
        string text = importedCount.ToString() + " out of " + (object) this.fileListBox.SelectedItems.Count + " file(s) were imported.";
        if (importedCount < (long) this.fileListBox.SelectedItems.Count)
        {
          if (DialogResult.Yes == Utils.Dialog((IWin32Window) this.progressDialog, text + Environment.NewLine + "Would you like to open the Log File to view the log of files that were not imported?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            SystemUtil.ShellExecute(Tracing.LogFile);
        }
        else
        {
          if (this.failedValidationFieldIds.Count > 0)
          {
            string str = text + Environment.NewLine + "However field(s)";
            for (int index = 1; index <= this.failedValidationFieldIds.Count; ++index)
            {
              str += this.failedValidationFieldIds[index - 1];
              if (index != this.failedValidationFieldIds.Count)
                str += ", ";
            }
            text = str + " was not able to be imported because your field data entry rules prevent it.";
          }
          int num = (int) Utils.Dialog((IWin32Window) this.progressDialog, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        this.progressDialog.Close();
      }
    }

    private void browseBtn_Click(object sender, EventArgs e)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      using (LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(Session.DefaultInstance, false, aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank), aclManager.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl)))
      {
        templateSelectDialog.DisableAppendDataCheckbox();
        if (templateSelectDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FileSystemEntry selectedItem = templateSelectDialog.SelectedItem;
        if (selectedItem != null)
        {
          this.loanTemplate = new LoanTemplateSelection(selectedItem, templateSelectDialog.AppendData);
          this.txtTemplate.Text = this.loanTemplate.TemplateEntry.Name;
        }
        else
        {
          this.loanTemplate = (LoanTemplateSelection) null;
          this.txtTemplate.Text = "";
        }
      }
    }

    private void txtTemplate_TextChanged(object sender, EventArgs e)
    {
      this.importBtn.Enabled = this.loanImportRequirement.FannieMaeImportRequirementType == LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired || this.loanImportRequirement.FannieMaeImportRequirementType != LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired && this.txtTemplate.Text.Trim() != string.Empty;
    }

    private delegate void UpdateProgressCallback(string text);

    private delegate void CloseProgressCallback(long importedCount);

    public class FannieImportParameters
    {
      private string fannieFolder = string.Empty;
      private string emliteFolder = string.Empty;
      private string[] fileNames;
      private bool useFannieMae34;

      public string FannieFolder => this.fannieFolder;

      public string EmliteFolder => this.emliteFolder;

      public string[] FileNames => this.fileNames;

      public bool UseFannieMae34 => this.useFannieMae34;

      public FannieImportParameters(string fannieFolder, string emliteFolder, string[] fileNames)
        : this(fannieFolder, emliteFolder, fileNames, false)
      {
      }

      public FannieImportParameters(
        string fannieFolder,
        string emliteFolder,
        string[] fileNames,
        bool useFannieMae34)
      {
        this.fannieFolder = fannieFolder;
        this.emliteFolder = emliteFolder;
        this.fileNames = fileNames;
        this.useFannieMae34 = useFannieMae34;
      }
    }
  }
}
