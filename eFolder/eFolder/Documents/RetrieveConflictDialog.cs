// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.RetrieveConflictDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class RetrieveConflictDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog[] docList;
    private GridViewDataManager gvDownloadsMgr;
    private GridViewDataManager gvEpassMgr;
    private List<GVItem> existingList = new List<GVItem>();
    private List<FileAttachment> fileList = new List<FileAttachment>();
    private DocumentLog doc;
    private IContainer components;
    private Button btnCancel;
    private GroupContainer gcDownloads;
    private GridView gvDownloads;
    private Button btnDownload;
    private GroupContainer gcEpass;
    private Button btnRetrieve;
    private GridView gvEpass;
    private Label lblRetrieve;
    private ToolTip tooltip;
    private CheckBox chkShowExisting;
    private EMHelpLink helpLink;

    public RetrieveConflictDialog(LoanDataMgr loanDataMgr, DocumentLog[] docList)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.docList = docList;
      this.initDownloadList();
      this.loadDownloadList();
      this.initEpassList();
      this.loadEpassList();
    }

    public FileAttachment[] Files => this.fileList.ToArray();

    private void initDownloadList()
    {
      this.gvDownloadsMgr = new GridViewDataManager(this.gvDownloads, this.loanDataMgr);
      this.gvDownloadsMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.CheckBoxColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.SenderColumn,
        GridViewDataManager.DateTimeColumn
      });
      this.gvDownloads.Sort(3, SortOrder.Ascending);
    }

    private void loadDownloadList()
    {
      DownloadLog[] allDownloads = this.loanDataMgr.LoanData.GetLogList().GetAllDownloads();
      foreach (DownloadLog availableDownload in RetrieveDownloadDialog.GetAvailableDownloads(this.loanDataMgr))
      {
        GVItem gvItem = this.gvDownloadsMgr.AddItem(availableDownload);
        gvItem.Checked = true;
        foreach (DownloadLog downloadLog in allDownloads)
        {
          if (downloadLog.FileSource == availableDownload.FileSource && downloadLog.FileID == availableDownload.FileID)
            this.existingList.Add(gvItem);
        }
      }
      this.gvDownloads.ReSort();
      foreach (GVItem existing in this.existingList)
        existing.State = GVItemState.Hidden;
      if (this.gvDownloads.VisibleItems.Count <= 0)
        return;
      this.btnDownload.Enabled = true;
    }

    private void chkShowExisting_CheckedChanged(object sender, EventArgs e)
    {
      bool flag = this.chkShowExisting.Checked;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDownloads.Items)
        gvItem.State = !this.existingList.Contains(gvItem) ? (flag ? GVItemState.Hidden : GVItemState.Normal) : (flag ? GVItemState.Normal : GVItemState.Hidden);
      this.gvDownloads.ReSort();
      this.enableDownload();
    }

    private void gvDownloads_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.enableDownload();
    }

    private void enableDownload()
    {
      bool flag = false;
      foreach (GVItem visibleItem in this.gvDownloads.VisibleItems)
      {
        if (visibleItem.Checked)
          flag = true;
      }
      this.btnDownload.Enabled = flag;
    }

    private void btnDownload_Click(object sender, EventArgs e)
    {
      foreach (GVItem visibleItem in this.gvDownloads.VisibleItems)
      {
        DownloadLog tag = (DownloadLog) visibleItem.Tag;
        if (visibleItem.Checked)
        {
          using (RetrieveDownloadDialog retrieveDownloadDialog = new RetrieveDownloadDialog(this.loanDataMgr))
          {
            FileAttachment[] collection = retrieveDownloadDialog.Retrieve(tag);
            if (collection != null)
              this.fileList.AddRange((IEnumerable<FileAttachment>) collection);
            else
              break;
          }
        }
      }
      if (this.fileList.Count <= 0)
        return;
      this.DialogResult = DialogResult.OK;
    }

    public DocumentLog Document => this.doc;

    private void initEpassList()
    {
      this.gvEpassMgr = new GridViewDataManager(this.gvEpass, this.loanDataMgr);
      this.gvEpassMgr.CreateLayout(new TableLayout.Column[4]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DateColumn
      });
    }

    private void loadEpassList()
    {
      foreach (DocumentLog doc in this.docList)
      {
        if (Epass.IsEpassDoc(doc.Title) || doc.IsePASS)
          this.gvEpassMgr.AddItem(doc);
      }
    }

    private void gvEpass_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRetrieve.Enabled = this.gvEpass.SelectedItems.Count > 0;
    }

    private void btnRetrieve_Click(object sender, EventArgs e)
    {
      this.doc = (DocumentLog) this.gvEpass.SelectedItems[0].Tag;
      this.DialogResult = DialogResult.OK;
    }

    private void RetrieveConflictDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Session.Application.DisplayHelp(this.helpLink.HelpTag);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RetrieveConflictDialog));
      this.btnCancel = new Button();
      this.gcDownloads = new GroupContainer();
      this.chkShowExisting = new CheckBox();
      this.gvDownloads = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.btnDownload = new Button();
      this.gcEpass = new GroupContainer();
      this.btnRetrieve = new Button();
      this.gvEpass = new GridView();
      this.lblRetrieve = new Label();
      this.helpLink = new EMHelpLink();
      this.gcDownloads.SuspendLayout();
      this.gcEpass.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(498, 492);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gcDownloads.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDownloads.Controls.Add((Control) this.chkShowExisting);
      this.gcDownloads.Controls.Add((Control) this.gvDownloads);
      this.gcDownloads.Controls.Add((Control) this.btnDownload);
      this.gcDownloads.Location = new Point(8, 44);
      this.gcDownloads.Name = "gcDownloads";
      this.gcDownloads.Size = new Size(564, 216);
      this.gcDownloads.TabIndex = 1;
      this.gcDownloads.Text = "Received from Borrower";
      this.chkShowExisting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkShowExisting.AutoSize = true;
      this.chkShowExisting.BackColor = Color.Transparent;
      this.chkShowExisting.Location = new Point(292, 5);
      this.chkShowExisting.Name = "chkShowExisting";
      this.chkShowExisting.Size = new Size(199, 18);
      this.chkShowExisting.TabIndex = 3;
      this.chkShowExisting.Text = "Show previously downloaded items";
      this.chkShowExisting.UseVisualStyleBackColor = false;
      this.chkShowExisting.CheckedChanged += new EventHandler(this.chkShowExisting_CheckedChanged);
      this.gvDownloads.AllowMultiselect = false;
      this.gvDownloads.BorderStyle = BorderStyle.None;
      this.gvDownloads.ClearSelectionsOnEmptyRowClick = false;
      this.gvDownloads.Dock = DockStyle.Fill;
      this.gvDownloads.HoverToolTip = this.tooltip;
      this.gvDownloads.Location = new Point(1, 26);
      this.gvDownloads.Name = "gvDownloads";
      this.gvDownloads.Size = new Size(562, 189);
      this.gvDownloads.TabIndex = 2;
      this.gvDownloads.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDownloads.SubItemCheck += new GVSubItemEventHandler(this.gvDownloads_SubItemCheck);
      this.btnDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDownload.Enabled = false;
      this.btnDownload.Location = new Point(492, 2);
      this.btnDownload.Name = "btnDownload";
      this.btnDownload.Size = new Size(68, 22);
      this.btnDownload.TabIndex = 4;
      this.btnDownload.Text = "Download";
      this.btnDownload.UseVisualStyleBackColor = true;
      this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
      this.gcEpass.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcEpass.Controls.Add((Control) this.btnRetrieve);
      this.gcEpass.Controls.Add((Control) this.gvEpass);
      this.gcEpass.Location = new Point(8, 268);
      this.gcEpass.Name = "gcEpass";
      this.gcEpass.Size = new Size(564, 216);
      this.gcEpass.TabIndex = 5;
      this.gcEpass.Text = "OR Retrieve from Service Providers";
      this.btnRetrieve.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRetrieve.Enabled = false;
      this.btnRetrieve.Location = new Point(500, 2);
      this.btnRetrieve.Name = "btnRetrieve";
      this.btnRetrieve.Size = new Size(60, 22);
      this.btnRetrieve.TabIndex = 7;
      this.btnRetrieve.Text = "Retrieve";
      this.btnRetrieve.UseVisualStyleBackColor = true;
      this.btnRetrieve.Click += new EventHandler(this.btnRetrieve_Click);
      this.gvEpass.AllowMultiselect = false;
      this.gvEpass.BorderStyle = BorderStyle.None;
      this.gvEpass.ClearSelectionsOnEmptyRowClick = false;
      this.gvEpass.Dock = DockStyle.Fill;
      this.gvEpass.HoverToolTip = this.tooltip;
      this.gvEpass.Location = new Point(1, 26);
      this.gvEpass.Name = "gvEpass";
      this.gvEpass.Size = new Size(562, 189);
      this.gvEpass.TabIndex = 6;
      this.gvEpass.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvEpass.SelectedIndexChanged += new EventHandler(this.gvEpass_SelectedIndexChanged);
      this.lblRetrieve.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblRetrieve.Location = new Point(8, 8);
      this.lblRetrieve.Name = "lblRetrieve";
      this.lblRetrieve.Size = new Size(564, 28);
      this.lblRetrieve.TabIndex = 0;
      this.lblRetrieve.Text = componentResourceManager.GetString("lblRetrieve.Text");
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.HelpTag = "Retrieve Documents";
      this.helpLink.Location = new Point(8, 494);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 9;
      this.helpLink.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(580, 523);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gcDownloads);
      this.Controls.Add((Control) this.gcEpass);
      this.Controls.Add((Control) this.lblRetrieve);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RetrieveConflictDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Retrieve";
      this.KeyDown += new KeyEventHandler(this.RetrieveConflictDialog_KeyDown);
      this.gcDownloads.ResumeLayout(false);
      this.gcDownloads.PerformLayout();
      this.gcEpass.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
