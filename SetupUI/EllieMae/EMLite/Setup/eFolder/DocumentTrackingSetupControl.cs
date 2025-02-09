// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.DocumentTrackingSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.WebServices;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class DocumentTrackingSetupControl : SettingsUserControl
  {
    private Sessions.Session session;
    private DocumentTrackingSetup docSetup;
    private ImageAttachmentSettings imageAttachmentSettings;
    private GridViewDataManager gvTemplatesMgr;
    private VerifDays verifDays;
    private IContainer components;
    private StandardIconButton btnReset;
    private ToolTip toolTip;
    private StandardIconButton btnSave;
    private RadioButton rdoCalendar;
    private GroupContainer gcTemplates;
    private StandardIconButton btnDeleteTemplate;
    private StandardIconButton btnEditTemplate;
    private StandardIconButton btnNewTemplate;
    private GroupContainer gcDays;
    private RadioButton rdoBusiness;
    private GridView gvTemplates;
    private CheckBox chkCreateDocumentEntry;
    private Label lblDays;
    private CheckBox chkSaveCopyInfoDocs;
    private CheckBox chkApplyTimeStampToInfoDocs;
    private CheckBox chkUseBackgroundConversion;
    private CheckBox chkIgnoreIntendedFor;

    public DocumentTrackingSetupControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup() ?? new DocumentTrackingSetup();
      this.imageAttachmentSettings = this.session.ConfigurationManager.GetImageAttachmentSettings();
      this.verifDays = (VerifDays) this.session.GetSystemSettings(typeof (VerifDays));
      this.initTemplateList();
      this.loadTemplateList();
      this.initDocPreferences();
    }

    private void initTemplateList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.session, this.gvTemplates, (LoanDataMgr) null);
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      columnList.InsertRange(0, (IEnumerable<TableLayout.Column>) new TableLayout.Column[11]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.DocTypeColumn,
        GridViewDataManager.DocSourceColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.DaysTillExpireColumn,
        GridViewDataManager.OpeningDocumentColumn,
        GridViewDataManager.PreClosingDocumentColumn,
        GridViewDataManager.ClosingDocumentColumn,
        GridViewDataManager.AvailableExternallyColumn,
        GridViewDataManager.SignatureTypeColumn
      });
      if (this.imageAttachmentSettings.UseImageAttachments)
        columnList.AddRange((IEnumerable<TableLayout.Column>) new TableLayout.Column[2]
        {
          GridViewDataManager.ConversionTypeColumn,
          GridViewDataManager.MaintainOriginalColumn
        });
      this.gvTemplatesMgr.CreateLayout(columnList.ToArray());
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void loadTemplateList()
    {
      this.gvTemplatesMgr.ClearItems();
      foreach (DocumentTemplate template in this.docSetup)
      {
        if (string.Compare(template.Source, "GFE Provider", true) == 0)
          template.Source = "Settlement Service List of Providers";
        if (string.Compare(template.Source, "GFE Provider (Letter)", true) == 0)
          template.Source = "Settlement Service List of Providers (Letter)";
        this.gvTemplatesMgr.AddItem(template);
      }
      string str1 = "Color";
      if (this.imageAttachmentSettings.ConversionType.Equals((object) ImageConversionType.BlackAndWhite))
        str1 = "Black & White";
      string str2 = this.imageAttachmentSettings.SaveOriginalFormat ? "Yes" : "No";
      for (int index = 1; index <= 5; ++index)
      {
        string str3 = (string) null;
        switch (index)
        {
          case 1:
            str3 = "VOD";
            break;
          case 2:
            str3 = "VOE";
            break;
          case 3:
            str3 = "VOL";
            break;
          case 4:
            str3 = "VOR";
            break;
          case 5:
            str3 = "VOM";
            break;
        }
        int recvDays = this.verifDays.GetRecvDays(str3);
        int expDays = this.verifDays.GetExpDays(str3);
        GVItem gvItem = this.gvTemplates.Items.Add(str3);
        gvItem.SubItems[2].Text = "Verification";
        gvItem.SubItems[3].Text = str3;
        if (recvDays > 0)
          gvItem.SubItems[4].Value = (object) recvDays;
        if (expDays > 0)
          gvItem.SubItems[5].Value = (object) expDays;
        gvItem.SubItems[6].Text = "No";
        gvItem.SubItems[7].Text = "No";
        gvItem.SubItems[8].Text = "No";
        gvItem.SubItems[9].Text = "Webcenter, TPO, EDM Lenders";
        gvItem.SubItems[10].Text = "Wet Sign Only";
        gvItem.SubItems[11].Text = str1;
        gvItem.SubItems[12].Text = str2;
        gvItem.Tag = (object) str3;
      }
      this.gcTemplates.Text = "Documents (" + this.gvTemplates.Items.Count.ToString() + ")";
      this.gvTemplates.ReSort();
    }

    private void editSelectedItem()
    {
      if (this.gvTemplates.SelectedItems.Count != 1)
        return;
      GVItem selectedItem = this.gvTemplates.SelectedItems[0];
      if (selectedItem.Tag is DocumentTemplate)
      {
        using (DocumentTemplateDialog documentTemplateDialog = new DocumentTemplateDialog(this.docSetup, (DocumentTemplate) selectedItem.Tag, this.session))
        {
          if (documentTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
            return;
          this.loadTemplateList();
        }
      }
      else
      {
        if (!(selectedItem.Tag is string))
          return;
        using (VerifDaysSetupDialog verifDaysSetupDialog = new VerifDaysSetupDialog(this.verifDays, (string) selectedItem.Tag))
        {
          if (verifDaysSetupDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
            return;
          this.loadTemplateList();
        }
      }
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvTemplates.SelectedItems.Count;
      this.btnEditTemplate.Enabled = count == 1;
      this.btnDeleteTemplate.Enabled = count > 0;
    }

    private void gvTemplates_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void btnNewTemplate_Click(object sender, EventArgs e)
    {
      using (DocumentTemplateDialog documentTemplateDialog = new DocumentTemplateDialog(this.docSetup, (DocumentTemplate) null, this.session))
      {
        if (documentTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadTemplateList();
      }
    }

    private void btnEditTemplate_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void btnDeleteTemplate_Click(object sender, EventArgs e)
    {
      List<string> stringList = new List<string>();
      List<DocumentTemplate> documentTemplateList1 = new List<DocumentTemplate>();
      List<DocumentTemplate> documentTemplateList2 = new List<DocumentTemplate>();
      foreach (GVItem selectedItem in this.gvTemplates.SelectedItems)
      {
        if (selectedItem.Tag is DocumentTemplate)
        {
          DocumentTemplate tag = (DocumentTemplate) selectedItem.Tag;
          if (Epass.IsEpassDoc(tag.Name))
            documentTemplateList1.Add(tag);
          else
            documentTemplateList2.Add(tag);
        }
        else if (selectedItem.Tag is string)
          stringList.Add(selectedItem.Tag as string);
      }
      if (stringList.Count > 0 || documentTemplateList1.Count > 0)
      {
        string str1 = string.Empty;
        foreach (DocumentTemplate documentTemplate in documentTemplateList1)
          str1 = str1 + documentTemplate.Name + "\r\n";
        foreach (string str2 in stringList)
          str1 = str1 + str2 + "\r\n";
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following document(s) are predefined and cannot be deleted:\r\n\r\n" + str1, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (documentTemplateList2.Count <= 0)
        return;
      string str = string.Empty;
      foreach (DocumentTemplate documentTemplate in documentTemplateList2)
        str = str + documentTemplate.Name + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete the following document(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      foreach (DocumentTemplate template in documentTemplateList2)
        this.docSetup.Remove(template);
      this.session.ConfigurationManager.SaveDocumentTrackingSetup(this.docSetup);
      this.loadTemplateList();
    }

    public List<string> SelectedDocumentTemplates
    {
      get
      {
        List<string> documentTemplates = new List<string>();
        foreach (GVItem selectedItem in this.gvTemplates.SelectedItems)
        {
          if (selectedItem.Tag is DocumentTemplate)
            documentTemplates.Add(((DocumentTemplate) selectedItem.Tag).Name);
        }
        return documentTemplates;
      }
      set
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
        {
          if (gvItem.Tag is DocumentTemplate && value.Contains(((DocumentTemplate) gvItem.Tag).Name))
            gvItem.Selected = true;
        }
      }
    }

    public List<string> SelectedVerifDaysSettings
    {
      get
      {
        List<string> verifDaysSettings = new List<string>();
        foreach (GVItem selectedItem in this.gvTemplates.SelectedItems)
        {
          if (!(selectedItem.Tag is DocumentTemplate))
          {
            switch (string.Concat(selectedItem.Tag))
            {
              case "VOD":
                verifDaysSettings.Add("vodRecvDays");
                verifDaysSettings.Add("vodExpDays");
                continue;
              case "VOE":
                verifDaysSettings.Add("voeRecvDays");
                verifDaysSettings.Add("voeExpDays");
                continue;
              case "VOL":
                verifDaysSettings.Add("volRecvDays");
                verifDaysSettings.Add("volExpDays");
                continue;
              case "VOR":
                verifDaysSettings.Add("vorRecvDays");
                verifDaysSettings.Add("vorExpDays");
                continue;
              case "VOM":
                verifDaysSettings.Add("vomRecvDays");
                verifDaysSettings.Add("vomExpDays");
                continue;
              default:
                continue;
            }
          }
        }
        return verifDaysSettings;
      }
      set
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
        {
          if (!(gvItem.Tag is DocumentTemplate))
          {
            gvItem.Selected = false;
            switch (string.Concat(gvItem.Tag))
            {
              case "VOD":
                string str1 = "vod";
                if (value.Contains(str1 + "RecvDays") || value.Contains(str1 + "ExpDays"))
                {
                  gvItem.Selected = true;
                  continue;
                }
                continue;
              case "VOE":
                string str2 = "voe";
                if (value.Contains(str2 + "RecvDays") || value.Contains(str2 + "ExpDays"))
                {
                  gvItem.Selected = true;
                  continue;
                }
                continue;
              case "VOR":
                string str3 = "vor";
                if (value.Contains(str3 + "RecvDays") || value.Contains(str3 + "ExpDays"))
                {
                  gvItem.Selected = true;
                  continue;
                }
                continue;
              case "VOM":
                string str4 = "vom";
                if (value.Contains(str4 + "RecvDays") || value.Contains(str4 + "ExpDays"))
                {
                  gvItem.Selected = true;
                  continue;
                }
                continue;
              case "VOL":
                string str5 = "vol";
                if (value.Contains(str5 + "RecvDays") || value.Contains(str5 + "ExpDays"))
                {
                  gvItem.Selected = true;
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    private void initDocPreferences()
    {
      if ((AutoDayCountSetting) Enum.Parse(typeof (AutoDayCountSetting), string.Concat(this.session.ServerManager.GetServerSetting("Policies.DocumentTrackingDayCount")), true) == AutoDayCountSetting.BusinessDays)
        this.rdoBusiness.Checked = true;
      else
        this.rdoCalendar.Checked = true;
      this.chkCreateDocumentEntry.Checked = !this.docSetup.DoNotCreateInfoDocs;
      this.chkSaveCopyInfoDocs.Checked = this.docSetup.SaveCopyInfoDocs;
      this.chkApplyTimeStampToInfoDocs.Checked = this.docSetup.ApplyTimeStampToInfoDocs;
      this.chkSaveCopyInfoDocs.Enabled = this.chkCreateDocumentEntry.Checked;
      this.chkApplyTimeStampToInfoDocs.Enabled = this.chkSaveCopyInfoDocs.Checked;
      this.chkUseBackgroundConversion.Checked = this.docSetup.UseBackgroundConversion;
      this.chkIgnoreIntendedFor.Checked = this.docSetup.IgnoreIntendedFor;
      this.setDirtyFlag(false);
    }

    protected override void setDirtyFlag(bool val)
    {
      this.btnSave.Enabled = val;
      this.btnReset.Enabled = val;
      base.setDirtyFlag(val);
    }

    private void btnSave_Click(object sender, EventArgs e) => this.Save();

    public override void Save()
    {
      AutoDayCountSetting autoDayCountSetting = AutoDayCountSetting.CalendarDays;
      if (this.rdoBusiness.Checked)
        autoDayCountSetting = AutoDayCountSetting.BusinessDays;
      this.session.ServerManager.UpdateServerSetting("Policies.DocumentTrackingDayCount", (object) autoDayCountSetting);
      if (this.docSetup.DoNotCreateInfoDocs == this.chkCreateDocumentEntry.Checked || this.docSetup.SaveCopyInfoDocs != this.chkSaveCopyInfoDocs.Checked || this.docSetup.ApplyTimeStampToInfoDocs != this.chkApplyTimeStampToInfoDocs.Checked || this.docSetup.UseBackgroundConversion != this.chkUseBackgroundConversion.Checked || this.docSetup.IgnoreIntendedFor != this.chkIgnoreIntendedFor.Checked)
      {
        bool backgroundConversion = this.docSetup.UseBackgroundConversion;
        this.docSetup.DoNotCreateInfoDocs = !this.chkCreateDocumentEntry.Checked;
        this.docSetup.SaveCopyInfoDocs = this.chkSaveCopyInfoDocs.Checked;
        this.docSetup.ApplyTimeStampToInfoDocs = this.chkApplyTimeStampToInfoDocs.Checked;
        this.docSetup.UseBackgroundConversion = this.chkUseBackgroundConversion.Checked;
        this.docSetup.IgnoreIntendedFor = this.chkIgnoreIntendedFor.Checked;
        this.session.ConfigurationManager.SaveDocumentTrackingSetup(this.docSetup);
        if (this.docSetup.UseBackgroundConversion != backgroundConversion)
        {
          using (CompanySettingsService companySettingsService = new CompanySettingsService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CompanySettingsServiceUrl))
          {
            companySettingsService.CompanySettingsCredentialsValue = new CompanySettingsCredentials();
            companySettingsService.CompanySettingsCredentialsValue.ClientID = Session.CompanyInfo.ClientID;
            companySettingsService.CompanySettingsCredentialsValue.UserID = Session.UserID;
            companySettingsService.CompanySettingsCredentialsValue.Password = EpassLogin.LoginPassword;
            companySettingsService.SaveCompanySettings(new CompanySettings()
            {
              ClientID = Session.CompanyInfo.ClientID,
              SettingName = "BackgroundConversion",
              SettingValue = this.docSetup.UseBackgroundConversion
            });
          }
        }
      }
      this.initTemplateList();
      this.loadTemplateList();
      this.setDirtyFlag(false);
    }

    private void btnReset_Click(object sender, EventArgs e) => this.initDocPreferences();

    public override void Reset() => this.initDocPreferences();

    private void rdoBusiness_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void rdoCalendar_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void chkCreateDocumentEntry_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      if (!this.chkCreateDocumentEntry.Checked)
        this.chkSaveCopyInfoDocs.Checked = false;
      this.chkSaveCopyInfoDocs.Enabled = this.chkCreateDocumentEntry.Checked;
    }

    private void chkSaveCopyInfoDocs_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      if (!this.chkSaveCopyInfoDocs.Checked)
        this.chkApplyTimeStampToInfoDocs.Checked = false;
      this.chkApplyTimeStampToInfoDocs.Enabled = this.chkSaveCopyInfoDocs.Checked;
    }

    private void chkUseBackgroundConversion_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkIgnoreIntendedFor_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkApplyTimeStampToInfoDocs_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkSeparate_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnReset = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.rdoCalendar = new RadioButton();
      this.gcTemplates = new GroupContainer();
      this.gvTemplates = new GridView();
      this.btnDeleteTemplate = new StandardIconButton();
      this.btnEditTemplate = new StandardIconButton();
      this.btnNewTemplate = new StandardIconButton();
      this.toolTip = new ToolTip(this.components);
      this.gcDays = new GroupContainer();
      this.chkIgnoreIntendedFor = new CheckBox();
      this.chkUseBackgroundConversion = new CheckBox();
      this.chkApplyTimeStampToInfoDocs = new CheckBox();
      this.chkSaveCopyInfoDocs = new CheckBox();
      this.chkCreateDocumentEntry = new CheckBox();
      this.rdoBusiness = new RadioButton();
      this.lblDays = new Label();
      ((ISupportInitialize) this.btnReset).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.gcTemplates.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteTemplate).BeginInit();
      ((ISupportInitialize) this.btnEditTemplate).BeginInit();
      ((ISupportInitialize) this.btnNewTemplate).BeginInit();
      this.gcDays.SuspendLayout();
      this.SuspendLayout();
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.BackColor = Color.Transparent;
      this.btnReset.Location = new Point(852, 5);
      this.btnReset.MouseDownImage = (Image) null;
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(16, 16);
      this.btnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnReset.TabIndex = 8;
      this.btnReset.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnReset, "Reset");
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(830, 5);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 9;
      this.btnSave.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnSave, "Save");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.rdoCalendar.AutoSize = true;
      this.rdoCalendar.Location = new Point(226, 32);
      this.rdoCalendar.Name = "rdoCalendar";
      this.rdoCalendar.Size = new Size(94, 17);
      this.rdoCalendar.TabIndex = 7;
      this.rdoCalendar.TabStop = true;
      this.rdoCalendar.Text = "Calendar Days";
      this.rdoCalendar.UseVisualStyleBackColor = true;
      this.rdoCalendar.CheckedChanged += new EventHandler(this.rdoCalendar_CheckedChanged);
      this.gcTemplates.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcTemplates.Controls.Add((Control) this.gvTemplates);
      this.gcTemplates.Controls.Add((Control) this.btnDeleteTemplate);
      this.gcTemplates.Controls.Add((Control) this.btnEditTemplate);
      this.gcTemplates.Controls.Add((Control) this.btnNewTemplate);
      this.gcTemplates.Dock = DockStyle.Fill;
      this.gcTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcTemplates.Location = new Point(0, 0);
      this.gcTemplates.Name = "gcTemplates";
      this.gcTemplates.Size = new Size(876, 143);
      this.gcTemplates.TabIndex = 12;
      this.gcTemplates.Text = "Documents (#)";
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(874, 117);
      this.gvTemplates.TabIndex = 12;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.gvTemplates.ItemDoubleClick += new GVItemEventHandler(this.gvTemplates_ItemDoubleClick);
      this.btnDeleteTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteTemplate.BackColor = Color.Transparent;
      this.btnDeleteTemplate.Enabled = false;
      this.btnDeleteTemplate.Location = new Point(854, 5);
      this.btnDeleteTemplate.MouseDownImage = (Image) null;
      this.btnDeleteTemplate.Name = "btnDeleteTemplate";
      this.btnDeleteTemplate.Size = new Size(16, 16);
      this.btnDeleteTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteTemplate.TabIndex = 11;
      this.btnDeleteTemplate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDeleteTemplate, "Delete");
      this.btnDeleteTemplate.Click += new EventHandler(this.btnDeleteTemplate_Click);
      this.btnEditTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditTemplate.BackColor = Color.Transparent;
      this.btnEditTemplate.Enabled = false;
      this.btnEditTemplate.Location = new Point(832, 5);
      this.btnEditTemplate.MouseDownImage = (Image) null;
      this.btnEditTemplate.Name = "btnEditTemplate";
      this.btnEditTemplate.Size = new Size(16, 16);
      this.btnEditTemplate.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditTemplate.TabIndex = 10;
      this.btnEditTemplate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnEditTemplate, "Edit");
      this.btnEditTemplate.Click += new EventHandler(this.btnEditTemplate_Click);
      this.btnNewTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewTemplate.BackColor = Color.Transparent;
      this.btnNewTemplate.Location = new Point(810, 5);
      this.btnNewTemplate.MouseDownImage = (Image) null;
      this.btnNewTemplate.Name = "btnNewTemplate";
      this.btnNewTemplate.Size = new Size(16, 16);
      this.btnNewTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNewTemplate.TabIndex = 9;
      this.btnNewTemplate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnNewTemplate, "New");
      this.btnNewTemplate.Click += new EventHandler(this.btnNewTemplate_Click);
      this.gcDays.Controls.Add((Control) this.chkIgnoreIntendedFor);
      this.gcDays.Controls.Add((Control) this.chkUseBackgroundConversion);
      this.gcDays.Controls.Add((Control) this.chkApplyTimeStampToInfoDocs);
      this.gcDays.Controls.Add((Control) this.chkSaveCopyInfoDocs);
      this.gcDays.Controls.Add((Control) this.chkCreateDocumentEntry);
      this.gcDays.Controls.Add((Control) this.btnSave);
      this.gcDays.Controls.Add((Control) this.btnReset);
      this.gcDays.Controls.Add((Control) this.rdoCalendar);
      this.gcDays.Controls.Add((Control) this.rdoBusiness);
      this.gcDays.Controls.Add((Control) this.lblDays);
      this.gcDays.Dock = DockStyle.Bottom;
      this.gcDays.HeaderForeColor = SystemColors.ControlText;
      this.gcDays.Location = new Point(0, 143);
      this.gcDays.Name = "gcDays";
      this.gcDays.Size = new Size(876, 124);
      this.gcDays.TabIndex = 11;
      this.gcDays.Text = "Options";
      this.chkIgnoreIntendedFor.AutoSize = true;
      this.chkIgnoreIntendedFor.Location = new Point(550, 75);
      this.chkIgnoreIntendedFor.Name = "chkIgnoreIntendedFor";
      this.chkIgnoreIntendedFor.Size = new Size(267, 17);
      this.chkIgnoreIntendedFor.TabIndex = 14;
      this.chkIgnoreIntendedFor.Text = "Treat all documents as if intended for All Recipients";
      this.chkIgnoreIntendedFor.UseVisualStyleBackColor = true;
      this.chkIgnoreIntendedFor.Visible = false;
      this.chkIgnoreIntendedFor.CheckedChanged += new EventHandler(this.chkIgnoreIntendedFor_CheckedChanged);
      this.chkUseBackgroundConversion.AutoSize = true;
      this.chkUseBackgroundConversion.Location = new Point(550, 52);
      this.chkUseBackgroundConversion.Name = "chkUseBackgroundConversion";
      this.chkUseBackgroundConversion.Size = new Size(225, 17);
      this.chkUseBackgroundConversion.TabIndex = 13;
      this.chkUseBackgroundConversion.Text = "Queue Documents for Upload/Conversion";
      this.chkUseBackgroundConversion.UseVisualStyleBackColor = true;
      this.chkUseBackgroundConversion.CheckedChanged += new EventHandler(this.chkUseBackgroundConversion_CheckedChanged);
      this.chkApplyTimeStampToInfoDocs.AutoSize = true;
      this.chkApplyTimeStampToInfoDocs.Location = new Point(62, 98);
      this.chkApplyTimeStampToInfoDocs.Name = "chkApplyTimeStampToInfoDocs";
      this.chkApplyTimeStampToInfoDocs.Size = new Size(206, 17);
      this.chkApplyTimeStampToInfoDocs.TabIndex = 12;
      this.chkApplyTimeStampToInfoDocs.Text = "Insert Date and Time in the Document";
      this.chkApplyTimeStampToInfoDocs.UseVisualStyleBackColor = true;
      this.chkApplyTimeStampToInfoDocs.CheckedChanged += new EventHandler(this.chkApplyTimeStampToInfoDocs_CheckedChanged);
      this.chkSaveCopyInfoDocs.AutoSize = true;
      this.chkSaveCopyInfoDocs.Location = new Point(36, 75);
      this.chkSaveCopyInfoDocs.Name = "chkSaveCopyInfoDocs";
      this.chkSaveCopyInfoDocs.Size = new Size(258, 17);
      this.chkSaveCopyInfoDocs.TabIndex = 11;
      this.chkSaveCopyInfoDocs.Text = "Save copy of Informational Documents in eFolder";
      this.chkSaveCopyInfoDocs.UseVisualStyleBackColor = true;
      this.chkSaveCopyInfoDocs.CheckedChanged += new EventHandler(this.chkSaveCopyInfoDocs_CheckedChanged);
      this.chkCreateDocumentEntry.AutoSize = true;
      this.chkCreateDocumentEntry.Location = new Point(11, 52);
      this.chkCreateDocumentEntry.Name = "chkCreateDocumentEntry";
      this.chkCreateDocumentEntry.Size = new Size(388, 17);
      this.chkCreateDocumentEntry.TabIndex = 10;
      this.chkCreateDocumentEntry.Text = "Create a document entry when Informational Documents are sent to borrower";
      this.chkCreateDocumentEntry.UseVisualStyleBackColor = true;
      this.chkCreateDocumentEntry.CheckedChanged += new EventHandler(this.chkCreateDocumentEntry_CheckedChanged);
      this.rdoBusiness.AutoSize = true;
      this.rdoBusiness.Location = new Point(140, 32);
      this.rdoBusiness.Name = "rdoBusiness";
      this.rdoBusiness.Size = new Size(81, 17);
      this.rdoBusiness.TabIndex = 6;
      this.rdoBusiness.TabStop = true;
      this.rdoBusiness.Text = "Week Days";
      this.rdoBusiness.UseVisualStyleBackColor = true;
      this.rdoBusiness.CheckedChanged += new EventHandler(this.rdoBusiness_CheckedChanged);
      this.lblDays.AutoSize = true;
      this.lblDays.Location = new Point(8, 33);
      this.lblDays.Name = "lblDays";
      this.lblDays.Size = new Size(126, 13);
      this.lblDays.TabIndex = 5;
      this.lblDays.Text = "Document Days to Count";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTemplates);
      this.Controls.Add((Control) this.gcDays);
      this.Name = nameof (DocumentTrackingSetupControl);
      this.Size = new Size(876, 267);
      ((ISupportInitialize) this.btnReset).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.gcTemplates.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeleteTemplate).EndInit();
      ((ISupportInitialize) this.btnEditTemplate).EndInit();
      ((ISupportInitialize) this.btnNewTemplate).EndInit();
      this.gcDays.ResumeLayout(false);
      this.gcDays.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
