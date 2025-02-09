// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.EditFileDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocumentConverter;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class EditFileDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private NativeAttachment originalFile;
    private FileAttachment editedFile;
    private string[] pdfPageList;
    private List<string> changeList = new List<string>();
    private SDCDocument sdcDocumentCopy;
    private SDCHelper sdcHelper;
    private SDCMapper sdcMapper;
    private const string className = "EditFileDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private IContainer components;
    private BorderPanel pnlRight;
    private ToolTip tooltip;
    private StandardIconButton btnMovePageDown;
    private StandardIconButton btnDeletePage;
    private FlowLayoutPanel pnlPageToolbar;
    private StandardIconButton btnMovePageUp;
    private Button btnSave;
    private Button btnCancel;
    private Panel pnlFooter;
    private CollapsibleSplitter csPages;
    private GroupContainer gcPages;
    private GridView gvPages;
    private PageViewerControl pageViewer;
    private IconButton btnRotatePage;

    public EditFileDialog(LoanDataMgr loanDataMgr, NativeAttachment file, string[] pdfPageList)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.originalFile = file;
      this.pdfPageList = pdfPageList;
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        this.sdcHelper = new SDCHelper(loanDataMgr);
        this.sdcMapper = new SDCMapper();
        this.sdcDocumentCopy = file.CurrentSDCDocument == null ? Utils.DeepClone<SDCDocument>(this.originalFile.OriginalSDCDocument) : Utils.DeepClone<SDCDocument>(this.originalFile.CurrentSDCDocument);
      }
      this.applySecurity();
      this.loadPageList();
    }

    public FileAttachment File => this.editedFile;

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
      this.btnMovePageUp.Visible = folderAccessRights.CanEditFile;
      this.btnMovePageDown.Visible = folderAccessRights.CanEditFile;
      this.btnRotatePage.Visible = folderAccessRights.CanEditFile;
      this.btnDeletePage.Visible = folderAccessRights.CanDeletePageFromFile;
      this.btnSave.Visible = folderAccessRights.CanEditFile;
      if (folderAccessRights.CanEditFile)
        return;
      this.btnCancel.Text = "Close";
    }

    private void loadPageList()
    {
      for (int index = 0; index < this.pdfPageList.Length; ++index)
        this.gvPages.Items.Add(new GVItem()
        {
          Text = "Page " + Convert.ToString(index + 1),
          Tag = (object) this.pdfPageList[index],
          Selected = true
        });
      this.showPageFiles();
    }

    private void gvPages_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count1 = this.gvPages.Items.Count;
      int count2 = this.gvPages.SelectedItems.Count;
      this.btnRotatePage.Enabled = count2 > 0;
      this.btnMovePageUp.Enabled = count2 > 0 && count1 > count2;
      this.btnMovePageDown.Enabled = count2 > 0 && count1 > count2;
      this.btnDeletePage.Enabled = count2 > 0 && count1 > count2;
    }

    private void gvPages_SelectedIndexCommitted(object sender, EventArgs e) => this.showPageFiles();

    private void showPageFiles()
    {
      List<string> stringList = new List<string>();
      foreach (GVItem selectedItem in this.gvPages.SelectedItems)
        stringList.Add((string) selectedItem.Tag);
      if (stringList.Count > 0)
        this.pageViewer.LoadFiles(stringList.ToArray());
      else
        this.pageViewer.CloseFiles();
    }

    private void btnRotatePage_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvPages.SelectedItems)
      {
        using (PdfEditor pdfEditor = new PdfEditor((string) selectedItem.Tag))
        {
          selectedItem.Tag = (object) pdfEditor.Rotate(90);
          if (this.loanDataMgr.UseSkyDriveClassic)
            this.sdcMapper.PageRotationMapper(this.sdcDocumentCopy, this.sdcDocumentCopy.Pages[selectedItem.Index].Id, 90);
        }
      }
      this.showPageFiles();
      this.btnSave.Enabled = true;
    }

    private void btnMovePageUp_Click(object sender, EventArgs e)
    {
      GVItem gvItem1 = (GVItem) null;
      for (int nItemIndex = 0; nItemIndex < this.gvPages.Items.Count; ++nItemIndex)
      {
        GVItem gvItem2 = this.gvPages.Items[nItemIndex];
        if (!gvItem2.Selected)
          gvItem1 = gvItem2;
        else if (gvItem1 != null)
        {
          this.gvPages.Items.Remove(gvItem2);
          if (this.loanDataMgr.UseSkyDriveClassic)
          {
            Tracing.Log(EditFileDialog.sw, TraceLevel.Verbose, nameof (EditFileDialog), "SkyDriveClassic: Calling UpdatePageOrder");
            this.sdcHelper.UpdatePageOrder(this.sdcDocumentCopy, gvItem2.Index, gvItem1.DisplayIndex);
          }
          this.gvPages.Items.Insert(gvItem1.DisplayIndex, gvItem2);
          gvItem2.Selected = true;
          this.btnSave.Enabled = true;
        }
      }
    }

    private void btnMovePageDown_Click(object sender, EventArgs e)
    {
      GVItem gvItem1 = (GVItem) null;
      for (int nItemIndex = this.gvPages.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        GVItem gvItem2 = this.gvPages.Items[nItemIndex];
        if (!gvItem2.Selected)
          gvItem1 = gvItem2;
        else if (gvItem1 != null)
        {
          this.gvPages.Items.Remove(gvItem2);
          if (this.loanDataMgr.UseSkyDriveClassic)
          {
            Tracing.Log(EditFileDialog.sw, TraceLevel.Verbose, nameof (EditFileDialog), "SkyDriveClassic: Calling UpdatePageOrder");
            this.sdcHelper.UpdatePageOrder(this.sdcDocumentCopy, gvItem2.Index, gvItem1.DisplayIndex + 1);
          }
          this.gvPages.Items.Insert(gvItem1.DisplayIndex + 1, gvItem2);
          gvItem2.Selected = true;
          this.btnSave.Enabled = true;
        }
      }
    }

    private void btnDeletePage_Click(object sender, EventArgs e)
    {
      List<GVItem> source = new List<GVItem>();
      List<int> intList = new List<int>();
      foreach (GVItem selectedItem in this.gvPages.SelectedItems)
        source.Add(selectedItem);
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        intList.AddRange(source.Cast<GVItem>().Select(item => new
        {
          item = item,
          pageId = this.sdcDocumentCopy.Pages[item.Index].Id
        }).Select(_param1 => _param1.pageId));
        Tracing.Log(EditFileDialog.sw, TraceLevel.Verbose, nameof (EditFileDialog), "SkyDriveClassic: Calling DeletePages for deleting pageIDs-(" + string.Join<int>(",", (IEnumerable<int>) intList) + ").");
        this.sdcHelper.DeletePages(this.sdcDocumentCopy, intList);
      }
      foreach (GVItem gvItem in source)
      {
        this.gvPages.Items.Remove(gvItem);
        this.changeList.Add(gvItem.Text + " deleted");
      }
      this.showPageFiles();
      this.btnSave.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      long ticks = DateTime.Now.Ticks;
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvPages.Items)
        stringList.Add(gvItem.Tag as string);
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        string filePath = (string) null;
        string convertedFile = this.originalFile.GetConvertedFile();
        try
        {
          using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
          {
            Tracing.Log(EditFileDialog.sw, TraceLevel.Verbose, nameof (EditFileDialog), "SkyDriveClassic: Calling MergeFiles");
            filePath = pdfPageBuilder.MergeFiles(stringList.ToArray(), convertedFile);
            if (filePath == null)
              return;
          }
          Tracing.Log(EditFileDialog.sw, TraceLevel.Verbose, nameof (EditFileDialog), "SkyDriveClassic: Calling CreateNextVersionFile");
          this.originalFile.SetConvertedFile(this.sdcHelper.CreateNextVersionFile(filePath, this.originalFile.OriginalSDCDocument.Version));
          this.originalFile.CurrentSDCDocument = this.sdcDocumentCopy;
          this.editedFile = (FileAttachment) this.originalFile;
          if (this.loanDataMgr.IsAutosaveEnabled)
            this.editedFile.MarkAsDirty();
          this.loanDataMgr.FileAttachments.OnFileAttachmentChanged(this.editedFile);
        }
        catch (Exception ex)
        {
          Tracing.Log(EditFileDialog.sw, TraceLevel.Error, nameof (EditFileDialog), string.Format("SkyDriveClassic: Error in EditFileDialog save opration for skyDriveObjectId-{0}. Ex: {1}", (object) this.originalFile.ObjectId, (object) ex));
          throw;
        }
      }
      else
      {
        string filepath = (string) null;
        using (PdfPageBuilder pdfPageBuilder = new PdfPageBuilder())
        {
          filepath = pdfPageBuilder.MergeFiles(stringList.ToArray());
          if (filepath == null)
            return;
        }
        using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
        {
          this.editedFile = attachFilesDialog.ReplaceFile(AddReasonType.Edit, this.originalFile, filepath, this.loanDataMgr.FileAttachments.GetLinkedDocument(this.originalFile.ID));
          if (this.editedFile == null)
            return;
        }
      }
      foreach (string change in this.changeList)
        this.loanDataMgr.LoanHistory.TrackChange(this.editedFile, change);
      RemoteLogger.Write(TraceLevel.Info, "Edited NativeAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
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
      GVColumn gvColumn = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditFileDialog));
      this.pnlRight = new BorderPanel();
      this.pageViewer = new PageViewerControl();
      this.pnlPageToolbar = new FlowLayoutPanel();
      this.btnDeletePage = new StandardIconButton();
      this.btnMovePageDown = new StandardIconButton();
      this.btnMovePageUp = new StandardIconButton();
      this.btnRotatePage = new IconButton();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.pnlFooter = new Panel();
      this.tooltip = new ToolTip(this.components);
      this.csPages = new CollapsibleSplitter();
      this.gcPages = new GroupContainer();
      this.gvPages = new GridView();
      this.pnlRight.SuspendLayout();
      this.pnlPageToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnDeletePage).BeginInit();
      ((ISupportInitialize) this.btnMovePageDown).BeginInit();
      ((ISupportInitialize) this.btnMovePageUp).BeginInit();
      ((ISupportInitialize) this.btnRotatePage).BeginInit();
      this.pnlFooter.SuspendLayout();
      this.gcPages.SuspendLayout();
      this.SuspendLayout();
      this.pnlRight.BackgroundImageLayout = ImageLayout.Center;
      this.pnlRight.Controls.Add((Control) this.pageViewer);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(267, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(396, 309);
      this.pnlRight.TabIndex = 4;
      this.pageViewer.Dock = DockStyle.Fill;
      this.pageViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pageViewer.Location = new Point(1, 1);
      this.pageViewer.Name = "pageViewer";
      this.pageViewer.Size = new Size(394, 307);
      this.pageViewer.TabIndex = 5;
      this.pnlPageToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlPageToolbar.BackColor = Color.Transparent;
      this.pnlPageToolbar.Controls.Add((Control) this.btnDeletePage);
      this.pnlPageToolbar.Controls.Add((Control) this.btnMovePageDown);
      this.pnlPageToolbar.Controls.Add((Control) this.btnMovePageUp);
      this.pnlPageToolbar.Controls.Add((Control) this.btnRotatePage);
      this.pnlPageToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlPageToolbar.Location = new Point(128, 2);
      this.pnlPageToolbar.Name = "pnlPageToolbar";
      this.pnlPageToolbar.Size = new Size(128, 22);
      this.pnlPageToolbar.TabIndex = 1;
      this.btnDeletePage.BackColor = Color.Transparent;
      this.btnDeletePage.Location = new Point(112, 3);
      this.btnDeletePage.Margin = new Padding(4, 3, 0, 3);
      this.btnDeletePage.Name = "btnDeletePage";
      this.btnDeletePage.Size = new Size(16, 16);
      this.btnDeletePage.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeletePage.TabIndex = 28;
      this.btnDeletePage.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeletePage, "Remove Page");
      this.btnDeletePage.Click += new EventHandler(this.btnDeletePage_Click);
      this.btnMovePageDown.BackColor = Color.Transparent;
      this.btnMovePageDown.Location = new Point(92, 3);
      this.btnMovePageDown.Margin = new Padding(4, 3, 0, 3);
      this.btnMovePageDown.Name = "btnMovePageDown";
      this.btnMovePageDown.Size = new Size(16, 16);
      this.btnMovePageDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMovePageDown.TabIndex = 27;
      this.btnMovePageDown.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMovePageDown, "Move Page Down");
      this.btnMovePageDown.Click += new EventHandler(this.btnMovePageDown_Click);
      this.btnMovePageUp.BackColor = Color.Transparent;
      this.btnMovePageUp.Location = new Point(72, 3);
      this.btnMovePageUp.Margin = new Padding(4, 3, 0, 3);
      this.btnMovePageUp.Name = "btnMovePageUp";
      this.btnMovePageUp.Size = new Size(16, 16);
      this.btnMovePageUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMovePageUp.TabIndex = 26;
      this.btnMovePageUp.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnMovePageUp, "Move Page Up");
      this.btnMovePageUp.Click += new EventHandler(this.btnMovePageUp_Click);
      this.btnRotatePage.BackColor = Color.Transparent;
      this.btnRotatePage.DisabledImage = (Image) Resources.rotate_disabled;
      this.btnRotatePage.Image = (Image) Resources.rotate;
      this.btnRotatePage.Location = new Point(52, 3);
      this.btnRotatePage.Margin = new Padding(4, 3, 0, 3);
      this.btnRotatePage.MouseOverImage = (Image) Resources.rotate_over;
      this.btnRotatePage.Name = "btnRotatePage";
      this.btnRotatePage.Size = new Size(16, 16);
      this.btnRotatePage.TabIndex = 32;
      this.btnRotatePage.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRotatePage, "Rotate Page");
      this.btnRotatePage.Click += new EventHandler(this.btnRotatePage_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(505, 9);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 7;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(581, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.pnlFooter.Controls.Add((Control) this.btnCancel);
      this.pnlFooter.Controls.Add((Control) this.btnSave);
      this.pnlFooter.Dock = DockStyle.Bottom;
      this.pnlFooter.Location = new Point(0, 309);
      this.pnlFooter.Name = "pnlFooter";
      this.pnlFooter.Size = new Size(663, 40);
      this.pnlFooter.TabIndex = 6;
      this.csPages.AnimationDelay = 20;
      this.csPages.AnimationStep = 20;
      this.csPages.BorderStyle3D = Border3DStyle.Flat;
      this.csPages.ControlToHide = (Control) this.gcPages;
      this.csPages.ExpandParentForm = false;
      this.csPages.Location = new Point(260, 0);
      this.csPages.Name = "csLeft";
      this.csPages.TabIndex = 3;
      this.csPages.TabStop = false;
      this.csPages.UseAnimations = false;
      this.csPages.VisualStyle = VisualStyles.Encompass;
      this.gcPages.Controls.Add((Control) this.pnlPageToolbar);
      this.gcPages.Controls.Add((Control) this.gvPages);
      this.gcPages.Dock = DockStyle.Left;
      this.gcPages.HeaderForeColor = SystemColors.ControlText;
      this.gcPages.Location = new Point(0, 0);
      this.gcPages.Name = "gcPages";
      this.gcPages.Size = new Size(260, 309);
      this.gcPages.TabIndex = 0;
      this.gcPages.Text = "Pages";
      this.gvPages.BorderStyle = BorderStyle.None;
      this.gvPages.ClearSelectionsOnEmptyRowClick = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "colPage";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Page";
      gvColumn.Width = 258;
      this.gvPages.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvPages.Dock = DockStyle.Fill;
      this.gvPages.HeaderHeight = 0;
      this.gvPages.HeaderVisible = false;
      this.gvPages.HoverToolTip = this.tooltip;
      this.gvPages.Location = new Point(1, 26);
      this.gvPages.Name = "gvPages";
      this.gvPages.Size = new Size(258, 282);
      this.gvPages.TabIndex = 2;
      this.gvPages.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvPages.UseCompatibleEditingBehavior = true;
      this.gvPages.SelectedIndexChanged += new EventHandler(this.gvPages_SelectedIndexChanged);
      this.gvPages.SelectedIndexCommitted += new EventHandler(this.gvPages_SelectedIndexCommitted);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(663, 349);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.csPages);
      this.Controls.Add((Control) this.gcPages);
      this.Controls.Add((Control) this.pnlFooter);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.Name = nameof (EditFileDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Edit File";
      this.pnlRight.ResumeLayout(false);
      this.pnlPageToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeletePage).EndInit();
      ((ISupportInitialize) this.btnMovePageDown).EndInit();
      ((ISupportInitialize) this.btnMovePageUp).EndInit();
      ((ISupportInitialize) this.btnRotatePage).EndInit();
      this.pnlFooter.ResumeLayout(false);
      this.gcPages.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
