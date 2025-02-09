// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic.SDCBookmarksHelper
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.eFolder.SkyDriveClassic;
using EllieMae.EMLite.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.eFolder.SkyDriveClassic
{
  public class SDCBookmarksHelper
  {
    private const string className = "SDCBookmarksHelper�";
    private static readonly string sw = Tracing.SwEFolder;

    public List<Bookmark> RebaseChildBookMark(
      List<Bookmark> bookmarks,
      Dictionary<int, int> pageLookOut)
    {
      try
      {
        JArray jarray = JArray.FromObject((object) bookmarks);
        string propToRebase = "PageId";
        foreach (JProperty jproperty in jarray.Descendants().OfType<JProperty>().Where<JProperty>((Func<JProperty, bool>) (p => propToRebase.Equals(p.Name))))
        {
          int num = 0;
          if (pageLookOut.TryGetValue(int.Parse(jproperty.Value.ToString()), out num))
            jproperty.Value = (JToken) num;
        }
        return jarray.ToObject<List<Bookmark>>();
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error in rebasing child bookMark. Ex: {0}", (object) ex));
        throw;
      }
    }

    public bool MoveSplitPageBookMarks(
      SDCDocument sourceDocument,
      SDCDocument separatedJson,
      List<int> pageIndexes)
    {
      bool flag = false;
      List<Bookmark> bookmarks = (List<Bookmark>) null;
      SDCDocument sdcDocument = Utils.DeepClone<SDCDocument>(sourceDocument);
      try
      {
        if (sdcDocument.Bookmarks != null && this.SourceContainsBookMarkForSplit(sdcDocument.Bookmarks, pageIndexes))
        {
          foreach (Bookmark bookmark in sdcDocument.Bookmarks)
            bookmarks = this.AddBookmarkRecursively(bookmark, pageIndexes, new List<Bookmark>(), 0, 0);
        }
        if (bookmarks != null)
        {
          flag = true;
          List<Bookmark> collection = this.RebasePageIndexesForSplitBookMarks(bookmarks, pageIndexes);
          if (separatedJson.Bookmarks == null)
            separatedJson.Bookmarks = new List<Bookmark>();
          separatedJson.Bookmarks.AddRange((IEnumerable<Bookmark>) collection);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error while moveing bookmarks from source document to new document. Ex: {0}", (object) ex));
        throw;
      }
      return flag;
    }

    public List<Bookmark> RebasePageIndexesForSplitBookMarks(
      List<Bookmark> bookmarks,
      List<int> pageIndexes)
    {
      JArray jarray = JArray.FromObject((object) bookmarks);
      string propToRebase = "PageId";
      IEnumerable<JProperty> jproperties = jarray.Descendants().OfType<JProperty>().Where<JProperty>((Func<JProperty, bool>) (p => propToRebase.Equals(p.Name)));
      for (int index = 1; index <= pageIndexes.Count; ++index)
      {
        foreach (JProperty jproperty in jproperties)
        {
          if (int.Parse(jproperty.Value.ToString()) == pageIndexes[index - 1])
            jproperty.Value = (JToken) index;
        }
      }
      return jarray.ToObject<List<Bookmark>>();
    }

    public bool SourceContainsBookMarkForSplit(List<Bookmark> bookmarks, List<int> pageIndexes)
    {
      JArray jarray = JArray.FromObject((object) bookmarks);
      string propToRebase = "PageId";
      IEnumerable<JProperty> jproperties = jarray.Descendants().OfType<JProperty>().Where<JProperty>((Func<JProperty, bool>) (p => propToRebase.Equals(p.Name)));
      for (int index = 1; index <= pageIndexes.Count; ++index)
      {
        foreach (JProperty jproperty in jproperties)
        {
          if (pageIndexes.Contains(int.Parse(jproperty.Value.ToString())))
            return true;
        }
      }
      return false;
    }

    public List<Bookmark> AddBookmarkRecursively(
      Bookmark sourceBookmark,
      List<int> splitPages,
      List<Bookmark> parentBookmarks,
      int currentOutlineLevel,
      int moveBookmarkLevel)
    {
      try
      {
        if (splitPages.Contains(sourceBookmark.PageId))
        {
          List<Bookmark> bookmarkList = new List<Bookmark>();
          if (sourceBookmark.Children != null)
          {
            bookmarkList.AddRange((IEnumerable<Bookmark>) sourceBookmark.Children);
            sourceBookmark.Children = (List<Bookmark>) null;
          }
          if (currentOutlineLevel == moveBookmarkLevel)
            parentBookmarks.Add(sourceBookmark);
          else
            this.AddChildren(parentBookmarks[parentBookmarks.Count - 1], sourceBookmark, currentOutlineLevel - moveBookmarkLevel);
          if (bookmarkList.Count > 0)
          {
            ++currentOutlineLevel;
            foreach (Bookmark sourceBookmark1 in bookmarkList)
              parentBookmarks = this.AddBookmarkRecursively(sourceBookmark1, splitPages, parentBookmarks, currentOutlineLevel, moveBookmarkLevel);
          }
        }
        else
        {
          if (sourceBookmark.Children == null || sourceBookmark.Children.Count <= 0)
            return parentBookmarks;
          ++currentOutlineLevel;
          ++moveBookmarkLevel;
          foreach (Bookmark child in sourceBookmark.Children)
            parentBookmarks = this.AddBookmarkRecursively(child, splitPages, parentBookmarks, currentOutlineLevel, moveBookmarkLevel);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error while adding bookmarks recursively. Ex: {0}", (object) ex));
        throw;
      }
      return parentBookmarks;
    }

    private void AddChildren(Bookmark parentBookmark, Bookmark bookmark, int outlineLevel)
    {
      try
      {
        if (outlineLevel == 1)
        {
          if (parentBookmark.Children == null)
            parentBookmark.Children = new List<Bookmark>();
          parentBookmark.Children.Add(bookmark);
        }
        else
          this.AddChildren(parentBookmark.Children[parentBookmark.Children.Count - 1], bookmark, --outlineLevel);
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error in adding child bookmarks. Ex: {0}", (object) ex));
        throw;
      }
    }

    public IList<Bookmark> MapEmbeddedBoomarks(IList<PdfEmbeddedBoomark> embeddedBoomarks)
    {
      List<Bookmark> bookmarkList = new List<Bookmark>(embeddedBoomarks.Count);
      try
      {
        foreach (PdfEmbeddedBoomark embeddedBoomark in (IEnumerable<PdfEmbeddedBoomark>) embeddedBoomarks)
        {
          string[] strArray = embeddedBoomark.Page.Split(' ');
          Bookmark bookmark = new Bookmark()
          {
            Id = Guid.NewGuid().ToString(),
            Name = embeddedBoomark.Title,
            Type = strArray[1],
            Top = double.Parse(strArray[strArray.Length - 1]),
            Left = strArray.Length <= 2 ? 0.0 : double.Parse(strArray[2]),
            PageId = int.Parse(strArray[0]),
            Children = embeddedBoomark.Kids == null || embeddedBoomark.Kids.Count <= 0 ? (List<Bookmark>) null : (List<Bookmark>) this.MapEmbeddedBoomarks((IList<PdfEmbeddedBoomark>) embeddedBoomark.Kids)
          };
          bookmarkList.Add(bookmark);
        }
        return (IList<Bookmark>) bookmarkList;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error in mapping pdf embedded boomarks to document json bookmarks. Exception : {0}", (object) ex));
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
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error in mapping document json bookmarks to Pdf Embedded Boomarks. Exception : {0}", (object) ex));
        throw;
      }
    }

    public List<Dictionary<string, object>> MapPdfBookmarksToSimpleBookMarks(
      IList<PdfEmbeddedBoomark> pdfEmbeddedBoomarks)
    {
      List<Dictionary<string, object>> simpleBookMarks = new List<Dictionary<string, object>>(pdfEmbeddedBoomarks.Count);
      try
      {
        foreach (PdfEmbeddedBoomark pdfEmbeddedBoomark in (IEnumerable<PdfEmbeddedBoomark>) pdfEmbeddedBoomarks)
        {
          Dictionary<string, object> dictionary = new Dictionary<string, object>()
          {
            ["Action"] = (object) "GoTo",
            ["Title"] = (object) pdfEmbeddedBoomark.Title,
            ["Page"] = (object) pdfEmbeddedBoomark.Page,
            ["Color"] = (object) "0 0 0",
            ["Kids"] = pdfEmbeddedBoomark.Kids == null || pdfEmbeddedBoomark.Kids.Count <= 0 ? (object) (List<Dictionary<string, object>>) null : (object) this.MapPdfBookmarksToSimpleBookMarks((IList<PdfEmbeddedBoomark>) pdfEmbeddedBoomark.Kids)
          };
          simpleBookMarks.Add(dictionary);
        }
        return simpleBookMarks;
      }
      catch (Exception ex)
      {
        Tracing.Log(SDCBookmarksHelper.sw, TraceLevel.Error, nameof (SDCBookmarksHelper), string.Format("SkyDriveClassic: Error in mapping Pdf Embedded Bookmarks to SimpleBookMarks. Exception : {0}", (object) ex));
        throw;
      }
    }
  }
}
