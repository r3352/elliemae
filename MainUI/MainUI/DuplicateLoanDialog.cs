// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.DuplicateLoanDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class DuplicateLoanDialog : Form
  {
    private FeaturesAclManager featMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
    private bool isAdministrator = Session.UserInfo.IsSuperAdministrator() || Session.UserInfo.IsAdministrator();
    private bool isBrokerEdition = Session.EncompassEdition == EncompassEdition.Broker;
    private bool hasDuplicateBlankLoanAccess;
    private readonly LoanFolderInfo _defaultFolder;
    private readonly LoanFolderInfo[] _defaultAllowedFolders;
    private LoanFolderInfo[] _piggybackAllowedFolders;
    private IContainer components;
    private Label label1;
    private RadioButton radDuplicate;
    private DialogButtons dialogButtons1;
    private RadioButton radDuplicateAsSecond;
    private Label label2;
    private ComboBox cboFolder;
    private Label labelDupTemplate;
    private ComboBox cboTemplates;
    private RadioButton radPiggyback;

    public DuplicateLoanDialog(LoanFolderInfo[] allowedFolders, LoanFolderInfo defaultFolder)
    {
      this._defaultAllowedFolders = allowedFolders;
      this._defaultFolder = defaultFolder;
      this.InitializeComponent();
      this.hasDuplicateBlankLoanAccess = this.featMgr.GetPermission(AclFeature.LoanMgmt_Duplicate_Blank, Session.UserInfo.GetPersonaIDs());
      if (this.isBrokerEdition)
        this.dialogButtons1.OKButton.Enabled = true;
      else if (!this.hasDuplicateBlankLoanAccess && this.cboTemplates.SelectedIndex == -1)
        this.dialogButtons1.OKButton.Enabled = false;
      if (!Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_Duplicate))
      {
        this.radDuplicate.Enabled = false;
        this.radDuplicateAsSecond.Checked = true;
      }
      else
        this.loadLoanDuplicateTemplates();
      if (!Session.ACL.IsAuthorizedForFeature(AclFeature.LoanMgmt_Duplicate_For_Second))
      {
        this.radDuplicateAsSecond.Enabled = false;
        this.radDuplicate.Checked = true;
      }
      this.radioButtons_Click((object) null, (EventArgs) null);
      if (!this.isBrokerEdition)
        return;
      this.cboTemplates.Visible = false;
      this.labelDupTemplate.Visible = false;
      this.Height = this.cboTemplates.Top + this.cboTemplates.Height + this.dialogButtons1.Height;
    }

    private void loadLoanDuplicateTemplates()
    {
      if (this.isBrokerEdition)
        return;
      LoanDuplicationTemplateAclInfo[] duplicationTemplateAclInfoArray = (LoanDuplicationTemplateAclInfo[]) null;
      if (!this.isAdministrator)
        duplicationTemplateAclInfoArray = ((LoanDuplicationAclManager) Session.ACL.GetAclManager(AclCategory.LoanDuplicationTemplates)).GetAccessibleLoanDuplicationTemplates(Session.UserInfo.GetPersonaIDs());
      this.cboTemplates.Items.Clear();
      if (!this.isAdministrator && (duplicationTemplateAclInfoArray == null || duplicationTemplateAclInfoArray.Length == 0))
        return;
      List<string> stringList = (List<string>) null;
      if (!this.isAdministrator)
      {
        stringList = new List<string>();
        for (int index = 0; index < duplicationTemplateAclInfoArray.Length; ++index)
          stringList.Add(duplicationTemplateAclInfoArray[index].TemplateName);
      }
      this.cboTemplates.BeginUpdate();
      try
      {
        if (this.isAdministrator || !this.hasDuplicateBlankLoanAccess)
          this.cboTemplates.Items.Add((object) "");
        FileSystemEntry[] templateDirEntries = Session.ConfigurationManager.GetFilteredTemplateDirEntries(TemplateSettingsType.LoanDuplicationTemplate, new FileSystemEntry("\\", "", FileSystemEntry.Types.File, (string) null));
        for (int index = 0; index < templateDirEntries.Length; ++index)
        {
          if (stringList == null || stringList.Contains(templateDirEntries[index].Name))
            this.cboTemplates.Items.Add((object) templateDirEntries[index].Name);
        }
      }
      catch (Exception ex)
      {
      }
      this.cboTemplates.EndUpdate();
    }

    public bool DuplicateAsSecond => this.radDuplicateAsSecond.Checked;

    public bool DuplicateAsPiggyback => this.radPiggyback.Checked;

    public string SelectedFolder => ((LoanFolderInfo) this.cboFolder.SelectedItem).Name.ToString();

    public string SelectedTemplate
    {
      get
      {
        return this.cboTemplates.SelectedItem == null ? "" : this.cboTemplates.SelectedItem.ToString();
      }
    }

    private void loadFolderList(LoanFolderInfo[] allowedFolders)
    {
      this.cboFolder.Items.AddRange((object[]) allowedFolders);
    }

    private void radioButtons_Click(object sender, EventArgs e)
    {
      if (this.radPiggyback.Checked)
        this.SetPiggybackAllowedFolders();
      else
        this.SetDefaultAllowedFolders();
      if (this.radDuplicate.Checked && this.radDuplicate.Enabled)
      {
        this.cboTemplates.Enabled = true;
        if (this.isBrokerEdition)
          this.dialogButtons1.OKButton.Enabled = true;
        else if (this.hasDuplicateBlankLoanAccess && !this.isAdministrator)
        {
          this.dialogButtons1.OKButton.Enabled = false;
        }
        else
        {
          if (this.cboTemplates.Items.Count <= 0)
            return;
          this.cboTemplates.SelectedIndex = 0;
        }
      }
      else
      {
        this.cboTemplates.Enabled = false;
        this.cboTemplates.SelectedItem = (object) null;
        this.dialogButtons1.OKButton.Enabled = true;
      }
    }

    private void SetPiggybackAllowedFolders()
    {
      this.cboFolder.Items.Clear();
      if (this._piggybackAllowedFolders == null)
      {
        List<string> stringList = new List<string>((IEnumerable<string>) ((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Piggyback));
        if (stringList.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to piggyback loans into any folder.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        int selectedIndex = -1;
        this._piggybackAllowedFolders = LoanFolderUtil.LoanFolderNames2LoanFolderInfos(stringList.ToArray(), "", out selectedIndex);
      }
      if (this._piggybackAllowedFolders == null || this._piggybackAllowedFolders.Length == 0)
        return;
      this.loadFolderList(this._piggybackAllowedFolders);
      ClientCommonUtils.PopulateLoanFolderDropdown(this.cboFolder, this._piggybackAllowedFolders[0], false);
    }

    private void SetDefaultAllowedFolders()
    {
      this.cboFolder.Items.Clear();
      if (this._defaultAllowedFolders == null || this._defaultAllowedFolders.Length == 0)
        return;
      this.loadFolderList(this._defaultAllowedFolders);
      ClientCommonUtils.PopulateLoanFolderDropdown(this.cboFolder, this._defaultFolder, false);
      if (this.cboFolder.SelectedIndex >= 0)
        return;
      this.cboFolder.SelectedIndex = 0;
    }

    private void cboTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.radDuplicate.Checked || !this.radDuplicate.Enabled)
        return;
      if (this.cboTemplates.SelectedIndex != -1)
      {
        if (!this.isAdministrator)
        {
          if (this.cboTemplates.SelectedItem.ToString().Equals("") && this.hasDuplicateBlankLoanAccess)
            this.dialogButtons1.OKButton.Enabled = false;
          else
            this.dialogButtons1.OKButton.Enabled = true;
        }
        else
          this.dialogButtons1.OKButton.Enabled = true;
      }
      if (!this.isBrokerEdition)
        return;
      this.dialogButtons1.OKButton.Enabled = true;
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
      this.radDuplicate = new RadioButton();
      this.dialogButtons1 = new DialogButtons();
      this.radDuplicateAsSecond = new RadioButton();
      this.label2 = new Label();
      this.cboFolder = new ComboBox();
      this.labelDupTemplate = new Label();
      this.cboTemplates = new ComboBox();
      this.radPiggyback = new RadioButton();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(144, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select the duplication option:";
      this.radDuplicate.AutoSize = true;
      this.radDuplicate.CheckAlign = ContentAlignment.TopLeft;
      this.radDuplicate.Checked = true;
      this.radDuplicate.Location = new Point(20, 29);
      this.radDuplicate.Name = "radDuplicate";
      this.radDuplicate.Size = new Size(157, 18);
      this.radDuplicate.TabIndex = 1;
      this.radDuplicate.TabStop = true;
      this.radDuplicate.Text = "Duplicate the selected loan.";
      this.radDuplicate.TextAlign = ContentAlignment.TopLeft;
      this.radDuplicate.UseVisualStyleBackColor = true;
      this.radDuplicate.Click += new EventHandler(this.radioButtons_Click);
      this.dialogButtons1.DialogResult = DialogResult.OK;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 217);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(448, 44);
      this.dialogButtons1.TabIndex = 2;
      this.radDuplicateAsSecond.CheckAlign = ContentAlignment.TopLeft;
      this.radDuplicateAsSecond.Location = new Point(20, 51);
      this.radDuplicateAsSecond.Name = "radDuplicateAsSecond";
      this.radDuplicateAsSecond.Size = new Size(373, 21);
      this.radDuplicateAsSecond.TabIndex = 3;
      this.radDuplicateAsSecond.Text = "Create a second lien using the data from the selected loan.";
      this.radDuplicateAsSecond.TextAlign = ContentAlignment.TopLeft;
      this.radDuplicateAsSecond.UseVisualStyleBackColor = true;
      this.radDuplicateAsSecond.Click += new EventHandler(this.radioButtons_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 110);
      this.label2.Name = "label2";
      this.label2.Size = new Size(172, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = "Select the folder for the new loan:";
      this.cboFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFolder.FormattingEnabled = true;
      this.cboFolder.Location = new Point(20, 126);
      this.cboFolder.Name = "cboFolder";
      this.cboFolder.Size = new Size(416, 22);
      this.cboFolder.TabIndex = 5;
      this.labelDupTemplate.AutoSize = true;
      this.labelDupTemplate.Location = new Point(8, 167);
      this.labelDupTemplate.Name = "labelDupTemplate";
      this.labelDupTemplate.Size = new Size(178, 14);
      this.labelDupTemplate.TabIndex = 6;
      this.labelDupTemplate.Text = "Select the loan duplication template:";
      this.cboTemplates.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTemplates.FormattingEnabled = true;
      this.cboTemplates.Location = new Point(20, 184);
      this.cboTemplates.Name = "cboTemplates";
      this.cboTemplates.Size = new Size(416, 22);
      this.cboTemplates.TabIndex = 7;
      this.cboTemplates.SelectedIndexChanged += new EventHandler(this.cboTemplates_SelectedIndexChanged);
      this.radPiggyback.CheckAlign = ContentAlignment.TopLeft;
      this.radPiggyback.Location = new Point(20, 74);
      this.radPiggyback.Name = "radPiggyback";
      this.radPiggyback.Size = new Size(416, 21);
      this.radPiggyback.TabIndex = 8;
      this.radPiggyback.Text = "Create a second lien linked as a piggyback using the data from the selected loan.";
      this.radPiggyback.TextAlign = ContentAlignment.TopLeft;
      this.radPiggyback.UseVisualStyleBackColor = true;
      this.radPiggyback.Click += new EventHandler(this.radioButtons_Click);
      this.AcceptButton = (IButtonControl) this.dialogButtons1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(448, 261);
      this.Controls.Add((Control) this.radPiggyback);
      this.Controls.Add((Control) this.cboTemplates);
      this.Controls.Add((Control) this.labelDupTemplate);
      this.Controls.Add((Control) this.cboFolder);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.radDuplicateAsSecond);
      this.Controls.Add((Control) this.dialogButtons1);
      this.Controls.Add((Control) this.radDuplicate);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DuplicateLoanDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Duplicate Loan";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
