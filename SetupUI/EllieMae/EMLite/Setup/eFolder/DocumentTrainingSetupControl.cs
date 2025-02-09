// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.DocumentTrainingSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class DocumentTrainingSetupControl : SettingsUserControl
  {
    private Sessions.Session session;
    private DocClassificationControllerClient svcClient;
    private int selectedIndex;
    private IContainer components;
    private GroupContainer gcTemplates;
    private GridView gvTemplates;
    private StandardIconButton btnDeleteTemplate;
    private StandardIconButton btnAddTemplate;
    private Panel pnlRight;
    private GroupContainer gcDocuments;
    private GridView gvDocuments;
    private StandardIconButton btnDeleteDocument;
    private StandardIconButton btnAddDocument;
    private CollapsibleSplitter csTemplates;
    private CollapsibleSplitter csDocuments;
    private Button btnReject;
    private Button btnApprove;
    private FlowLayoutPanel pnlToolbar;
    private VerticalSeparator separator;
    private CollapsibleSplitter csText;
    private GroupContainer gcText;
    private BorderPanel borderImage;
    private PictureBox pctImage;
    private Panel pnlImage;
    private TextBox txtText;
    private Button btnSuggest;
    private Button btnDeleteTraining;
    private FileAttachmentViewerControl fileViewer;

    public DocumentTrainingSetupControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.svcClient = DocumentationClassificationClient.InitializeServiceClient();
      this.initDocumentList();
      this.initTemplateList();
      this.loadDocumentList();
    }

    private void initDocumentList()
    {
      this.gvDocuments.Columns.Add("Name", 100).SpringToFit = true;
      this.gvDocuments.Sort(0, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      this.gvDocuments.Items.Clear();
      foreach (DocumentClass documentClass in this.svcClient.GetDocumentClassList(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword))
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) documentClass.DocumentTitle);
        gvItem.Tag = (object) documentClass;
        if (documentClass.PendingSuggestions)
          gvItem.SubItems[0].Font = EncompassFonts.Normal2.Font;
        this.gvDocuments.Items.Add(gvItem);
      }
      this.gvDocuments.ReSort();
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      DocumentClass docClass = (DocumentClass) null;
      if (this.gvDocuments.SelectedItems.Count > 0)
      {
        this.selectedIndex = this.gvDocuments.SelectedItems[0].Index;
        docClass = this.gvDocuments.SelectedItems[0].Tag as DocumentClass;
      }
      this.loadTemplateList(docClass);
      this.btnDeleteDocument.Enabled = this.gvDocuments.SelectedItems.Count > 0;
    }

    public void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      PageClass page = (PageClass) null;
      if (this.gvTemplates.SelectedItems.Count > 0)
        page = this.gvTemplates.SelectedItems[0].Tag as PageClass;
      this.loadTemplateItem(page);
    }

    private void btnAddDocument_Click(object sender, EventArgs e)
    {
      ArrayList existingNames = new ArrayList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
        existingNames.Add((object) gvItem.Text);
      NewPhraseForm newPhraseForm = new NewPhraseForm("", existingNames);
      newPhraseForm.WindowTitle = "Add New Document";
      newPhraseForm.Description = "Enter New Document Title:";
      newPhraseForm.IgnoreCase = true;
      if (newPhraseForm.ShowDialog() == DialogResult.Cancel)
        return;
      this.svcClient.AddDocumentClass(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, newPhraseForm.NewPhrase.Trim(), this.session.UserInfo.FullName);
      this.loadDocumentList();
    }

    private void btnDeleteDocument_Click(object sender, EventArgs e)
    {
      if (this.gvDocuments.SelectedItems.Count <= 0)
        return;
      DocumentClass tag = this.gvDocuments.SelectedItems[0].Tag as DocumentClass;
      if (this.gvTemplates.Items.Count > 0 && Utils.Dialog((IWin32Window) this, "This Document contains one or more pages. Are you sure you want to delete this Document?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      DocumentClass documentClass = this.svcClient.DeleteDocumentClass(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, tag.DocumentGUID, this.session.UserInfo.FullName);
      if (documentClass == null || !(documentClass.AdminStatus == "Deleted"))
        return;
      this.loadDocumentList();
    }

    private void initTemplateList()
    {
      this.gvTemplates.Columns.Add("Status", 200);
      this.gvTemplates.Columns.Add("Status By", 200);
      this.gvTemplates.Columns.Add(new GVColumn("Status Date")
      {
        Width = 200,
        SortMethod = GVSortMethod.DateTime
      });
      this.gvTemplates.Columns.Add("Suggested By", 200);
      this.gvTemplates.Columns.Add(new GVColumn("Suggested Date")
      {
        Width = 200,
        SortMethod = GVSortMethod.DateTime
      });
      this.gvTemplates.Sort(2, SortOrder.Descending);
    }

    private void loadTemplateList(DocumentClass docClass)
    {
      this.gvTemplates.Items.Clear();
      if (docClass == null)
        return;
      foreach (PageClass pageClass in this.svcClient.GetPageClassList(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, docClass.DocumentGUID))
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems.Add((object) pageClass.Status);
        gvItem.SubItems.Add((object) pageClass.StatusBy);
        gvItem.SubItems.Add((object) this.getLocalDateTimeString(pageClass.StatusDate));
        gvItem.SubItems.Add((object) pageClass.SuggestedBy);
        gvItem.SubItems.Add((object) this.getLocalDateTimeString(pageClass.SuggestedDate));
        if (pageClass.Status == "Suggested")
        {
          foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
            subItem.Font = EncompassFonts.Normal2.Font;
        }
        gvItem.Tag = (object) pageClass;
        this.gvTemplates.Items.Add(gvItem);
      }
      this.gvTemplates.ReSort();
    }

    private void loadTemplateItem(PageClass page)
    {
      this.txtText.Text = string.Empty;
      if (this.pctImage.Image != null)
      {
        this.pctImage.Image.Dispose();
        this.pctImage.Image = (Image) null;
        this.pctImage.Visible = false;
      }
      this.setButtons(page);
      if (page == null)
        return;
      PageClassData pageClassData = this.svcClient.GetPageClassData(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, page.PageGUID);
      this.txtText.Text = pageClassData.PageText;
      if (pageClassData.Data == null)
        return;
      float num1 = 0.0f;
      float num2 = 0.0f;
      using (Graphics graphics = this.CreateGraphics())
      {
        num1 = graphics.DpiX;
        num2 = graphics.DpiY;
      }
      using (MemoryStream memoryStream = new MemoryStream(pageClassData.Data))
      {
        using (Image original = Image.FromStream((Stream) memoryStream))
        {
          this.pctImage.Width = Convert.ToInt32((float) original.Width / original.HorizontalResolution * num1);
          this.pctImage.Height = Convert.ToInt32((float) original.Height / original.VerticalResolution * num2);
          this.pctImage.Image = (Image) new Bitmap(original);
          this.pctImage.Visible = true;
        }
      }
    }

    private void reloadTemplateItem(PageClass page)
    {
      if (this.gvTemplates.SelectedItems.Count == 0)
        return;
      GVItem selectedItem = this.gvTemplates.SelectedItems[0];
      selectedItem.SubItems[0].Value = (object) page.Status;
      selectedItem.SubItems[1].Value = (object) page.StatusBy;
      selectedItem.SubItems[2].Value = (object) this.getLocalDateTimeString(page.StatusDate);
      selectedItem.Tag = (object) page;
      if (page.Status != "Suggested")
      {
        foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) selectedItem.SubItems)
          subItem.Font = (Font) null;
      }
      this.setButtons(page);
      if (this.gvDocuments.SelectedItems.Count <= 0 || !(this.gvDocuments.SelectedItems[0].Tag as DocumentClass).PendingSuggestions)
        return;
      bool flag = false;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
      {
        if (gvItem.SubItems[0].Value.ToString() == "Suggested")
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) this.gvDocuments.SelectedItems[0].SubItems)
        subItem.Font = (Font) null;
    }

    private void btnSuggest_Click(object sender, EventArgs e)
    {
      DocumentClass docClass = (DocumentClass) null;
      if (this.gvDocuments.SelectedItems.Count > 0)
        docClass = this.gvDocuments.SelectedItems[0].Tag as DocumentClass;
      using (SuggestTrainingSetupDialog trainingSetupDialog = new SuggestTrainingSetupDialog(docClass, this.session))
      {
        if (trainingSetupDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadDocumentList();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
        {
          if (gvItem.Text == trainingSetupDialog.UpdatedDocumentClass)
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
    }

    private void btnApprove_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count == 0)
        return;
      this.reloadTemplateItem(this.svcClient.ApproveTraining(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, (this.gvTemplates.SelectedItems[0].Tag as PageClass).PageGUID, this.session.UserInfo.FullName));
    }

    private void btnReject_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count == 0)
        return;
      this.reloadTemplateItem(this.svcClient.RejectTraining(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, (this.gvTemplates.SelectedItems[0].Tag as PageClass).PageGUID, this.session.UserInfo.FullName));
    }

    private void btnDeleteTraining_Click(object sender, EventArgs e)
    {
      if (this.gvTemplates.SelectedItems.Count == 0)
        return;
      PageClass tag = this.gvTemplates.SelectedItems[0].Tag as PageClass;
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to permanently delete this page?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.svcClient.DeleteTraining(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, tag.PageGUID, this.session.UserInfo.FullName);
      DocumentClass docClass = (DocumentClass) null;
      if (this.gvDocuments.SelectedItems.Count > 0)
        docClass = this.gvDocuments.SelectedItems[0].Tag as DocumentClass;
      this.loadDocumentList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        if (gvItem.Text == docClass.DocumentTitle)
        {
          gvItem.Selected = true;
          break;
        }
      }
      this.loadTemplateList(docClass);
    }

    private void setButtons(PageClass page)
    {
      this.btnApprove.Enabled = false;
      this.btnReject.Enabled = false;
      this.btnDeleteTraining.Enabled = false;
      if (page == null)
        return;
      this.btnApprove.Enabled = page.Status != "Approved";
      this.btnReject.Enabled = page.Status != "Rejected";
      this.btnDeleteTraining.Enabled = true;
    }

    private string getLocalDateTimeString(string serverDateTime)
    {
      return System.TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(serverDateTime), System.TimeZoneInfo.Local).ToString("MM/dd/yyyy hh:mm:ss tt");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.svcClient != null)
          this.svcClient.Close();
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcTemplates = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDeleteTemplate = new StandardIconButton();
      this.btnAddTemplate = new StandardIconButton();
      this.separator = new VerticalSeparator();
      this.btnDeleteTraining = new Button();
      this.btnReject = new Button();
      this.btnApprove = new Button();
      this.btnSuggest = new Button();
      this.gvTemplates = new GridView();
      this.pnlRight = new Panel();
      this.borderImage = new BorderPanel();
      this.pnlImage = new Panel();
      this.pctImage = new PictureBox();
      this.fileViewer = new FileAttachmentViewerControl();
      this.csText = new CollapsibleSplitter();
      this.gcText = new GroupContainer();
      this.txtText = new TextBox();
      this.csTemplates = new CollapsibleSplitter();
      this.gcDocuments = new GroupContainer();
      this.gvDocuments = new GridView();
      this.btnDeleteDocument = new StandardIconButton();
      this.btnAddDocument = new StandardIconButton();
      this.csDocuments = new CollapsibleSplitter();
      this.gcTemplates.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteTemplate).BeginInit();
      ((ISupportInitialize) this.btnAddTemplate).BeginInit();
      this.pnlRight.SuspendLayout();
      this.borderImage.SuspendLayout();
      this.pnlImage.SuspendLayout();
      ((ISupportInitialize) this.pctImage).BeginInit();
      this.gcText.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteDocument).BeginInit();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      this.SuspendLayout();
      this.gcTemplates.Controls.Add((Control) this.pnlToolbar);
      this.gcTemplates.Controls.Add((Control) this.gvTemplates);
      this.gcTemplates.Dock = DockStyle.Top;
      this.gcTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcTemplates.Location = new Point(0, 0);
      this.gcTemplates.Name = "gcTemplates";
      this.gcTemplates.Size = new Size(611, 164);
      this.gcTemplates.TabIndex = 13;
      this.gcTemplates.Text = "Templates";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDeleteTemplate);
      this.pnlToolbar.Controls.Add((Control) this.btnAddTemplate);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnDeleteTraining);
      this.pnlToolbar.Controls.Add((Control) this.btnReject);
      this.pnlToolbar.Controls.Add((Control) this.btnApprove);
      this.pnlToolbar.Controls.Add((Control) this.btnSuggest);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(250, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(357, 22);
      this.pnlToolbar.TabIndex = 15;
      this.btnDeleteTemplate.BackColor = Color.Transparent;
      this.btnDeleteTemplate.Location = new Point(341, 3);
      this.btnDeleteTemplate.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteTemplate.MouseDownImage = (Image) null;
      this.btnDeleteTemplate.Name = "btnDeleteTemplate";
      this.btnDeleteTemplate.Size = new Size(16, 17);
      this.btnDeleteTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteTemplate.TabIndex = 11;
      this.btnDeleteTemplate.TabStop = false;
      this.btnDeleteTemplate.Visible = false;
      this.btnAddTemplate.BackColor = Color.Transparent;
      this.btnAddTemplate.Location = new Point(321, 3);
      this.btnAddTemplate.Margin = new Padding(4, 3, 0, 3);
      this.btnAddTemplate.MouseDownImage = (Image) null;
      this.btnAddTemplate.Name = "btnAddTemplate";
      this.btnAddTemplate.Size = new Size(16, 17);
      this.btnAddTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddTemplate.TabIndex = 10;
      this.btnAddTemplate.TabStop = false;
      this.btnAddTemplate.Visible = false;
      this.separator.Location = new Point(315, 3);
      this.separator.Margin = new Padding(4, 3, 0, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 15;
      this.separator.TabStop = false;
      this.separator.Visible = false;
      this.btnDeleteTraining.Enabled = false;
      this.btnDeleteTraining.Location = new Point(236, 0);
      this.btnDeleteTraining.Margin = new Padding(0);
      this.btnDeleteTraining.Name = "btnDeleteTraining";
      this.btnDeleteTraining.Size = new Size(75, 22);
      this.btnDeleteTraining.TabIndex = 16;
      this.btnDeleteTraining.Text = "Delete";
      this.btnDeleteTraining.UseVisualStyleBackColor = true;
      this.btnDeleteTraining.Click += new EventHandler(this.btnDeleteTraining_Click);
      this.btnReject.Enabled = false;
      this.btnReject.Location = new Point(161, 0);
      this.btnReject.Margin = new Padding(0);
      this.btnReject.Name = "btnReject";
      this.btnReject.Size = new Size(75, 22);
      this.btnReject.TabIndex = 14;
      this.btnReject.Text = "Reject";
      this.btnReject.UseVisualStyleBackColor = true;
      this.btnReject.Click += new EventHandler(this.btnReject_Click);
      this.btnApprove.Enabled = false;
      this.btnApprove.Location = new Point(86, 0);
      this.btnApprove.Margin = new Padding(0);
      this.btnApprove.Name = "btnApprove";
      this.btnApprove.Size = new Size(75, 22);
      this.btnApprove.TabIndex = 13;
      this.btnApprove.Text = "Approve";
      this.btnApprove.UseVisualStyleBackColor = true;
      this.btnApprove.Click += new EventHandler(this.btnApprove_Click);
      this.btnSuggest.Location = new Point(11, 0);
      this.btnSuggest.Margin = new Padding(0);
      this.btnSuggest.Name = "btnSuggest";
      this.btnSuggest.Size = new Size(75, 22);
      this.btnSuggest.TabIndex = 17;
      this.btnSuggest.Text = "Suggest";
      this.btnSuggest.UseVisualStyleBackColor = true;
      this.btnSuggest.Click += new EventHandler(this.btnSuggest_Click);
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(609, 137);
      this.gvTemplates.TabIndex = 12;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.pnlRight.Controls.Add((Control) this.borderImage);
      this.pnlRight.Controls.Add((Control) this.csText);
      this.pnlRight.Controls.Add((Control) this.gcText);
      this.pnlRight.Controls.Add((Control) this.csTemplates);
      this.pnlRight.Controls.Add((Control) this.gcTemplates);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(295, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(611, 432);
      this.pnlRight.TabIndex = 16;
      this.borderImage.BackColor = Color.WhiteSmoke;
      this.borderImage.Controls.Add((Control) this.pnlImage);
      this.borderImage.Controls.Add((Control) this.fileViewer);
      this.borderImage.Dock = DockStyle.Fill;
      this.borderImage.Location = new Point(0, 171);
      this.borderImage.Name = "borderImage";
      this.borderImage.Size = new Size(611, 166);
      this.borderImage.TabIndex = 18;
      this.pnlImage.AutoScroll = true;
      this.pnlImage.Controls.Add((Control) this.pctImage);
      this.pnlImage.Dock = DockStyle.Fill;
      this.pnlImage.Location = new Point(1, 1);
      this.pnlImage.Name = "pnlImage";
      this.pnlImage.Size = new Size(609, 164);
      this.pnlImage.TabIndex = 1;
      this.pctImage.Location = new Point(2, 2);
      this.pctImage.Name = "pctImage";
      this.pctImage.Size = new Size(100, 50);
      this.pctImage.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pctImage.TabIndex = 0;
      this.pctImage.TabStop = false;
      this.pctImage.Visible = false;
      this.fileViewer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(614, 256);
      this.fileViewer.TabIndex = 1;
      this.csText.AnimationDelay = 20;
      this.csText.AnimationStep = 20;
      this.csText.BorderStyle3D = Border3DStyle.Flat;
      this.csText.ControlToHide = (Control) this.gcText;
      this.csText.Dock = DockStyle.Bottom;
      this.csText.ExpandParentForm = false;
      this.csText.Location = new Point(0, 337);
      this.csText.Name = "csLeft";
      this.csText.TabIndex = 17;
      this.csText.TabStop = false;
      this.csText.UseAnimations = false;
      this.csText.VisualStyle = VisualStyles.Encompass;
      this.gcText.Controls.Add((Control) this.txtText);
      this.gcText.Dock = DockStyle.Bottom;
      this.gcText.HeaderForeColor = SystemColors.ControlText;
      this.gcText.Location = new Point(0, 344);
      this.gcText.Name = "gcText";
      this.gcText.Size = new Size(611, 88);
      this.gcText.TabIndex = 16;
      this.gcText.Text = "Text";
      this.txtText.BackColor = Color.WhiteSmoke;
      this.txtText.BorderStyle = BorderStyle.None;
      this.txtText.Dock = DockStyle.Fill;
      this.txtText.Location = new Point(1, 26);
      this.txtText.Multiline = true;
      this.txtText.Name = "txtText";
      this.txtText.ReadOnly = true;
      this.txtText.ScrollBars = ScrollBars.Vertical;
      this.txtText.Size = new Size(609, 61);
      this.txtText.TabIndex = 0;
      this.csTemplates.AnimationDelay = 20;
      this.csTemplates.AnimationStep = 20;
      this.csTemplates.BorderStyle3D = Border3DStyle.Flat;
      this.csTemplates.ControlToHide = (Control) this.gcTemplates;
      this.csTemplates.Dock = DockStyle.Top;
      this.csTemplates.ExpandParentForm = false;
      this.csTemplates.Location = new Point(0, 164);
      this.csTemplates.Name = "csLeft";
      this.csTemplates.TabIndex = 15;
      this.csTemplates.TabStop = false;
      this.csTemplates.UseAnimations = false;
      this.csTemplates.VisualStyle = VisualStyles.Encompass;
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Controls.Add((Control) this.btnDeleteDocument);
      this.gcDocuments.Controls.Add((Control) this.btnAddDocument);
      this.gcDocuments.Dock = DockStyle.Left;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(288, 432);
      this.gcDocuments.TabIndex = 14;
      this.gcDocuments.Text = "Documents";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(286, 405);
      this.gvDocuments.TabIndex = 12;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.btnDeleteDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteDocument.BackColor = Color.Transparent;
      this.btnDeleteDocument.Location = new Point(266, 5);
      this.btnDeleteDocument.MouseDownImage = (Image) null;
      this.btnDeleteDocument.Name = "btnDeleteDocument";
      this.btnDeleteDocument.Size = new Size(16, 17);
      this.btnDeleteDocument.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteDocument.TabIndex = 11;
      this.btnDeleteDocument.TabStop = false;
      this.btnDeleteDocument.Click += new EventHandler(this.btnDeleteDocument_Click);
      this.btnAddDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDocument.BackColor = Color.Transparent;
      this.btnAddDocument.Location = new Point(244, 5);
      this.btnAddDocument.MouseDownImage = (Image) null;
      this.btnAddDocument.Name = "btnAddDocument";
      this.btnAddDocument.Size = new Size(16, 17);
      this.btnAddDocument.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocument.TabIndex = 10;
      this.btnAddDocument.TabStop = false;
      this.btnAddDocument.Click += new EventHandler(this.btnAddDocument_Click);
      this.csDocuments.AnimationDelay = 20;
      this.csDocuments.AnimationStep = 20;
      this.csDocuments.BorderStyle3D = Border3DStyle.Flat;
      this.csDocuments.ControlToHide = (Control) this.gcDocuments;
      this.csDocuments.ExpandParentForm = false;
      this.csDocuments.Location = new Point(288, 0);
      this.csDocuments.Name = "csLeft";
      this.csDocuments.TabIndex = 17;
      this.csDocuments.TabStop = false;
      this.csDocuments.UseAnimations = false;
      this.csDocuments.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csDocuments);
      this.Controls.Add((Control) this.gcDocuments);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DocumentTrainingSetupControl);
      this.Size = new Size(906, 432);
      this.gcTemplates.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeleteTemplate).EndInit();
      ((ISupportInitialize) this.btnAddTemplate).EndInit();
      this.pnlRight.ResumeLayout(false);
      this.borderImage.ResumeLayout(false);
      this.pnlImage.ResumeLayout(false);
      ((ISupportInitialize) this.pctImage).EndInit();
      this.gcText.ResumeLayout(false);
      this.gcText.PerformLayout();
      this.gcDocuments.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeleteDocument).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      this.ResumeLayout(false);
    }
  }
}
