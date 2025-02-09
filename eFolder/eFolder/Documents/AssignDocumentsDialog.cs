// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.AssignDocumentsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class AssignDocumentsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionLog cond;
    private GridViewDataManager gvDocumentsMgr;
    private ArrayList docList = new ArrayList();
    private IContainer components;
    private Button btnCancel;
    private Button btnAssign;
    private CollapsibleSplitter csDocuments;
    private GroupContainer gcDocuments;
    private GridView gvDocuments;
    private Panel pnlFooter;
    private BorderPanel pnlRight;
    private FileAttachmentViewerControl fileViewer;
    private ToolTip tooltip;

    public AssignDocumentsDialog(LoanDataMgr loanDataMgr, ConditionLog cond)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.cond = cond;
      this.initDocumentList();
      this.loadDocumentList();
    }

    public DocumentLog[] Documents => (DocumentLog[]) this.docList.ToArray(typeof (DocumentLog));

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[3]
      {
        GridViewDataManager.HasAttachmentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn
      });
      this.gvDocuments.Sort(1, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
      {
        if (!allDocument.Conditions.Contains(this.cond))
          this.gvDocumentsMgr.AddItem(allDocument);
      }
      this.gvDocuments.ReSort();
    }

    private void gvDocuments_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAssign.Enabled = this.gvDocuments.SelectedItems.Count > 0;
    }

    private void gvDocuments_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showDocumentFiles();
    }

    private void showDocumentFiles()
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
        documentLogList.Add((DocumentLog) selectedItem.Tag);
      FileAttachment[] attachments = this.loanDataMgr.FileAttachments.GetAttachments(documentLogList.ToArray());
      if (attachments.Length != 0)
        this.fileViewer.LoadFiles(attachments);
      else
        this.fileViewer.CloseFile();
    }

    private void btnAssign_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
      {
        DocumentLog tag = (DocumentLog) selectedItem.Tag;
        tag.Conditions.Add(this.cond);
        this.docList.Add((object) tag);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void AssignDocumentsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
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
      this.btnCancel = new Button();
      this.btnAssign = new Button();
      this.csDocuments = new CollapsibleSplitter();
      this.gcDocuments = new GroupContainer();
      this.gvDocuments = new GridView();
      this.pnlFooter = new Panel();
      this.pnlRight = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.tooltip = new ToolTip(this.components);
      this.gcDocuments.SuspendLayout();
      this.pnlFooter.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(692, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnAssign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAssign.Enabled = false;
      this.btnAssign.Location = new Point(616, 9);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(75, 22);
      this.btnAssign.TabIndex = 6;
      this.btnAssign.Text = "Assign";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
      this.csDocuments.AnimationDelay = 20;
      this.csDocuments.AnimationStep = 20;
      this.csDocuments.BorderStyle3D = Border3DStyle.Flat;
      this.csDocuments.ControlToHide = (Control) this.gcDocuments;
      this.csDocuments.ExpandParentForm = false;
      this.csDocuments.Location = new Point(356, 0);
      this.csDocuments.Name = "csLeft";
      this.csDocuments.TabIndex = 2;
      this.csDocuments.TabStop = false;
      this.csDocuments.UseAnimations = false;
      this.csDocuments.VisualStyle = VisualStyles.Encompass;
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Dock = DockStyle.Left;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(356, 521);
      this.gcDocuments.TabIndex = 0;
      this.gcDocuments.Text = "Documents";
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HoverToolTip = this.tooltip;
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(354, 494);
      this.gvDocuments.TabIndex = 1;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.pnlFooter.Controls.Add((Control) this.btnCancel);
      this.pnlFooter.Controls.Add((Control) this.btnAssign);
      this.pnlFooter.Dock = DockStyle.Bottom;
      this.pnlFooter.Location = new Point(0, 521);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(774, 40);
      this.pnlFooter.TabIndex = 5;
      this.pnlRight.Controls.Add((Control) this.fileViewer);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(363, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(411, 521);
      this.pnlRight.TabIndex = 13;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(409, 519);
      this.fileViewer.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(774, 561);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csDocuments);
      this.Controls.Add((Control) this.gcDocuments);
      this.Controls.Add((Control) this.pnlFooter);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MinimizeBox = false;
      this.Name = nameof (AssignDocumentsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Assign Documents";
      this.FormClosing += new FormClosingEventHandler(this.AssignDocumentsDialog_FormClosing);
      this.gcDocuments.ResumeLayout(false);
      this.pnlFooter.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
