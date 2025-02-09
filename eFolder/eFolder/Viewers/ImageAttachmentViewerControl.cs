// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.ImageAttachmentViewerControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class ImageAttachmentViewerControl : UserControl
  {
    private const string className = "ImageAttachmentViewerControl";
    private static readonly string sw = Tracing.SwEFolder;
    private ImageAttachment[] attachmentList;
    private IContainer components;
    private CollapsibleSplitter csThumbnails;
    private CollapsibleSplitter csAnnotations;
    private PageThumbnailListControl thumbnailCtl;
    private PageImageListControl pageCtl;
    private PageAnnotationListControl annotationCtl;
    private CollapsibleSplitter csPages;

    public ImageAttachmentViewerControl()
    {
      this.InitializeComponent();
      this.csAnnotations.IsCollapsed = true;
    }

    public void LoadAttachments(ImageAttachment[] attachmentList)
    {
      Tracing.Log(ImageAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (ImageAttachmentViewerControl), nameof (LoadAttachments));
      try
      {
        using (PerformanceMeter.StartNew("ImageAttachmentViewerControl.LoadAttachments", attachmentList.Length.ToString() + " files", 58, nameof (LoadAttachments), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Viewers\\ImageAttachmentViewerControl.cs"))
        {
          this.thumbnailCtl.LoadThumbnails(attachmentList);
          this.pageCtl.LoadPages(attachmentList);
          this.annotationCtl.LoadAnnotations(attachmentList);
          this.attachmentList = attachmentList;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(ImageAttachmentViewerControl.sw, TraceLevel.Error, nameof (ImageAttachmentViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to load the files:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    public void CloseAttachments()
    {
      Tracing.Log(ImageAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (ImageAttachmentViewerControl), nameof (CloseAttachments));
      try
      {
        if (this.attachmentList == null)
          return;
        Tracing.Log(ImageAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (ImageAttachmentViewerControl), "Clearing Attachment List");
        this.attachmentList = (ImageAttachment[]) null;
        this.thumbnailCtl.CancelWorker();
        this.pageCtl.CancelWorker();
      }
      catch (Exception ex)
      {
        Tracing.Log(ImageAttachmentViewerControl.sw, TraceLevel.Error, nameof (ImageAttachmentViewerControl), ex.ToString());
        this.showMessage("The following error occurred when trying to close the file:\n\n" + ex.Message + "\n\n" + ex.StackTrace);
      }
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
      Tracing.Log(ImageAttachmentViewerControl.sw, TraceLevel.Verbose, nameof (ImageAttachmentViewerControl), "Handle Destroyed");
      base.OnHandleDestroyed(e);
      this.thumbnailCtl.CancelWorker();
      this.pageCtl.CancelWorker();
    }

    private void showMessage(string message)
    {
    }

    private void thumbnailCtl_PagesChanged(object sender, EventArgs e)
    {
      if (this.attachmentList == null)
        return;
      List<ImageAttachment> imageAttachmentList = new List<ImageAttachment>();
      for (int index = 0; index < this.attachmentList.Length; ++index)
      {
        ImageAttachment attachment = this.attachmentList[index];
        if (attachment.Pages.Count > 0)
          imageAttachmentList.Add(attachment);
        else if (Session.LoanDataMgr.FileAttachments.Remove(RemoveReasonType.User, (FileAttachment) attachment))
          DeleteAttachmentWarningDialog.ShowDeleteWarning((IWin32Window) this);
      }
      this.attachmentList = imageAttachmentList.ToArray();
      if (this.attachmentList.Length != 0)
      {
        this.pageCtl.LoadPages(this.attachmentList);
        this.annotationCtl.LoadAnnotations(this.attachmentList);
      }
      else
        this.OnCloseFile(EventArgs.Empty);
      this.OnImageUpdated(EventArgs.Empty);
    }

    private void thumbnailCtl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.thumbnailCtl.SelectedIndices.Count <= 0)
        return;
      this.pageCtl.ScrollComplete -= new EventHandler(this.pageCtl_ScrollComplete);
      this.pageCtl.SetCurrentPage(this.thumbnailCtl.SelectedIndices[0]);
      this.pageCtl.ScrollComplete += new EventHandler(this.pageCtl_ScrollComplete);
    }

    private void pageCtl_AnnotationsChanged(object sender, EventArgs e)
    {
      this.annotationCtl.LoadAnnotations(this.attachmentList);
      this.OnImageUpdated(EventArgs.Empty);
    }

    private void pageCtl_ScrollComplete(object sender, EventArgs e)
    {
      int currentPage = this.pageCtl.GetCurrentPage();
      if (currentPage < 0)
        return;
      if (!this.thumbnailCtl.SelectedIndices.Contains(currentPage))
      {
        this.thumbnailCtl.SelectedIndexChanged -= new EventHandler(this.thumbnailCtl_SelectedIndexChanged);
        this.thumbnailCtl.BeginUpdate();
        this.thumbnailCtl.SelectedIndices.Clear();
        this.thumbnailCtl.SelectedIndices.Add(currentPage);
        this.thumbnailCtl.EndUpdate();
        this.thumbnailCtl.SelectedIndexChanged += new EventHandler(this.thumbnailCtl_SelectedIndexChanged);
      }
      this.thumbnailCtl.EnsureVisible(currentPage);
    }

    private void annotationCtl_AnnotationsChanged(object sender, EventArgs e)
    {
      this.pageCtl.LoadPages(this.attachmentList);
      this.OnImageUpdated(EventArgs.Empty);
    }

    private void annotationCtl_SelectedIndexCommitted(object sender, EventArgs e)
    {
      PageAnnotation selectedAnnotation = this.annotationCtl.GetSelectedAnnotation();
      if (selectedAnnotation == null)
        return;
      this.pageCtl.SetCurrentPage(selectedAnnotation);
    }

    protected virtual void OnCloseFile(EventArgs e)
    {
      if (this.CloseFile == null)
        return;
      this.CloseFile((object) this, e);
    }

    public event EventHandler CloseFile;

    protected virtual void OnImageUpdated(EventArgs e)
    {
      if (this.ImageUpdated == null)
        return;
      this.ImageUpdated((object) this, e);
    }

    public event EventHandler ImageUpdated;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pageCtl = new PageImageListControl();
      this.csAnnotations = new CollapsibleSplitter();
      this.annotationCtl = new PageAnnotationListControl();
      this.csThumbnails = new CollapsibleSplitter();
      this.thumbnailCtl = new PageThumbnailListControl();
      this.csPages = new CollapsibleSplitter();
      this.SuspendLayout();
      this.pageCtl.Dock = DockStyle.Fill;
      this.pageCtl.Location = new Point(183, 0);
      this.pageCtl.Name = "pageCtl";
      this.pageCtl.Size = new Size(625, 186);
      this.pageCtl.TabIndex = 7;
      this.pageCtl.TabStop = false;
      this.pageCtl.AnnotationsChanged += new EventHandler(this.pageCtl_AnnotationsChanged);
      this.pageCtl.ScrollComplete += new EventHandler(this.pageCtl_ScrollComplete);
      this.csAnnotations.AnimationDelay = 20;
      this.csAnnotations.AnimationStep = 20;
      this.csAnnotations.BorderStyle3D = Border3DStyle.Flat;
      this.csAnnotations.ControlToHide = (Control) this.annotationCtl;
      this.csAnnotations.Dock = DockStyle.Bottom;
      this.csAnnotations.ExpandParentForm = false;
      this.csAnnotations.Location = new Point(183, 186);
      this.csAnnotations.Name = "pnlSplitter";
      this.csAnnotations.TabIndex = 5;
      this.csAnnotations.TabStop = false;
      this.csAnnotations.UseAnimations = false;
      this.csAnnotations.VisualStyle = VisualStyles.Encompass;
      this.annotationCtl.Dock = DockStyle.Bottom;
      this.annotationCtl.Location = new Point(183, 193);
      this.annotationCtl.Name = "annotationCtl";
      this.annotationCtl.Size = new Size(625, 85);
      this.annotationCtl.TabIndex = 8;
      this.annotationCtl.TabStop = false;
      this.annotationCtl.AnnotationsChanged += new EventHandler(this.annotationCtl_AnnotationsChanged);
      this.annotationCtl.SelectedIndexCommitted += new EventHandler(this.annotationCtl_SelectedIndexCommitted);
      this.csThumbnails.AnimationDelay = 20;
      this.csThumbnails.AnimationStep = 20;
      this.csThumbnails.BorderStyle3D = Border3DStyle.Flat;
      this.csThumbnails.ControlToHide = (Control) this.thumbnailCtl;
      this.csThumbnails.ExpandParentForm = false;
      this.csThumbnails.Location = new Point(176, 0);
      this.csThumbnails.Name = "pnlSplitter";
      this.csThumbnails.TabIndex = 1;
      this.csThumbnails.TabStop = false;
      this.csThumbnails.UseAnimations = false;
      this.csThumbnails.VisualStyle = VisualStyles.Encompass;
      this.thumbnailCtl.Dock = DockStyle.Left;
      this.thumbnailCtl.Location = new Point(0, 0);
      this.thumbnailCtl.Name = "thumbnailCtl";
      this.thumbnailCtl.Size = new Size(176, 278);
      this.thumbnailCtl.TabIndex = 4;
      this.thumbnailCtl.TabStop = false;
      this.thumbnailCtl.PagesChanged += new EventHandler(this.thumbnailCtl_PagesChanged);
      this.thumbnailCtl.SelectedIndexChanged += new EventHandler(this.thumbnailCtl_SelectedIndexChanged);
      this.csPages.AnimationDelay = 20;
      this.csPages.AnimationStep = 20;
      this.csPages.BorderStyle3D = Border3DStyle.Flat;
      this.csPages.ControlToHide = (Control) this.thumbnailCtl;
      this.csPages.ExpandParentForm = false;
      this.csPages.Location = new Point(176, 0);
      this.csPages.Name = "pnlSplitter";
      this.csPages.TabIndex = 1;
      this.csPages.TabStop = false;
      this.csPages.UseAnimations = false;
      this.csPages.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pageCtl);
      this.Controls.Add((Control) this.csAnnotations);
      this.Controls.Add((Control) this.annotationCtl);
      this.Controls.Add((Control) this.csThumbnails);
      this.Controls.Add((Control) this.thumbnailCtl);
      this.Name = nameof (ImageAttachmentViewerControl);
      this.Size = new Size(808, 278);
      this.ResumeLayout(false);
    }
  }
}
