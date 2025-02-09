// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.History.HistoryTrackingControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.History
{
  public class HistoryTrackingControl : UserControl, IRefreshContents
  {
    private const string SHOWUNASSIGNEDFILES = "Unassigned Files";
    private const string SHOWDOCUMENTS = "Documents";
    private const string SHOWPRELIMINARYCONDITIONS = "Preliminary Conditions";
    private const string SHOWUNDERWRITINGCONDITIONS = "Underwriting Conditions";
    private const string SHOWSELLCONDITIONS = "Delivery Conditions";
    private const string SHOWPOSTCLOSINGCONDITIONS = "Post-Closing Conditions";
    private const string SHOWENHANCEDCONDITIONS = "Conditions";
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvObjectsMgr;
    private GridViewDataManager gvHistoryMgr;
    private IContainer components;
    private GroupContainer gcObjects;
    internal GridView gvObjects;
    private ToolStripMenuItem mnuItemExcel2;
    private CollapsibleSplitter csDocTracking;
    private GridView gvHistory;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem mnuItemExcel1;
    private ToolStripSeparator mnuItemSeparator2;
    private ToolStripMenuItem mnuItemSelectAll;
    private GroupContainer gcHistory;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnExcel;
    private ToolTip tooltip;
    private ContextMenuStrip mnuHistory;
    private GradientPanel pnlObjectType;
    private ComboBox cboObjectType;
    private Label lblObjectType;
    private CheckBox chkIncludeRemoved;

    public HistoryTrackingControl(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.initObjectList();
      this.initObjectTypeList();
      this.initHistoryList();
      this.loadObjectList();
    }

    private void initObjectList()
    {
      this.gvObjectsMgr = new GridViewDataManager(this.gvObjects, this.loanDataMgr);
      this.gvObjectsMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameWithIconColumn,
        GridViewDataManager.BorrowerColumn
      });
      this.gvObjects.Columns[1].SpringToFit = true;
      this.gvObjects.Sort(0, SortOrder.Ascending);
    }

    private void initObjectTypeList()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      if (folderAccessRights.CanAccessDocumentTab)
        this.cboObjectType.Items.Add((object) "Documents");
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.cboObjectType.Items.Add((object) "Conditions");
      }
      else
      {
        if (folderAccessRights.CanAccessPreliminaryTab)
          this.cboObjectType.Items.Add((object) "Preliminary Conditions");
        if (folderAccessRights.CanAccessUnderwritingTab)
          this.cboObjectType.Items.Add((object) "Underwriting Conditions");
        if (folderAccessRights.CanAccessPostClosingTab)
          this.cboObjectType.Items.Add((object) "Post-Closing Conditions");
        if (folderAccessRights.CanAccessSellTab)
          this.cboObjectType.Items.Add((object) "Delivery Conditions");
      }
      if (folderAccessRights.CanAccessDocumentTab)
        this.cboObjectType.Items.Add((object) "Unassigned Files");
      if (this.cboObjectType.Items.Count <= 0)
        return;
      this.cboObjectType.SelectedIndex = 0;
    }

    private void loadObjectList()
    {
      bool includeRemoved = this.chkIncludeRemoved.Checked;
      switch ((string) this.cboObjectType.SelectedItem)
      {
        case "Conditions":
          this.loadConditionObjects(ConditionType.Enhanced, includeRemoved);
          break;
        case "Delivery Conditions":
          this.loadConditionObjects(ConditionType.Sell, includeRemoved);
          break;
        case "Documents":
          this.loadDocumentObjects(includeRemoved);
          break;
        case "Post-Closing Conditions":
          this.loadConditionObjects(ConditionType.PostClosing, includeRemoved);
          break;
        case "Preliminary Conditions":
          this.loadConditionObjects(ConditionType.Preliminary, includeRemoved);
          break;
        case "Unassigned Files":
          this.loadUnassignedFileObjects(includeRemoved);
          break;
        case "Underwriting Conditions":
          this.loadConditionObjects(ConditionType.Underwriting, includeRemoved);
          break;
      }
      this.gvObjects.ReSort();
      this.gcObjects.Text = "Documents / Files / Conditions (" + this.gvObjects.Items.Count.ToString() + ")";
      this.loadHistoryList();
    }

    private void loadUnassignedFileObjects(bool includeRemoved)
    {
      if (this.gvObjects.ItemGrouping)
        this.gvObjects.ItemGrouping = false;
      this.addFileItems(this.gvObjects.Items, this.loanDataMgr.FileAttachments.GetUnassignedAttachments(includeRemoved));
    }

    private void loadDocumentObjects(bool includeRemoved)
    {
      if (!this.gvObjects.ItemGrouping)
        this.gvObjects.ItemGrouping = true;
      this.addDocumentItems(this.gvObjects.Items, this.loanDataMgr.LoanData.GetLogList().GetAllDocuments(true, includeRemoved));
    }

    private void loadConditionObjects(ConditionType condType, bool includeRemoved)
    {
      if (!this.gvObjects.ItemGrouping)
        this.gvObjects.ItemGrouping = true;
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      this.addConditionItems(this.gvObjects.Items, logList.GetAllConditions(condType, true, includeRemoved), logList.GetAllDocuments());
    }

    private void addConditionItems(
      GVItemCollection itemList,
      ConditionLog[] condList,
      DocumentLog[] docList)
    {
      foreach (ConditionLog cond in condList)
      {
        GVItem itemByTag = itemList.GetItemByTag((object) cond);
        if (itemByTag == null)
        {
          itemByTag = this.gvObjectsMgr.CreateItem(cond, docList);
          itemByTag.State = GVItemState.Collapsed;
          itemList.Add(itemByTag);
        }
        else
          this.gvObjectsMgr.RefreshItem(itemByTag, cond, docList);
        List<DocumentLog> documentLogList = new List<DocumentLog>();
        foreach (DocumentLog doc in docList)
        {
          if (doc.Conditions.Contains(cond))
            documentLogList.Add(doc);
        }
        this.addDocumentItems(itemByTag.GroupItems, documentLogList.ToArray());
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) itemList)
      {
        if (Array.IndexOf<object>((object[]) condList, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        itemList.Remove(gvItem);
    }

    private void addDocumentItems(GVItemCollection itemList, DocumentLog[] docList)
    {
      foreach (DocumentLog doc in docList)
      {
        GVItem itemByTag = itemList.GetItemByTag((object) doc);
        if (itemByTag == null)
        {
          itemByTag = this.gvObjectsMgr.CreateItem(doc);
          itemByTag.GroupItems.DisableSort = true;
          itemByTag.State = GVItemState.Collapsed;
          itemList.Add(itemByTag);
        }
        else
          this.gvObjectsMgr.RefreshItem(itemByTag, doc);
        FileAttachment[] attachments = this.loanDataMgr.FileAttachments.GetAttachments(doc, false);
        this.addFileItems(itemByTag.GroupItems, attachments);
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) itemList)
      {
        if (Array.IndexOf<object>((object[]) docList, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        itemList.Remove(gvItem);
    }

    private void addFileItems(GVItemCollection itemList, FileAttachment[] fileList)
    {
      if (itemList.DisableSort)
      {
        FileAttachment fileAttachment = (FileAttachment) null;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) itemList)
        {
          if (gvItem.Selected)
            fileAttachment = gvItem.Tag as FileAttachment;
        }
        itemList.Clear();
        foreach (FileAttachment file in fileList)
        {
          GVItem gvItem = this.gvObjectsMgr.CreateItem(file, (FileAttachmentReference) null);
          itemList.Add(gvItem);
          if (file == fileAttachment)
            gvItem.Selected = true;
        }
      }
      else
      {
        foreach (FileAttachment file in fileList)
        {
          GVItem itemByTag = itemList.GetItemByTag((object) file);
          if (itemByTag == null)
          {
            GVItem gvItem = this.gvObjectsMgr.CreateItem(file, (FileAttachmentReference) null);
            itemList.Add(gvItem);
          }
          else
            this.gvObjectsMgr.RefreshItem(itemByTag, file, (FileAttachmentReference) null);
        }
        List<GVItem> gvItemList = new List<GVItem>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) itemList)
        {
          if (Array.IndexOf<object>((object[]) fileList, gvItem.Tag) < 0)
            gvItemList.Add(gvItem);
        }
        foreach (GVItem gvItem in gvItemList)
          itemList.Remove(gvItem);
      }
    }

    private void cboObjectType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.loadObjectList();
    }

    private void chkIncludeRemoved_CheckedChanged(object sender, EventArgs e)
    {
      this.loadObjectList();
    }

    private void gvObjects_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.loadHistoryList();
    }

    private void initHistoryList()
    {
      this.gvHistoryMgr = new GridViewDataManager(this.gvHistory, this.loanDataMgr);
      this.gvHistoryMgr.FilterChanged += new EventHandler(this.gvHistoryMgr_FilterChanged);
      this.gvHistoryMgr.CreateLayout(new TableLayout.Column[6]
      {
        GridViewDataManager.DateTimeColumn,
        GridViewDataManager.ObjectTypeColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.EventColumn,
        GridViewDataManager.UserColumn
      });
      this.gvHistory.Sort(0, SortOrder.Ascending);
    }

    private void loadHistoryList()
    {
      LoanHistoryEntry[] array = new LoanHistoryEntry[0];
      if (this.gvObjects.SelectedItems.Count > 0)
      {
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.gvObjects.SelectedItems)
        {
          string[] objectList = this.getObjectList(selectedItem);
          stringList.AddRange((IEnumerable<string>) objectList);
        }
        array = this.loanDataMgr.LoanHistory.GetHistory(stringList.ToArray());
      }
      foreach (LoanHistoryEntry entry in array)
      {
        GVItem itemByTag = this.gvHistory.Items.GetItemByTag((object) entry);
        if (itemByTag == null)
          this.gvHistoryMgr.AddItem(entry);
        else
          this.gvHistoryMgr.RefreshItem(itemByTag, entry);
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvHistory.Items)
      {
        if (Array.IndexOf<object>((object[]) array, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvHistory.Items.Remove(gvItem);
      this.gvHistoryMgr.ApplyFilter();
    }

    private string[] getObjectList(GVItem item)
    {
      List<string> stringList = new List<string>();
      if (item.Tag is FileAttachment)
      {
        string[] objectList = this.getObjectList((FileAttachment) item.Tag);
        stringList.AddRange((IEnumerable<string>) objectList);
      }
      else if (item.Tag is LogRecordBase)
      {
        LogRecordBase tag = (LogRecordBase) item.Tag;
        stringList.Add(tag.Guid);
      }
      foreach (GVItem groupItem in (IEnumerable<GVItem>) item.GroupItems)
      {
        string[] objectList = this.getObjectList(groupItem);
        stringList.AddRange((IEnumerable<string>) objectList);
      }
      return stringList.ToArray();
    }

    private string[] getObjectList(FileAttachment file)
    {
      List<string> stringList = new List<string>();
      stringList.Add(file.ID);
      if (file is ImageAttachment)
      {
        foreach (PageImage page in ((ImageAttachment) file).Pages)
          stringList.Add(page.ImageKey);
      }
      foreach (string source in file.Sources)
      {
        string[] objectList = this.getObjectList(this.loanDataMgr.FileAttachments[source, false, true]);
        stringList.AddRange((IEnumerable<string>) objectList);
      }
      return stringList.ToArray();
    }

    private void gvHistoryMgr_FilterChanged(object sender, EventArgs e)
    {
      this.gcHistory.Text = "History (" + this.gvHistory.VisibleItems.Count.ToString() + ")";
    }

    public ToolStripDropDown Menu => (ToolStripDropDown) this.mnuHistory;

    private void btnExcel_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = this.gvHistory.SelectedItems.Count <= 0 ? new List<GVItem>((IEnumerable<GVItem>) this.gvHistory.VisibleItems) : new List<GVItem>((IEnumerable<GVItem>) this.gvHistory.SelectedItems);
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn gvColumn in this.gvHistory.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(gvColumn.Text);
        foreach (GVItem gvItem in gvItemList)
        {
          string[] data = new string[this.gvHistory.Columns.Count];
          for (int index1 = 0; index1 < data.Length; ++index1)
          {
            int index2 = this.gvHistory.Columns.DisplaySequence[index1].Index;
            data[index1] = gvItem.SubItems[index2].Text;
          }
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private void mnuItemSelectAll_Click(object sender, EventArgs e)
    {
      this.gvHistory.Items.SelectAll();
    }

    public void RefreshContents() => this.loadObjectList();

    public void RefreshLoanContents() => this.loadObjectList();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.gcObjects = new GroupContainer();
      this.gvObjects = new GridView();
      this.pnlObjectType = new GradientPanel();
      this.chkIncludeRemoved = new CheckBox();
      this.cboObjectType = new ComboBox();
      this.lblObjectType = new Label();
      this.mnuItemExcel2 = new ToolStripMenuItem();
      this.csDocTracking = new CollapsibleSplitter();
      this.gvHistory = new GridView();
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuItemExcel1 = new ToolStripMenuItem();
      this.mnuItemSeparator2 = new ToolStripSeparator();
      this.mnuItemSelectAll = new ToolStripMenuItem();
      this.gcHistory = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnExcel = new StandardIconButton();
      this.mnuHistory = new ContextMenuStrip(this.components);
      this.tooltip = new ToolTip(this.components);
      this.gcObjects.SuspendLayout();
      this.pnlObjectType.SuspendLayout();
      this.mnuContext.SuspendLayout();
      this.gcHistory.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnExcel).BeginInit();
      this.mnuHistory.SuspendLayout();
      this.SuspendLayout();
      this.gcObjects.Controls.Add((Control) this.gvObjects);
      this.gcObjects.Controls.Add((Control) this.pnlObjectType);
      this.gcObjects.Dock = DockStyle.Left;
      this.gcObjects.HeaderForeColor = SystemColors.ControlText;
      this.gcObjects.Location = new Point(0, 0);
      this.gcObjects.Name = "gcObjects";
      this.gcObjects.Size = new Size(364, 452);
      this.gcObjects.TabIndex = 0;
      this.gcObjects.Text = "Documents / Files / Conditions";
      this.gvObjects.BorderStyle = BorderStyle.None;
      this.gvObjects.ClearSelectionsOnEmptyRowClick = false;
      this.gvObjects.Dock = DockStyle.Fill;
      this.gvObjects.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvObjects.Location = new Point(1, 57);
      this.gvObjects.Name = "gvObjects";
      this.gvObjects.Size = new Size(362, 394);
      this.gvObjects.TabIndex = 1;
      this.gvObjects.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvObjects.SelectedIndexCommitted += new EventHandler(this.gvObjects_SelectedIndexCommitted);
      this.pnlObjectType.Borders = AnchorStyles.Bottom;
      this.pnlObjectType.Controls.Add((Control) this.chkIncludeRemoved);
      this.pnlObjectType.Controls.Add((Control) this.cboObjectType);
      this.pnlObjectType.Controls.Add((Control) this.lblObjectType);
      this.pnlObjectType.Dock = DockStyle.Top;
      this.pnlObjectType.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlObjectType.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlObjectType.Location = new Point(1, 26);
      this.pnlObjectType.Name = "pnlObjectType";
      this.pnlObjectType.Padding = new Padding(8, 0, 0, 0);
      this.pnlObjectType.Size = new Size(362, 31);
      this.pnlObjectType.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlObjectType.TabIndex = 0;
      this.chkIncludeRemoved.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkIncludeRemoved.AutoSize = true;
      this.chkIncludeRemoved.BackColor = Color.Transparent;
      this.chkIncludeRemoved.Location = new Point(260, 7);
      this.chkIncludeRemoved.Name = "chkIncludeRemoved";
      this.chkIncludeRemoved.Size = new Size(98, 18);
      this.chkIncludeRemoved.TabIndex = 2;
      this.chkIncludeRemoved.Text = "Include deleted";
      this.chkIncludeRemoved.UseVisualStyleBackColor = false;
      this.chkIncludeRemoved.CheckedChanged += new EventHandler(this.chkIncludeRemoved_CheckedChanged);
      this.cboObjectType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboObjectType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboObjectType.FormattingEnabled = true;
      this.cboObjectType.Location = new Point(48, 4);
      this.cboObjectType.Name = "cboObjectType";
      this.cboObjectType.Size = new Size(204, 22);
      this.cboObjectType.TabIndex = 1;
      this.cboObjectType.TabStop = false;
      this.cboObjectType.SelectionChangeCommitted += new EventHandler(this.cboObjectType_SelectionChangeCommitted);
      this.lblObjectType.AutoSize = true;
      this.lblObjectType.BackColor = Color.Transparent;
      this.lblObjectType.Location = new Point(8, 8);
      this.lblObjectType.Name = "lblObjectType";
      this.lblObjectType.Size = new Size(36, 14);
      this.lblObjectType.TabIndex = 0;
      this.lblObjectType.Text = "Show";
      this.lblObjectType.TextAlign = ContentAlignment.MiddleLeft;
      this.mnuItemExcel2.Image = (Image) Resources.excel;
      this.mnuItemExcel2.Name = "mnuItemExcel2";
      this.mnuItemExcel2.Size = new Size(159, 22);
      this.mnuItemExcel2.Text = "E&xport to Excel...";
      this.mnuItemExcel2.Click += new EventHandler(this.btnExcel_Click);
      this.csDocTracking.AnimationDelay = 20;
      this.csDocTracking.AnimationStep = 20;
      this.csDocTracking.BorderStyle3D = Border3DStyle.Flat;
      this.csDocTracking.ControlToHide = (Control) this.gcObjects;
      this.csDocTracking.ExpandParentForm = false;
      this.csDocTracking.Location = new Point(364, 0);
      this.csDocTracking.Name = "csDocTracking";
      this.csDocTracking.TabIndex = 1;
      this.csDocTracking.TabStop = false;
      this.csDocTracking.UseAnimations = false;
      this.csDocTracking.VisualStyle = VisualStyles.Encompass;
      this.gvHistory.AllowColumnReorder = true;
      this.gvHistory.BorderStyle = BorderStyle.None;
      this.gvHistory.ContextMenuStrip = this.mnuContext;
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.FilterVisible = true;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(336, 425);
      this.gvHistory.TabIndex = 1;
      this.gvHistory.TextTrimming = StringTrimming.EllipsisCharacter;
      this.mnuContext.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.mnuItemExcel1,
        (ToolStripItem) this.mnuItemSeparator2,
        (ToolStripItem) this.mnuItemSelectAll
      });
      this.mnuContext.Name = "mnuDocuments";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(169, 54);
      this.mnuItemExcel1.Name = "mnuItemExcel1";
      this.mnuItemExcel1.Size = new Size(168, 22);
      this.mnuItemExcel1.Text = "Export to Excel...";
      this.mnuItemExcel1.Click += new EventHandler(this.btnExcel_Click);
      this.mnuItemSeparator2.Name = "mnuItemSeparator2";
      this.mnuItemSeparator2.Size = new Size(165, 6);
      this.mnuItemSelectAll.Name = "mnuItemSelectAll";
      this.mnuItemSelectAll.Size = new Size(168, 22);
      this.mnuItemSelectAll.Text = "Select All on This Page";
      this.mnuItemSelectAll.Click += new EventHandler(this.mnuItemSelectAll_Click);
      this.gcHistory.Controls.Add((Control) this.gvHistory);
      this.gcHistory.Controls.Add((Control) this.pnlToolbar);
      this.gcHistory.Dock = DockStyle.Fill;
      this.gcHistory.HeaderForeColor = SystemColors.ControlText;
      this.gcHistory.Location = new Point(371, 0);
      this.gcHistory.Name = "gcHistory";
      this.gcHistory.Size = new Size(338, 452);
      this.gcHistory.TabIndex = 2;
      this.gcHistory.Text = "History";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnExcel);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(205, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(129, 22);
      this.pnlToolbar.TabIndex = 0;
      this.btnExcel.BackColor = Color.Transparent;
      this.btnExcel.Location = new Point(113, 3);
      this.btnExcel.Margin = new Padding(4, 3, 0, 3);
      this.btnExcel.MouseDownImage = (Image) null;
      this.btnExcel.Name = "btnExcel";
      this.btnExcel.Size = new Size(16, 17);
      this.btnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExcel.TabIndex = 5;
      this.btnExcel.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnExcel, "Export to Excel");
      this.btnExcel.Click += new EventHandler(this.btnExcel_Click);
      this.mnuHistory.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.mnuItemExcel2
      });
      this.mnuHistory.Name = "mnuDocuments";
      this.mnuHistory.Size = new Size(160, 26);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcHistory);
      this.Controls.Add((Control) this.csDocTracking);
      this.Controls.Add((Control) this.gcObjects);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (HistoryTrackingControl);
      this.Size = new Size(709, 452);
      this.gcObjects.ResumeLayout(false);
      this.pnlObjectType.ResumeLayout(false);
      this.pnlObjectType.PerformLayout();
      this.mnuContext.ResumeLayout(false);
      this.gcHistory.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnExcel).EndInit();
      this.mnuHistory.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
