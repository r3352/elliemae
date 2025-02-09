// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.IAttachmentProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public interface IAttachmentProvider : IAttachmentProviderBase
  {
    FileAttachment[] GetAttachments(
      Loan loan,
      IEnumerable<string> attachmentIds,
      bool includeRemoved = true);

    FileAttachment[] GetAttachments(Loan loan, string[] attachmentIds);

    bool SaveAttachments(Loan loan, FileAttachment[] xmlDoc);

    void ReInsertData(Loan loan, FileAttachment[] xmlDoc);

    bool DeleteAttachments(Loan loan, FileAttachment attachment);

    void DeleteAttachments(Loan loan);

    void ReplaceAttachment(Loan loan, FileAttachment attachment);
  }
}
