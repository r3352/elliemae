// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic.SDCMapper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocumentConverter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic
{
  public class SDCMapper
  {
    private const string className = "SDCMapper�";
    private static readonly string sw = Tracing.SwEFolder;

    public void AnnotationMapper(
      SDCDocument sdcDocumentCopy,
      PdfTextAnnotation annotation,
      string userId,
      string userName,
      string createdDate,
      int annotationX,
      int annotationY)
    {
      try
      {
        if (annotation == null)
          return;
        if (sdcDocumentCopy.Annotations == null)
          sdcDocumentCopy.Annotations = new List<Annotation>()
          {
            new Annotation()
            {
              AnnotationId = annotation.AnnotationGuid,
              CreatedBy = new CreatedBy()
            }
          };
        else
          sdcDocumentCopy.Annotations.Add(new Annotation()
          {
            AnnotationId = annotation.AnnotationGuid,
            CreatedBy = new CreatedBy()
          });
        Annotation annotation1 = sdcDocumentCopy.Annotations.Where<Annotation>((Func<Annotation, bool>) (a => a.AnnotationId == annotation.AnnotationGuid)).FirstOrDefault<Annotation>();
        Pages page = sdcDocumentCopy.Pages[annotation.PageIndex - 1];
        annotation1.PageId = page.Id;
        annotation1.Title = annotation.Title;
        annotation1.Content = annotation.Contents;
        annotation1.Position = new List<double>()
        {
          (double) annotationX,
          (double) annotationY,
          20.0,
          20.0
        };
        annotation1.CreatedBy.EntityId = userId;
        annotation1.CreatedBy.EntityName = userName;
        annotation1.CreatedBy.EntityType = "encompass";
        annotation1.CreatedBy.CreatedDate = createdDate;
        int index = sdcDocumentCopy.Annotations.IndexOf(sdcDocumentCopy.Annotations.Find((Predicate<Annotation>) (a => a.AnnotationId == annotation.AnnotationGuid)));
        sdcDocumentCopy.Annotations[index] = annotation1;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCMapper.sw, TraceLevel.Error, nameof (SDCMapper), string.Format("SkyDriveClassic: Error in mapping annotations. Ex: {0}", (object) ex));
        throw;
      }
    }

    public List<PdfTextAnnotation> PdfTextAnnotationMapper(SDCDocument doc)
    {
      List<PdfTextAnnotation> pdfTextAnnotationList = new List<PdfTextAnnotation>();
      foreach (Annotation annotation1 in doc.Annotations)
      {
        Annotation annotation = annotation1;
        PdfTextAnnotation pdfTextAnnotation = new PdfTextAnnotation()
        {
          Title = annotation.Title,
          Contents = annotation.Content,
          Color = Color.FromArgb(254, 228, 57),
          Icon = "Comment",
          PageIndex = doc.Pages.FindIndex((Predicate<Pages>) (x => x.Id == annotation.PageId)) + 1,
          X = (float) annotation.Position[0],
          Y = (float) annotation.Position[1]
        };
        pdfTextAnnotationList.Add(pdfTextAnnotation);
      }
      return pdfTextAnnotationList;
    }

    public void PageRotationMapper(SDCDocument sdcDocumentCopy, int pageId, int rotation)
    {
      try
      {
        int pageRotation = sdcDocumentCopy.Pages.Where<Pages>((Func<Pages, bool>) (page => page.Id == pageId)).ToList<Pages>().Select<Pages, int>((Func<Pages, int>) (r => r.Rotation)).Single<int>();
        if (pageRotation >= 360)
          pageRotation -= 360 + rotation;
        else
          pageRotation += rotation;
        if (pageRotation == 360)
          pageRotation = 0;
        sdcDocumentCopy.Pages.Where<Pages>((Func<Pages, bool>) (page => page.Id == pageId)).ToList<Pages>().ForEach((Action<Pages>) (r => r.Rotation = pageRotation));
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCMapper.sw, TraceLevel.Error, nameof (SDCMapper), string.Format("SkyDriveClassic: Error in mapping page rotation. Ex: {0}", (object) ex));
        throw;
      }
    }

    public IList<Bookmark> MapEmbeddedBookmarks(IList<PdfEmbeddedBoomark> embeddedBookmarks)
    {
      List<Bookmark> bookmarkList = new List<Bookmark>(embeddedBookmarks.Count);
      try
      {
        foreach (PdfEmbeddedBoomark embeddedBookmark in (IEnumerable<PdfEmbeddedBoomark>) embeddedBookmarks)
        {
          string[] strArray = embeddedBookmark.Page.Split(' ');
          Bookmark bookmark = new Bookmark()
          {
            Id = Guid.NewGuid().ToString(),
            Name = embeddedBookmark.Title,
            Type = strArray[1],
            Top = double.Parse(strArray[strArray.Length - 1]),
            Left = strArray.Length <= 2 ? 0.0 : double.Parse(strArray[2]),
            PageId = int.Parse(strArray[0]),
            Children = embeddedBookmark.Kids == null || embeddedBookmark.Kids.Count <= 0 ? (List<Bookmark>) null : (List<Bookmark>) this.MapEmbeddedBookmarks((IList<PdfEmbeddedBoomark>) embeddedBookmark.Kids)
          };
          bookmarkList.Add(bookmark);
        }
        return (IList<Bookmark>) bookmarkList;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCMapper.sw, TraceLevel.Error, nameof (SDCMapper), string.Format("SkyDriveClassic: Error in mapping pdf embedded bookmarks to document json bookmarks. Exception : {0}", (object) ex));
        throw;
      }
    }

    public IList<PdfEmbeddedBoomark> MapBookmarksToPdfBookMarks(IList<Bookmark> bookmarks)
    {
      IList<PdfEmbeddedBoomark> pdfBookMarks = (IList<PdfEmbeddedBoomark>) new List<PdfEmbeddedBoomark>(bookmarks.Count);
      try
      {
        foreach (Bookmark bookmark in (IEnumerable<Bookmark>) bookmarks)
        {
          string str = string.Format("{0} {1}", (object) bookmark.PageId, (object) bookmark.Type);
          if (bookmark.Left != 0.0)
            str = string.Format("{0} {1}", (object) str, (object) bookmark.Left);
          if (bookmark.Top != 0.0)
            str = string.Format("{0} {1}", (object) str, (object) bookmark.Top);
          pdfBookMarks.Add(new PdfEmbeddedBoomark()
          {
            Color = "0 0 0",
            Title = bookmark.Name,
            Page = str,
            Action = "GoTo",
            Kids = bookmark.Children == null || bookmark.Children.Count <= 0 ? (List<PdfEmbeddedBoomark>) null : (List<PdfEmbeddedBoomark>) this.MapBookmarksToPdfBookMarks((IList<Bookmark>) bookmark.Children)
          });
        }
        return pdfBookMarks;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCMapper.sw, TraceLevel.Error, nameof (SDCMapper), string.Format("SkyDriveClassic: Error in mapping document json bookmarks to Pdf Embedded Bookmarks. Exception : {0}", (object) ex));
        throw;
      }
    }

    public List<Dictionary<string, object>> MapPdfBookmarksToSimpleBookMarks(
      IList<PdfEmbeddedBoomark> pdfEmbeddedBookmarks)
    {
      List<Dictionary<string, object>> simpleBookMarks = new List<Dictionary<string, object>>(pdfEmbeddedBookmarks.Count);
      try
      {
        foreach (PdfEmbeddedBoomark embeddedBookmark in (IEnumerable<PdfEmbeddedBoomark>) pdfEmbeddedBookmarks)
        {
          Dictionary<string, object> dictionary = new Dictionary<string, object>()
          {
            ["Action"] = (object) "GoTo",
            ["Title"] = (object) embeddedBookmark.Title,
            ["Page"] = (object) embeddedBookmark.Page,
            ["Color"] = (object) "0 0 0",
            ["Kids"] = embeddedBookmark.Kids == null || embeddedBookmark.Kids.Count <= 0 ? (object) (List<Dictionary<string, object>>) null : (object) this.MapPdfBookmarksToSimpleBookMarks((IList<PdfEmbeddedBoomark>) embeddedBookmark.Kids)
          };
          simpleBookMarks.Add(dictionary);
        }
        return simpleBookMarks;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCMapper.sw, TraceLevel.Error, nameof (SDCMapper), string.Format("SkyDriveClassic: Error in mapping Pdf Embedded Bookmarks to SimpleBookMarks. Exception : {0}", (object) ex));
        throw;
      }
    }
  }
}
