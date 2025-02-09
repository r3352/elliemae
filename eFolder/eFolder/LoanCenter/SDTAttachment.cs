// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.SDTAttachment
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class SDTAttachment : LoanCenterAttachment
  {
    private DocumentLog[] docList;

    public SDTAttachment(string filepath, DocumentLog[] docList)
      : base(filepath, "MERGED_DOCUMENT")
    {
      this.docList = docList;
    }

    public DocumentLog[] Documents => this.docList;
  }
}
