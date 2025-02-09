// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.SplitAssignedFileDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class SplitAssignedFileDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private NativeAttachment originalFile;
    private string[] pdfPageList;
    private List<FileAttachment> fileList = new List<FileAttachment>();
    private List<string> changeList = new List<string>();
    private Point dragPoint = Point.Empty;
    private SDCDocument sdcDocumentCopy;
    private SDCHelper sdcHelper;
    private const string className = "SplitAssignedFileDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private IContainer components;
    private Button btnCancel;
    private ToolTip tooltip;
    private CollapsibleSplitter csLeft;
    private Panel pnlFooter;
    private BorderPanel pnlRight;
    private PageViewerControl pageViewer;
    private Button btnSplit;
    private Panel pnlLeft;
    private GroupContainer gcNew;
    private GridView gvNew;
    private CollapsibleSplitter csOriginal;
    private GroupContainer gcOriginal;
    private GridView gvOriginal;
    private BorderPanel pnlDragDrop;
    private Label lblDragDrop;
    private FlowLayoutPanel pnlToolbar2;
    private StandardIconButton btnDeleteNew;
    private StandardIconButton btnMoveNewDown;
    private StandardIconButton btnMoveNewUp;
    private FlowLayoutPanel pnlToolbar1;
    private StandardIconButton btnDeleteOriginal;
    private StandardIconButton btnMoveOriginalDown;
    private StandardIconButton btnMoveOriginalUp;

    public SplitAssignedFileDialog(
      LoanDataMgr loanDataMgr,
      NativeAttachment file,
      string[] pdfPageList)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.originalFile = file;
      this.pdfPageList = pdfPageList;
      this.applySecurity();
      this.loadOriginalList();
      if (!this.loanDataMgr.UseSkyDriveClassic)
        return;
      this.sdcHelper = new SDCHelper(loanDataMgr);
      this.sdcDocumentCopy = file.CurrentSDCDocument == null ? Utils.DeepClone<SDCDocument>(this.originalFile.OriginalSDCDocument) : Utils.DeepClone<SDCDocument>(this.originalFile.CurrentSDCDocument);
    }

    public FileAttachment[] Files => this.fileList.ToArray();

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

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) this.loanDataMgr.FileAttachments.GetLinkedDocument(this.originalFile.ID));
      this.btnDeleteOriginal.Visible = folderAccessRights.CanDeletePageFromFile;
      this.btnDeleteNew.Visible = folderAccessRights.CanDeletePageFromFile;
    }

    private void loadOriginalList()
    {
      for (int index = 0; index < this.pdfPageList.Length; ++index)
        this.gvOriginal.Items.Add(new GVItem()
        {
          Text = "Page " + Convert.ToString(index + 1),
          Tag = (object) this.pdfPageList[index],
          Selected = true
        });
      this.showOriginalPages();
    }

    private void gvOriginal_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count1 = this.gvOriginal.Items.Count;
      int count2 = this.gvOriginal.SelectedItems.Count;
      this.btnMoveOriginalUp.Enabled = count2 > 0 && count1 > count2;
      this.btnMoveOriginalDown.Enabled = count2 > 0 && count1 > count2;
      this.btnDeleteOriginal.Enabled = count2 > 0;
    }

    private void gvOriginal_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showOriginalPages();
    }

    private void showOriginalPages()
    {
      this.gvNew.SelectedItems.Clear();
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvOriginal.SelectedItems)
        stringList.Add((string) selectedItem.Tag);
      if (stringList.Count > 0)
        this.pageViewer.LoadFiles(stringList.ToArray());
      else
        this.pageViewer.CloseFiles();
    }

    private void btnMoveOriginalUp_Click(object sender, EventArgs e)
    {
      GVItem gvItem1 = (GVItem) null;
      for (int nItemIndex = 0; nItemIndex < this.gvOriginal.Items.Count; ++nItemIndex)
      {
        GVItem gvItem2 = this.gvOriginal.Items[nItemIndex];
        if (!gvItem2.Selected)
          gvItem1 = gvItem2;
        else if (gvItem1 != null)
        {
          this.gvOriginal.Items.Remove(gvItem2);
          if (this.loanDataMgr.UseSkyDriveClassic)
          {
            Tracing.Log(SplitAssignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitAssignedFileDialog), "Calling UpdatePageOrder.");
            this.sdcHelper.UpdatePageOrder(this.sdcDocumentCopy, gvItem2.Index, gvItem1.DisplayIndex);
          }
          this.gvOriginal.Items.Insert(gvItem1.DisplayIndex, gvItem2);
          gvItem2.Selected = true;
        }
      }
    }

    private void btnMoveOriginalDown_Click(object sender, EventArgs e)
    {
      GVItem gvItem1 = (GVItem) null;
      for (int nItemIndex = this.gvOriginal.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        GVItem gvItem2 = this.gvOriginal.Items[nItemIndex];
        if (!gvItem2.Selected)
          gvItem1 = gvItem2;
        else if (gvItem1 != null)
        {
          this.gvOriginal.Items.Remove(gvItem2);
          if (this.loanDataMgr.UseSkyDriveClassic)
          {
            Tracing.Log(SplitAssignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitAssignedFileDialog), "Calling UpdatePageOrder.");
            this.sdcHelper.UpdatePageOrder(this.sdcDocumentCopy, gvItem2.Index, gvItem1.DisplayIndex + 1);
          }
          this.gvOriginal.Items.Insert(gvItem1.DisplayIndex + 1, gvItem2);
          gvItem2.Selected = true;
        }
      }
    }

    private void btnDeleteOriginal_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gvOriginal.SelectedItems)
        gvItemList.Add(selectedItem);
      List<int> intList = new List<int>();
      foreach (GVItem gvItem in gvItemList)
      {
        if (this.loanDataMgr.UseSkyDriveClassic)
        {
          int id = this.sdcDocumentCopy.Pages[gvItem.Index].Id;
          intList.Add(id);
        }
        this.gvOriginal.Items.Remove(gvItem);
        this.changeList.Add(gvItem.Text + " deleted");
      }
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        Tracing.Log(SplitAssignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitAssignedFileDialog), "Calling DeletePages for deleting or removing pageIDs-(" + string.Join<int>(",", (IEnumerable<int>) intList) + ").");
        this.sdcHelper.DeletePages(this.sdcDocumentCopy, intList);
      }
      this.showOriginalPages();
    }

    private void gvNew_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count1 = this.gvNew.Items.Count;
      int count2 = this.gvNew.SelectedItems.Count;
      this.btnMoveNewUp.Enabled = count2 > 0 && count1 > count2;
      this.btnMoveNewDown.Enabled = count2 > 0 && count1 > count2;
      this.btnDeleteNew.Enabled = count2 > 0;
    }

    private void gvNew_SelectedIndexCommitted(object sender, EventArgs e) => this.showNewPages();

    private void showNewPages()
    {
      this.gvOriginal.SelectedItems.Clear();
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvNew.SelectedItems)
        stringList.Add((string) selectedItem.Tag);
      if (stringList.Count > 0)
        this.pageViewer.LoadFiles(stringList.ToArray());
      else
        this.pageViewer.CloseFiles();
    }

    private void btnMoveNewUp_Click(object sender, EventArgs e)
    {
      GVItem gvItem1 = (GVItem) null;
      for (int nItemIndex = 0; nItemIndex < this.gvNew.Items.Count; ++nItemIndex)
      {
        GVItem gvItem2 = this.gvNew.Items[nItemIndex];
        if (!gvItem2.Selected)
          gvItem1 = gvItem2;
        else if (gvItem1 != null)
        {
          this.gvNew.Items.Remove(gvItem2);
          this.gvNew.Items.Insert(gvItem1.DisplayIndex, gvItem2);
          gvItem2.Selected = true;
        }
      }
    }

    private void btnMoveNewDown_Click(object sender, EventArgs e)
    {
      GVItem gvItem1 = (GVItem) null;
      for (int nItemIndex = this.gvNew.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        GVItem gvItem2 = this.gvNew.Items[nItemIndex];
        if (!gvItem2.Selected)
          gvItem1 = gvItem2;
        else if (gvItem1 != null)
        {
          this.gvNew.Items.Remove(gvItem2);
          this.gvNew.Items.Insert(gvItem1.DisplayIndex + 1, gvItem2);
          gvItem2.Selected = true;
        }
      }
    }

    private void btnDeleteNew_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gvNew.SelectedItems)
        gvItemList.Add(selectedItem);
      foreach (GVItem gvItem in gvItemList)
      {
        this.gvNew.Items.Remove(gvItem);
        this.changeList.Add(gvItem.Text + " deleted");
      }
      this.showNewPages();
    }

    private void gvOriginal_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.gvOriginal.GetItemAt(e.X, e.Y) == null)
        return;
      this.dragPoint = e.Location;
    }

    private void gvOriginal_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      int num = (int) this.gvOriginal.DoDragDrop((object) this.gvOriginal.SelectedItems, DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvOriginal_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvOriginal_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.None;
      if (!e.Data.GetDataPresent(typeof (GVSelectedItemCollection)) || ((GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection)))[0].GridView != this.gvNew)
        return;
      e.Effect = DragDropEffects.Move;
    }

    private void gvOriginal_DragDrop(object sender, DragEventArgs e)
    {
      foreach (GVItem gvItem in (GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection)))
      {
        this.gvNew.Items.Remove(gvItem);
        this.gvOriginal.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.showOriginalPages();
    }

    private void gvNew_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.gvNew.GetItemAt(e.X, e.Y) == null)
        return;
      this.dragPoint = e.Location;
    }

    private void gvNew_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      int num = (int) this.gvNew.DoDragDrop((object) this.gvNew.SelectedItems, DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvNew_MouseUp(object sender, MouseEventArgs e) => this.dragPoint = Point.Empty;

    private void gvNew_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.None;
      if (!e.Data.GetDataPresent(typeof (GVSelectedItemCollection)) || ((GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection)))[0].GridView != this.gvOriginal)
        return;
      e.Effect = DragDropEffects.Move;
    }

    private void gvNew_DragDrop(object sender, DragEventArgs e)
    {
      foreach (GVItem gvItem in (GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection)))
      {
        this.gvOriginal.Items.Remove(gvItem);
        this.gvNew.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.showNewPages();
    }

    private void SplitFileDialog_Resize(object sender, EventArgs e)
    {
      this.gcOriginal.Height = (this.pnlLeft.ClientSize.Height - this.csOriginal.Height) / 2;
    }

    private void btnSplit_Click(object sender, EventArgs e)
    {
      List<string> stringList1 = new List<string>();
      Dictionary<string, List<int>> pageIndexTable = (Dictionary<string, List<int>>) null;
      Dictionary<string, List<string>> dictionary = (Dictionary<string, List<string>>) null;
      List<int> displayIndices1 = new List<int>();
      List<string> stringList2 = new List<string>();
      List<string> stringList3 = (List<string>) null;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvOriginal.Items)
      {
        stringList1.Add(gvItem.Tag as string);
        if (this.loanDataMgr.UseSkyDriveClassic)
        {
          int num = int.Parse(gvItem.Text.Split(' ')[1]);
          displayIndices1.Add(num);
          stringList2.Add(gvItem.Tag as string);
        }
      }
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        Tracing.Log(SplitAssignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitAssignedFileDialog), "Calling GetPageIndexListForSplit.");
        List<int> indexListForSplit = this.sdcHelper.GetPageIndexListForSplit(this.originalFile, displayIndices1);
        pageIndexTable = new Dictionary<string, List<int>>();
        dictionary = new Dictionary<string, List<string>>();
        pageIndexTable.Add("SourceFile", indexListForSplit);
        dictionary.Add("SourceFile", stringList2);
      }
      List<string> stringList4 = new List<string>();
      List<string> stringList5 = new List<string>();
      List<int> displayIndices2 = new List<int>();
      int num1 = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvNew.Items)
      {
        stringList4.Add(gvItem.Tag as string);
        if (this.loanDataMgr.UseSkyDriveClassic)
        {
          int num2 = int.Parse(gvItem.Text.Split(' ')[1]);
          displayIndices2.Add(num2);
          stringList5.Add(gvItem.Tag as string);
        }
      }
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        List<int> indexListForSplit = this.sdcHelper.GetPageIndexListForSplit(this.originalFile, displayIndices2);
        pageIndexTable = pageIndexTable == null ? new Dictionary<string, List<int>>() : pageIndexTable;
        dictionary = dictionary == null ? new Dictionary<string, List<string>>() : dictionary;
        int num3 = num1 + 1;
        pageIndexTable.Add(string.Format("NewFile-{0}", (object) num3), indexListForSplit);
        dictionary.Add(string.Format("NewFile-{0}", (object) num3), stringList5);
      }
      if (stringList1.Count == 0 || stringList4.Count == 0)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "In order to split the file, you must have pages in both the original file and in the new file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.loanDataMgr.UseSkyDriveClassic)
        {
          List<string> stringList6 = new List<string>();
          SDCHelper sdcHelper = new SDCHelper(this.loanDataMgr);
          stringList3 = new List<string>();
          string str1 = string.Empty;
          this.originalFile.CurrentSDCDocument = this.sdcDocumentCopy;
          foreach (string key in pageIndexTable.Keys)
          {
            List<int> intList = pageIndexTable[key];
            List<string> stringList7 = dictionary[key];
            string empty;
            for (int index = 0; index < intList.Count; ++index)
            {
              string str2 = sdcHelper.RemoveEditsFromPage(this.originalFile, intList[index], stringList7[index]);
              stringList6.Add(str2);
              empty = string.Empty;
            }
            using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
            {
              str1 = pdfPageBuilder.MergeFiles(stringList6.ToArray());
              if (str1 == null)
                return;
              stringList6 = new List<string>();
            }
            stringList3.Add(str1);
            empty = string.Empty;
          }
        }
        long ticks = DateTime.Now.Ticks;
        string filepath1 = (string) null;
        using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
        {
          filepath1 = pdfPageBuilder.MergeFiles(stringList1.ToArray());
          if (filepath1 == null)
            return;
        }
        string filepath2 = (string) null;
        using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
        {
          filepath2 = pdfPageBuilder.MergeFiles(stringList4.ToArray());
          if (filepath2 == null)
            return;
        }
        if (!this.loanDataMgr.LockLoanWithExclusiveA())
          return;
        using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
        {
          string[] array = stringList3 == null ? (string[]) null : stringList3.ToArray();
          FileAttachment[] collection = attachFilesDialog.SplitFile(this.originalFile, filepath1, filepath2, this.loanDataMgr.FileAttachments.GetLinkedDocument(this.originalFile.ID), pageIndexTable, array);
          if (collection == null)
            return;
          this.fileList.AddRange((IEnumerable<FileAttachment>) collection);
        }
        foreach (string change in this.changeList)
          this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) this.originalFile, change);
        RemoteLogger.Write(TraceLevel.Info, "Split NativeAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, 2 files");
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SplitAssignedFileDialog));
      this.btnCancel = new Button();
      this.tooltip = new ToolTip(this.components);
      this.btnDeleteOriginal = new StandardIconButton();
      this.btnMoveOriginalDown = new StandardIconButton();
      this.btnMoveOriginalUp = new StandardIconButton();
      this.btnDeleteNew = new StandardIconButton();
      this.btnMoveNewDown = new StandardIconButton();
      this.btnMoveNewUp = new StandardIconButton();
      this.csLeft = new CollapsibleSplitter();
      this.pnlLeft = new Panel();
      this.gcNew = new GroupContainer();
      this.pnlToolbar2 = new FlowLayoutPanel();
      this.gvNew = new GridView();
      this.csOriginal = new CollapsibleSplitter();
      this.gcOriginal = new GroupContainer();
      this.pnlToolbar1 = new FlowLayoutPanel();
      this.gvOriginal = new GridView();
      this.pnlDragDrop = new BorderPanel();
      this.lblDragDrop = new Label();
      this.pnlFooter = new Panel();
      this.btnSplit = new Button();
      this.pnlRight = new BorderPanel();
      this.pageViewer = new PageViewerControl();
      ((ISupportInitialize) this.btnDeleteOriginal).BeginInit();
      ((ISupportInitialize) this.btnMoveOriginalDown).BeginInit();
      ((ISupportInitialize) this.btnMoveOriginalUp).BeginInit();
      ((ISupportInitialize) this.btnDeleteNew).BeginInit();
      ((ISupportInitialize) this.btnMoveNewDown).BeginInit();
      ((ISupportInitialize) this.btnMoveNewUp).BeginInit();
      this.pnlLeft.SuspendLayout();
      this.gcNew.SuspendLayout();
      this.pnlToolbar2.SuspendLayout();
      this.gcOriginal.SuspendLayout();
      this.pnlToolbar1.SuspendLayout();
      this.pnlDragDrop.SuspendLayout();
      this.pnlFooter.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(693, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 15;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnDeleteOriginal.BackColor = Color.Transparent;
      this.btnDeleteOriginal.Enabled = false;
      this.btnDeleteOriginal.Location = new Point(68, 3);
      this.btnDeleteOriginal.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteOriginal.Name = "btnDeleteOriginal";
      this.btnDeleteOriginal.Size = new Size(16, 16);
      this.btnDeleteOriginal.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteOriginal.TabIndex = 28;
      this.btnDeleteOriginal.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteOriginal, "Remove Page");
      this.btnDeleteOriginal.Click += new EventHandler(this.btnDeleteOriginal_Click);
      this.btnMoveOriginalDown.BackColor = Color.Transparent;
      this.btnMoveOriginalDown.Enabled = false;
      this.btnMoveOriginalDown.Location = new Point(48, 3);
      this.btnMoveOriginalDown.Margin = new Padding(4, 3, 0, 3);
      this.btnMoveOriginalDown.Name = "btnMoveOriginalDown";
      this.btnMoveOriginalDown.Size = new Size(16, 16);
      this.btnMoveOriginalDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveOriginalDown.TabIndex = 27;
      this.btnMoveOriginalDown.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveOriginalDown, "Move Page Down");
      this.btnMoveOriginalDown.Click += new EventHandler(this.btnMoveOriginalDown_Click);
      this.btnMoveOriginalUp.BackColor = Color.Transparent;
      this.btnMoveOriginalUp.Enabled = false;
      this.btnMoveOriginalUp.Location = new Point(28, 3);
      this.btnMoveOriginalUp.Margin = new Padding(4, 3, 0, 3);
      this.btnMoveOriginalUp.Name = "btnMoveOriginalUp";
      this.btnMoveOriginalUp.Size = new Size(16, 16);
      this.btnMoveOriginalUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveOriginalUp.TabIndex = 26;
      this.btnMoveOriginalUp.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveOriginalUp, "Move Page Up");
      this.btnMoveOriginalUp.Click += new EventHandler(this.btnMoveOriginalUp_Click);
      this.btnDeleteNew.BackColor = Color.Transparent;
      this.btnDeleteNew.Enabled = false;
      this.btnDeleteNew.Location = new Point(68, 3);
      this.btnDeleteNew.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteNew.Name = "btnDeleteNew";
      this.btnDeleteNew.Size = new Size(16, 16);
      this.btnDeleteNew.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteNew.TabIndex = 28;
      this.btnDeleteNew.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteNew, "Remove Page");
      this.btnDeleteNew.Click += new EventHandler(this.btnDeleteNew_Click);
      this.btnMoveNewDown.BackColor = Color.Transparent;
      this.btnMoveNewDown.Enabled = false;
      this.btnMoveNewDown.Location = new Point(48, 3);
      this.btnMoveNewDown.Margin = new Padding(4, 3, 0, 3);
      this.btnMoveNewDown.Name = "btnMoveNewDown";
      this.btnMoveNewDown.Size = new Size(16, 16);
      this.btnMoveNewDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveNewDown.TabIndex = 27;
      this.btnMoveNewDown.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveNewDown, "Move Page Down");
      this.btnMoveNewDown.Click += new EventHandler(this.btnMoveNewDown_Click);
      this.btnMoveNewUp.BackColor = Color.Transparent;
      this.btnMoveNewUp.Enabled = false;
      this.btnMoveNewUp.Location = new Point(28, 3);
      this.btnMoveNewUp.Margin = new Padding(4, 3, 0, 3);
      this.btnMoveNewUp.Name = "btnMoveNewUp";
      this.btnMoveNewUp.Size = new Size(16, 16);
      this.btnMoveNewUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveNewUp.TabIndex = 26;
      this.btnMoveNewUp.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveNewUp, "Move Page Up");
      this.btnMoveNewUp.Click += new EventHandler(this.btnMoveNewUp_Click);
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(356, 0);
      this.csLeft.Name = "csLeft";
      this.csLeft.TabIndex = 10;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.pnlLeft.Controls.Add((Control) this.gcNew);
      this.pnlLeft.Controls.Add((Control) this.csOriginal);
      this.pnlLeft.Controls.Add((Control) this.gcOriginal);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(356, 439);
      this.pnlLeft.TabIndex = 0;
      this.gcNew.Controls.Add((Control) this.pnlToolbar2);
      this.gcNew.Controls.Add((Control) this.gvNew);
      this.gcNew.Dock = DockStyle.Fill;
      this.gcNew.HeaderForeColor = SystemColors.ControlText;
      this.gcNew.Location = new Point(0, 263);
      this.gcNew.Name = "gcNew";
      this.gcNew.Size = new Size(356, 176);
      this.gcNew.TabIndex = 7;
      this.gcNew.Text = "New File";
      this.pnlToolbar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar2.BackColor = Color.Transparent;
      this.pnlToolbar2.Controls.Add((Control) this.btnDeleteNew);
      this.pnlToolbar2.Controls.Add((Control) this.btnMoveNewDown);
      this.pnlToolbar2.Controls.Add((Control) this.btnMoveNewUp);
      this.pnlToolbar2.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar2.Location = new Point(268, 2);
      this.pnlToolbar2.Name = "pnlToolbar2";
      this.pnlToolbar2.Size = new Size(84, 22);
      this.pnlToolbar2.TabIndex = 8;
      this.gvNew.AllowDrop = true;
      this.gvNew.BorderStyle = BorderStyle.None;
      this.gvNew.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colNewPages";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Page";
      gvColumn1.Width = 354;
      this.gvNew.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvNew.Dock = DockStyle.Fill;
      this.gvNew.HeaderHeight = 0;
      this.gvNew.HeaderVisible = false;
      this.gvNew.HoverToolTip = this.tooltip;
      this.gvNew.Location = new Point(1, 26);
      this.gvNew.Name = "gvNew";
      this.gvNew.Size = new Size(354, 149);
      this.gvNew.TabIndex = 9;
      this.gvNew.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvNew.UseCompatibleEditingBehavior = true;
      this.gvNew.MouseUp += new MouseEventHandler(this.gvNew_MouseUp);
      this.gvNew.DragEnter += new DragEventHandler(this.gvNew_DragEnter);
      this.gvNew.SelectedIndexChanged += new EventHandler(this.gvNew_SelectedIndexChanged);
      this.gvNew.SelectedIndexCommitted += new EventHandler(this.gvNew_SelectedIndexCommitted);
      this.gvNew.DragDrop += new DragEventHandler(this.gvNew_DragDrop);
      this.gvNew.MouseDown += new MouseEventHandler(this.gvNew_MouseDown);
      this.gvNew.MouseMove += new MouseEventHandler(this.gvNew_MouseMove);
      this.csOriginal.AnimationDelay = 20;
      this.csOriginal.AnimationStep = 20;
      this.csOriginal.BorderStyle3D = Border3DStyle.Flat;
      this.csOriginal.ControlToHide = (Control) this.gcOriginal;
      this.csOriginal.Dock = DockStyle.Top;
      this.csOriginal.ExpandParentForm = false;
      this.csOriginal.Location = new Point(0, 256);
      this.csOriginal.Name = "csLeft";
      this.csOriginal.TabIndex = 6;
      this.csOriginal.TabStop = false;
      this.csOriginal.UseAnimations = false;
      this.csOriginal.VisualStyle = VisualStyles.Encompass;
      this.gcOriginal.Controls.Add((Control) this.pnlToolbar1);
      this.gcOriginal.Controls.Add((Control) this.gvOriginal);
      this.gcOriginal.Controls.Add((Control) this.pnlDragDrop);
      this.gcOriginal.Dock = DockStyle.Top;
      this.gcOriginal.HeaderForeColor = SystemColors.ControlText;
      this.gcOriginal.Location = new Point(0, 0);
      this.gcOriginal.Name = "gcOriginal";
      this.gcOriginal.Size = new Size(356, 256);
      this.gcOriginal.TabIndex = 1;
      this.gcOriginal.Text = "Original File";
      this.pnlToolbar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar1.BackColor = Color.Transparent;
      this.pnlToolbar1.Controls.Add((Control) this.btnDeleteOriginal);
      this.pnlToolbar1.Controls.Add((Control) this.btnMoveOriginalDown);
      this.pnlToolbar1.Controls.Add((Control) this.btnMoveOriginalUp);
      this.pnlToolbar1.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar1.Location = new Point(268, 2);
      this.pnlToolbar1.Name = "pnlToolbar1";
      this.pnlToolbar1.Size = new Size(84, 22);
      this.pnlToolbar1.TabIndex = 2;
      this.gvOriginal.AllowDrop = true;
      this.gvOriginal.BorderStyle = BorderStyle.None;
      this.gvOriginal.ClearSelectionsOnEmptyRowClick = false;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colOriginalPages";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Page";
      gvColumn2.Width = 354;
      this.gvOriginal.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvOriginal.Dock = DockStyle.Fill;
      this.gvOriginal.HeaderHeight = 0;
      this.gvOriginal.HeaderVisible = false;
      this.gvOriginal.HoverToolTip = this.tooltip;
      this.gvOriginal.Location = new Point(1, 26);
      this.gvOriginal.Name = "gvOriginal";
      this.gvOriginal.Size = new Size(354, 202);
      this.gvOriginal.TabIndex = 3;
      this.gvOriginal.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvOriginal.UseCompatibleEditingBehavior = true;
      this.gvOriginal.MouseUp += new MouseEventHandler(this.gvOriginal_MouseUp);
      this.gvOriginal.DragEnter += new DragEventHandler(this.gvOriginal_DragEnter);
      this.gvOriginal.SelectedIndexChanged += new EventHandler(this.gvOriginal_SelectedIndexChanged);
      this.gvOriginal.SelectedIndexCommitted += new EventHandler(this.gvOriginal_SelectedIndexCommitted);
      this.gvOriginal.DragDrop += new DragEventHandler(this.gvOriginal_DragDrop);
      this.gvOriginal.MouseDown += new MouseEventHandler(this.gvOriginal_MouseDown);
      this.gvOriginal.MouseMove += new MouseEventHandler(this.gvOriginal_MouseMove);
      this.pnlDragDrop.Borders = AnchorStyles.Top;
      this.pnlDragDrop.Controls.Add((Control) this.lblDragDrop);
      this.pnlDragDrop.Dock = DockStyle.Bottom;
      this.pnlDragDrop.Location = new Point(1, 228);
      this.pnlDragDrop.Name = "pnlDragDrop";
      this.pnlDragDrop.Size = new Size(354, 27);
      this.pnlDragDrop.TabIndex = 4;
      this.lblDragDrop.BackColor = Color.WhiteSmoke;
      this.lblDragDrop.Dock = DockStyle.Fill;
      this.lblDragDrop.Location = new Point(0, 1);
      this.lblDragDrop.Name = "lblDragDrop";
      this.lblDragDrop.Size = new Size(354, 26);
      this.lblDragDrop.TabIndex = 5;
      this.lblDragDrop.Text = "Select pages above and drag them to the file below";
      this.lblDragDrop.TextAlign = ContentAlignment.MiddleCenter;
      this.pnlFooter.Controls.Add((Control) this.btnSplit);
      this.pnlFooter.Controls.Add((Control) this.btnCancel);
      this.pnlFooter.Dock = DockStyle.Bottom;
      this.pnlFooter.Location = new Point(0, 439);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(775, 40);
      this.pnlFooter.TabIndex = 13;
      this.btnSplit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSplit.Location = new Point(617, 9);
      this.btnSplit.Name = "btnSplit";
      this.btnSplit.Size = new Size(75, 22);
      this.btnSplit.TabIndex = 14;
      this.btnSplit.Text = "Split";
      this.btnSplit.UseVisualStyleBackColor = true;
      this.btnSplit.Click += new EventHandler(this.btnSplit_Click);
      this.pnlRight.BackgroundImageLayout = ImageLayout.Center;
      this.pnlRight.Controls.Add((Control) this.pageViewer);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(363, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(412, 439);
      this.pnlRight.TabIndex = 11;
      this.pageViewer.Dock = DockStyle.Fill;
      this.pageViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pageViewer.Location = new Point(1, 1);
      this.pageViewer.Name = "pageViewer";
      this.pageViewer.Size = new Size(410, 437);
      this.pageViewer.TabIndex = 12;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(775, 479);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlFooter);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.Name = "SplitFileDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Split File";
      this.Resize += new EventHandler(this.SplitFileDialog_Resize);
      ((ISupportInitialize) this.btnDeleteOriginal).EndInit();
      ((ISupportInitialize) this.btnMoveOriginalDown).EndInit();
      ((ISupportInitialize) this.btnMoveOriginalUp).EndInit();
      ((ISupportInitialize) this.btnDeleteNew).EndInit();
      ((ISupportInitialize) this.btnMoveNewDown).EndInit();
      ((ISupportInitialize) this.btnMoveNewUp).EndInit();
      this.pnlLeft.ResumeLayout(false);
      this.gcNew.ResumeLayout(false);
      this.pnlToolbar2.ResumeLayout(false);
      this.gcOriginal.ResumeLayout(false);
      this.pnlToolbar1.ResumeLayout(false);
      this.pnlDragDrop.ResumeLayout(false);
      this.pnlFooter.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
