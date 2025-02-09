// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CalendarSO
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class CalendarSO
  {
    private const string className = "CalendarSO�";
    private string userId;

    public CalendarSO(string userId) => this.userId = userId;

    public string UserID => this.userId;

    public AppointmentInfo GetAppointment(string dataKey)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("Appointments"), new DbValue("DataKey", (object) dataKey));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (AppointmentInfo) null : this.rowToAppointmentInfo(dataRowCollection[0]);
    }

    public AppointmentInfo[] GetAppointmentAndVariances(string dataKey)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("Appointments");
      dbQueryBuilder.SelectFrom(table, new DbValue("DataKey", (object) dataKey));
      dbQueryBuilder.SelectFrom(table, new DbValue("OwnerKey", (object) dataKey));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet.Tables[0].Rows.Count == 0)
        return new AppointmentInfo[0];
      ArrayList arrayList = new ArrayList();
      for (int index1 = 0; index1 < dataSet.Tables.Count; ++index1)
      {
        for (int index2 = 0; index2 < dataSet.Tables[index1].Rows.Count; ++index2)
          arrayList.Add((object) this.rowToAppointmentInfo(dataSet.Tables[index1].Rows[index2]));
      }
      return (AppointmentInfo[]) arrayList.ToArray(typeof (AppointmentInfo));
    }

    public AppointmentInfo[] GetAllAppointmentsForUser(string userID)
    {
      return this.QueryAppointments(userID, (QueryCriterion) null);
    }

    public AppointmentInfo[] GetAppointmentsForUser(
      string UserID,
      DateTime startTime,
      DateTime endTime)
    {
      return this.GetAppointmentsForUser(UserID, startTime, endTime, (QueryCriterion) null);
    }

    public AppointmentInfo[] GetAppointmentsForUser(
      string UserID,
      DateTime startTime,
      DateTime endTime,
      QueryCriterion filter)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (endTime < startTime)
        Err.Raise(TraceLevel.Warning, nameof (CalendarSO), (ServerException) new ServerArgumentException("End time must be on or after start time"));
      if (UserID == "")
        dbQueryBuilder.AppendLine("select * from Appointments Appointment where UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId));
      else
        dbQueryBuilder.AppendLine("select * from Appointments Appointment where UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) UserID));
      if (filter != null)
        dbQueryBuilder.AppendLine("and (" + filter.ToSQLClause() + ")");
      if (startTime < EllieMae.EMLite.DataAccess.SQL.MinSmallDatetime)
        startTime = EllieMae.EMLite.DataAccess.SQL.MinSmallDatetime;
      else if (startTime > EllieMae.EMLite.DataAccess.SQL.MaxSmallDatetime)
        startTime = EllieMae.EMLite.DataAccess.SQL.MaxSmallDatetime;
      if (endTime < EllieMae.EMLite.DataAccess.SQL.MinSmallDatetime)
        endTime = EllieMae.EMLite.DataAccess.SQL.MinSmallDatetime;
      else if (endTime > EllieMae.EMLite.DataAccess.SQL.MaxSmallDatetime)
        endTime = EllieMae.EMLite.DataAccess.SQL.MaxSmallDatetime;
      dbQueryBuilder.AppendLine("and (((AllDayEvent = 0)");
      dbQueryBuilder.AppendLine("      and ((StartDateTime = " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(startTime, true) + ")");
      dbQueryBuilder.AppendLine("           or ((StartDateTime < " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(endTime, true) + ")");
      dbQueryBuilder.AppendLine("               and (EndDateTime > " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(startTime, true) + "))))");
      dbQueryBuilder.AppendLine("  or ((AllDayEvent = 1)");
      dbQueryBuilder.AppendLine("      and (StartDateTime < " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(endTime.TimeOfDay == TimeSpan.Zero ? endTime.Date : endTime.Date.AddDays(1.0), true) + ")");
      dbQueryBuilder.AppendLine("      and (EndDateTime >= " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(startTime.Date, true) + ")))");
      dbQueryBuilder.AppendLine("order by StartDateTime");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      AppointmentInfo[] appointmentsForUser = new AppointmentInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        appointmentsForUser[index] = this.rowToAppointmentInfo(dataRowCollection[index]);
      return appointmentsForUser;
    }

    public AppointmentInfo[] GetAllAppointmentsForContact(ContactInfo cInfo, string UserID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select Appointments.* from Appointments, AppointmentsXref  where   AppointmentsXref.ContactID = " + cInfo.ContactID + " and  AppointmentsXref.ContactType = " + (object) (int) cInfo.ContactType + " and  Appointments.DataKey = AppointmentsXref.DataKey and StartDatetime is not null and EndDatetime is not null");
      if (UserID == "")
        dbQueryBuilder.Append(" and UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId));
      else
        dbQueryBuilder.Append(" and UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) UserID));
      dbQueryBuilder.Append(" order by StartDateTime");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      AppointmentInfo[] appointmentsForContact = new AppointmentInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        appointmentsForContact[index] = this.rowToAppointmentInfo(dataRowCollection[index]);
      return appointmentsForContact;
    }

    public AppointmentInfo[] GetAllAppointmentsForContact(
      ContactInfo cInfo,
      CSListInfo csList,
      string userID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select Appointments.* from Appointments, AppointmentsXref  where   AppointmentsXref.ContactID = " + cInfo.ContactID + " and  AppointmentsXref.ContactType = " + (object) (int) cInfo.ContactType + " and  Appointments.DataKey = AppointmentsXref.DataKey and StartDatetime is not null and EndDatetime is not null");
      dbQueryBuilder.Append(" order by StartDateTime");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      AppointmentInfo[] appointmentsForContact = new AppointmentInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        appointmentsForContact[index] = this.rowToAppointmentInfo(dataRowCollection[index]);
      foreach (AppointmentInfo appointmentInfo in appointmentsForContact)
        appointmentInfo.AccessLevel = !(appointmentInfo.UserID == userID) ? (!csList.ContainsCSContact(appointmentInfo.UserID + "_" + userID) ? CSMessage.AccessLevel.ReadOnly : csList.GetCSContact(appointmentInfo.UserID + "_" + userID).AccessLevel) : CSMessage.AccessLevel.Full;
      return appointmentsForContact;
    }

    public int AddNewBlankRecordForAppointment(string userID)
    {
      DbValueList values = new DbValueList();
      if ((userID ?? "") == "")
        values.Add("UserID", (object) this.userId);
      else
        values.Add("UserID", (object) userID);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.InsertInto(DbAccessManager.GetTable("Appointments"), values, true, false);
      dbQueryBuilder.SelectIdentity();
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public void UpdateAppointment(AppointmentInfo apptInfo, ContactInfo[] contacts)
    {
      DbValueList databaseValueList = this.getDatabaseValueList(apptInfo);
      DbValue key1 = new DbValue("ID", (object) int.Parse(apptInfo.DataKey));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("Appointments");
      dbQueryBuilder.Update(table1, databaseValueList, key1);
      if ((apptInfo.OwnerKey ?? "") != "" && apptInfo.IsRemoved)
      {
        DbValue key2 = new DbValue("ID", (object) int.Parse(apptInfo.OwnerKey));
        dbQueryBuilder.Update(table1, new DbValueList()
        {
          {
            "LastModified",
            (object) "GetDate()",
            (IDbEncoder) DbEncoding.None
          }
        }, key2);
      }
      if (contacts != null)
      {
        DbTableInfo table2 = DbAccessManager.GetTable("AppointmentsXref");
        dbQueryBuilder.DeleteFrom(table2, new DbValue("DataKey", (object) apptInfo.DataKey));
        foreach (ContactInfo contact in contacts)
          dbQueryBuilder.InsertInto(table2, new DbValueList()
          {
            {
              "DataKey",
              (object) apptInfo.DataKey
            },
            {
              "ContactID",
              (object) contact.ContactID
            },
            {
              "ContactType",
              (object) (int) contact.ContactType
            }
          }, true, false);
      }
      SqlCommand sqlCmd = new SqlCommand(dbQueryBuilder.ToString());
      if (apptInfo.Recurrence != null)
        sqlCmd.Parameters.Add("@Recurrence", SqlDbType.Image, apptInfo.Recurrence.Length).Value = (object) apptInfo.Recurrence;
      if (apptInfo.AllProperties != null)
        sqlCmd.Parameters.Add("@AllProperties", SqlDbType.Image, apptInfo.AllProperties.Length).Value = (object) apptInfo.AllProperties;
      using (DbAccessManager dbAccessManager = new DbAccessManager())
        dbAccessManager.ExecuteNonQuery((IDbCommand) sqlCmd);
    }

    public void UpdateAppointmentAndDeletedOccurrences(
      AppointmentInfo rootAppt,
      AppointmentInfo[] deletedInfo)
    {
      this.UpdateAppointment(rootAppt, (ContactInfo[]) null);
      DbValueList keys = new DbValueList();
      keys.Add("OwnerKey", (object) rootAppt.DataKey);
      keys.Add("IsRemoved", (object) 1);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("Appointments"), keys);
      dbQueryBuilder.ExecuteNonQuery();
      if (deletedInfo == null)
        return;
      for (int index = 0; index < deletedInfo.Length; ++index)
      {
        if ((deletedInfo[index].DataKey ?? "") == "")
        {
          int num = this.AddNewBlankRecordForAppointment(rootAppt.UserID);
          deletedInfo[index].DataKey = num.ToString();
        }
        this.UpdateAppointment(deletedInfo[index], (ContactInfo[]) null);
      }
    }

    public string DeleteAppointment(string DataKey)
    {
      DbValue key1 = new DbValue(nameof (DataKey), (object) DataKey);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("Appointments"), new string[1]
      {
        "UserID"
      }, key1);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("AppointmentsXref"), key1);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("Appointments"), key1);
      DbValue key2 = new DbValue("OwnerKey", (object) DataKey);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("Appointments"), key2);
      return (string) EllieMae.EMLite.DataAccess.SQL.Decode(dbQueryBuilder.ExecuteScalar(), (object) null);
    }

    [PgReady]
    public AppointmentInfo[] QueryAppointments(string userId, QueryCriterion cri)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        if ((userId ?? "") == "")
          userId = this.userId;
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from Appointments Appointment");
        pgDbQueryBuilder.AppendLine("where UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " and (StartDatetime is not null) and (EndDatetime is not null)");
        if (cri != null)
          pgDbQueryBuilder.AppendLine("    and (" + cri.ToSQLClause() + ")");
        pgDbQueryBuilder.Append("    order by StartDateTime");
        DbCommandParameter parameter = new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.AnsiString);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(parameter);
        AppointmentInfo[] appointmentInfoArray = new AppointmentInfo[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          appointmentInfoArray[index] = this.rowToAppointmentInfo(dataRowCollection[index]);
        return appointmentInfoArray;
      }
      if ((userId ?? "") == "")
        userId = this.userId;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from Appointments Appointment");
      dbQueryBuilder.AppendLine("where UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " and (StartDatetime is not null) and (EndDatetime is not null)");
      if (cri != null)
        dbQueryBuilder.AppendLine("    and (" + cri.ToSQLClause() + ")");
      dbQueryBuilder.Append("    order by StartDateTime");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      AppointmentInfo[] appointmentInfoArray1 = new AppointmentInfo[dataRowCollection1.Count];
      for (int index = 0; index < dataRowCollection1.Count; ++index)
        appointmentInfoArray1[index] = this.rowToAppointmentInfo(dataRowCollection1[index]);
      return appointmentInfoArray1;
    }

    private DbValueList getDatabaseValueList(AppointmentInfo apptInfo)
    {
      DbValueList databaseValueList = new DbValueList();
      databaseValueList.Add("Subject", (object) apptInfo.Subject);
      databaseValueList.Add("StartDateTime", (object) apptInfo.StartDateTime);
      databaseValueList.Add("EndDateTime", (object) apptInfo.EndDateTime);
      databaseValueList.Add("Description", (object) apptInfo.Description);
      databaseValueList.Add("AllDayEvent", (object) apptInfo.AllDayEvent, (IDbEncoder) DbEncoding.Flag);
      databaseValueList.Add("ReminderEnabled", (object) apptInfo.ReminderEnabled, (IDbEncoder) DbEncoding.Flag);
      databaseValueList.Add("ReminderInterval", (object) apptInfo.ReminderInterval);
      databaseValueList.Add("ReminderUnits", (object) apptInfo.ReminderUnits);
      databaseValueList.Add("OwnerKey", (object) apptInfo.OwnerKey);
      databaseValueList.Add("IsRemoved", (object) apptInfo.IsRemoved, (IDbEncoder) DbEncoding.Flag);
      databaseValueList.Add("OriginalStartDateTime", (object) apptInfo.OriginalStartDateTime);
      databaseValueList.Add("DataKey", (object) apptInfo.DataKey);
      databaseValueList.Add("UserID", (object) apptInfo.UserID);
      databaseValueList.Add("LastModified", (object) DateTime.Now);
      if (apptInfo.RecurrenceId.ToString().Trim().Length != 0)
        databaseValueList.Add("RecurrenceId", (object) apptInfo.RecurrenceId);
      if (apptInfo.Recurrence == null)
        databaseValueList.Add("Recurrence", (object) null);
      else
        databaseValueList.Add("Recurrence", (object) "@Recurrence", (IDbEncoder) DbEncoding.None);
      if (apptInfo.AllProperties != null)
        databaseValueList.Add("AllProperties", (object) "@AllProperties", (IDbEncoder) DbEncoding.None);
      return databaseValueList;
    }

    [PgReady]
    private AppointmentInfo rowToAppointmentInfo(DataRow r)
    {
      return ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres ? new AppointmentInfo(EllieMae.EMLite.DataAccess.SQL.Decode(r["Subject"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["StartDateTime"], (object) DateTime.MinValue), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["EndDateTime"], (object) DateTime.MinValue), EllieMae.EMLite.DataAccess.SQL.Decode(r["Description"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["AllDayEvent"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["ReminderEnabled"]), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["ReminderInterval"], (object) 0), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["ReminderUnits"], (object) 0), EllieMae.EMLite.DataAccess.SQL.Decode(r["OwnerKey"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["RecurrenceId"], (object) "").ToString(), (byte[]) EllieMae.EMLite.DataAccess.SQL.Decode(r["Recurrence"], (object) null), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["IsRemoved"]), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["OriginalStartDateTime"], (object) DateTime.MinValue), EllieMae.EMLite.DataAccess.SQL.Decode(r["DataKey"], (object) "").ToString(), (byte[]) EllieMae.EMLite.DataAccess.SQL.Decode(r["AllProperties"], (object) null), EllieMae.EMLite.DataAccess.SQL.Decode(r["UserID"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["LastModified"], (object) DateTime.MinValue), CSMessage.AccessLevel.Full) : new AppointmentInfo(EllieMae.EMLite.DataAccess.SQL.Decode(r["Subject"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["StartDateTime"], (object) DateTime.MinValue), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["EndDateTime"], (object) DateTime.MinValue), EllieMae.EMLite.DataAccess.SQL.Decode(r["Description"], (object) "").ToString(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["AllDayEvent"], (object) false), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["ReminderEnabled"], (object) false), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["ReminderInterval"], (object) 0), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["ReminderUnits"], (object) 0), EllieMae.EMLite.DataAccess.SQL.Decode(r["OwnerKey"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["RecurrenceId"], (object) "").ToString(), (byte[]) EllieMae.EMLite.DataAccess.SQL.Decode(r["Recurrence"], (object) null), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["IsRemoved"], (object) false), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["OriginalStartDateTime"], (object) DateTime.MinValue), EllieMae.EMLite.DataAccess.SQL.Decode(r["DataKey"], (object) "").ToString(), (byte[]) EllieMae.EMLite.DataAccess.SQL.Decode(r["AllProperties"], (object) null), EllieMae.EMLite.DataAccess.SQL.Decode(r["UserID"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["LastModified"], (object) DateTime.MinValue), CSMessage.AccessLevel.Full);
    }

    public ContactInfo[] GetContactInfo(string DataKey)
    {
      string sql1 = "Select ContactID , FirstName , LastName from BizPartner where (ContactID IN(Select ContactID from AppointmentsXref where DataKey = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DataKey) + " and ContactType = " + (object) 1 + "))";
      string sql2 = "Select ContactID , FirstName , LastName from Borrower where (ContactID IN(Select ContactID from AppointmentsXref where DataKey = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DataKey) + " and ContactType = " + (object) 0 + "))";
      using (DbAccessManager dbAccessManager = new DbAccessManager())
      {
        DataRowCollection dataRowCollection1 = dbAccessManager.ExecuteQuery(sql1);
        DataRowCollection dataRowCollection2 = dbAccessManager.ExecuteQuery(sql2);
        ContactInfo[] contactInfo = new ContactInfo[dataRowCollection1.Count + dataRowCollection2.Count];
        int num = 0;
        for (int index = 0; index < dataRowCollection1.Count; ++index)
          contactInfo[num++] = new ContactInfo(dataRowCollection1[index]["FirstName"].ToString() + " " + dataRowCollection1[index]["LastName"].ToString(), dataRowCollection1[index]["ContactID"].ToString(), CategoryType.BizPartner);
        for (int index = 0; index < dataRowCollection2.Count; ++index)
          contactInfo[num++] = new ContactInfo(dataRowCollection2[index]["FirstName"].ToString() + " " + dataRowCollection2[index]["LastName"].ToString(), dataRowCollection2[index]["ContactID"].ToString(), CategoryType.Borrower);
        return contactInfo;
      }
    }
  }
}
