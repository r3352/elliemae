// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.FileFormats;
using Elli.Domain.JsonSerialization;
using Elli.Domain.JsonSerialization.JsonSerializers;
using Elli.Domain.JsonSerialization.Utils;
using Elli.Domain.Mortgage;
using Elli.Domain.Mortgage.Enums;
using Elli.ElliEnum;
using Elli.Metrics;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanAccessor : ILoanDataAccessor, IDisposable
  {
    private const string ClassName = "LoanAccessor�";
    private ILoanAccessorMetricsRecorder _loanAccessorMR;
    private ILoanSerializationMatrixRecorder _loanSerializationMR;
    private readonly ILoanSerializer _serializer;
    private readonly IDbContext _dbContext;

    public LoanAccessor(
      IDbContext dbContext,
      ILoanAccessorMetricsRecorder loanAccessorMetricsRecorder,
      ILoanSerializationMatrixRecorder loanSerializationMetricsRecorder)
    {
      this._dbContext = dbContext;
      this._loanAccessorMR = loanAccessorMetricsRecorder;
      this._loanSerializationMR = loanSerializationMetricsRecorder;
      this._serializer = (ILoanSerializer) new LoanSerializer(this._loanSerializationMR);
    }

    public string GetLoan(Guid loanId, LoanFileFormatType fileFormatType)
    {
      Elli.Domain.Mortgage.Loan loan = this.GetLoan(loanId);
      return loan == null ? (string) null : FileFormat.Create(fileFormatType, (string) null).WriteFile(loan);
    }

    public Elli.Domain.Mortgage.Loan GetLoan(Guid loanId)
    {
      IDataReader dataReader = (IDataReader) null;
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Get_Loan))
      {
        try
        {
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
            dataReader = service.ExecuteReaderQuery(this.GenerateSelectForLoan(loanId));
          List<LoanFragment> LoanPieces = new List<LoanFragment>();
          while (dataReader.Read())
          {
            LoanFragment loanFragment = new LoanFragment()
            {
              Data = (string) dataReader["EntityData"],
              EntityId = (Guid) dataReader["EntityId"],
              LoanId = loanId,
              EntityType = Type.GetType((string) dataReader["TypeName"] + ",Elli.Domain"),
              InstanceId = (string) dataReader["InstanceId"],
              ModelPathIndex = (int) dataReader["ModelpathIndex"]
            };
            LoanPieces.Add(loanFragment);
          }
          return LoanPieces.Count < 1 ? (Elli.Domain.Mortgage.Loan) null : this._serializer.DeserializeLoan(LoanPieces);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
        }
        finally
        {
          dataReader?.Close();
          this._loanAccessorMR.IncrementLoanGetCount();
        }
      }
      return (Elli.Domain.Mortgage.Loan) null;
    }

    public Elli.Domain.Mortgage.Loan SaveLoan(
      string loanXml,
      UserInfo currentUser,
      string loanFolder,
      string loanName,
      LoanFileFormatType fileFormatType)
    {
      try
      {
        if (string.IsNullOrEmpty(loanFolder) || string.IsNullOrEmpty(loanName))
          throw new ApplicationException("LoanFolder and/or LoanName cannot be null or empty");
        using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Save_Loan))
        {
          IFileFormat fileFormat = FileFormat.Create(fileFormatType, loanXml);
          Elli.Domain.Mortgage.Loan loan = fileFormat.ReadFile(Elli.Domain.Mortgage.Loan.Create(fileFormat.FileId));
          loan.AddLoanTag(new TagItem()
          {
            TagType = Enum.GetName(typeof (LoanTagType), (object) LoanTagType.LoanFolder),
            TagName = loanFolder
          });
          loan.AddLoanTag(new TagItem()
          {
            TagType = Enum.GetName(typeof (LoanTagType), (object) LoanTagType.LoanName),
            TagName = loanName
          });
          this.SaveLoan(loan);
          this._loanAccessorMR.IncrementLoanSaveCount();
          return loan;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanAccessor), ex);
        return (Elli.Domain.Mortgage.Loan) null;
      }
    }

    private void SaveLoan(Elli.Domain.Mortgage.Loan loan)
    {
      LoanSerializationLevel serializationLevel = LoanSerializationLevel.Full_Loan;
      if (loan.EncompassId == Guid.Empty)
        throw new ApplicationException("Loan request does not contains valid EncompassId, please verify request and retry");
      int? fileSequenceNumber = loan.Miscellaneous.LoanFileSequenceNumber;
      List<LoanFragment> fragments = this._serializer.SerializeLoan(loan, serializationLevel, this._dbContext.InstanceId);
      if (fragments == null)
        return;
      this.SaveLoanFragments((IEnumerable<LoanFragment>) fragments, loan.EncompassId, serializationLevel, fileSequenceNumber);
    }

    private void SaveLoanFragments(
      IEnumerable<LoanFragment> fragments,
      Guid encompassId,
      LoanSerializationLevel serializationLevel,
      int? loanVersion)
    {
      StringBuilder command = new StringBuilder();
      using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
      {
        if (serializationLevel.Has<LoanSerializationLevel>(LoanSerializationLevel.Logs))
          this.AppendDeleteAllLogsForLoan(command, encompassId);
        if (serializationLevel.Has<LoanSerializationLevel>(LoanSerializationLevel.SnapShots))
          this.AppendDeleteOptimalBlueSnapshotForLoan(command, encompassId);
        foreach (LoanFragment fragment in fragments)
          this.AppendUpsertLoanFragmentCommand(command, fragment);
        service.ExecuteNonQuery(command.ToString());
      }
    }

    private void AppendUpsertLoanFragmentCommand(StringBuilder command, LoanFragment fragment)
    {
      if (fragment.ToFragmentEntityTypeId() == 3)
        this.AddParametersToCommand("INSERT INTO ElliSnapshot(SnapshotType, TypeName, LoanId, ParentId, InstanceId, ModelpathIndex, SnapshotData) VALUES({0}) ON CONFLICT ON CONSTRAINT ElliSnapshot_UxSnapshotOnUpdateConstraint DO NOTHING;", this.UpsertSnapshotParameters(fragment), command);
      else if (fragment.ToFragmentEntityTypeId() == 2)
        this.AddParametersToCommand("INSERT INTO ElliLoan(EntityId, EntityType, TypeName, LoanId, InstanceId, ModelpathIndex, EntityData) VALUES({0});", this.UpsertElliLoanParameters(fragment), command);
      else
        this.AddParametersToCommand("INSERT INTO ElliLoan(EntityId, EntityType, TypeName, LoanId, InstanceId, ModelpathIndex, EntityData) VALUES({0}) ON CONFLICT ON CONSTRAINT ElliLoan_pkey DO UPDATE SET RowModified = EXCLUDED.RowModified, EntityData = EXCLUDED.EntityData;", this.UpsertElliLoanParameters(fragment), command);
    }

    private void AppendDeleteAllLogsForLoan(StringBuilder command, Guid loanId)
    {
      string format = string.Format("DELETE FROM ElliLoan where EntityType = 2 AND LoanId = {0};", (object) SQL.EncodeString(loanId.ToString(), true));
      command.AppendFormat(format);
    }

    private void AppendDeleteOptimalBlueSnapshotForLoan(StringBuilder command, Guid loanId)
    {
      string format = string.Format("DELETE FROM ElliSnapshot where TypeName = {0} AND LoanId = {1};", (object) SQL.EncodeString(typeof (OptimalBlueSnapshot).FullName, true), (object) SQL.EncodeString(loanId.ToString(), true));
      command.AppendFormat(format);
    }

    private void AppendDeleteFullLoanFromTable(
      StringBuilder command,
      List<string> tables,
      Guid loanId)
    {
      foreach (string table in tables)
      {
        string format = string.Format("DELETE FROM {0} where LoanId = {1};", (object) SQL.EncodeString(table, false), (object) SQL.EncodeString(loanId.ToString(), true));
        command.AppendFormat(format);
      }
    }

    private List<string> UpsertSnapshotParameters(LoanFragment fragment)
    {
      return new List<string>()
      {
        fragment.ToFragmentEntityTypeId().ToString(),
        SQL.EncodeString(fragment.EntityName, true),
        SQL.EncodeString(fragment.LoanId.ToString(), true),
        SQL.EncodeString(fragment.EntityId.ToString(), true),
        SQL.EncodeString(fragment.InstanceId, true),
        fragment.ModelPathIndex.ToString(),
        SQL.EncodeString(fragment.Data, true)
      };
    }

    private List<string> UpsertElliLoanParameters(LoanFragment fragment)
    {
      return new List<string>()
      {
        SQL.EncodeString(fragment.EntityId.ToString(), true),
        fragment.ToFragmentEntityTypeId().ToString(),
        SQL.EncodeString(fragment.EntityName, true),
        SQL.EncodeString(fragment.LoanId.ToString(), true),
        SQL.EncodeString(fragment.InstanceId, true),
        fragment.ModelPathIndex.ToString(),
        SQL.EncodeString(fragment.Data, true)
      };
    }

    private void AddParametersToCommand(string sql, List<string> parameters, StringBuilder command)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string parameter in parameters)
        stringBuilder.Append(parameter + ",");
      --stringBuilder.Length;
      command.AppendFormat(sql, (object) stringBuilder);
    }

    public List<string> GetSubmitLoanGuid(
      UserInfo currentUser,
      int start,
      int limit,
      string sort,
      string filter)
    {
      throw new NotImplementedException();
    }

    private string GenerateSelectForLoan(Guid loanId, FragmentEntityType type = FragmentEntityType.All)
    {
      return type != FragmentEntityType.All ? string.Format("SELECT  EntityId, EntityType, TypeName, LoanId, InstanceId, ModelpathIndex, EntityData FROM ElliLoan WHERE LoanId = {0} AND EntityType = {1};", (object) SQL.EncodeString(loanId.ToString(), true), (object) type) : string.Format("SELECT  EntityId, EntityType, TypeName, LoanId, InstanceId, ModelpathIndex, EntityData FROM ElliLoan WHERE LoanId = {0};", (object) SQL.EncodeString(loanId.ToString(), true));
    }

    public SnapshotObject GetLoanSnapshotObject(
      Guid loanId,
      Guid snapshotGuid,
      LogSnapshotType type)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Get_Loan_Snapshot_Object))
      {
        string instanceId = this._dbContext.InstanceId;
        NpgsqlCommand command = new NpgsqlCommand("SELECT SnapshotId, SnapshotType, TypeName, LoanId, ParentId, InstanceId, SnapshotData FROM ElliSnapshot WHERE InstanceId=@InstanceId AND LoanId=@LoanId AND ParentId = @ParentId AND SnapshotType = @SnapshotType;");
        command.Parameters.AddWithValue("InstanceId", (object) instanceId);
        command.Parameters.AddWithValue("LoanId", (object) loanId);
        command.Parameters.AddWithValue("ParentId", (object) snapshotGuid);
        command.Parameters.AddWithValue("SnapshotType", (object) (int) type);
        try
        {
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          {
            DataTable dataTable = service.ExecuteTableQuery((IDbCommand) command, DbTransactionType.Default);
            if (dataTable == null || dataTable.Rows.Count < 1)
              return (SnapshotObject) null;
            DataRow row = dataTable.Rows[0];
            SnapshotObject loanSnapshotObject = this._serializer.Deserialize<SnapshotObject>((string) row["SnapshotData"], (JsonSerializerSettings) null);
            loanSnapshotObject.Id = (Guid) row["SnapshotId"];
            return loanSnapshotObject;
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
          return (SnapshotObject) null;
        }
      }
    }

    public void SaveLoanSnapshotObject(
      Guid loanId,
      Guid snapshotGuid,
      LogSnapshotType type,
      SnapshotObject data)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Save_Loan_Snapshot_Object))
      {
        string instanceId = this._dbContext.InstanceId;
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO ElliSnapshot(SnapshotType, TypeName, LoanId, ParentId, InstanceId, SnapshotData) VALUES(@SnapshotType, @TypeName, @LoanId, @ParentId, @InstanceId, @SnapshotData) ON CONFLICT ON CONSTRAINT ElliSnapshot_UxSnapshotOnUpdateConstraint DO UPDATE SET RowModified = EXCLUDED.RowModified, SnapshotData=EXCLUDED.SnapshotData; ");
        command.Parameters.AddWithValue("SnapshotType", (object) (int) type);
        command.Parameters.AddWithValue("TypeName", (object) type.GetType().FullName);
        command.Parameters.AddWithValue("LoanId", (object) loanId);
        command.Parameters.AddWithValue("ParentId", (object) snapshotGuid);
        command.Parameters.AddWithValue("InstanceId", (object) instanceId);
        command.Parameters.AddWithValue("SnapshotData", NpgsqlDbType.Jsonb, (object) this._serializer.Serialize<SnapshotObject>(data, (JsonSerializerSettings) null));
        try
        {
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
            service.ExecuteNonQuery((IDbCommand) command, DbTransactionType.Default);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
        }
      }
    }

    public string GetLoanEventLogXml(Guid loanId)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Get_Loan_EventLogXml))
      {
        string instanceId = this._dbContext.InstanceId;
        NpgsqlCommand command = new NpgsqlCommand("SELECT EventLogId, LoanId, InstanceId, EventLogData FROM ElliLoanEventLog WHERE InstanceId=@InstanceId AND LoanId=@LoanId;");
        command.Parameters.AddWithValue("InstanceId", (object) instanceId);
        command.Parameters.AddWithValue("LoanId", (object) loanId);
        try
        {
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          {
            DataTable dataTable = service.ExecuteTableQuery((IDbCommand) command, DbTransactionType.Default);
            return dataTable == null || dataTable.Rows.Count < 1 ? (string) null : (string) dataTable.Rows[0]["EventLogData"];
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
          return (string) null;
        }
      }
    }

    public void SaveLoanEventLogXml(Guid loanId, string xml)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Save_Loan_EventLogXml))
      {
        string instanceId = this._dbContext.InstanceId;
        NpgsqlCommand command = new NpgsqlCommand("INSERT INTO ElliLoanEventLog (LoanId, InstanceId, EventLogData) VALUES (@LoanId, @InstanceId, @EventLogData) ON CONFLICT ON CONSTRAINT ElliLoanEventLog_UxEventLogOnUpdateConstraint DO UPDATE SET EventLogData=EXCLUDED.EventLogData;");
        command.Parameters.AddWithValue("LoanId", (object) loanId);
        command.Parameters.AddWithValue("InstanceId", (object) instanceId);
        command.Parameters.AddWithValue("EventLogData", NpgsqlDbType.Xml, (object) xml);
        try
        {
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
            service.ExecuteNonQuery((IDbCommand) command, DbTransactionType.Default);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
        }
      }
    }

    public FileAttachment[] GetLoanAttachments(
      Guid loanId,
      System.Func<XmlDocument, FileAttachment[]> deserializer)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Get_Loan_Attachments))
      {
        string instanceId = this._dbContext.InstanceId;
        NpgsqlCommand command = new NpgsqlCommand("SELECT AttachmentId, LoanId, InstanceId, FileName, AttachmentData FROM ElliAttachment WHERE InstanceId=@InstanceId AND LoanId=@LoanId;");
        command.Parameters.AddWithValue("InstanceId", (object) instanceId);
        command.Parameters.AddWithValue("LoanId", (object) loanId);
        try
        {
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
          {
            DataTable dataTable = service.ExecuteTableQuery((IDbCommand) command, DbTransactionType.Default);
            if (dataTable == null || dataTable.Rows.Count < 1)
              return (FileAttachment[]) null;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<Attachments>");
            for (int index = 0; index < dataTable.Rows.Count; ++index)
              stringBuilder.AppendLine((string) dataTable.Rows[index]["AttachmentData"]);
            stringBuilder.AppendLine("</Attachments>");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(stringBuilder.ToString());
            return deserializer(xmlDocument);
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
          return (FileAttachment[]) null;
        }
      }
    }

    public void SaveLoanAttachments(Guid loanId, FileAttachment[] attachments)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Save_Loan_Attachments))
      {
        string instanceId = this._dbContext.InstanceId;
        try
        {
          StringBuilder command = new StringBuilder();
          command.AppendLine(string.Format("DELETE FROM ElliAttachment WHERE LoanId={0} AND InstanceId={1};", (object) SQL.EncodeString(loanId.ToString(), true), (object) SQL.EncodeString(instanceId.ToString(), true)));
          foreach (FileAttachment attachment in attachments)
          {
            string name = (string) null;
            switch (attachment)
            {
              case NativeAttachment _:
                name = "File";
                break;
              case ImageAttachment _:
                name = "Image";
                break;
              case CloudAttachment _:
                name = "Cloud";
                break;
              case BackgroundAttachment _:
                name = "Background";
                break;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<Attachments/>");
            XmlElement element = xmlDocument.CreateElement(name);
            xmlDocument.DocumentElement.AppendChild((XmlNode) element);
            attachment.ToXml(element);
            this.AddParametersToCommand("INSERT INTO ElliAttachment (LoanId, InstanceId, FileName, AttachmentData) VALUES ({0});", this.UpsertElliAttachmentParameters(loanId, instanceId, attachment.ID, xmlDocument.FirstChild.InnerXml), command);
          }
          using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
            service.ExecuteNonQuery((IDbCommand) new NpgsqlCommand(command.ToString()), DbTransactionType.Default);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (LoanAccessor), ex);
        }
      }
    }

    private List<string> UpsertElliAttachmentParameters(
      Guid loanId,
      string instanceId,
      string attachmentId,
      string xmlDoc)
    {
      return new List<string>()
      {
        SQL.EncodeString(loanId.ToString(), true),
        SQL.EncodeString(instanceId, true),
        SQL.EncodeString(attachmentId, true),
        SQL.EncodeString(xmlDoc, true)
      };
    }

    public void DeleteLoan(Guid loanId)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Delete_Loan))
      {
        StringBuilder command = new StringBuilder();
        List<string> tables = new List<string>()
        {
          "ElliLoan",
          "ElliSnapshot",
          "ElliAttachment",
          "ElliLoanEventLog"
        };
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
        {
          this.AppendDeleteFullLoanFromTable(command, tables, loanId);
          service.ExecuteNonQuery(command.ToString());
        }
      }
    }

    public void MoveLoanToFolder(Guid encompassGuid, string destinationFolderName, string userId)
    {
      using (this._loanAccessorMR.RecordLoanAccessorTime(LoanAccessorOperationType.Move_Loan_To_Folder))
      {
        StringBuilder stringBuilder = new StringBuilder();
        using (IDbAccessManager service = DataAccessFramework.Runtime.CreateService<IDbAccessManager>(this._dbContext))
        {
          string str = "UPDATE ElliLoan p SET EntityData = jsonb_set(jsonb_set(p.EntityData, folderNameArrayPath, folderName, false), '{LoanFolder}', folderName) FROM ElliLoan p1, LATERAL ( SELECT ARRAY['LoanTags', (ord - 1)::text, 'TagName'] AS folderNameArrayPath , to_jsonb(" + SQL.EncodeString(destinationFolderName, true) + " :: text) AS folderName FROM jsonb_array_elements(p1.EntityData #> '{LoanTags}') WITH ORDINALITY arr(opt, ord) WHERE  opt->> 'TagType' = 'LoanFolder') opt WHERE p1.LoanId = p.LoanId AND p.EntityType = 1 and p.LoanId = " + SQL.EncodeString(encompassGuid.ToString(), true);
          stringBuilder.AppendLine(str);
          service.ExecuteNonQuery(stringBuilder.ToString());
        }
      }
    }

    public void Dispose()
    {
    }
  }
}
