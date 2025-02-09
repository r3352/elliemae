// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.EVaultRetrieveDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.WebServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class EVaultRetrieveDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvDocumentsMgr;
    private string auditPath;
    private string _tokenType = string.Empty;
    private string _accessToken = string.Empty;
    private const string ClassName = "EVaultRetrieveDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private IContainer components;
    private GridView gvDocuments;
    private Button btnContinue;
    private Button btnCancel;
    private Button btnPreview;
    private GroupContainer docListContainer;
    private GradientPanel gradientPanel3;
    private Label lblInfo2;

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public EVaultRetrieveDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
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
      Session.Application.GetService<IEFolder>();
      this.gvDocuments.Items.Clear();
      LoanData loan = this.loanDataMgr.LoanData;
      CompanyInfo companyInfo = Session.CompanyInfo;
      UserInfo userInfo = Session.UserInfo;
      this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken =>
      {
        if (this.loanDataMgr.IsPlatformLoan() && accessToken != null)
        {
          this._tokenType = accessToken.Type;
          this._accessToken = accessToken.Token;
        }
        using (EVaultRetrieve evaultRetrieve = new EVaultRetrieve(this._tokenType, this._accessToken, Session.SessionObjects?.StartupInfo.ServiceUrls?.EVaultRetrieveServiceUrl))
        {
          foreach (LoanCenterDocument signedDocument in evaultRetrieve.GetSignedDocumentList(new EVaultRetrieveCredentials()
          {
            ClientID = companyInfo.ClientID,
            UserID = userInfo.Userid,
            Password = EpassLogin.LoginPassword
          }, companyInfo.ClientID, loan.GetSimpleField("GUID").Replace("{", string.Empty).Replace("}", string.Empty)))
          {
            if (loan.GetLogList().GetRecordByID(signedDocument.DocumentGUID, false, true) is DocumentLog recordById2)
              this.gvDocumentsMgr.AddItem(signedDocument, recordById2);
          }
        }
        this.gvDocuments.Sort(0, SortOrder.Ascending);
        this.docListContainer.Text = "eVault Documents (" + this.gvDocuments.Items.Count.ToString() + ")";
      }));
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IEFolder>();
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
        this.Cursor = Cursors.WaitCursor;
        LoanData loanData = this.loanDataMgr.LoanData;
        LoanCenterDocument document = this.gvDocuments.SelectedItems[0].Tag as LoanCenterDocument;
        CompanyInfo companyInfo = Session.CompanyInfo;
        UserInfo userInfo = Session.UserInfo;
        this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken =>
        {
          if (this.loanDataMgr.IsPlatformLoan() && accessToken != null)
          {
            this._tokenType = accessToken.Type;
            this._accessToken = accessToken.Token;
          }
          using (EVaultRetrieve evaultRetrieve = new EVaultRetrieve(this._tokenType, this._accessToken, Session.SessionObjects?.StartupInfo.ServiceUrls?.EVaultRetrieveServiceUrl))
          {
            SignedDocument signedDocument = evaultRetrieve.GetSignedDocument(new EVaultRetrieveCredentials()
            {
              ClientID = companyInfo.ClientID,
              UserID = userInfo.Userid,
              Password = EpassLogin.LoginPassword
            }, document);
            string path = SystemSettings.TempFolderRoot + "EVaultRetrieve\\";
            try
            {
              if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
              this.Cursor = Cursors.Default;
              int num3 = (int) Utils.Dialog((IWin32Window) this, "Failed to create temporary location at " + path + ".");
              return;
            }
            string str;
            try
            {
              BinaryObject binaryObject = new BinaryObject(Convert.FromBase64String(signedDocument.DataBase64));
              str = path + document.PackageEntityGUID + ".pdf";
              binaryObject.Write(str);
            }
            catch (Exception ex)
            {
              this.Cursor = Cursors.Default;
              int num4 = (int) Utils.Dialog((IWin32Window) this, "Signed document is not available at this time.");
              return;
            }
            this.Cursor = Cursors.Default;
            int num5 = (int) new PdfPreviewDialog(str, false, false, false).ShowDialog((IWin32Window) this);
          }
        }));
      }
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IEFolder>();
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
        this.Cursor = Cursors.WaitCursor;
        LoanData loanData = this.loanDataMgr.LoanData;
        LoanCenterDocument document = this.gvDocuments.SelectedItems[0].Tag as LoanCenterDocument;
        CompanyInfo companyInfo = Session.CompanyInfo;
        UserInfo userInfo = Session.UserInfo;
        this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken =>
        {
          if (this.loanDataMgr.IsPlatformLoan() && accessToken != null)
          {
            this._tokenType = accessToken.Type;
            this._accessToken = accessToken.Token;
          }
          using (EVaultRetrieve evaultRetrieve = new EVaultRetrieve(this._tokenType, this._accessToken, Session.SessionObjects?.StartupInfo.ServiceUrls?.EVaultRetrieveServiceUrl))
          {
            string auditData = evaultRetrieve.GetAuditData(new EVaultRetrieveCredentials()
            {
              ClientID = companyInfo.ClientID,
              UserID = userInfo.Userid,
              Password = EpassLogin.LoginPassword
            }, document);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(auditData);
            string path = SystemSettings.TempFolderRoot + "EVaultRetrieve\\";
            try
            {
              if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
              this.Cursor = Cursors.Default;
              int num3 = (int) Utils.Dialog((IWin32Window) this, "Failed to create temporary location at " + path + ".");
              return;
            }
            string filename = path + document.PackageEntityGUID + ".xml";
            xmlDocument.Save(filename);
            this.auditPath = filename;
            this.Cursor = Cursors.Default;
            this.DialogResult = DialogResult.OK;
          }
        }));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EVaultRetrieveDialog));
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
      this.docListContainer.Text = "eVault Documents (0)";
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
      this.Name = nameof (EVaultRetrieveDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select a Document";
      this.docListContainer.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
