// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.eDeliveryRetrieveDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.EDelivery;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class eDeliveryRetrieveDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvDocumentsMgr;
    private string auditPath;
    private string _tokenType = string.Empty;
    private string _accessToken = string.Empty;
    private const string ClassName = "eDeliveryRetrieveDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private IContainer components;
    private GridView gvDocuments;
    private Button btnContinue;
    private Button btnCancel;
    private Button btnPreview;
    private GroupContainer docListContainer;
    private GradientPanel gradientPanel3;
    private Label lblInfo2;

    public eDeliveryRetrieveDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.auditPath = string.Empty;
      this.initDocumentList();
      this.loadDocumentList();
    }

    public string AuditPath => this.auditPath;

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[5]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DateTimeColumn
      });
    }

    private void loadDocumentList()
    {
      this.gvDocuments.Items.Clear();
      LoanData loanData = this.loanDataMgr.LoanData;
      foreach (EDeliverySignedDocument document in new EDeliveryRestClient(this.loanDataMgr).GetSignedDocumentList(loanData.GetSimpleField("GUID").Replace("{", string.Empty).Replace("}", string.Empty)).Result)
      {
        if (loanData.GetLogList().GetRecordByID(document.documentId, false, true) is DocumentLog recordById)
          this.gvDocumentsMgr.AddItem(document, recordById);
      }
      this.gvDocuments.Sort(0, SortOrder.Ascending);
      this.docListContainer.Text = "Documents (" + this.gvDocuments.Items.Count.ToString() + ")";
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      if (this.gvDocuments.SelectedItems == null)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "There are no documents to select.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.gvDocuments.SelectedItems.Count == 0)
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "Please select a document.", "Preview", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        EDeliverySignedDocument tag = this.gvDocuments.SelectedItems[0].Tag as EDeliverySignedDocument;
        string loanGuid = this.loanDataMgr.LoanData.GetSimpleField("GUID").Replace("{", string.Empty).Replace("}", string.Empty);
        EDeliveryServiceClient edeliveryServiceClient = new EDeliveryServiceClient(this.loanDataMgr);
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetTempPath() + tag.packageId);
        if (directoryInfo.Exists)
          directoryInfo.Delete(true);
        directoryInfo.Create();
        Task<string> task = edeliveryServiceClient.DownloadFilesFromMediaServer((Request) null, loanGuid, tag.packageId, tag.documentId, directoryInfo.FullName);
        this.Cursor = Cursors.Default;
        int num3 = (int) new PdfPreviewDialog(task.Result, false, false, false).ShowDialog((IWin32Window) this);
      }
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      if (this.gvDocuments.SelectedItems == null)
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "There are no documents to select.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.gvDocuments.SelectedItems.Count == 0)
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "Please select a document.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        EDeliverySignedDocument tag = this.gvDocuments.SelectedItems[0].Tag as EDeliverySignedDocument;
        string loanId = this.loanDataMgr.LoanData.GetSimpleField("GUID").Replace("{", string.Empty).Replace("}", string.Empty);
        string empty = string.Empty;
        EDeliveryRestClient edeliveryRestClient = new EDeliveryRestClient(this.loanDataMgr);
        string result;
        try
        {
          result = edeliveryRestClient.GetVaultExportData(loanId, tag.packageId, tag.documentId).Result;
        }
        catch (Exception ex)
        {
          if (ex.Message == "Conflict" || ex.InnerException.Message == "Conflict")
          {
            this.Cursor = Cursors.Default;
            int num3 = (int) Utils.Dialog((IWin32Window) this, "Ordering 4506T and 4506C is not currently supported for eClosing.");
            return;
          }
          throw;
        }
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(result);
        string path = SystemSettings.TempFolderRoot + "eDeliveryRetrieve\\";
        try
        {
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        }
        catch (Exception ex)
        {
          this.Cursor = Cursors.Default;
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Failed to create temporary location at " + path + ".");
          return;
        }
        string filename = path + tag.packageId + ".xml";
        xmlDocument.Save(filename);
        this.auditPath = filename;
        this.Cursor = Cursors.Default;
        this.DialogResult = DialogResult.OK;
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
      this.btnContinue = new Button();
      this.btnCancel = new Button();
      this.btnPreview = new Button();
      this.docListContainer = new GroupContainer();
      this.gradientPanel3 = new GradientPanel();
      this.lblInfo2 = new Label();
      this.gvDocuments = new GridView();
      this.docListContainer.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.SuspendLayout();
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.Location = new Point(550, 357);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 24);
      this.btnContinue.TabIndex = 1;
      this.btnContinue.Text = "Continue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(710, 357);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnPreview.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnPreview.Location = new Point(630, 357);
      this.btnPreview.Name = "btnPreview";
      this.btnPreview.Size = new Size(75, 24);
      this.btnPreview.TabIndex = 4;
      this.btnPreview.Text = "Preview";
      this.btnPreview.UseVisualStyleBackColor = true;
      this.btnPreview.Click += new EventHandler(this.btnPreview_Click);
      this.docListContainer.Controls.Add((Control) this.gradientPanel3);
      this.docListContainer.Controls.Add((Control) this.gvDocuments);
      this.docListContainer.HeaderForeColor = SystemColors.ControlText;
      this.docListContainer.Location = new Point(6, 8);
      this.docListContainer.Name = "docListContainer";
      this.docListContainer.Size = new Size(780, 341);
      this.docListContainer.TabIndex = 7;
      this.docListContainer.Text = "Documents (0)";
      this.gradientPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.lblInfo2);
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 25);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(780, 28);
      this.gradientPanel3.TabIndex = 54;
      this.lblInfo2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblInfo2.BackColor = Color.Transparent;
      this.lblInfo2.Location = new Point(7, 7);
      this.lblInfo2.Name = "lblInfo2";
      this.lblInfo2.Size = new Size(762, 15);
      this.lblInfo2.TabIndex = 1;
      this.lblInfo2.Text = "Select an attachment from the list below.  You can Preview documents before selecting.";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(0, 55);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(780, 286);
      this.gvDocuments.TabIndex = 0;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(793, 391);
      this.Controls.Add((Control) this.docListContainer);
      this.Controls.Add((Control) this.btnPreview);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnContinue);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = Resources.icon_allsizes_bug;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "RetrieveDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select a Document";
      this.docListContainer.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
