// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Comment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single comment in a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.Comments" /> collection.
  /// </summary>
  public class Comment : IComment
  {
    private CommentEntry comment;

    internal Comment(CommentEntry e) => this.comment = e;

    /// <summary>Gets the text the comment.</summary>
    public string Text => this.comment.Comments;

    /// <summary>Gets the UserID of the user that added the comment.</summary>
    public string AddedBy => this.comment.AddedBy;

    /// <summary>Gets the date and time the comment was added.</summary>
    public DateTime DateAdded => this.comment.Date;
  }
}
