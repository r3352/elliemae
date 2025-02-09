// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic.SDCAnnotationsHelper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic
{
  public class SDCAnnotationsHelper
  {
    private const string className = "SDCAnnotationsHelper�";
    private static readonly string sw = Tracing.SwEFolder;

    public bool MoveSplitPageAnnotations(
      SDCDocument sourceDocumentCopy,
      SDCDocument separatedJson,
      List<int> pageIndexes)
    {
      int index1 = 0;
      bool flag = false;
      SDCDocument sdcDocument = Utils.DeepClone<SDCDocument>(sourceDocumentCopy);
      try
      {
        foreach (int pageIndex in pageIndexes)
        {
          int pageId = pageIndex;
          int separatedJsonPageId = separatedJson.Pages[index1].Id;
          if (sdcDocument.Annotations != null && sdcDocument.Annotations.Any<Annotation>((Func<Annotation, bool>) (annots => annots.PageId == pageId)))
          {
            flag = true;
            if (separatedJson.Annotations == null)
              separatedJson.Annotations = new List<Annotation>();
            separatedJson.Annotations.AddRange((IEnumerable<Annotation>) sdcDocument.Annotations.Where<Annotation>((Func<Annotation, bool>) (annots => annots.PageId == pageId)).ToList<Annotation>());
            IEnumerable<Annotation> source = separatedJson.Annotations.Where<Annotation>((Func<Annotation, bool>) (annots => annots.PageId == pageId));
            for (int index2 = 0; index2 < source.Count<Annotation>(); ++index2)
              source.All<Annotation>((Func<Annotation, bool>) (annots =>
              {
                annots.PageId = separatedJsonPageId;
                return true;
              }));
          }
          ++index1;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCAnnotationsHelper.sw, TraceLevel.Error, nameof (SDCAnnotationsHelper), string.Format("SkyDriveClassic: Error while moving Annotations from source to new document. Ex: {0}", (object) ex));
        throw;
      }
      return flag;
    }

    public void MergeFilesAnnotations(
      SDCDocument sdcDocumentCopy,
      Annotation fileAnnotation,
      int pageCounter = 0)
    {
      Annotation annotation = new Annotation()
      {
        AnnotationId = fileAnnotation.AnnotationId,
        PageId = pageCounter,
        Title = fileAnnotation.Title,
        Content = fileAnnotation.Content,
        CreatedBy = fileAnnotation.CreatedBy,
        Visibility = fileAnnotation.Visibility,
        Position = fileAnnotation.Position
      };
      if (sdcDocumentCopy.Annotations == null)
        sdcDocumentCopy.Annotations = new List<Annotation>()
        {
          annotation
        };
      else
        sdcDocumentCopy.Annotations.Add(annotation);
    }

    public static void RebaseAnnotationPageIds(
      List<Annotation> annotations,
      Dictionary<int, int> pageIdMap)
    {
      foreach (int key1 in pageIdMap.Keys)
      {
        int key = key1;
        foreach (Annotation annotation in annotations.FindAll((Predicate<Annotation>) (a => a.PageId == key)))
          annotation.PageId = pageIdMap[key];
      }
    }
  }
}
