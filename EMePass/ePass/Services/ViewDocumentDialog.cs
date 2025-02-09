// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Services.ViewDocumentDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.Services
{
  public class ViewDocumentDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog[] docList;
    private GridViewDataManager gvDocumentsMgr;
    private DocumentLog doc;
    private IContainer components;
    private Button btnView;
    private Button btnCancel;
    private Button btnRetrieve;
    private GroupContainer gcEpass;
    private GridView gvDocuments;

    public ViewDocumentDialog(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.docList = docList;
      this.initDocumentList();
      this.loadDocumentList();
    }

    public DocumentLog Document => this.doc;

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[6]
      {
        GridViewDataManager.HasAttachmentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DateColumn
      });
    }

    private void loadDocumentList()
    {
      foreach (DocumentLog doc in this.docList)
        this.gvDocumentsMgr.AddItem(doc);
      this.gvDocuments.Sort(1, SortOrder.Descending);
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvDocuments.SelectedItems.Count;
      this.btnRetrieve.Enabled = count > 0;
      this.btnView.Enabled = count > 0;
    }

    private void btnRetrieve_Click(object sender, EventArgs e)
    {
      this.doc = (DocumentLog) this.gvDocuments.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.Retry;
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      this.doc = (DocumentLog) this.gvDocuments.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnView = new Button();
      this.btnCancel = new Button();
      this.btnRetrieve = new Button();
      this.gcEpass = new GroupContainer();
      this.gvDocuments = new GridView();
      this.gcEpass.SuspendLayout();
      this.SuspendLayout();
      this.btnView.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnView.Enabled = false;
      this.btnView.Location = new Point(576, 360);
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(101, 22);
      this.btnView.TabIndex = 3;
      this.btnView.Text = "View Document";
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(678, 360);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnRetrieve.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnRetrieve.Enabled = false;
      this.btnRetrieve.Location = new Point(500, 360);
      this.btnRetrieve.Name = "btnRetrieve";
      this.btnRetrieve.Size = new Size(75, 22);
      this.btnRetrieve.TabIndex = 2;
      this.btnRetrieve.Text = "Retrieve";
      this.btnRetrieve.Click += new EventHandler(this.btnRetrieve_Click);
      this.gcEpass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcEpass.Controls.Add((Control) this.gvDocuments);
      this.gcEpass.Location = new Point(8, 8);
      this.gcEpass.Name = "gcEpass";
      this.gcEpass.Size = new Size(744, 344);
      this.gcEpass.TabIndex = 0;
      this.gcEpass.Text = "From Service Providers";
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(742, 317);
      this.gvDocuments.TabIndex = 1;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(759, 390);
      this.Controls.Add((Control) this.gcEpass);
      this.Controls.Add((Control) this.btnRetrieve);
      this.Controls.Add((Control) this.btnView);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewDocumentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Services";
      this.gcEpass.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
