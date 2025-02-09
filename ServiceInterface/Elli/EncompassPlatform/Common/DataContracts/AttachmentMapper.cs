// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.AttachmentMapper
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  public class AttachmentMapper
  {
    public void WriteTo(FileAttachment source, UnassignedAttachmentGetContract target)
    {
      if (source == null)
        return;
      if (source.ID != null)
        target.AttachmentId = source.ID;
      target.DateCreated = source.Date;
      if (source.Title != null)
        target.Title = source.Title;
      target.FileSize = source.FileSize;
      if (source.UserID != null)
        target.CreatedBy = source.UserID;
      if (source.UserName != null)
        target.CreatedByName = source.UserName;
      switch (source)
      {
        case NativeAttachment _:
          target.AttachmentType = 0;
          break;
        case ImageAttachment _:
          target.AttachmentType = 1;
          break;
        case BackgroundAttachment _:
          target.AttachmentType = 2;
          break;
      }
    }

    public void WriteTo(
      NativeAttachment source,
      AttachmentGetContract target,
      string documentId,
      bool? isActive)
    {
      if (source == null)
        return;
      this.setCommonTargetProperties((FileAttachment) source, target, documentId, isActive);
      target.AttachmentType = 0;
      target.Rotation = source.Rotation;
    }

    public void WriteTo(
      ImageAttachment source,
      AttachmentGetContract target,
      string documentId,
      bool? isActive)
    {
      if (source == null)
        return;
      this.setCommonTargetProperties((FileAttachment) source, target, documentId, isActive);
      target.AttachmentType = 1;
      target.Pages = new List<PageImageContract>();
      foreach (PageImage page in source.Pages)
      {
        PageImageContract pageImageContract = new PageImageContract()
        {
          FileSize = page.FileSize,
          Height = page.Height,
          HorizontalResolution = page.HorizontalResolution,
          ImageKey = page.ImageKey,
          NativeKey = page.NativeKey,
          Rotation = page.Rotation,
          VerticalResolution = page.VerticalResolution,
          Width = page.Width,
          ZipKey = page.ZipKey
        };
        pageImageContract.Annotations = new List<PageAnnotationContract>();
        foreach (PageAnnotation annotation in page.Annotations)
        {
          PageAnnotationContract annotationContract = new PageAnnotationContract()
          {
            CreatedBy = annotation.AddedBy,
            DateCreated = annotation.Date,
            Height = annotation.Height,
            Left = annotation.Left,
            Text = annotation.Text,
            Top = annotation.Top,
            VisibilityType = (int) annotation.Visibility,
            Width = annotation.Width
          };
          pageImageContract.Annotations.Add(annotationContract);
        }
        PageThumbnailContract thumbnailContract = new PageThumbnailContract()
        {
          Height = page.Thumbnail.Height,
          HorizontalResolution = page.Thumbnail.HorizontalResolution,
          ImageKey = page.Thumbnail.ImageKey,
          VerticalResolution = page.Thumbnail.VerticalResolution,
          Width = page.Thumbnail.Width,
          ZipKey = page.Thumbnail.ZipKey
        };
        pageImageContract.Thumbnail = thumbnailContract;
        target.Pages.Add(pageImageContract);
      }
    }

    private void setCommonTargetProperties(
      FileAttachment source,
      AttachmentGetContract target,
      string documentId,
      bool? isActive)
    {
      if (source.ID != null)
        target.AttachmentId = source.ID;
      if (source.UserID != null)
        target.CreatedBy = source.UserID;
      if (source.UserName != null)
        target.CreatedByName = source.UserName;
      target.DateCreated = source.Date;
      if (source.Title != null)
        target.Title = source.Title;
      target.FileSize = source.FileSize;
      if (documentId != null)
        target.DocumentId = new Guid?(new Guid(documentId));
      target.IsActive = isActive;
    }
  }
}
