// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.eFolder.SelectDocumentDialog
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
namespace EllieMae.EMLite.ePass.eFolder
{
  public class SelectDocumentDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvDocumentsMgr;
    private DocumentLog doc;
    private IContainer components;
    private Label lblSelect;
    private Button btnContinue;
    private GridView gvDocuments;
    private ToolTip tooltip;
    private Button btnCancel;

    public SelectDocumentDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
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
      this.gvDocuments.Sort(1, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
      {
        if (allDocument.IsThirdPartyDoc && this.loanDataMgr.FileAttachments.ContainsAttachment(allDocument))
          this.gvDocumentsMgr.AddItem(allDocument);
      }
      this.gvDocuments.ReSort();
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnContinue.Enabled = this.gvDocuments.SelectedItems.Count > 0;
    }

    private void btnContinue_Click(object sender, EventArgs e)
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
      this.components = (IContainer) new System.ComponentModel.Container();
      this.lblSelect = new Label();
      this.btnContinue = new Button();
      this.gvDocuments = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblSelect.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSelect.AutoSize = true;
      this.lblSelect.Location = new Point(12, 12);
      this.lblSelect.Name = "lblSelect";
      this.lblSelect.Size = new Size(434, 14);
      this.lblSelect.TabIndex = 0;
      this.lblSelect.Text = "Only a document with attachments can be selected. Click Continue when done selecting.";
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.Enabled = false;
      this.btnContinue.Location = new Point(639, 491);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(75, 22);
      this.btnContinue.TabIndex = 2;
      this.btnContinue.Text = "Continue";
      this.btnContinue.UseVisualStyleBackColor = true;
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Location = new Point(12, 36);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(777, 447);
      this.gvDocuments.SortOption = GVSortOption.None;
      this.gvDocuments.TabIndex = 1;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(715, 491);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnContinue;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(800, 523);
      this.Controls.Add((Control) this.lblSelect);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.gvDocuments);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectDocumentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Document";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
