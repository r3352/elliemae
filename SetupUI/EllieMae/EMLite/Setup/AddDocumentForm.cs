// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddDocumentForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddDocumentForm : Form
  {
    private bool edit;
    private OpenFileDialog openFileDialog1 = new OpenFileDialog();
    private DocumentSettingInfo document;
    private string userName;
    private string filePath;
    private Guid guid = Guid.NewGuid();
    public bool DocumentChanged;
    public bool IsAssignToRelatedDoc;
    public List<int> externalOrgIds = new List<int>();
    public DocumentSettingInfo relatedDocument;
    public DocumentPlacement documentPlacement;
    private List<DocumentSettingInfo> availableRelatedDocList = new List<DocumentSettingInfo>();
    public AssignTo assignTo;
    private Sessions.Session session;
    private List<ExternalSettingValue> docCategories;
    private string fileSize;
    private string vaultFileId;
    private IContainer components;
    private Label label8;
    private TextBox txtFileName;
    private Label lblFile;
    private IconButton btnAttachBrowse;
    private TextBox txtDocDisplayName;
    private Label lblDocDisplayName;
    private DatePicker dtpStartDate;
    private DatePicker dtpEndDate;
    private Label lblStartDate;
    private Label lblEndDate;
    private ComboBox cboCategory;
    private Label label1;
    private Label lblChannel;
    private ComboBox cboChannel;
    private Button btnOK;
    private Button btnCancel;
    private Label label2;
    private Label lblAssignTo;
    private ComboBox cboAssignto;
    private Panel pnlAccessRelatedDoc;
    private ComboBox cboCategoryToMove;
    private Label lblMovetoCategory;
    private RadioButton rdoArchiveNo;
    private RadioButton rdoArchiveYes;
    private Label lblArchivedoc;
    private GroupContainer grpAccessRelatedDoc;
    private GridView grdViewDocs;
    private StandardIconButton stdBtnSearchDocs;
    private TextBox txtSearchTpoDoc;
    private Panel pnlChooseTpoCompanies;
    private GroupContainer grpChooseTpoCompanies;
    private StandardIconButton stdBtnSearchTpos;
    private GridView grdViewTpos;
    private Panel panel1;
    private Panel panel2;
    private ComboBox cboPlaceDocument_relatedDocs;
    private Label lblPlaceDocument;
    private Panel panel3;
    private ComboBox cboPlaceDocument_selctTpos;
    private Label label3;
    private RadioButton rdoAvailableToAllTposNo;
    private RadioButton rdoAvailableToAllTposYes;
    private Label label4;

    public DocumentSettingInfo Document => this.document;

    public string Path => this.filePath;

    public string FileName
    {
      get
      {
        __Boxed<Guid> guid = (ValueType) this.guid;
        string str = ((IEnumerable<string>) this.filePath.Trim().Split('.')).Last<string>();
        return guid.ToString() + "." + str;
      }
    }

    public AddDocumentForm(
      DocumentSettingInfo document,
      List<ExternalSettingValue> documentCategories,
      string userName,
      Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.Height = 660;
      this.edit = document != null;
      this.userName = userName;
      this.document = document;
      this.docCategories = documentCategories;
      if (this.edit)
        this.guid = document.Guid;
      this.cboCategory.DataSource = (object) documentCategories;
      this.cboCategory.DisplayMember = "settingValue";
      this.cboCategory.ValueMember = "settingId";
      foreach (object documentCategory in documentCategories)
        this.cboCategoryToMove.Items.Add(documentCategory);
      this.cboCategoryToMove.DisplayMember = "settingValue";
      this.rdoAvailableToAllTposNo.Checked = true;
      this.cboAssignto.SelectedIndex = 0;
      if (this.edit)
      {
        this.Text = "Edit Document";
        this.populateFields();
        this.pnlAccessRelatedDoc.Visible = false;
        this.pnlChooseTpoCompanies.Visible = false;
        this.cboAssignto.Visible = false;
        this.lblAssignTo.Visible = false;
        this.vaultFileId = document.VaultFileId;
      }
      else
      {
        this.Text = "Add New Document";
        this.cboChannel.SelectedIndex = 0;
      }
      this.cboPlaceDocument_relatedDocs.SelectedIndex = 0;
    }

    private void populateFields()
    {
      this.txtFileName.Text = this.document.FileName;
      this.txtDocDisplayName.Text = this.document.DisplayName;
      this.dtpStartDate.Value = this.document.StartDate;
      this.dtpEndDate.Value = this.document.EndDate;
      this.cboCategory.SelectedValue = (object) this.document.Category;
      this.cboChannel.SelectedIndex = this.GetChannelInt(this.document.Channel);
      if (this.document.AvailbleAllTPO)
        this.rdoAvailableToAllTposYes.Checked = true;
      else
        this.rdoAvailableToAllTposNo.Checked = true;
      this.btnAttachBrowse.Visible = false;
    }

    private bool validateValue()
    {
      if (this.txtFileName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The file has not been specified.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.btnAttachBrowse_Click((object) null, (EventArgs) null);
        return false;
      }
      if (this.txtDocDisplayName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Document Display Name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtDocDisplayName.Focus();
        return false;
      }
      if (this.dtpStartDate.Value < DateTime.MinValue || this.dtpStartDate.Value > DateTime.MaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for Start Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.dtpStartDate.Focus();
        return false;
      }
      if (this.dtpEndDate.Value < DateTime.MinValue || this.dtpEndDate.Value > DateTime.MaxValue)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for End Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.dtpEndDate.Focus();
        return false;
      }
      if (!(this.dtpEndDate.Value < this.dtpStartDate.Value) || !(this.dtpStartDate.Value != DateTime.MinValue) || !(this.dtpEndDate.Value != DateTime.MinValue))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Start Date cannot be greater than End Date.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      this.dtpStartDate.Focus();
      return false;
    }

    private void btnAttachBrowse_Click(object sender, EventArgs e)
    {
      if (this.edit && Utils.Dialog((IWin32Window) this, "If you select another source file, the original file will be overwritten. Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      string[] array = new string[12]
      {
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".txt",
        ".tif",
        ".jpg",
        ".jpeg",
        ".jpe",
        ".csv",
        ".xml"
      };
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Title = "Browse and Attach";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = false;
        openFileDialog.ShowReadOnly = false;
        openFileDialog.Filter = "All Supported Formats|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt;*.tif;*.jpg;*.jpeg;*.jpe;*.csv;*.xml|Adobe PDF Documents (*.pdf)|*.pdf|Microsoft Word Documents (*.doc,*.docx)|*.doc;*.docx|Microsoft Excel Documents (*.xls,*.xlsx)|*.xls;*.xlsx|Text Documents (*.txt)|*.txt|TIFF Images (*.tif)|*.tif|JPEG Images (*.jpg, *.jpeg, *.jpe)|*.jpg;*.jpeg;*.jpe|CSV Files (*.csv)|*.csv|XML Files (*.xml)|*.xml";
        if (openFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        string str = System.IO.Path.GetExtension(openFileDialog.FileName).ToLower().Trim();
        if (Array.IndexOf<string>(array, str) < 0)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The '" + str + "' file type is not supported. The allowed file types are '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.txt', '.tif', '.jpg', '.jpeg', '.jpe', '.csv', and '.xml'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
          if (fileInfo.Length > 25000000L)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "File attachments cannot exceed 25 MB. Please select another file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            this.fileSize = Utils.FormatByteSize(fileInfo.Length);
            this.setFileName(openFileDialog.FileName);
          }
        }
      }
    }

    private void setFileName(string path)
    {
      this.filePath = path;
      this.txtFileName.Text = ((IEnumerable<string>) path.Trim().Split('\\')).Last<string>();
      this.DocumentChanged = true;
    }

    private void txtFileName_TextChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.txtFileName.Text.Trim() != string.Empty;
      if ((!this.grdViewDocs.Visible || this.grdViewDocs.SelectedItems.Any<GVItem>() && !(this.txtDocDisplayName.Text == string.Empty)) && (!this.grdViewTpos.Visible || this.grdViewTpos.Items.Any<GVItem>() && !(this.txtDocDisplayName.Text == string.Empty)))
        return;
      this.btnOK.Enabled = false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.validateValue())
        return;
      int num = this.cboCategory.SelectedIndex <= 0 ? -1 : ((ExternalSettingValue) this.cboCategory.SelectedItem).settingId;
      if (this.document == null)
      {
        this.document = new DocumentSettingInfo();
        this.document.Active = true;
        this.document.Guid = this.guid;
      }
      this.document.FileName = this.txtFileName.Text.Trim();
      this.document.DisplayName = this.txtDocDisplayName.Text.Trim();
      this.document.StartDate = this.dtpStartDate.Value;
      this.document.EndDate = this.dtpEndDate.Value;
      this.document.Category = num;
      this.document.Channel = this.GetSelectedChannel();
      this.document.DateAdded = DateTime.Now;
      this.document.AvailbleAllTPO = this.rdoAvailableToAllTposYes.Checked;
      this.document.AddedBy = this.userName;
      this.document.IsArchive = false;
      this.document.FileSize = this.fileSize;
      this.document.VaultFileId = this.vaultFileId;
      this.AssignDocument();
      this.DialogResult = DialogResult.OK;
    }

    private void AssignDocument()
    {
      if (!this.cboAssignto.Visible)
        return;
      if (this.cboAssignto.SelectedIndex == 1)
      {
        this.IsAssignToRelatedDoc = true;
        this.externalOrgIds.Clear();
        this.externalOrgIds = this.session.ConfigurationManager.GetExternalOrgsByDocumentGuid(this.relatedDocument.Guid);
        this.SetDocumentPlacement();
        this.SetAssignTo();
        if (this.cboCategoryToMove.SelectedIndex != -1)
        {
          this.relatedDocument.Category = ((ExternalSettingValue) this.cboCategoryToMove.SelectedItem).settingId;
          this.session.ConfigurationManager.UpdateDocument(-1, this.relatedDocument);
        }
        if (!this.rdoArchiveYes.Checked)
          return;
        this.session.ConfigurationManager.ArchiveDocuments(-1, new List<string>()
        {
          this.relatedDocument.Guid.ToString()
        });
      }
      else
      {
        this.SetDocumentPlacement();
        this.SetAssignTo();
        this.externalOrgIds.Clear();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.grdViewTpos.Items)
          this.externalOrgIds.Add(((ExternalOriginatorManagementData) gvItem.Tag).oid);
      }
    }

    public void SetAssignTo()
    {
      if (this.rdoAvailableToAllTposYes.Checked)
        this.assignTo = AssignTo.All;
      else if (this.cboAssignto.SelectedIndex == 0)
        this.assignTo = AssignTo.None;
      else if (this.cboAssignto.SelectedIndex == 1)
      {
        this.assignTo = AssignTo.RelatedDocs;
      }
      else
      {
        if (this.cboAssignto.SelectedIndex != 2)
          return;
        this.assignTo = AssignTo.SelectTpos;
      }
    }

    public void SetDocumentPlacement()
    {
      if (this.cboPlaceDocument_relatedDocs.Visible)
      {
        if (this.cboPlaceDocument_relatedDocs.SelectedIndex == 0)
          this.documentPlacement = DocumentPlacement.TopofRelatedDoc;
        else if (this.cboPlaceDocument_relatedDocs.SelectedIndex == 1)
          this.documentPlacement = DocumentPlacement.BottomOfRelatedDoc;
        else if (this.cboPlaceDocument_relatedDocs.SelectedIndex == 2)
        {
          this.documentPlacement = DocumentPlacement.TopOfCategory;
        }
        else
        {
          if (this.cboPlaceDocument_relatedDocs.SelectedIndex != 3)
            return;
          this.documentPlacement = DocumentPlacement.BottomOfCategory;
        }
      }
      else
      {
        if (!this.cboPlaceDocument_selctTpos.Visible)
          return;
        if (this.cboPlaceDocument_selctTpos.SelectedIndex == 0)
          this.documentPlacement = DocumentPlacement.TopOfCategory;
        else
          this.documentPlacement = DocumentPlacement.BottomOfCategory;
      }
    }

    public ExternalOriginatorEntityType GetSelectedChannel()
    {
      if (this.cboChannel.SelectedItem != null && !(this.cboChannel.SelectedItem.ToString() == "All"))
      {
        if (this.cboChannel.SelectedItem.ToString() == "Broker")
          return ExternalOriginatorEntityType.Broker;
        if (this.cboChannel.SelectedItem.ToString() == "Correspondent")
          return ExternalOriginatorEntityType.Correspondent;
      }
      return ExternalOriginatorEntityType.Both;
    }

    public int GetChannelInt(ExternalOriginatorEntityType channelType)
    {
      int channelInt = 0;
      switch (channelType)
      {
        case ExternalOriginatorEntityType.Broker:
        case ExternalOriginatorEntityType.Correspondent:
          channelInt = (int) channelType;
          break;
        case ExternalOriginatorEntityType.Both:
          channelInt = 0;
          break;
      }
      return channelInt;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void cboAssignto_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboAssignto.SelectedIndex == 1)
      {
        this.rdoAvailableToAllTposNo.Checked = true;
        this.availableRelatedDocList = this.session.ConfigurationManager.GetExternalDocuments(-1, -1, -1);
        this.PopulateTpoDocs(this.availableRelatedDocList);
        this.pnlChooseTpoCompanies.Visible = false;
        this.pnlAccessRelatedDoc.Visible = true;
        this.Height = 660;
        this.btnOK.Enabled = false;
      }
      else if (this.cboAssignto.SelectedIndex == 2)
      {
        this.rdoAvailableToAllTposNo.Checked = true;
        this.pnlAccessRelatedDoc.Visible = false;
        this.pnlChooseTpoCompanies.Visible = true;
        this.Height = 660;
        if (this.grdViewTpos.Items.Any<GVItem>() && this.txtDocDisplayName.Text != string.Empty && this.txtFileName.Text != string.Empty)
          this.btnOK.Enabled = true;
        else
          this.btnOK.Enabled = false;
      }
      else
      {
        this.pnlAccessRelatedDoc.Visible = false;
        this.pnlChooseTpoCompanies.Visible = false;
        this.Height = 330;
        this.btnOK.Enabled = true;
      }
    }

    private void PopluateTpos()
    {
      TPOList tpoList = new TPOList(this.session.ConfigurationManager.GetAllExternalParentOrganizations(false), this.grdViewTpos.Items.Select<GVItem, ExternalOriginatorManagementData>((Func<GVItem, ExternalOriginatorManagementData>) (item => (ExternalOriginatorManagementData) item.Tag)).ToList<ExternalOriginatorManagementData>());
      if (tpoList.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      List<ExternalOriginatorManagementData> selectedCompanies = tpoList.selectedCompanies;
      this.grdViewTpos.Items.Clear();
      foreach (ExternalOriginatorManagementData originatorManagementData in selectedCompanies)
        this.grdViewTpos.Items.Add(new GVItem()
        {
          SubItems = {
            (object) originatorManagementData.OrganizationName
          },
          Tag = (object) originatorManagementData
        });
    }

    private void PopulateTpoDocs(List<DocumentSettingInfo> documents)
    {
      this.grdViewDocs.Items.Clear();
      foreach (DocumentSettingInfo document in documents)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) document.DisplayName);
        gvItem.SubItems.Add((object) this.GetDocCategory(document));
        gvItem.SubItems.Add((object) document.ChannelStr);
        GVSubItemCollection subItems1 = gvItem.SubItems;
        DateTime dateTime;
        string str1;
        if (!(document.StartDate != DateTime.MinValue))
        {
          str1 = "";
        }
        else
        {
          dateTime = document.StartDate;
          str1 = dateTime.ToString("d");
        }
        subItems1.Add((object) str1);
        GVSubItemCollection subItems2 = gvItem.SubItems;
        string str2;
        if (!(document.EndDate != DateTime.MinValue))
        {
          str2 = "";
        }
        else
        {
          dateTime = document.EndDate;
          str2 = dateTime.ToString("d");
        }
        subItems2.Add((object) str2);
        gvItem.SubItems.Add((object) document.StatusStr);
        gvItem.Tag = (object) document;
        this.grdViewDocs.Items.Add(gvItem);
      }
    }

    private void grdViewDocs_Click(object sender, EventArgs e)
    {
      if (this.grdViewDocs.SelectedItems.Any<GVItem>())
      {
        this.relatedDocument = (DocumentSettingInfo) this.grdViewDocs.SelectedItems[0].Tag;
        if (this.relatedDocument.AvailbleAllTPO)
        {
          if (Utils.Dialog((IWin32Window) this, "The document you have selected is available to all TPOs. If you proceed, the current document will also be assigned to all TPOs. Would you like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
          {
            this.rdoAvailableToAllTposYes.Checked = true;
          }
          else
          {
            this.grdViewDocs.SelectedItems.Clear();
            this.btnOK.Enabled = false;
            return;
          }
        }
        this.btnOK.Enabled = true;
      }
      else
        this.btnOK.Enabled = false;
    }

    private string GetDocCategory(DocumentSettingInfo doc)
    {
      string docCategory = "";
      if (this.docCategories != null && this.docCategories.Any<ExternalSettingValue>())
      {
        ExternalSettingValue externalSettingValue = this.docCategories.Where<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (a => a.settingId == doc.Category)).SingleOrDefault<ExternalSettingValue>();
        if (externalSettingValue != null)
          docCategory = externalSettingValue.settingValue;
      }
      return docCategory;
    }

    private void stdBtnSearchDocs_Click(object sender, EventArgs e)
    {
      if (this.txtSearchTpoDoc.Text == string.Empty)
        this.PopulateTpoDocs(this.availableRelatedDocList);
      else
        this.PopulateTpoDocs(this.availableRelatedDocList.Where<DocumentSettingInfo>((Func<DocumentSettingInfo, bool>) (a => a.DisplayName.ToUpper().Contains(this.txtSearchTpoDoc.Text.ToUpper()))).ToList<DocumentSettingInfo>());
    }

    private void rdoAvailableToAllTposYes_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoAvailableToAllTposYes.Checked)
      {
        this.pnlAccessRelatedDoc.Visible = false;
        this.pnlChooseTpoCompanies.Visible = false;
        this.cboAssignto.Visible = false;
        this.lblAssignTo.Visible = false;
        this.Height = 320;
        this.btnOK.Enabled = true;
      }
      else
      {
        if (this.edit)
          return;
        this.cboAssignto.Visible = true;
        this.lblAssignTo.Visible = true;
        this.Height = 330;
        this.cboAssignto.SelectedIndex = 0;
      }
    }

    private void rdoAvailableToAllTposNo_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdoAvailableToAllTposYes.Checked)
      {
        this.pnlAccessRelatedDoc.Visible = false;
        this.pnlChooseTpoCompanies.Visible = false;
        this.cboAssignto.Visible = false;
        this.lblAssignTo.Visible = false;
        this.Height = 320;
        this.btnOK.Enabled = true;
      }
      else
      {
        if (this.edit)
          return;
        this.cboAssignto.Visible = true;
        this.lblAssignTo.Visible = true;
        this.Height = 330;
        this.cboAssignto.SelectedIndex = 0;
      }
    }

    private void stdBtnSearchTpos_Click(object sender, EventArgs e)
    {
      this.PopluateTpos();
      if (this.grdViewTpos.Items.Any<GVItem>() && this.txtDocDisplayName.Text != string.Empty && this.txtFileName.Text != string.Empty)
        this.btnOK.Enabled = true;
      else
        this.btnOK.Enabled = false;
    }

    private void txtDocDisplayName_TextChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.txtDocDisplayName.Text.Trim() != string.Empty;
      if ((!this.grdViewDocs.Visible || this.grdViewDocs.SelectedItems.Any<GVItem>() && !(this.txtFileName.Text == string.Empty)) && (!this.grdViewTpos.Visible || this.grdViewTpos.Items.Any<GVItem>() && !(this.txtFileName.Text == string.Empty)))
        return;
      this.btnOK.Enabled = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddDocumentForm));
      this.label8 = new Label();
      this.txtFileName = new TextBox();
      this.lblFile = new Label();
      this.txtDocDisplayName = new TextBox();
      this.lblDocDisplayName = new Label();
      this.lblStartDate = new Label();
      this.lblEndDate = new Label();
      this.cboCategory = new ComboBox();
      this.label1 = new Label();
      this.lblChannel = new Label();
      this.cboChannel = new ComboBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label2 = new Label();
      this.lblAssignTo = new Label();
      this.cboAssignto = new ComboBox();
      this.pnlAccessRelatedDoc = new Panel();
      this.cboCategoryToMove = new ComboBox();
      this.lblMovetoCategory = new Label();
      this.rdoArchiveNo = new RadioButton();
      this.rdoArchiveYes = new RadioButton();
      this.lblArchivedoc = new Label();
      this.grpAccessRelatedDoc = new GroupContainer();
      this.grdViewDocs = new GridView();
      this.stdBtnSearchDocs = new StandardIconButton();
      this.txtSearchTpoDoc = new TextBox();
      this.panel2 = new Panel();
      this.cboPlaceDocument_relatedDocs = new ComboBox();
      this.lblPlaceDocument = new Label();
      this.pnlChooseTpoCompanies = new Panel();
      this.grpChooseTpoCompanies = new GroupContainer();
      this.stdBtnSearchTpos = new StandardIconButton();
      this.grdViewTpos = new GridView();
      this.panel3 = new Panel();
      this.cboPlaceDocument_selctTpos = new ComboBox();
      this.label3 = new Label();
      this.panel1 = new Panel();
      this.rdoAvailableToAllTposNo = new RadioButton();
      this.rdoAvailableToAllTposYes = new RadioButton();
      this.label4 = new Label();
      this.btnAttachBrowse = new IconButton();
      this.dtpStartDate = new DatePicker();
      this.dtpEndDate = new DatePicker();
      this.pnlAccessRelatedDoc.SuspendLayout();
      this.grpAccessRelatedDoc.SuspendLayout();
      ((ISupportInitialize) this.stdBtnSearchDocs).BeginInit();
      this.panel2.SuspendLayout();
      this.pnlChooseTpoCompanies.SuspendLayout();
      this.grpChooseTpoCompanies.SuspendLayout();
      ((ISupportInitialize) this.stdBtnSearchTpos).BeginInit();
      this.panel3.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.btnAttachBrowse).BeginInit();
      this.SuspendLayout();
      this.label8.AutoSize = true;
      this.label8.ForeColor = Color.Red;
      this.label8.Location = new Point(86, 16);
      this.label8.Name = "label8";
      this.label8.Size = new Size(11, 13);
      this.label8.TabIndex = 58;
      this.label8.Text = "*";
      this.txtFileName.Location = new Point(176, 13);
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.ReadOnly = true;
      this.txtFileName.Size = new Size(372, 20);
      this.txtFileName.TabIndex = 57;
      this.txtFileName.TextChanged += new EventHandler(this.txtFileName_TextChanged);
      this.lblFile.AutoSize = true;
      this.lblFile.Location = new Point(32, 16);
      this.lblFile.Name = "lblFile";
      this.lblFile.Size = new Size(54, 13);
      this.lblFile.TabIndex = 56;
      this.lblFile.Text = "File Name";
      this.txtDocDisplayName.Location = new Point(176, 39);
      this.txtDocDisplayName.MaxLength = 250;
      this.txtDocDisplayName.Name = "txtDocDisplayName";
      this.txtDocDisplayName.Size = new Size(372, 20);
      this.txtDocDisplayName.TabIndex = 59;
      this.txtDocDisplayName.TextChanged += new EventHandler(this.txtDocDisplayName_TextChanged);
      this.lblDocDisplayName.AutoSize = true;
      this.lblDocDisplayName.Location = new Point(32, 42);
      this.lblDocDisplayName.Name = "lblDocDisplayName";
      this.lblDocDisplayName.Size = new Size(124, 13);
      this.lblDocDisplayName.TabIndex = 60;
      this.lblDocDisplayName.Text = "Document Display Name";
      this.lblStartDate.AutoSize = true;
      this.lblStartDate.Location = new Point(32, 65);
      this.lblStartDate.Name = "lblStartDate";
      this.lblStartDate.Size = new Size(55, 13);
      this.lblStartDate.TabIndex = 63;
      this.lblStartDate.Text = "Start Date";
      this.lblEndDate.AutoSize = true;
      this.lblEndDate.Location = new Point(32, 94);
      this.lblEndDate.Name = "lblEndDate";
      this.lblEndDate.Size = new Size(52, 13);
      this.lblEndDate.TabIndex = 64;
      this.lblEndDate.Text = "End Date";
      this.cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategory.FormattingEnabled = true;
      this.cboCategory.Location = new Point(176, 121);
      this.cboCategory.Name = "cboCategory";
      this.cboCategory.Size = new Size(208, 21);
      this.cboCategory.TabIndex = 65;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(32, 124);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 13);
      this.label1.TabIndex = 66;
      this.label1.Text = "Category";
      this.lblChannel.AutoSize = true;
      this.lblChannel.Location = new Point(32, 151);
      this.lblChannel.Name = "lblChannel";
      this.lblChannel.Size = new Size(46, 13);
      this.lblChannel.TabIndex = 68;
      this.lblChannel.Text = "Channel";
      this.cboChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboChannel.FormattingEnabled = true;
      this.cboChannel.Items.AddRange(new object[3]
      {
        (object) "All",
        (object) "Broker",
        (object) "Correspondent"
      });
      this.cboChannel.Location = new Point(176, 148);
      this.cboChannel.Name = "cboChannel";
      this.cboChannel.Size = new Size(208, 21);
      this.cboChannel.TabIndex = 67;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(441, 829);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 71;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(543, 829);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 72;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label2.AutoSize = true;
      this.label2.ForeColor = Color.Red;
      this.label2.Location = new Point(156, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(11, 13);
      this.label2.TabIndex = 73;
      this.label2.Text = "*";
      this.lblAssignTo.AutoSize = true;
      this.lblAssignTo.Location = new Point(32, 206);
      this.lblAssignTo.Name = "lblAssignTo";
      this.lblAssignTo.Size = new Size(50, 13);
      this.lblAssignTo.TabIndex = 74;
      this.lblAssignTo.Text = "Assign to";
      this.cboAssignto.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAssignto.FormattingEnabled = true;
      this.cboAssignto.Items.AddRange(new object[3]
      {
        (object) "None",
        (object) "TPOs accessing related document",
        (object) "Select TPOs"
      });
      this.cboAssignto.Location = new Point(176, 203);
      this.cboAssignto.Name = "cboAssignto";
      this.cboAssignto.Size = new Size(243, 21);
      this.cboAssignto.TabIndex = 75;
      this.cboAssignto.SelectedIndexChanged += new EventHandler(this.cboAssignto_SelectedIndexChanged);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.cboCategoryToMove);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.lblMovetoCategory);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.rdoArchiveNo);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.rdoArchiveYes);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.lblArchivedoc);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.grpAccessRelatedDoc);
      this.pnlAccessRelatedDoc.Controls.Add((Control) this.panel2);
      this.pnlAccessRelatedDoc.Dock = DockStyle.Top;
      this.pnlAccessRelatedDoc.Location = new Point(0, 230);
      this.pnlAccessRelatedDoc.Name = "pnlAccessRelatedDoc";
      this.pnlAccessRelatedDoc.Size = new Size(641, 310);
      this.pnlAccessRelatedDoc.TabIndex = 78;
      this.cboCategoryToMove.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCategoryToMove.FormattingEnabled = true;
      this.cboCategoryToMove.Location = new Point(212, 282);
      this.cboCategoryToMove.Name = "cboCategoryToMove";
      this.cboCategoryToMove.Size = new Size(192, 21);
      this.cboCategoryToMove.TabIndex = 78;
      this.lblMovetoCategory.AutoSize = true;
      this.lblMovetoCategory.Location = new Point(34, 285);
      this.lblMovetoCategory.Name = "lblMovetoCategory";
      this.lblMovetoCategory.Size = new Size(176, 13);
      this.lblMovetoCategory.TabIndex = 4;
      this.lblMovetoCategory.Text = "Change chosen document category";
      this.lblMovetoCategory.TextAlign = ContentAlignment.TopCenter;
      this.rdoArchiveNo.AutoSize = true;
      this.rdoArchiveNo.Location = new Point(261, 261);
      this.rdoArchiveNo.Name = "rdoArchiveNo";
      this.rdoArchiveNo.Size = new Size(39, 17);
      this.rdoArchiveNo.TabIndex = 3;
      this.rdoArchiveNo.TabStop = true;
      this.rdoArchiveNo.Text = "No";
      this.rdoArchiveNo.UseVisualStyleBackColor = true;
      this.rdoArchiveYes.AutoSize = true;
      this.rdoArchiveYes.Location = new Point(212, 261);
      this.rdoArchiveYes.Name = "rdoArchiveYes";
      this.rdoArchiveYes.Size = new Size(43, 17);
      this.rdoArchiveYes.TabIndex = 2;
      this.rdoArchiveYes.TabStop = true;
      this.rdoArchiveYes.Text = "Yes";
      this.rdoArchiveYes.UseVisualStyleBackColor = true;
      this.lblArchivedoc.AutoSize = true;
      this.lblArchivedoc.Location = new Point(34, 261);
      this.lblArchivedoc.Name = "lblArchivedoc";
      this.lblArchivedoc.Size = new Size(172, 13);
      this.lblArchivedoc.TabIndex = 1;
      this.lblArchivedoc.Text = "Move chosen document to archive";
      this.grpAccessRelatedDoc.Controls.Add((Control) this.grdViewDocs);
      this.grpAccessRelatedDoc.Controls.Add((Control) this.stdBtnSearchDocs);
      this.grpAccessRelatedDoc.Controls.Add((Control) this.txtSearchTpoDoc);
      this.grpAccessRelatedDoc.HeaderForeColor = SystemColors.ControlText;
      this.grpAccessRelatedDoc.Location = new Point(35, 36);
      this.grpAccessRelatedDoc.Name = "grpAccessRelatedDoc";
      this.grpAccessRelatedDoc.Size = new Size(583, 212);
      this.grpAccessRelatedDoc.TabIndex = 0;
      this.grpAccessRelatedDoc.Text = "Choose an existing related document";
      this.grdViewDocs.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Document Display Name";
      gvColumn1.Width = 130;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Category";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Channel";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Date;
      gvColumn4.Text = "Start Date";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.Date;
      gvColumn5.Text = "End Date";
      gvColumn5.Width = 80;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Status";
      gvColumn6.Width = 80;
      this.grdViewDocs.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.grdViewDocs.Dock = DockStyle.Fill;
      this.grdViewDocs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdViewDocs.Location = new Point(1, 26);
      this.grdViewDocs.Name = "grdViewDocs";
      this.grdViewDocs.Size = new Size(581, 185);
      this.grdViewDocs.TabIndex = 2;
      this.grdViewDocs.Click += new EventHandler(this.grdViewDocs_Click);
      this.stdBtnSearchDocs.BackColor = Color.Transparent;
      this.stdBtnSearchDocs.Location = new Point(555, 6);
      this.stdBtnSearchDocs.MouseDownImage = (Image) null;
      this.stdBtnSearchDocs.Name = "stdBtnSearchDocs";
      this.stdBtnSearchDocs.Size = new Size(16, 16);
      this.stdBtnSearchDocs.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdBtnSearchDocs.TabIndex = 1;
      this.stdBtnSearchDocs.TabStop = false;
      this.stdBtnSearchDocs.Click += new EventHandler(this.stdBtnSearchDocs_Click);
      this.txtSearchTpoDoc.Location = new Point(362, 2);
      this.txtSearchTpoDoc.Name = "txtSearchTpoDoc";
      this.txtSearchTpoDoc.Size = new Size(187, 20);
      this.txtSearchTpoDoc.TabIndex = 0;
      this.panel2.Controls.Add((Control) this.cboPlaceDocument_relatedDocs);
      this.panel2.Controls.Add((Control) this.lblPlaceDocument);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(641, 36);
      this.panel2.TabIndex = 85;
      this.cboPlaceDocument_relatedDocs.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPlaceDocument_relatedDocs.FormattingEnabled = true;
      this.cboPlaceDocument_relatedDocs.Items.AddRange(new object[4]
      {
        (object) "Above selected document",
        (object) "Below selected document",
        (object) "Top of document's catergory",
        (object) "Bottom of documents's category"
      });
      this.cboPlaceDocument_relatedDocs.Location = new Point(176, 5);
      this.cboPlaceDocument_relatedDocs.Name = "cboPlaceDocument_relatedDocs";
      this.cboPlaceDocument_relatedDocs.Size = new Size(243, 21);
      this.cboPlaceDocument_relatedDocs.TabIndex = 84;
      this.lblPlaceDocument.AutoSize = true;
      this.lblPlaceDocument.Location = new Point(34, 11);
      this.lblPlaceDocument.Name = "lblPlaceDocument";
      this.lblPlaceDocument.Size = new Size(84, 13);
      this.lblPlaceDocument.TabIndex = 83;
      this.lblPlaceDocument.Text = "Place document";
      this.pnlChooseTpoCompanies.Controls.Add((Control) this.grpChooseTpoCompanies);
      this.pnlChooseTpoCompanies.Controls.Add((Control) this.panel3);
      this.pnlChooseTpoCompanies.Dock = DockStyle.Top;
      this.pnlChooseTpoCompanies.Location = new Point(0, 540);
      this.pnlChooseTpoCompanies.Name = "pnlChooseTpoCompanies";
      this.pnlChooseTpoCompanies.Size = new Size(641, 273);
      this.pnlChooseTpoCompanies.TabIndex = 79;
      this.grpChooseTpoCompanies.Controls.Add((Control) this.stdBtnSearchTpos);
      this.grpChooseTpoCompanies.Controls.Add((Control) this.grdViewTpos);
      this.grpChooseTpoCompanies.HeaderForeColor = SystemColors.ControlText;
      this.grpChooseTpoCompanies.Location = new Point(37, 42);
      this.grpChooseTpoCompanies.Name = "grpChooseTpoCompanies";
      this.grpChooseTpoCompanies.Size = new Size(581, 216);
      this.grpChooseTpoCompanies.TabIndex = 0;
      this.grpChooseTpoCompanies.Text = "Choose TPO Companies";
      this.stdBtnSearchTpos.BackColor = Color.Transparent;
      this.stdBtnSearchTpos.Location = new Point(553, 4);
      this.stdBtnSearchTpos.MouseDownImage = (Image) null;
      this.stdBtnSearchTpos.Name = "stdBtnSearchTpos";
      this.stdBtnSearchTpos.Size = new Size(16, 16);
      this.stdBtnSearchTpos.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.stdBtnSearchTpos.TabIndex = 2;
      this.stdBtnSearchTpos.TabStop = false;
      this.stdBtnSearchTpos.Click += new EventHandler(this.stdBtnSearchTpos_Click);
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column1";
      gvColumn7.Text = "TPO Companies";
      gvColumn7.Width = 450;
      this.grdViewTpos.Columns.AddRange(new GVColumn[1]
      {
        gvColumn7
      });
      this.grdViewTpos.Dock = DockStyle.Fill;
      this.grdViewTpos.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdViewTpos.Location = new Point(1, 26);
      this.grdViewTpos.Name = "grdViewTpos";
      this.grdViewTpos.Size = new Size(579, 189);
      this.grdViewTpos.TabIndex = 0;
      this.panel3.Controls.Add((Control) this.cboPlaceDocument_selctTpos);
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(641, 36);
      this.panel3.TabIndex = 86;
      this.cboPlaceDocument_selctTpos.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPlaceDocument_selctTpos.FormattingEnabled = true;
      this.cboPlaceDocument_selctTpos.Items.AddRange(new object[2]
      {
        (object) "Top of document's category",
        (object) "Bottom of documents's category"
      });
      this.cboPlaceDocument_selctTpos.Location = new Point(176, 5);
      this.cboPlaceDocument_selctTpos.Name = "cboPlaceDocument_selctTpos";
      this.cboPlaceDocument_selctTpos.Size = new Size(243, 21);
      this.cboPlaceDocument_selctTpos.TabIndex = 84;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(32, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(84, 13);
      this.label3.TabIndex = 83;
      this.label3.Text = "Place document";
      this.panel1.Controls.Add((Control) this.rdoAvailableToAllTposNo);
      this.panel1.Controls.Add((Control) this.rdoAvailableToAllTposYes);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.cboAssignto);
      this.panel1.Controls.Add((Control) this.lblAssignTo);
      this.panel1.Controls.Add((Control) this.lblDocDisplayName);
      this.panel1.Controls.Add((Control) this.lblFile);
      this.panel1.Controls.Add((Control) this.txtFileName);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.txtDocDisplayName);
      this.panel1.Controls.Add((Control) this.btnAttachBrowse);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.dtpStartDate);
      this.panel1.Controls.Add((Control) this.dtpEndDate);
      this.panel1.Controls.Add((Control) this.lblStartDate);
      this.panel1.Controls.Add((Control) this.lblEndDate);
      this.panel1.Controls.Add((Control) this.lblChannel);
      this.panel1.Controls.Add((Control) this.cboCategory);
      this.panel1.Controls.Add((Control) this.cboChannel);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(641, 230);
      this.panel1.TabIndex = 82;
      this.rdoAvailableToAllTposNo.AutoSize = true;
      this.rdoAvailableToAllTposNo.Location = new Point(222, 176);
      this.rdoAvailableToAllTposNo.Name = "rdoAvailableToAllTposNo";
      this.rdoAvailableToAllTposNo.Size = new Size(39, 17);
      this.rdoAvailableToAllTposNo.TabIndex = 78;
      this.rdoAvailableToAllTposNo.TabStop = true;
      this.rdoAvailableToAllTposNo.Text = "No";
      this.rdoAvailableToAllTposNo.UseVisualStyleBackColor = true;
      this.rdoAvailableToAllTposNo.CheckedChanged += new EventHandler(this.rdoAvailableToAllTposNo_CheckedChanged);
      this.rdoAvailableToAllTposYes.AutoSize = true;
      this.rdoAvailableToAllTposYes.Location = new Point(176, 176);
      this.rdoAvailableToAllTposYes.Name = "rdoAvailableToAllTposYes";
      this.rdoAvailableToAllTposYes.Size = new Size(43, 17);
      this.rdoAvailableToAllTposYes.TabIndex = 77;
      this.rdoAvailableToAllTposYes.TabStop = true;
      this.rdoAvailableToAllTposYes.Text = "Yes";
      this.rdoAvailableToAllTposYes.UseVisualStyleBackColor = true;
      this.rdoAvailableToAllTposYes.CheckedChanged += new EventHandler(this.rdoAvailableToAllTposYes_CheckedChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(32, 178);
      this.label4.Name = "label4";
      this.label4.Size = new Size(106, 13);
      this.label4.TabIndex = 76;
      this.label4.Text = "Available to All TPOs";
      this.btnAttachBrowse.BackColor = Color.Transparent;
      this.btnAttachBrowse.DisabledImage = (Image) null;
      this.btnAttachBrowse.Image = (Image) componentResourceManager.GetObject("btnAttachBrowse.Image");
      this.btnAttachBrowse.Location = new Point(555, 16);
      this.btnAttachBrowse.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachBrowse.MouseDownImage = (Image) null;
      this.btnAttachBrowse.MouseOverImage = (Image) null;
      this.btnAttachBrowse.Name = "btnAttachBrowse";
      this.btnAttachBrowse.Size = new Size(16, 16);
      this.btnAttachBrowse.TabIndex = 55;
      this.btnAttachBrowse.TabStop = false;
      this.btnAttachBrowse.Click += new EventHandler(this.btnAttachBrowse_Click);
      this.dtpStartDate.BackColor = SystemColors.Window;
      this.dtpStartDate.Location = new Point(176, 65);
      this.dtpStartDate.MaxValue = new DateTime(2079, 6, 6, 0, 0, 0, 0);
      this.dtpStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtpStartDate.Name = "dtpStartDate";
      this.dtpStartDate.Size = new Size(85, 21);
      this.dtpStartDate.TabIndex = 61;
      this.dtpStartDate.Tag = (object) "763";
      this.dtpStartDate.ToolTip = "";
      this.dtpStartDate.Value = new DateTime(0L);
      this.dtpEndDate.BackColor = SystemColors.Window;
      this.dtpEndDate.Location = new Point(176, 94);
      this.dtpEndDate.MaxValue = new DateTime(2079, 6, 6, 0, 0, 0, 0);
      this.dtpEndDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtpEndDate.Name = "dtpEndDate";
      this.dtpEndDate.Size = new Size(85, 21);
      this.dtpEndDate.TabIndex = 62;
      this.dtpEndDate.Tag = (object) "763";
      this.dtpEndDate.ToolTip = "";
      this.dtpEndDate.Value = new DateTime(0L);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(641, 874);
      this.Controls.Add((Control) this.pnlChooseTpoCompanies);
      this.Controls.Add((Control) this.pnlAccessRelatedDoc);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddDocumentForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add new Document";
      this.pnlAccessRelatedDoc.ResumeLayout(false);
      this.pnlAccessRelatedDoc.PerformLayout();
      this.grpAccessRelatedDoc.ResumeLayout(false);
      this.grpAccessRelatedDoc.PerformLayout();
      ((ISupportInitialize) this.stdBtnSearchDocs).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.pnlChooseTpoCompanies.ResumeLayout(false);
      this.grpChooseTpoCompanies.ResumeLayout(false);
      ((ISupportInitialize) this.stdBtnSearchTpos).EndInit();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.btnAttachBrowse).EndInit();
      this.ResumeLayout(false);
    }
  }
}
