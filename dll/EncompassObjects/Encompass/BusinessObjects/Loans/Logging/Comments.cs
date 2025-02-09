// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class Comments : IComments, IEnumerable
  {
    private LogEntry logEntry;
    private CommentEntryCollection comments;
    private List<Comment> cachedComments = new List<Comment>();

    internal Comments(LogEntry entry, CommentEntryCollection comments)
    {
      this.logEntry = entry;
      this.comments = comments;
    }

    public int Count => ((CollectionBase) this.comments).Count;

    public Comment Add(string commentText)
    {
      CommentEntry e = new CommentEntry(commentText, this.logEntry.Loan.Session.UserID, this.logEntry.Loan.Session.GetCurrentUser().FullName, false, false);
      this.comments.Add(e);
      return new Comment(e);
    }

    public Comment this[int index]
    {
      get
      {
        this.cacheComments();
        return this.cachedComments[index];
      }
    }

    public IEnumerator GetEnumerator()
    {
      this.cacheComments();
      return (IEnumerator) this.cachedComments.GetEnumerator();
    }

    private void cacheComments()
    {
      for (int count = this.cachedComments.Count; count < ((CollectionBase) this.comments).Count; ++count)
        this.cachedComments.Add(new Comment(this.comments[count]));
    }
  }
}
