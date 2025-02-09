// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Files.DragDropDetailCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.eFolder.Files
{
  public static class DragDropDetailCollection
  {
    private static List<DragDropDetail> dragDropDetails = new List<DragDropDetail>();

    public static void AddDragDropDetail(DragDropDetail detail)
    {
      string jobId = DragDropDetailCollection.GetJobId(detail.loanId, detail.attachmentId);
      if (!string.IsNullOrEmpty(jobId))
        DragDropDetailCollection.RemoveAttachmentJobId(jobId, detail.attachmentId);
      DragDropDetailCollection.dragDropDetails.Add(detail);
    }

    public static string GetJobId(string loanId, string attachmentId)
    {
      DragDropDetail dragDropDetail = DragDropDetailCollection.dragDropDetails.SingleOrDefault<DragDropDetail>((Func<DragDropDetail, bool>) (d => d.loanId == loanId && d.attachmentId == attachmentId));
      return dragDropDetail != null ? dragDropDetail.jobId : string.Empty;
    }

    public static void RemoveJobId(string jobId)
    {
      DragDropDetailCollection.dragDropDetails.RemoveAll((Predicate<DragDropDetail>) (d => d.jobId == jobId));
    }

    public static void RemoveAttachmentJobId(string jobId, string attachmentId)
    {
      DragDropDetailCollection.dragDropDetails.RemoveAll((Predicate<DragDropDetail>) (d => d.jobId == jobId && d.attachmentId == attachmentId));
    }

    public static string[] GetHiddenAttachmentIds(string jobId)
    {
      List<string> stringList = new List<string>();
      foreach (DragDropDetail dragDropDetail in DragDropDetailCollection.dragDropDetails)
      {
        if (dragDropDetail.jobId == jobId && dragDropDetail.hideAttachment)
          stringList.Add(dragDropDetail.attachmentId);
      }
      return stringList.ToArray();
    }

    public static bool IsAttachmentHidden(string loanId, string attachmentId)
    {
      foreach (DragDropDetail dragDropDetail in DragDropDetailCollection.dragDropDetails)
      {
        if (dragDropDetail.loanId == loanId && dragDropDetail.attachmentId == attachmentId && dragDropDetail.hideAttachment)
          return true;
      }
      return false;
    }
  }
}
