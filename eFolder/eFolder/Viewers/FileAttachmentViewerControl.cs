// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.FileAttachmentViewerControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Elli.Web.Host;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class FileAttachmentViewerControl : UserControl
  {
    private string currentViewer;
    private FileAttachment[] attachmentList;
    private string[] attachmentIdList;
    private IContainer components;
    private PictureBox pctEmpty;
    private PictureBox backgroundViewer;
    private NativeAttachmentViewerControl nativeViewer;
    private ImageAttachmentViewerControl imageViewer;
    private CloudAttachmentViewerControl cloudViewer;
    private SkyDriveViewerControl skyDriveViewer;

    public FileAttachmentViewerControl()
    {
      this.InitializeComponent();
      this.pctEmpty.Location = new Point(0, 0);
      this.nativeViewer.Location = new Point(0, 0);
      this.imageViewer.Location = new Point(0, 0);
      this.backgroundViewer.Location = new Point(0, 0);
      this.cloudViewer.Location = new Point(0, 0);
      this.skyDriveViewer.Location = new Point(0, 0);
      this.pctEmpty.BringToFront();
      this.currentViewer = this.pctEmpty.Name;
    }

    public FileAttachment[] Attachments => this.attachmentList;

    public string MergeJobId => this.cloudViewer.MergeJobId;

    private void FileAttachmentViewerControl_Resize(object sender, EventArgs e)
    {
      this.pctEmpty.Size = this.ClientSize;
      this.backgroundViewer.Size = this.ClientSize;
      this.nativeViewer.Size = this.ClientSize;
      this.imageViewer.Size = this.ClientSize;
      this.cloudViewer.Size = this.ClientSize;
      this.skyDriveViewer.Size = this.ClientSize;
    }

    public void LoadFiles(FileAttachment[] attachmentList)
    {
      this.attachmentList = attachmentList;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      foreach (FileAttachment attachment in attachmentList)
      {
        switch (attachment)
        {
          case NativeAttachment _:
            flag1 = true;
            break;
          case BackgroundAttachment _:
            flag2 = true;
            break;
          case ImageAttachment _:
            flag3 = true;
            break;
          case CloudAttachment _:
            flag4 = true;
            break;
        }
      }
      if (flag2)
      {
        this.backgroundViewer.BringToFront();
        this.currentViewer = this.backgroundViewer.Name;
      }
      else if (flag3 && !flag4 && !flag1)
      {
        List<ImageAttachment> imageAttachmentList = new List<ImageAttachment>();
        foreach (ImageAttachment attachment in attachmentList)
        {
          if (attachment.Pages.Count == 0)
          {
            if (Session.LoanDataMgr.FileAttachments.Remove(RemoveReasonType.User, (FileAttachment) attachment))
              DeleteAttachmentWarningDialog.ShowDeleteWarning((IWin32Window) this);
          }
          else
            imageAttachmentList.Add(attachment);
        }
        if (imageAttachmentList.Count == 0)
        {
          this.CloseFile();
        }
        else
        {
          this.imageViewer.LoadAttachments(imageAttachmentList.ToArray());
          this.imageViewer.BringToFront();
          this.currentViewer = this.imageViewer.Name;
        }
      }
      else if (flag4 && !flag3 && !flag1)
      {
        this.cloudViewer.LoadFiles(attachmentList);
        this.cloudViewer.BringToFront();
        this.currentViewer = this.cloudViewer.Name;
      }
      else
      {
        this.nativeViewer.LoadFiles(attachmentList);
        this.nativeViewer.BringToFront();
        this.currentViewer = this.nativeViewer.Name;
      }
    }

    public void LoadFiles(string[] fileIdList)
    {
      this.attachmentIdList = fileIdList;
      this.skyDriveViewer.LoadFiles(this.attachmentIdList);
      this.skyDriveViewer.BringToFront();
      this.currentViewer = this.skyDriveViewer.Name;
    }

    public void CloseFile()
    {
      if (this.currentViewer == this.pctEmpty.Name)
        return;
      if (this.currentViewer == this.nativeViewer.Name)
        this.nativeViewer.CloseFile();
      else if (this.currentViewer == this.imageViewer.Name)
        this.imageViewer.CloseAttachments();
      else if (this.currentViewer == this.cloudViewer.Name)
        this.cloudViewer.CloseFile();
      this.pctEmpty.BringToFront();
      this.currentViewer = this.pctEmpty.Name;
    }

    internal void BrowserDragDrop(ViewerMessage2 viewerMessage)
    {
      if (!(this.currentViewer == this.cloudViewer.Name))
        return;
      this.cloudViewer.BrowserDragDrop(viewerMessage);
    }

    public bool CanCloseViewer()
    {
      return !(this.currentViewer == this.cloudViewer.Name) || this.cloudViewer.CanCloseViewer();
    }

    public bool IsCloudViewer() => this.currentViewer == this.cloudViewer.Name;

    private void imageViewer_CloseFile(object sender, EventArgs e) => this.CloseFile();

    private void imageViewer_ImageUpdated(object sender, EventArgs e)
    {
      this.OnAttachmentUpdated(EventArgs.Empty);
    }

    private void nativeViewer_LoadAttachments(object sender, EventArgs e)
    {
      this.CloseFile();
      this.attachmentList = this.nativeViewer.NewAttachments;
      this.LoadFiles(this.attachmentList);
      this.OnLoadAttachments(EventArgs.Empty);
    }

    private void cloudViewer_FileChanged(object source, FileAttachmentEventArgs e)
    {
      this.CloseFile();
      this.OnViewerFileChanged(e);
    }

    private void cloudViewer_MergeFileSelected(object source, FileAttachmentEventArgs e)
    {
      this.CloseFile();
      this.OnViewerMergeFileSelected(e);
      this.cloudViewer.BringToFront();
    }

    private void nativeViewer_FileUpdated(object sender, EventArgs e)
    {
      this.OnAttachmentUpdated(EventArgs.Empty);
    }

    protected virtual void OnLoadAttachments(EventArgs e)
    {
      if (this.LoadAttachments == null)
        return;
      this.LoadAttachments((object) this, e);
    }

    public event EventHandler LoadAttachments;

    protected virtual void OnAttachmentUpdated(EventArgs e)
    {
      if (this.AttachmentUpdated == null)
        return;
      this.AttachmentUpdated((object) this, e);
    }

    public event EventHandler AttachmentUpdated;

    protected virtual void OnViewerFileChanged(FileAttachmentEventArgs e)
    {
      if (this.ViewerFileChanged == null)
        return;
      this.ViewerFileChanged((object) this, e);
    }

    public event FileAttachmentEventHandler ViewerFileChanged;

    protected virtual void OnViewerMergeFileSelected(FileAttachmentEventArgs e)
    {
      if (this.ViewerMergeFileSelected == null)
        return;
      this.ViewerMergeFileSelected((object) this, e);
    }

    public event FileAttachmentEventHandler ViewerMergeFileSelected;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pctEmpty = new PictureBox();
      this.backgroundViewer = new PictureBox();
      this.imageViewer = new ImageAttachmentViewerControl();
      this.nativeViewer = new NativeAttachmentViewerControl();
      this.cloudViewer = new CloudAttachmentViewerControl();
      this.skyDriveViewer = new SkyDriveViewerControl();
      ((ISupportInitialize) this.pctEmpty).BeginInit();
      ((ISupportInitialize) this.backgroundViewer).BeginInit();
      this.SuspendLayout();
      this.pctEmpty.BackColor = Color.WhiteSmoke;
      this.pctEmpty.Image = (Image) Resources.file_viewer;
      this.pctEmpty.Location = new Point(12, 12);
      this.pctEmpty.Name = "pctEmpty";
      this.pctEmpty.Size = new Size(132, 192);
      this.pctEmpty.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pctEmpty.TabIndex = 3;
      this.pctEmpty.TabStop = false;
      this.backgroundViewer.BackColor = Color.WhiteSmoke;
      this.backgroundViewer.Image = (Image) Resources.background_viewer;
      this.backgroundViewer.Location = new Point(736, 12);
      this.backgroundViewer.Name = "backgroundViewer";
      this.backgroundViewer.Size = new Size(152, 188);
      this.backgroundViewer.SizeMode = PictureBoxSizeMode.CenterImage;
      this.backgroundViewer.TabIndex = 3;
      this.backgroundViewer.TabStop = false;
      this.imageViewer.Location = new Point(420, 12);
      this.imageViewer.Name = "imageViewer";
      this.imageViewer.Size = new Size(304, 192);
      this.imageViewer.TabIndex = 1;
      this.imageViewer.CloseFile += new EventHandler(this.imageViewer_CloseFile);
      this.imageViewer.ImageUpdated += new EventHandler(this.imageViewer_ImageUpdated);
      this.nativeViewer.Location = new Point(156, 12);
      this.nativeViewer.Name = "nativeViewer";
      this.nativeViewer.Size = new Size(252, 192);
      this.nativeViewer.TabIndex = 0;
      this.nativeViewer.LoadAttachments += new EventHandler(this.nativeViewer_LoadAttachments);
      this.nativeViewer.FileUpdated += new EventHandler(this.nativeViewer_FileUpdated);
      this.cloudViewer.Location = new Point(740, 12);
      this.cloudViewer.MergeJobId = (string) null;
      this.cloudViewer.Name = "cloudViewer";
      this.cloudViewer.Size = new Size(75, 90);
      this.cloudViewer.TabIndex = 4;
      this.cloudViewer.ViewerFileChanged += new FileAttachmentEventHandler(this.cloudViewer_FileChanged);
      this.cloudViewer.ViewerMergeFileSelected += new FileAttachmentEventHandler(this.cloudViewer_MergeFileSelected);
      this.skyDriveViewer.AutoSize = true;
      this.skyDriveViewer.Location = new Point(740, 108);
      this.skyDriveViewer.Name = "skyDriveViewer";
      this.skyDriveViewer.Size = new Size(348, 90);
      this.skyDriveViewer.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.imageViewer);
      this.Controls.Add((Control) this.nativeViewer);
      this.Controls.Add((Control) this.pctEmpty);
      this.Controls.Add((Control) this.backgroundViewer);
      this.Controls.Add((Control) this.cloudViewer);
      this.Controls.Add((Control) this.skyDriveViewer);
      this.Name = nameof (FileAttachmentViewerControl);
      this.Size = new Size(904, 220);
      this.Resize += new EventHandler(this.FileAttachmentViewerControl_Resize);
      ((ISupportInitialize) this.pctEmpty).EndInit();
      ((ISupportInitialize) this.backgroundViewer).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
