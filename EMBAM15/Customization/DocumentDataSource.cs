// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.DocumentDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class DocumentDataSource(LoanData loan, UserInfo currentUser, bool readOnly) : 
    LoanDataSource(loan, currentUser, readOnly),
    IDocumentDataSource
  {
    public bool Exists(string docTitle) => this.getDocumentByTitle(docTitle) != null;

    public bool IsReceived(string docTitle)
    {
      DocumentLog documentByTitle = this.getDocumentByTitle(docTitle);
      return documentByTitle != null && documentByTitle.Received;
    }

    public bool IsExpired(string docTitle)
    {
      DocumentLog documentByTitle = this.getDocumentByTitle(docTitle);
      return documentByTitle != null && documentByTitle.IsExpired;
    }

    public bool IsOrdered(string docTitle)
    {
      DocumentLog documentByTitle = this.getDocumentByTitle(docTitle);
      return documentByTitle != null && documentByTitle.Requested;
    }

    private DocumentLog getDocumentByTitle(string docTitle)
    {
      foreach (DocumentLog documentByTitle in this.Loan.GetLogList().GetAllRecordsOfType(typeof (DocumentLog)))
      {
        if (string.Compare(documentByTitle.Title, docTitle, true) == 0)
          return documentByTitle;
      }
      return (DocumentLog) null;
    }
  }
}
