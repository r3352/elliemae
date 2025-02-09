// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.AttachmentAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class AttachmentAccessor : IAttachmentAccessor, IDisposable
  {
    private const string ClassName = "AttachmentAccessor�";
    private readonly IDbContext _dbContext;

    public AttachmentAccessor(IDbContext dbContext) => this._dbContext = dbContext;

    public void UpdateLoanAttachmentsToDB(int loanXRefId, FileAttachment[] attachments)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (FileAttachment attachment in attachments)
          stringBuilder.AppendLine("UPDATE [LoanFileAttachments] SET [Type] = '" + (object) (int) attachment.AttachmentType + "',[IsRemoved] = " + (object) Convert.ToInt16(attachment.IsRemoved) + ",[XMLData] = N" + SQL.Encode((object) attachment.ToXml().OuterXml) + ",[LastModifiedBy] = " + SQL.Encode((object) attachment.UserID) + ",[LastModifiedDate] = GetDate() WHERE [AttachmentId]=" + SQL.Encode((object) attachment.ID) + " and [LoanXRefId]=" + loanXRefId.ToString() ?? "");
        if (stringBuilder.Length <= 0)
          return;
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          service.ExecuteNonQuery(stringBuilder.ToString());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
    }

    public FileAttachment[] GetLoanAttachmentsFromDatabase(
      int loanXRefId,
      IEnumerable<string> attachmentIds,
      bool includeRemoved = true)
    {
      try
      {
        DataRowCollection dataRowCollection = (DataRowCollection) null;
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          dataRowCollection = service.ExecuteQuery("select XMLData,IsRemoved from [LoanFileAttachments] where " + (attachmentIds == null || attachmentIds.Count<string>() <= 0 ? "" : " AttachmentId in (" + string.Join(",", attachmentIds.Select<string, string>((System.Func<string, string>) (x => SQL.Encode((object) x)))) + ") and") + " LoanXRefId=" + loanXRefId.ToString() + (!includeRemoved ? " and IsRemoved = 0 " : ""));
        List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          XmlDocument xmlDocument = new XmlDocument();
          string xml = dataRow["xmlData"].ToString();
          if (!Convert.IsDBNull((object) xml) && !string.IsNullOrEmpty(xml))
          {
            xmlDocument.LoadXml(xml);
            FileAttachment fileAttachment = AttachmentAccessor.BuildAttachmentFromXML(xmlDocument.DocumentElement, Convert.ToBoolean(dataRow["isRemoved"]));
            if (fileAttachment != null)
              fileAttachmentList.Add(fileAttachment);
          }
        }
        return fileAttachmentList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
      return (FileAttachment[]) null;
    }

    public IEnumerable<Tuple<XmlElement, bool>> BuildAttachmentXmlFromDbForMigration(
      Guid loanId,
      int loanXrefId,
      bool isDataRequired,
      out string loanPropertiesFlag,
      out string instanceLevelFlag)
    {
      loanPropertiesFlag = "";
      instanceLevelFlag = "";
      try
      {
        List<Tuple<XmlElement, bool>> tupleList = (List<Tuple<XmlElement, bool>>) null;
        DataSet dataSet = (DataSet) null;
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
        {
          string sql = "DECLARE @LoanAttachmentsFlag nvarchar(1024), @CompanySettingsAttachmentsFlag nvarchar(1024); select @LoanAttachmentsFlag = Value from LoanProperties where GUID='{" + (object) loanId + "}' and Category='LoanStorage' and Attribute='AttachmentsMetaData'; select @CompanySettingsAttachmentsFlag = Value from company_settings where Category='LoanStorage' and Attribute='AttachmentsMetaData'; select @LoanAttachmentsFlag as LoanFlag, @CompanySettingsAttachmentsFlag as InstanceFlag; ";
          if (isDataRequired)
            sql = sql + "if @LoanAttachmentsFlag = 'DB' and @CompanySettingsAttachmentsFlag = 'CIFS' begin select XMLData,IsRemoved from [LoanFileAttachments] where  LoanXRefId=" + loanXrefId.ToString() + "; end";
          dataSet = service.ExecuteSetQuery(sql);
        }
        if (dataSet.Tables.Count > 0)
        {
          loanPropertiesFlag = dataSet.Tables[0].Rows[0]["LoanFlag"].ToString();
          instanceLevelFlag = dataSet.Tables[0].Rows[0]["InstanceFlag"].ToString();
        }
        if (dataSet.Tables.Count > 1)
        {
          tupleList = new List<Tuple<XmlElement, bool>>();
          foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[1].Rows)
          {
            XmlDocument xmlDocument = new XmlDocument();
            string xml = row["xmlData"].ToString();
            if (!Convert.IsDBNull((object) xml) && !string.IsNullOrEmpty(xml))
            {
              xmlDocument.LoadXml(xml);
              tupleList.Add(new Tuple<XmlElement, bool>(xmlDocument.DocumentElement, Convert.ToBoolean(row["isRemoved"])));
            }
          }
        }
        return (IEnumerable<Tuple<XmlElement, bool>>) tupleList;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
      return (IEnumerable<Tuple<XmlElement, bool>>) null;
    }

    public void InsertLoanAttachmentsToDB(
      Guid loanId,
      int loanXRefId,
      FileAttachment[] attachments,
      EllieMae.EMLite.Server.DbQueryBuilder sql = null)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        List<string> values = new List<string>();
        foreach (FileAttachment attachment in attachments)
          values.Add("('{" + loanId.ToString() + "}', " + (object) loanXRefId + ", " + SQL.Encode((object) attachment.ID) + ", " + (object) (int) attachment.AttachmentType + ", " + (object) Convert.ToInt16(attachment.IsRemoved) + ", N" + SQL.Encode((object) attachment.ToXml().OuterXml) + ", " + SQL.Encode((object) attachment.UserID) + ", " + SQL.Encode((object) attachment.UserID) + ")");
        if (values.Count <= 0)
          return;
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          service.ExecuteNonQuery("INSERT INTO LoanFileAttachments(LoanId, LoanXRefId, AttachmentId, Type, IsRemoved, XMLData, CreatedBy, LastModifiedBy) VALUES " + string.Join(",", (IEnumerable<string>) values));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
    }

    public void Dispose()
    {
    }

    public void DeleteAttachments(int loanXRefId, string attachmentId)
    {
      try
      {
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          service.ExecuteNonQuery("delete from LoanFileAttachments where AttachmentId=" + SQL.Encode((object) attachmentId) + " and LoanXRefId=" + loanXRefId.ToString() + ";");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
    }

    public void DeleteAttachments(int loanXRefId)
    {
      try
      {
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          service.ExecuteNonQuery("delete from LoanFileAttachments where LoanXRefId=" + loanXRefId.ToString() + ";");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
    }

    public void ReInsertData(Guid loanId, int loanXRefId, FileAttachment[] attachments)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        List<string> values = new List<string>();
        foreach (FileAttachment attachment in attachments)
          values.Add("('{" + loanId.ToString() + "}', " + (object) loanXRefId + ", " + SQL.Encode((object) attachment.ID) + ", " + (object) (int) attachment.AttachmentType + ", " + (object) Convert.ToInt16(attachment.IsRemoved) + ", N" + SQL.Encode((object) attachment.ToXml().OuterXml) + ", " + SQL.Encode((object) attachment.UserID) + ", " + SQL.Encode((object) attachment.UserID) + ")");
        if (values.Count <= 0)
          return;
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          service.ExecuteNonQuery("IF EXISTS(SELECT 1 from LoanFileAttachments where LoanXRefId=" + loanXRefId.ToString() + ") BEGIN delete from LoanFileAttachments where LoanXRefId=" + loanXRefId.ToString() + " END INSERT INTO LoanFileAttachments(LoanId, LoanXRefId, AttachmentId, Type, IsRemoved, XMLData, CreatedBy, LastModifiedBy) VALUES " + string.Join(",", (IEnumerable<string>) values));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AttachmentAccessor), ex);
      }
    }

    private static FileAttachment BuildAttachmentFromXML(XmlElement attachmentXml, bool isRemoved)
    {
      if (attachmentXml == null)
        return (FileAttachment) null;
      switch (attachmentXml.Name)
      {
        case "File":
          return (FileAttachment) new NativeAttachment(attachmentXml, isRemoved);
        case "Image":
          return (FileAttachment) new ImageAttachment(attachmentXml, isRemoved);
        case "Cloud":
          return (FileAttachment) new CloudAttachment(attachmentXml, isRemoved);
        case "Background":
          return (FileAttachment) new BackgroundAttachment(attachmentXml, isRemoved);
        default:
          return (FileAttachment) null;
      }
    }
  }
}
