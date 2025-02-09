// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.AnnotateFileDialog
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
using EllieMae.EMLite.eFolder.Viewers;
using EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public class AnnotateFileDialog : Form
  {
    private const string className = "AnnotateFileDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private NativeAttachment originalFile;
    private FileAttachment editedFile;
    private SDCDocument sdcDocumentCopy;
    private string pdfFile;
    private IntPtr hCursor;
    private List<string> changeList = new List<string>();
    private IContainer components;
    private ToolTip tooltip;
    private Panel pnlBottom;
    private GradientPanel pnlHeader;
    private ImageViewerControl imageViewer;
    private Button btnCancel;
    private Button btnSave;
    private CollapsibleSplitter csAnnotations;
    private GroupContainer gcAnnotations;
    private FlowLayoutPanel pnlToolbar2;
    private StandardIconButton btnDeleteAnnotation;
    private ListBoxVariable lstAnnotations;
    private PageListNavigator pageList;
    private PictureBox pctCursor;
    private GroupContainer gcViewer;

    [DllImport("user32.dll")]
    private static extern bool GetIconInfo(IntPtr hIcon, ref AnnotateFileDialog.IconInfo pIconInfo);

    [DllImport("user32.dll")]
    private static extern IntPtr CreateIconIndirect(ref AnnotateFileDialog.IconInfo icon);

    [DllImport("user32.dll")]
    private static extern bool DestroyIcon(IntPtr hIcon);

    public AnnotateFileDialog(LoanDataMgr loanDataMgr, NativeAttachment file, string pdfFile)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.loanDataMgr = loanDataMgr;
      this.originalFile = file;
      this.pdfFile = pdfFile;
      if (loanDataMgr.UseSkyDriveClassic)
        this.sdcDocumentCopy = file.CurrentSDCDocument == null ? Utils.DeepClone<SDCDocument>(this.originalFile.OriginalSDCDocument) : Utils.DeepClone<SDCDocument>(this.originalFile.CurrentSDCDocument);
      this.loadAnnotationList();
      this.applySecurity();
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
      this.btnDeleteAnnotation.Visible = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) this.loanDataMgr.FileAttachments.GetLinkedDocument(this.originalFile.ID)).CanDeleteAnnotations;
    }

    private void AnnotateFileDialog_Load(object sender, EventArgs e)
    {
      using (Bitmap bitmap = new Bitmap(this.pctCursor.Image))
      {
        IntPtr hicon = bitmap.GetHicon();
        if (hicon != IntPtr.Zero)
        {
          AnnotateFileDialog.IconInfo iconInfo = new AnnotateFileDialog.IconInfo();
          if (AnnotateFileDialog.GetIconInfo(hicon, ref iconInfo))
          {
            iconInfo.xHotspot = 0;
            iconInfo.yHotspot = 0;
            iconInfo.fIcon = false;
            this.hCursor = AnnotateFileDialog.CreateIconIndirect(ref iconInfo);
            if (this.hCursor != IntPtr.Zero)
              this.imageViewer.ImageCursor = new Cursor(this.hCursor);
          }
          AnnotateFileDialog.DestroyIcon(hicon);
        }
      }
      using (PdfEditor pdfEditor = new PdfEditor(this.pdfFile))
        this.pageList.NumberOfItems = pdfEditor.PageCount;
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      if (this.hCursor != IntPtr.Zero)
        AnnotateFileDialog.DestroyIcon(this.hCursor);
      base.OnFormClosed(e);
    }

    private void pageList_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.imageViewer.LoadPage(this.pdfFile, e.PageIndex + 1);
    }

    private void loadAnnotationList()
    {
      this.lstAnnotations.Items.Clear();
      using (PdfEditor pdfEditor = new PdfEditor(this.pdfFile))
      {
        foreach (object annotation in pdfEditor.Annotations)
          this.lstAnnotations.Items.Add(annotation);
      }
      this.gcAnnotations.Text = "Notes (" + (object) this.lstAnnotations.Items.Count + ")";
    }

    private void lstAnnotations_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lstAnnotations.SelectedItem != null)
        this.btnDeleteAnnotation.Enabled = true;
      else
        this.btnDeleteAnnotation.Enabled = false;
    }

    private void btnDeleteAnnotation_Click(object sender, EventArgs e)
    {
      PdfTextAnnotation annotation = (PdfTextAnnotation) this.lstAnnotations.SelectedItem;
      string currentFilePath = string.Empty;
      if (Session.LoanDataMgr.UseSkyDriveClassic)
      {
        this.sdcDocumentCopy.Annotations.Remove(this.sdcDocumentCopy.Annotations.FirstOrDefault<Annotation>((Func<Annotation, bool>) (annotationItem => annotationItem.Title == annotation.Title)));
        currentFilePath = this.pdfFile;
      }
      Tracing.Log(AnnotateFileDialog.sw, TraceLevel.Verbose, nameof (AnnotateFileDialog), "Calling DeleteAnnotation");
      using (PdfEditor pdfEditor = new PdfEditor(this.pdfFile))
        this.pdfFile = pdfEditor.DeleteAnnotation(annotation, currentFilePath);
      this.loadAnnotationList();
      if (annotation.PageIndex == this.pageList.CurrentPageIndex + 1)
        this.imageViewer.LoadPage(this.pdfFile, annotation.PageIndex);
      this.changeList.Add("Notes deleted \"" + annotation.Contents + "\"");
      this.btnSave.Enabled = true;
    }

    private void imageViewer_ImageClick(ImageMouseEventArgs e)
    {
      PdfTextAnnotation pdfTextAnnotation = new PdfTextAnnotation();
      DateTime now = DateTime.Now;
      pdfTextAnnotation.Title = Session.UserInfo.FullName + " - " + now.ToString("MM/dd/yy hh:mm tt");
      pdfTextAnnotation.Contents = string.Empty;
      pdfTextAnnotation.Color = Color.FromArgb(254, 228, 57);
      pdfTextAnnotation.Icon = "Comment";
      pdfTextAnnotation.PageIndex = this.pageList.CurrentPageIndex + 1;
      pdfTextAnnotation.X = (float) e.PdfX;
      pdfTextAnnotation.Y = (float) e.PdfY;
      using (TextAnnotationDialog annotationDialog = new TextAnnotationDialog(pdfTextAnnotation, Session.LoanDataMgr.UseSkyDriveClassic))
      {
        if (annotationDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
      }
      string currentFilePath = string.Empty;
      if (Session.LoanDataMgr.UseSkyDriveClassic)
      {
        if (this.sdcDocumentCopy != null)
        {
          try
          {
            SDCMapper sdcMapper = new SDCMapper();
            int x = e.X;
            int y = e.Y;
            pdfTextAnnotation.AnnotationGuid = Guid.NewGuid().ToString();
            pdfTextAnnotation.Title = Session.UserInfo.FullName + " - " + now.ToString("MM/dd/yy hh:mm:ss tt");
            Tracing.Log(AnnotateFileDialog.sw, TraceLevel.Verbose, nameof (AnnotateFileDialog), "SkyDriveClassic: Calling AnnotationMapper");
            sdcMapper.AnnotationMapper(this.sdcDocumentCopy, pdfTextAnnotation, Session.UserInfo.Userid, Session.UserInfo.userName, now.ToString(), x, y);
            currentFilePath = this.pdfFile;
          }
          catch (Exception ex)
          {
            Tracing.Log(AnnotateFileDialog.sw, TraceLevel.Error, nameof (AnnotateFileDialog), string.Format("SkyDriveClassic: Error while adding new annotation for skyDriveObjectId-{0}. Ex: {1}", (object) this.originalFile.ObjectId, (object) ex));
            throw;
          }
        }
      }
      using (PdfEditor pdfEditor = new PdfEditor(this.pdfFile))
        this.pdfFile = pdfEditor.AddAnnotation(pdfTextAnnotation, currentFilePath);
      this.loadAnnotationList();
      this.imageViewer.LoadPage(this.pdfFile, pdfTextAnnotation.PageIndex);
      this.changeList.Add("Notes added \"" + pdfTextAnnotation.Contents + "\"");
      this.btnSave.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      long ticks = DateTime.Now.Ticks;
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      if (this.loanDataMgr.UseSkyDriveClassic)
      {
        try
        {
          this.originalFile.SetConvertedFile(this.pdfFile);
          this.originalFile.CurrentSDCDocument = this.sdcDocumentCopy;
          this.editedFile = (FileAttachment) this.originalFile;
          this.loanDataMgr.FileAttachments.OnFileAttachmentChanged(this.editedFile);
          if (this.loanDataMgr.IsAutosaveEnabled)
            this.originalFile.MarkAsDirty();
        }
        catch (Exception ex)
        {
          Tracing.Log(AnnotateFileDialog.sw, TraceLevel.Error, nameof (AnnotateFileDialog), string.Format("SkyDriveClassic: Error in AnnotateFileDialog save opration for skyDriveObjectId-{0}. Ex: {1}", (object) this.originalFile.ObjectId, (object) ex));
          throw;
        }
      }
      else
      {
        using (AttachFilesDialog attachFilesDialog = new AttachFilesDialog(this.loanDataMgr))
        {
          this.editedFile = attachFilesDialog.ReplaceFile(AddReasonType.Annotate, this.originalFile, this.pdfFile, this.loanDataMgr.FileAttachments.GetLinkedDocument(this.originalFile.ID));
          if (this.editedFile == null)
            return;
        }
      }
      foreach (string change in this.changeList)
        this.loanDataMgr.LoanHistory.TrackChange(this.editedFile, change);
      RemoteLogger.Write(TraceLevel.Info, "Annotated NativeAttachment: " + (object) TimeSpan.FromTicks(DateTime.Now.Ticks - ticks).TotalMilliseconds + " ms");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AnnotateFileDialog));
      this.pnlBottom = new Panel();
      this.pctCursor = new PictureBox();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.tooltip = new ToolTip(this.components);
      this.btnDeleteAnnotation = new StandardIconButton();
      this.pnlHeader = new GradientPanel();
      this.pageList = new PageListNavigator();
      this.csAnnotations = new CollapsibleSplitter();
      this.gcAnnotations = new GroupContainer();
      this.lstAnnotations = new ListBoxVariable();
      this.pnlToolbar2 = new FlowLayoutPanel();
      this.gcViewer = new GroupContainer();
      this.imageViewer = new ImageViewerControl();
      this.pnlBottom.SuspendLayout();
      ((ISupportInitialize) this.pctCursor).BeginInit();
      ((ISupportInitialize) this.btnDeleteAnnotation).BeginInit();
      this.pnlHeader.SuspendLayout();
      this.gcAnnotations.SuspendLayout();
      this.pnlToolbar2.SuspendLayout();
      this.gcViewer.SuspendLayout();
      this.SuspendLayout();
      this.pnlBottom.Controls.Add((Control) this.pctCursor);
      this.pnlBottom.Controls.Add((Control) this.btnCancel);
      this.pnlBottom.Controls.Add((Control) this.btnSave);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 375);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(658, 40);
      this.pnlBottom.TabIndex = 8;
      this.pctCursor.Image = (Image) Resources.pdf_comment;
      this.pctCursor.Location = new Point(6, 6);
      this.pctCursor.Name = "pctCursor";
      this.pctCursor.Size = new Size(28, 28);
      this.pctCursor.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pctCursor.TabIndex = 14;
      this.pctCursor.TabStop = false;
      this.pctCursor.Visible = false;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(575, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(499, 9);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 9;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnDeleteAnnotation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteAnnotation.BackColor = Color.Transparent;
      this.btnDeleteAnnotation.Enabled = false;
      this.btnDeleteAnnotation.Location = new Point(36, 3);
      this.btnDeleteAnnotation.Margin = new Padding(4, 3, 0, 3);
      this.btnDeleteAnnotation.Name = "btnDeleteAnnotation";
      this.btnDeleteAnnotation.Size = new Size(16, 16);
      this.btnDeleteAnnotation.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteAnnotation.TabIndex = 13;
      this.btnDeleteAnnotation.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDeleteAnnotation, "Delete Note");
      this.btnDeleteAnnotation.Click += new EventHandler(this.btnDeleteAnnotation_Click);
      this.pnlHeader.Borders = AnchorStyles.Bottom;
      this.pnlHeader.Controls.Add((Control) this.pageList);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlHeader.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlHeader.Location = new Point(1, 26);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Padding = new Padding(8, 0, 0, 0);
      this.pnlHeader.Size = new Size(457, 31);
      this.pnlHeader.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlHeader.TabIndex = 5;
      this.pageList.AutoSize = true;
      this.pageList.BackColor = Color.Transparent;
      this.pageList.Font = new Font("Arial", 8f);
      this.pageList.ItemsPerPage = 1;
      this.pageList.Location = new Point(0, 4);
      this.pageList.Name = "pageList";
      this.pageList.NumberOfItems = 0;
      this.pageList.Size = new Size(253, 25);
      this.pageList.TabIndex = 6;
      this.pageList.TabStop = false;
      this.pageList.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.pageList_PageChangedEvent);
      this.csAnnotations.AnimationDelay = 20;
      this.csAnnotations.AnimationStep = 20;
      this.csAnnotations.BorderStyle3D = Border3DStyle.Flat;
      this.csAnnotations.ControlToHide = (Control) this.gcAnnotations;
      this.csAnnotations.ExpandParentForm = false;
      this.csAnnotations.Location = new Point(192, 0);
      this.csAnnotations.Name = "csLeft";
      this.csAnnotations.TabIndex = 3;
      this.csAnnotations.TabStop = false;
      this.csAnnotations.UseAnimations = false;
      this.csAnnotations.VisualStyle = VisualStyles.Encompass;
      this.gcAnnotations.Controls.Add((Control) this.lstAnnotations);
      this.gcAnnotations.Controls.Add((Control) this.pnlToolbar2);
      this.gcAnnotations.Dock = DockStyle.Left;
      this.gcAnnotations.HeaderForeColor = SystemColors.ControlText;
      this.gcAnnotations.Location = new Point(0, 0);
      this.gcAnnotations.Name = "gcAnnotations";
      this.gcAnnotations.Size = new Size(192, 375);
      this.gcAnnotations.TabIndex = 0;
      this.gcAnnotations.Text = "Notes (#)";
      this.lstAnnotations.BorderStyle = BorderStyle.None;
      this.lstAnnotations.Dock = DockStyle.Fill;
      this.lstAnnotations.FormattingEnabled = true;
      this.lstAnnotations.ItemHeight = 14;
      this.lstAnnotations.Location = new Point(1, 26);
      this.lstAnnotations.Name = "lstAnnotations";
      this.lstAnnotations.Size = new Size(190, 348);
      this.lstAnnotations.TabIndex = 2;
      this.lstAnnotations.SelectedIndexChanged += new EventHandler(this.lstAnnotations_SelectedIndexChanged);
      this.pnlToolbar2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar2.BackColor = Color.Transparent;
      this.pnlToolbar2.Controls.Add((Control) this.btnDeleteAnnotation);
      this.pnlToolbar2.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar2.Location = new Point(136, 2);
      this.pnlToolbar2.Name = "pnlToolbar2";
      this.pnlToolbar2.Size = new Size(52, 22);
      this.pnlToolbar2.TabIndex = 1;
      this.gcViewer.Controls.Add((Control) this.imageViewer);
      this.gcViewer.Controls.Add((Control) this.pnlHeader);
      this.gcViewer.Dock = DockStyle.Fill;
      this.gcViewer.HeaderForeColor = SystemColors.ControlText;
      this.gcViewer.Location = new Point(199, 0);
      this.gcViewer.Name = "gcViewer";
      this.gcViewer.Size = new Size(459, 375);
      this.gcViewer.TabIndex = 4;
      this.gcViewer.Text = "Click on the location in the page where you would like to add a note";
      this.imageViewer.AutoScroll = true;
      this.imageViewer.BackColor = SystemColors.AppWorkspace;
      this.imageViewer.Dock = DockStyle.Fill;
      this.imageViewer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.imageViewer.Location = new Point(1, 57);
      this.imageViewer.Name = "imageViewer";
      this.imageViewer.Size = new Size(457, 317);
      this.imageViewer.TabIndex = 7;
      this.imageViewer.ImageClick += new ImageMouseEventHandler(this.imageViewer_ImageClick);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(658, 415);
      this.Controls.Add((Control) this.gcViewer);
      this.Controls.Add((Control) this.csAnnotations);
      this.Controls.Add((Control) this.gcAnnotations);
      this.Controls.Add((Control) this.pnlBottom);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = Resources.icon_allsizes_bug;
      this.Name = nameof (AnnotateFileDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Notes";
      this.Load += new EventHandler(this.AnnotateFileDialog_Load);
      this.pnlBottom.ResumeLayout(false);
      this.pnlBottom.PerformLayout();
      ((ISupportInitialize) this.pctCursor).EndInit();
      ((ISupportInitialize) this.btnDeleteAnnotation).EndInit();
      this.pnlHeader.ResumeLayout(false);
      this.pnlHeader.PerformLayout();
      this.gcAnnotations.ResumeLayout(false);
      this.pnlToolbar2.ResumeLayout(false);
      this.gcViewer.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private struct IconInfo
    {
      public bool fIcon;
      public int xHotspot;
      public int yHotspot;
      public IntPtr hbmMask;
      public IntPtr hbmColor;
    }
  }
}
