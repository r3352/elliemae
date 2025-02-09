// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.SQLAttachmentXMLProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public sealed class SQLAttachmentXMLProvider : IAttachmentProvider, IAttachmentProviderBase
  {
    private EllieMae.EMLite.Server.DbQueryBuilder dbQuery = new EllieMae.EMLite.Server.DbQueryBuilder();

    public FileAttachment[] GetAttachments(
      Loan loan,
      IEnumerable<string> attachmentIds,
      bool includeRemoved = true)
    {
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
        return service.GetLoanAttachmentsFromDatabase(loan.Identity.XrefId, attachmentIds, includeRemoved);
    }

    public XmlDocument GetAttachmentXml(Loan loan) => this.GetAttachmentXml(loan.Identity.XrefId);

    public XmlDocument GetAttachmentXml(int loanXrefId)
    {
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml("<Attachments/>");
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      FileAttachment[] attachmentsFromDatabase;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
        attachmentsFromDatabase = service.GetLoanAttachmentsFromDatabase(loanXrefId);
      if (attachmentsFromDatabase != null)
      {
        foreach (FileAttachment attachment in attachmentsFromDatabase)
          FileAttachmentStore.writeAttachment(xmlDoc, attachment);
      }
      return xmlDoc;
    }

    public bool SaveAttachments(Loan loan, FileAttachment[] fileAttachments)
    {
      if (fileAttachments.Length == 0)
        return false;
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
      {
        IEnumerable<FileAttachment> source1 = (IEnumerable<FileAttachment>) null;
        FileAttachment[] existingAttachments = service.GetLoanAttachmentsFromDatabase(loan.Identity.XrefId, ((IEnumerable<FileAttachment>) fileAttachments).Select<FileAttachment, string>((Func<FileAttachment, string>) (s => s.ID)));
        IEnumerable<FileAttachment> source2;
        if (existingAttachments != null && existingAttachments.Length != 0)
        {
          source1 = ((IEnumerable<FileAttachment>) fileAttachments).Where<FileAttachment>((Func<FileAttachment, bool>) (attachment => ((IEnumerable<FileAttachment>) existingAttachments).Count<FileAttachment>() > 0 && ((IEnumerable<FileAttachment>) existingAttachments).Any<FileAttachment>((Func<FileAttachment, bool>) (existingAttachment => existingAttachment.ID == attachment.ID))));
          source2 = ((IEnumerable<FileAttachment>) fileAttachments).Where<FileAttachment>((Func<FileAttachment, bool>) (attachment => ((IEnumerable<FileAttachment>) existingAttachments).Count<FileAttachment>() <= 0 || !((IEnumerable<FileAttachment>) existingAttachments).Any<FileAttachment>((Func<FileAttachment, bool>) (existingAttachment => existingAttachment.ID == attachment.ID))));
        }
        else
          source2 = (IEnumerable<FileAttachment>) fileAttachments;
        if (source2 != null && source2.Count<FileAttachment>() > 0)
          service.InsertLoanAttachmentsToDB(new Guid(loan.Identity.Guid), loan.Identity.XrefId, source2.ToArray<FileAttachment>());
        if (source1 != null && source1.Count<FileAttachment>() > 0)
          service.UpdateLoanAttachmentsToDB(loan.Identity.XrefId, source1.ToArray<FileAttachment>());
        return source2 != null && source2.Count<FileAttachment>() > 0;
      }
    }

    public bool DeleteAttachments(Loan loan, FileAttachment attachment)
    {
      bool flag = false;
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
      {
        FileAttachment[] attachmentsFromDatabase = service.GetLoanAttachmentsFromDatabase(loan.Identity.XrefId, (IEnumerable<string>) new List<string>()
        {
          attachment.ID
        });
        if (attachmentsFromDatabase != null)
        {
          if (attachmentsFromDatabase.Length != 0)
          {
            service.DeleteAttachments(loan.Identity.XrefId, attachment.ID);
            flag = true;
          }
        }
      }
      return flag;
    }

    public void ReplaceAttachment(Loan loan, FileAttachment attachment)
    {
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
        service.UpdateLoanAttachmentsToDB(loan.Identity.XrefId, new FileAttachment[1]
        {
          attachment
        });
    }

    public FileAttachment[] GetAttachments(Loan loan, string[] attachmentIds)
    {
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
        return service.GetLoanAttachmentsFromDatabase(loan.Identity.XrefId, (IEnumerable<string>) attachmentIds);
    }

    public void DeleteAttachments(Loan loan) => this.DeleteAttachments(loan.Identity.XrefId);

    public void DeleteAttachments(int loanXRefId)
    {
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
        service.DeleteAttachments(loanXRefId);
    }

    public void ReInsertData(Loan loan, FileAttachment[] fileAttachments)
    {
      DataAccessFramework runtime = DataAccessFramework.Runtime;
      SQLDbContext sqlDbContext = new SQLDbContext();
      sqlDbContext.ConnectionString = ClientContext.GetCurrent().Settings.GetStorageModeSettings().SqlConnectionString;
      using (IAttachmentAccessor service = runtime.CreateService<IAttachmentAccessor>((IDbContext) sqlDbContext))
        service.ReInsertData(new Guid(loan.Identity.Guid), loan.Identity.XrefId, fileAttachments);
    }
  }
}
