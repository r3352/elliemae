// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AllFilesDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.eFolder.UI;
using EllieMae.EMLite.eFolder.Utilities;
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AllFilesDialog : Form
  {
    private const string className = "AllFilesDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private static AllFilesDialog _instance = (AllFilesDialog) null;
    private LoanDataMgr loanDataMgr;
    private StackingOrderSetTemplate stackingTemplate;
    private GridViewDataManager gvDocumentsMgr;
    private GridViewDataManager gvUnassignedMgr;
    private Point dragPoint = Point.Empty;
    private DateTime lastDragScrollDate;
    private GVItem previousUnassignedDropItem;
    private GVItem previousDocDropItem;
    private bool refreshUnassignedFiles;
    private bool refreshDocuments;
    private bool isDragDropJobRunningorFailed;
    private Sessions.Session session;
    private bool isHideFileSizeEnabled;
    private IContainer components;
    private GroupContainer gcUnassigned;
    private GridView gvUnassigned;
    private Panel pnlLeft;
    private CollapsibleSplitter csUnassigned;
    private GroupContainer gcAssigned;
    private BorderPanel pnlDragDrop;
    private Label lblDragDrop;
    private StandardIconButton btnDeleteFile;
    private StandardIconButton btnEditDocument;
    private StandardIconButton btnAddDocument;
    private Panel pnlClose;
    private Button btnClose;
    private CollapsibleSplitter csLeft;
    private GridView gvDocuments;
    private BorderPanel pnlRight;
    private FileAttachmentViewerControl fileViewer;
    private IconButton btnAttachBrowse;
    private IconButton btnAttachScan;
    private IconButton btnAttachForms;
    private ToolTip tooltip;
    private FlowLayoutPanel pnlToolbar1;
    private FlowLayoutPanel pnlToolbar2;
    private EMHelpLink helpLink;
    private VerticalSeparator separator2;
    private Button btnAutoAssign;
    private IconButton btnMergeFiles;
    private VerticalSeparator separator1;
    private VerticalSeparator trainSeparator;
    private Button btnSuggestTraining;
    private GradientPanel pnlStackingOrder;
    private ComboBox cboStackingOrder;
    private Label label2;
    private ImageList imageList1;

    public static void ShowInstance(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      if (AllFilesDialog._instance == null)
      {
        AllFilesDialog._instance = new AllFilesDialog(loanDataMgr, session);
        AllFilesDialog._instance.FormClosing += new FormClosingEventHandler(AllFilesDialog._instance_FormClosing);
        AllFilesDialog._instance.Show();
      }
      else
      {
        if (AllFilesDialog._instance.WindowState == FormWindowState.Minimized)
          AllFilesDialog._instance.WindowState = FormWindowState.Normal;
        AllFilesDialog._instance.Activate();
      }
    }

    public static void ShowInstance(
      LoanDataMgr loanDataMgr,
      DocumentLog doc,
      Sessions.Session session)
    {
      AllFilesDialog.ShowInstance(loanDataMgr, session);
      AllFilesDialog._instance.loadDocumentList();
      AllFilesDialog._instance.showDocumentFiles(doc);
    }

    public static void ShowInstance(
      LoanDataMgr loanDataMgr,
      FileAttachment[] fileList,
      Sessions.Session session)
    {
      AllFilesDialog.ShowInstance(loanDataMgr, session);
      AllFilesDialog._instance.loadUnassignedList();
      AllFilesDialog._instance.showUnassignedFiles(fileList);
    }

    private static void _instance_FormClosing(object sender, FormClosingEventArgs e)
    {
      AllFilesDialog._instance = (AllFilesDialog) null;
    }

    private AllFilesDialog(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.session = session;
      this.loanDataMgr = loanDataMgr;
      this.isHideFileSizeEnabled = string.Equals(this.session.ConfigurationManager.GetCompanySetting("eFolder", "HideFileSize"), "TRUE", StringComparison.OrdinalIgnoreCase);
      this.initEventHandlers();
      this.initUnassignedList();
      this.initDocumentList();
      this.initializeStackingOrder();
      this.loadUnassignedList();
      this.loadDocumentList();
      this.applySecurity();
    }

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

    private void initUnassignedList()
    {
      this.gvUnassignedMgr = new GridViewDataManager(this.gvUnassigned, this.loanDataMgr);
      this.gvUnassignedMgr.CreateLayout(new EllieMae.EMLite.ClientServer.TableLayout.Column[4]
      {
        GridViewDataManager.NameWithIconColumn,
        GridViewDataManager.CurrentRowColumn,
        GridViewDataManager.FileSizeColumn,
        GridViewDataManager.DateTimeColumn
      });
      this.gvUnassigned.Columns[3].SpringToFit = true;
      this.gvUnassigned.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.gvUnassigned.Columns[1].Width = 0;
      if (!this.isHideFileSizeEnabled)
        return;
      this.gvUnassigned.Columns[2].Width = 0;
    }

    private void loadUnassignedList()
    {
      FileAttachment[] unassignedAttachments = this.loanDataMgr.FileAttachments.GetUnassignedAttachments();
      foreach (FileAttachment file in unassignedAttachments)
      {
        GVItem itemByTag = this.gvUnassigned.Items.GetItemByTag((object) file);
        if (itemByTag == null)
          this.gvUnassignedMgr.AddItem(file, (FileAttachmentReference) null);
        else
          this.gvUnassignedMgr.RefreshItem(itemByTag, file, (FileAttachmentReference) null);
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUnassigned.Items)
      {
        if (Array.IndexOf<object>((object[]) unassignedAttachments, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvUnassigned.Items.Remove(gvItem);
      this.gvUnassigned.ReSort();
    }

    private void showUnassignedFiles()
    {
      this.gvDocuments.SelectedItems.Clear();
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem selectedItem in this.gvUnassigned.SelectedItems)
        fileAttachmentList.Add((FileAttachment) selectedItem.Tag);
      if (fileAttachmentList.Count > 0)
        this.fileViewer.LoadFiles(fileAttachmentList.ToArray());
      else
        this.fileViewer.CloseFile();
      if (this.fileViewer.IsCloudViewer())
      {
        this.gvUnassigned.Columns[1].Width = GridViewDataManager.CurrentRowColumn.Width;
        this.clearCurrentFileIndicators();
      }
      else
        this.gvUnassigned.Columns[1].Width = 0;
    }

    private void showUnassignedFiles(FileAttachment file)
    {
      this.showUnassignedFiles(new FileAttachment[1]{ file });
    }

    private void showUnassignedFiles(FileAttachment[] fileList)
    {
      this.gvUnassigned.SelectedItems.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUnassigned.Items)
      {
        if (Array.IndexOf<object>((object[]) fileList, gvItem.Tag) >= 0)
          gvItem.Selected = true;
      }
      this.showUnassignedFiles();
    }

    private void gvUnassigned_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      string title = e.EditorControl.Text.Trim();
      this.gvUnassigned.CancelEditing();
      if (!(title != e.SubItem.Text) || !(title != string.Empty))
        return;
      FileAttachment tag = (FileAttachment) e.SubItem.Item.Tag;
      if (tag is CloudAttachment)
      {
        try
        {
          Task.WaitAll((Task) new EBSRestClient(this.loanDataMgr).SetAttachmentTitle((CloudAttachment) tag, title));
        }
        catch (Exception ex)
        {
          Tracing.Log(AllFilesDialog.sw, nameof (AllFilesDialog), TraceLevel.Error, "Exception in SetAttachmentTitle: " + ex.Message);
        }
      }
      else
        tag.Title = title;
    }

    private void gvUnassigned_ItemDoubleClick(object source, GVItemEventArgs e)
    {
    }

    private void gvUnassigned_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    private void gvUnassigned_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvUnassigned.SelectedItems.Count;
      this.btnDeleteFile.Enabled = count > 0;
      this.btnMergeFiles.Enabled = count > 1;
      this.btnAutoAssign.Enabled = count > 0;
    }

    private void gvUnassigned_SelectedIndexCommitted(object sender, EventArgs e)
    {
      this.showUnassignedFiles();
    }

    private void btnAttachBrowse_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
      {
        FileAttachment[] fileList = attachFilesDialog.AttachFiles((DocumentLog) null);
        if (fileList == null)
          return;
        this.showUnassignedFiles(fileList);
      }
    }

    private void btnAttachScan_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (ScanFileDialog scanFileDialog = new ScanFileDialog(this.loanDataMgr, (DocumentLog) null))
      {
        if (scanFileDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.showUnassignedFiles(scanFileDialog.Files);
      }
    }

    private void btnAttachForms_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      using (AttachFormsDialog attachFormsDialog = new AttachFormsDialog(this.loanDataMgr, (DocumentLog) null))
      {
        if (attachFormsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.showUnassignedFiles(attachFormsDialog.Files);
      }
    }

    private void btnDeleteFile_Click(object sender, EventArgs e)
    {
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem selectedItem in this.gvUnassigned.SelectedItems)
        fileAttachmentList.Add((FileAttachment) selectedItem.Tag);
      string str = string.Empty;
      foreach (FileAttachment fileAttachment in fileAttachmentList)
        str = str + fileAttachment.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete the following files(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No || !this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      foreach (FileAttachment attachment in fileAttachmentList)
        this.loanDataMgr.FileAttachments.Remove(RemoveReasonType.User, attachment);
      this.showUnassignedFiles();
    }

    private void btnMergeFiles_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      long ticks = DateTime.Now.Ticks;
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      foreach (GVItem selectedItem in this.gvUnassigned.SelectedItems)
      {
        FileAttachment tag = (FileAttachment) selectedItem.Tag;
        switch (tag)
        {
          case BackgroundAttachment _:
            ++num1;
            break;
          case ImageAttachment _:
            ++num2;
            break;
          case NativeAttachment _:
            ++num3;
            break;
          case CloudAttachment _:
            ++num4;
            break;
        }
        fileAttachmentList.Add(tag);
      }
      bool flag1 = this.loanDataMgr.SystemConfiguration.ImageAttachmentSettings.UseImageAttachments;
      bool flag2 = false;
      if (num3 == fileAttachmentList.Count)
        flag1 = false;
      else if (num2 == fileAttachmentList.Count)
        flag1 = true;
      else if (num4 == fileAttachmentList.Count)
        flag2 = true;
      if (!flag1 && !flag2)
      {
        using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(this.loanDataMgr))
        {
          string file1 = pdfFileBuilder.CreateFile(fileAttachmentList.ToArray());
          if (file1 == null)
            return;
          using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
          {
            FileAttachment file2 = attachFilesDialog.MergeFiles(fileAttachmentList.ToArray(), file1, (DocumentLog) null);
            if (file2 == null)
              return;
            RemoteLogger.Write(TraceLevel.Info, "Merged NativeAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) fileAttachmentList.Count + " files");
            this.showUnassignedFiles(file2);
          }
        }
      }
      else if (flag1 && !flag2)
      {
        List<ImageAttachment> imageAttachmentList = new List<ImageAttachment>();
        foreach (FileAttachment fileAttachment1 in fileAttachmentList)
        {
          if (fileAttachment1 is ImageAttachment)
          {
            ImageAttachment imageAttachment = (ImageAttachment) fileAttachment1;
            imageAttachmentList.Add(imageAttachment);
          }
          else if (fileAttachment1 is NativeAttachment)
          {
            NativeAttachment attachment = (NativeAttachment) fileAttachment1;
            string filepath = this.loanDataMgr.FileAttachments.DownloadAttachment(attachment);
            if (filepath == null)
              return;
            using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
            {
              FileAttachment fileAttachment2 = attachFilesDialog.ReplaceFile(AddReasonType.ConversionForMerge, attachment, filepath, (DocumentLog) null);
              if (fileAttachment2 == null)
                return;
              ImageAttachment imageAttachment = (ImageAttachment) fileAttachment2;
              imageAttachmentList.Add(imageAttachment);
            }
          }
        }
        ImageAttachment file = this.loanDataMgr.FileAttachments.MergeAttachments(imageAttachmentList.ToArray(), (DocumentLog) null);
        RemoteLogger.Write(TraceLevel.Info, "Merged ImageAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) fileAttachmentList.Count + " files");
        this.showUnassignedFiles((FileAttachment) file);
      }
      else
      {
        if (!flag2)
          return;
        this.isDragDropJobRunningorFailed = false;
        List<CloudAttachment> cloudAttachmentList = new List<CloudAttachment>();
        foreach (CloudAttachment cloudAttachment in fileAttachmentList)
          cloudAttachmentList.Add(cloudAttachment);
        EOSClient eosClient = new EOSClient(Session.LoanDataMgr);
        List<CloudAttachment> source = cloudAttachmentList;
        ParallelOptions parallelOptions = new ParallelOptions();
        parallelOptions.MaxDegreeOfParallelism = 4;
        Action<CloudAttachment, ParallelLoopState> body = (Action<CloudAttachment, ParallelLoopState>) ((attachment, state) =>
        {
          string objectId = attachment.ObjectId;
          this.isDragDropJobRunningorFailed = false;
          string jobId = DragDropDetailCollection.GetJobId(Session.LoanDataMgr.LoanData.GUID, attachment.ID);
          if (string.IsNullOrEmpty(jobId))
            return;
          Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Creating EOSClient");
          PerformanceMeter.Current.AddCheckpoint("EOSClient created.", 626, nameof (btnMergeFiles_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Files\\AllFilesDialog.cs");
          Task<DragDropJobStatusResponse> task = eosClient.CheckDragDropJobStatus(jobId);
          Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Waiting for CheckDragDropJobStatus Task");
          Task.WaitAll((Task) task);
          PerformanceMeter.Current.AddCheckpoint("CheckDragDropJobStatus completed.", 633, nameof (btnMergeFiles_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Files\\AllFilesDialog.cs");
          Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Getting CheckDragDropJobStatus Response");
          DragDropJobStatusResponse result = task.Result;
          if (result.status == "Running")
            this.isDragDropJobRunningorFailed = true;
          else if (result.status == "Success")
          {
            foreach (string hiddenAttachmentId in DragDropDetailCollection.GetHiddenAttachmentIds(jobId))
            {
              FileAttachment fileAttachment = Session.LoanDataMgr.FileAttachments[hiddenAttachmentId];
              if (fileAttachment != null)
                Session.LoanDataMgr.FileAttachments.Remove(RemoveReasonType.Merge, fileAttachment);
            }
            this.isDragDropJobRunningorFailed = false;
            DragDropDetailCollection.RemoveJobId(jobId);
          }
          else
          {
            if (!(result.status == "Failed"))
              return;
            DragDropDetailCollection.RemoveJobId(jobId);
            this.isDragDropJobRunningorFailed = true;
            state.Stop();
          }
        });
        Parallel.ForEach<CloudAttachment>((IEnumerable<CloudAttachment>) source, parallelOptions, body);
        if (!this.isDragDropJobRunningorFailed)
        {
          CloudAttachment file = (CloudAttachment) null;
          using (EOSClientDialog eosClientDialog = new EOSClientDialog(this.loanDataMgr))
            file = eosClientDialog.MergeAttachments(this.fileViewer.MergeJobId, cloudAttachmentList.ToArray(), (DocumentLog) null);
          if (file == null)
            return;
          RemoteLogger.Write(TraceLevel.Info, "Merged CloudAttachments: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms, " + (object) fileAttachmentList.Count + " files");
          this.showUnassignedFiles((FileAttachment) file);
        }
        else
          RemoteLogger.Write(TraceLevel.Info, "One or more attachment(s) previous drag drop operation has failed or in running state hence we cannot procced with merge. Please retry again.");
      }
    }

    private void btnAutoAssign_Click(object sender, EventArgs e)
    {
      if (!this.fileViewer.CanCloseViewer())
        return;
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem selectedItem in this.gvUnassigned.SelectedItems)
      {
        FileAttachment tag = selectedItem.Tag as FileAttachment;
        fileAttachmentList.Add(tag);
      }
      using (AutoAssignFilesDialog assignFilesDialog = new AutoAssignFilesDialog(this.loanDataMgr, this.session))
        assignFilesDialog.AssignFiles(fileAttachmentList.ToArray());
      this.showUnassignedFiles();
    }

    private void initializeStackingOrder()
    {
      this.cboStackingOrder.Items.Add((object) GridViewDataManager.DefaultStackingOrderName);
      this.cboStackingOrder.SelectedItem = (object) GridViewDataManager.DefaultStackingOrderName;
    }

    private void cboStackingOrder_DropDown(object sender, EventArgs e)
    {
      foreach (FileSystemEntry templateDirEntry in Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, FileSystemEntry.PublicRoot))
      {
        FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(templateDirEntry);
        if (!this.cboStackingOrder.Items.Contains((object) systemEntryListItem))
          this.cboStackingOrder.Items.Add((object) systemEntryListItem);
      }
      this.cboStackingOrder.DropDown -= new EventHandler(this.cboStackingOrder_DropDown);
    }

    private void cboStackingOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboStackingOrder.SelectedItem is FileSystemEntryListItem)
      {
        this.stackingTemplate = (StackingOrderSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry);
        this.gvDocuments.SortOption = GVSortOption.Owner;
        this.gvDocuments.SortIconVisible = false;
        if (this.gvDocuments.Items.Count > 0)
          this.gvDocuments.ReSort();
      }
      else
      {
        this.stackingTemplate = (StackingOrderSetTemplate) null;
        this.gvDocuments.SortOption = GVSortOption.Auto;
        this.gvDocuments.SortIconVisible = true;
      }
      this.loadDocumentList();
    }

    private void gvDocuments_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.stackingTemplate != null)
        this.gvDocuments.Items.Sort((IComparer<GVItem>) new DocumentSortComparer(this.loanDataMgr.LoanData, this.stackingTemplate));
      e.Cancel = true;
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.CreateLayout(new EllieMae.EMLite.ClientServer.TableLayout.Column[4]
      {
        GridViewDataManager.NameWithIconColumn,
        GridViewDataManager.CurrentRowColumn,
        GridViewDataManager.FileSizeColumn,
        GridViewDataManager.BorrowerColumn
      });
      this.gvDocuments.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.gvDocuments.Columns[3].SpringToFit = true;
      this.gvDocuments.Columns[1].Width = 0;
      if (this.isHideFileSizeEnabled)
        this.gvDocuments.Columns[2].Width = 0;
      this.gvDocuments.Sort(0, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      string str = (string) null;
      if (this.gvDocuments.SelectedItems.Count > 0 && this.gvDocuments.SelectedItems[0].Tag is FileAttachment)
        str = ((FileAttachment) this.gvDocuments.SelectedItems[0].Tag).ID;
      DocumentLog[] array = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      if (this.stackingTemplate != null && this.stackingTemplate.FilterDocuments)
      {
        List<DocumentLog> documentLogList = new List<DocumentLog>();
        foreach (DocumentLog documentLog in array)
        {
          if (this.stackingTemplate.DocNames.Contains((object) documentLog.Title))
            documentLogList.Add(documentLog);
        }
        array = documentLogList.ToArray();
      }
      foreach (DocumentLog doc in array)
      {
        GVItem gvItem1 = this.gvDocuments.Items.GetItemByTag((object) doc);
        if (gvItem1 == null)
        {
          gvItem1 = this.gvDocumentsMgr.AddItem(doc);
          gvItem1.GroupItems.DisableSort = true;
          gvItem1.State = GVItemState.Collapsed;
        }
        else
          this.gvDocumentsMgr.RefreshItem(gvItem1, doc);
        if (gvItem1.GroupItems.Count > 0)
          gvItem1.GroupItems.Clear();
        foreach (FileAttachmentReference fileRef in doc.Files.ToArray())
        {
          if (fileRef.IsActive)
          {
            FileAttachment attachment = this.loanDataMgr.FileAttachments.GetAttachment(fileRef);
            if (attachment != null)
            {
              bool flag = false;
              if (str == attachment.ID)
                flag = true;
              GVItem gvItem2 = this.gvDocumentsMgr.CreateItem(attachment, fileRef);
              gvItem2.Selected = flag;
              gvItem1.GroupItems.Add(gvItem2);
            }
          }
        }
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        if (Array.IndexOf<object>((object[]) array, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvDocuments.Items.Remove(gvItem);
      this.gvDocuments.ReSort();
    }

    private void showDocumentFiles()
    {
      this.gvUnassigned.SelectedItems.Clear();
      List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
      {
        if (selectedItem.Tag is DocumentLog)
        {
          foreach (GVItem groupItem in (IEnumerable<GVItem>) selectedItem.GroupItems)
            fileAttachmentList.Add((FileAttachment) groupItem.Tag);
        }
        if (selectedItem.Tag is FileAttachment)
          fileAttachmentList.Add((FileAttachment) selectedItem.Tag);
      }
      if (fileAttachmentList.Count > 0)
        this.fileViewer.LoadFiles(fileAttachmentList.ToArray());
      else
        this.fileViewer.CloseFile();
      if (this.fileViewer.IsCloudViewer())
      {
        this.gvDocuments.Columns[1].Width = GridViewDataManager.CurrentRowColumn.Width;
        this.clearCurrentFileIndicators();
      }
      else
        this.gvDocuments.Columns[1].Width = 0;
    }

    private void showDocumentFiles(DocumentLog doc)
    {
      this.gvDocuments.SelectedItems.Clear();
      GVItem itemByTag = this.gvDocuments.Items.GetItemByTag((object) doc);
      if (itemByTag != null)
        itemByTag.Selected = true;
      this.showDocumentFiles();
    }

    private void showDocumentFiles(DocumentLog doc, FileAttachment attachment)
    {
      GVItem itemByTag = this.gvDocuments.Items.GetItemByTag((object) doc);
      if (itemByTag != null)
      {
        foreach (GVItem groupItem in (IEnumerable<GVItem>) itemByTag.GroupItems)
        {
          if ((groupItem.Tag as FileAttachment).ID == attachment.ID)
          {
            groupItem.Selected = true;
            this.fileViewer.LoadFiles(new FileAttachment[1]
            {
              attachment
            });
            return;
          }
        }
      }
      this.fileViewer.CloseFile();
    }

    private void gvDocuments_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (!(e.SubItem.Item.Tag is DocumentLog))
        return;
      e.Cancel = true;
    }

    private void gvDocuments_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      string title = e.EditorControl.Text.Trim();
      this.gvDocuments.CancelEditing();
      if (!(title != e.SubItem.Text) || !(title != string.Empty))
        return;
      FileAttachment tag = (FileAttachment) e.SubItem.Item.Tag;
      if (tag is CloudAttachment)
      {
        try
        {
          Task.WaitAll((Task) new EBSRestClient(this.loanDataMgr).SetAttachmentTitle((CloudAttachment) tag, title));
        }
        catch (Exception ex)
        {
          Tracing.Log(AllFilesDialog.sw, nameof (AllFilesDialog), TraceLevel.Error, "Exception in SetAttachmentTitle: " + ex.Message);
        }
      }
      else
        tag.Title = title;
    }

    private void gvDocuments_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!(e.Item.Tag is DocumentLog))
        return;
      this.btnEditDocument_Click(source, EventArgs.Empty);
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag1 = false;
      bool flag2 = false;
      if (this.gvDocuments.SelectedItems.Count > 0)
      {
        if (this.gvDocuments.SelectedItems[0].ParentItem == null)
        {
          flag1 = true;
          flag2 = true;
        }
        else if (this.gvDocuments.SelectedItems[0].Tag is NativeAttachment || this.gvDocuments.SelectedItems[0].Tag is ImageAttachment || this.gvDocuments.SelectedItems[0].Tag is CloudAttachment)
          flag2 = true;
      }
      this.btnEditDocument.Enabled = flag1;
      this.btnSuggestTraining.Enabled = flag2;
    }

    private void gvDocuments_BeforeSelectedIndexCommitted(object sender, CancelEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
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
        this.showDocumentFiles(addDocumentDialog.Documents[0]);
      }
    }

    private void btnEditDocument_Click(object sender, EventArgs e)
    {
      if (this.gvDocuments.SelectedItems.Count == 0)
        return;
      DocumentDetailsDialog.ShowInstance(this.loanDataMgr, (DocumentLog) this.gvDocuments.SelectedItems[0].Tag);
    }

    private void btnSuggestTraining_Click(object sender, EventArgs e)
    {
      if (this.gvDocuments.SelectedItems.Count == 0)
        return;
      FileAttachment[] attachmentList = (FileAttachment[]) null;
      DocumentLog docLog = (DocumentLog) null;
      if (this.gvDocuments.SelectedItems[0].Tag is DocumentLog)
      {
        docLog = this.gvDocuments.SelectedItems[0].Tag as DocumentLog;
        using (SuggestTrainingDialog suggestTrainingDialog = new SuggestTrainingDialog(this.loanDataMgr, docLog))
        {
          if (suggestTrainingDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
            return;
          attachmentList = suggestTrainingDialog.Attachments;
        }
      }
      else if (this.gvDocuments.SelectedItems[0].Tag is FileAttachment)
      {
        attachmentList = new FileAttachment[1]
        {
          this.gvDocuments.SelectedItems[0].Tag as FileAttachment
        };
        docLog = this.gvDocuments.SelectedItems[0].ParentItem.Tag as DocumentLog;
      }
      using (DocumentationClassificationClient classificationClient = new DocumentationClassificationClient(this.session))
      {
        switch (classificationClient.SuggestTraining(docLog, attachmentList))
        {
          case DialogResult.OK:
            int num1 = (int) MessageBox.Show((IWin32Window) this, "All the selected files have been suggested for training. ", "Template Training", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          case DialogResult.Abort:
            int num2 = (int) MessageBox.Show((IWin32Window) this, "Some selected files have been suggested for training. ", "Template Training", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          default:
            int num3 = (int) MessageBox.Show((IWin32Window) this, "The selected files could not be suggested for training.", "Template Training", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
        }
      }
    }

    private void gvUnassigned_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.gvUnassigned.GetItemAt(e.X, e.Y) == null)
        return;
      this.dragPoint = e.Location;
    }

    private void gvUnassigned_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      FileAttachment[] data = new FileAttachment[this.gvUnassigned.SelectedItems.Count];
      for (int index = 0; index < data.Length; ++index)
        data[index] = (FileAttachment) this.gvUnassigned.SelectedItems[index].Tag;
      int num = (int) this.gvUnassigned.DoDragDrop((object) data, DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvUnassigned_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvUnassigned_DragOver(object sender, DragEventArgs e)
    {
      if (this.previousUnassignedDropItem != null)
        this.previousUnassignedDropItem.BackColor = Color.Empty;
      Point client = this.gvUnassigned.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvUnassigned.GetItemAt(client.X, client.Y);
      this.previousUnassignedDropItem = itemAt;
      if (e.Data.GetDataPresent(typeof (PageImage[])))
      {
        if (itemAt != null && itemAt.Tag is NativeAttachment)
        {
          e.Effect = DragDropEffects.None;
        }
        else
        {
          e.Effect = DragDropEffects.Move;
          eFolderUIHelper.SetPageThumbnailCursor((PageImage[]) e.Data.GetData(typeof (PageImage[])));
          if (itemAt == null)
            return;
          itemAt.BackColor = EncompassColors.DragAnddropColor;
        }
      }
      else if (e.Data.GetDataPresent(typeof (FileAttachment[])))
        e.Effect = DragDropEffects.Move;
      else if (e.Data.GetDataPresent(DataFormats.FileDrop))
        e.Effect = DragDropEffects.Copy;
      else if (e.Data.GetDataPresent("DragImageBits"))
      {
        e.Effect = DragDropEffects.Move;
        if (itemAt == null)
          return;
        itemAt.BackColor = EncompassColors.DragAnddropColor;
      }
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvUnassigned_DragDrop(object sender, DragEventArgs e)
    {
      Point client = this.gvUnassigned.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvUnassigned.GetItemAt(client.X, client.Y);
      if (e.Data.GetDataPresent(typeof (FileAttachment[])))
      {
        FileAttachment[] data = (FileAttachment[]) e.Data.GetData(typeof (FileAttachment[]));
        foreach (FileAttachment file in data)
        {
          if (!this.assignDocument(file, (DocumentLog) null))
            return;
        }
        this.showUnassignedFiles(data);
      }
      else if (e.Data.GetDataPresent(typeof (PageImage[])))
      {
        PageImage[] data = (PageImage[]) e.Data.GetData(typeof (PageImage[]));
        ImageAttachment attachment = (ImageAttachment) null;
        if (itemAt != null)
          attachment = itemAt.Tag as ImageAttachment;
        if (!this.assignPageDrop(data, attachment, (DocumentLog) null))
          return;
        if (this.gvDocuments.SelectedItems.Count > 0)
          this.showDocumentFiles();
        else
          this.showUnassignedFiles();
      }
      else if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        FileAttachment[] fileList = this.assignFileDrop((string[]) e.Data.GetData(DataFormats.FileDrop), (DocumentLog) null);
        if (fileList == null)
          return;
        this.showUnassignedFiles(fileList);
      }
      else
      {
        if (!e.Data.GetDataPresent("DragImageBits"))
          return;
        CloudAttachment destAttachment = (CloudAttachment) null;
        if (itemAt != null)
          destAttachment = itemAt.Tag as CloudAttachment;
        this.assignPageDrop(destAttachment, (DocumentLog) null);
      }
    }

    private void gvDocuments_MouseDown(object sender, MouseEventArgs e)
    {
      GVItem itemAt = this.gvDocuments.GetItemAt(e.X, e.Y);
      if (itemAt == null || !(itemAt.Tag is FileAttachment))
        return;
      this.dragPoint = e.Location;
    }

    private void gvDocuments_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      if (this.gvDocuments.SelectedItems.Count > 0)
      {
        int num = (int) this.gvDocuments.DoDragDrop((object) new FileAttachment[1]
        {
          (FileAttachment) this.gvDocuments.SelectedItems[0].Tag
        }, DragDropEffects.Move);
      }
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_DragOver(object sender, DragEventArgs e)
    {
      if (this.previousDocDropItem != null)
        this.previousDocDropItem.BackColor = Color.Empty;
      Point client = this.gvDocuments.PointToClient(new Point(e.X, e.Y));
      if (this.lastDragScrollDate <= DateTime.Now.AddSeconds(-0.15))
      {
        if (client.Y <= this.gvDocuments.ItemHeight * 2)
        {
          if (this.gvDocuments.ScrollVPosition > 0)
            this.gvDocuments.SetVScroll(this.gvDocuments.ScrollVPosition - 1);
          this.lastDragScrollDate = DateTime.Now;
        }
        else if (client.Y > this.gvDocuments.ClientSize.Height - this.gvDocuments.Font.Height)
        {
          this.gvDocuments.SetVScroll(this.gvDocuments.ScrollVPosition + 1);
          this.lastDragScrollDate = DateTime.Now;
        }
      }
      GVItem itemAt = this.gvDocuments.GetItemAt(client.X, client.Y);
      this.previousDocDropItem = itemAt;
      if (itemAt == null)
        e.Effect = DragDropEffects.None;
      else if (e.Data.GetDataPresent(typeof (PageImage[])))
      {
        if (itemAt.Tag is NativeAttachment)
        {
          e.Effect = DragDropEffects.None;
        }
        else
        {
          e.Effect = DragDropEffects.Move;
          eFolderUIHelper.SetPageThumbnailCursor((PageImage[]) e.Data.GetData(typeof (PageImage[])));
          itemAt.BackColor = EncompassColors.DragAnddropColor;
        }
      }
      else if (e.Data.GetDataPresent(typeof (FileAttachment[])))
      {
        if (itemAt.Tag is DocumentLog)
        {
          e.Effect = DragDropEffects.Move;
          itemAt.BackColor = EncompassColors.DragAnddropColor;
        }
        else
          e.Effect = DragDropEffects.None;
      }
      else if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        if (itemAt.Tag is DocumentLog)
          e.Effect = DragDropEffects.Copy;
        else
          e.Effect = DragDropEffects.None;
      }
      else if (e.Data.GetDataPresent("DragImageBits"))
      {
        e.Effect = DragDropEffects.Move;
        itemAt.BackColor = EncompassColors.DragAnddropColor;
      }
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvDocuments_DragDrop(object sender, DragEventArgs e)
    {
      Point client = this.gvDocuments.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvDocuments.GetItemAt(client.X, client.Y);
      if (e.Data.GetDataPresent(typeof (FileAttachment[])))
      {
        FileAttachment[] data = (FileAttachment[]) e.Data.GetData(typeof (FileAttachment[]));
        DocumentLog tag = (DocumentLog) itemAt.Tag;
        foreach (FileAttachment file in data)
        {
          if (!this.assignDocument(file, tag))
            return;
        }
        this.showDocumentFiles(tag);
      }
      else if (e.Data.GetDataPresent(typeof (PageImage[])))
      {
        if (!this.assignPageDrop((PageImage[]) e.Data.GetData(typeof (PageImage[])), itemAt.Tag as ImageAttachment, itemAt.Tag as DocumentLog))
          return;
        if (this.gvDocuments.SelectedItems.Count > 0)
          this.showDocumentFiles();
        else
          this.showUnassignedFiles();
      }
      else if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
        DocumentLog tag = (DocumentLog) itemAt.Tag;
        if (this.assignFileDrop(data, tag) == null)
          return;
        this.showDocumentFiles(tag);
      }
      else if (e.Data.GetDataPresent("DragImageBits"))
        this.assignPageDrop(itemAt.Tag as CloudAttachment, itemAt.Tag as DocumentLog);
      itemAt.State = GVItemState.Normal;
    }

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      this.btnAttachBrowse.Visible = folderAccessRights.CanBrowseAttach;
      this.btnAttachScan.Visible = folderAccessRights.CanScanAttach;
      this.btnAttachForms.Visible = folderAccessRights.CanAttachForms;
      this.separator1.Visible = (folderAccessRights.CanBrowseAttach || folderAccessRights.CanScanAttach || folderAccessRights.CanAttachForms) && (folderAccessRights.CanMergeFiles || folderAccessRights.CanDeleteFiles);
      this.btnMergeFiles.Visible = folderAccessRights.CanMergeFiles;
      this.btnDeleteFile.Visible = folderAccessRights.CanDeleteFiles;
      this.separator2.Visible = (folderAccessRights.CanBrowseAttach || folderAccessRights.CanScanAttach || folderAccessRights.CanAttachForms || folderAccessRights.CanMergeFiles || folderAccessRights.CanDeleteFiles) && folderAccessRights.CanAutoAssignFiles;
      this.btnAutoAssign.Visible = folderAccessRights.CanAutoAssignFiles;
      this.btnAddDocument.Visible = folderAccessRights.CanAddDocuments;
      if ((EnableDisableSetting) this.session.GetComponentSetting("Scanning", (object) EnableDisableSetting.Enabled) == EnableDisableSetting.Disabled)
        this.btnAttachScan.Visible = false;
      this.btnSuggestTraining.Visible = this.trainSeparator.Visible = folderAccessRights.CanSuggestTrainings;
    }

    private bool assignDocument(FileAttachment file, DocumentLog doc)
    {
      DocumentLog linkedDocument = this.loanDataMgr.FileAttachments.GetLinkedDocument(file.ID);
      if (linkedDocument != null && !new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) linkedDocument).CanRemoveDocumentFiles)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to remove the '" + file.Title + "' file from the '" + linkedDocument.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (doc != null && !new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc).CanAttachUnassignedFiles)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to assign the '" + file.Title + "' file to the '" + doc.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      linkedDocument?.Files.Remove(file.ID);
      doc?.Files.Add(file.ID, this.session.UserID, true);
      return true;
    }

    private FileAttachment[] assignFileDrop(string[] dropList, DocumentLog doc)
    {
      if (!new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) doc).CanBrowseAttach)
      {
        string str = "You do not have rights to attach files";
        int num = (int) Utils.Dialog((IWin32Window) this, doc == null ? str + "." : str + " to the '" + doc.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (FileAttachment[]) null;
      }
      using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
        return attachFilesDialog.AttachFiles(dropList, doc);
    }

    private bool assignPageDrop(PageImage[] pageList, ImageAttachment attachment, DocumentLog doc)
    {
      long ticks = DateTime.Now.Ticks;
      bool flag1 = false;
      bool flag2 = false;
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      DocumentLog logEntry = doc;
      if (attachment != null)
        logEntry = this.loanDataMgr.FileAttachments.GetLinkedDocument(attachment.ID);
      foreach (PageImage page in pageList)
      {
        DocumentLog linkedDocument = this.loanDataMgr.FileAttachments.GetLinkedDocument(page.Attachment.ID);
        if (linkedDocument != null)
          documentLogList.Add(linkedDocument);
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) linkedDocument);
        if (!folderAccessRights.CanSplitFiles)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to split the '" + page.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        flag1 = true;
        if (linkedDocument != logEntry)
        {
          if (!folderAccessRights.CanRemoveDocumentFiles)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to remove the pages from the '" + page.Attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          if (logEntry != null && !new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) logEntry).CanAttachUnassignedFiles)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + page.Attachment.Title + "' file to the '" + logEntry.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        else if (attachment != null)
        {
          if (!new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) logEntry).CanMergeFiles)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + page.Attachment.Title + "' file to the '" + attachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
          flag2 = true;
        }
      }
      if (attachment == null)
      {
        using (AddImageAttachmentDialog attachmentDialog = new AddImageAttachmentDialog(this.loanDataMgr, pageList, doc))
        {
          if (attachmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return false;
          attachment = attachmentDialog.Attachment;
        }
      }
      else
      {
        attachment.Pages.AddRange(pageList);
        logEntry?.MarkLastUpdated();
      }
      foreach (LogRecordBase logRecordBase in documentLogList)
        logRecordBase.MarkLastUpdated();
      DateTime now;
      TimeSpan timeSpan;
      if (flag1)
      {
        now = DateTime.Now;
        timeSpan = TimeSpan.FromTicks(now.Ticks - ticks);
        RemoteLogger.Write(TraceLevel.Info, "Split ImageAttachment: " + (object) timeSpan.TotalMilliseconds + " ms");
      }
      if (flag2)
      {
        now = DateTime.Now;
        timeSpan = TimeSpan.FromTicks(now.Ticks - ticks);
        RemoteLogger.Write(TraceLevel.Info, "Merged ImageAttachment: " + (object) timeSpan.TotalMilliseconds + " ms");
      }
      return true;
    }

    private bool assignPageDrop(CloudAttachment destAttachment, DocumentLog destDoc)
    {
      long ticks = DateTime.Now.Ticks;
      CloudAttachment cloudAttachment = (CloudAttachment) null;
      if (this.fileViewer.Attachments != null && this.fileViewer.Attachments.Length == 1)
        cloudAttachment = this.fileViewer.Attachments[0] as CloudAttachment;
      if (cloudAttachment == null)
      {
        Tracing.Log(AllFilesDialog.sw, TraceLevel.Warning, nameof (AllFilesDialog), "DragDrop: Unable to identify the source attachment");
        return false;
      }
      if (cloudAttachment == destAttachment)
      {
        Tracing.Log(AllFilesDialog.sw, TraceLevel.Warning, nameof (AllFilesDialog), "DragDrop: Cannot drag and drop to same attachment");
        return false;
      }
      DocumentLog linkedDocument = this.loanDataMgr.FileAttachments.GetLinkedDocument(cloudAttachment.ID);
      if (destAttachment != null)
        destDoc = this.loanDataMgr.FileAttachments.GetLinkedDocument(destAttachment.ID);
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) linkedDocument);
      if (!folderAccessRights.CanSplitFiles)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to split the '" + cloudAttachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (linkedDocument != destDoc)
      {
        if (!folderAccessRights.CanRemoveDocumentFiles)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to remove the pages from the '" + cloudAttachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (destDoc != null && !new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) destDoc).CanAttachUnassignedFiles)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to move pages from the '" + cloudAttachment.Title + "' file to the '" + destDoc.Title + "' document.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      else if (destAttachment != null && !new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) destDoc).CanMergeFiles)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add pages from the '" + cloudAttachment.Title + "' file to the '" + destAttachment.Title + "' file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      ViewerMessage2 viewerMessage = new ViewerMessage2()
      {
        payload = new ViewerMessagePayload()
      };
      if (destAttachment == null)
      {
        using (AddCloudAttachmentDialog attachmentDialog = new AddCloudAttachmentDialog(destDoc))
        {
          if (attachmentDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return false;
          viewerMessage.type = "CreateFile";
          viewerMessage.payload.title = attachmentDialog.Title;
        }
      }
      else
      {
        viewerMessage.type = "MergeFile";
        viewerMessage.payload.attachmentId = destAttachment.ID;
      }
      if (destDoc != null)
        viewerMessage.payload.documentId = destDoc.Guid;
      this.BeginInvoke((Delegate) (() =>
      {
        Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Calling BrowserDragDrop");
        this.fileViewer.BrowserDragDrop(viewerMessage);
      }));
      if (destAttachment == null)
        RemoteLogger.Write(TraceLevel.Info, "SplitCreate CloudAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      else
        RemoteLogger.Write(TraceLevel.Info, "SplitMerge CloudAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
      return true;
    }

    private void initEventHandlers()
    {
      this.FormClosed += new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing += new EventHandler(this.btnClose_Click);
      if (AutoAssignUtils.IsNGAutoAssignEnabled)
        this.loanDataMgr.OnLoanRefreshedFromServer += new EventHandler(this.onLoanRefreshedFromServer);
      this.loanDataMgr.LoanData.LogRecordAdded += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordChanged += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved += new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentAdded += new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentChanged += new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentRemoved += new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.BackgroundAttachmentUploaded += new FileAttachmentEventHandler(this.backgroundAttachmentUploaded);
      this.fileViewer.ViewerFileChanged += new FileAttachmentEventHandler(this.fileViewer_FileChanged);
      this.fileViewer.ViewerMergeFileSelected += new FileAttachmentEventHandler(this.fileViewer_MergeFileSelected);
    }

    private void releaseEventHandlers()
    {
      this.FormClosed -= new FormClosedEventHandler(this.onFormClosed);
      this.loanDataMgr.LoanClosing -= new EventHandler(this.btnClose_Click);
      if (AutoAssignUtils.IsNGAutoAssignEnabled)
        this.loanDataMgr.OnLoanRefreshedFromServer -= new EventHandler(this.onLoanRefreshedFromServer);
      this.loanDataMgr.LoanData.LogRecordAdded -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordChanged -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.LoanData.LogRecordRemoved -= new LogRecordEventHandler(this.logRecordChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentAdded -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentChanged -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.FileAttachmentRemoved -= new FileAttachmentEventHandler(this.fileAttachmentChanged);
      this.loanDataMgr.FileAttachments.BackgroundAttachmentUploaded -= new FileAttachmentEventHandler(this.backgroundAttachmentUploaded);
      this.fileViewer.ViewerFileChanged -= new FileAttachmentEventHandler(this.fileViewer_FileChanged);
      this.fileViewer.ViewerMergeFileSelected -= new FileAttachmentEventHandler(this.fileViewer_MergeFileSelected);
    }

    private void onLoanRefreshedFromServer(object sender, EventArgs e)
    {
      this.gvDocuments.Items.Clear();
      this.initUnassignedList();
      this.initDocumentList();
      this.initializeStackingOrder();
      this.loadUnassignedList();
      this.loadDocumentList();
    }

    private void onFormClosed(object sender, FormClosedEventArgs e) => this.releaseEventHandlers();

    private void backgroundAttachmentUploaded(object source, FileAttachmentEventArgs e)
    {
      Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Checking InvokeRequired For BackgroundAttachmentUploaded");
      if (this.InvokeRequired)
      {
        FileAttachmentEventHandler method = new FileAttachmentEventHandler(this.backgroundAttachmentUploaded);
        Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Calling BeginInvoke For BackgroundAttachmentUploaded");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else if (this.loanDataMgr.FileAttachments.GetLinkedDocument(e.File.ID) == null)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUnassigned.Items)
        {
          if (((FileAttachment) gvItem.Tag).ID == e.File.ID)
            gvItem.Tag = (object) e.File;
        }
        if (this == Form.ActiveForm)
        {
          this.loadUnassignedList();
          bool flag = false;
          foreach (GVItem selectedItem in this.gvUnassigned.SelectedItems)
          {
            if (selectedItem.Tag == e.File)
              flag = true;
          }
          if (!flag)
            return;
          this.showUnassignedFiles();
        }
        else
          this.refreshUnassignedFiles = true;
      }
      else if (this == Form.ActiveForm)
      {
        this.loadDocumentList();
        if (this.gvDocuments.SelectedItems.Count <= 0)
          return;
        GVItem selectedItem = this.gvDocuments.SelectedItems[0];
        if (selectedItem.Tag != e.File && !selectedItem.GroupItems.ContainsTag((object) e.File))
          return;
        this.showDocumentFiles();
      }
      else
        this.refreshDocuments = true;
    }

    private void fileAttachmentChanged(object source, FileAttachmentEventArgs e)
    {
      Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Checking InvokeRequired For FileAttachmentChanged");
      if (this.InvokeRequired)
      {
        FileAttachmentEventHandler method = new FileAttachmentEventHandler(this.fileAttachmentChanged);
        Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Calling BeginInvoke For FileAttachmentChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        bool flag1 = false;
        bool flag2 = false;
        if (this.loanDataMgr.FileAttachments.GetLinkedDocument(e.File.ID) == null)
        {
          flag1 = true;
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
          {
            if (gvItem.GroupItems.ContainsTag((object) e.File))
              flag2 = true;
          }
        }
        else
        {
          if (this.gvUnassigned.Items.ContainsTag((object) e.File))
            flag1 = true;
          flag2 = true;
        }
        if (flag1)
        {
          if (this == Form.ActiveForm)
            this.loadUnassignedList();
          else
            this.refreshUnassignedFiles = true;
        }
        if (!flag2)
          return;
        if (this == Form.ActiveForm)
          this.loadDocumentList();
        else
          this.refreshDocuments = true;
      }
    }

    private void fileViewer_LoadAttachments(object source, EventArgs e)
    {
      FileAttachment[] attachments = this.fileViewer.Attachments;
      DocumentLog linkedDocument = this.loanDataMgr.FileAttachments.GetLinkedDocument(attachments[0].ID);
      if (linkedDocument == null)
      {
        bool flag = this.gvUnassigned.SelectedItems.Count != attachments.Length;
        if (!flag)
        {
          for (int index = 0; index < attachments.Length; ++index)
          {
            if (this.gvUnassigned.SelectedItems[index].Tag != attachments[index])
              flag = true;
          }
          if (!flag)
            return;
        }
        this.showUnassignedFiles(attachments);
      }
      else if (attachments.Length == 1)
      {
        if (this.gvDocuments.SelectedItems.Count == 1 && this.gvDocuments.SelectedItems[0].Tag == attachments[0])
          return;
        this.showDocumentFiles(linkedDocument, attachments[0]);
      }
      else
      {
        if (this.gvDocuments.SelectedItems.Count == 1 && this.gvDocuments.SelectedItems[0].Tag == linkedDocument)
          return;
        this.showDocumentFiles(linkedDocument);
      }
    }

    private void fileViewer_FileChanged(object source, FileAttachmentEventArgs e)
    {
      if (this == Form.ActiveForm)
      {
        this.loadUnassignedList();
        this.loadDocumentList();
        if (e.File == null)
          return;
        DocumentLog linkedDocument = this.loanDataMgr.FileAttachments.GetLinkedDocument(e.File.ID);
        if (linkedDocument == null)
          this.showUnassignedFiles(e.File);
        else
          this.showDocumentFiles(linkedDocument, e.File);
      }
      else
      {
        this.refreshUnassignedFiles = true;
        this.refreshDocuments = true;
      }
    }

    private void fileViewer_MergeFileSelected(object source, FileAttachmentEventArgs e)
    {
      if (this != Form.ActiveForm || e.File == null)
        return;
      this.clearCurrentFileIndicators();
      if (this.loanDataMgr.FileAttachments.GetLinkedDocument(e.File.ID) == null)
      {
        this.gvUnassigned.Items.GetItemByTag((object) e.File).SubItems[1].ImageIndex = 0;
      }
      else
      {
        GVItem selectedItem = this.gvDocuments.SelectedItems[0];
        if (selectedItem == null)
          return;
        foreach (GVItem groupItem in (IEnumerable<GVItem>) selectedItem.GroupItems)
        {
          if ((groupItem.Tag as FileAttachment).ID == e.File.ID)
            groupItem.SubItems[1].ImageIndex = 0;
        }
      }
    }

    private void logRecordChanged(object source, LogRecordEventArgs e)
    {
      Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Checking InvokeRequired For LogRecordChanged");
      if (this.InvokeRequired)
      {
        LogRecordEventHandler method = new LogRecordEventHandler(this.logRecordChanged);
        Tracing.Log(AllFilesDialog.sw, TraceLevel.Verbose, nameof (AllFilesDialog), "Calling BeginInvoke For LogRecordChanged");
        this.BeginInvoke((Delegate) method, source, (object) e);
      }
      else
      {
        if (!(e.LogRecord is DocumentLog))
          return;
        if (this == Form.ActiveForm)
        {
          this.loadUnassignedList();
          this.loadDocumentList();
        }
        else
        {
          this.refreshUnassignedFiles = true;
          this.refreshDocuments = true;
        }
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void clearCurrentFileIndicators()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvUnassigned.Items)
        gvItem.SubItems[1].ImageIndex = -1;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        foreach (GVItem groupItem in (IEnumerable<GVItem>) gvItem.GroupItems)
          groupItem.SubItems[1].ImageIndex = -1;
      }
    }

    private void AllFilesDialog_Resize(object sender, EventArgs e)
    {
      this.gcUnassigned.Height = (this.pnlLeft.ClientSize.Height - this.csUnassigned.Height) / 2;
    }

    private void AllFilesDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.session.Application.DisplayHelp(this.helpLink.HelpTag);
    }

    private void AllFilesDialog_Activated(object sender, EventArgs e)
    {
      if (this.refreshUnassignedFiles)
      {
        int count = this.gvUnassigned.SelectedItems.Count;
        this.loadUnassignedList();
        if (count > 0)
          this.showUnassignedFiles();
        this.refreshUnassignedFiles = false;
      }
      if (!this.refreshDocuments)
        return;
      int count1 = this.gvDocuments.SelectedItems.Count;
      this.loadDocumentList();
      if (count1 > 0)
        this.showDocumentFiles();
      this.refreshDocuments = false;
    }

    private void AllFilesDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.fileViewer.CanCloseViewer())
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.helpLink.Font.Dispose();
        this.fileViewer.Font.Dispose();
        this.Font.Dispose();
        this.DisposeCustomControl((Control) this.pnlRight);
        this.DisposeCustomControl((Control) this.csLeft);
        this.DisposeCustomControl((Control) this.pnlLeft);
        this.DisposeCustomControl((Control) this.pnlClose);
      }
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void DisposeCustomControl(Control myCtrl)
    {
      if (myCtrl.Controls != null && myCtrl.Controls.Count > 0)
      {
        foreach (Component control in (ArrangedElementCollection) myCtrl.Controls)
          control.Dispose();
      }
      myCtrl.Controls.Clear();
      myCtrl.Dispose();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AllFilesDialog));
      this.tooltip = new ToolTip(this.components);
      this.btnEditDocument = new StandardIconButton();
      this.btnAddDocument = new StandardIconButton();
      this.btnAutoAssign = new Button();
      this.btnDeleteFile = new StandardIconButton();
      this.btnMergeFiles = new IconButton();
      this.btnAttachForms = new IconButton();
      this.btnAttachScan = new IconButton();
      this.btnAttachBrowse = new IconButton();
      this.btnSuggestTraining = new Button();
      this.pnlLeft = new Panel();
      this.gcAssigned = new GroupContainer();
      this.pnlStackingOrder = new GradientPanel();
      this.cboStackingOrder = new ComboBox();
      this.label2 = new Label();
      this.pnlToolbar2 = new FlowLayoutPanel();
      this.trainSeparator = new VerticalSeparator();
      this.gvDocuments = new GridView();
      this.imageList1 = new ImageList(this.components);
      this.csUnassigned = new CollapsibleSplitter();
      this.gcUnassigned = new GroupContainer();
      this.pnlToolbar1 = new FlowLayoutPanel();
      this.separator2 = new VerticalSeparator();
      this.separator1 = new VerticalSeparator();
      this.gvUnassigned = new GridView();
      this.pnlDragDrop = new BorderPanel();
      this.lblDragDrop = new Label();
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnClose = new Button();
      this.pnlRight = new BorderPanel();
      this.fileViewer = new FileAttachmentViewerControl();
      this.csLeft = new CollapsibleSplitter();
      ((ISupportInitialize) this.btnEditDocument).BeginInit();
      ((ISupportInitialize) this.btnAddDocument).BeginInit();
      ((ISupportInitialize) this.btnDeleteFile).BeginInit();
      ((ISupportInitialize) this.btnMergeFiles).BeginInit();
      ((ISupportInitialize) this.btnAttachForms).BeginInit();
      ((ISupportInitialize) this.btnAttachScan).BeginInit();
      ((ISupportInitialize) this.btnAttachBrowse).BeginInit();
      this.pnlLeft.SuspendLayout();
      this.gcAssigned.SuspendLayout();
      this.pnlStackingOrder.SuspendLayout();
      this.pnlToolbar2.SuspendLayout();
      this.gcUnassigned.SuspendLayout();
      this.pnlToolbar1.SuspendLayout();
      this.pnlDragDrop.SuspendLayout();
      this.pnlClose.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.SuspendLayout();
      this.btnEditDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditDocument.BackColor = Color.Transparent;
      this.btnEditDocument.Enabled = false;
      this.btnEditDocument.Location = new Point(131, 3);
      this.btnEditDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnEditDocument.MouseDownImage = (Image) null;
      this.btnEditDocument.Name = "btnEditDocument";
      this.btnEditDocument.Size = new Size(16, 16);
      this.btnEditDocument.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditDocument.TabIndex = 13;
      this.btnEditDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEditDocument, "Edit Document");
      this.btnEditDocument.Click += new EventHandler(this.btnEditDocument_Click);
      this.btnAddDocument.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddDocument.BackColor = Color.Transparent;
      this.btnAddDocument.Location = new Point(111, 3);
      this.btnAddDocument.Margin = new Padding(4, 3, 0, 3);
      this.btnAddDocument.MouseDownImage = (Image) null;
      this.btnAddDocument.Name = "btnAddDocument";
      this.btnAddDocument.Size = new Size(16, 16);
      this.btnAddDocument.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocument.TabIndex = 12;
      this.btnAddDocument.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddDocument, "New Document");
      this.btnAddDocument.Click += new EventHandler(this.btnAddDocument_Click);
      this.btnAutoAssign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAutoAssign.BackColor = SystemColors.Control;
      this.btnAutoAssign.Enabled = false;
      this.btnAutoAssign.Location = new Point(184, 0);
      this.btnAutoAssign.Margin = new Padding(0);
      this.btnAutoAssign.Name = "btnAutoAssign";
      this.btnAutoAssign.Size = new Size(76, 22);
      this.btnAutoAssign.TabIndex = 5;
      this.btnAutoAssign.TabStop = false;
      this.btnAutoAssign.Text = "Auto Assign";
      this.tooltip.SetToolTip((Control) this.btnAutoAssign, "Automatically assigns files to their corresponding documents");
      this.btnAutoAssign.UseVisualStyleBackColor = true;
      this.btnAutoAssign.Click += new EventHandler(this.btnAutoAssign_Click);
      this.btnDeleteFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteFile.BackColor = Color.Transparent;
      this.btnDeleteFile.Enabled = false;
      this.btnDeleteFile.Location = new Point(159, 3);
      this.btnDeleteFile.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteFile.MouseDownImage = (Image) null;
      this.btnDeleteFile.Name = "btnDeleteFile";
      this.btnDeleteFile.Size = new Size(16, 16);
      this.btnDeleteFile.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteFile.TabIndex = 11;
      this.btnDeleteFile.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteFile, "Delete File");
      this.btnDeleteFile.Click += new EventHandler(this.btnDeleteFile_Click);
      this.btnMergeFiles.BackColor = Color.Transparent;
      this.btnMergeFiles.DisabledImage = (Image) Resources.merge_disabled;
      this.btnMergeFiles.Enabled = false;
      this.btnMergeFiles.Image = (Image) Resources.merge;
      this.btnMergeFiles.Location = new Point(139, 3);
      this.btnMergeFiles.Margin = new Padding(4, 3, 0, 3);
      this.btnMergeFiles.MouseDownImage = (Image) null;
      this.btnMergeFiles.MouseOverImage = (Image) Resources.merge_over;
      this.btnMergeFiles.Name = "btnMergeFiles";
      this.btnMergeFiles.Size = new Size(16, 16);
      this.btnMergeFiles.TabIndex = 52;
      this.btnMergeFiles.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMergeFiles, "Merge Files");
      this.btnMergeFiles.Click += new EventHandler(this.btnMergeFiles_Click);
      this.btnAttachForms.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAttachForms.BackColor = Color.Transparent;
      this.btnAttachForms.DisabledImage = (Image) Resources.attach_forms_disabled;
      this.btnAttachForms.Image = (Image) Resources.attach_forms;
      this.btnAttachForms.Location = new Point(113, 3);
      this.btnAttachForms.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachForms.MouseDownImage = (Image) null;
      this.btnAttachForms.MouseOverImage = (Image) Resources.attach_forms_over;
      this.btnAttachForms.Name = "btnAttachForms";
      this.btnAttachForms.Size = new Size(16, 16);
      this.btnAttachForms.TabIndex = 16;
      this.btnAttachForms.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachForms, "Attach Encompass Forms");
      this.btnAttachForms.Click += new EventHandler(this.btnAttachForms_Click);
      this.btnAttachScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAttachScan.BackColor = Color.Transparent;
      this.btnAttachScan.DisabledImage = (Image) Resources.attach_scan_disabled;
      this.btnAttachScan.Image = (Image) Resources.attach_scan;
      this.btnAttachScan.Location = new Point(93, 3);
      this.btnAttachScan.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachScan.MouseDownImage = (Image) null;
      this.btnAttachScan.MouseOverImage = (Image) Resources.attach_scan_over;
      this.btnAttachScan.Name = "btnAttachScan";
      this.btnAttachScan.Size = new Size(16, 16);
      this.btnAttachScan.TabIndex = 17;
      this.btnAttachScan.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachScan, "Scan and Attach");
      this.btnAttachScan.Click += new EventHandler(this.btnAttachScan_Click);
      this.btnAttachBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAttachBrowse.BackColor = Color.Transparent;
      this.btnAttachBrowse.DisabledImage = (Image) Resources.attach_browse_disabled;
      this.btnAttachBrowse.Image = (Image) Resources.attach_browse;
      this.btnAttachBrowse.Location = new Point(73, 3);
      this.btnAttachBrowse.Margin = new Padding(4, 3, 0, 3);
      this.btnAttachBrowse.MouseDownImage = (Image) null;
      this.btnAttachBrowse.MouseOverImage = (Image) Resources.attach_browse_over;
      this.btnAttachBrowse.Name = "btnAttachBrowse";
      this.btnAttachBrowse.Size = new Size(16, 16);
      this.btnAttachBrowse.TabIndex = 18;
      this.btnAttachBrowse.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAttachBrowse, "Browse and Attach");
      this.btnAttachBrowse.Click += new EventHandler(this.btnAttachBrowse_Click);
      this.btnSuggestTraining.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSuggestTraining.BackColor = SystemColors.Control;
      this.btnSuggestTraining.Enabled = false;
      this.btnSuggestTraining.Location = new Point(38, 0);
      this.btnSuggestTraining.Margin = new Padding(0);
      this.btnSuggestTraining.Name = "btnSuggestTraining";
      this.btnSuggestTraining.Size = new Size(63, 22);
      this.btnSuggestTraining.TabIndex = 16;
      this.btnSuggestTraining.TabStop = false;
      this.btnSuggestTraining.Text = "Train";
      this.tooltip.SetToolTip((Control) this.btnSuggestTraining, "Suggest that this file be trained as a template for automatic assignment.");
      this.btnSuggestTraining.UseVisualStyleBackColor = true;
      this.btnSuggestTraining.Visible = false;
      this.btnSuggestTraining.Click += new EventHandler(this.btnSuggestTraining_Click);
      this.pnlLeft.Controls.Add((Control) this.gcAssigned);
      this.pnlLeft.Controls.Add((Control) this.csUnassigned);
      this.pnlLeft.Controls.Add((Control) this.gcUnassigned);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(356, 521);
      this.pnlLeft.TabIndex = 0;
      this.gcAssigned.Controls.Add((Control) this.pnlStackingOrder);
      this.gcAssigned.Controls.Add((Control) this.pnlToolbar2);
      this.gcAssigned.Controls.Add((Control) this.gvDocuments);
      this.gcAssigned.Dock = DockStyle.Fill;
      this.gcAssigned.HeaderForeColor = SystemColors.ControlText;
      this.gcAssigned.Location = new Point(0, 263);
      this.gcAssigned.Name = "gcAssigned";
      this.gcAssigned.Size = new Size(356, 258);
      this.gcAssigned.TabIndex = 10;
      this.gcAssigned.Text = "Documents";
      this.pnlStackingOrder.Borders = AnchorStyles.Bottom;
      this.pnlStackingOrder.Controls.Add((Control) this.cboStackingOrder);
      this.pnlStackingOrder.Controls.Add((Control) this.label2);
      this.pnlStackingOrder.Dock = DockStyle.Top;
      this.pnlStackingOrder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlStackingOrder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlStackingOrder.Location = new Point(1, 26);
      this.pnlStackingOrder.Name = "pnlStackingOrder";
      this.pnlStackingOrder.Size = new Size(354, 25);
      this.pnlStackingOrder.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlStackingOrder.TabIndex = 13;
      this.cboStackingOrder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboStackingOrder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStackingOrder.FormattingEnabled = true;
      this.cboStackingOrder.Location = new Point(92, 1);
      this.cboStackingOrder.Name = "cboStackingOrder";
      this.cboStackingOrder.Size = new Size((int) byte.MaxValue, 22);
      this.cboStackingOrder.TabIndex = 1;
      this.cboStackingOrder.DropDown += new EventHandler(this.cboStackingOrder_DropDown);
      this.cboStackingOrder.SelectedIndexChanged += new EventHandler(this.cboStackingOrder_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(7, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(79, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Stacking Order";
      this.pnlToolbar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar2.BackColor = Color.Transparent;
      this.pnlToolbar2.Controls.Add((Control) this.btnEditDocument);
      this.pnlToolbar2.Controls.Add((Control) this.btnAddDocument);
      this.pnlToolbar2.Controls.Add((Control) this.trainSeparator);
      this.pnlToolbar2.Controls.Add((Control) this.btnSuggestTraining);
      this.pnlToolbar2.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar2.Location = new Point(205, 2);
      this.pnlToolbar2.Name = "pnlToolbar2";
      this.pnlToolbar2.Size = new Size(147, 22);
      this.pnlToolbar2.TabIndex = 11;
      this.trainSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.trainSeparator.Location = new Point(105, 3);
      this.trainSeparator.Margin = new Padding(4, 3, 0, 3);
      this.trainSeparator.MaximumSize = new Size(2, 16);
      this.trainSeparator.MinimumSize = new Size(2, 16);
      this.trainSeparator.Name = "trainSeparator";
      this.trainSeparator.Size = new Size(2, 16);
      this.trainSeparator.TabIndex = 15;
      this.trainSeparator.TabStop = false;
      this.trainSeparator.Visible = false;
      this.gvDocuments.AllowDrop = true;
      this.gvDocuments.AllowMultiselect = false;
      this.gvDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.ImageList = this.imageList1;
      this.gvDocuments.ItemGrouping = true;
      this.gvDocuments.Location = new Point(1, 55);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(354, 202);
      this.gvDocuments.SortingType = SortingType.AlphaNumeric;
      this.gvDocuments.TabIndex = 12;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.UseCompatibleEditingBehavior = true;
      this.gvDocuments.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvDocuments_BeforeSelectedIndexCommitted);
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.gvDocuments.SelectedIndexCommitted += new EventHandler(this.gvDocuments_SelectedIndexCommitted);
      this.gvDocuments.SortItems += new GVColumnSortEventHandler(this.gvDocuments_SortItems);
      this.gvDocuments.ItemDoubleClick += new GVItemEventHandler(this.gvDocuments_ItemDoubleClick);
      this.gvDocuments.EditorOpening += new GVSubItemEditingEventHandler(this.gvDocuments_EditorOpening);
      this.gvDocuments.EditorClosing += new GVSubItemEditingEventHandler(this.gvDocuments_EditorClosing);
      this.gvDocuments.DragDrop += new DragEventHandler(this.gvDocuments_DragDrop);
      this.gvDocuments.DragOver += new DragEventHandler(this.gvDocuments_DragOver);
      this.gvDocuments.MouseDown += new MouseEventHandler(this.gvDocuments_MouseDown);
      this.gvDocuments.MouseMove += new MouseEventHandler(this.gvDocuments_MouseMove);
      this.gvDocuments.MouseUp += new MouseEventHandler(this.gvDocuments_MouseUp);
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "move-previous-clean.png");
      this.csUnassigned.AnimationDelay = 20;
      this.csUnassigned.AnimationStep = 20;
      this.csUnassigned.BorderStyle3D = Border3DStyle.Flat;
      this.csUnassigned.ControlToHide = (Control) this.gcUnassigned;
      this.csUnassigned.Dock = DockStyle.Top;
      this.csUnassigned.ExpandParentForm = false;
      this.csUnassigned.Location = new Point(0, 256);
      this.csUnassigned.Name = "csLeft";
      this.csUnassigned.TabIndex = 9;
      this.csUnassigned.TabStop = false;
      this.csUnassigned.UseAnimations = false;
      this.csUnassigned.VisualStyle = VisualStyles.Encompass;
      this.gcUnassigned.Controls.Add((Control) this.pnlToolbar1);
      this.gcUnassigned.Controls.Add((Control) this.gvUnassigned);
      this.gcUnassigned.Controls.Add((Control) this.pnlDragDrop);
      this.gcUnassigned.Dock = DockStyle.Top;
      this.gcUnassigned.HeaderForeColor = SystemColors.ControlText;
      this.gcUnassigned.Location = new Point(0, 0);
      this.gcUnassigned.Name = "gcUnassigned";
      this.gcUnassigned.Size = new Size(356, 256);
      this.gcUnassigned.TabIndex = 1;
      this.gcUnassigned.Text = "Unassigned";
      this.pnlToolbar1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar1.BackColor = Color.Transparent;
      this.pnlToolbar1.Controls.Add((Control) this.btnAutoAssign);
      this.pnlToolbar1.Controls.Add((Control) this.separator2);
      this.pnlToolbar1.Controls.Add((Control) this.btnDeleteFile);
      this.pnlToolbar1.Controls.Add((Control) this.btnMergeFiles);
      this.pnlToolbar1.Controls.Add((Control) this.separator1);
      this.pnlToolbar1.Controls.Add((Control) this.btnAttachForms);
      this.pnlToolbar1.Controls.Add((Control) this.btnAttachScan);
      this.pnlToolbar1.Controls.Add((Control) this.btnAttachBrowse);
      this.pnlToolbar1.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar1.Location = new Point(92, 2);
      this.pnlToolbar1.Name = "pnlToolbar1";
      this.pnlToolbar1.Size = new Size(260, 22);
      this.pnlToolbar1.TabIndex = 2;
      this.separator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator2.Location = new Point(179, 3);
      this.separator2.Margin = new Padding(4, 3, 3, 3);
      this.separator2.MaximumSize = new Size(2, 16);
      this.separator2.MinimumSize = new Size(2, 16);
      this.separator2.Name = "separator2";
      this.separator2.Size = new Size(2, 16);
      this.separator2.TabIndex = 4;
      this.separator2.TabStop = false;
      this.separator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator1.Location = new Point(133, 3);
      this.separator1.Margin = new Padding(4, 3, 0, 3);
      this.separator1.MaximumSize = new Size(2, 16);
      this.separator1.MinimumSize = new Size(2, 16);
      this.separator1.Name = "separator1";
      this.separator1.Size = new Size(2, 16);
      this.separator1.TabIndex = 3;
      this.separator1.TabStop = false;
      this.gvUnassigned.AllowDrop = true;
      this.gvUnassigned.BorderStyle = BorderStyle.None;
      this.gvUnassigned.ClearSelectionsOnEmptyRowClick = false;
      this.gvUnassigned.Dock = DockStyle.Fill;
      this.gvUnassigned.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvUnassigned.ImageList = this.imageList1;
      this.gvUnassigned.Location = new Point(1, 26);
      this.gvUnassigned.Name = "gvUnassigned";
      this.gvUnassigned.Size = new Size(354, 202);
      this.gvUnassigned.TabIndex = 6;
      this.gvUnassigned.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvUnassigned.UseCompatibleEditingBehavior = true;
      this.gvUnassigned.BeforeSelectedIndexCommitted += new CancelEventHandler(this.gvUnassigned_BeforeSelectedIndexCommitted);
      this.gvUnassigned.SelectedIndexChanged += new EventHandler(this.gvUnassigned_SelectedIndexChanged);
      this.gvUnassigned.SelectedIndexCommitted += new EventHandler(this.gvUnassigned_SelectedIndexCommitted);
      this.gvUnassigned.ItemDoubleClick += new GVItemEventHandler(this.gvUnassigned_ItemDoubleClick);
      this.gvUnassigned.EditorClosing += new GVSubItemEditingEventHandler(this.gvUnassigned_EditorClosing);
      this.gvUnassigned.DragDrop += new DragEventHandler(this.gvUnassigned_DragDrop);
      this.gvUnassigned.DragOver += new DragEventHandler(this.gvUnassigned_DragOver);
      this.gvUnassigned.MouseDown += new MouseEventHandler(this.gvUnassigned_MouseDown);
      this.gvUnassigned.MouseMove += new MouseEventHandler(this.gvUnassigned_MouseMove);
      this.gvUnassigned.MouseUp += new MouseEventHandler(this.gvUnassigned_MouseUp);
      this.pnlDragDrop.Borders = AnchorStyles.Top;
      this.pnlDragDrop.Controls.Add((Control) this.lblDragDrop);
      this.pnlDragDrop.Dock = DockStyle.Bottom;
      this.pnlDragDrop.Location = new Point(1, 228);
      this.pnlDragDrop.Name = "pnlDragDrop";
      this.pnlDragDrop.Size = new Size(354, 27);
      this.pnlDragDrop.TabIndex = 7;
      this.lblDragDrop.BackColor = Color.WhiteSmoke;
      this.lblDragDrop.Dock = DockStyle.Fill;
      this.lblDragDrop.Location = new Point(0, 1);
      this.lblDragDrop.Name = "lblDragDrop";
      this.lblDragDrop.Size = new Size(354, 26);
      this.lblDragDrop.TabIndex = 8;
      this.lblDragDrop.Text = "Select a file above and drag it to a document below";
      this.lblDragDrop.TextAlign = ContentAlignment.MiddleCenter;
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnClose);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 521);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(774, 40);
      this.pnlClose.TabIndex = 16;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Manage Files";
      this.helpLink.Location = new Point(8, 11);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 18;
      this.helpLink.TabStop = false;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(691, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 17;
      this.btnClose.TabStop = false;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlRight.Controls.Add((Control) this.fileViewer);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(363, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(411, 521);
      this.pnlRight.TabIndex = 14;
      this.fileViewer.Dock = DockStyle.Fill;
      this.fileViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fileViewer.Location = new Point(1, 1);
      this.fileViewer.Name = "fileViewer";
      this.fileViewer.Size = new Size(409, 519);
      this.fileViewer.TabIndex = 15;
      this.fileViewer.LoadAttachments += new EventHandler(this.fileViewer_LoadAttachments);
      this.csLeft.AnimationDelay = 20;
      this.csLeft.AnimationStep = 20;
      this.csLeft.BorderStyle3D = Border3DStyle.Flat;
      this.csLeft.ControlToHide = (Control) this.pnlLeft;
      this.csLeft.ExpandParentForm = false;
      this.csLeft.Location = new Point(356, 0);
      this.csLeft.Name = "csLeft";
      this.csLeft.TabIndex = 13;
      this.csLeft.TabStop = false;
      this.csLeft.UseAnimations = false;
      this.csLeft.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(774, 561);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csLeft);
      this.Controls.Add((Control) this.pnlLeft);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.KeyPreview = true;
      this.Name = nameof (AllFilesDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "File Manager";
      this.Activated += new EventHandler(this.AllFilesDialog_Activated);
      this.FormClosing += new FormClosingEventHandler(this.AllFilesDialog_FormClosing);
      this.KeyDown += new KeyEventHandler(this.AllFilesDialog_KeyDown);
      this.Resize += new EventHandler(this.AllFilesDialog_Resize);
      ((ISupportInitialize) this.btnEditDocument).EndInit();
      ((ISupportInitialize) this.btnAddDocument).EndInit();
      ((ISupportInitialize) this.btnDeleteFile).EndInit();
      ((ISupportInitialize) this.btnMergeFiles).EndInit();
      ((ISupportInitialize) this.btnAttachForms).EndInit();
      ((ISupportInitialize) this.btnAttachScan).EndInit();
      ((ISupportInitialize) this.btnAttachBrowse).EndInit();
      this.pnlLeft.ResumeLayout(false);
      this.gcAssigned.ResumeLayout(false);
      this.pnlStackingOrder.ResumeLayout(false);
      this.pnlStackingOrder.PerformLayout();
      this.pnlToolbar2.ResumeLayout(false);
      this.gcUnassigned.ResumeLayout(false);
      this.pnlToolbar1.ResumeLayout(false);
      this.pnlDragDrop.ResumeLayout(false);
      this.pnlClose.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
