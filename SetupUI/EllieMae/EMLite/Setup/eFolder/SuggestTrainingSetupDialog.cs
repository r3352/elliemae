// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.SuggestTrainingSetupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class SuggestTrainingSetupDialog : Form
  {
    private Sessions.Session session;
    private DocClassificationControllerClient svcClient;
    private DocumentClass docClass;
    private IContainer components;
    private EMHelpLink helpLink;
    private Button btnCancel;
    private Button btnOK;
    private GroupBox grpTemplateInfo;
    private GroupBox grpDocument;
    private ComboBox cboDocumentClasses;
    private Label label1;
    private GroupBox grpTrainingSource;
    private IconButton btnBrowse;
    private RadioButton rdoTemplates;
    private RadioButton rdoFile;
    private GridView gvTemplates;
    private CollapsibleSplitter csDocuments;
    private Panel pnlImage;
    private PictureBox pctImage;
    private GroupContainer gcText;
    private TextBox txtText;
    private Label lblProgress;

    public SuggestTrainingSetupDialog(DocumentClass docClass, Sessions.Session session)
    {
      this.InitializeComponent();
      this.docClass = docClass;
      this.session = session;
      this.svcClient = DocumentationClassificationClient.InitializeServiceClient();
      this.initPageTemplateList();
      this.loadPageTemplates();
      this.loadDocumentClasses();
    }

    public string UpdatedDocumentClass => this.cboDocumentClasses.Text.Trim();

    private void initPageTemplateList()
    {
      this.gvTemplates.Columns.Add("Name", 100).SpringToFit = true;
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void loadPageTemplates()
    {
      this.gvTemplates.Items.Clear();
      foreach (DocumentClass pageTemplate in this.svcClient.GetPageTemplateList(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword))
        this.gvTemplates.Items.Add(new GVItem()
        {
          SubItems = {
            (object) pageTemplate.DocumentTitle
          },
          Tag = (object) pageTemplate
        });
      this.gvTemplates.ReSort();
    }

    public void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.txtText.Text = string.Empty;
      this.pctImage.Visible = false;
      DocumentClass documentClass = (DocumentClass) null;
      if (this.gvTemplates.SelectedItems.Count > 0)
        documentClass = this.gvTemplates.SelectedItems[0].Tag as DocumentClass;
      if (documentClass == null)
        return;
      PageClassData pageTemplateData = this.svcClient.GetPageTemplateData(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword, documentClass.DocumentTitle);
      this.txtText.Text = pageTemplateData.PageText;
      if (pageTemplateData.Data == null)
        return;
      float num1 = 0.0f;
      float num2 = 0.0f;
      using (Graphics graphics = this.CreateGraphics())
      {
        num1 = graphics.DpiX;
        num2 = graphics.DpiY;
      }
      using (MemoryStream memoryStream = new MemoryStream(pageTemplateData.Data))
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

    private void loadDocumentClasses()
    {
      this.cboDocumentClasses.Items.Clear();
      foreach (DocumentClass documentClass in this.svcClient.GetDocumentClassList(Session.CompanyInfo.ClientID, this.session.UserID, EpassLogin.LoginPassword))
        this.cboDocumentClasses.Items.Add((object) documentClass.DocumentTitle);
      if (this.docClass == null)
        return;
      this.cboDocumentClasses.Text = this.docClass.DocumentTitle;
    }

    private void TrainingSource_CheckedChanged(object sender, EventArgs e)
    {
      this.btnBrowse.Enabled = this.rdoFile.Checked;
      this.grpTemplateInfo.Enabled = this.rdoTemplates.Checked;
      if (this.rdoTemplates.Checked)
        return;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
        gvItem.Selected = false;
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      if (this.cboDocumentClasses.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must specify a Document Class.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string filepath = string.Empty;
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          openFileDialog.Title = "Browse and Suggest";
          openFileDialog.CheckFileExists = true;
          openFileDialog.Multiselect = false;
          openFileDialog.ShowReadOnly = false;
          openFileDialog.Filter = "All Supported Formats|*.pdf;*.doc;*.docx;*.txt;*.tif;*.jpg;*.jpeg;*.jpe;*.emf;*.xps|Adobe PDF Documents (*.pdf)|*.pdf|Microsoft Word Documents (*.doc,*.docx)|*.doc;*.docx|Text Documents (*.txt)|*.txt|TIFF Images (*.tif)|*.tif|JPEG Images (*.jpg, *.jpeg, *.jpe)|*.jpg;*.jpeg;*.jpe|Windows Enhanced Metafile (*.emf)|*.emf|XPS Files (*.xps)|*.xps";
          if (openFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK)
            filepath = openFileDialog.FileName;
        }
        if (string.IsNullOrEmpty(filepath))
          return;
        ImageAttachmentSettings attachmentSettings = this.session.ConfigurationManager.GetImageAttachmentSettings();
        ImageCreator imageCreator = new ImageCreator(SystemSettings.TempFolderRoot, attachmentSettings.ConversionType, (float) attachmentSettings.DpiX, (float) attachmentSettings.DpiY);
        imageCreator.ProgressChanged += new ProgressEventHandler(this.creator_ProgressChanged);
        try
        {
          Cursor.Current = Cursors.WaitCursor;
          this.lblProgress.Text = "Converting file...";
          this.lblProgress.Update();
          List<ImageProperties> imagePropertiesList = new List<ImageProperties>();
          ImageProperties[] collection = imageCreator.ConvertFile(filepath);
          imagePropertiesList.AddRange((IEnumerable<ImageProperties>) collection);
          if (imagePropertiesList.Count == 0)
            throw new Exception("Failed to convert file. Please check the page sizes and (if needed) reduce to letter or legal size.");
          this.lblProgress.Text = "Adding Suggested pages...";
          this.lblProgress.Update();
          int num2 = 0;
          for (int index = imagePropertiesList.Count - 1; index >= 0; --index)
          {
            ++num2;
            this.lblProgress.Text = "Adding Suggested page " + num2.ToString() + " of " + imagePropertiesList.Count.ToString() + ".";
            this.lblProgress.Update();
            byte[] content = File.ReadAllBytes(imagePropertiesList[index].ImageFile);
            this.svcClient.SuggestTraining(Session.CompanyInfo.ClientID, Session.UserID, EpassLogin.LoginPassword, Guid.NewGuid().ToString(), this.cboDocumentClasses.Text, Session.UserInfo.FullName, content);
          }
        }
        catch (Exception ex)
        {
          Cursor.Current = Cursors.Default;
          this.lblProgress.Text = string.Empty;
          this.lblProgress.Update();
          int num3 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to suggest " + filepath + ". '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
          this.lblProgress.Text = string.Empty;
          this.lblProgress.Update();
          imageCreator.ProgressChanged -= new ProgressEventHandler(this.creator_ProgressChanged);
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void creator_ProgressChanged(object source, ProgressEventArgs e)
    {
      TransferProgressEventArgs e1 = new TransferProgressEventArgs(e.PercentCompleted / 2);
      this.OnFileAttachmentUploadProgress(e1);
      if (!e1.Cancel)
        return;
      e.Abort = true;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.rdoTemplates.Checked)
        return;
      if (this.gvTemplates.SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a Page Template.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        DocumentClass tag = this.gvTemplates.SelectedItems[0].Tag as DocumentClass;
        if (this.cboDocumentClasses.Text.Trim() == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a Document Class.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.DialogResult = DialogResult.None;
        }
        else
        {
          try
          {
            this.svcClient.SuggestTrainingFromTemplate(Session.CompanyInfo.ClientID, Session.UserID, EpassLogin.LoginPassword, Guid.NewGuid().ToString(), this.cboDocumentClasses.Text, Session.UserInfo.FullName, tag.DocumentTitle);
            this.DialogResult = DialogResult.OK;
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to suggest " + tag.DocumentTitle + ". '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
      }
    }

    public event EllieMae.EMLite.eFolder.Files.TransferProgressEventHandler FileAttachmentUploadProgress;

    internal void OnFileAttachmentUploadProgress(TransferProgressEventArgs e)
    {
      if (e.PercentCompleted != 100)
      {
        this.lblProgress.Text = "Converting file. " + (object) e.PercentCompleted + "% complete.";
        this.lblProgress.Update();
      }
      if (this.FileAttachmentUploadProgress == null)
        return;
      this.FileAttachmentUploadProgress((object) this, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.grpTemplateInfo = new GroupBox();
      this.gcText = new GroupContainer();
      this.txtText = new TextBox();
      this.pnlImage = new Panel();
      this.pctImage = new PictureBox();
      this.gvTemplates = new GridView();
      this.csDocuments = new CollapsibleSplitter();
      this.grpDocument = new GroupBox();
      this.label1 = new Label();
      this.cboDocumentClasses = new ComboBox();
      this.helpLink = new EMHelpLink();
      this.grpTrainingSource = new GroupBox();
      this.lblProgress = new Label();
      this.btnBrowse = new IconButton();
      this.rdoTemplates = new RadioButton();
      this.rdoFile = new RadioButton();
      this.grpTemplateInfo.SuspendLayout();
      this.gcText.SuspendLayout();
      this.pnlImage.SuspendLayout();
      ((ISupportInitialize) this.pctImage).BeginInit();
      this.grpDocument.SuspendLayout();
      this.grpTrainingSource.SuspendLayout();
      ((ISupportInitialize) this.btnBrowse).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(596, 490);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(515, 490);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 12;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.grpTemplateInfo.Controls.Add((Control) this.gcText);
      this.grpTemplateInfo.Controls.Add((Control) this.pnlImage);
      this.grpTemplateInfo.Controls.Add((Control) this.gvTemplates);
      this.grpTemplateInfo.Controls.Add((Control) this.csDocuments);
      this.grpTemplateInfo.Enabled = false;
      this.grpTemplateInfo.Location = new Point(12, 100);
      this.grpTemplateInfo.Name = "grpTemplateInfo";
      this.grpTemplateInfo.Size = new Size(659, 294);
      this.grpTemplateInfo.TabIndex = 22;
      this.grpTemplateInfo.TabStop = false;
      this.grpTemplateInfo.Text = "Select Template";
      this.gcText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.gcText.Controls.Add((Control) this.txtText);
      this.gcText.HeaderForeColor = SystemColors.ControlText;
      this.gcText.Location = new Point(298, 200);
      this.gcText.Name = "gcText";
      this.gcText.Size = new Size(355, 88);
      this.gcText.TabIndex = 22;
      this.gcText.Text = "Text";
      this.txtText.BackColor = Color.WhiteSmoke;
      this.txtText.BorderStyle = BorderStyle.None;
      this.txtText.Dock = DockStyle.Fill;
      this.txtText.Location = new Point(1, 26);
      this.txtText.Multiline = true;
      this.txtText.Name = "txtText";
      this.txtText.ReadOnly = true;
      this.txtText.ScrollBars = ScrollBars.Vertical;
      this.txtText.Size = new Size(353, 61);
      this.txtText.TabIndex = 0;
      this.pnlImage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlImage.AutoScroll = true;
      this.pnlImage.Controls.Add((Control) this.pctImage);
      this.pnlImage.Location = new Point(298, 16);
      this.pnlImage.Name = "pnlImage";
      this.pnlImage.Size = new Size(355, 175);
      this.pnlImage.TabIndex = 21;
      this.pctImage.Location = new Point(2, 2);
      this.pctImage.Name = "pctImage";
      this.pctImage.Size = new Size(100, 50);
      this.pctImage.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pctImage.TabIndex = 0;
      this.pctImage.TabStop = false;
      this.pctImage.Visible = false;
      this.gvTemplates.AllowMultiselect = false;
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTemplates.Location = new Point(6, 19);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(286, 268);
      this.gvTemplates.TabIndex = 18;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.csDocuments.AnimationDelay = 20;
      this.csDocuments.AnimationStep = 20;
      this.csDocuments.BorderStyle3D = Border3DStyle.Flat;
      this.csDocuments.ControlToHide = (Control) null;
      this.csDocuments.ExpandParentForm = false;
      this.csDocuments.Location = new Point(3, 16);
      this.csDocuments.Name = "csLeft";
      this.csDocuments.TabIndex = 19;
      this.csDocuments.TabStop = false;
      this.csDocuments.UseAnimations = false;
      this.csDocuments.VisualStyle = VisualStyles.Encompass;
      this.grpDocument.Controls.Add((Control) this.label1);
      this.grpDocument.Controls.Add((Control) this.cboDocumentClasses);
      this.grpDocument.Location = new Point(13, 400);
      this.grpDocument.Name = "grpDocument";
      this.grpDocument.Size = new Size(659, 55);
      this.grpDocument.TabIndex = 23;
      this.grpDocument.TabStop = false;
      this.grpDocument.Text = "Destination";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 14);
      this.label1.TabIndex = 21;
      this.label1.Text = "Document Class";
      this.cboDocumentClasses.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboDocumentClasses.FormattingEnabled = true;
      this.cboDocumentClasses.Location = new Point(104, 21);
      this.cboDocumentClasses.Name = "cboDocumentClasses";
      this.cboDocumentClasses.Size = new Size(389, 22);
      this.cboDocumentClasses.Sorted = true;
      this.cboDocumentClasses.TabIndex = 2;
      this.helpLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Documents";
      this.helpLink.Location = new Point(12, 494);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 17);
      this.helpLink.TabIndex = 11;
      this.grpTrainingSource.Controls.Add((Control) this.lblProgress);
      this.grpTrainingSource.Controls.Add((Control) this.btnBrowse);
      this.grpTrainingSource.Controls.Add((Control) this.rdoTemplates);
      this.grpTrainingSource.Controls.Add((Control) this.rdoFile);
      this.grpTrainingSource.Dock = DockStyle.Top;
      this.grpTrainingSource.Location = new Point(0, 0);
      this.grpTrainingSource.Name = "grpTrainingSource";
      this.grpTrainingSource.Size = new Size(668, 82);
      this.grpTrainingSource.TabIndex = 24;
      this.grpTrainingSource.TabStop = false;
      this.grpTrainingSource.Text = "Source";
      this.lblProgress.AutoSize = true;
      this.lblProgress.Location = new Point(150, 27);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new Size(0, 14);
      this.lblProgress.TabIndex = 51;
      this.btnBrowse.BackColor = Color.Transparent;
      this.btnBrowse.DisabledImage = (Image) Resources.attach_browse_disabled;
      this.btnBrowse.Enabled = false;
      this.btnBrowse.Image = (Image) Resources.attach_browse;
      this.btnBrowse.Location = new Point(111, 23);
      this.btnBrowse.Margin = new Padding(4, 3, 0, 3);
      this.btnBrowse.MouseDownImage = (Image) null;
      this.btnBrowse.MouseOverImage = (Image) Resources.attach_browse_over;
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(16, 16);
      this.btnBrowse.TabIndex = 50;
      this.btnBrowse.TabStop = false;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.rdoTemplates.AutoSize = true;
      this.rdoTemplates.Location = new Point(9, 47);
      this.rdoTemplates.Name = "rdoTemplates";
      this.rdoTemplates.Size = new Size(73, 18);
      this.rdoTemplates.TabIndex = 49;
      this.rdoTemplates.TabStop = true;
      this.rdoTemplates.Text = "Templates";
      this.rdoTemplates.UseVisualStyleBackColor = true;
      this.rdoTemplates.CheckedChanged += new EventHandler(this.TrainingSource_CheckedChanged);
      this.rdoFile.AutoSize = true;
      this.rdoFile.Location = new Point(9, 23);
      this.rdoFile.Name = "rdoFile";
      this.rdoFile.Size = new Size(95, 18);
      this.rdoFile.TabIndex = 48;
      this.rdoFile.TabStop = true;
      this.rdoFile.Text = "Browse to File";
      this.rdoFile.UseVisualStyleBackColor = true;
      this.rdoFile.CheckedChanged += new EventHandler(this.TrainingSource_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(668, 463);
      this.Controls.Add((Control) this.grpTrainingSource);
      this.Controls.Add((Control) this.grpDocument);
      this.Controls.Add((Control) this.grpTemplateInfo);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SuggestTrainingSetupDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Suggest Training";
      this.grpTemplateInfo.ResumeLayout(false);
      this.gcText.ResumeLayout(false);
      this.gcText.PerformLayout();
      this.pnlImage.ResumeLayout(false);
      ((ISupportInitialize) this.pctImage).EndInit();
      this.grpDocument.ResumeLayout(false);
      this.grpDocument.PerformLayout();
      this.grpTrainingSource.ResumeLayout(false);
      this.grpTrainingSource.PerformLayout();
      ((ISupportInitialize) this.btnBrowse).EndInit();
      this.ResumeLayout(false);
    }
  }
}
