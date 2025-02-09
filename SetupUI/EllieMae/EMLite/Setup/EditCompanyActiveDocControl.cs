// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyActiveDocControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
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
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyActiveDocControl : UserControl
  {
    private Dictionary<int, List<DocumentSettingInfo>> documents;
    private Sessions.Session session;
    private List<ExternalSettingValue> settingsList;
    private AnimationProgress an;
    private Task<bool> t;
    private int companyOrgId = -1;
    private int orgId = -1;
    private bool isTPOTool;
    private bool hasTPODocDeleteRight = true;
    private bool hasTPODocAddEditRight = true;
    private bool hasTPODocDisableRight;
    private static string sw = Tracing.SwDataEngine;
    private const string className = "EditCompanyActiveDocControl";
    private string SourceTabText = string.Empty;
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
    private StandardIconButton btnDelete;
    private StandardIconButton btnUp;
    private StandardIconButton btnDown;
    private Label lblStatus;
    private Label lblChannelType;

    public EditCompanyActiveDocControl(
      Sessions.Session session,
      int orgID,
      int companyOrgId,
      bool isTPOTool,
      string sourceTabText)
    {
      this.session = session;
      this.SourceTabText = sourceTabText;
      this.settingsList = this.session.ConfigurationManager.GetExternalOrgSettingsByName("Document Category");
      this.settingsList.Insert(0, new ExternalSettingValue(-1, -1, "", "", 0));
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.btnView.Enabled = this.btnArchiveSelected.Enabled = this.btnEdit.Enabled = this.btnDelete.Enabled = this.btnDown.Enabled = this.btnUp.Enabled = false;
      if (companyOrgId == -1 || companyOrgId == 0)
        companyOrgId = orgID;
      this.companyOrgId = companyOrgId;
      if (companyOrgId != orgID)
      {
        this.btnAdd.Visible = false;
        this.btnEdit.Visible = false;
        this.btnArchiveSelected.Visible = false;
        this.btnDelete.Visible = false;
        this.btnUp.Visible = false;
        this.btnDown.Visible = false;
      }
      this.orgId = orgID;
      this.cboChannelType.SelectedIndex = 0;
      this.cboStatus.SelectedIndex = 0;
      FeaturesAclManager aclManager = (FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features);
      this.hasTPODocDisableRight = !session.UserInfo.IsSuperAdministrator() && aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDisableDocumentsTab);
      this.hasTPODocDeleteRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDeleteDocumentsTab);
      this.hasTPODocAddEditRight = aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCEditDocumentsTab);
      this.isTPOTool = isTPOTool;
      this.enforceAccessRights();
    }

    public EditCompanyActiveDocControl(SessionObjects obj)
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
      this.btnDelete.Enabled = false;
      this.btnArchiveSelected.Enabled = false;
    }

    public void RefreshData()
    {
      this.gridViewDocumentList.BeginUpdate();
      this.gridViewDocumentList.Items.Clear();
      KeyValuePair<int, List<DocumentSettingInfo>> categoryDocs = new KeyValuePair<int, List<DocumentSettingInfo>>();
      this.documents = this.session.ConfigurationManager.GetExternalOrgDocuments(this.companyOrgId, this.GetSelectedChannelType(), this.GetSelectedStatus(), this.hasTPODocDisableRight);
      foreach (KeyValuePair<int, List<DocumentSettingInfo>> document1 in this.documents)
      {
        if (document1.Value.Count > 0)
        {
          if (document1.Key == -1)
          {
            categoryDocs = document1;
          }
          else
          {
            this.gridViewDocumentList.Items.Add(this.createGVItemForCategory(document1));
            foreach (DocumentSettingInfo document2 in document1.Value)
              this.gridViewDocumentList.Items.Add(this.createGVItemForDocument(document2));
          }
        }
      }
      if (categoryDocs.Value != null && categoryDocs.Value.Count > 0)
      {
        this.gridViewDocumentList.Items.Add(this.createGVItemForCategory(categoryDocs));
        foreach (DocumentSettingInfo document in categoryDocs.Value)
          this.gridViewDocumentList.Items.Add(this.createGVItemForDocument(document));
      }
      this.gridViewDocumentList.ReSort();
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
      gvItemForDocument.SubItems[0].CheckBoxEnabled = this.btnAdd.Visible;
      if (this.isTPOTool)
        gvItemForDocument.SubItems[0].CheckBoxEnabled = false;
      gvItemForDocument.SubItems.Add((object) ("       " + document.DisplayName));
      if (document.IsDefault && document.AvailbleAllTPO)
        gvItemForDocument.SubItems.Add((object) "All TPOs");
      else if (document.IsDefault)
        gvItemForDocument.SubItems.Add((object) "Assigned");
      else
        gvItemForDocument.SubItems.Add((object) "Uploaded");
      gvItemForDocument.SubItems.Add((object) this.GetChannelStringValue(document.Channel));
      if (document.StartDate == DateTime.MinValue)
        gvItemForDocument.SubItems.Add((object) "");
      else
        gvItemForDocument.SubItems.Add((object) document.StartDate.ToString("d"));
      if (document.EndDate == DateTime.MinValue)
        gvItemForDocument.SubItems.Add((object) "");
      else
        gvItemForDocument.SubItems.Add((object) document.EndDate.ToString("d"));
      gvItemForDocument.SubItems.Add((object) document.AddedBy);
      gvItemForDocument.SubItems.Add((object) document.DateAdded);
      gvItemForDocument.SubItems.Add((object) this.GetStatusStringValue(document.Status));
      gvItemForDocument.Tag = (object) document;
      if (document.Status == ExternalOriginatorStatus.Expired)
        gvItemForDocument.ForeColor = Color.Red;
      return gvItemForDocument;
    }

    private Label createCategoryLabel(string labelVal, bool isBold)
    {
      Label categoryLabel = new Label();
      if (isBold)
        categoryLabel.Font = new Font(categoryLabel.Font, FontStyle.Bold);
      categoryLabel.Text = labelVal;
      return categoryLabel;
    }

    private GVItem createGVItemForCategory(
      KeyValuePair<int, List<DocumentSettingInfo>> categoryDocs)
    {
      string str = "No Category";
      if (categoryDocs.Key != -1 && this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == categoryDocs.Key)) != null)
        str = this.settingsList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (it => it.settingId == categoryDocs.Key)).settingValue;
      GVItem gvItemForCategory = new GVItem(new string[9]
      {
        "",
        str,
        "",
        "",
        "",
        "",
        "",
        "",
        ""
      });
      gvItemForCategory.SubItems[0].Checked = this.checkIfCategoryNeedsToBeActive(categoryDocs.Value);
      gvItemForCategory.SubItems[0].CheckBoxEnabled = this.btnAdd.Visible;
      if (this.isTPOTool)
        gvItemForCategory.SubItems[0].CheckBoxEnabled = false;
      gvItemForCategory.SubItems[1].Font = new Font(this.lblChannelType.Font, FontStyle.Bold);
      gvItemForCategory.Tag = (object) new DocumentSettingInfo()
      {
        Category = categoryDocs.Key,
        ExternalOrgId = -1
      };
      return gvItemForCategory;
    }

    private bool checkIfCategoryNeedsToBeActive(List<DocumentSettingInfo> docs)
    {
      return docs.Where<DocumentSettingInfo>((Func<DocumentSettingInfo, bool>) (doc => doc.Active)).ToList<DocumentSettingInfo>().Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (AddOrChooseTPODoc addOrChooseTpoDoc = new AddOrChooseTPODoc(this.hasTPODocDisableRight))
      {
        if (addOrChooseTpoDoc.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (addOrChooseTpoDoc.Value == 0)
        {
          using (AddEditTPODocument addEditTpoDocument = new AddEditTPODocument((DocumentSettingInfo) null, this.settingsList, this.session.UserInfo.FullName))
          {
            if (addEditTpoDocument.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            using (BinaryObject data = new BinaryObject(addEditTpoDocument.Path))
            {
              this.addProgressDocument(new FileSystemEntry("\\\\" + addEditTpoDocument.FileName, FileSystemEntry.Types.File, (string) null), data);
              addEditTpoDocument.Document.ExternalOrgId = this.companyOrgId;
              this.session.ConfigurationManager.AddDocument(this.companyOrgId, addEditTpoDocument.Document, addEditTpoDocument.IsTopOfCategory);
              WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
              this.RefreshData();
              try
              {
                using (EMSiteWebService emSiteWebService = new EMSiteWebService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.CenterwiseServicesUrl))
                {
                  emSiteWebService.Timeout = 15000;
                  string key = FormsAuthentication.HashPasswordForStoringInConfigFile(Session.CompanyInfo.ClientID + Session.CompanyInfo.ClientID, "sha1");
                  if (emSiteWebService.AddShowDocument(Session.CompanyInfo.ClientID, addEditTpoDocument.Document.Guid, data.GetBytes(), addEditTpoDocument.Document.FileName, key))
                    Tracing.Log(EditCompanyActiveDocControl.sw, nameof (EditCompanyActiveDocControl), TraceLevel.Info, "Document has been uploaded to TPO WebCenter. Guid:" + addEditTpoDocument.Document.Guid.ToString());
                  else
                    Tracing.Log(EditCompanyActiveDocControl.sw, nameof (EditCompanyActiveDocControl), TraceLevel.Error, "Document upload to TPO WebCenter Failed. Guid:" + addEditTpoDocument.Document.Guid.ToString());
                }
              }
              catch (Exception ex)
              {
                Tracing.Log(EditCompanyActiveDocControl.sw, nameof (EditCompanyActiveDocControl), TraceLevel.Error, "Document upload to TPO WebCenter Failed. Guid:" + addEditTpoDocument.Document.Guid.ToString() + ", Exception :" + ex.Message);
              }
            }
          }
        }
        else
        {
          List<DocumentSettingInfo> forOrgAssignment = this.session.ConfigurationManager.GetExternalDocumentsForOrgAssignment(this.companyOrgId);
          if (forOrgAssignment.Count == 0)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "There are no existing TPO documents or all TPO documents are already assigned to this TPO.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            using (AddDocumentFromSettings documentFromSettings = new AddDocumentFromSettings(forOrgAssignment, this.settingsList))
            {
              if (documentFromSettings.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              int num2 = (int) Utils.Dialog((IWin32Window) this, "Document added successfully!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              documentFromSettings.SelectedDoc.ExternalOrgId = this.companyOrgId;
              this.session.ConfigurationManager.AssignDocumentToOrg(documentFromSettings.SelectedDoc, documentFromSettings.IsTopOfCategory);
              WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
              this.RefreshData();
            }
          }
        }
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Count == 0)
        return;
      using (AddEditTPODocument addEditTpoDocument = new AddEditTPODocument((DocumentSettingInfo) this.gridViewDocumentList.SelectedItems[0].Tag, this.settingsList, this.session.UserInfo.FullName))
      {
        if (addEditTpoDocument.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.session.ConfigurationManager.UpdateDocument(this.companyOrgId, addEditTpoDocument.Document);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
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
      this.session.ConfigurationManager.ArchiveDocuments(this.companyOrgId, guids);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
      this.RefreshData();
    }

    private void gridViewDocumentList_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      DocumentSettingInfo tag = (DocumentSettingInfo) e.SubItem.Item.Tag;
      if (tag != null && tag.ExternalOrgId != -1)
      {
        tag.Active = e.SubItem.Checked;
        this.session.ConfigurationManager.UpdateActiveStatus(this.companyOrgId, tag.Active, tag.IsDefault, tag.Guid);
      }
      else
        this.session.ConfigurationManager.UpdateActiveStatusAllDocsInCategory(this.companyOrgId, tag.Category, e.SubItem.Checked);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
      this.RefreshData();
    }

    private void gridViewDocumentList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnView.Enabled = this.btnDelete.Enabled = this.btnEdit.Enabled = this.btnArchiveSelected.Enabled = this.btnUp.Enabled = this.btnDown.Enabled = this.gridViewDocumentList.SelectedItems.Count == 1;
      if (this.gridViewDocumentList.SelectedItems.Count != 1)
        return;
      DocumentSettingInfo tag = (DocumentSettingInfo) this.gridViewDocumentList.SelectedItems[0].Tag;
      if (this.gridViewDocumentList.SelectedItems[0].Index == 1)
        this.btnUp.Enabled = false;
      else
        this.btnUp.Enabled = true;
      if (this.gridViewDocumentList.SelectedItems[0].Index == this.gridViewDocumentList.Items.Count - 1)
        this.btnDown.Enabled = false;
      else
        this.btnDown.Enabled = true;
      if (this.gridViewDocumentList.SelectedItems[0].Index > 1 && ((DocumentSettingInfo) this.gridViewDocumentList.Items[this.gridViewDocumentList.SelectedItems[0].Index - 1].Tag).ExternalOrgId == -1)
        this.btnUp.Enabled = false;
      if (this.gridViewDocumentList.SelectedItems[0].Index != this.gridViewDocumentList.Items.Count - 1 && ((DocumentSettingInfo) this.gridViewDocumentList.Items[this.gridViewDocumentList.SelectedItems[0].Index + 1].Tag).ExternalOrgId == -1)
        this.btnDown.Enabled = false;
      this.setButtonStatus(tag);
      this.enforceAccessRights();
    }

    private void enforceAccessRights()
    {
      if (!this.hasTPODocDeleteRight)
        this.btnDelete.Enabled = false;
      if (!this.hasTPODocAddEditRight)
      {
        this.btnAdd.Enabled = false;
        this.btnEdit.Enabled = false;
        this.btnArchiveSelected.Enabled = false;
        this.btnUp.Enabled = false;
        this.btnDown.Enabled = false;
      }
      if (!this.isTPOTool)
        return;
      this.DisableControls();
    }

    private void setButtonStatus(DocumentSettingInfo document)
    {
      if (document.IsDefault || document.AvailbleAllTPO || document.ExternalOrgId == -1)
      {
        this.btnEdit.Enabled = false;
        this.btnArchiveSelected.Enabled = false;
        if (document.AvailbleAllTPO)
          this.btnDelete.Enabled = false;
        else
          this.btnDelete.Enabled = true;
        this.btnView.Enabled = true;
        if (document.ExternalOrgId != -1)
          return;
        this.btnUp.Enabled = false;
        this.btnDown.Enabled = false;
        this.btnView.Enabled = false;
        this.btnDelete.Enabled = false;
      }
      else
      {
        this.btnEdit.Enabled = true;
        this.btnArchiveSelected.Enabled = true;
        this.btnDelete.Enabled = true;
        this.btnView.Enabled = true;
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Count == 0)
        return;
      DocumentSettingInfo tag = (DocumentSettingInfo) this.gridViewDocumentList.SelectedItems[0].Tag;
      if (!tag.AvailbleAllTPO && tag.IsDefault)
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to unassign the selected document?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        this.session.ConfigurationManager.RemoveAssignedDocFromTPO(tag.Guid.ToString(), this.companyOrgId);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected document?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        // ISSUE: variable of a boxed type
        __Boxed<Guid> guid = (ValueType) tag.Guid;
        string str = ((IEnumerable<string>) tag.FileName.Split('.')).Last<string>();
        FileSystemEntry entry = new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), FileSystemEntry.Types.File, (string) null);
        this.session.ConfigurationManager.DeleteDocument(this.companyOrgId, tag.Guid, entry);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
      }
      this.RefreshData();
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Count != 1)
        return;
      int index = this.gridViewDocumentList.SelectedItems[0].Index;
      this.SwapDocuments(index, index - 1);
    }

    private void SwapDocuments(int firstDocIndex, int secondDocIndex)
    {
      this.session.ConfigurationManager.SwapDocumentSortIds(this.companyOrgId, (DocumentSettingInfo) this.gridViewDocumentList.Items[firstDocIndex].Tag, (DocumentSettingInfo) this.gridViewDocumentList.Items[secondDocIndex].Tag);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.companyOrgId);
      this.gridViewDocumentList.BeginUpdate();
      this.RefreshData();
      this.gridViewDocumentList.Items[secondDocIndex].Selected = true;
      this.gridViewDocumentList.EndUpdate();
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      if (this.gridViewDocumentList.SelectedItems.Count != 1)
        return;
      int index = this.gridViewDocumentList.SelectedItems[0].Index;
      this.SwapDocuments(index, index + 1);
    }

    public void DisableControls()
    {
      this.btnAdd.Visible = false;
      this.btnEdit.Visible = false;
      this.btnArchiveSelected.Visible = false;
      this.btnDelete.Visible = false;
      this.btnUp.Visible = false;
      this.btnDown.Visible = false;
      this.disableControl(this.Controls);
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      this.cboChannelType = new ComboBox();
      this.cboStatus = new ComboBox();
      this.pnlTop = new GradientPanel();
      this.lblStatus = new Label();
      this.lblChannelType = new Label();
      this.gcDocumentList = new GroupContainer();
      this.btnDown = new StandardIconButton();
      this.btnUp = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnArchiveSelected = new Button();
      this.btnEdit = new StandardIconButton();
      this.btnView = new StandardIconButton();
      this.gridViewDocumentList = new GridView();
      this.btnAdd = new StandardIconButton();
      this.pnlTop.SuspendLayout();
      this.gcDocumentList.SuspendLayout();
      ((ISupportInitialize) this.btnDown).BeginInit();
      ((ISupportInitialize) this.btnUp).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
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
      this.cboChannelType.Location = new Point(93, 13);
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
      this.cboStatus.Location = new Point(358, 13);
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
      this.lblStatus.Location = new Point(306, 18);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(40, 13);
      this.lblStatus.TabIndex = 40;
      this.lblStatus.Text = "Status:";
      this.lblChannelType.AutoSize = true;
      this.lblChannelType.BackColor = Color.Transparent;
      this.lblChannelType.Location = new Point(7, 18);
      this.lblChannelType.Name = "lblChannelType";
      this.lblChannelType.Size = new Size(76, 13);
      this.lblChannelType.TabIndex = 39;
      this.lblChannelType.Text = "Channel Type:";
      this.gcDocumentList.Borders = AnchorStyles.Top;
      this.gcDocumentList.Controls.Add((Control) this.btnDown);
      this.gcDocumentList.Controls.Add((Control) this.btnUp);
      this.gcDocumentList.Controls.Add((Control) this.btnDelete);
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
      this.btnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDown.BackColor = Color.Transparent;
      this.btnDown.Location = new Point(750, 4);
      this.btnDown.MouseDownImage = (Image) null;
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(16, 16);
      this.btnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnDown.TabIndex = 17;
      this.btnDown.TabStop = false;
      this.btnDown.Click += new EventHandler(this.btnDown_Click);
      this.btnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUp.BackColor = Color.Transparent;
      this.btnUp.Location = new Point(728, 4);
      this.btnUp.MouseDownImage = (Image) null;
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(16, 16);
      this.btnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnUp.TabIndex = 16;
      this.btnUp.TabStop = false;
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(851, 4);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 15;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnArchiveSelected.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnArchiveSelected.Location = new Point(611, 0);
      this.btnArchiveSelected.Name = "btnArchiveSelected";
      this.btnArchiveSelected.Size = new Size(106, 23);
      this.btnArchiveSelected.TabIndex = 14;
      this.btnArchiveSelected.Text = "&Archive Selected";
      this.btnArchiveSelected.UseVisualStyleBackColor = true;
      this.btnArchiveSelected.Click += new EventHandler(this.btnArchiveSelected_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(807, 4);
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
      this.btnView.Location = new Point(829, 4);
      this.btnView.MouseDownImage = (Image) null;
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(16, 16);
      this.btnView.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnView.TabIndex = 12;
      this.btnView.TabStop = false;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.gridViewDocumentList.AllowMultiselect = false;
      this.gridViewDocumentList.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnActive";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "Active";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnCategoryDocDisplayName";
      gvColumn2.SortMethod = GVSortMethod.None;
      gvColumn2.Text = "Category & Document Display Name";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnSource";
      gvColumn3.SortMethod = GVSortMethod.None;
      gvColumn3.Text = "Source";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnChannel";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "Channel";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnStartDate";
      gvColumn5.SortMethod = GVSortMethod.None;
      gvColumn5.Text = "Start Date";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 70;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnEndDate";
      gvColumn6.SortMethod = GVSortMethod.None;
      gvColumn6.Text = "End Date";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 70;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnAddedBy";
      gvColumn7.SortMethod = GVSortMethod.None;
      gvColumn7.Text = "Added By";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnTimeAdded";
      gvColumn8.SortMethod = GVSortMethod.None;
      gvColumn8.Text = "Date/Time Added";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 110;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnStatus";
      gvColumn9.SortMethod = GVSortMethod.None;
      gvColumn9.Text = "Status";
      gvColumn9.Width = 80;
      this.gridViewDocumentList.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gridViewDocumentList.Dock = DockStyle.Fill;
      this.gridViewDocumentList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewDocumentList.Location = new Point(0, 26);
      this.gridViewDocumentList.Name = "gridViewDocumentList";
      this.gridViewDocumentList.Padding = new Padding(0, 0, 0, 5);
      this.gridViewDocumentList.Size = new Size(872, 545);
      this.gridViewDocumentList.SortOption = GVSortOption.None;
      this.gridViewDocumentList.TabIndex = 8;
      this.gridViewDocumentList.SelectedIndexChanged += new EventHandler(this.gridViewDocumentList_SelectedIndexChanged);
      this.gridViewDocumentList.SubItemCheck += new GVSubItemEventHandler(this.gridViewDocumentList_SubItemCheck);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(785, 4);
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
      this.Name = nameof (EditCompanyActiveDocControl);
      this.Size = new Size(872, 620);
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.gcDocumentList.ResumeLayout(false);
      ((ISupportInitialize) this.btnDown).EndInit();
      ((ISupportInitialize) this.btnUp).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnView).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.ResumeLayout(false);
    }
  }
}
