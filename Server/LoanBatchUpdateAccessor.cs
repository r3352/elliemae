// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanBatchUpdateAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class LoanBatchUpdateAccessor
  {
    private const string className = "LoanBatchUpdateAccessor�";
    private const string loanEventType = "LoanBatchUpdate�";
    private static readonly int _kafkaLoanCount;

    static LoanBatchUpdateAccessor()
    {
      int result;
      if (int.TryParse(ConfigurationManager.AppSettings["LoanBatchLoanCount"], out result))
        LoanBatchUpdateAccessor._kafkaLoanCount = result;
      else
        LoanBatchUpdateAccessor._kafkaLoanCount = 1000;
    }

    public static void SubmitBatch(
      LoanBatch loanBatch,
      UserInfo currentUser,
      bool isExternalOrganization,
      double lockTimeout = 60.0)
    {
      LoanXDBTableList loanXdbTableList = LoanXDBStore.GetLoanXDBTableList();
      DbTableInfo table1 = DbAccessManager.GetTable("LoanUpdates");
      DbTableInfo table2 = DbAccessManager.GetTable("AuditTrail");
      DbTableInfo table3 = DbAccessManager.GetTable("LoanSummary");
      string[] loanBatchGuidList = LoanBatchUpdateAccessor.getLoanBatchGuidList(loanBatch, isExternalOrganization);
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      FieldSettings fieldSettings = LoanConfiguration.GetFieldSettings();
      List<string> stringList = new List<string>();
      string[] source = new string[1]{ "guid" };
      int num1 = 1;
      List<List<LoanBatchField>> loanBatchFieldListList = new List<List<LoanBatchField>>();
      int num2 = Utils.ParseInt(ClientContext.GetCurrent().Settings.GetServerSetting("POLICIES.LOANBATCHFIELDSLIMIT", false));
      bool flag = loanBatch.Fields.Count > num2;
      List<LoanBatchField> loanBatchFieldList1 = new List<LoanBatchField>();
      foreach (LoanBatchField field1 in loanBatch.Fields)
      {
        if (((IEnumerable<string>) source).Contains<string>(field1.FieldID.ToString().ToLower()))
        {
          TraceLog.WriteWarning(nameof (LoanBatchUpdateAccessor), "Field '" + field1.FieldID + "' is readonly.");
        }
        else
        {
          FieldDefinition field2 = EncompassFields.GetField(field1.FieldID, fieldSettings, true);
          if (field2 == null)
          {
            stringList.Add("Field '" + field1.FieldID + "' is not defined.");
          }
          else
          {
            if (flag)
            {
              loanBatchFieldList1.Add(field1);
              if (num1 == num2)
              {
                loanBatchFieldListList.Add(loanBatchFieldList1);
                loanBatchFieldList1 = new List<LoanBatchField>();
                num1 = 1;
              }
              else
                ++num1;
            }
            try
            {
              dictionary.Add(field1.FieldID, field2.ToNativeValue(string.Concat(field1.FieldValue), true));
            }
            catch
            {
              if (string.Concat(field1.FieldValue) == "")
                dictionary.Add(field1.FieldID, (object) null);
              else
                stringList.Add("Value '" + field1.FieldValue + "' is invalid for field '" + field1.FieldID + "'");
            }
          }
        }
      }
      if (!flag)
        loanBatchFieldListList.Add(loanBatch.Fields);
      else if (num1 > 1)
        loanBatchFieldListList.Add(loanBatchFieldList1);
      if (stringList.Count > 0)
        throw new Exception("One or more field values was invalid and the batch was not updated." + Environment.NewLine + string.Join(Environment.NewLine, stringList.ToArray()));
      string batchId = Guid.NewGuid().ToString();
      using (PerformanceMeter.Current.BeginOperation("Loan Batch Update"))
      {
        string str1 = string.Empty;
        foreach (string str2 in loanBatchGuidList)
        {
          foreach (List<LoanBatchField> loanBatchFieldList2 in loanBatchFieldListList)
          {
            DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
            dbQueryBuilder.Declare("@loanXRef", "int");
            dbQueryBuilder.Declare("@priorData", "sql_variant");
            foreach (LoanBatchField loanBatchField in loanBatchFieldList2)
            {
              if (flag)
                ++num1;
              if (!((IEnumerable<string>) source).Contains<string>(loanBatchField.FieldID.ToString().ToLower()))
              {
                str1 = str1 + loanBatchField.FieldID + " : " + dictionary[loanBatchField.FieldID] + "|";
                dbQueryBuilder.AppendLine("select @loanXRef = XRefID from LoanXRef where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) str2));
                dbQueryBuilder.If("@loanXRef is not null");
                dbQueryBuilder.Begin();
                dbQueryBuilder.InsertInto(table1, new DbValueList()
                {
                  {
                    "LoanXRef",
                    (object) "@loanXRef",
                    (IDbEncoder) DbEncoding.None
                  },
                  {
                    "FieldID",
                    (object) loanBatchField.FieldID
                  },
                  {
                    "Data",
                    dictionary[loanBatchField.FieldID]
                  },
                  {
                    "UserID",
                    (object) currentUser.Userid
                  },
                  {
                    "BatchID",
                    (object) batchId
                  }
                }, true, false);
                DbValue key1 = new DbValue("Guid", (object) str2);
                DbValueList values = new DbValueList();
                values.Add("LastModified", (object) DateTime.Now);
                if (loanBatchField.FieldID.Equals("TPO.X15"))
                  values.Add("TPOCompanyID", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X39"))
                  values.Add("TPOBranchID", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X62"))
                  values.Add("TPOLOID", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X75"))
                  values.Add("TPOLPID", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X1"))
                  values.Add("TPOSiteID", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X8"))
                  values.Add("TPOArchived", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X61"))
                  values.Add("TPOLOName", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X74"))
                  values.Add("TPOLPName", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X14"))
                  values.Add("TPOCompanyName", dictionary[loanBatchField.FieldID]);
                if (loanBatchField.FieldID.Equals("TPO.X38"))
                  values.Add("TPOBranchName", dictionary[loanBatchField.FieldID]);
                dbQueryBuilder.Update(table3, values, key1);
                LoanXDBField field = loanXdbTableList.GetField(loanBatchField.FieldID);
                if (field != null)
                {
                  if (field.Auditable)
                  {
                    dbQueryBuilder.AppendLine("select @priorData = " + field.ColumnName + " from " + field.TableName + " where XRefID = @loanXRef");
                    dbQueryBuilder.AppendLine("update AuditTrail set IsCurrent = 0 where LoanXRef = @loanXRef and FieldXRef = " + (object) field.FieldXRefID + " and IsCurrent = 1");
                    dbQueryBuilder.InsertInto(table2, new DbValueList()
                    {
                      {
                        "LoanXRef",
                        (object) "@loanXRef",
                        (IDbEncoder) DbEncoding.None
                      },
                      {
                        "FieldXRef",
                        (object) field.FieldXRefID
                      },
                      {
                        "UserID",
                        (object) currentUser.Userid
                      },
                      {
                        "Data",
                        dictionary[loanBatchField.FieldID]
                      },
                      {
                        "PreviousData",
                        (object) "@priorData",
                        (IDbEncoder) DbEncoding.None
                      },
                      {
                        "ModifiedDTTM",
                        (object) DateTime.Now
                      },
                      {
                        "IsCurrent",
                        (object) 1
                      }
                    }, true, false);
                  }
                  DbTableInfo dynamicTable = DbAccessManager.GetDynamicTable(field.TableName);
                  DbValue key2 = new DbValue("XRefId", (object) "@loanXRef", (IDbEncoder) DbEncoding.None);
                  dbQueryBuilder.Upsert(dynamicTable, new DbValueList()
                  {
                    {
                      field.ColumnName,
                      dictionary[loanBatchField.FieldID]
                    }
                  }, key2);
                }
                dbQueryBuilder.End();
              }
            }
            if (!string.IsNullOrEmpty(str1) && str1.Length > 1)
              str1 = str1.Remove(str1.Length - 1);
            try
            {
              dbQueryBuilder.ExecuteNonQuery();
              TraceLog.WriteInfo(nameof (LoanBatchUpdateAccessor), "Batch loan update record written: Loan = '" + str2 + "', Field With Value = '" + (string.IsNullOrEmpty(str1) ? "No Fields available to update" : str1) + "'");
            }
            catch (Exception ex)
            {
              throw new ServerDataException("Error saving loan batch data for loan '" + str2 + "', field  With Value =  '" + (string.IsNullOrEmpty(str1) ? "No Fields available to update" : str1) + "'", ex);
            }
          }
        }
      }
      LoanBatchUpdateAccessor.SubmitLoanBatchRequestToKafka(batchId, ((IEnumerable<string>) loanBatchGuidList).ToList<string>(), currentUser.Userid, false);
    }

    private static bool SubmitLoanBatchRequestToKafka(
      string batchId,
      List<string> loanIds,
      string userId,
      bool isSourceEncompass)
    {
      ClientContext current = ClientContext.GetCurrent();
      DateTime now = DateTime.Now;
      IEnumerable<IEnumerable<string>> kafkaBatches = LoanBatchUpdateAccessor.GetKafkaBatches<string>((IEnumerable<string>) loanIds, LoanBatchUpdateAccessor._kafkaLoanCount);
      TraceLog.WriteInfo(nameof (LoanBatchUpdateAccessor), string.Format("Batch Loan Update: Submitting {0} loans for batch", (object) loanIds.Count));
      bool kafka = true;
      foreach (IEnumerable<string> source in kafkaBatches)
      {
        try
        {
          LoanBatchUpdateEvent queueEvent = new LoanBatchUpdateEvent(current.InstanceName, "siteId", "LoanBatchUpdate", userId, isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_SDK : Enums.Source.URN_ELLI_SERVICE_EBS, now);
          queueEvent.AddKafkaMessage(Guid.NewGuid().ToString(), batchId, (IEnumerable<string>) source.ToList<string>());
          if (queueEvent.QueueMessages.Count > 0)
          {
            IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
            IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
            queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          }
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (LoanBatchUpdateAccessor), string.Format("Exception publishing loanEvent to kafka for BatchId - {0}. Exception details {1}", (object) batchId, (object) ex.StackTrace));
          kafka = false;
        }
      }
      return kafka;
    }

    public static IEnumerable<IEnumerable<T>> GetKafkaBatches<T>(IEnumerable<T> list, int size)
    {
      if (size < 1)
        throw new ArgumentException();
      for (IEnumerable<T> rest = list; rest.Any<T>(); rest = rest.Skip<T>(size))
        yield return rest.Take<T>(size);
    }

    public static long ApplyBatchUpdatesToLoan(LoanData loan)
    {
      long sequenceNumber = loan.GetBatchUpdateSequenceNum();
      long num = sequenceNumber;
      loan.BatchAppliedSinceLastSave = false;
      ExternalUserInfo externalUserInfo = new ExternalUserInfo();
      bool flag1 = false;
      bool flag2 = false;
      Dictionary<string, string> tpoLoanOfficerFields = new Dictionary<string, string>()
      {
        {
          "TPO.X61",
          ""
        },
        {
          "TPO.X62",
          ""
        },
        {
          "TPO.X63",
          ""
        },
        {
          "TPO.X65",
          ""
        },
        {
          "TPO.X66",
          ""
        },
        {
          "TPO.X67",
          ""
        },
        {
          "TPO.X74",
          ""
        },
        {
          "TPO.X75",
          ""
        },
        {
          "TPO.X76",
          ""
        },
        {
          "TPO.X78",
          ""
        },
        {
          "TPO.X79",
          ""
        },
        {
          "TPO.X80",
          ""
        }
      };
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select lu.FieldID, lu.Data, coalesce(lu.SequenceNumber, lu.recordID) SequenceNumber ");
      dbQueryBuilder.AppendLine("from LoanUpdates lu inner join LoanXRef xref on lu.LoanXRef = xref.XRefID");
      dbQueryBuilder.AppendLine("where xref.LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loan.GUID));
      dbQueryBuilder.AppendLine("   and coalesce(lu.SequenceNumber, lu.recordID) > " + (object) sequenceNumber);
      dbQueryBuilder.AppendLine("order by coalesce(lu.SequenceNumber, lu.recordID)");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable.Rows.Count == 0)
        return sequenceNumber;
      if (loan.Validator != null)
        throw new Exception("The specified LoanData object has an attached validator and cannot be used.");
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str = EllieMae.EMLite.DataAccess.SQL.DecodeString(row["FieldID"]);
        object obj = EllieMae.EMLite.DataAccess.SQL.Decode(row["Data"]);
        if (tpoLoanOfficerFields.ContainsKey(str))
        {
          tpoLoanOfficerFields[str] = obj.ToString();
          if (str == "TPO.X62")
            flag1 = true;
          if (str == "TPO.X75")
            flag2 = true;
        }
        try
        {
          loan.SetField(str, string.Concat(obj));
          TraceLog.WriteVerbose(nameof (LoanBatchUpdateAccessor), "Wrote batch update data value to loan. Loan = '" + loan.GUID + "', Field = '" + str + "', Value = '" + obj + "'");
        }
        catch
        {
          TraceLog.WriteWarning(nameof (LoanBatchUpdateAccessor), "Failed to batch update data value to loan. Loan = '" + loan.GUID + "', Field = '" + str + "', Value = '" + obj + "'");
        }
      }
      if (dataTable.Rows.Count > 0)
        sequenceNumber = EllieMae.EMLite.DataAccess.SQL.DecodeLong(dataTable.Rows[dataTable.Rows.Count - 1]["SequenceNumber"]);
      if (flag1)
        LoanBatchUpdateAccessor.UpdateMilestoneLog(loan, tpoLoanOfficerFields, RealWorldRoleID.TPOLoanOfficer);
      if (flag2)
        LoanBatchUpdateAccessor.UpdateMilestoneLog(loan, tpoLoanOfficerFields, RealWorldRoleID.TPOLoanProcessor);
      loan.SetBatchUpdateSequenceNum(sequenceNumber);
      loan.ReplaceCachedXML();
      loan.ClearDirtyTable();
      loan.BatchAppliedSinceLastSave = sequenceNumber != num;
      return sequenceNumber;
    }

    private static void UpdateMilestoneLog(
      LoanData loan,
      Dictionary<string, string> tpoLoanOfficerFields,
      RealWorldRoleID realWorldRoleID)
    {
      LogList logList = loan.GetLogList();
      RolesMappingInfo roleMappingInfo = WorkflowBpmDbAccessor.GetRoleMappingInfo(realWorldRoleID);
      if (roleMappingInfo == null)
        return;
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      for (int index = 0; index < allMilestones.Length; ++index)
      {
        if (allMilestones[index].RoleID == roleMappingInfo.RoleIDList[0] && index > 0)
        {
          switch (realWorldRoleID)
          {
            case RealWorldRoleID.TPOLoanOfficer:
              allMilestones[index].SetLoanAssociate(tpoLoanOfficerFields["TPO.X62"], tpoLoanOfficerFields["TPO.X61"], tpoLoanOfficerFields["TPO.X63"], tpoLoanOfficerFields["TPO.X65"], tpoLoanOfficerFields["TPO.X67"], tpoLoanOfficerFields["TPO.X66"], "");
              goto label_8;
            case RealWorldRoleID.TPOLoanProcessor:
              allMilestones[index].SetLoanAssociate(tpoLoanOfficerFields["TPO.X75"], tpoLoanOfficerFields["TPO.X74"], tpoLoanOfficerFields["TPO.X76"], tpoLoanOfficerFields["TPO.X78"], tpoLoanOfficerFields["TPO.X80"], tpoLoanOfficerFields["TPO.X79"], "");
              goto label_8;
            default:
              goto label_8;
          }
        }
      }
label_8:
      foreach (MilestoneFreeRoleLog milestoneFreeRole in logList.GetAllMilestoneFreeRoles())
      {
        if (milestoneFreeRole.RoleID == roleMappingInfo.RoleIDList[0])
        {
          if (realWorldRoleID == RealWorldRoleID.TPOLoanOfficer)
          {
            milestoneFreeRole.SetLoanAssociate(tpoLoanOfficerFields["TPO.X62"], tpoLoanOfficerFields["TPO.X61"], tpoLoanOfficerFields["TPO.X63"], tpoLoanOfficerFields["TPO.X65"], tpoLoanOfficerFields["TPO.X67"], tpoLoanOfficerFields["TPO.X66"], "");
            break;
          }
          if (realWorldRoleID != RealWorldRoleID.TPOLoanProcessor)
            break;
          milestoneFreeRole.SetLoanAssociate(tpoLoanOfficerFields["TPO.X75"], tpoLoanOfficerFields["TPO.X74"], tpoLoanOfficerFields["TPO.X76"], tpoLoanOfficerFields["TPO.X78"], tpoLoanOfficerFields["TPO.X80"], tpoLoanOfficerFields["TPO.X79"], "");
          break;
        }
      }
    }

    public static void DeleteAppliedUpdatesForLoan(LoanData loan)
    {
      long updateSequenceNum = loan.GetBatchUpdateSequenceNum();
      if (updateSequenceNum < 0L)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("if not exists (select 1 from LoanLock where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loan.GUID) + " and lockedfor <> 0)");
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("   delete from LoanUpdates where LoanXRef = (select XRefID from LoanXRef where LoanGuid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loan.GUID) + ") and coalesce(SequenceNumber, recordID) <= " + (object) updateSequenceNum);
      dbQueryBuilder.AppendLine("   select 1 as RecordsDeleted");
      dbQueryBuilder.End();
      dbQueryBuilder.Else();
      dbQueryBuilder.AppendLine("   select 0 as RecordsDeleted");
      if (EllieMae.EMLite.DataAccess.SQL.DecodeInt(dbQueryBuilder.ExecuteScalar()) <= 0)
        return;
      loan.SetBatchUpdateSequenceNum(-1L);
      loan.ReplaceCachedXML();
    }

    private static string[] getLoanBatchGuidList(LoanBatch batch, bool isExternalOrganization)
    {
      switch (batch)
      {
        case LoanSetBatch _:
          return ((LoanSetBatch) batch).LoanGuids.ToArray();
        case LoanQueryBatch _:
          return LoanBatchUpdateAccessor.queryLoanGuids(((LoanQueryBatch) batch).Criteria, isExternalOrganization);
        default:
          return new string[0];
      }
    }

    private static string[] queryLoanGuids(QueryCriterion cri, bool isExternalOrganization)
    {
      DataQuery query = new DataQuery();
      query.Selections.AddField("Loan.Guid");
      query.Filter = cri;
      QueryResult queryResult = new LoanQuery((UserInfo) null).Execute(query, isExternalOrganization);
      List<string> stringList = new List<string>();
      for (int row = 0; row < queryResult.RecordCount; ++row)
        stringList.Add(string.Concat(queryResult[row, 0]));
      return stringList.ToArray();
    }

    public static void MigrateTpoClassicLoansToTpoConnect(LoanData loanData)
    {
      if (loanData == null || string.IsNullOrEmpty(loanData.GetField("TPO.X1")))
        return;
      string field = loanData.GetField("TPO_MIGRATION");
      if (!string.IsNullOrEmpty(field) && field == "Y")
        return;
      object obj = loanData.Clone();
      ClientContext current = ClientContext.GetCurrent();
      try
      {
        string companySetting = Company.GetCompanySetting((IClientContext) current, "MIGRATION", "TPOMIGRATIONSTATUS");
        if (string.IsNullOrEmpty(companySetting) || Convert.ToInt16(companySetting) != (short) 2)
          return;
        string tpoConnectSiteId = LoanBatchUpdateAccessor.GetTpoConnectSiteId(loanData.GUID);
        if (tpoConnectSiteId != null)
          loanData.SetField("TPO.X1", tpoConnectSiteId);
        IEnumerable<RolesMappingInfo> source1 = ((IEnumerable<RolesMappingInfo>) WorkflowBpmDbAccessor.GetAllRoleMappingInfos()).Where<RolesMappingInfo>((System.Func<RolesMappingInfo, bool>) (x => x.RealWorldRoleID == RealWorldRoleID.TPOLoanOfficer || x.RealWorldRoleID == RealWorldRoleID.TPOLoanProcessor || x.RealWorldRoleID == RealWorldRoleID.PrimarySalesRep));
        LogList logList = loanData.GetLogList();
        if (!(source1 is IList<RolesMappingInfo> rolesMappingInfoList))
          rolesMappingInfoList = (IList<RolesMappingInfo>) source1.ToList<RolesMappingInfo>();
        foreach (RolesMappingInfo rolesMappingInfo in (IEnumerable<RolesMappingInfo>) rolesMappingInfoList)
        {
          UserInfo userInfo = (UserInfo) null;
          RolesMappingInfo role = rolesMappingInfo;
          bool flag = false;
          switch (role.RealWorldRoleID)
          {
            case RealWorldRoleID.PrimarySalesRep:
              UserInfo userById = User.GetUserById(loanData.GetField("TPO.X62"));
              if ((object) userById == null)
                userById = User.GetUserById(loanData.GetField("TPO.X75"));
              userInfo = userById;
              if (!(userInfo == (UserInfo) null))
              {
                ExternalUserInfo userInfoByContactId = ExternalOrgManagementAccessor.GetExternalUserInfoByContactId(userInfo.Userid);
                userInfo = !((UserInfo) userInfoByContactId == (UserInfo) null) ? User.GetUserById(userInfoByContactId.SalesRepID) : (UserInfo) null;
                break;
              }
              break;
            case RealWorldRoleID.TPOLoanOfficer:
              userInfo = User.GetUserById(loanData.GetField("TPO.X62"));
              break;
            case RealWorldRoleID.TPOLoanProcessor:
              userInfo = User.GetUserById(loanData.GetField("TPO.X75"));
              break;
          }
          foreach (DocumentLog allDocument in loanData.GetLogList().GetAllDocuments(false))
          {
            try
            {
              if (allDocument.IsTPOWebcenterPortal)
                LoanBatchUpdateAccessor.AddRolesToDocument(allDocument, rolesMappingInfo);
              else
                LoanBatchUpdateAccessor.RemoveRolesFromDocument(allDocument, rolesMappingInfo);
            }
            catch (Exception ex)
            {
              TraceLog.WriteError(nameof (LoanBatchUpdateAccessor), string.Format("Failed to update role mapping to TPO document or Update loan history monitor. Loan GUID={0}.   Stack={1}", (object) loanData.GUID, (object) ex.StackTrace));
            }
          }
          if (!(userInfo == (UserInfo) null))
          {
            foreach (MilestoneLog milestoneLog in ((IEnumerable<MilestoneLog>) logList.GetAllMilestones()).Where<MilestoneLog>((System.Func<MilestoneLog, bool>) (x => x.RoleID == role.RoleIDList[0])))
            {
              milestoneLog.RoleID = role.RoleIDList[0];
              milestoneLog.SetLoanAssociate(userInfo.Userid, userInfo.FullName, userInfo.Email, userInfo.Phone, userInfo.CellPhone, userInfo.Fax, userInfo.JobTitle);
              flag = true;
            }
            if (!flag)
            {
              IEnumerable<MilestoneFreeRoleLog> source2 = ((IEnumerable<MilestoneFreeRoleLog>) logList.GetAllMilestoneFreeRoles()).Where<MilestoneFreeRoleLog>((System.Func<MilestoneFreeRoleLog, bool>) (x => x.RoleID == role.RoleIDList[0]));
              if (!(source2 is IList<MilestoneFreeRoleLog> milestoneFreeRoleLogList))
                milestoneFreeRoleLogList = (IList<MilestoneFreeRoleLog>) source2.ToList<MilestoneFreeRoleLog>();
              IList<MilestoneFreeRoleLog> source3 = milestoneFreeRoleLogList;
              if (!source3.Any<MilestoneFreeRoleLog>())
              {
                MilestoneFreeRoleLog milestoneFreeRoleLog = new MilestoneFreeRoleLog();
                milestoneFreeRoleLog.RoleID = role.RoleIDList[0];
                MilestoneFreeRoleLog rec = milestoneFreeRoleLog;
                rec.SetLoanAssociate(userInfo.Userid, userInfo.FullName, userInfo.Email, userInfo.Phone, userInfo.CellPhone, userInfo.Fax, userInfo.JobTitle);
                loanData.GetLogList().AddRecord((LogRecordBase) rec);
              }
              else
              {
                foreach (MilestoneFreeRoleLog milestoneFreeRoleLog in (IEnumerable<MilestoneFreeRoleLog>) source3)
                {
                  milestoneFreeRoleLog.RoleID = role.RoleIDList[0];
                  milestoneFreeRoleLog.SetLoanAssociate(userInfo.Userid, userInfo.FullName, userInfo.Email, userInfo.Phone, userInfo.CellPhone, userInfo.Fax, userInfo.FullName);
                }
              }
            }
          }
        }
        loanData.SetField("TPO_MIGRATION", "Y");
      }
      catch (Exception ex)
      {
        loanData = (LoanData) obj;
        TraceLog.WriteError(nameof (LoanBatchUpdateAccessor), string.Format("Failed to migrate TPO classic loan to TPOConnect loan.  Loan GUID={0}.   Stack={1}", (object) loanData.GUID, (object) ex));
      }
    }

    private static string GetTpoConnectSiteId(string guid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select TPOSiteID from loansummary where Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
      DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
      return dataRow != null ? (string) dataRow["TPOSiteID"] : (string) null;
    }

    private static void AddRolesToDocument(
      DocumentLog documentLog,
      RolesMappingInfo rolesMappingInfo)
    {
      if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.TPOLoanProcessor && !documentLog.IsAccessibleToRole(rolesMappingInfo.RoleIDList[0]))
        documentLog.GrantAccess(rolesMappingInfo.RoleIDList[0]);
      if (rolesMappingInfo.RealWorldRoleID != RealWorldRoleID.TPOLoanOfficer || documentLog.IsAccessibleToRole(rolesMappingInfo.RoleIDList[0]))
        return;
      documentLog.GrantAccess(rolesMappingInfo.RoleIDList[0]);
    }

    private static void RemoveRolesFromDocument(
      DocumentLog documentLog,
      RolesMappingInfo rolesMappingInfo)
    {
      if (rolesMappingInfo.RealWorldRoleID == RealWorldRoleID.TPOLoanProcessor && documentLog.IsAccessibleToRole(rolesMappingInfo.RoleIDList[0]))
        documentLog.RemoveAccess(rolesMappingInfo.RoleIDList[0]);
      if (rolesMappingInfo.RealWorldRoleID != RealWorldRoleID.TPOLoanOfficer || !documentLog.IsAccessibleToRole(rolesMappingInfo.RoleIDList[0]))
        return;
      documentLog.RemoveAccess(rolesMappingInfo.RoleIDList[0]);
    }
  }
}
