// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.VerificationDocumentList
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class VerificationDocumentList
  {
    private List<VerificationDocument> documents;

    public VerificationDocumentList() => this.documents = new List<VerificationDocument>();

    public void AddDocument(VerificationDocument document) => this.documents.Add(document);

    public void RemoveDocument(string docName)
    {
      foreach (VerificationDocument document in this.documents)
      {
        if (string.Compare(document.DocName, docName, true) == 0)
        {
          this.documents.Remove(document);
          break;
        }
      }
    }

    public int DocumentCount => this.documents.Count;

    public VerificationDocument GetDocumentAt(int i) => this.documents[i];

    public void Sort()
    {
      for (int index1 = 0; index1 < this.documents.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.documents.Count; ++index2)
        {
          if (this.documents[index1].Date < this.documents[index2].Date)
          {
            VerificationDocument document = this.documents[index1];
            this.documents[index1] = this.documents[index2];
            this.documents[index2] = document;
          }
        }
      }
    }
  }
}
