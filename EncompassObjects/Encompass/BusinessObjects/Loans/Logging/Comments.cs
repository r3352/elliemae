// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Comment" /> objects.
  /// </summary>
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

    /// <summary>Gets the number of Comments in the collection.</summary>
    public int Count => this.comments.Count;

    /// <summary>Adds a comment to the collection.</summary>
    /// <param name="commentText">The text of the comment to be added.</param>
    /// <returns>Returns the new <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Comment" /> object.</returns>
    public Comment Add(string commentText)
    {
      CommentEntry commentEntry = new CommentEntry(commentText, this.logEntry.Loan.Session.UserID, this.logEntry.Loan.Session.GetCurrentUser().FullName, false);
      this.comments.Add(commentEntry);
      return new Comment(commentEntry);
    }

    /// <summary>Retrieves a comment from the collection by index.</summary>
    /// <param name="index">The index of the comment to retrieve.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Comment" /> at the specified index.</returns>
    public Comment this[int index]
    {
      get
      {
        this.cacheComments();
        return this.cachedComments[index];
      }
    }

    /// <summary>Returns an enumerator for the comments collection.</summary>
    /// <returns>Reurns an IEnumerator implementation for the collection.</returns>
    public IEnumerator GetEnumerator()
    {
      this.cacheComments();
      return (IEnumerator) this.cachedComments.GetEnumerator();
    }

    /// <summary>
    /// Ensures that all of the comments are wrapped up and added to the cachedComments collection.
    /// </summary>
    private void cacheComments()
    {
      for (int count = this.cachedComments.Count; count < this.comments.Count; ++count)
        this.cachedComments.Add(new Comment(this.comments[count]));
    }
  }
}
