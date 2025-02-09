// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.ArchiveDocumentList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web.Security;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class ArchiveDocumentList : UserControl
  {
    private Sessions.Session session;
    private List<DocumentSettingInfo> docList = new List<DocumentSettingInfo>();
    private SessionObjects objs;
    private List<ExternalSettingValue> settingsList;
    private static string sw = Tracing.SwDataEngine;
    private const string className = "ArchiveDocumentList";
    private IContainer components;
    private GradientPanel panel1;
    private ComboBox cmbChannelType;
    private Label lblChannelType;
    private GroupContainer grpCntArchive;
    private GridView grdArchDocList;
    private StandardIconButton stnBtnDelete;
    private Button btnUnArchive;

    public ArchiveDocumentList(Sessions.Session session)
    {
      this.session = session;
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Document Category");
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.InitializeComponent();
      this.cmbChannelType.SelectedIndex = 0;
      this.docList = this.session.ConfigurationManager.GetAllArchiveDocuments(-1);
      this.PopulateArchiveDocuments(this.docList);
      this.btnUnArchive.Enabled = false;
      this.stnBtnDelete.Enabled = false;
    }

    public ArchiveDocumentList(SessionObjects session)
    {
      this.objs = session;
      this.InitializeComponent();
      this.docList = this.objs.ConfigurationManager.GetAllArchiveDocuments(-1);
      this.PopulateArchiveDocuments(this.docList);
      this.btnUnArchive.Enabled = false;
      this.stnBtnDelete.Enabled = false;
    }

    private void PopulateArchiveDocuments(List<DocumentSettingInfo> docListforGridView)
    {
      this.grdArchDocList.Items.Clear();
      foreach (DocumentSettingInfo documentSettingInfo in docListforGridView)
      {
        DateTime dateTime;
        string str1;
        if (!(documentSettingInfo.StartDate == DateTime.MinValue))
        {
          dateTime = documentSettingInfo.StartDate;
          str1 = dateTime.ToString("d");
        }
        else
          str1 = "";
        string str2 = str1;
        string str3;
        if (!(documentSettingInfo.EndDate == DateTime.MinValue))
        {
          dateTime = documentSettingInfo.EndDate;
          str3 = dateTime.ToString("d");
        }
        else
          str3 = "";
        string str4 = str3;
        string[] items = new string[7]
        {
          documentSettingInfo.DisplayName,
          this.GetDocumentCategory(documentSettingInfo.Category),
          this.GetChannelStringValue(documentSettingInfo.Channel),
          str2,
          str4,
          documentSettingInfo.AddedBy,
          null
        };
        dateTime = documentSettingInfo.DateAdded;
        items[6] = dateTime.ToString();
        this.grdArchDocList.Items.Add(new GVItem(items)
        {
          Tag = (object) documentSettingInfo
        });
      }
    }

    private string GetDocumentCategory(int category)
    {
      string documentCategory = "";
      if (category == -1)
        documentCategory = "";
      else if (this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == category)) != null)
        documentCategory = this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == category)).settingValue;
      return documentCategory;
    }

    private void stnBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.grdArchDocList.SelectedItems.Count == 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected document(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;
      foreach (GVItem selectedItem in this.grdArchDocList.SelectedItems)
      {
        DocumentSettingInfo tag = (DocumentSettingInfo) selectedItem.Tag;
        // ISSUE: variable of a boxed type
        __Boxed<Guid> guid = (ValueType) tag.Guid;
        string str = ((IEnumerable<string>) tag.FileName.Split('.')).Last<string>();
        FileSystemEntry entry = new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), FileSystemEntry.Types.File, (string) null);
        this.session.ConfigurationManager.DeleteDocument(-1, tag.Guid, entry);
        try
        {
          using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
          {
            emSiteWebService.Timeout = 15000;
            string key = FormsAuthentication.HashPasswordForStoringInConfigFile(Session.CompanyInfo.ClientID + Session.CompanyInfo.ClientID, "sha1");
            if (emSiteWebService.DeleteShowDocument(Session.CompanyInfo.ClientID, tag.Guid, tag.FileName, key))
              Tracing.Log(ArchiveDocumentList.sw, nameof (ArchiveDocumentList), TraceLevel.Info, "Document has been deleted from TPO WebCenter. Guid:" + tag.Guid.ToString());
            else
              Tracing.Log(ArchiveDocumentList.sw, nameof (ArchiveDocumentList), TraceLevel.Error, "Document delete from TPO WebCenter Failed. Guid:" + tag.Guid.ToString());
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(ArchiveDocumentList.sw, nameof (ArchiveDocumentList), TraceLevel.Error, "Document delete from TPO WebCenter Failed. Guid:" + tag.Guid.ToString() + ", Exception :" + ex.Message);
        }
      }
      this.RefreshData();
    }

    private void grdArchDocList_SelectedIndexChanged(object sender, EventArgs e)
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (this.grdArchDocList.SelectedItems.Any<GVItem>())
      {
        this.btnUnArchive.Enabled = true;
        this.stnBtnDelete.Enabled = true;
      }
      else
      {
        this.btnUnArchive.Enabled = false;
        this.stnBtnDelete.Enabled = false;
      }
      if (!aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDeleteDocuments))
        this.stnBtnDelete.Enabled = false;
      if (aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCEditDocuments))
        return;
      this.btnUnArchive.Enabled = false;
      this.stnBtnDelete.Enabled = false;
    }

    private void cmbChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.RefreshData();
    }

    public void RefreshData()
    {
      this.docList = this.session.ConfigurationManager.GetAllArchiveDocuments(-1);
      if (this.cmbChannelType.SelectedItem.ToString() != "All")
        this.PopulateArchiveDocuments(this.docList.Where<DocumentSettingInfo>((Func<DocumentSettingInfo, bool>) (a => a.Channel == this.GetSelectedChannel() || a.Channel == ExternalOriginatorEntityType.Both)).ToList<DocumentSettingInfo>());
      else
        this.PopulateArchiveDocuments(this.docList);
    }

    public ExternalOriginatorEntityType GetSelectedChannel()
    {
      if (this.cmbChannelType.SelectedItem.ToString() == "All")
        return ExternalOriginatorEntityType.Both;
      if (this.cmbChannelType.SelectedItem.ToString() == "Broker")
        return ExternalOriginatorEntityType.Broker;
      return this.cmbChannelType.SelectedItem.ToString() == "Correspondent" ? ExternalOriginatorEntityType.Correspondent : ExternalOriginatorEntityType.Both;
    }

    public string GetChannelStringValue(ExternalOriginatorEntityType channelType)
    {
      string channelStringValue = "All";
      switch (channelType)
      {
        case ExternalOriginatorEntityType.Broker:
          channelStringValue = "Broker";
          break;
        case ExternalOriginatorEntityType.Correspondent:
          channelStringValue = "Correspondent";
          break;
        case ExternalOriginatorEntityType.Both:
          channelStringValue = "All";
          break;
      }
      return channelStringValue;
    }

    private void btnUnArchive_Click(object sender, EventArgs e)
    {
      GVSelectedItemCollection selectedItems = this.grdArchDocList.SelectedItems;
      List<string> guids = new List<string>();
      foreach (GVItem gvItem in selectedItems)
      {
        DocumentSettingInfo tag = (DocumentSettingInfo) gvItem.Tag;
        guids.Add(tag.Guid.ToString());
      }
      this.session.ConfigurationManager.UnArchiveDocuments(-1, guids);
      this.RefreshData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.panel1 = new GradientPanel();
      this.cmbChannelType = new ComboBox();
      this.lblChannelType = new Label();
      this.grpCntArchive = new GroupContainer();
      this.grdArchDocList = new GridView();
      this.stnBtnDelete = new StandardIconButton();
      this.btnUnArchive = new Button();
      this.panel1.SuspendLayout();
      this.grpCntArchive.SuspendLayout();
      ((ISupportInitialize) this.stnBtnDelete).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.cmbChannelType);
      this.panel1.Controls.Add((Control) this.lblChannelType);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.panel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(754, 40);
      this.panel1.TabIndex = 1;
      this.cmbChannelType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbChannelType.FormattingEnabled = true;
      this.cmbChannelType.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Broker",
        (object) "Correspondent"
      });
      this.cmbChannelType.Location = new Point(97, 11);
      this.cmbChannelType.Name = "cmbChannelType";
      this.cmbChannelType.Size = new Size(121, 21);
      this.cmbChannelType.TabIndex = 1;
      this.cmbChannelType.SelectedIndexChanged += new EventHandler(this.cmbChannelType_SelectedIndexChanged);
      this.lblChannelType.AutoSize = true;
      this.lblChannelType.BackColor = Color.Transparent;
      this.lblChannelType.Location = new Point(13, 15);
      this.lblChannelType.Name = "lblChannelType";
      this.lblChannelType.Size = new Size(76, 13);
      this.lblChannelType.TabIndex = 0;
      this.lblChannelType.Text = "Channel Type:";
      this.grpCntArchive.Controls.Add((Control) this.grdArchDocList);
      this.grpCntArchive.Controls.Add((Control) this.stnBtnDelete);
      this.grpCntArchive.Controls.Add((Control) this.btnUnArchive);
      this.grpCntArchive.Dock = DockStyle.Fill;
      this.grpCntArchive.HeaderForeColor = SystemColors.ControlText;
      this.grpCntArchive.Location = new Point(0, 40);
      this.grpCntArchive.Name = "grpCntArchive";
      this.grpCntArchive.Size = new Size(754, 530);
      this.grpCntArchive.TabIndex = 2;
      this.grpCntArchive.Text = "Document List";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnDisplayName";
      gvColumn1.Text = "Document Display Name";
      gvColumn1.Width = 140;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnCategory";
      gvColumn2.Text = "Category";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnChannel";
      gvColumn3.Text = "Channel";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnStartDate";
      gvColumn4.SortMethod = GVSortMethod.Date;
      gvColumn4.Text = "Start Date";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnEndDate";
      gvColumn5.SortMethod = GVSortMethod.Date;
      gvColumn5.Text = "End Date";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnAddedBy";
      gvColumn6.Text = "Added By";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnTimeAdded";
      gvColumn7.SortMethod = GVSortMethod.DateTime;
      gvColumn7.Text = "Date/Time Added";
      gvColumn7.Width = 100;
      this.grdArchDocList.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.grdArchDocList.Dock = DockStyle.Fill;
      this.grdArchDocList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdArchDocList.Location = new Point(1, 26);
      this.grdArchDocList.Name = "grdArchDocList";
      this.grdArchDocList.Size = new Size(752, 503);
      this.grdArchDocList.TabIndex = 2;
      this.grdArchDocList.SelectedIndexChanged += new EventHandler(this.grdArchDocList_SelectedIndexChanged);
      this.stnBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stnBtnDelete.BackColor = Color.Transparent;
      this.stnBtnDelete.Location = new Point(734, 4);
      this.stnBtnDelete.MouseDownImage = (Image) null;
      this.stnBtnDelete.Name = "stnBtnDelete";
      this.stnBtnDelete.Size = new Size(16, 16);
      this.stnBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stnBtnDelete.TabIndex = 1;
      this.stnBtnDelete.TabStop = false;
      this.stnBtnDelete.Click += new EventHandler(this.stnBtnDelete_Click);
      this.btnUnArchive.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUnArchive.Location = new Point(619, 0);
      this.btnUnArchive.Name = "btnUnArchive";
      this.btnUnArchive.Size = new Size(109, 23);
      this.btnUnArchive.TabIndex = 0;
      this.btnUnArchive.Text = "Unarchive";
      this.btnUnArchive.UseVisualStyleBackColor = true;
      this.btnUnArchive.Click += new EventHandler(this.btnUnArchive_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpCntArchive);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (ArchiveDocumentList);
      this.Size = new Size(754, 570);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.grpCntArchive.ResumeLayout(false);
      ((ISupportInitialize) this.stnBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
