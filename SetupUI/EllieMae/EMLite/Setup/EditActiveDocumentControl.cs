// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditActiveDocumentControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditActiveDocumentControl : UserControl
  {
    private List<DocumentSettingInfo> documents;
    private Sessions.Session session;
    private List<ExternalSettingValue> settingsList;
    private AnimationProgress an;
    private Task<bool> t;
    private int assignCountSortOrder;
    private static string sw = Tracing.SwDataEngine;
    private const string className = "EditActiveDocumentControl";
    private SessionObjects obj;
    private IContainer components;
    private GroupContainer gcDocumentList;
    private StandardIconButton btnEdit;
    private StandardIconButton btnView;
    private GridView gridViewDocumentList;
    private StandardIconButton btnAdd;
    private ComboBox cboChannelType;
    private ComboBox cboStatus;
    private Button btnArchiveSelected;
    private GradientPanel pnlTop;
    private Label lblStatus;
    private Label lblChannelType;

    public EditActiveDocumentControl(Sessions.Session session)
    {
      this.session = session;
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Document Category");
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.InitializeComponent();
      this.cboChannelType.SelectedIndex = 0;
      this.cboStatus.SelectedIndex = 0;
      this.Dock = DockStyle.Fill;
      this.btnArchiveSelected.Enabled = false;
      this.btnView.Enabled = false;
      this.btnEdit.Enabled = false;
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCEditDocuments))
        return;
      this.btnAdd.Enabled = false;
      this.btnEdit.Enabled = false;
    }

    public EditActiveDocumentControl(SessionObjects obj)
    {
      this.InitializeComponent();
      this.obj = obj;
      this.cboChannelType.SelectedIndex = 0;
      this.cboStatus.SelectedIndex = 0;
      this.Dock = DockStyle.Fill;
      this.RefreshData();
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDeleteDocuments))
        return;
      this.btnAdd.Enabled = false;
      this.btnEdit.Enabled = false;
    }

    public void RefreshData()
    {
      this.gridViewDocumentList.BeginUpdate();
      this.gridViewDocumentList.Items.Clear();
      List<string> stringList = new List<string>();
      this.documents = this.session.ConfigurationManager.GetExternalDocuments(-1, this.GetSelectedChannelType(), this.GetSelectedStatus());
      foreach (DocumentSettingInfo document in this.documents)
        stringList.Add(document.Guid.ToString());
      foreach (DocumentSettingInfo document in this.documents)
        this.gridViewDocumentList.Items.Add(this.createGVItemForDocument(document));
      this.gridViewDocumentList.Sort(7, SortOrder.Descending);
      this.gridViewDocumentList.EndUpdate();
    }

    public int GetSelectedChannelType()
    {
      if (this.cboChannelType.SelectedItem != null)
      {
        if (this.cboChannelType.SelectedItem.ToString() == "Broker")
          return 1;
        if (this.cboChannelType.SelectedItem.ToString() == "Correspondent")
          return 2;
      }
      return -1;
    }

    public int GetSelectedStatus()
    {
      if (this.cboStatus.SelectedItem != null)
      {
        if (this.cboStatus.SelectedItem.ToString() == "Pending")
          return 1;
        if (this.cboStatus.SelectedItem.ToString() == "Active")
          return 2;
        if (this.cboStatus.SelectedItem.ToString() == "Not Active")
          return 3;
        if (this.cboStatus.SelectedItem.ToString() == "Expired")
          return 4;
      }
      return -1;
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
      }
      return channelStringValue;
    }

    public string GetStatusStringValue(ExternalOriginatorStatus status)
    {
      string statusStringValue = "";
      switch (status)
      {
        case ExternalOriginatorStatus.Pending:
          statusStringValue = "Pending";
          break;
        case ExternalOriginatorStatus.Active:
          statusStringValue = "Active";
          break;
        case ExternalOriginatorStatus.NotActive:
          statusStringValue = "Not Active";
          break;
        case ExternalOriginatorStatus.Expired:
          statusStringValue = "Expired";
          break;
      }
      return statusStringValue;
    }

    private GVItem createGVItemForDocument(DocumentSettingInfo document)
    {
      GVItem gvItemForDocument = new GVItem();
      gvItemForDocument.SubItems.Add((object) "");
      gvItemForDocument.SubItems[0].Checked = document.Active;
      gvItemForDocument.SubItems.Add((object) document.DisplayName);
      if (document.Category == -1)
        gvItemForDocument.SubItems.Add((object) "");
      else if (this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == document.Category)) != null)
        gvItemForDocument.SubItems.Add((object) this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == document.Category)).settingValue);
      else
        gvItemForDocument.SubItems.Add((object) "");
      gvItemForDocument.SubItems.Add((object) this.GetChannelStringValue(document.Channel));
      DateTime dateTime;
      if (document.StartDate == DateTime.MinValue)
      {
        gvItemForDocument.SubItems.Add((object) "");
      }
      else
      {
        GVSubItemCollection subItems = gvItemForDocument.SubItems;
        dateTime = document.StartDate;
        string str = dateTime.ToString("d");
        subItems.Add((object) str);
      }
      if (document.EndDate == DateTime.MinValue)
      {
        gvItemForDocument.SubItems.Add((object) "");
      }
      else
      {
        GVSubItemCollection subItems = gvItemForDocument.SubItems;
        dateTime = document.EndDate;
        string str = dateTime.ToString("d");
        subItems.Add((object) str);
      }
      gvItemForDocument.SubItems.Add((object) document.AddedBy);
      gvItemForDocument.SubItems.Add((object) document.DateAdded);
      gvItemForDocument.SubItems.Add((object) this.GetStatusStringValue(document.Status));
      if (document.Status == ExternalOriginatorStatus.Expired)
        gvItemForDocument.ForeColor = Color.Red;
      gvItemForDocument.SubItems.Add((object) "");
      EllieMae.EMLite.UI.LinkLabel linkLabel = new EllieMae.EMLite.UI.LinkLabel();
      linkLabel.Click += new EventHandler(this.linkLabelAssign_Click);
      linkLabel.ForeColor = Color.Blue;
      linkLabel.Width = this.gridViewDocumentList.Columns[9].Width;
      linkLabel.AutoSize = false;
      if (document.AssignCount > 0)
        linkLabel.Text = document.AssignCount.ToString();
      else
        linkLabel.Text = "0";
      gvItemForDocument.SubItems[9].Value = (object) linkLabel;
      gvItemForDocument.SubItems[9].ForeColor = Color.Blue;
      gvItemForDocument.Tag = (object) document;
      return gvItemForDocument;
    }

    private void linkLabelAssign_Click(object sender, EventArgs e)
    {
      EllieMae.EMLite.UI.LinkLabel linkLabel1 = (EllieMae.EMLite.UI.LinkLabel) null;
      if (sender != null)
        linkLabel1 = (EllieMae.EMLite.UI.LinkLabel) sender;
      for (int nItemIndex = 0; nItemIndex < this.gridViewDocumentList.Items.Count; ++nItemIndex)
      {
        if (this.gridViewDocumentList.Items[nItemIndex].SubItems[9].Value is EllieMae.EMLite.UI.LinkLabel)
        {
          EllieMae.EMLite.UI.LinkLabel linkLabel2 = (EllieMae.EMLite.UI.LinkLabel) this.gridViewDocumentList.Items[nItemIndex].SubItems[9].Value;
          if (linkLabel1 == linkLabel2)
          {
            DocumentSettingInfo tag = (DocumentSettingInfo) this.gridViewDocumentList.Items[nItemIndex].Tag;
            if (tag.AssignCount < 0)
              break;
            using (ViewDocAssignedTPOs viewDocAssignedTpOs = new ViewDocAssignedTPOs(tag, this.settingsList, this.session))
            {
              int num = (int) viewDocAssignedTpOs.ShowDialog((IWin32Window) this);
              this.RefreshData();
            }
          }
        }
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (AddDocumentForm addDocumentForm = new AddDocumentForm((DocumentSettingInfo) null, this.settingsList, this.session.UserInfo.FullName, this.session))
      {
        if (addDocumentForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        using (BinaryObject data = new BinaryObject(addDocumentForm.Path))
        {
          this.addProgressDocument(new FileSystemEntry("\\\\" + addDocumentForm.FileName, FileSystemEntry.Types.File, (string) null), data);
          this.session.ConfigurationManager.AddDocument(-1, addDocumentForm.Document, false);
          if (addDocumentForm.Document.AvailbleAllTPO)
            this.session.ConfigurationManager.AssignDefaultDocumentToAll(addDocumentForm.Document);
          else
            this.session.ConfigurationManager.RemoveDefaultDocumentFromAll(addDocumentForm.Document);
          if (addDocumentForm.assignTo == AssignTo.RelatedDocs)
          {
            DocumentSettingInfo relatedDocument = (DocumentSettingInfo) null;
            bool IsTopOfCategoryorDoc = false;
            if (addDocumentForm.documentPlacement == DocumentPlacement.BottomOfRelatedDoc || addDocumentForm.documentPlacement == DocumentPlacement.TopofRelatedDoc)
              relatedDocument = addDocumentForm.relatedDocument;
            if (addDocumentForm.documentPlacement == DocumentPlacement.TopOfCategory || addDocumentForm.documentPlacement == DocumentPlacement.TopofRelatedDoc)
              IsTopOfCategoryorDoc = true;
            this.session.ConfigurationManager.AssignDocumentsToTposByRelatedDoc(addDocumentForm.externalOrgIds, addDocumentForm.Document, relatedDocument, IsTopOfCategoryorDoc);
          }
          else if (addDocumentForm.assignTo == AssignTo.SelectTpos)
          {
            bool IsTopOfCategoryorDoc = false;
            if (addDocumentForm.documentPlacement == DocumentPlacement.TopOfCategory)
              IsTopOfCategoryorDoc = true;
            this.session.ConfigurationManager.AssignDocumentsToTposByRelatedDoc(addDocumentForm.externalOrgIds, addDocumentForm.Document, (DocumentSettingInfo) null, IsTopOfCategoryorDoc);
          }
          this.RefreshData();
          try
          {
            using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
            {
              emSiteWebService.Timeout = 15000;
              string key = FormsAuthentication.HashPasswordForStoringInConfigFile(Session.CompanyInfo.ClientID + Session.CompanyInfo.ClientID, "sha1");
              if (emSiteWebService.AddShowDocument(Session.CompanyInfo.ClientID, addDocumentForm.Document.Guid, data.GetBytes(), addDocumentForm.Document.FileName, key))
                Tracing.Log(EditActiveDocumentControl.sw, nameof (EditActiveDocumentControl), TraceLevel.Info, "Document has been uploaded to TPO WebCenter. Guid:" + addDocumentForm.Document.Guid.ToString());
              else
                Tracing.Log(EditActiveDocumentControl.sw, nameof (EditActiveDocumentControl), TraceLevel.Error, "Document upload to TPO WebCenter Failed. Guid:" + addDocumentForm.Document.Guid.ToString());
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(EditActiveDocumentControl.sw, nameof (EditActiveDocumentControl), TraceLevel.Error, "Document upload to TPO WebCenter Failed. Guid:" + addDocumentForm.Document.Guid.ToString() + ", Exception :" + ex.Message);
          }
        }
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Count == 0)
        return;
      bool availbleAllTpo = ((DocumentSettingInfo) this.gridViewDocumentList.SelectedItems[0].Tag).AvailbleAllTPO;
      using (AddDocumentForm addDocumentForm = new AddDocumentForm((DocumentSettingInfo) this.gridViewDocumentList.SelectedItems[0].Tag, this.settingsList, this.session.UserInfo.FullName, this.session))
      {
        if (addDocumentForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.session.ConfigurationManager.UpdateDocument(-1, addDocumentForm.Document);
        if (addDocumentForm.Document.AvailbleAllTPO != availbleAllTpo)
        {
          if (addDocumentForm.Document.AvailbleAllTPO)
            this.session.ConfigurationManager.AssignDefaultDocumentToAll(addDocumentForm.Document);
          else
            this.session.ConfigurationManager.RemoveDefaultDocumentFromAll(addDocumentForm.Document);
        }
        this.RefreshData();
      }
    }

    private void addProgressDocument(FileSystemEntry newEntry, BinaryObject data)
    {
      TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
      this.an = new AnimationProgress();
      this.t = Task<bool>.Factory.StartNew((Func<bool>) (() => this.addDocument(newEntry, data)));
      this.t.ContinueWith((Action<Task<bool>>) (task =>
      {
        if (!task.IsCompleted)
          return;
        this.an.Dispose();
        this.an.Close();
        if (task.IsCanceled || task.IsFaulted || !task.Result)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "Document upload failed!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Document uploaded successfully!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }), scheduler);
      this.an.StartPosition = FormStartPosition.CenterParent;
      int num = (int) this.an.ShowDialog((IWin32Window) this);
    }

    private bool addDocument(FileSystemEntry newEntry, BinaryObject data)
    {
      try
      {
        this.session.ConfigurationManager.CreateDocumentInDataFolder(newEntry, data);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void cboChannelType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboStatus.SelectedItem == null)
        return;
      this.RefreshData();
    }

    private void cboStatus_SelectedIndexChanged(object sender, EventArgs e) => this.RefreshData();

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Count == 0)
        return;
      DocumentSettingInfo tag = (DocumentSettingInfo) this.gridViewDocumentList.SelectedItems[0].Tag;
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) tag.Guid;
      string str = ((IEnumerable<string>) tag.FileName.Split('.')).Last<string>();
      string fileName = guid.ToString() + "." + str;
      string realFileName = ((IEnumerable<string>) tag.FileName.Split('\\')).Last<string>();
      try
      {
        this.previewAttachment(this.session.ConfigurationManager.ReadDocumentFromDataFolder(fileName), realFileName);
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        if (ex.Message.Contains(":"))
          message = ex.Message.Split(':')[1];
        int num = (int) Utils.Dialog((IWin32Window) this, message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void previewAttachment(BinaryObject obj, string realFileName)
    {
      string localDocumentDir = SystemSettings.LocalDocumentDir;
      Directory.CreateDirectory(Path.GetDirectoryName(localDocumentDir));
      string str = SystemUtil.CombinePath(localDocumentDir, realFileName);
      if (obj == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + realFileName + "' document cannot be found or no longer exists.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        try
        {
          obj.Write(str);
          obj.Download();
          Process.Start(str);
        }
        catch (Exception ex)
        {
          throw new Exception("The document is already open.\r\n");
        }
      }
    }

    private void btnArchiveSelected_Click(object sender, EventArgs e)
    {
      GVSelectedItemCollection selectedItems = this.gridViewDocumentList.SelectedItems;
      List<string> guids = new List<string>();
      foreach (GVItem gvItem in selectedItems)
      {
        DocumentSettingInfo tag = (DocumentSettingInfo) gvItem.Tag;
        guids.Add(tag.Guid.ToString());
      }
      this.session.ConfigurationManager.ArchiveDocuments(-1, guids);
      this.RefreshData();
    }

    private void gridViewDocumentList_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      DocumentSettingInfo tag = (DocumentSettingInfo) e.SubItem.Item.Tag;
      tag.Active = e.SubItem.Checked;
      this.session.ConfigurationManager.UpdateActiveStatus(-1, tag.Active, false, tag.Guid);
      this.RefreshData();
    }

    private void gridViewDocumentList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Any<GVItem>())
      {
        this.btnArchiveSelected.Enabled = true;
        if (this.gridViewDocumentList.SelectedItems.Count == 1)
        {
          this.btnView.Enabled = true;
          this.btnEdit.Enabled = true;
        }
        else
        {
          this.btnView.Enabled = false;
          this.btnEdit.Enabled = false;
        }
      }
      else
      {
        this.btnArchiveSelected.Enabled = false;
        this.btnView.Enabled = false;
        this.btnEdit.Enabled = false;
      }
      if (((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCEditDocuments))
        return;
      this.btnEdit.Enabled = false;
      this.btnEdit.Enabled = false;
      this.btnArchiveSelected.Enabled = false;
    }

    private void gridViewDocumentList_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      if (e.Column != 9)
        return;
      Comparison<DocumentSettingInfo> comparison = new Comparison<DocumentSettingInfo>(DocumentSettingInfo.CompareByAssignCount);
      if (this.assignCountSortOrder == 0)
      {
        this.documents.Sort(comparison);
        this.assignCountSortOrder = 1;
      }
      else
      {
        this.documents.Sort(comparison);
        this.documents.Reverse();
        this.assignCountSortOrder = 0;
      }
      this.gridViewDocumentList.BeginUpdate();
      this.gridViewDocumentList.Items.Clear();
      foreach (DocumentSettingInfo document in this.documents)
        this.gridViewDocumentList.Items.Add(this.createGVItemForDocument(document));
      this.gridViewDocumentList.EndUpdate();
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
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      this.cboChannelType = new ComboBox();
      this.cboStatus = new ComboBox();
      this.pnlTop = new GradientPanel();
      this.lblStatus = new Label();
      this.lblChannelType = new Label();
      this.gcDocumentList = new GroupContainer();
      this.btnArchiveSelected = new Button();
      this.btnEdit = new StandardIconButton();
      this.btnView = new StandardIconButton();
      this.gridViewDocumentList = new GridView();
      this.btnAdd = new StandardIconButton();
      this.pnlTop.SuspendLayout();
      this.gcDocumentList.SuspendLayout();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnView).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.SuspendLayout();
      this.cboChannelType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboChannelType.DropDownWidth = 175;
      this.cboChannelType.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboChannelType.FormattingEnabled = true;
      this.cboChannelType.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Broker",
        (object) "Correspondent"
      });
      this.cboChannelType.Location = new Point(94, 11);
      this.cboChannelType.Name = "cboChannelType";
      this.cboChannelType.Size = new Size(176, 22);
      this.cboChannelType.TabIndex = 14;
      this.cboChannelType.SelectedIndexChanged += new EventHandler(this.cboChannelType_SelectedIndexChanged);
      this.cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStatus.DropDownWidth = 175;
      this.cboStatus.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboStatus.FormattingEnabled = true;
      this.cboStatus.Items.AddRange(new object[5]
      {
        (object) "All",
        (object) "Pending",
        (object) "Active",
        (object) "Not Active",
        (object) "Expired"
      });
      this.cboStatus.Location = new Point(365, 11);
      this.cboStatus.Name = "cboStatus";
      this.cboStatus.Size = new Size(176, 22);
      this.cboStatus.TabIndex = 36;
      this.cboStatus.SelectedIndexChanged += new EventHandler(this.cboStatus_SelectedIndexChanged);
      this.pnlTop.Controls.Add((Control) this.lblStatus);
      this.pnlTop.Controls.Add((Control) this.lblChannelType);
      this.pnlTop.Controls.Add((Control) this.cboStatus);
      this.pnlTop.Controls.Add((Control) this.cboChannelType);
      this.pnlTop.Dock = DockStyle.Top;
      this.pnlTop.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlTop.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlTop.Location = new Point(0, 0);
      this.pnlTop.Name = "pnlTop";
      this.pnlTop.Size = new Size(872, 49);
      this.pnlTop.TabIndex = 38;
      this.lblStatus.AutoSize = true;
      this.lblStatus.BackColor = Color.Transparent;
      this.lblStatus.Location = new Point(313, 16);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(40, 13);
      this.lblStatus.TabIndex = 38;
      this.lblStatus.Text = "Status:";
      this.lblChannelType.AutoSize = true;
      this.lblChannelType.BackColor = Color.Transparent;
      this.lblChannelType.Location = new Point(7, 16);
      this.lblChannelType.Name = "lblChannelType";
      this.lblChannelType.Size = new Size(76, 13);
      this.lblChannelType.TabIndex = 37;
      this.lblChannelType.Text = "Channel Type:";
      this.gcDocumentList.Borders = AnchorStyles.Top;
      this.gcDocumentList.Controls.Add((Control) this.btnArchiveSelected);
      this.gcDocumentList.Controls.Add((Control) this.btnEdit);
      this.gcDocumentList.Controls.Add((Control) this.btnView);
      this.gcDocumentList.Controls.Add((Control) this.gridViewDocumentList);
      this.gcDocumentList.Controls.Add((Control) this.btnAdd);
      this.gcDocumentList.Dock = DockStyle.Fill;
      this.gcDocumentList.HeaderForeColor = SystemColors.ControlText;
      this.gcDocumentList.Location = new Point(0, 49);
      this.gcDocumentList.Name = "gcDocumentList";
      this.gcDocumentList.Size = new Size(872, 571);
      this.gcDocumentList.TabIndex = 4;
      this.gcDocumentList.Text = "Document List";
      this.btnArchiveSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnArchiveSelected.Location = new Point(690, 0);
      this.btnArchiveSelected.Name = "btnArchiveSelected";
      this.btnArchiveSelected.Size = new Size(106, 23);
      this.btnArchiveSelected.TabIndex = 14;
      this.btnArchiveSelected.Text = "&Archive Selected";
      this.btnArchiveSelected.UseVisualStyleBackColor = true;
      this.btnArchiveSelected.Click += new EventHandler(this.btnArchiveSelected_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(824, 6);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 13;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnView.BackColor = Color.Transparent;
      this.btnView.Enabled = false;
      this.btnView.Location = new Point(846, 6);
      this.btnView.MouseDownImage = (Image) null;
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(16, 16);
      this.btnView.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnView.TabIndex = 12;
      this.btnView.TabStop = false;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.gridViewDocumentList.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnActive";
      gvColumn1.SortMethod = GVSortMethod.Checkbox;
      gvColumn1.Text = "Active";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnDocDisplayName";
      gvColumn2.Text = "Document Display Name";
      gvColumn2.Width = 135;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnCategory";
      gvColumn3.Text = "Category";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnChannel";
      gvColumn4.Text = "Channel";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnStartDate";
      gvColumn5.SortMethod = GVSortMethod.Date;
      gvColumn5.Text = "Start Date";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 70;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnEndDate";
      gvColumn6.SortMethod = GVSortMethod.Date;
      gvColumn6.Text = "End Date";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 70;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnAddedBy";
      gvColumn7.Text = "Added By";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnTimeAdded";
      gvColumn8.SortMethod = GVSortMethod.DateTime;
      gvColumn8.Text = "Date/Time Added";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnStatus";
      gvColumn9.Text = "Status";
      gvColumn9.Width = 80;
      gvColumn10.ActivatedEditorType = GVActivatedEditorType.UserType;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnAssign";
      gvColumn10.SortMethod = GVSortMethod.Custom;
      gvColumn10.Text = "# Assign";
      gvColumn10.Width = 100;
      this.gridViewDocumentList.Columns.AddRange(new GVColumn[10]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.gridViewDocumentList.Dock = DockStyle.Fill;
      this.gridViewDocumentList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewDocumentList.Location = new Point(0, 26);
      this.gridViewDocumentList.Name = "gridViewDocumentList";
      this.gridViewDocumentList.Size = new Size(872, 545);
      this.gridViewDocumentList.TabIndex = 8;
      this.gridViewDocumentList.SelectedIndexChanged += new EventHandler(this.gridViewDocumentList_SelectedIndexChanged);
      this.gridViewDocumentList.ColumnClick += new GVColumnClickEventHandler(this.gridViewDocumentList_ColumnClick);
      this.gridViewDocumentList.SubItemCheck += new GVSubItemEventHandler(this.gridViewDocumentList_SubItemCheck);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(802, 6);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 9;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcDocumentList);
      this.Controls.Add((Control) this.pnlTop);
      this.Margin = new Padding(0);
      this.Name = nameof (EditActiveDocumentControl);
      this.Size = new Size(872, 620);
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.gcDocumentList.ResumeLayout(false);
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnView).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
