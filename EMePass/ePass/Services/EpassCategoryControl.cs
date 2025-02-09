// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Services.EpassCategoryControl
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.ePass.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.Services
{
  public class EpassCategoryControl : UserControl
  {
    private string title;
    private string documentType;
    private string requestType;
    private string url;
    private ToolStripMenuItem menuItem;
    private DocumentLog[] docList;
    private IContainer components;
    private GradientPanel pnlCategory;
    private PictureBox pctBullet;
    private Label lblRequestType;
    private Label lblTitle;
    private Panel pnlBorder;
    private IconButton btnRetrieve;
    private IconButton btnView;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;

    public EpassCategoryControl(string title, string documentType, string requestType, string url)
    {
      this.InitializeComponent();
      this.title = title;
      this.documentType = documentType;
      this.requestType = requestType;
      this.url = url;
      this.pnlBorder.BackColor = EncompassColors.Primary7;
      this.lblRequestType.ForeColor = EncompassColors.Primary4;
      this.lblRequestType.Text = requestType;
      this.lblTitle.ForeColor = EncompassColors.Primary4;
      this.lblTitle.Text = title;
      this.menuItem = this.createMenuItem();
    }

    public string Title => this.title;

    public string DocumentType => this.documentType;

    public string RequestType => this.requestType;

    public string Url => this.url;

    public ToolStripMenuItem MenuItem => this.menuItem;

    private ToolStripMenuItem createMenuItem()
    {
      string text = this.title.Replace("&", "&&");
      switch (text)
      {
        case "AVM":
          text = "A&VM";
          break;
        case "Additional Services":
          text = "Additio&nal Services";
          break;
        case "Appraisal":
          text = "&Appraisal";
          break;
        case "Appraisal Management":
          text = "Appraisal Mana&gement";
          break;
        case "Credit Report":
          text = "&Credit Report";
          break;
        case "Doc Preparation":
          text = "&Doc Preparation";
          break;
        case "Flood Certification":
          text = "&Flood Certification";
          break;
        case "Fraud/Audit Services":
          text = "Fraud/Aud&it Services";
          break;
        case "HMDA Management":
          text = "&HMDA Management";
          break;
        case "Lenders":
          text = "&Lenders";
          break;
        case "MERS":
          text = "M&ERS";
          break;
        case "Mortgage Insurance":
          text = "&Mortgage Insurance";
          break;
        case "Mortgage Signing":
          text = "Mortgage &Signing";
          break;
        case "My Custom Links":
          text = "My Custom Lin&ks";
          break;
        case "Product and Pricing":
          text = "&Product and Pricing";
          break;
        case "Tax Services":
          text = "Ta&x Services";
          break;
        case "Title && Closing":
          text = "&Title && Closing";
          break;
        case "Underwriting":
          text = "&Underwriting";
          break;
      }
      ToolStripMenuItem menuItem = new ToolStripMenuItem(text);
      menuItem.Click += new EventHandler(this.click);
      return menuItem;
    }

    public void RefreshContents(DocumentLog[] docList)
    {
      this.RefreshContents(docList, (DocumentLog[]) null);
    }

    public void RefreshContents(DocumentLog[] docList, DocumentLog[] docListDeleted)
    {
      this.docList = this.getCategoryList(docList);
      bool flag1 = false;
      bool flag2 = false;
      foreach (DocumentLog doc in this.docList)
      {
        if (doc.Received)
          flag2 = true;
        else if (doc.EPASSSignature != string.Empty)
          flag1 = true;
      }
      this.btnRetrieve.Visible = flag1;
      this.btnView.Visible = flag2 && !flag1;
      if (flag1 || docListDeleted == null)
        return;
      foreach (DocumentLog documentLog in docListDeleted)
      {
        if (documentLog.Title.StartsWith("Appraisal") && this.lblTitle.Text == "Appraisal" || documentLog.Title.StartsWith("Title Report") && this.lblTitle.Text == "Title & Closing")
        {
          this.btnView.Visible = false;
          this.btnRetrieve.Visible = true;
          break;
        }
      }
    }

    private DocumentLog[] getCategoryList(DocumentLog[] docList)
    {
      if (this.documentType == "All")
        return docList;
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      if (this.documentType != null)
      {
        foreach (DocumentLog doc in docList)
        {
          if (doc.Title.EndsWith(this.documentType))
            documentLogList.Add(doc);
        }
        if (this.documentType == Epass.Title.FullName)
        {
          foreach (DocumentLog doc in docList)
          {
            if (doc.Title.EndsWith(Epass.Closing.FullName))
              documentLogList.Add(doc);
          }
        }
      }
      return documentLogList.ToArray();
    }

    private void EpassCategoryControl_Resize(object sender, EventArgs e)
    {
      if (this.Height == 24)
        return;
      this.Height = 24;
    }

    private void lblRequestType_SizeChanged(object sender, EventArgs e)
    {
      this.lblTitle.Left = this.lblRequestType.Right - 4;
    }

    private void mouseEnter(object sender, EventArgs e)
    {
      this.pnlBorder.BackColor = EncompassColors.Accent1;
    }

    private void mouseLeave(object sender, EventArgs e)
    {
      this.pnlBorder.BackColor = EncompassColors.Primary7;
    }

    private void click(object sender, EventArgs e)
    {
      if (this.documentType == "Product and Pricing")
      {
        Session.LoanData.SetField("OPTIMAL.REQUEST", "");
        Session.LoanData.SetField("OPTIMAL.RESPONSE", "");
        if (Session.StartupInfo.ProductPricingPartner != null && Session.StartupInfo.ProductPricingPartner.IsEPPS && LoanLockUtils.GetAllowedRequestType(Session.SessionObjects, Session.LoanDataMgr) == LoanLockUtils.AllowedRequestType.ReLockOnly && !LoanLockUtils.IsAllowGetPricingForReLock(Session.SessionObjects, Session.LoanDataMgr))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The pricing request violates your administrative settings and cannot be processed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      Session.Application.GetService<IEPass>().ProcessURL(this.url);
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.documentType == "Appraisal")
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;PREFERREDAPPRAISER;APPRAISALLOG");
      else if (this.documentType == "Title Report" || this.documentType == "Escrow/Closing")
      {
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;PREFERREDTITLE;TITLELOG");
      }
      else
      {
        using (ViewDocumentDialog viewDocumentDialog = new ViewDocumentDialog(Session.LoanDataMgr, this.docList))
        {
          switch (viewDocumentDialog.ShowDialog((IWin32Window) Form.ActiveForm))
          {
            case DialogResult.OK:
              Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, viewDocumentDialog.Document);
              break;
            case DialogResult.Retry:
              Session.Application.GetService<IEPass>().Retrieve(viewDocumentDialog.Document);
              break;
          }
        }
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
      this.components = (IContainer) new System.ComponentModel.Container();
      this.pnlCategory = new GradientPanel();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnView = new IconButton();
      this.btnRetrieve = new IconButton();
      this.lblTitle = new Label();
      this.lblRequestType = new Label();
      this.pctBullet = new PictureBox();
      this.pnlBorder = new Panel();
      this.tooltip = new ToolTip(this.components);
      this.pnlCategory.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnView).BeginInit();
      ((ISupportInitialize) this.btnRetrieve).BeginInit();
      ((ISupportInitialize) this.pctBullet).BeginInit();
      this.pnlBorder.SuspendLayout();
      this.SuspendLayout();
      this.pnlCategory.Borders = AnchorStyles.None;
      this.pnlCategory.Controls.Add((Control) this.pnlToolbar);
      this.pnlCategory.Controls.Add((Control) this.lblTitle);
      this.pnlCategory.Controls.Add((Control) this.lblRequestType);
      this.pnlCategory.Controls.Add((Control) this.pctBullet);
      this.pnlCategory.Cursor = Cursors.Hand;
      this.pnlCategory.Dock = DockStyle.Fill;
      this.pnlCategory.GradientColor1 = Color.White;
      this.pnlCategory.GradientColor2 = Color.WhiteSmoke;
      this.pnlCategory.Location = new Point(1, 1);
      this.pnlCategory.Name = "pnlCategory";
      this.pnlCategory.Size = new Size(235, 20);
      this.pnlCategory.TabIndex = 1;
      this.pnlCategory.MouseLeave += new EventHandler(this.mouseLeave);
      this.pnlCategory.Click += new EventHandler(this.click);
      this.pnlCategory.MouseEnter += new EventHandler(this.mouseEnter);
      this.pnlToolbar.AutoSize = true;
      this.pnlToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnView);
      this.pnlToolbar.Controls.Add((Control) this.btnRetrieve);
      this.pnlToolbar.Dock = DockStyle.Right;
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(195, 0);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(40, 20);
      this.pnlToolbar.TabIndex = 4;
      this.pnlToolbar.WrapContents = false;
      this.pnlToolbar.MouseLeave += new EventHandler(this.mouseLeave);
      this.pnlToolbar.Click += new EventHandler(this.click);
      this.pnlToolbar.MouseEnter += new EventHandler(this.mouseEnter);
      this.btnView.BackColor = Color.Transparent;
      this.btnView.Cursor = Cursors.Default;
      this.btnView.DisabledImage = (Image) null;
      this.btnView.Image = (Image) Resources.document;
      this.btnView.Location = new Point(20, 2);
      this.btnView.Margin = new Padding(0, 2, 4, 2);
      this.btnView.MouseOverImage = (Image) Resources.document_over;
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(16, 16);
      this.btnView.TabIndex = 34;
      this.btnView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnView, "View Document");
      this.btnView.Visible = false;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.btnRetrieve.BackColor = Color.Transparent;
      this.btnRetrieve.Cursor = Cursors.Default;
      this.btnRetrieve.DisabledImage = (Image) null;
      this.btnRetrieve.Image = (Image) Resources.waiting;
      this.btnRetrieve.Location = new Point(0, 2);
      this.btnRetrieve.Margin = new Padding(0, 2, 4, 2);
      this.btnRetrieve.MouseOverImage = (Image) Resources.waiting_over;
      this.btnRetrieve.Name = "btnRetrieve";
      this.btnRetrieve.Size = new Size(16, 16);
      this.btnRetrieve.TabIndex = 33;
      this.btnRetrieve.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRetrieve, "Check Status");
      this.btnRetrieve.Visible = false;
      this.btnRetrieve.Click += new EventHandler(this.btnView_Click);
      this.lblTitle.AutoSize = true;
      this.lblTitle.BackColor = Color.Transparent;
      this.lblTitle.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(96, 3);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(31, 14);
      this.lblTitle.TabIndex = 3;
      this.lblTitle.Text = "Title";
      this.lblTitle.UseMnemonic = false;
      this.lblTitle.MouseLeave += new EventHandler(this.mouseLeave);
      this.lblTitle.Click += new EventHandler(this.click);
      this.lblTitle.MouseEnter += new EventHandler(this.mouseEnter);
      this.lblRequestType.AutoSize = true;
      this.lblRequestType.BackColor = Color.Transparent;
      this.lblRequestType.Location = new Point(18, 3);
      this.lblRequestType.Name = "lblRequestType";
      this.lblRequestType.Size = new Size(74, 14);
      this.lblRequestType.TabIndex = 2;
      this.lblRequestType.Text = "Request Type";
      this.lblRequestType.MouseLeave += new EventHandler(this.mouseLeave);
      this.lblRequestType.Click += new EventHandler(this.click);
      this.lblRequestType.SizeChanged += new EventHandler(this.lblRequestType_SizeChanged);
      this.lblRequestType.MouseEnter += new EventHandler(this.mouseEnter);
      this.pctBullet.BackColor = Color.Transparent;
      this.pctBullet.Image = (Image) Resources.bullet;
      this.pctBullet.Location = new Point(9, 8);
      this.pctBullet.Name = "pctBullet";
      this.pctBullet.Size = new Size(7, 5);
      this.pctBullet.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pctBullet.TabIndex = 0;
      this.pctBullet.TabStop = false;
      this.pctBullet.MouseLeave += new EventHandler(this.mouseLeave);
      this.pctBullet.Click += new EventHandler(this.click);
      this.pctBullet.MouseEnter += new EventHandler(this.mouseEnter);
      this.pnlBorder.Controls.Add((Control) this.pnlCategory);
      this.pnlBorder.Dock = DockStyle.Fill;
      this.pnlBorder.Location = new Point(1, 1);
      this.pnlBorder.Name = "pnlBorder";
      this.pnlBorder.Padding = new Padding(1);
      this.pnlBorder.Size = new Size(237, 22);
      this.pnlBorder.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlBorder);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EpassCategoryControl);
      this.Padding = new Padding(1);
      this.Size = new Size(239, 24);
      this.Resize += new EventHandler(this.EpassCategoryControl_Resize);
      this.pnlCategory.ResumeLayout(false);
      this.pnlCategory.PerformLayout();
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnView).EndInit();
      ((ISupportInitialize) this.btnRetrieve).EndInit();
      ((ISupportInitialize) this.pctBullet).EndInit();
      this.pnlBorder.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
