// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.RetrieveBorrowerDialog
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
  public class RetrieveBorrowerDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvDownloadsMgr;
    private List<GVItem> existingList = new List<GVItem>();
    private List<FileAttachment> fileList = new List<FileAttachment>();
    private IContainer components;
    private ToolTip tooltip;
    private Button btnCancel;
    private Button btnDownload;
    private GroupContainer gcDownloads;
    private GridView gvDownloads;
    private CheckBox chkShowExisting;
    private EMHelpLink helpLink;

    public RetrieveBorrowerDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.initDownloadList();
      this.loadDownloadList();
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

    private void RetrieveBorrowerDialog_KeyDown(object sender, KeyEventArgs e)
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
      this.tooltip = new ToolTip(this.components);
      this.btnCancel = new Button();
      this.btnDownload = new Button();
      this.gcDownloads = new GroupContainer();
      this.chkShowExisting = new CheckBox();
      this.gvDownloads = new GridView();
      this.helpLink = new EMHelpLink();
      this.gcDownloads.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(498, 448);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnDownload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDownload.Location = new Point(422, 448);
      this.btnDownload.Name = "btnDownload";
      this.btnDownload.Size = new Size(75, 22);
      this.btnDownload.TabIndex = 2;
      this.btnDownload.Text = "Download";
      this.btnDownload.UseVisualStyleBackColor = true;
      this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
      this.gcDownloads.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcDownloads.Controls.Add((Control) this.chkShowExisting);
      this.gcDownloads.Controls.Add((Control) this.gvDownloads);
      this.gcDownloads.Location = new Point(8, 8);
      this.gcDownloads.Name = "gcDownloads";
      this.gcDownloads.Size = new Size(564, 432);
      this.gcDownloads.TabIndex = 0;
      this.gcDownloads.Text = "Received from Borrower";
      this.chkShowExisting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkShowExisting.AutoSize = true;
      this.chkShowExisting.BackColor = Color.Transparent;
      this.chkShowExisting.Location = new Point(364, 5);
      this.chkShowExisting.Name = "chkShowExisting";
      this.chkShowExisting.Size = new Size(199, 18);
      this.chkShowExisting.TabIndex = 2;
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
      this.gvDownloads.Size = new Size(562, 405);
      this.gvDownloads.TabIndex = 1;
      this.gvDownloads.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDownloads.SubItemCheck += new GVSubItemEventHandler(this.gvDownloads_SubItemCheck);
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.HelpTag = "Retrieve Documents";
      this.helpLink.Location = new Point(8, 450);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 4;
      this.helpLink.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(580, 479);
      this.Controls.Add((Control) this.helpLink);
      this.Controls.Add((Control) this.gcDownloads);
      this.Controls.Add((Control) this.btnDownload);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RetrieveBorrowerDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Retrieve";
      this.KeyDown += new KeyEventHandler(this.RetrieveBorrowerDialog_KeyDown);
      this.gcDownloads.ResumeLayout(false);
      this.gcDownloads.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
