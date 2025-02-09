// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.SplitUnassignedFileDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Documents;
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
  public class SplitUnassignedFileDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvDocumentsMgr;
    private NativeAttachment originalFile;
    private string[] pdfPageList;
    private List<FileAttachment> fileList = new List<FileAttachment>();
    private List<string> changeList = new List<string>();
    private Point dragPoint = Point.Empty;
    private SDCDocument sdcDocumentCopy;
    private SDCHelper sdcHelper;
    private const string className = "SplitUnassignedFileDialog";
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
    private CollapsibleSplitter csOriginal;
    private GroupContainer gcOriginal;
    private GridView gvOriginal;
    private BorderPanel pnlDragDrop;
    private Label lblDragDrop;
    private FlowLayoutPanel pnlToolbar1;
    private StandardIconButton btnDeleteOriginal;
    private StandardIconButton btnMoveOriginalDown;
    private StandardIconButton btnMoveOriginalUp;
    private GroupContainer gcAssigned;
    private FlowLayoutPanel pnlToolbar2;
    private StandardIconButton btnAddDocument;
    private GridView gvDocuments;
    private ImageList imageList;

    public SplitUnassignedFileDialog(
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
      this.initDocumentList();
      this.loadDocumentList();
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
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      this.btnDeleteOriginal.Visible = folderAccessRights.CanDeletePageFromFile;
      this.btnAddDocument.Visible = folderAccessRights.CanAddDocuments;
    }

    private bool canassignDocument(DocumentLog doc)
    {
      if (doc == null || new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc).CanAttachUnassignedFiles)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to assign a file to the '" + doc.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
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
      this.gvDocuments.SelectedItems.Clear();
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
            Tracing.Log(SplitUnassignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitUnassignedFileDialog), "Calling UpdatePageOrder from btnMoveOriginalUp_Click");
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
            Tracing.Log(SplitUnassignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitUnassignedFileDialog), "Calling UpdatePageOrder from btnMoveOriginalDown_Click");
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
        Tracing.Log(SplitUnassignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitUnassignedFileDialog), "Calling DeletePages for deleting or removing pageIDs-(" + string.Join<int>(",", (IEnumerable<int>) intList) + ").");
        this.sdcHelper.DeletePages(this.sdcDocumentCopy, intList);
      }
      this.showOriginalPages();
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[2]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.BorrowerColumn
      });
      this.gvDocuments.Columns[1].SpringToFit = true;
      this.gvDocuments.Sort(0, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      foreach (DocumentLog allDocument in this.loanDataMgr.LoanData.GetLogList().GetAllDocuments())
      {
        GVItem itemByTag = this.gvDocuments.Items.GetItemByTag((object) allDocument);
        if (itemByTag == null)
        {
          GVItem gvItem = this.gvDocumentsMgr.AddItem(allDocument);
          gvItem.GroupItems.DisableSort = true;
          gvItem.ImageIndex = 0;
          gvItem.State = GVItemState.Collapsed;
        }
        else
          this.gvDocumentsMgr.RefreshItem(itemByTag, allDocument);
      }
      this.gvDocuments.ReSort();
    }

    private void showDocumentFiles()
    {
      this.gvOriginal.SelectedItems.Clear();
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
      {
        if (selectedItem.Tag is DocumentLog)
        {
          foreach (GVItem groupItem in (IEnumerable<GVItem>) selectedItem.GroupItems)
            stringList.Add((string) groupItem.Tag);
        }
        if (selectedItem.Tag is string)
          stringList.Add((string) selectedItem.Tag);
      }
      if (stringList.Count > 0)
        this.pageViewer.LoadFiles(stringList.ToArray());
      else
        this.pageViewer.CloseFiles();
    }

    private void showDocumentFiles(DocumentLog doc)
    {
      this.gvOriginal.SelectedItems.Clear();
      GVItem itemByTag = this.gvDocuments.Items.GetItemByTag((object) doc);
      if (itemByTag != null)
        itemByTag.Selected = true;
      this.showDocumentFiles();
    }

    private void gvDocuments_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showDocumentFiles();
    }

    private void btnAddDocument_Click(object sender, EventArgs e)
    {
      using (AddDocumentDialog addDocumentDialog = new AddDocumentDialog(this.loanDataMgr))
      {
        if (addDocumentDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadDocumentList();
        this.showDocumentFiles(addDocumentDialog.Documents[0]);
      }
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
      if (e.Data.GetDataPresent(typeof (GVItem)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvOriginal_DragDrop(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(typeof (GVItem)))
        return;
      GVItem data = (GVItem) e.Data.GetData(typeof (GVItem));
      if (data.GridView != this.gvDocuments)
        return;
      GVItem parentItem = data.ParentItem;
      if (parentItem == null)
        return;
      parentItem.GroupItems.Remove(data);
      this.gvOriginal.Items.Add(data);
      data.Selected = true;
      this.showOriginalPages();
    }

    private void gvDocuments_MouseDown(object sender, MouseEventArgs e)
    {
      GVItem itemAt = this.gvDocuments.GetItemAt(e.X, e.Y);
      if (itemAt == null || !(itemAt.Tag is string))
        return;
      this.dragPoint = e.Location;
    }

    private void gvDocuments_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      int num = (int) this.gvDocuments.DoDragDrop((object) this.gvDocuments.SelectedItems[0], DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof (GVSelectedItemCollection)) || e.Data.GetDataPresent(typeof (GVItem)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvDocuments_DragOver(object sender, DragEventArgs e)
    {
      Point client = this.gvDocuments.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvDocuments.GetItemAt(client.X, client.Y);
      if (itemAt != null && itemAt.Tag is DocumentLog)
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvDocuments_DragDrop(object sender, DragEventArgs e)
    {
      Point client = this.gvDocuments.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvDocuments.GetItemAt(client.X, client.Y);
      DocumentLog tag = (DocumentLog) itemAt.Tag;
      if (tag == null)
        return;
      if (e.Data.GetDataPresent(typeof (GVSelectedItemCollection)))
      {
        GVSelectedItemCollection data = (GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection));
        foreach (GVItem gvItem in data)
        {
          if (this.canassignDocument(tag) && data[0].GridView == this.gvOriginal)
          {
            this.gvOriginal.Items.Remove(gvItem);
            itemAt.GroupItems.Add(gvItem);
          }
        }
      }
      else
      {
        GVItem data = (GVItem) e.Data.GetData(typeof (GVItem));
        if (this.canassignDocument(tag) && data.GridView == this.gvDocuments)
        {
          GVItem parentItem = data.ParentItem;
          if (parentItem != null)
          {
            parentItem.GroupItems.Remove(data);
            itemAt.GroupItems.Add(data);
          }
        }
      }
      itemAt.State = GVItemState.Normal;
      this.showDocumentFiles(tag);
    }

    private void SplitUnassignedFileDialog_Resize(object sender, EventArgs e)
    {
      this.gcOriginal.Height = (this.pnlLeft.ClientSize.Height - this.csOriginal.Height) / 2;
    }

    private void btnSplit_Click(object sender, EventArgs e)
    {
      try
      {
        bool flag = false;
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
        {
          if (gvItem.GroupItems.Count > 0)
            flag = true;
        }
        if (!flag)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "In order to split the file, you must have at least one page assigned to a document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          long ticks = DateTime.Now.Ticks;
          List<string> stringList1 = new List<string>();
          List<string> stringList2 = (List<string>) null;
          List<DocumentLog> documentLogList = new List<DocumentLog>();
          Dictionary<string, DocumentLog> dictionary1 = new Dictionary<string, DocumentLog>();
          Dictionary<string, List<int>> pageIndexTable = (Dictionary<string, List<int>>) null;
          Dictionary<string, List<string>> dictionary2 = (Dictionary<string, List<string>>) null;
          List<int> displayIndices1 = new List<int>();
          List<string> stringList3 = new List<string>();
          if (this.gvOriginal.Items.Count > 0)
          {
            List<string> stringList4 = new List<string>();
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvOriginal.Items)
            {
              stringList4.Add(gvItem.Tag as string);
              if (this.loanDataMgr.UseSkyDriveClassic)
              {
                int num2 = int.Parse(gvItem.Text.Split(' ')[1]);
                displayIndices1.Add(num2);
                stringList3.Add(gvItem.Tag as string);
              }
            }
            if (this.loanDataMgr.UseSkyDriveClassic)
            {
              pageIndexTable = new Dictionary<string, List<int>>();
              dictionary2 = new Dictionary<string, List<string>>();
              List<int> indexListForSplit = this.sdcHelper.GetPageIndexListForSplit(this.originalFile, displayIndices1);
              pageIndexTable.Add("SourceFile", indexListForSplit);
              dictionary2.Add("SourceFile", stringList3);
            }
            using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
            {
              string str = pdfPageBuilder.MergeFiles(stringList4.ToArray());
              if (str == null)
                return;
              stringList1.Add(str);
              documentLogList.Add((DocumentLog) null);
            }
          }
          int num3 = 0;
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
          {
            if (gvItem.GroupItems.Count != 0)
            {
              List<string> stringList5 = new List<string>();
              List<int> displayIndices2 = new List<int>();
              List<string> stringList6 = new List<string>();
              foreach (GVItem groupItem in (IEnumerable<GVItem>) gvItem.GroupItems)
              {
                stringList5.Add(groupItem.Tag as string);
                if (this.loanDataMgr.UseSkyDriveClassic)
                {
                  int num4 = int.Parse(groupItem.Text.Split(' ')[1]);
                  displayIndices2.Add(num4);
                  stringList6.Add(groupItem.Tag as string);
                }
              }
              if (this.loanDataMgr.UseSkyDriveClassic)
              {
                ++num3;
                pageIndexTable = pageIndexTable == null ? new Dictionary<string, List<int>>() : pageIndexTable;
                dictionary2 = dictionary2 == null ? new Dictionary<string, List<string>>() : dictionary2;
                List<int> indexListForSplit = this.sdcHelper.GetPageIndexListForSplit(this.originalFile, displayIndices2);
                pageIndexTable.Add(string.Format("NewFile-{0}", (object) num3), indexListForSplit);
                dictionary2.Add(string.Format("NewFile-{0}", (object) num3), stringList6);
              }
              string str = (string) null;
              using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
              {
                str = pdfPageBuilder.MergeFiles(stringList5.ToArray());
                if (str == null)
                  return;
              }
              DocumentLog tag = gvItem.Tag as DocumentLog;
              stringList1.Add(str);
              documentLogList.Add(tag);
            }
          }
          if (this.loanDataMgr.UseSkyDriveClassic)
          {
            List<string> stringList7 = new List<string>();
            SDCHelper sdcHelper = new SDCHelper(this.loanDataMgr);
            stringList2 = new List<string>();
            this.originalFile.CurrentSDCDocument = this.sdcDocumentCopy;
            foreach (string key in pageIndexTable.Keys)
            {
              int num5 = 0;
              string str1 = string.Empty;
              List<int> intList = pageIndexTable[key];
              List<string> stringList8 = dictionary2[key];
              for (int index = 0; index < intList.Count; ++index)
              {
                Tracing.Log(SplitUnassignedFileDialog.sw, TraceLevel.Verbose, nameof (SplitUnassignedFileDialog), string.Format("Calling RemoveEditsFromPage for pageid-{0}", (object) intList[index]));
                string str2 = sdcHelper.RemoveEditsFromPage(this.originalFile, intList[index], stringList8[index]);
                stringList7.Add(str2);
                str1 = string.Empty;
                ++num5;
              }
              using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
              {
                str1 = pdfPageBuilder.MergeFiles(stringList7.ToArray());
                if (str1 == null)
                  return;
                stringList7 = new List<string>();
              }
              stringList2.Add(str1);
              string empty = string.Empty;
            }
          }
          if (!this.loanDataMgr.LockLoanWithExclusiveA())
            return;
          using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
          {
            string[] array = stringList2 == null ? (string[]) null : stringList2.ToArray();
            FileAttachment[] collection = attachFilesDialog.SplitFile(this.originalFile, stringList1.ToArray(), documentLogList.ToArray(), pageIndexTable, array);
            if (collection == null)
              return;
            this.fileList.AddRange((IEnumerable<FileAttachment>) collection);
          }
          foreach (string change in this.changeList)
            this.loanDataMgr.LoanHistory.TrackChange((FileAttachment) this.originalFile, change);
          RemoteLogger.Write(TraceLevel.Info, "Split NativeAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) stringList1.Count + " files");
          this.DialogResult = DialogResult.OK;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(SplitUnassignedFileDialog.sw, TraceLevel.Error, nameof (SplitUnassignedFileDialog), string.Format("Error in spliting and saving file opration. Ex: {0}", (object) ex));
        throw;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SplitUnassignedFileDialog));
      GVColumn gvColumn = new GVColumn();
      this.btnCancel = new Button();
      this.tooltip = new ToolTip(this.components);
      this.btnDeleteOriginal = new StandardIconButton();
      this.btnMoveOriginalDown = new StandardIconButton();
      this.btnMoveOriginalUp = new StandardIconButton();
      this.btnAddDocument = new StandardIconButton();
      this.csLeft = new CollapsibleSplitter();
      this.pnlLeft = new Panel();
      this.gcAssigned = new GroupContainer();
      this.pnlToolbar2 = new FlowLayoutPanel();
      this.gvDocuments = new GridView();
      this.imageList = new ImageList(this.components);
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
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      this.pnlLeft.SuspendLayout();
      this.gcAssigned.SuspendLayout();
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
      this.btnDeleteOriginal.MouseDownImage = (Image) null;
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
      this.btnMoveOriginalDown.MouseDownImage = (Image) null;
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
      this.btnMoveOriginalUp.MouseDownImage = (Image) null;
      this.btnMoveOriginalUp.Name = "btnMoveOriginalUp";
      this.btnMoveOriginalUp.Size = new Size(16, 16);
      this.btnMoveOriginalUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveOriginalUp.TabIndex = 26;
      this.btnMoveOriginalUp.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMoveOriginalUp, "Move Page Up");
      this.btnMoveOriginalUp.Click += new EventHandler(this.btnMoveOriginalUp_Click);
      this.btnAddDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDocument.BackColor = Color.Transparent;
      this.btnAddDocument.Location = new Point(84, 3);
      this.btnAddDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnAddDocument.MouseDownImage = (Image) null;
      this.btnAddDocument.Name = "btnAddDocument";
      this.btnAddDocument.Size = new Size(16, 16);
      this.btnAddDocument.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocument.TabIndex = 12;
      this.btnAddDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddDocument, "New document");
      this.btnAddDocument.Click += new EventHandler(this.btnAddDocument_Click);
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
      this.pnlLeft.Controls.Add((Control) this.gcAssigned);
      this.pnlLeft.Controls.Add((Control) this.csOriginal);
      this.pnlLeft.Controls.Add((Control) this.gcOriginal);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(356, 439);
      this.pnlLeft.TabIndex = 0;
      this.gcAssigned.Controls.Add((Control) this.pnlToolbar2);
      this.gcAssigned.Controls.Add((Control) this.gvDocuments);
      this.gcAssigned.Dock = DockStyle.Fill;
      this.gcAssigned.HeaderForeColor = SystemColors.ControlText;
      this.gcAssigned.Location = new Point(0, 263);
      this.gcAssigned.Name = "gcAssigned";
      this.gcAssigned.Size = new Size(356, 176);
      this.gcAssigned.TabIndex = 11;
      this.gcAssigned.Text = "Documents";
      this.pnlToolbar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar2.BackColor = Color.Transparent;
      this.pnlToolbar2.Controls.Add((Control) this.btnAddDocument);
      this.pnlToolbar2.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar2.Location = new Point(252, 2);
      this.pnlToolbar2.Name = "pnlToolbar2";
      this.pnlToolbar2.Size = new Size(100, 22);
      this.pnlToolbar2.TabIndex = 11;
      this.gvDocuments.AllowDrop = true;
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.ImageList = this.imageList;
      this.gvDocuments.ItemGrouping = true;
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(354, 149);
      this.gvDocuments.TabIndex = 12;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.UseCompatibleEditingBehavior = true;
      this.gvDocuments.MouseUp += new MouseEventHandler(this.gvDocuments_MouseUp);
      this.gvDocuments.DragEnter += new DragEventHandler(this.gvDocuments_DragEnter);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.DragDrop += new DragEventHandler(this.gvDocuments_DragDrop);
      this.gvDocuments.MouseDown += new MouseEventHandler(this.gvDocuments_MouseDown);
      this.gvDocuments.MouseMove += new MouseEventHandler(this.gvDocuments_MouseMove);
      this.gvDocuments.DragOver += new DragEventHandler(this.gvDocuments_DragOver);
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "document.png");
      this.imageList.Images.SetKeyName(1, "");
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
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colOriginalPages";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Page";
      gvColumn.Width = 354;
      this.gvOriginal.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvOriginal.Dock = DockStyle.Fill;
      this.gvOriginal.HeaderHeight = 0;
      this.gvOriginal.HeaderVisible = false;
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
      this.lblDragDrop.Text = "Select a file above and drag it to a document below";
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
      this.Name = nameof (SplitUnassignedFileDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Split File";
      this.Resize += new EventHandler(this.SplitUnassignedFileDialog_Resize);
      ((ISupportInitialize) this.btnDeleteOriginal).EndInit();
      ((ISupportInitialize) this.btnMoveOriginalDown).EndInit();
      ((ISupportInitialize) this.btnMoveOriginalUp).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      this.pnlLeft.ResumeLayout(false);
      this.gcAssigned.ResumeLayout(false);
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
