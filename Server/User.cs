// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.User
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using Elli.Common.Security;
using Elli.ElliEnum;
using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.MessageServices.Message;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataAccess.Postgres;
using EllieMae.EMLite.JedLib;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.Query;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.DBQueryGenerator;
using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class User : IDisposable
  {
    private const string className = "User�";
    private static string sw = Tracing.SwOutsideLoan;
    private string userId;
    private UserEntry data;
    private ICacheLock<UserEntry> innerLock;
    private UserEntry priorData;
    private static string currentLoginUserID = "";
    private static User.Helper helper = new User.Helper();
    private static b jed = (b) null;

    static User() => User.jed = a.b("8ddrw372kr0WXky0");

    public User(ICacheLock<UserEntry> innerLock, string currentLoginUser)
    {
      this.innerLock = innerLock;
      this.userId = innerLock.Identifier.ToString();
      User.currentLoginUserID = currentLoginUser;
      if (innerLock.Value == null)
        this.initializeUser();
      this.data = innerLock.Value;
      if (this.data == null)
        return;
      this.data.UserInfo.Password = (string) null;
      this.priorData = (UserEntry) this.data.Clone();
    }

    public User(UserEntry userEntry, string userId)
    {
      this.userId = userId;
      this.data = userEntry;
      if (this.data == null)
        return;
      this.priorData = (UserEntry) this.data.Clone();
    }

    public bool Exists => this.data != null;

    public string UserID
    {
      get
      {
        this.validateExists();
        return this.data.UserInfo.Userid;
      }
    }

    public UserInfo UserInfo
    {
      get
      {
        this.validateExists();
        return this.data.UserInfo;
      }
      set
      {
        this.validateInstance();
        if (value == (UserInfo) null)
          Err.Raise(TraceLevel.Error, nameof (User), new ServerException("UserInfo cannot be set to null"));
        if (value.Userid != this.userId)
          Err.Raise(TraceLevel.Error, nameof (User), new ServerException("UserID mismtach within UserInfo"));
        if (!value.ApiUser && !string.IsNullOrEmpty(value.Password) && ClientContext.GetCurrent().Settings.GetPasswordValidator().Check(value.Password) != PwdRuleValidator.ViolationCode.NoViolation)
          Err.Raise(TraceLevel.Error, nameof (User), (ServerException) new SecurityException("The password specified does not meet the requirements for this system."));
        if (value.LastLogin == DateTime.MinValue)
          value.LastLogin = this.data.UserInfo.LastLogin;
        this.data.UserInfo = value;
      }
    }

    public UserEntry PriorData => this.priorData;

    public static void updateUserDataServicesOpt(string userId, string key)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("update users set data_services_opt = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) key) + " where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    public int FailedLoginAttempts
    {
      get => this.data.ServerInfo.FailedLoginAttempts;
      set
      {
        this.data.ServerInfo.setFailedLoginAttemps(this.userId, value);
        this.UserInfo.failed_login_attempts = value;
      }
    }

    public DateTime LastPasswordChangedDate => this.data.ServerInfo.LastPasswordChangedDate;

    public DateTime? LastLockOutDateTime => this.data.ServerInfo.LastLockedOutDateTime;

    public bool IsTrustedUser
    {
      get
      {
        this.validateExists();
        return this.userId == "(trusted)";
      }
    }

    public bool IsInvalidPassword(UserInfo info)
    {
      return !info.ApiUser && !info.SSOOnly && ClientContext.GetCurrent().Settings.GetPasswordValidator().Check(info.Password) != 0;
    }

    public void CreateNew(UserInfo info, UserInfo loggedInUser)
    {
      this.validateInstance(false);
      if (this.Exists)
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("User already exists"));
      if (info == (UserInfo) null)
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Null argument to Create operation"));
      if (this.validateUserID(info.Userid))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("The provided UserID is not in valid format."));
      if (info.Userid != this.userId)
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("UserID mismatch for UserInfo object"));
      if (this.IsInvalidPassword(info))
        Err.Raise(TraceLevel.Error, nameof (User), (ServerException) new SecurityException("The password specified does not meet the requirements for this system."));
      if (info.Status == UserInfo.UserStatusEnum.Enabled)
        User.validateFreeUserLicense();
      User.deleteUserDirectory(info.Userid);
      User.getUserDirectory(info.Userid);
      User.getUserPipelineView(info.Userid);
      User.createUserInDatabase(info, loggedInUser);
      this.data = new UserEntry(info, new UserServerInfo())
      {
        UserInfo = {
          Password = (string) null
        }
      };
      this.innerLock.CheckIn(this.data, true);
    }

    public void Delete(
      UserInfo loggedInUser,
      UserAssignedContactsBehaviorEnums? assignedContactsBehavior = null,
      string reassignContactsToUser = null)
    {
      this.validateInstance();
      User.deleteUserFromDatabase(this.userId, assignedContactsBehavior, reassignContactsToUser);
      User.deleteUserDirectory(this.userId);
      this.innerLock.CheckIn((UserEntry) null);
      UserStore.RemoveCache(this.userId);
      User.PublishUserKafkaEvent((IEnumerable<string>) new List<string>()
      {
        this.UserInfo.Userid
      }, UserWebhookMessage.UserType.InternalUsers, "delete", loggedInUser);
      this.Dispose();
    }

    public byte[] GetPasswordHash(bool isFromOldTable)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (isFromOldTable)
      {
        DbTableInfo table = DbAccessManager.GetTable("users");
        DbValue key = new DbValue("userid", (object) this.userId);
        dbQueryBuilder.SelectFrom(table, new string[1]
        {
          "password"
        }, key);
      }
      else
      {
        dbQueryBuilder.AppendLine("declare @password varbinary(255)");
        dbQueryBuilder.AppendLine("select @password = password from [UserCredentials] where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId));
        dbQueryBuilder.AppendLine("if (@password is NULL)");
        dbQueryBuilder.AppendLine("    select @password = password from [Users] where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId));
        dbQueryBuilder.AppendLine("select @password");
      }
      return (byte[]) EllieMae.EMLite.DataAccess.SQL.Decode(dbQueryBuilder.ExecuteScalar());
    }

    public bool ComparePassword(string password)
    {
      this.validateExists();
      return User.compareUserPassword(this.userId, password);
    }

    public void VerifyPasswordHistoryRules(string password)
    {
      User.PasswordHistory passwordHistory = User.getPasswordHistory(this.UserID, password);
      ClientContext current = ClientContext.GetCurrent();
      int serverSetting1 = (int) current.Settings.GetServerSetting("Password.HistorySize");
      int serverSetting2 = (int) current.Settings.GetServerSetting("Password.DaysToReuse");
      if ((serverSetting1 > 0 || serverSetting2 > 0 || this.UserInfo.RequirePasswordChange) && this.ComparePassword(password))
        Err.Raise(TraceLevel.Info, nameof (User), (ServerException) new SecurityException("New password cannot be the same as your current password."));
      if (!(passwordHistory.LastDate is DateTime))
        return;
      TimeSpan timeSpan = DateTime.Now - (DateTime) passwordHistory.LastDate;
      if (serverSetting2 > 0 && timeSpan.Days < serverSetting2)
        Err.Raise(TraceLevel.Info, nameof (User), (ServerException) new SecurityException("Passwords cannot be reused within a " + (object) serverSetting2 + " day period."));
      if (serverSetting1 <= 0 || passwordHistory.PasswordsSince < 0 || passwordHistory.PasswordsSince >= serverSetting1)
        return;
      Err.Raise(TraceLevel.Info, nameof (User), (ServerException) new SecurityException("New password cannot be the same as your last " + (object) serverSetting1 + " password(s)."));
    }

    public DateTime GetPasswordExpirationDate()
    {
      this.validateExists();
      if (this.data.UserInfo.PasswordNeverExpires)
        return DateTime.MaxValue;
      int serverSetting = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.Lifetime");
      return serverSetting == 0 ? DateTime.MaxValue : this.data.ServerInfo.LastPasswordChangedDate.AddDays((double) serverSetting);
    }

    public void ChangeOrganization(int orgId, bool changeOrg = true)
    {
      this.validateInstance();
      if (changeOrg)
        User.updateUserOrganization(this.userId, orgId, true);
      bool isInRootOrg = orgId == OrganizationStore.RootOrganizationID;
      this.UserInfo = new UserInfo(this.UserInfo, orgId, isInRootOrg);
      if (this.UserInfo.InheritParentCompPlan)
        this.UserInfo.InheritParentCompPlan = false;
      this.innerLock.CheckIn(true);
    }

    public string[] GetRecentLoanGuids(int count, bool isExternalOrganization)
    {
      this.validateExists();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DataQuery query = new DataQuery();
      query.MaxNumberOfResults = 10;
      query.Selections.AddField("Loan.Guid");
      query.Filter = (QueryCriterion) new DateValueCriterion("UserActivity.LastAccessTime", DateTime.MaxValue);
      query.SortFields.Add(new SortField("UserActivity.LastAccessTime", FieldSortOrder.Descending));
      QueryResult queryResult = new LoanQuery(this.UserInfo).Execute(query, isExternalOrganization);
      List<string> stringList = new List<string>();
      for (int row = 0; row < queryResult.RecordCount; ++row)
      {
        if (string.Concat(queryResult[row, 0]) != "")
          stringList.Add(string.Concat(queryResult[row, 0]));
      }
      return stringList.ToArray();
    }

    public List<RecentLoanInfo> GetRecentLoansInfo(int count, bool isExternalOrganization)
    {
      List<RecentLoanInfo> recentLoansInfo = new List<RecentLoanInfo>();
      this.validateExists();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DataQuery query = new DataQuery();
      query.MaxNumberOfResults = 10;
      query.Selections.AddField("Loan.Guid");
      query.Selections.AddField("UserActivity.LastAccessTime");
      query.Filter = (QueryCriterion) new DateValueCriterion("UserActivity.LastAccessTime", DateTime.MaxValue);
      query.SortFields.Add(new SortField("UserActivity.LastAccessTime", FieldSortOrder.Descending));
      QueryResult queryResult = new LoanQuery(this.UserInfo).Execute(query, isExternalOrganization);
      for (int row = 0; row < queryResult.RecordCount; ++row)
      {
        string str = queryResult[row, 0].ToString();
        DateTime dateTime = DateTime.Parse(queryResult[row, 1].ToString());
        if (!str.IsNullOrEmpty())
        {
          RecentLoanInfo recentLoanInfo = new RecentLoanInfo()
          {
            LoanId = str,
            LastAccessTime = dateTime,
            UserId = this.UserID
          };
          recentLoansInfo.Add(recentLoanInfo);
        }
      }
      return recentLoansInfo;
    }

    public FileSystemEntry[] GetRecentReports(int count)
    {
      this.validateExists();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select top " + (object) count + " ReportPath, LastExecutionTime from recent_reports where (userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") order by LastExecutionTime desc");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      FileSystemEntry[] recentReports = new FileSystemEntry[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        DataRow dataRow = dataRowCollection[index];
        recentReports[index] = FileSystemEntry.Parse(dataRow["ReportPath"].ToString(), this.userId);
        recentReports[index].Properties[(object) "LastExecutionTime"] = (object) EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["LastExecutionTime"]);
      }
      return recentReports;
    }

    public string getXMLSettingsRpt(string reportID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT XMLReport FROM [SettingsRptQueue] WHERE reportID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) reportID);
      dbQueryBuilder.AppendLine(text);
      return dbQueryBuilder.ExecuteScalar().ToString();
    }

    public SettingsRptJobInfo[] getSettingsRptJobs()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string text = "SELECT reportID, reportName, jobtype, ID, Description, Status, Message, CreatedBy, CreateDate FROM [SettingsRptQueue] order by ReportID desc";
      dbQueryBuilder.AppendLine(text);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      SettingsRptJobInfo[] settingsRptJobs = new SettingsRptJobInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        DataRow dataRow = dataRowCollection[index];
        SettingsRptJobInfo.jobStatus int16_1 = (SettingsRptJobInfo.jobStatus) Convert.ToInt16(dataRowCollection[index]["status"].ToString());
        SettingsRptJobInfo.jobType int16_2 = (SettingsRptJobInfo.jobType) Convert.ToInt16(dataRowCollection[index]["jobtype"].ToString());
        settingsRptJobs[index] = new SettingsRptJobInfo(int16_2, dataRowCollection[index]["reportname"].ToString(), int16_1, dataRowCollection[index]["createdby"].ToString(), dataRowCollection[index]["createdate"].ToString());
        settingsRptJobs[index].ReportID = dataRowCollection[index]["reportID"].ToString();
        settingsRptJobs[index].Message = dataRowCollection[index]["message"].ToString();
      }
      return settingsRptJobs;
    }

    public void AddToRecentLoans(string guid)
    {
      this.validateInstance();
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @guid varchar(40)");
        dbQueryBuilder.AppendLine("declare @userId varchar(16)");
        dbQueryBuilder.AppendLine("select @guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
        dbQueryBuilder.AppendLine("select @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId));
        dbQueryBuilder.AppendLine("delete from recent_loans where (guid = @guid) and (userid = @userId)");
        dbQueryBuilder.AppendLine("insert into recent_loans (userid, guid, LastAccessTime) values (@userId, @guid, GetDate())");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    [PgReady]
    public void AddRemoveRecentLoans(string guid, int count = 0)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        this.validateExists();
        try
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("delete from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + ");");
          pgDbQueryBuilder.AppendLine("insert into [recent_loans] ([userid], [guid], [LastAccessTime]) values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + ", GetDate());");
          if (count <= 0)
          {
            pgDbQueryBuilder.ExecuteNonQuery();
          }
          else
          {
            pgDbQueryBuilder.AppendLine("select [LastAccessTime] from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") order by [LastAccessTime] desc limit " + (object) (count + 1) + ";");
            DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
            if (dataRowCollection.Count <= count)
              return;
            DateTime dateTime = (DateTime) dataRowCollection[dataRowCollection.Count - 1]["LastAccessTime"];
            pgDbQueryBuilder.Reset();
            pgDbQueryBuilder.AppendLine("delete from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([LastAccessTime] <= " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTime, false) + ");");
            pgDbQueryBuilder.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
        }
      }
      else
      {
        this.validateExists();
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("delete from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([guid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + ")");
          dbQueryBuilder.AppendLine("insert into [recent_loans] ([userid], [guid], [LastAccessTime]) values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + ", GetDate())");
          if (count <= 0)
          {
            dbQueryBuilder.ExecuteNonQuery();
          }
          else
          {
            dbQueryBuilder.AppendLine("select top " + (object) (count + 1) + "[LastAccessTime] from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") order by [LastAccessTime] desc");
            DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
            if (dataRowCollection.Count <= count)
              return;
            DateTime dateTime = (DateTime) dataRowCollection[dataRowCollection.Count - 1]["LastAccessTime"];
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("delete from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([LastAccessTime] <= " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTime, false) + ")");
            dbQueryBuilder.ExecuteNonQuery();
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
        }
      }
    }

    public void AddRemoveRecentLoans(string[] guids, int count = 0)
    {
      this.validateExists();
      try
      {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        string str = "";
        foreach (string guid in guids)
        {
          stringBuilder1.Append(str);
          stringBuilder2.Append(str);
          stringBuilder1.Append(EllieMae.EMLite.DataAccess.SQL.Encode((object) guid));
          stringBuilder2.Append("(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + "," + EllieMae.EMLite.DataAccess.SQL.Encode((object) guid) + ", GetDate())");
          str = ",";
        }
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([guid] IN (" + stringBuilder1.ToString() + "))");
        dbQueryBuilder.AppendLine("insert into [recent_loans] ([userid], [guid], [LastAccessTime]) values " + stringBuilder2.ToString());
        if (count <= 0)
        {
          dbQueryBuilder.ExecuteNonQuery();
        }
        else
        {
          dbQueryBuilder.AppendLine("select top " + (object) (count + 1) + "[LastAccessTime] from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") order by [LastAccessTime] desc");
          if (dbQueryBuilder.Execute().Count <= count)
            return;
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("delete from [recent_loans] where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and [guid] NOT IN (SELECT TOP " + EllieMae.EMLite.DataAccess.SQL.Encode((object) count) + " [Guid] FROM [recent_loans] WHERE ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") ORDER BY [LastAccessTime] desc)");
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    public void AddToRecentReports(FileSystemEntry fsEntry)
    {
      this.validateExists();
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("declare @reportPath varchar(512)");
        dbQueryBuilder.AppendLine("declare @userId varchar(16)");
        dbQueryBuilder.AppendLine("declare @itemCount int");
        dbQueryBuilder.AppendLine("select @reportPath = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) fsEntry.ToDisplayString()));
        dbQueryBuilder.AppendLine("select @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId));
        dbQueryBuilder.AppendLine("delete from recent_reports where (ReportPath = @reportPath) and (UserID = @userId)");
        dbQueryBuilder.AppendLine("insert into recent_reports (UserID, ReportPath, LastExecutionTime) values (@userId, @reportPath, GetDate())");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    [PgReady]
    public string[] AddRemoveAndGetRecentSettings(
      bool isCompanySettings,
      string settingTitle,
      bool add,
      int count,
      bool deleteExtra)
    {
      this.validateInstance();
      string str = isCompanySettings ? "[RecentCompanySettings]" : "[RecentPersonalSettings]";
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        if (settingTitle != null)
        {
          pgDbQueryBuilder.AppendLine("delete from " + str + " where ([settingTitle] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingTitle) + ") and ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ");");
          if (add)
            pgDbQueryBuilder.AppendLine("insert into " + str + " ([userid], [settingTitle], [lastAccessTime]) values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingTitle) + ", GetDate());");
        }
        if (count <= 0)
        {
          pgDbQueryBuilder.ExecuteNonQuery();
          return new string[0];
        }
        pgDbQueryBuilder.AppendLine("select [settingTitle], [lastAccessTime] from " + str + " where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") order by [lastAccessTime] desc limit " + (object) count + ";");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        string[] recentSettings = new string[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          recentSettings[index] = dataRowCollection[index][nameof (settingTitle)].ToString();
        if (deleteExtra)
        {
          if (dataRowCollection.Count == count)
          {
            try
            {
              DateTime dateTime = (DateTime) dataRowCollection[count - 1]["lastAccessTime"];
              pgDbQueryBuilder.Reset();
              pgDbQueryBuilder.AppendLine("delete from " + str + " where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([lastAccessTime] < " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTime, false) + ");");
              pgDbQueryBuilder.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              Tracing.Log(User.sw, TraceLevel.Warning, nameof (User), "Error deleting extra recent settings from table " + str + ": " + ex.Message);
            }
          }
        }
        return recentSettings;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (settingTitle != null)
      {
        dbQueryBuilder.AppendLine("delete from " + str + " where ([settingTitle] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingTitle) + ") and ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ")");
        if (add)
          dbQueryBuilder.AppendLine("insert into " + str + " ([userid], [settingTitle], [lastAccessTime]) values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) settingTitle) + ", GetDate())");
      }
      if (count <= 0)
      {
        dbQueryBuilder.ExecuteNonQuery();
        return new string[0];
      }
      dbQueryBuilder.AppendLine("select top " + (object) count + " [settingTitle], [lastAccessTime] from " + str + " where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") order by [lastAccessTime] desc");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      string[] recentSettings1 = new string[dataRowCollection1.Count];
      for (int index = 0; index < dataRowCollection1.Count; ++index)
        recentSettings1[index] = dataRowCollection1[index][nameof (settingTitle)].ToString();
      if (deleteExtra)
      {
        if (dataRowCollection1.Count == count)
        {
          try
          {
            DateTime dateTime = (DateTime) dataRowCollection1[count - 1]["lastAccessTime"];
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("delete from " + str + " where ([userid] = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.userId) + ") and ([lastAccessTime] < " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(dateTime, false) + ")");
            dbQueryBuilder.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            Tracing.Log(User.sw, TraceLevel.Warning, nameof (User), "Error deleting extra recent settings from table " + str + ": " + ex.Message);
          }
        }
      }
      return recentSettings1;
    }

    public bool IsPasswordChangeRequired()
    {
      this.validateExists();
      if (this.userId == "admin" || string.Compare(this.userId, "tpowcadmin", true) == 0 || this.UserInfo.SSOOnly)
        return false;
      return this.UserInfo.RequirePasswordChange || this.GetPasswordExpirationDate().Date < DateTime.Today;
    }

    public LOLicenseInfo[] GetLOLicenses() => User.getLOLicensesFromDatabase(this.userId);

    public LOLicenseInfo GetLOLicense(string state)
    {
      return User.getLOLicenseFromDatabase(this.userId, state);
    }

    public string[] GetAllLicensedStates() => User.getLOLicensedStateList(this.userId);

    public void DeleteLOLicense(string state)
    {
      User.deleteLOLicenseFromDatabase(this.userId, state);
    }

    public void DeleteAllLOLicenses() => User.deleteAllLOLicensesFromDatabase(this.userId);

    public void AddLOLicense(LOLicenseInfo license)
    {
      User.addLOLicenseToDatabase(this.userId, license);
    }

    public void CheckIn(string loggedInUserId, bool loginFailed)
    {
      this.CheckIn(false, loggedInUserId, loginFailed);
    }

    public void CheckIn(bool keepCheckedOut, string loggedInUserId, bool loginFailed)
    {
      this.CheckIn(this.UserInfo, keepCheckedOut, loggedInUserId, loginFailed);
    }

    public void CheckIn(UserInfo newInfo, string loggedInUserId)
    {
      this.CheckIn(newInfo, false, loggedInUserId);
    }

    public void ResetLoginInfo(bool keepCheckedOut = false)
    {
      try
      {
        if (this.UserInfo.Status == UserInfo.UserStatusEnum.Disabled && this.userId == "admin")
          throw new ServerException("The 'admin' user cannot be disabled");
        if (this.data.UserInfo.Userid == "(trusted)")
          Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot update trusted user account"));
        if (this.UserInfo.Locked)
        {
          this.UserInfo.lastModifiedDate = new DateTime?(DateTime.UtcNow);
          this.UserInfo.lastModifiedBy = this.userId;
        }
        this.UserInfo.Locked = false;
        this.data.UserInfo = this.UserInfo;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine(string.Format("update users set failed_login_attempts = {0} ,last_login = {1}, locked = {2},lastLockedOutDateTime = {3}, LastModifiedDate = {4}, LastModifiedBy = {5}", (object) this.UserInfo.failed_login_attempts, (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(this.UserInfo.LastLogin), (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(this.UserInfo.Locked), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(this.UserInfo.LastLockedOutDateTime.GetValueOrDefault(), DateTime.MinValue), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(this.UserInfo.lastModifiedDate.GetValueOrDefault(), DateTime.MinValue), (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(this.UserInfo.lastModifiedBy)));
        if (!string.IsNullOrEmpty(this.UserInfo.EncompassVersion))
          dbQueryBuilder.AppendLine(", enc_version = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.UserInfo.EncompassVersion));
        dbQueryBuilder.AppendLine(" where userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.UserInfo.Userid));
        dbQueryBuilder.ExecuteNonQuery();
        this.innerLock.CheckIn(this.data, keepCheckedOut);
        if (keepCheckedOut)
          return;
        this.Dispose();
      }
      catch (Exception ex)
      {
        Tracing.Log(User.sw, TraceLevel.Warning, nameof (User), "CheckInLoginInfo failed for the user :" + this.data.UserInfo.Userid + ": " + ex.Message);
      }
    }

    public void CheckIn(
      UserInfo newInfo,
      bool keepCheckedOut,
      string loggedInUserID,
      bool loginFailed = false)
    {
      this.validateInstance();
      if (this.priorData != null && this.priorData.UserInfo.Status == UserInfo.UserStatusEnum.Disabled && newInfo.Status == UserInfo.UserStatusEnum.Enabled)
        User.validateFreeUserLicense();
      if (newInfo.Status == UserInfo.UserStatusEnum.Disabled && this.userId == "admin")
        throw new ServerException("The 'admin' user cannot be disabled");
      newInfo.IsTopLevelUser = newInfo.OrgId == OrganizationStore.RootOrganizationID;
      this.data.UserInfo = newInfo;
      User.updateUser(this.data, this.priorData, loggedInUserID, loginFailed);
      this.UserInfo.Password = (string) null;
      this.innerLock.CheckIn(this.data, keepCheckedOut);
      if (keepCheckedOut)
        return;
      this.Dispose();
    }

    public void UndoCheckout()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.UndoCheckout();
      this.Dispose();
    }

    public void Dispose()
    {
      if (this.innerLock == null)
        return;
      this.innerLock.Dispose();
      this.innerLock = (ICacheLock<UserEntry>) null;
    }

    public UserLicenseInfo GetUserLicense()
    {
      this.validateExists();
      string clientId = Company.GetCompanyInfo().ClientID;
      try
      {
        UserLicenseInfo userLicense = UserLicenseInfo.Parse(User.GetPrivateProfileString(this.userId, "USER", "LICENSE"));
        if (userLicense.UserID == this.userId)
        {
          if (clientId == userLicense.ClientID)
            return userLicense;
        }
      }
      catch
      {
      }
      return new UserLicenseInfo(clientId, this.userId);
    }

    public void UpdateUserLicense(UserLicenseInfo info)
    {
      this.validateExists();
      info.Timestamp = DateTime.Now;
      User.WritePrivateProfileString(this.userId, "USER", "LICENSE", info.ToString());
    }

    public static List<string> GetUserIdFromClientId(string clientId)
    {
      List<string> userIdFromClientId = (List<string>) null;
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select userid from users where OAuthClientId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) clientId));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return userIdFromClientId;
        userIdFromClientId = new List<string>();
        for (int index = 0; index < dataRowCollection.Count; ++index)
          userIdFromClientId.Add(EllieMae.EMLite.DataAccess.SQL.Decode(dataRowCollection[index]["userid"], (object) "").ToString());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
      return userIdFromClientId;
    }

    public static string GetPrivateProfileString(string userId, string section, string key)
    {
      try
      {
        DataRowCollection rows = new UserSettingsAccessor().GetRows((string) null, new UserSettingsAccessor.PrimaryKeyValues(key, section, userId));
        if (rows.Count != 1)
          privateProfileString = "";
        else if (!(rows[0]["value"] is string privateProfileString))
          privateProfileString = "";
        return privateProfileString;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (string) null;
      }
    }

    public static IDictionary GetPrivateProfileSettings(string userId, string section)
    {
      DataRowCollection rows = new UserSettingsAccessor().GetRows((string) null, new UserSettingsAccessor.PrimaryKeyValues((string) null, section, userId));
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (DataRow dataRow in (InternalDataCollectionBase) rows)
      {
        Hashtable hashtable = insensitiveHashtable;
        string key = dataRow["category"].ToString() + "." + dataRow["attribute"];
        if (!(dataRow["value"] is string str))
          str = "";
        hashtable[(object) key] = (object) str;
      }
      return (IDictionary) insensitiveHashtable;
    }

    public static void WritePrivateProfileString(
      string userId,
      string section,
      string key,
      string value)
    {
      try
      {
        UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
        UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues(key, section, userId);
        settingsAccessor.UpsertValue(value?.Substring(0, Math.Min(1024, value.Length)), pk);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    [PgReady]
    public static void WritePrivateProfileString2(
      ClientContext context,
      string userId,
      string section,
      string key,
      string value)
    {
      try
      {
        using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
          UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues(key, section, userId);
          settingsAccessor.UpsertValue(value, pk);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    public static void RenameFavoriteLoanTemplateSet(
      bool isFile,
      string existingPath,
      string replacementPath,
      string category)
    {
      UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
      UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues((string) null, category, (string) null);
      if (isFile)
        settingsAccessor.UpdateValues(existingPath, replacementPath, pk);
      else
        settingsAccessor.ReplaceAllInValues(existingPath, replacementPath, pk);
    }

    public static void DeleteFavoriteLoanTemplateSet(string path, string category)
    {
      try
      {
        UserSettingsAccessor settingsAccessor = new UserSettingsAccessor();
        UserSettingsAccessor.PrimaryKeyValues pk = new UserSettingsAccessor.PrimaryKeyValues((string) null, category, (string) null);
        settingsAccessor.Delete(path, pk);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    [PgReady]
    public Hashtable GetAllPrivateProfileStrings()
    {
      this.validateExists();
      try
      {
        return User.GetPrivateProfileSettings(this.userId, (string) null) as Hashtable;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (Hashtable) null;
      }
    }

    public static string[] GetSettingsFileNames(string userId)
    {
      DirectoryInfo userDirectory = User.getUserDirectory(userId);
      if (!userDirectory.Exists)
        return new string[0];
      FileInfo[] files = userDirectory.GetFiles();
      string[] settingsFileNames = new string[files.Length];
      for (int index = 0; index < files.Length; ++index)
        settingsFileNames[index] = files[index].Name;
      return settingsFileNames;
    }

    public static BinaryObject GetSettingsFile(string userId, string key)
    {
      return User.readUserDataFile(userId, key);
    }

    public static void SaveSettingsFile(string userId, string key, BinaryObject data)
    {
      User.writeUserDataFile(userId, key, data);
    }

    public static BinaryObject GetDashboardMapFile(string userId, string key)
    {
      return User.readUserDashboardMapFile(userId, key);
    }

    public static void SaveDashboardMapFile(string userId, string key, BinaryObject data)
    {
      User.writeUserDashboardMapFile(userId, key, data);
    }

    public static BinaryObject GetCustomDataObjectFile(string userId, string key)
    {
      return User.readCustomDataFile(userId, key);
    }

    public static void SaveCustomDataObjectFile(string userId, string key, BinaryObject data)
    {
      User.writeCustomDataFile(userId, key, data);
    }

    public static void AppendToCustomDataObjectFile(string userId, string key, BinaryObject data)
    {
      User.appendToCustomDataFile(userId, key, data);
    }

    public static string GetDefaultProviderInfo(string userId)
    {
      using (BinaryObject binaryObject = User.readUserDataFile(userId, "Providers.xml"))
        return binaryObject == null ? "" : binaryObject.ToString();
    }

    public static void SaveDefaultProviderInfo(string userId, string providerInfo)
    {
      User.writeUserDataFile(userId, "Providers.xml", new BinaryObject(providerInfo, Encoding.Default));
    }

    public static UserInfo[] GetAclGroupScopedUsers(int groupId, int roleId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join AclGroupRoleUserRef gruf on gruf.userid = u.userid and rp.roleID = gruf.roleID");
      dbQueryBuilder.AppendLine("where rp.roleID = " + (object) roleId);
      dbQueryBuilder.AppendLine("  and gruf.GroupID = " + (object) groupId);
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join AclGroupRoleOrgRef gruf on gruf.orgID = u.org_id and rp.roleID = gruf.roleID");
      dbQueryBuilder.AppendLine("where rp.roleID = " + (object) roleId);
      dbQueryBuilder.AppendLine("  and gruf.GroupID = " + (object) groupId);
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join org_descendents od on u.org_id = od.descendent");
      dbQueryBuilder.AppendLine("  inner join AclGroupRoleOrgRef gruf on gruf.orgID = od.oid and rp.roleID = gruf.roleID");
      dbQueryBuilder.AppendLine("where rp.roleID = " + (object) roleId);
      dbQueryBuilder.AppendLine("  and gruf.GroupID = " + (object) groupId);
      dbQueryBuilder.AppendLine("  and gruf.inclusive = 1");
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static Dictionary<int, Dictionary<int, List<UserInfo>>> GetAclGroupScopedUsers(
      int[] groupIDs,
      int[] roleIDs,
      Hashtable personaLookup)
    {
      string str1 = string.Join<int>(", ", (IEnumerable<int>) groupIDs);
      string str2 = string.Join<int>(", ", (IEnumerable<int>) roleIDs);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select rp.roleID, gruf.GroupID, u.*, p.*, org_chart.depth from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join AclGroupRoleUserRef gruf on gruf.userid = u.userid and rp.roleID = gruf.roleID");
      dbQueryBuilder.AppendLine("  inner join org_chart on u.org_id = org_chart.oid");
      dbQueryBuilder.AppendLine("where rp.roleID in (" + str2 + ")");
      dbQueryBuilder.AppendLine("  and gruf.GroupID in (" + str1 + ")");
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select rp.roleID, gruf.GroupID, u.*, p.*, org_chart.depth from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join AclGroupRoleOrgRef gruf on gruf.orgID = u.org_id and rp.roleID = gruf.roleID");
      dbQueryBuilder.AppendLine("  inner join org_chart on u.org_id = org_chart.oid");
      dbQueryBuilder.AppendLine("where rp.roleID in (" + str2 + ")");
      dbQueryBuilder.AppendLine("  and gruf.GroupID in (" + str1 + ")");
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select rp.roleID, gruf.GroupID, u.*, p.*, org_chart.depth from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join org_descendents od on u.org_id = od.descendent");
      dbQueryBuilder.AppendLine("  inner join AclGroupRoleOrgRef gruf on gruf.orgID = od.oid and rp.roleID = gruf.roleID");
      dbQueryBuilder.AppendLine("  inner join org_chart on u.org_id = org_chart.oid");
      dbQueryBuilder.AppendLine("where rp.roleID in (" + str2 + ")");
      dbQueryBuilder.AppendLine("  and gruf.GroupID in (" + str1 + ")");
      dbQueryBuilder.AppendLine("  and gruf.inclusive = 1");
      DataRowCollection rows = dbQueryBuilder.Execute();
      Dictionary<int, Dictionary<int, List<UserInfo>>> groupScopedUsers = new Dictionary<int, Dictionary<int, List<UserInfo>>>();
      Dictionary<string, Persona[]> personas1 = User.buildDatabaseRowsToPersonas(rows, personaLookup, User.UserRoleGroupEnum.RoleGroup);
      foreach (DataRow r in (InternalDataCollectionBase) rows)
      {
        int key1 = (int) r["roleID"];
        int key2 = (int) r["GroupID"];
        string key3 = key1.ToString() + "_" + (object) key2;
        if (!groupScopedUsers.ContainsKey(key1))
          groupScopedUsers.Add(key1, new Dictionary<int, List<UserInfo>>());
        Dictionary<int, List<UserInfo>> dictionary = groupScopedUsers[key1];
        if (!dictionary.ContainsKey(key2))
          dictionary.Add(key2, new List<UserInfo>());
        List<UserInfo> userInfoList = dictionary[key2];
        Persona[] personas2 = (Persona[]) null;
        if (personas1.TryGetValue(key3, out personas2))
          userInfoList.Add(User.databaseRowToUserInfo(r, personas2, ""));
      }
      return groupScopedUsers;
    }

    public static UserInfo[] GetAllUsersWithRole(int roleId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("where rp.roleID = " + (object) roleId);
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static UserInfo[] GetAllUsersWithPersona(int personaId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("where p.personaID = " + (object) personaId);
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static UserInfo[] GetAllUsersInUserGroup(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join AclGroupUserRef as g on g.userid=u.userid");
      dbQueryBuilder.AppendLine("where g.groupID = " + (object) groupId);
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static UserInfo[] GetUsersByName(string name)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select userid from users where ");
      dbQueryBuilder.Append("(first_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(name) + "%') or (last_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(name) + "%')");
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static Dictionary<int, List<UserInfo>> GetAllUsersWithRoles(
      int[] roleIDs,
      Hashtable personaLookup)
    {
      try
      {
        string str = string.Join<int>(", ", (IEnumerable<int>) roleIDs);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select rp.roleID, u.*, p.*, org_chart.depth from users u");
        dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
        dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
        dbQueryBuilder.AppendLine("  inner join org_chart on u.org_id = org_chart.oid");
        dbQueryBuilder.AppendLine("where rp.roleID in (" + str + ")");
        return User.dataRowsToUserInfosByRole(dbQueryBuilder.Execute(DbTransactionType.None), personaLookup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), User.checkInvalidOrgIDs(ex));
        return (Dictionary<int, List<UserInfo>>) null;
      }
    }

    private static Dictionary<int, List<UserInfo>> dataRowsToUserInfosByRole(
      DataRowCollection rows,
      Hashtable personaLookup)
    {
      Dictionary<int, List<UserInfo>> userInfosByRole = new Dictionary<int, List<UserInfo>>();
      Dictionary<string, Persona[]> personas1 = User.buildDatabaseRowsToPersonas(rows, personaLookup, User.UserRoleGroupEnum.RoleOnly);
      foreach (DataRow row in (InternalDataCollectionBase) rows)
      {
        int key = (int) row["roleID"];
        if (!userInfosByRole.ContainsKey(key))
          userInfosByRole.Add(key, new List<UserInfo>());
        List<UserInfo> userInfoList = userInfosByRole[key];
        Persona[] personas2 = (Persona[]) null;
        if (personas1.TryGetValue(key.ToString(), out personas2))
          userInfoList.Add(User.databaseRowToUserInfo(row, personas2, ""));
      }
      return userInfosByRole;
    }

    private static Exception checkInvalidOrgIDs(Exception ex)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select [userid], [org_id] from [users] where [org_id] not in (select [oid] from [org_chart])");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection.Count > 0)
          ex = new Exception("There is/are " + (object) dataRowCollection.Count + " user(s) with invalid org IDs.", ex);
      }
      catch
      {
      }
      return ex;
    }

    public static UserInfo[] GetAclGroupUsersBelowWithRole(int roleId, string currentUserId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join users cu on cu.org_id = u.org_id");
      dbQueryBuilder.AppendLine("where rp.roleID = " + (object) roleId);
      dbQueryBuilder.AppendLine("  and cu.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) currentUserId));
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select distinct u.userid from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join org_descendents od on u.org_id = od.descendent");
      dbQueryBuilder.AppendLine("  inner join users cu on cu.org_id = od.oid");
      dbQueryBuilder.AppendLine("where rp.roleID = " + (object) roleId);
      dbQueryBuilder.AppendLine("  and cu.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) currentUserId));
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static Dictionary<int, List<UserInfo>> GetAclGroupUsersBelowWithRoles(
      int[] roleIDs,
      string currentUserId,
      Hashtable personaLookup)
    {
      string str = string.Join<int>(", ", (IEnumerable<int>) roleIDs);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select rp.roleID, u.*, p.*, org_chart.depth from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join users cu on cu.org_id = u.org_id");
      dbQueryBuilder.AppendLine("  inner join org_chart on u.org_id = org_chart.oid");
      dbQueryBuilder.AppendLine("where rp.roleID in (" + str + ")");
      dbQueryBuilder.AppendLine("  and cu.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) currentUserId));
      dbQueryBuilder.AppendLine("union");
      dbQueryBuilder.AppendLine("select rp.roleID, u.*, p.*, org_chart.depth from users u");
      dbQueryBuilder.AppendLine("  inner join UserPersona p on u.userid = p.userid");
      dbQueryBuilder.AppendLine("  inner join RolePersonas rp on p.personaid = rp.personaid");
      dbQueryBuilder.AppendLine("  inner join org_descendents od on u.org_id = od.descendent");
      dbQueryBuilder.AppendLine("  inner join users cu on cu.org_id = od.oid");
      dbQueryBuilder.AppendLine("  inner join org_chart on u.org_id = org_chart.oid");
      dbQueryBuilder.AppendLine("where rp.roleID in (" + str + ")");
      dbQueryBuilder.AppendLine("  and cu.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) currentUserId));
      return User.dataRowsToUserInfosByRole(dbQueryBuilder.Execute(), personaLookup);
    }

    public static UserInfo[] GetScopedUsersWithRole(string userId, int roleId)
    {
      UserInfo userInfo1 = User.IsVirtualUser(userId) ? User.getVirtualUserEntry(userId).UserInfo : User.GetUserById(userId);
      if (UserInfo.IsSuperAdministrator(userInfo1.Userid, userInfo1.UserPersonas))
        return User.GetAllUsersWithRole(roleId);
      AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userId);
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = true;
      Set set = (Set) new ListSet();
      for (int index = 0; index < groupsOfUser.Length; ++index)
      {
        AclGroup aclGroup = groupsOfUser[index];
        AclGroupRoleAccessLevel groupRoleAccessLevel = AclGroupRoleAccessor.GetAclGroupRoleAccessLevel(aclGroup.ID, roleId);
        if (!groupRoleAccessLevel.HideDisabledAccount)
          flag3 = false;
        if (!flag2 && groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.All)
        {
          UserInfo[] allUsersWithRole = User.GetAllUsersWithRole(roleId);
          set.AddAll((ICollection) allUsersWithRole);
          flag2 = true;
          flag1 = false;
        }
        else if (!flag2 && groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.BelowInOrg)
          flag1 = true;
        else if (!flag2)
        {
          UserInfo[] groupScopedUsers = User.GetAclGroupScopedUsers(aclGroup.ID, roleId);
          set.AddAll((ICollection) groupScopedUsers);
        }
        if (!flag3 & flag2)
          break;
      }
      if (flag1)
      {
        UserInfo[] usersBelowWithRole = User.GetAclGroupUsersBelowWithRole(roleId, userId);
        set.AddAll((ICollection) usersBelowWithRole);
      }
      UserInfo[] scopedUsersWithRole = new UserInfo[set.Count];
      set.CopyTo((Array) scopedUsersWithRole, 0);
      if (flag3)
      {
        ArrayList arrayList = new ArrayList();
        foreach (UserInfo userInfo2 in scopedUsersWithRole)
        {
          if (userInfo2.Status != UserInfo.UserStatusEnum.Disabled)
            arrayList.Add((object) userInfo2);
        }
        scopedUsersWithRole = (UserInfo[]) arrayList.ToArray(typeof (UserInfo));
      }
      return scopedUsersWithRole;
    }

    public static UserInfo[] GetScopedUsersWithRoles(UserInfo userInfo, int[] roleIDs)
    {
      if (userInfo == (UserInfo) null || roleIDs == null)
        return (UserInfo[]) null;
      if (roleIDs.Length == 0)
        return new UserInfo[0];
      string userid = userInfo.Userid;
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      if (UserInfo.IsSuperAdministrator(userid, userInfo.UserPersonas))
      {
        List<UserInfo> userInfoList = new List<UserInfo>();
        Dictionary<int, List<UserInfo>> allUsersWithRoles = User.GetAllUsersWithRoles(roleIDs, personaLookup);
        foreach (int key in allUsersWithRoles.Keys)
        {
          List<UserInfo> collection = allUsersWithRoles[key];
          userInfoList.AddRange((IEnumerable<UserInfo>) collection);
        }
        return userInfoList.ToArray();
      }
      Dictionary<string, UserInfo> dictionary1 = new Dictionary<string, UserInfo>();
      int[] aclGroupIdsByUser = AclGroupRoleAccessor.GetAclGroupIDsByUser(userid);
      Dictionary<int, Dictionary<int, AclGroupRoleAccessLevel>> roleAccessLevels = AclGroupRoleAccessor.GetAclGroupRoleAccessLevels(userid, roleIDs, aclGroupIdsByUser);
      Dictionary<int, Dictionary<int, List<UserInfo>>> groupScopedUsers = User.GetAclGroupScopedUsers(aclGroupIdsByUser, roleIDs, personaLookup);
      Dictionary<int, List<UserInfo>> allUsersWithRoles1 = User.GetAllUsersWithRoles(roleIDs, personaLookup);
      Dictionary<int, List<UserInfo>> usersBelowWithRoles = User.GetAclGroupUsersBelowWithRoles(roleIDs, userid, personaLookup);
      foreach (int key1 in roleAccessLevels.Keys)
      {
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = true;
        Set set = (Set) new ListSet();
        Dictionary<int, AclGroupRoleAccessLevel> dictionary2 = roleAccessLevels[key1];
        foreach (int key2 in dictionary2.Keys)
        {
          AclGroupRoleAccessLevel groupRoleAccessLevel = dictionary2[key2];
          if (!groupRoleAccessLevel.HideDisabledAccount)
            flag3 = false;
          if (!flag2 && groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.All)
          {
            UserInfo[] c = !allUsersWithRoles1.ContainsKey(key1) ? new UserInfo[0] : allUsersWithRoles1[key1].ToArray();
            set.AddAll((ICollection) c);
            flag2 = true;
            flag1 = false;
          }
          else if (!flag2 && groupRoleAccessLevel.Access == AclGroupRoleAccessEnum.BelowInOrg)
            flag1 = true;
          else if (!flag2 && groupScopedUsers.ContainsKey(key1))
          {
            Dictionary<int, List<UserInfo>> dictionary3 = groupScopedUsers[key1];
            if (dictionary3.ContainsKey(key2))
            {
              UserInfo[] array = dictionary3[key2].ToArray();
              set.AddAll((ICollection) array);
            }
          }
          if (!flag3 & flag2)
            break;
        }
        if (flag1 && usersBelowWithRoles.ContainsKey(key1))
        {
          UserInfo[] array = usersBelowWithRoles[key1].ToArray();
          set.AddAll((ICollection) array);
        }
        UserInfo[] userInfoArray = new UserInfo[set.Count];
        set.CopyTo((Array) userInfoArray, 0);
        if (flag3)
        {
          ArrayList arrayList = new ArrayList();
          foreach (UserInfo userInfo1 in userInfoArray)
          {
            if (userInfo1.Status != UserInfo.UserStatusEnum.Disabled)
              arrayList.Add((object) userInfo1);
          }
          userInfoArray = (UserInfo[]) arrayList.ToArray(typeof (UserInfo));
        }
        foreach (UserInfo userInfo2 in userInfoArray)
        {
          if (!dictionary1.ContainsKey(userInfo2.Userid))
            dictionary1.Add(userInfo2.Userid, userInfo2);
        }
      }
      UserInfo[] array1 = new UserInfo[dictionary1.Count];
      dictionary1.Values.CopyTo(array1, 0);
      return array1;
    }

    public static UserInfo GetUserById(string userId)
    {
      UserInfo[] usersFromSql = User.GetUsersFromSQL(EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
      if (usersFromSql == null)
        return (UserInfo) null;
      return usersFromSql.Length != 1 ? (UserInfo) null : usersFromSql[0];
    }

    public static UserInfo[] GetAllUsers(string userId)
    {
      return User.GetUsersFromSQL("select userid from users where user_type is NULL ");
    }

    public static UserInfo[] GetExternalTPOUsers()
    {
      return User.GetUsersFromSQL("Select contactID from ExternalUsers where (Roles & 2 = 2  or Roles & 1 = 1)");
    }

    public static UserInfo[] GetAllIntAndExtUsers(string userId)
    {
      return User.GetUsersFromSQL("select userid from users");
    }

    public static UserInfo[] GetUsers(string[] userIds)
    {
      return User.GetUsersFromSQL(EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) userIds));
    }

    public static UserInfo[] GetUsersByName(string firstName, string lastName)
    {
      return User.GetUsersByName(firstName, "", lastName, "");
    }

    public static UserInfo[] GetUsersByName(
      string firstName,
      string middleName,
      string lastName,
      string suffixName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select userid from users where ");
      if (firstName != "" && lastName != "")
        dbQueryBuilder.Append("(first_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(firstName) + "') and (last_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(lastName) + "')");
      else if (firstName != "")
        dbQueryBuilder.Append("first_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(firstName) + "'");
      else if (lastName != "")
        dbQueryBuilder.Append("last_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(lastName) + "'");
      if (middleName != "")
        dbQueryBuilder.Append(" and middle_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(middleName) + "'");
      if (suffixName != "")
        dbQueryBuilder.Append(" and suffix_name like '" + EllieMae.EMLite.DataAccess.SQL.Escape(suffixName) + "'");
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    [PgReady]
    public static string[] GetUserIDsInOrgForCCSite(int orgID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.Append("SELECT userid FROM USERS WHERE org_id  = " + (object) orgID + " and (inheritParentccsite = 1 or inheritParentccsite is null)");
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count == 0)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          stringList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["userid"]));
        return stringList?.ToArray();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("SELECT userid FROM USERS WHERE org_id  = " + (object) orgID + " and (inheritParentccsite = 1 or inheritParentccsite is null)");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0)
        return (string[]) null;
      List<string> stringList1 = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        stringList1.Add(row["userid"].ToString());
      return stringList1?.ToArray();
    }

    [PgReady]
    public static string[] GetUserIDsInOrgForLOComp(int orgID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.Append("SELECT userid FROM USERS WHERE org_id  = " + (object) orgID + " and COALESCE(inheritParentccsite,1) = 1 ");
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count == 0)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          stringList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(row["userid"]));
        return stringList?.ToArray();
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("SELECT userid FROM users WHERE org_id = " + (object) orgID + " AND [inheritParentCompPlan] = 'True'");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery(DbTransactionType.None);
      if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0)
        return (string[]) null;
      List<string> stringList1 = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        stringList1.Add(row["userid"].ToString());
      return stringList1?.ToArray();
    }

    public static UserInfo[] GetUsersInOrganization(int orgId)
    {
      return User.GetUsersFromSQL("select userid from users where org_id = " + (object) orgId);
    }

    public static UserInfo[] GetUsersUnderOrganization(int orgId)
    {
      return User.GetUsersFromSQL("select userid from users where org_id = " + (object) orgId + " or org_id in (Select descendent from org_descendents where oid = " + (object) orgId + ")");
    }

    public static HashSet<string> GetValidExternalUserId(HashSet<string> useridList)
    {
      string str = "'" + string.Join("','", (IEnumerable<string>) useridList) + "'";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT userid  FROM users WHERE LOWER(user_type) = 'external' AND userid in (" + str + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      HashSet<string> validExternalUserId = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      for (int index = 0; index < dataRowCollection.Count; ++index)
        validExternalUserId.Add((string) dataRowCollection[index]["userid"]);
      return validExternalUserId;
    }

    public static HashSet<string> GetValidUserIdsUnderOrganization(
      int orgId,
      HashSet<string> useridList)
    {
      string str = "'" + string.Join("','", (IEnumerable<string>) useridList) + "'";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT userid FROM users WHERE userid IN (" + str + ") ");
      dbQueryBuilder.AppendLine(string.Format("AND (org_id = {0} or org_id IN (SELECT descendent FROM org_descendents WHERE oid = {1}))", (object) orgId, (object) orgId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      HashSet<string> underOrganization = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      for (int index = 0; index < dataRowCollection.Count; ++index)
        underOrganization.Add((string) dataRowCollection[index]["userid"]);
      return underOrganization;
    }

    public static UserInfo[] GetOrganizationSSOUsers(int orgId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(User.getDescendentWithInheritParentSSOSql(orgId));
      dbQueryBuilder.AppendLine("SELECT d.oid FROM DescendentsWithInheritParentSSO d");
      dbQueryBuilder.AppendLine("OPTION(MAXRECURSION 100);");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] data = new int[dataRowCollection.Count];
      for (int index = 0; index < data.Length; ++index)
        data[index] = (int) dataRowCollection[index]["oid"];
      return User.GetUsersFromSQL("SELECT u.userid FROM users u INNER JOIN UserPersona up on u.userid = up.userid WHERE org_id IN (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) data) + ") AND apiUser = 0 AND u.ssoDisconnectedFromOrg = 0 AND u.userid != 'admin' AND u.userid != '(trusted) and up.personaID <> 0'");
    }

    public static UserInfo[] GetUsersUnderDescendentOrganization(int orgId)
    {
      return User.GetUsersFromSQL("select userid from users where org_id in (Select descendent from org_descendents where oid = " + (object) orgId + ")");
    }

    public static Dictionary<string, UserLoginInfo> GetUserLoginInfos(
      string[] userIds,
      string loginUserID,
      bool isTPOMVP = false)
    {
      UserInfo userById = User.GetUserById(loginUserID);
      Dictionary<string, UserInfo> dictionary = new Dictionary<string, UserInfo>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      UserInfo[] allAccessibleUsers = User.GetAllAccessibleUsers(userById);
      foreach (UserInfo userInfo in allAccessibleUsers)
        dictionary[userInfo.Userid] = userInfo;
      if (isTPOMVP)
      {
        List<string> stringList = new List<string>();
        foreach (UserInfo userInfo in allAccessibleUsers)
        {
          if (!stringList.Contains(userInfo.Userid))
            stringList.Add(userInfo.Userid);
        }
        foreach (UserInfo userInfo in User.GetAllAccessibleExtContactsLoginInfo(stringList.ToArray()))
          dictionary[userInfo.Userid] = userInfo;
      }
      Dictionary<string, UserLoginInfo> userLoginInfos = new Dictionary<string, UserLoginInfo>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      for (int index = 0; index < userIds.Length; ++index)
      {
        if (!userLoginInfos.ContainsKey(userIds[index]) && dictionary.ContainsKey(userIds[index]))
          userLoginInfos[userIds[index]] = new UserLoginInfo(dictionary[userIds[index]]);
      }
      return userLoginInfos;
    }

    public static UserInfo[] GetAeAccessibleContacts(string userIds)
    {
      return User.GetAllAccessibleExtContactsLoginInfo(new string[1]
      {
        userIds
      });
    }

    private static UserInfo[] GetAllAccessibleExtContactsLoginInfo(string[] userIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("IF OBJECT_ID('tempdb..#userids') IS NOT NULL");
      dbQueryBuilder.AppendLine("DROP TABLE #userids;");
      dbQueryBuilder.AppendLine("CREATE TABLE #userids (userid varchar(16) NOT NULL);");
      dbQueryBuilder.AppendLine("IF OBJECT_ID('tempdb..#tpocontactids') IS NOT NULL");
      dbQueryBuilder.AppendLine("DROP TABLE #tpocontactids;");
      dbQueryBuilder.AppendLine("CREATE TABLE #tpocontactids (tpocontactid varchar(16) NOT NULL);");
      int num = 0;
      for (int index = 0; index < userIds.Length; ++index)
      {
        dbQueryBuilder.AppendLine("INSERT INTO #userids ");
        for (; num < 1000 && index < userIds.Length - 1; ++index)
        {
          dbQueryBuilder.Append("select " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userIds[index]) + " union all ");
          ++num;
        }
        dbQueryBuilder.Append("select " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userIds[index]) + ";");
        num = 0;
      }
      dbQueryBuilder.AppendLine("CREATE CLUSTERED INDEX ix_userid ON #userids (userid);");
      dbQueryBuilder.Append("insert into #tpocontactids\r\n                         select distinct ae.TPOContactID\r\n                         from [AEAccessibleExternalUsers] ae\r\n                         inner join #userids u on ae.[AEID] = u.userid;");
      dbQueryBuilder.AppendLine("CREATE CLUSTERED INDEX ix_tpocontactid ON #tpocontactids (tpocontactid);");
      dbQueryBuilder.AppendLine("select u.*, o.depth\r\n                             from users u\r\n                             inner join org_chart o on u.org_id = o.oid\r\n                             inner join #tpocontactids t on u.userid = t.tpocontactid;");
      dbQueryBuilder.AppendLine("select *\r\n                             from UserPersona u\r\n                             inner join #tpocontactids t on u.userid = t.tpocontactid;");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      dataSet.Relations.Add("Personas", table1.Columns["userid"], table2.Columns["userid"]);
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      ArrayList arrayList = new ArrayList();
      foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
      {
        DataRow[] childRows = row.GetChildRows("Personas");
        arrayList.Add((object) User.databaseRowToUserInfo(row, User.databaseRowsToPersonas(childRows, personaLookup), ""));
      }
      return (UserInfo[]) arrayList.ToArray(typeof (UserInfo));
    }

    public static UserInfo[] GetAllAccessibleUsers(UserInfo user)
    {
      int[] sourceArray = OrganizationStore.GetDescendentsOfOrg(user.OrgId);
      if (user.UserPersonas != null)
      {
        foreach (Persona userPersona in user.UserPersonas)
        {
          if (userPersona.ID == 0 || 1 == userPersona.ID)
          {
            int[] destinationArray = new int[sourceArray.Length + 1];
            destinationArray[0] = user.OrgId;
            Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 1, sourceArray.Length);
            sourceArray = destinationArray;
            break;
          }
        }
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select userid from users where (userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) user.Userid) + ")");
      if (sourceArray != null && sourceArray.Length != 0)
        dbQueryBuilder.Append(" or (org_id in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) sourceArray) + "))");
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static UserInfo[] GetAllAccessibleUsers(User user)
    {
      int[] sourceArray = OrganizationStore.GetDescendentsOfOrg(user.UserInfo.OrgId);
      if (user.UserInfo.UserPersonas != null)
      {
        foreach (Persona userPersona in user.UserInfo.UserPersonas)
        {
          if (userPersona.ID == 0 || 1 == userPersona.ID)
          {
            int[] destinationArray = new int[sourceArray.Length + 1];
            destinationArray[0] = user.UserInfo.OrgId;
            Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 1, sourceArray.Length);
            sourceArray = destinationArray;
            break;
          }
        }
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select userid from users where (userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) user.UserID) + ")");
      if (sourceArray != null && sourceArray.Length != 0)
        dbQueryBuilder.Append(" or (org_id in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) sourceArray) + "))");
      return User.GetUsersFromSQL(dbQueryBuilder.ToString());
    }

    public static UserInfo[] GetAllAccessibleSalesRepUsers()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @userids table(userid varchar(16)) ");
      dbQueryBuilder.AppendLine("insert into @userids(userid) ");
      dbQueryBuilder.AppendLine("select userid from userpersona up inner join acl_features af on up.personaID = af.personaID ");
      dbQueryBuilder.AppendLine("where af.featureID = " + (object) Convert.ToInt32((object) AclFeature.ExternalSettings_ContactSalesRep));
      dbQueryBuilder.AppendLine(" and af.access = 1 and up.personaID != 0 and up.personaID != 1 ");
      dbQueryBuilder.AppendLine(" union ");
      dbQueryBuilder.AppendLine(" select userid from Acl_Features_User ");
      dbQueryBuilder.AppendLine(" where featureID = " + (object) Convert.ToInt32((object) AclFeature.ExternalSettings_ContactSalesRep) + " and access = 1 ");
      dbQueryBuilder.AppendLine(" except ");
      dbQueryBuilder.AppendLine(" select userid from Acl_Features_User ");
      dbQueryBuilder.AppendLine(" where featureID = " + (object) Convert.ToInt32((object) AclFeature.ExternalSettings_ContactSalesRep) + " and access = 0 ");
      dbQueryBuilder.AppendLine("select u.*, org_chart.depth from users u inner join @userids tmp on u.userid = tmp.userid ");
      dbQueryBuilder.AppendLine(" inner join org_chart on u.org_id = org_chart.oid ");
      dbQueryBuilder.AppendLine(" where u.status = 0 ");
      dbQueryBuilder.AppendLine("select up.userid, up.personaid ");
      dbQueryBuilder.AppendLine("from UserPersona up inner join @userids tmp on up.userid = tmp.userid");
      dbQueryBuilder.AppendLine(" inner join users u on up.userid = u.userid ");
      dbQueryBuilder.AppendLine(" where u.status = 0");
      return User.GetSalesRepUsersFromSQL(dbQueryBuilder.ExecuteSetQuery());
    }

    public static UserInfoSummary[] ConvertToUserInfoSummaries(UserInfo[] users)
    {
      UserInfoSummary[] userInfoSummaries = new UserInfoSummary[users.Length];
      for (int index = 0; index < users.Length; ++index)
        userInfoSummaries[index] = new UserInfoSummary(users[index]);
      return userInfoSummaries;
    }

    public static int GetEnabledUserCount()
    {
      return User.GetEnabledUserCount((IClientContext) ClientContext.GetCurrent());
    }

    [PgReady]
    public static int GetEnabledUserCount(IClientContext context)
    {
      if (context.Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder(context);
        pgDbQueryBuilder.Append("select count(*) from users where user_type is NULL and status = " + (object) 0);
        return EllieMae.EMLite.DataAccess.SQL.DecodeInt(pgDbQueryBuilder.ExecuteScalar(DbTransactionType.None));
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(context);
      dbQueryBuilder.Append("select count(*) from users where user_type is NULL and status = " + (object) 0);
      return (int) dbQueryBuilder.ExecuteScalar(DbTransactionType.None);
    }

    private static void deleteUserDirectory(string userId)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(ClientContext.GetCurrent().Settings.GetUserDataFolderPath(userId, false));
      if (!directoryInfo.Exists)
        return;
      directoryInfo.Delete(true);
    }

    public static bool IsVirtualUser(string userId)
    {
      return !string.IsNullOrEmpty(userId) && userId.StartsWith("<") && userId.EndsWith(">");
    }

    private static DirectoryInfo getUserDirectory(string userId)
    {
      return new DirectoryInfo(ClientContext.GetCurrent().Settings.GetUserDataFolderPath(userId));
    }

    private static DirectoryInfo getUserPipelineView(string userId)
    {
      DirectoryInfo userPipelineView = new DirectoryInfo(Path.Combine(User.getUserDirectory(userId).FullName, "TemplateSettings", "PipelineView"));
      if (!userPipelineView.Exists)
        userPipelineView.Create();
      return userPipelineView;
    }

    private static DirectoryInfo getUserCustomDataDirectory(string userId)
    {
      DirectoryInfo customDataDirectory = new DirectoryInfo(Path.Combine(User.getUserDirectory(userId).FullName, "CustomData"));
      if (!customDataDirectory.Exists)
        customDataDirectory.Create();
      return customDataDirectory;
    }

    private static DirectoryInfo getUserDashboardMapDirectory(string userId)
    {
      DirectoryInfo dashboardMapDirectory = new DirectoryInfo(Path.Combine(User.getUserDirectory(userId).FullName, "DashboardMap"));
      if (!dashboardMapDirectory.Exists)
        dashboardMapDirectory.Create();
      return dashboardMapDirectory;
    }

    private void initializeUser() => this.innerLock.CheckIn(User.fetchUser(this.userId), true);

    private void validateInstance() => this.validateInstance(true);

    private void validateInstance(bool requireExists)
    {
      if (this.innerLock == null)
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Attempt to access disposed User object"));
      if (!requireExists)
        return;
      this.validateExists();
    }

    private void validateExists()
    {
      if (this.Exists)
        return;
      Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Object does not exist"));
    }

    private bool validateUserID(string userID)
    {
      bool flag = false;
      if (userID.Length > 16)
        flag = true;
      else if (userID.IndexOf('.') == 0 || userID.EndsWith("."))
      {
        flag = true;
      }
      else
      {
        foreach (char c in userID)
        {
          if (!char.IsLetterOrDigit(c) && c != '_' && c != '@' && c != '-' && c != '.')
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private static BinaryObject readUserDataFile(string userId, string filename)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid user settings file path \"" + filename + "\""));
      try
      {
        DataFile latestVersion = FileStore.GetLatestVersion(Path.Combine(User.getUserDirectory(userId).FullName, filename));
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (BinaryObject) null;
      }
    }

    private static BinaryObject readUserDashboardMapFile(string userId, string filename)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid user settings file path \"" + filename + "\""));
      try
      {
        DataFile latestVersion = FileStore.GetLatestVersion(Path.Combine(User.getUserDashboardMapDirectory(userId).FullName, filename));
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (BinaryObject) null;
      }
    }

    private static void writeUserDataFile(string userId, string filename, BinaryObject data)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid user settings file path \"" + filename + "\""));
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(Path.Combine(User.getUserDirectory(userId).FullName, filename)))
          dataFile.CheckIn(data);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    private static void writeUserDashboardMapFile(
      string userId,
      string filename,
      BinaryObject data)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid user settings file path \"" + filename + "\""));
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(Path.Combine(User.getUserDashboardMapDirectory(userId).FullName, filename), MutexAccess.Write))
          dataFile.CheckIn(data);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    private static BinaryObject readCustomDataFile(string userId, string filename)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid data file name \"" + filename + "\""));
      try
      {
        DataFile latestVersion = FileStore.GetLatestVersion(Path.Combine(User.getUserCustomDataDirectory(userId).FullName, filename));
        return !latestVersion.Exists ? (BinaryObject) null : latestVersion.GetData();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (BinaryObject) null;
      }
    }

    private static void writeCustomDataFile(string userId, string filename, BinaryObject data)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid data file name \"" + filename + "\""));
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(Path.Combine(User.getUserCustomDataDirectory(userId).FullName, filename), MutexAccess.Write))
        {
          if (data == null)
            dataFile.Delete();
          else
            dataFile.CheckIn(data);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    private static void appendToCustomDataFile(string userId, string filename, BinaryObject data)
    {
      if (!DataFile.IsValidFilename(filename))
        Err.Raise(TraceLevel.Error, nameof (User), new ServerException("Invalid data file name \"" + filename + "\""));
      try
      {
        using (DataFile dataFile = FileStore.CheckOut(Path.Combine(User.getUserCustomDataDirectory(userId).FullName, filename), MutexAccess.Write))
        {
          if (!dataFile.Exists)
            dataFile.CheckIn(data);
          else
            dataFile.Append(data, false);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    private static void validateFreeUserLicense()
    {
      int enabledUserCount = User.GetEnabledUserCount();
      LicenseInfo serverLicense = Company.GetServerLicense();
      User.logUserLimitMessageAccordingToThreshold(enabledUserCount, serverLicense);
      if (serverLicense.UserLimit <= 0 || enabledUserCount < serverLicense.UserLimitWithFlex)
        return;
      Err.Raise(TraceLevel.Warning, nameof (User), (ServerException) new LicenseException(serverLicense, LicenseExceptionType.LicensesExceeded, "Number of enabled users is already at license limit"));
    }

    private static void logUserLimitMessageAccordingToThreshold(int userCount, LicenseInfo license)
    {
      if (license.UserLimit <= 0)
        return;
      ClientContext current = ClientContext.GetCurrent();
      string className = string.Format("{0}|{1}", (object) current.InstanceName, (object) current.ClientID);
      if (userCount >= license.UserLimitWithFlex)
        TraceLog.WriteWarning(className, "License hard stop message displayed.");
      else if (userCount > 5 && userCount <= 14)
      {
        if (license.UserLimit - userCount == 2)
        {
          TraceLog.WriteWarning(className, "License warning message displayed.");
        }
        else
        {
          if (license.UserLimit - userCount != 1)
            return;
          TraceLog.WriteWarning(className, "License hard stop message displayed.");
        }
      }
      else if (userCount > 14 && userCount <= 999)
      {
        if (license.UserLimit - userCount == 1)
        {
          TraceLog.WriteWarning(className, "License hard stop message displayed.");
        }
        else
        {
          if (userCount < license.UserLimitWith90Percent)
            return;
          TraceLog.WriteWarning(className, "License warning message displayed.");
        }
      }
      else
      {
        if (userCount <= 999)
          return;
        if (license.UserLimit - userCount == 1)
        {
          TraceLog.WriteWarning(className, "License hard stop message displayed.");
        }
        else
        {
          if (license.UserLimit - userCount > 100)
            return;
          TraceLog.WriteWarning(className, "License warning message displayed.");
        }
      }
    }

    private static void createUserInDatabase(UserInfo info, UserInfo loggedInUser)
    {
      if (info.Userid == "(trusted)")
        Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot create user account with ID '(trusted)'"));
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("if not exists(select 1 from org_chart where oid = " + (object) info.OrgId + ")");
        dbQueryBuilder.RaiseError("Invalid organization ID");
        dbQueryBuilder.AppendLine("else\n");
        dbQueryBuilder.AppendLine("begin\n");
        User.SetMetaDataDetails(info, loggedInUser.Userid, true, false);
        DbValueList dbValueList = User.createDbValueList(info, true);
        dbValueList.Add("userid", (object) info.Userid);
        dbValueList.Add("org_id", (object) info.OrgId);
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("users"), dbValueList, true, false);
        dbQueryBuilder.AppendLine("end");
        dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
        if (!string.IsNullOrEmpty(info.Password))
          User.writePasswordToUserCredentials(info.Userid, info.Password);
        User.updateDbPersonas(info.Userid, info.UserPersonas);
        User.PublishUserKafkaEvent((IEnumerable<string>) new List<string>()
        {
          info.Userid
        }, UserWebhookMessage.UserType.InternalUsers, "create", loggedInUser);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    private static void SetMetaDataDetails(
      UserInfo userInfo,
      string loggedInUserId,
      bool isCreate,
      bool loginFailed)
    {
      if (isCreate)
      {
        DateTime utcNow = DateTime.UtcNow;
        userInfo.createdDate = new DateTime?(utcNow);
        userInfo.createdBy = loggedInUserId;
        userInfo.lastModifiedDate = new DateTime?(utcNow);
        userInfo.lastModifiedBy = loggedInUserId;
      }
      else
      {
        userInfo.lastModifiedDate = !loginFailed || !userInfo.Locked ? (!loginFailed ? new DateTime?(DateTime.UtcNow) : userInfo.lastModifiedDate) : new DateTime?(DateTime.UtcNow);
        userInfo.lastModifiedBy = !loginFailed || userInfo.Locked ? loggedInUserId : userInfo.lastModifiedBy;
      }
    }

    private static DbValueList createDbValueList(UserInfo info, bool isCreate)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("last_name", (object) info.LastName);
      dbValueList.Add("suffix_name", (object) info.SuffixName);
      dbValueList.Add("first_name", (object) info.FirstName);
      dbValueList.Add("middle_name", (object) info.MiddleName);
      dbValueList.Add("employee_id", (object) info.EmployeeID);
      dbValueList.Add("email", (object) info.Email);
      dbValueList.Add("phone", (object) info.Phone);
      dbValueList.Add("cell_phone", (object) info.CellPhone);
      dbValueList.Add("fax", (object) info.Fax);
      dbValueList.Add("working_folder", (object) info.WorkingFolder);
      dbValueList.Add("access_mode", (object) (int) info.AccessMode);
      dbValueList.Add("status", (object) (int) info.Status);
      dbValueList.Add("no_pwd_expiration", (object) info.PasswordNeverExpires, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("require_pwd_change", (object) info.RequirePasswordChange, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("locked", (object) info.Locked, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("peerView", (object) (int) info.PeerView);
      dbValueList.Add("data_services_opt", (object) info.DataServicesOptOutKey);
      dbValueList.Add("chumid", (object) info.CHUMId);
      dbValueList.Add("nmlsOriginatorID", (object) info.NMLSOriginatorID);
      dbValueList.Add("jobtitle", (object) info.JobTitle);
      if (info.NMLSExpirationDate == DateTime.MaxValue)
        dbValueList.Add("nmlsExpirationDate", (object) null);
      else
        dbValueList.Add("nmlsExpirationDate", (object) info.NMLSExpirationDate, (IDbEncoder) DbEncoding.DateTime);
      if (!string.IsNullOrEmpty(info.Password))
        dbValueList.Add("password", (object) ("PwdEncrypt(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) User.getCaseSensitivePassword(info.Password)) + ")"), (IDbEncoder) DbEncoding.None);
      if (info.LastLogin != DateTime.MinValue)
        dbValueList.Add("last_login", (object) info.LastLogin);
      if ((info.EncompassVersion ?? "") != "")
        dbValueList.Add("enc_version", (object) info.EncompassVersion);
      dbValueList.Add("emailSignature", (object) info.EmailSignature);
      dbValueList.Add("profileURL", (object) info.ProfileURL);
      dbValueList.Add("personalStatusOnline", (object) info.PersonalStatusOnline, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("inheritParentCompPlan", (object) info.InheritParentCompPlan, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("apiUser", (object) info.ApiUser, (IDbEncoder) DbEncoding.Flag);
      if (info.ApiUser)
      {
        dbValueList.Add("oAuthClientId", (object) info.OAuthClientId);
        dbValueList.Add("allowImpersonation", (object) info.AllowImpersonation, (IDbEncoder) DbEncoding.Flag);
      }
      else
      {
        dbValueList.Add("oAuthClientId", (object) null);
        dbValueList.Add("allowImpersonation", (object) false, (IDbEncoder) DbEncoding.Flag);
      }
      dbValueList.Add("inheritParentCCSite", (object) info.InheritParentCCSite, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("personaAccessComments", (object) info.PersonaAccessComments);
      dbValueList.Add("lastLockedOutDateTime", (object) info.LastLockedOutDateTime);
      dbValueList.Add("ssoOnly", (object) info.SSOOnly, (IDbEncoder) DbEncoding.Flag);
      dbValueList.Add("ssoDisconnectedFromOrg", (object) info.SSODisconnectedFromOrg, (IDbEncoder) DbEncoding.Flag);
      if (isCreate)
      {
        dbValueList.Add("createdDate", (object) info.createdDate);
        dbValueList.Add("createdBy", (object) info.createdBy);
        dbValueList.Add("lastModifiedDate", (object) info.lastModifiedDate);
        dbValueList.Add("lastModifiedBy", (object) info.lastModifiedBy);
      }
      else
      {
        dbValueList.Add("lastModifiedDate", (object) info.lastModifiedDate);
        dbValueList.Add("lastModifiedBy", (object) info.lastModifiedBy);
      }
      return dbValueList;
    }

    private static User.PasswordHistory getPasswordHistory(string userId, string password)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select date_changed as Dates, PwdCompare(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) User.getCaseSensitivePassword(password)) + ", password, 0) as PwdComp1, ");
        dbQueryBuilder.AppendLine("PwdCompare(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) password) + ", password, 0) as PwdComp2, password, isNewHashed from password_history where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " order by Dates DESC");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          return new User.PasswordHistory((object) null, 0);
        PasswordEncryptor passwordEncryptor = new PasswordEncryptor();
        int passwordSince = 1;
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          DataRow dataRow = dataRowCollection[index];
          DateTime lastDate = EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRow["Dates"]);
          int num1 = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PwdComp1"], 0);
          int num2 = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRow["PwdComp2"], 0);
          byte[] hashedPassword = (byte[]) dataRow[nameof (password)];
          bool flag = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isNewHashed"], false);
          if (!flag && (num1 == 1 || num2 == 1) || flag && passwordEncryptor.Compare(password, hashedPassword))
            return new User.PasswordHistory((object) lastDate, passwordSince);
          ++passwordSince;
        }
        return new User.PasswordHistory((object) null, 0);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (User.PasswordHistory) null;
      }
    }

    [PgReady]
    private static bool compareUserPassword(string userId, string password)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        try
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) current);
          pgDbQueryBuilder.AppendLine("SELECT -1 AS PwdResult, password AS HashedPassword FROM [UserCredentials] WHERE userid = @userid");
          DbCommandParameter parameter = new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.String);
          DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(parameter);
          PasswordEncryptor passwordEncryptor = new PasswordEncryptor();
          return dataRowCollection.Count > 0 && passwordEncryptor.Compare(password, (byte[]) dataRowCollection[0]["HashedPassword"]);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
          return false;
        }
      }
      else
      {
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("declare @password varbinary(255)");
          dbQueryBuilder.AppendLine("declare @check1 int");
          dbQueryBuilder.AppendLine("declare @check2 int");
          dbQueryBuilder.AppendLine("select @password = password from [UserCredentials] where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
          dbQueryBuilder.AppendLine("if (@password is NULL)");
          dbQueryBuilder.AppendLine("begin");
          dbQueryBuilder.AppendLine("    select @check1 = PwdCompare(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) User.getCaseSensitivePassword(password)) + ", password, 0) from [Users] where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
          dbQueryBuilder.AppendLine("    select @check2 = PwdCompare(" + EllieMae.EMLite.DataAccess.SQL.Encode((object) password) + ", password, 0) from [Users] where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
          dbQueryBuilder.AppendLine("    if (@check1 = 1 OR @check2 = 1)");
          dbQueryBuilder.AppendLine("        select 1 as PwdResult");
          dbQueryBuilder.AppendLine("    else");
          dbQueryBuilder.AppendLine("        select 0 as PwdResult");
          dbQueryBuilder.AppendLine("end");
          dbQueryBuilder.AppendLine("else");
          dbQueryBuilder.AppendLine("    select -1 as PwdResult, @password as HashedPassword");
          DataRow dataRow = dbQueryBuilder.ExecuteRowQuery();
          int num = (int) dataRow["PwdResult"];
          if (num >= 0)
          {
            if (num == 0)
              return false;
            User.writePasswordToUserCredentials(userId, password);
            return true;
          }
          PasswordEncryptor passwordEncryptor = new PasswordEncryptor();
          byte[] hashedPassword = (byte[]) dataRow["HashedPassword"];
          return passwordEncryptor.Compare(password, hashedPassword);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
          return false;
        }
      }
    }

    [PgReady]
    private static void writePasswordToUserCredentials(string userId, string password)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        try
        {
          PgDbQueryBuilder idbqb = new PgDbQueryBuilder();
          byte[] numArray = new PasswordEncryptor().Hash(password);
          DbValue pkColumnValue = new DbValue("userid", (object) "@userid", (IDbEncoder) DbEncoding.None);
          DbValue nonPkColumnValues = new DbValue(nameof (password), (object) "@passwordHash", (IDbEncoder) DbEncoding.None);
          PgQueryHelpers.Upsert((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, DbConstraint.None, "UserCredentials", pkColumnValue, nonPkColumnValues);
          DbCommandParameter[] parameters = new DbCommandParameter[2]
          {
            new DbCommandParameter("passwordHash", (object) numArray, DbType.Binary),
            new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.AnsiString)
          };
          idbqb.ExecuteNonQuery(parameters);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
        }
      }
      else
      {
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          byte[] numArray = new PasswordEncryptor().Hash(password);
          dbQueryBuilder.AppendLine("if exists (select * from [UserCredentials] where userid =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + ")");
          dbQueryBuilder.AppendLine("    update [UserCredentials] set password = @passwordHash where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
          dbQueryBuilder.AppendLine("else");
          dbQueryBuilder.AppendLine("    insert into [UserCredentials] values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + ", @passwordHash)");
          DbCommandParameter[] parameters = new DbCommandParameter[1]
          {
            new DbCommandParameter("@passwordHash", (object) numArray, DbType.Binary)
          };
          dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Default, parameters);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
        }
      }
    }

    private static void updateUserOrganization(string userId, int orgId, bool uncheckParentInfo)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("if not exists(select 1 from org_chart where oid = " + (object) orgId + ")");
        dbQueryBuilder.AppendLine("    raiserror('Invalid organization ID', 16, 1)");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    update users set org_id = " + (object) orgId + (uncheckParentInfo ? (object) ", inheritParentCompPlan = 'False'" : (object) "") + " where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    private static string[] UpdateUsersSSOSettingDb(int orgId, bool loginAccess, bool applyToAll)
    {
      int num = loginAccess ? 1 : 0;
      string[] strArray = new string[0];
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("DECLARE @userIdTbl TABLE(userid varchar(16));");
        dbQueryBuilder.AppendLine("DECLARE @orgIdTbl TABLE(oid int, parent int);");
        dbQueryBuilder.AppendLine(User.getDescendentWithInheritParentSSOSql(orgId));
        dbQueryBuilder.AppendLine("INSERT INTO @orgIdTbl SELECT d.* FROM DescendentsWithInheritParentSSO d");
        dbQueryBuilder.AppendLine("OPTION(MAXRECURSION 100);");
        dbQueryBuilder.AppendLine("--update sso in db");
        dbQueryBuilder.AppendLine("UPDATE u set ssoOnly = " + (object) num);
        dbQueryBuilder.AppendLine("OUTPUT inserted.userid into @userIdTbl");
        dbQueryBuilder.AppendLine("FROM Users u");
        dbQueryBuilder.AppendLine("INNER JOIN UserPersona up ON u.userid = up.userid");
        dbQueryBuilder.AppendLine("INNER JOIN @orgIdTbl ot ON u.org_id = ot.oid");
        dbQueryBuilder.AppendLine("WHERE apiUser = 0 ");
        if (!applyToAll)
          dbQueryBuilder.AppendLine("AND ssoDisconnectedFromOrg = 0");
        dbQueryBuilder.AppendLine("AND u.userid != 'admin' AND u.userid != '(trusted)' and up.personaID <> 0");
        dbQueryBuilder.AppendLine("SELECT * FROM @userIdTbl");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
        if (dataSet != null)
          strArray = dataSet.Tables[0].Rows.Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (row => row["userid"].ToString())).ToArray<string>();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
      return strArray;
    }

    private static void UpdateUsersOrganization(
      string[] userIds,
      int orgId,
      bool uncheckParentInfo,
      bool loginAccess,
      bool applyToAll,
      bool enableSSO,
      string[] connectedUsers,
      string currentUserId)
    {
      bool flag = enableSSO && connectedUsers != null && connectedUsers.Length >= 1;
      try
      {
        if (userIds == null || userIds.Length == 0)
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("if not exists(select 1 from org_chart where oid = " + (object) orgId + ")");
        dbQueryBuilder.AppendLine("    raiserror('Invalid organization ID', 16, 1)");
        dbQueryBuilder.AppendLine("else");
        dbQueryBuilder.AppendLine("    update users set org_id = " + (object) orgId + (uncheckParentInfo ? (object) ", inheritParentCompPlan = 'False'" : (object) "") + ", LastModifiedDate = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) DateTime.UtcNow) + ", LastModifiedBy = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) currentUserId) + " where userid in (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userIds) + ")");
        if (flag)
        {
          dbQueryBuilder.AppendLine("    update users set ssoOnly = " + (loginAccess ? "1" : "0") + " where apiUser=0 and userid IN ( " + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) connectedUsers) + ")");
          if (!applyToAll)
            dbQueryBuilder.AppendLine(" And ssoDisconnectedFromOrg = 0");
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    public static void MoveUsersIntoOrganization(
      string[] userIds,
      int orgId,
      string traceHeader,
      bool loginAccess,
      bool enableSSO,
      List<string> coonectedUsers,
      string currentUserId)
    {
      using (ClientContext.GetCurrent().Cache.Lock(Organization.SyncRootKey))
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (User), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
          foreach (string userId in userIds)
            UserStore.RemoveCache(userId);
          if (userIds.Length != 0)
            User.UpdateUsersOrganization(userIds, orgId, true, loginAccess, false, enableSSO, coonectedUsers?.ToArray(), currentUserId);
          Task.Factory.StartNew((Action) (() =>
          {
            try
            {
              foreach (string userId in userIds)
                UserStore.RemoveCache(userId);
            }
            catch (Exception ex)
            {
              Err.Reraise(nameof (User), ex);
            }
          }));
          for (int index = 0; index < userIds.Length; ++index)
          {
            string userId = userIds[index];
            using (User user = UserStore.CheckOut(userId?.ToLower()))
            {
              if (!user.Exists)
                Err.Raise(TraceLevel.Warning, nameof (User), (ServerException) new ObjectNotFoundException("Invalid user ID", ObjectType.User, (object) userId));
              user.ChangeOrganization(latestVersion.OrganizationID, userIds.Length <= 1);
              TraceLog.WriteInfo(nameof (User), traceHeader + "Moved user \"" + userId + "\" into organization \"" + latestVersion.Name + "\" (" + (object) latestVersion.OrganizationID + ")");
            }
          }
        }
      }
    }

    public static void UpdateUsersSSOSetting(int orgId, bool loginAccess, bool applyToAll)
    {
      using (ClientContext.GetCurrent().Cache.Lock(Organization.SyncRootKey))
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (User), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
          foreach (string userId in User.UpdateUsersSSOSettingDb(latestVersion.OrganizationID, loginAccess, applyToAll))
          {
            TraceLog.WriteInfo(nameof (User), "Apply SSO Settings to user \"" + userId + "\" of organization \"" + latestVersion.Name + "\" (" + (object) latestVersion.OrganizationID + ")");
            UserStore.RemoveCache(userId);
          }
        }
      }
    }

    public static void UpdateUserCompPlansParentInfo(
      string userId,
      bool useParentInfo,
      string loggedInUserId)
    {
      TraceLog.WriteInfo(nameof (User), "User.UpdateUserCompPlansParentInfo: Creating SQL query for table Users.");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      try
      {
        int num = useParentInfo ? 1 : 0;
        DbTableInfo table = DbAccessManager.GetTable("users");
        DbValueList values = new DbValueList()
        {
          new DbValue("inheritParentCompPlan", (object) num),
          new DbValue("LastModifiedDate", (object) DateTime.UtcNow),
          new DbValue("LastModifiedBy", (object) loggedInUserId)
        };
        DbValue key = new DbValue("userid", (object) userId);
        dbQueryBuilder.Update(table, values, key);
        dbQueryBuilder.ExecuteNonQuery();
        TraceLog.WriteInfo(nameof (User), "User.UpdateUserCompPlansParentInfo: Update successful. Removing user " + userId + " cache.");
        UserStore.RemoveCache(userId);
        TraceLog.WriteInfo(nameof (User), "User.UpdateUserCompPlansParentInfo: Removed user " + userId + " cache.");
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (User), ex);
        throw new Exception("User.UpdateUserCompPlansParentInfo: Cannot update the User compensation plans use parent information due to the following issue:\r\n" + ex.Message);
      }
      finally
      {
        dbQueryBuilder.Reset();
      }
    }

    [PgReady]
    private static void updateUser(
      UserEntry data,
      UserEntry priorData,
      string loggedInUserId,
      bool loginFailed)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        if (data.UserInfo.Userid == "(trusted)")
          Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot update trusted user account"));
        try
        {
          UserInfo userInfo = data.UserInfo;
          UserServerInfo serverInfo = data.ServerInfo;
          PgDbQueryBuilder idbqb = new PgDbQueryBuilder((IClientContext) current);
          if (userInfo.Password != null)
          {
            int serverSetting1 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.HistorySize");
            int serverSetting2 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.DaysToReuse");
            int serverSetting3 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.Lifetime");
            if (!User.compareUserPassword(userInfo.Userid, userInfo.Password))
            {
              bool requirePasswordChange = priorData.UserInfo.RequirePasswordChange;
              bool flag1 = !priorData.UserInfo.PasswordNeverExpires && serverSetting3 > 0 && priorData.ServerInfo.LastPasswordChangedDate.AddDays((double) serverSetting3).Date <= DateTime.Today;
              bool flag2 = !(requirePasswordChange | flag1);
              string str = string.IsNullOrEmpty(User.currentLoginUserID) ? userInfo.Userid : User.currentLoginUserID;
              List<string> values1 = new List<string>();
              if (flag2)
                values1.Add("Current");
              if (requirePasswordChange)
                values1.Add("Forced");
              if (flag1)
                values1.Add("Expired");
              DbValueList values2 = new DbValueList()
              {
                new DbValue("UserID", (object) str),
                new DbValue("UserFullName", (object) userInfo.FullName),
                new DbValue("ActionType", (object) 11),
                new DbValue("DTTMStamp", (object) DateTime.Now),
                new DbValue("UserAccountID", (object) "v_userid", (IDbEncoder) DbEncoding.None),
                new DbValue("UserAccountName", (object) userInfo.FullName),
                new DbValue("ObjectType", (object) 1),
                new DbValue("PriorStatus", (object) string.Join(", ", (IEnumerable<string>) values1))
              };
              List<DbVariable> variables = new List<DbVariable>()
              {
                new DbVariable("v_userid", DbType.AnsiString, "@userid"),
                new DbVariable("v_seqnum", DbType.Int32)
              };
              VariableScope variableScope = new VariableScope((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, (IList<DbVariable>) variables);
              variableScope.EmitOpenScope();
              idbqb.AppendLine("   select coalesce(max(seqnum), 0) + 1 into v_seqnum from password_history where userid = v_userid;");
              idbqb.AppendLine("   insert into password_history (userid, seqnum, password, isNewHashed)");
              idbqb.AppendLine("       select u.userid, v_seqnum, uc.password, 1 from users as u, usercredentials as uc where u.userid = v_userid and uc.userid = v_userid;");
              idbqb.AppendLine("   delete from password_history where (userid = v_userid) and (seqnum <= (v_seqnum - " + (object) serverSetting1 + ")) and (date_changed < DateAdd('day', -1 * " + (object) serverSetting2 + ", GetDate()));");
              idbqb.InsertInto(DbAccessManager.GetTable("SysAT_UserPwdChanged"), values2, true, false);
              variableScope.EmitCloseScope();
            }
          }
          userInfo.createdDate = priorData.UserInfo.createdDate;
          userInfo.createdBy = priorData.UserInfo.createdBy;
          User.SetMetaDataDetails(userInfo, loggedInUserId, false, false);
          DbValueList dbValueList = User.createDbValueList(userInfo, false);
          dbValueList.Add("failed_login_attempts", (object) serverInfo.FailedLoginAttempts);
          if (userInfo.Password != null)
          {
            dbValueList.Add("password_changed", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
            User.writePasswordToUserCredentials(userInfo.Userid, userInfo.Password);
          }
          DbTableInfo table = DbAccessManager.GetTable("users");
          DbValue key = new DbValue("userid", (object) "@userId", (IDbEncoder) DbEncoding.None);
          idbqb.Update(table, dbValueList, key);
          idbqb.SelectFrom(table, new string[1]
          {
            "password_changed"
          }, key);
          DbCommandParameter parameter = new DbCommandParameter("userid", (object) userInfo.Userid.TrimEnd(), DbType.String);
          DataRowCollection source = idbqb.Execute(parameter);
          DateTime dateTime = source == null || !source.Cast<DataRow>().Any<DataRow>() || source[0].ItemArray.Length == 0 ? DateTime.MinValue : EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(source[0][0], DateTime.MinValue);
          data.ServerInfo.LastPasswordChangedDate = dateTime;
          User.updateDbPersonas(userInfo.Userid, userInfo.UserPersonas);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
        }
      }
      else
      {
        if (data.UserInfo.Userid == "(trusted)")
          Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot update trusted user account"));
        try
        {
          UserInfo userInfo = data.UserInfo;
          UserServerInfo serverInfo = data.ServerInfo;
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine(User.GetPasswordUpdateHistoryQuery(priorData, userInfo));
          userInfo.createdDate = priorData.UserInfo.createdDate;
          userInfo.createdBy = priorData.UserInfo.createdBy;
          User.SetMetaDataDetails(userInfo, loggedInUserId, false, false);
          DbValueList dbValueList = User.createDbValueList(userInfo, false);
          dbValueList.Add("failed_login_attempts", (object) serverInfo.FailedLoginAttempts);
          if (userInfo.Password != null)
          {
            dbValueList.Add("password_changed", (object) "(case @isCurrentPassword when 1 then password_changed else GetDate() end)", (IDbEncoder) DbEncoding.None);
            User.writePasswordToUserCredentials(userInfo.Userid, userInfo.Password);
          }
          DbTableInfo table = DbAccessManager.GetTable("users");
          DbValue key = new DbValue("userid", (object) "@userId", (IDbEncoder) DbEncoding.None);
          dbQueryBuilder.Update(table, dbValueList, key);
          dbQueryBuilder.SelectFrom(table, new string[1]
          {
            "password_changed"
          }, key);
          data.ServerInfo.LastPasswordChangedDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(dbQueryBuilder.ExecuteScalar(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout), (object) DateTime.MinValue);
          User.updateDbPersonas(userInfo.Userid, userInfo.UserPersonas);
          User.PublishUserKafkaEvent((IEnumerable<string>) new List<string>()
          {
            userInfo.Userid
          }, UserWebhookMessage.UserType.InternalUsers, "update", data.UserInfo);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
        }
      }
    }

    private static string getCaseSensitivePassword(string password)
    {
      int length = password.Length;
      password += "\\~@\\";
      for (int index = 0; index < length; ++index)
        password += char.IsLower(password[index]) ? "0" : "1";
      return password;
    }

    public static UserEntry LoadUserEntry(string userId) => User.fetchUser(userId);

    [PgReady]
    private static UserEntry fetchUser(string userId)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        if (userId == "(trusted)")
          return User.getTrustedUserEntry();
        if (User.IsVirtualUser(userId))
          return User.getVirtualUserEntry(userId);
        try
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder((IClientContext) current);
          pgDbQueryBuilder.AppendLine("select users.*, org_chart.depth, org_chart.org_name from users inner join org_chart on users.org_id = org_chart.oid where users.userid = @userid");
          DbCommandParameter parameter = new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.String);
          DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.None, parameter);
          if (dataRowCollection.Count == 0)
            return (UserEntry) null;
          Persona[] listForUserFromDb = PersonaAccessor.GetPersonaListForUserFromDB(userId);
          string companySetting;
          try
          {
            companySetting = Company.GetCompanySetting((IClientContext) current, "CLIENT", "CLIENTID");
          }
          catch (Exception ex)
          {
            throw new Exception("Cannot get client ID from database.\r\n" + ex.Message);
          }
          if (string.IsNullOrEmpty(companySetting))
            throw new Exception("Error getting client ID.");
          return User.databaseRowToUserEntry(dataRowCollection[0], listForUserFromDb, companySetting);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
          return (UserEntry) null;
        }
      }
      else
      {
        if (userId == "(trusted)")
          return User.getTrustedUserEntry();
        if (User.IsVirtualUser(userId))
          return User.getVirtualUserEntry(userId);
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
          dbQueryBuilder.AppendLine("select users.*, org_chart.depth, org_chart.org_name from users inner join org_chart on users.org_id = org_chart.oid where users.userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
          DataTable table = dbQueryBuilder.ExecuteSetQuery(DbTransactionType.None).Tables[0];
          if (table.Rows.Count == 0)
            return (UserEntry) null;
          Persona[] listForUserFromDb = PersonaAccessor.GetPersonaListForUserFromDB(userId);
          string companySetting;
          try
          {
            companySetting = Company.GetCompanySetting((IClientContext) current, "CLIENT", "CLIENTID");
          }
          catch (Exception ex)
          {
            throw new Exception("Cannot get client ID from database.\r\n" + ex.Message);
          }
          if (string.IsNullOrEmpty(companySetting))
            throw new Exception("Error getting client ID.");
          return User.databaseRowToUserEntry(table.Rows[0], listForUserFromDb, companySetting);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), ex);
          return (UserEntry) null;
        }
      }
    }

    internal static UserInfo[] GetSalesRepUsersFromSQL(DataSet data)
    {
      try
      {
        DataTable table1 = data.Tables[0];
        DataTable table2 = data.Tables[1];
        data.Relations.Add("Personas", table1.Columns["userid"], table2.Columns["userid"]);
        Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
        ArrayList arrayList1 = new ArrayList();
        foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
        {
          row["userid"].ToString();
          DataRow[] childRows = row.GetChildRows("Personas");
          ArrayList arrayList2 = new ArrayList();
          for (int index = 0; index < childRows.Length; ++index)
          {
            int key = (int) childRows[index]["personaID"];
            if (personaLookup.Contains((object) key) && !arrayList2.Contains(personaLookup[(object) key]))
              arrayList2.Add(personaLookup[(object) key]);
          }
          Persona[] array = (Persona[]) arrayList2.ToArray(typeof (Persona));
          if (childRows.Length != 0)
            arrayList1.Add((object) User.databaseRowToUserInfo(row, array, ""));
        }
        return (UserInfo[]) arrayList1.ToArray(typeof (UserInfo));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return (UserInfo[]) null;
      }
    }

    public static UserInfo[] GetPaginatedRecords(
      UserInfo user,
      string groupId,
      int[] roleIds,
      string personaId,
      string featureId,
      string scope,
      string orgId,
      string userName,
      string specificUserId,
      int offset,
      int recordCount,
      out int totalRecords,
      string fullName = null,
      string userType = "Internal�",
      bool includeDisabled = true)
    {
      return User.GetPaginatedRecords(user, groupId, roleIds, personaId, featureId, scope, orgId, userName, specificUserId, offset, recordCount, out totalRecords, out List<Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>> _, fullName, userType, includeDisabled: includeDisabled);
    }

    public static UserInfo[] GetPaginatedRecords(
      UserInfo user,
      string groupId,
      int[] roleIds,
      string personaId,
      string featureId,
      string scope,
      string orgId,
      string userName,
      string specificUserId,
      int offset,
      int recordCount,
      out int totalRecords,
      out List<Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>> userDetails,
      string fullName = null,
      string userType = "Internal�",
      bool populateCCsiteInfo = false,
      bool populateLicenseInfo = false,
      bool populateUserGroups = false,
      bool populateCompPlans = false,
      bool includeDisabled = true)
    {
      totalRecords = 0;
      userDetails = new List<Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>>();
      if (user == (UserInfo) null)
        return (UserInfo[]) null;
      int num = offset + recordCount - 1;
      if (string.IsNullOrEmpty(scope) || !string.IsNullOrEmpty(scope) && scope.Equals("group", StringComparison.OrdinalIgnoreCase))
      {
        SqlCommand sqlCmd = new SqlCommand();
        sqlCmd.CommandText = "GetUsers";
        sqlCmd.CommandType = CommandType.StoredProcedure;
        DbAccessManager dbAccessManager = new DbAccessManager();
        SqlParameterCollection parameters1 = sqlCmd.Parameters;
        SqlParameter sqlParameter1 = new SqlParameter();
        sqlParameter1.ParameterName = "UserId";
        sqlParameter1.Value = user.IsVirtualUser() ? (object) "admin" : (object) user.Userid;
        parameters1.Add(sqlParameter1);
        if (!string.IsNullOrEmpty(groupId))
        {
          SqlParameterCollection parameters2 = sqlCmd.Parameters;
          SqlParameter sqlParameter2 = new SqlParameter();
          sqlParameter2.ParameterName = "GroupId";
          sqlParameter2.Value = (object) groupId;
          parameters2.Add(sqlParameter2);
        }
        if (roleIds != null && roleIds.Length != 0)
        {
          SqlParameterCollection parameters3 = sqlCmd.Parameters;
          SqlParameter sqlParameter3 = new SqlParameter();
          sqlParameter3.ParameterName = "RoleId";
          sqlParameter3.Value = (object) roleIds[0];
          parameters3.Add(sqlParameter3);
        }
        if (!string.IsNullOrEmpty(personaId))
        {
          SqlParameterCollection parameters4 = sqlCmd.Parameters;
          SqlParameter sqlParameter4 = new SqlParameter();
          sqlParameter4.ParameterName = "PersonaId";
          sqlParameter4.Value = (object) personaId;
          parameters4.Add(sqlParameter4);
        }
        if (!string.IsNullOrEmpty(featureId))
        {
          SqlParameterCollection parameters5 = sqlCmd.Parameters;
          SqlParameter sqlParameter5 = new SqlParameter();
          sqlParameter5.ParameterName = "FeatureId";
          sqlParameter5.Value = (object) featureId;
          parameters5.Add(sqlParameter5);
        }
        if (!string.IsNullOrEmpty(orgId))
        {
          SqlParameterCollection parameters6 = sqlCmd.Parameters;
          SqlParameter sqlParameter6 = new SqlParameter();
          sqlParameter6.ParameterName = "OrgId";
          sqlParameter6.Value = (object) orgId;
          parameters6.Add(sqlParameter6);
        }
        if (!string.IsNullOrEmpty(scope))
        {
          SqlParameterCollection parameters7 = sqlCmd.Parameters;
          SqlParameter sqlParameter7 = new SqlParameter();
          sqlParameter7.ParameterName = "Scope";
          sqlParameter7.Value = (object) scope.ToLower();
          parameters7.Add(sqlParameter7);
        }
        if (!string.IsNullOrEmpty(userName))
        {
          SqlParameterCollection parameters8 = sqlCmd.Parameters;
          SqlParameter sqlParameter8 = new SqlParameter();
          sqlParameter8.ParameterName = "UserName";
          sqlParameter8.Value = (object) userName;
          parameters8.Add(sqlParameter8);
        }
        if (!string.IsNullOrEmpty(specificUserId))
        {
          SqlParameterCollection parameters9 = sqlCmd.Parameters;
          SqlParameter sqlParameter9 = new SqlParameter();
          sqlParameter9.ParameterName = "SpecificUserId";
          sqlParameter9.Value = (object) specificUserId;
          parameters9.Add(sqlParameter9);
        }
        SqlParameterCollection parameters10 = sqlCmd.Parameters;
        SqlParameter sqlParameter10 = new SqlParameter();
        sqlParameter10.ParameterName = "StartRecordNumber";
        sqlParameter10.Value = (object) offset;
        parameters10.Add(sqlParameter10);
        SqlParameterCollection parameters11 = sqlCmd.Parameters;
        SqlParameter sqlParameter11 = new SqlParameter();
        sqlParameter11.ParameterName = "EndRecordNumber";
        sqlParameter11.Value = (object) num;
        parameters11.Add(sqlParameter11);
        if (!string.IsNullOrEmpty(fullName))
        {
          SqlParameterCollection parameters12 = sqlCmd.Parameters;
          SqlParameter sqlParameter12 = new SqlParameter();
          sqlParameter12.ParameterName = "SpaceCount";
          sqlParameter12.Value = (object) (fullName.Split().Length - 1);
          parameters12.Add(sqlParameter12);
          SqlParameterCollection parameters13 = sqlCmd.Parameters;
          SqlParameter sqlParameter13 = new SqlParameter();
          sqlParameter13.ParameterName = "FullName";
          sqlParameter13.Value = (object) Regex.Replace(fullName, "\\s", "");
          parameters13.Add(sqlParameter13);
        }
        if (!string.IsNullOrEmpty(userType))
        {
          SqlParameterCollection parameters14 = sqlCmd.Parameters;
          SqlParameter sqlParameter14 = new SqlParameter();
          sqlParameter14.ParameterName = "UserType";
          sqlParameter14.Value = (object) Regex.Replace(userType, "\\s", "");
          parameters14.Add(sqlParameter14);
        }
        if (!includeDisabled)
        {
          SqlParameterCollection parameters15 = sqlCmd.Parameters;
          SqlParameter sqlParameter15 = new SqlParameter();
          sqlParameter15.ParameterName = "IncludeDisabled";
          sqlParameter15.Value = (object) includeDisabled;
          parameters15.Add(sqlParameter15);
        }
        if (populateCCsiteInfo)
        {
          SqlParameterCollection parameters16 = sqlCmd.Parameters;
          SqlParameter sqlParameter16 = new SqlParameter();
          sqlParameter16.ParameterName = "PopulateCCSiteInfo";
          sqlParameter16.Value = (object) populateCCsiteInfo;
          parameters16.Add(sqlParameter16);
        }
        if (populateLicenseInfo)
        {
          SqlParameterCollection parameters17 = sqlCmd.Parameters;
          SqlParameter sqlParameter17 = new SqlParameter();
          sqlParameter17.ParameterName = "PopulateLicensInfo";
          sqlParameter17.Value = (object) populateLicenseInfo;
          parameters17.Add(sqlParameter17);
        }
        if (populateUserGroups)
        {
          SqlParameterCollection parameters18 = sqlCmd.Parameters;
          SqlParameter sqlParameter18 = new SqlParameter();
          sqlParameter18.ParameterName = "PopulaetUserGroups";
          sqlParameter18.Value = (object) populateUserGroups;
          parameters18.Add(sqlParameter18);
        }
        if (populateCompPlans)
        {
          SqlParameterCollection parameters19 = sqlCmd.Parameters;
          SqlParameter sqlParameter19 = new SqlParameter();
          sqlParameter19.ParameterName = "PopulateCompPlans";
          sqlParameter19.Value = (object) populateCompPlans;
          parameters19.Add(sqlParameter19);
        }
        SqlParameter sqlParameter20 = new SqlParameter("TotalNumberOfRecords", SqlDbType.Int);
        sqlParameter20.Value = (object) 0;
        sqlParameter20.Direction = ParameterDirection.Output;
        sqlCmd.Parameters.Add(sqlParameter20);
        DataSet dataSet = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd, DbTransactionType.Default);
        if (Convert.ToInt32(sqlParameter20.Value) == 0)
          return (UserInfo[]) null;
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        dataSet.Relations.Add("Personas", table1.Columns["userid"], table2.Columns["userid"], false);
        int index1 = 1;
        if (populateCCsiteInfo)
        {
          ++index1;
          DataTable table3 = dataSet.Tables[index1];
          dataSet.Relations.Add("CCSiteInfo", table1.Columns["userid"], table3.Columns["userid"], false);
        }
        if (populateUserGroups)
        {
          ++index1;
          DataTable table4 = dataSet.Tables[index1];
          dataSet.Relations.Add("UserGroups", table1.Columns["userid"], table4.Columns["userid"], false);
        }
        if (populateLicenseInfo)
        {
          ++index1;
          DataTable table5 = dataSet.Tables[index1];
          dataSet.Relations.Add("LoLicenseInfo", table1.Columns["userid"], table5.Columns["userid"], false);
        }
        if (populateCompPlans)
        {
          int index2 = index1 + 1;
          DataTable table6 = dataSet.Tables[index2];
          dataSet.Relations.Add("LoCompPlans", table1.Columns["userid"], table6.Columns["userid"], false);
        }
        totalRecords = sqlParameter20.Value == DBNull.Value ? 0 : Convert.ToInt32(sqlParameter20.Value);
        Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
        ArrayList arrayList = new ArrayList();
        foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
        {
          DataRow[] childRows1 = row.GetChildRows("Personas");
          UserInfo userInfo = User.databaseRowToUserInfo(row, User.databaseRowsToPersonas(childRows1, personaLookup), "");
          arrayList.Add((object) userInfo);
          LoanCompHistoryList loanCompHistoryList = (LoanCompHistoryList) null;
          CCSiteInfo ccSiteInfo = (CCSiteInfo) null;
          AclGroup[] aclGroupArray = (AclGroup[]) null;
          DataRow[] childRows2 = row.GetChildRows("LoLicenseInfo");
          LOLicenseInfo[] loLicenseInfoArray = new LOLicenseInfo[childRows2.Length];
          for (int index3 = 0; index3 < childRows2.Length; ++index3)
            loLicenseInfoArray[index3] = User.dataRowToLOLicense(childRows2[index3]);
          DataRow[] childRows3 = row.GetChildRows("CCSiteInfo");
          if (childRows3.Length != 0)
            ccSiteInfo = CCSiteInfoAccessor.getUserCCSiteInfoFromDatarow(childRows3[0]);
          DataRow[] childRows4 = row.GetChildRows("LoCompPlans");
          if (childRows4.Length != 0)
            loanCompHistoryList = User.databaseRowsToLoanCompHistoryList(childRows4, userInfo.Userid);
          DataRow[] childRows5 = row.GetChildRows("UserGroups");
          if (childRows5.Length != 0)
            aclGroupArray = AclGroupAccessor.DatabaseRowsToUserGroups(childRows5);
          userDetails.Add(new Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>(userInfo, loLicenseInfoArray, ccSiteInfo, loanCompHistoryList, aclGroupArray));
        }
        return (UserInfo[]) arrayList.ToArray(typeof (UserInfo));
      }
      if (string.IsNullOrEmpty(scope) || !scope.Equals("role", StringComparison.OrdinalIgnoreCase) || roleIds == null || roleIds.Length == 0)
        return (UserInfo[]) null;
      Dictionary<string, UserInfo> dictionary = new Dictionary<string, UserInfo>();
      List<UserInfo> userInfoList = new List<UserInfo>();
      string userId = user.IsVirtualUser() ? "admin" : user.Userid;
      foreach (int roleId in roleIds)
      {
        foreach (UserInfo userInfo in ((IEnumerable<UserInfo>) User.GetScopedUsersWithRole(userId, roleId)).ToList<UserInfo>())
        {
          if (!dictionary.ContainsKey(userInfo.Userid))
            dictionary.Add(userInfo.Userid, userInfo);
        }
      }
      return User.GetPagedRecordsOfUsers(dictionary.Values.ToArray<UserInfo>(), offset, recordCount, out totalRecords);
    }

    private static UserInfo[] GetPagedRecordsOfUsers(
      UserInfo[] input,
      int start,
      int limit,
      out int totalRecords)
    {
      if (start >= 1)
        --start;
      totalRecords = 0;
      if (input == null || input.Length == 0)
        return (UserInfo[]) null;
      totalRecords = input.Length;
      if (start >= totalRecords)
        return (UserInfo[]) null;
      if (totalRecords < start + limit)
        limit = totalRecords - start;
      if (limit < 1)
        limit = 1;
      return ((IEnumerable<UserInfo>) input).ToList<UserInfo>().GetRange(start, limit).ToArray();
    }

    [PgReady]
    internal static UserInfo[] GetUsersFromSQL(string idListOrSubquery)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        if (string.IsNullOrEmpty(idListOrSubquery))
          return (UserInfo[]) null;
        try
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("select users.*, org_chart.depth, org_chart.org_name from users inner join org_chart on users.org_id = org_chart.oid where userid in (" + idListOrSubquery + ");");
          pgDbQueryBuilder.AppendLine("select * from UserPersona where userid in (" + idListOrSubquery + ")");
          DataSet dataSet = pgDbQueryBuilder.ExecuteSetQuery();
          DataTable table1 = dataSet.Tables[0];
          DataTable table2 = dataSet.Tables[1];
          dataSet.Relations.Add("Personas", table1.Columns["userid"], table2.Columns["userid"]);
          Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
          ArrayList arrayList = new ArrayList();
          foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
          {
            DataRow[] childRows = row.GetChildRows("Personas");
            arrayList.Add((object) User.databaseRowToUserInfo(row, User.databaseRowsToPersonas(childRows, personaLookup), ""));
          }
          return (UserInfo[]) arrayList.ToArray(typeof (UserInfo));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), User.checkInvalidOrgIDs(ex));
          return (UserInfo[]) null;
        }
      }
      else
      {
        if (string.IsNullOrEmpty(idListOrSubquery))
          return (UserInfo[]) null;
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
          dbQueryBuilder.AppendLine("select users.*, org_chart.depth, org_chart.org_name from users inner join org_chart on users.org_id = org_chart.oid where userid in (" + idListOrSubquery + ")");
          dbQueryBuilder.AppendLine("select * from UserPersona where userid in (" + idListOrSubquery + ")");
          DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
          DataTable table3 = dataSet.Tables[0];
          DataTable table4 = dataSet.Tables[1];
          dataSet.Relations.Add("Personas", table3.Columns["userid"], table4.Columns["userid"]);
          Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
          ArrayList arrayList = new ArrayList();
          foreach (DataRow row in (InternalDataCollectionBase) table3.Rows)
          {
            DataRow[] childRows = row.GetChildRows("Personas");
            arrayList.Add((object) User.databaseRowToUserInfo(row, User.databaseRowsToPersonas(childRows, personaLookup), ""));
          }
          return (UserInfo[]) arrayList.ToArray(typeof (UserInfo));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (User), User.checkInvalidOrgIDs(ex));
          return (UserInfo[]) null;
        }
      }
    }

    private static Persona[] databaseRowsToPersonas(DataRow[] rows, Hashtable personaLookup)
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < rows.Length; ++index)
      {
        int key = (int) rows[index]["personaID"];
        if (personaLookup.Contains((object) key))
          arrayList.Add(personaLookup[(object) key]);
      }
      return (Persona[]) arrayList.ToArray(typeof (Persona));
    }

    private static Dictionary<string, Persona[]> buildDatabaseRowsToPersonas(
      DataRowCollection rows,
      Hashtable personaLookup,
      User.UserRoleGroupEnum roleGroup)
    {
      Dictionary<string, ArrayList> dictionary = new Dictionary<string, ArrayList>();
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < rows.Count; ++index)
      {
        string key1 = rows[index]["roleID"].ToString();
        if (roleGroup == User.UserRoleGroupEnum.RoleGroup)
          key1 = key1 + "_" + rows[index]["GroupID"];
        int key2 = (int) rows[index]["personaID"];
        if (personaLookup.Contains((object) key2))
        {
          if (dictionary.TryGetValue(key1, out arrayList))
          {
            arrayList.Add(personaLookup[(object) key2]);
          }
          else
          {
            arrayList = new ArrayList();
            arrayList.Add(personaLookup[(object) key2]);
            dictionary.Add(key1, arrayList);
          }
        }
      }
      Dictionary<string, Persona[]> personas = new Dictionary<string, Persona[]>();
      foreach (KeyValuePair<string, ArrayList> keyValuePair in dictionary)
      {
        Persona[] array = (Persona[]) keyValuePair.Value.ToArray(typeof (Persona));
        personas.Add(keyValuePair.Key, array);
      }
      return personas;
    }

    [PgReady]
    private static void updateDbPersonas(string userid, Persona[] personas)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder idbqb = new PgDbQueryBuilder((IClientContext) current);
        idbqb.AppendLine("delete from UserPersona where userid = @userid;");
        if (personas != null && personas.Length != 0)
        {
          idbqb.AppendLine("insert into UserPersona (userid, personaID) VALUES");
          idbqb.AppendLine(string.Join(",\n", ((IEnumerable<Persona>) personas).Select<Persona, string>((System.Func<Persona, string>) (p => "(@userid, " + (object) p.ID + ")"))) + ";");
          DbValue pkColumnValue = new DbValue(nameof (userid), (object) "@userid", (IDbEncoder) DbEncoding.None);
          DbValue nonPkColumnValues = new DbValue("personaMigrated", (object) 1);
          PgQueryHelpers.Upsert((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, DbConstraint.Use, "acl_users", pkColumnValue, nonPkColumnValues);
        }
        DbCommandParameter parameter = new DbCommandParameter(nameof (userid), (object) userid.TrimEnd(), DbType.AnsiString);
        idbqb.Execute(parameter);
      }
      else
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from UserPersona");
        dbQueryBuilder.AppendLine("\twhere userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
        if (personas != null)
        {
          for (int index = 0; index < personas.Length; ++index)
          {
            dbQueryBuilder.AppendLine("insert into UserPersona");
            dbQueryBuilder.AppendLine("\t(userid, personaID) VALUES (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid) + ", " + (object) personas[index].ID + ")");
            dbQueryBuilder.AppendLine("Declare @userid" + (object) index + " varchar(16)");
            dbQueryBuilder.AppendLine("select @userid" + (object) index + " = userid from [Acl_Users] where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
            dbQueryBuilder.AppendLine("if @userid" + (object) index + " is NULL");
            dbQueryBuilder.AppendLine("begin");
            dbQueryBuilder.AppendLine("\tinsert into [Acl_Users] (userid, personaMigrated) values (" + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid) + ", 1)");
            dbQueryBuilder.AppendLine("end");
            dbQueryBuilder.AppendLine("else");
            dbQueryBuilder.AppendLine("begin");
            dbQueryBuilder.AppendLine("\tupdate [Acl_Users] set personaMigrated = 1 where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
            dbQueryBuilder.AppendLine("end");
          }
        }
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    private static UserInfo databaseRowToUserInfo(
      DataRow r,
      Persona[] personas,
      string dataServicesOptOutKey)
    {
      return new UserInfo(r["userid"].ToString(), (string) null, (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["last_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["suffix_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["first_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["middle_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["employee_id"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["profileURL"], (object) ""), personas, (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["working_folder"], (object) ""), (int) r["org_id"], (int) r["depth"] == 0, (UserInfo.AccessModeEnum) r["access_mode"], (UserInfo.UserStatusEnum) r["status"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["email"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["phone"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["cell_phone"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["fax"], (object) ""), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["no_pwd_expiration"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["require_pwd_change"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["locked"]), (UserInfo.UserPeerView) r["peerView"], dataServicesOptOutKey, r["delegate_tasks_right"].ToString() == "T", (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["last_login"], (object) DateTime.MinValue), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["chumid"], (object) ""), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["nmlsOriginatorID"]), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["nmlsExpirationDate"], (object) DateTime.MaxValue), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["enc_version"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["emailSignature"], ""), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["personalStatusOnline"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["inheritParentCompPlan"], false), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["apiUser"], false), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["oAuthClientId"], (object) null), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["allowImpersonation"], false), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["inheritParentccsite"], true), r["file_transfer_right"].ToString(), r["tmpl_table_right"].ToString(), r["tracking_setup_right"].ToString(), r["reports_right"].ToString(), r["myepass_custom_right"].ToString(), r["offline_right"].ToString(), (int) r["failed_login_attempts"], EllieMae.EMLite.DataAccess.SQL.DecodeString(r["lo_license"]), r["contact_export_right"].ToString(), r["plan_code_right"].ToString(), r["alt_lender_right"].ToString(), r["scope_lo"].ToString(), r["scope_lp"].ToString(), r["scope_closer"].ToString(), r["pipelineView"].ToString(), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["enc_version"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["firstLastName"]), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["userName"]), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["password_changed"], (object) DateTime.MaxValue), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["personaAccessComments"], (object) ""), (DateTime?) EllieMae.EMLite.DataAccess.SQL.Decode(r["lastLockedOutDateTime"], (object) null), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["ssoOnly"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["ssoDisconnectedFromOrg"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["passwordRequired"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["passwordExists"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTimeWithKind(r["createdDate"], DateTimeKind.Utc)), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["createdBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTimeWithKind(r["lastModifiedDate"], DateTimeKind.Utc)), EllieMae.EMLite.DataAccess.SQL.DecodeString(r["lastModifiedBy"]))
      {
        JobTitle = r.Table.Columns.Contains("jobtitle") ? (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["jobtitle"], (object) "") : "",
        UserType = r.Table.Columns.Contains("user_type") ? (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["user_type"], (object) "") : "",
        OrgName = r.Table.Columns.Contains("org_name") ? (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["org_name"], (object) null) : (string) null
      };
    }

    private static UserEntry databaseRowToUserEntry(DataRow r, Persona[] personas)
    {
      return new UserEntry(User.databaseRowToUserInfo(r, personas, ""), new UserServerInfo((DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["password_changed"], (object) DateTime.Now), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["failed_login_attempts"], (object) 0), (DateTime?) EllieMae.EMLite.DataAccess.SQL.Decode(r["lastLockedOutDateTime"], (object) null)));
    }

    private static UserEntry databaseRowToUserEntry(DataRow r, Persona[] personas, string clientID)
    {
      string dataServicesOptOutKey = "";
      if (EncompassServer.ServerMode != EncompassServerMode.Service)
      {
        lock (User.jed)
        {
          try
          {
            if (string.Concat(r["data_services_opt"]) != "")
            {
              byte[] A_0 = Convert.FromBase64String(r["data_services_opt"].ToString());
              User.jed.b();
              string str = User.jed.a(A_0, 0, A_0.Length);
              if (clientID + "|" + r["userid"] == str)
                dataServicesOptOutKey = r["data_services_opt"].ToString();
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
      return new UserEntry(User.databaseRowToUserInfo(r, personas, dataServicesOptOutKey), new UserServerInfo((DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["password_changed"], (object) DateTime.Now), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["failed_login_attempts"], (object) 0), (DateTime?) EllieMae.EMLite.DataAccess.SQL.Decode(r["lastLockedOutDateTime"], (object) null)));
    }

    private static void deleteUserFromDatabase(
      string userId,
      UserAssignedContactsBehaviorEnums? assignedContactsBehavior,
      string reassignContactsToUser)
    {
      try
      {
        if (userId == "(trusted)")
          Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot delete trusted user account"));
        if (userId == "admin")
          Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot delete admin user account"));
        DbValue key1 = new DbValue("userid", (object) userId);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (assignedContactsBehavior.HasValue)
        {
          UserAssignedContactsBehaviorEnums? nullable = assignedContactsBehavior;
          UserAssignedContactsBehaviorEnums contactsBehaviorEnums1 = UserAssignedContactsBehaviorEnums.Delete;
          if (nullable.GetValueOrDefault() == contactsBehaviorEnums1 & nullable.HasValue)
          {
            dbQueryBuilder.AppendLine("delete from Borrower where OwnerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) ?? "");
            dbQueryBuilder.AppendLine("delete from BizPartner where OwnerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) ?? "");
          }
          else
          {
            nullable = assignedContactsBehavior;
            UserAssignedContactsBehaviorEnums contactsBehaviorEnums2 = UserAssignedContactsBehaviorEnums.Reassign;
            if (nullable.GetValueOrDefault() == contactsBehaviorEnums2 & nullable.HasValue)
            {
              if (string.IsNullOrWhiteSpace(reassignContactsToUser))
                Err.Raise(TraceLevel.Warning, nameof (User), new ServerException("Cannot udpate Contacts reassignContactsToUser is null or whiteSpace"));
              dbQueryBuilder.AppendLine("update Borrower set OwnerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) reassignContactsToUser) + " where OwnerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) ?? "");
              dbQueryBuilder.AppendLine("update BizPartner set OwnerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) reassignContactsToUser) + " where OwnerID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) ?? "");
            }
          }
        }
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("AccessibleLoanOwnersOnly"), key1);
        DbValue key2 = new DbValue("OwnerID", (object) userId);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("AccessibleLoanOwnersOnly"), key2);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("recent_loans"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("loan_rights"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("user_settings"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("user_lo_licenses"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("users_CompPlans"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("users_ccsite"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("users"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("password_history"), key1);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("usercredentials"), key1);
        dbQueryBuilder.AppendLine("delete from AppointmentsXRef where DataKey in (select DataKey from Appointments where UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + ")");
        dbQueryBuilder.AppendLine("delete from users_ccsite where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) ?? "");
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("Appointments"), key1);
        dbQueryBuilder.AppendLineFormat("DELETE FROM ExternalOrgCompanyLenderContacts WHERE ContactSource = 0 AND ContactId IN (SELECT ContactId FROM ExternalOrgLenderContacts WHERE UserID = '{0}')", (object) userId);
        dbQueryBuilder.AppendLineFormat("DELETE FROM ExternalOrgCompanyLenderContacts WHERE ContactSource = 1 AND ContactId IN (SELECT salesRepId FROM ExternalOrgSalesReps WHERE UserID = '{0}')", (object) userId);
        DbValue key3 = new DbValue("UserId", (object) userId);
        dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("ExternalOrgLenderContacts"), key3);
        dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    [PgReady]
    private static LOLicenseInfo[] getLOLicensesFromDatabase(string userId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DbTableInfo table = DbAccessManager.GetTable("user_lo_licenses");
        string str = User.IsVirtualUser(userId) ? "admin" : userId;
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(table, new DbValue("userid", (object) "@userid", (IDbEncoder) DbEncoding.None));
        DbCommandParameter parameter = new DbCommandParameter("userid", (object) str.TrimEnd(), DbType.AnsiString);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.None, parameter);
        LOLicenseInfo[] licensesFromDatabase = new LOLicenseInfo[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          licensesFromDatabase[index] = User.dataRowToLOLicense(dataRowCollection[index]);
        return licensesFromDatabase;
      }
      DbTableInfo table1 = DbAccessManager.GetTable("user_lo_licenses");
      string str1 = User.IsVirtualUser(userId) ? "admin" : userId;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(table1, new DbValue("userid", (object) str1));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute(DbTransactionType.None);
      LOLicenseInfo[] licensesFromDatabase1 = new LOLicenseInfo[dataRowCollection1.Count];
      for (int index = 0; index < dataRowCollection1.Count; ++index)
        licensesFromDatabase1[index] = User.dataRowToLOLicense(dataRowCollection1[index]);
      return licensesFromDatabase1;
    }

    private static LOLicenseInfo getLOLicenseFromDatabase(string userId, string state)
    {
      DbTableInfo table = DbAccessManager.GetTable("user_lo_licenses");
      string str = User.IsVirtualUser(userId) ? "admin" : userId;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(table, new DbValueList()
      {
        new DbValue("userid", (object) str),
        new DbValue(nameof (state), (object) state.ToUpper())
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection.Count == 0 ? (LOLicenseInfo) null : User.dataRowToLOLicense(dataRowCollection[0]);
    }

    private static string[] getLOLicensedStateList(string userId)
    {
      DbTableInfo table = DbAccessManager.GetTable("user_lo_licenses");
      string str = User.IsVirtualUser(userId) ? "admin" : userId;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "state"
      }, new DbValueList()
      {
        new DbValue("userid", (object) str),
        new DbValue("enabled", (object) true, (IDbEncoder) DbEncoding.Flag)
      });
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      string[] licensedStateList = new string[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        licensedStateList[index] = (string) EllieMae.EMLite.DataAccess.SQL.Decode(dataRowCollection[index][0], (object) "");
      return licensedStateList;
    }

    [PgReady]
    private static LOLicenseInfo dataRowToLOLicense(DataRow r)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        LOLicenseInfo loLicense = new LOLicenseInfo((string) EllieMae.EMLite.DataAccess.SQL.Decode(r["userid"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["state"], (object) ""), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["enabled"], false), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["license"], (object) ""), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["expirationDate"], (object) DateTime.MaxValue));
        loLicense.IssueDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["issueDate"], (object) DateTime.MinValue);
        loLicense.StartDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["startDate"], (object) DateTime.MinValue);
        loLicense.LicenseStatus = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["status"], (object) "");
        loLicense.StatusDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["statusDate"], (object) DateTime.MinValue);
        loLicense.LastChecked = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["lastCheckedDate"], (object) DateTime.MinValue);
        return loLicense;
      }
      LOLicenseInfo loLicense1 = new LOLicenseInfo((string) EllieMae.EMLite.DataAccess.SQL.Decode(r["userid"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["state"], (object) ""), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["enabled"], (object) false), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["license"], (object) ""), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["expirationDate"], (object) DateTime.MaxValue));
      loLicense1.IssueDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["issueDate"], (object) DateTime.MinValue);
      loLicense1.StartDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["startDate"], (object) DateTime.MinValue);
      loLicense1.LicenseStatus = (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["status"], (object) "");
      loLicense1.StatusDate = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["statusDate"], (object) DateTime.MinValue);
      loLicense1.LastChecked = (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["lastCheckedDate"], (object) DateTime.MinValue);
      return loLicense1;
    }

    private static void addLOLicenseToDatabase(string userId, LOLicenseInfo license)
    {
      DbTableInfo table = DbAccessManager.GetTable("user_lo_licenses");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue dbValue1 = new DbValue("userid", (object) userId);
      DbValue dbValue2 = new DbValue("state", (object) license.StateAbbr.ToUpper());
      DbValue dbValue3 = new DbValue("enabled", (object) license.Enabled, (IDbEncoder) DbEncoding.Flag);
      DbValue dbValue4 = new DbValue(nameof (license), (object) license.License);
      DbValue dbValue5 = new DbValue("expirationDate", (object) null);
      if (license.ExpirationDate != DateTime.MinValue && license.ExpirationDate != DateTime.MaxValue)
        dbValue5 = new DbValue("expirationDate", (object) license.ExpirationDate, (IDbEncoder) DbEncoding.DateTime);
      DbValue dbValue6 = new DbValue("issueDate", (object) null);
      if (license.IssueDate != DateTime.MinValue && license.IssueDate != DateTime.MaxValue)
        dbValue6 = new DbValue("issueDate", (object) license.IssueDate, (IDbEncoder) DbEncoding.DateTime);
      DbValue dbValue7 = new DbValue("startDate", (object) null);
      if (license.StartDate != DateTime.MinValue && license.StartDate != DateTime.MaxValue)
        dbValue7 = new DbValue("startDate", (object) license.StartDate, (IDbEncoder) DbEncoding.DateTime);
      DbValue dbValue8 = new DbValue("status", (object) license.LicenseStatus);
      DbValue dbValue9 = new DbValue("statusDate", (object) null);
      if (license.StatusDate != DateTime.MinValue && license.StatusDate != DateTime.MaxValue)
        dbValue9 = new DbValue("statusDate", (object) license.StatusDate, (IDbEncoder) DbEncoding.DateTime);
      DbValue dbValue10 = new DbValue("lastCheckedDate", (object) null);
      if (license.LastChecked != DateTime.MinValue && license.LastChecked != DateTime.MaxValue)
        dbValue10 = new DbValue("lastCheckedDate", (object) license.LastChecked, (IDbEncoder) DbEncoding.DateTime);
      dbQueryBuilder.DeleteFrom(table, new DbValueList(new DbValue[2]
      {
        dbValue1,
        dbValue2
      }));
      dbQueryBuilder.InsertInto(table, new DbValueList(new DbValue[10]
      {
        dbValue1,
        dbValue2,
        dbValue3,
        dbValue4,
        dbValue5,
        dbValue6,
        dbValue7,
        dbValue8,
        dbValue9,
        dbValue10
      }), true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void deleteLOLicenseFromDatabase(string userId, string state)
    {
      DbTableInfo table = DbAccessManager.GetTable("user_lo_licenses");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue dbValue1 = new DbValue("userid", (object) userId);
      DbValue dbValue2 = new DbValue(nameof (state), (object) state.ToUpper());
      dbQueryBuilder.DeleteFrom(table, new DbValueList(new DbValue[2]
      {
        dbValue1,
        dbValue2
      }));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void deleteAllLOLicensesFromDatabase(string userId)
    {
      DbTableInfo table = DbAccessManager.GetTable("user_lo_licenses");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue key = new DbValue("userid", (object) userId);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static UserEntry getTrustedUserEntry()
    {
      return new UserEntry(new UserInfo("(trusted)", "", nameof (User), "", "Trusted", "", "", "", new Persona[1]
      {
        Persona.SuperAdministrator
      }, "", OrganizationStore.RootOrganizationID, true, UserInfo.AccessModeEnum.ReadWrite, UserInfo.UserStatusEnum.Enabled, "", "", "", "", true, true, false, UserInfo.UserPeerView.Edit, "", false, DateTime.MinValue, "", "", DateTime.MaxValue, "", "", false, false, false, (string) null, false, true), new UserServerInfo());
    }

    private static UserEntry getVirtualUserEntry(string userId)
    {
      return new UserEntry(new UserInfo(userId, "", nameof (User), "", userId, "", "", "", new Persona[1]
      {
        Persona.SuperAdministrator
      }, "", OrganizationStore.RootOrganizationID, true, UserInfo.AccessModeEnum.ReadWrite, UserInfo.UserStatusEnum.Enabled, "", "", "", "", true, true, false, UserInfo.UserPeerView.Edit, "", false, DateTime.MinValue, "", "", DateTime.MaxValue, "", "", false, false, false, (string) null, false, true), new UserServerInfo());
    }

    public static UserInfo[] GetUsersWithInOrgWithRole(int orgID, int roleId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT DISTINCT u.*, orgs.depth FROM RolePersonas rp INNER JOIN UserPersona up ON rp.PersonaId = up.PersonaId INNER JOIN Users u ON up.UserId = u.UserId INNER JOIN org_chart orgs ON u.org_id = orgs.oid WHERE rp.RoleId = " + (object) roleId + " and u.org_id = " + (object) orgID);
      dbQueryBuilder.AppendLine("SELECT up.* FROM UserPersona up WHERE up.UserId IN (SELECT u.UserId FROM RolePersonas rp INNER JOIN UserPersona up ON rp.PersonaId = up.PersonaId INNER JOIN Users u ON up.UserId = u.UserId WHERE rp.RoleId = " + (object) roleId + " and u.org_id = " + (object) orgID + ")");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      if (count == 0)
        return new UserInfo[0];
      dataSet.Relations.Add("UserPersonas", dataSet.Tables[0].Columns["UserId"], dataSet.Tables[1].Columns["UserId"]);
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      UserInfo[] withInOrgWithRole = new UserInfo[count];
      int num1 = 0;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        DataRow[] childRows = row.GetChildRows("UserPersonas");
        Persona[] personas = new Persona[childRows.Length];
        int num2 = 0;
        foreach (DataRow dataRow in childRows)
          personas[num2++] = (Persona) personaLookup[(object) (int) dataRow["PersonaId"]];
        withInOrgWithRole[num1++] = User.databaseRowToUserInfo(row, personas, "");
      }
      return withInOrgWithRole;
    }

    public static UserInfo[] GetUsersWithRole(int roleId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("SELECT DISTINCT u.*, orgs.depth FROM RolePersonas rp INNER JOIN UserPersona up ON rp.PersonaId = up.PersonaId INNER JOIN Users u ON up.UserId = u.UserId INNER JOIN org_chart orgs ON u.org_id = orgs.oid WHERE rp.RoleId = " + (object) roleId);
      dbQueryBuilder.AppendLine("SELECT up.* FROM UserPersona up WHERE up.UserId IN (SELECT u.UserId FROM RolePersonas rp INNER JOIN UserPersona up ON rp.PersonaId = up.PersonaId INNER JOIN Users u ON up.UserId = u.UserId WHERE rp.RoleId = " + (object) roleId + ")");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      if (count == 0)
        return (UserInfo[]) null;
      dataSet.Relations.Add("UserPersonas", dataSet.Tables[0].Columns["UserId"], dataSet.Tables[1].Columns["UserId"]);
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      UserInfo[] usersWithRole = new UserInfo[count];
      int num1 = 0;
      foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
      {
        DataRow[] childRows = row.GetChildRows("UserPersonas");
        Persona[] personas = new Persona[childRows.Length];
        int num2 = 0;
        foreach (DataRow dataRow in childRows)
          personas[num2++] = (Persona) personaLookup[(object) (int) dataRow["PersonaId"]];
        usersWithRole[num1++] = User.databaseRowToUserInfo(row, personas, "");
      }
      return usersWithRole;
    }

    public static List<string> GetUsersWithRoles(int[] roleIds)
    {
      List<string> usersWithRoles = new List<string>();
      foreach (int roleId in roleIds)
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Login);
        dbQueryBuilder.AppendLine("SELECT DISTINCT u.*, orgs.depth , rp.RoleId FROM RolePersonas rp INNER JOIN UserPersona up ON rp.PersonaId = up.PersonaId INNER JOIN Users u ON up.UserId = u.UserId INNER JOIN org_chart orgs ON u.org_id = orgs.oid WHERE rp.RoleId in ( " + (object) roleId + ")");
        dbQueryBuilder.AppendLine("SELECT up.* FROM UserPersona up WHERE up.UserId IN (SELECT u.UserId FROM RolePersonas rp INNER JOIN UserPersona up ON rp.PersonaId = up.PersonaId INNER JOIN Users u ON up.UserId = u.UserId WHERE rp.RoleId in (" + (object) roleId + "))");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet.Tables[0].Rows.Count == 0)
          return (List<string>) null;
        dataSet.Relations.Add("UserPersonas", dataSet.Tables[0].Columns["UserId"], dataSet.Tables[1].Columns["UserId"]);
        Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
        string empty = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase) dataSet.Tables[0].Rows)
        {
          DataRow[] childRows = row.GetChildRows("UserPersonas");
          Persona[] personaArray = new Persona[childRows.Length];
          int num = 0;
          foreach (DataRow dataRow in childRows)
            personaArray[num++] = (Persona) personaLookup[(object) (int) dataRow["PersonaId"]];
          string str = row["first_name"].ToString() + (string.Concat(row["middle_name"]) != string.Empty ? (object) (" " + row["middle_name"]) : (object) "") + " " + row["last_name"] + (string.Concat(row["suffix_name"]) != string.Empty ? (object) (" " + row["suffix_name"]) : (object) "");
          usersWithRoles.Add(row["RoleId"].ToString() + "$" + str);
        }
      }
      return usersWithRoles;
    }

    public static UserInfo[] GetUsersWithPersona(
      int personaID,
      int orgID,
      bool exactMatch,
      bool inclusive)
    {
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      if (orgID >= 0)
      {
        int[] descendentsOfOrg = OrganizationStore.GetDescendentsOfOrg(orgID);
        if (descendentsOfOrg != null && descendentsOfOrg.Length != 0)
          dictionary = ((IEnumerable<int>) descendentsOfOrg).Distinct<int>().ToDictionary<int, int, bool>((System.Func<int, int>) (x => x), (System.Func<int, bool>) (x => true));
        if (inclusive)
          dictionary[orgID] = true;
        if (dictionary.Count == 0)
          return new UserInfo[0];
      }
      Hashtable hashtable = new Hashtable();
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select U.userid, UP.personaID from users U inner join UserPersona UP on U.userid = UP.userid where UP.personaID = " + (object) personaID);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute(DbTransactionType.None))
      {
        string key = dataRow["UserId"].ToString();
        ArrayList arrayList = (ArrayList) hashtable[(object) key];
        if (arrayList == null)
          hashtable[(object) key] = (object) (arrayList = new ArrayList());
        arrayList.Add((object) (Persona) personaLookup[(object) (int) dataRow["PersonaId"]]);
      }
      dbQueryBuilder.Reset();
      string str = exactMatch ? " having Count(*) = 1" : "";
      dbQueryBuilder.AppendLine("select U.*, ORGS.Depth from users as U, org_chart ORGS");
      dbQueryBuilder.AppendLine("   where U.org_id = ORGS.oid and U.userid in");
      dbQueryBuilder.AppendLine("\t(");
      dbQueryBuilder.AppendLine("\t\tselect userid from UserPersona where personaID = " + (object) personaID);
      dbQueryBuilder.AppendLine("\t\tgroup by userid" + str);
      dbQueryBuilder.AppendLine("\t)");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      if (dataRowCollection == null)
        return (UserInfo[]) null;
      SortedDictionary<string, UserInfo> sortedDictionary = new SortedDictionary<string, UserInfo>();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
      {
        string key = r["UserId"].ToString();
        if (!sortedDictionary.ContainsKey(key))
        {
          Persona[] array = (Persona[]) ((ArrayList) hashtable[(object) key]).ToArray(typeof (Persona));
          UserInfo userInfo = User.databaseRowToUserInfo(r, array, "");
          if (dictionary.Count == 0 || dictionary.ContainsKey(userInfo.OrgId))
            sortedDictionary[key] = userInfo;
        }
      }
      return sortedDictionary.Values.ToArray<UserInfo>();
    }

    public static Hashtable GetUsers(string[] userIds, bool summariesOnly)
    {
      Hashtable users1 = new Hashtable();
      if (userIds.Length != 0)
      {
        UserInfo[] users2 = User.GetUsers(userIds);
        if (users2.Length != 0)
        {
          foreach (UserInfo userInfo in users2)
          {
            if (summariesOnly)
              users1[(object) userInfo.Userid] = (object) new UserInfoSummary(userInfo);
            else
              users1[(object) userInfo.Userid] = (object) userInfo;
          }
        }
      }
      return users1;
    }

    public static UserProfileInfo GetUserProfile(string userID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM user_profiles u WHERE userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection == null || dataRowCollection.Count == 0 ? (UserProfileInfo) null : User.databaseRowToUserProfileInfo(dataRowCollection[0]);
    }

    public UserProfileInfo GetUserProfile()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select top 1 * from user_profiles U where userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) this.UserID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      return dataRowCollection == null || dataRowCollection.Count <= 0 ? (UserProfileInfo) null : User.databaseRowToUserProfileInfo(dataRowCollection[0]);
    }

    public void InsertUserProfile(UserProfileInfo info)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbValueList dbValueList = User.createDbValueList(info);
        dbValueList.Add("userid", (object) info.UserId);
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("user_profiles"), dbValueList, true, false);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    public void UpdateUserProfile(UserProfileInfo info)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        User.createDbValueList(info).Add("userid", (object) info.UserId);
        dbQueryBuilder.Update(DbAccessManager.GetTable("user_profiles"), User.createDbValueList(info), new DbValue("userid", (object) info.UserId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    public void UpsertUserProfile(UserProfileInfo userProfileInfo)
    {
      try
      {
        DbValueList dbValueList = User.createDbValueList(userProfileInfo);
        dbValueList.Add("userid", (object) userProfileInfo.UserId);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("IF NOT EXISTS(SELECT TOP 1 1 from user_profiles WHERE userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userProfileInfo.UserId) + ")");
        dbQueryBuilder.AppendLine("BEGIN");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("user_profiles"), dbValueList, true, false);
        dbQueryBuilder.AppendLine("END");
        dbQueryBuilder.AppendLine("ELSE");
        dbQueryBuilder.AppendLine("BEGIN");
        dbQueryBuilder.Update(DbAccessManager.GetTable("user_profiles"), dbValueList, new DbValue("userid", (object) userProfileInfo.UserId));
        dbQueryBuilder.AppendLine("END");
        dbQueryBuilder.Execute();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
      }
    }

    [PgReady]
    private static UserProfileInfo databaseRowToUserProfileInfo(DataRow r)
    {
      return ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres ? new UserProfileInfo(r["userid"].ToString(), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["last_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["suffix_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["first_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["middle_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["job_title"], (object) ""), (int) r["phone1Type"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["phone1"], (object) ""), (int) r["phone2Type"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["phone2"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["email"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["link1"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["link2"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["link3"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["profileDesc"], (object) ""), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["firstname_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["lastname_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["middlename_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["suffix_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["phone1_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["email_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["phone2_default"]), EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(r["enable_profile"])) : new UserProfileInfo(r["userid"].ToString(), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["last_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["suffix_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["first_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["middle_name"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["job_title"], (object) ""), (int) r["phone1Type"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["phone1"], (object) ""), (int) r["phone2Type"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["phone2"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["email"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["link1"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["link2"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["link3"], (object) ""), (string) EllieMae.EMLite.DataAccess.SQL.Decode(r["profileDesc"], (object) ""), (bool) r["firstname_default"], (bool) r["lastname_default"], (bool) r["middlename_default"], (bool) r["suffix_default"], (bool) r["phone1_default"], (bool) r["email_default"], (bool) r["phone2_default"], (bool) r["enable_profile"]);
    }

    private static DbValueList createDbValueList(UserProfileInfo info)
    {
      return new DbValueList()
      {
        {
          "last_name",
          (object) info.LastName
        },
        {
          "first_name",
          (object) info.FirstName
        },
        {
          "middle_name",
          (object) info.MiddleName
        },
        {
          "suffix_name",
          (object) info.SuffixName
        },
        {
          "job_title",
          (object) info.JobTitle
        },
        {
          "phone1Type",
          (object) info.Phone1Type
        },
        {
          "phone1",
          (object) info.Phone1
        },
        {
          "phone2Type",
          (object) info.Phone2Type
        },
        {
          "phone2",
          (object) info.Phone2
        },
        {
          "email",
          (object) info.Email
        },
        {
          "link1",
          (object) info.Link1
        },
        {
          "link2",
          (object) info.Link2
        },
        {
          "link3",
          (object) info.Link3
        },
        {
          "profileDesc",
          (object) info.ProfileDesc
        },
        {
          "firstname_default",
          (object) (info.FirstName_IsDefault ? 1 : 0)
        },
        {
          "lastname_default",
          (object) (info.LastName_IsDefault ? 1 : 0)
        },
        {
          "middlename_default",
          (object) (info.MiddleName_IsDefault ? 1 : 0)
        },
        {
          "suffix_default",
          (object) (info.SuffixName_IsDefault ? 1 : 0)
        },
        {
          "phone1_default",
          (object) (info.Phone1_IsDefault ? 1 : 0)
        },
        {
          "email_default",
          (object) (info.Email_IsDefault ? 1 : 0)
        },
        {
          "phone2_default",
          (object) (info.Phone2_IsDefault ? 1 : 0)
        },
        {
          "enable_profile",
          (object) (info.Enable_Profile ? 1 : 0)
        }
      };
    }

    private static string GetPasswordUpdateHistoryQuery(UserEntry priorData, UserInfo newInfo)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("declare @userId varchar(16)");
      dbQueryBuilder.AppendLine("select @userId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) newInfo.Userid));
      bool requirePasswordChange = priorData.UserInfo.RequirePasswordChange;
      bool flag1 = false;
      if (!priorData.UserInfo.PasswordNeverExpires)
      {
        int serverSetting = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.Lifetime");
        if (serverSetting > 0)
        {
          DateTime dateTime = priorData.ServerInfo.LastPasswordChangedDate;
          dateTime = dateTime.AddDays((double) serverSetting);
          flag1 = dateTime.Date > DateTime.Today;
        }
      }
      bool flag2 = !(requirePasswordChange | flag1);
      UserPwdChangeAuditRecord changeAuditRecord = User.currentLoginUserID == null || User.currentLoginUserID == "" ? new UserPwdChangeAuditRecord(newInfo.Userid, newInfo.FullName, ActionType.UserPasswordChanged, DateTime.Now, newInfo.Userid, newInfo.FullName, "") : new UserPwdChangeAuditRecord(User.currentLoginUserID, newInfo.FullName, ActionType.UserPasswordChanged, DateTime.Now, newInfo.Userid, newInfo.FullName, "");
      string str = "";
      if (flag2)
      {
        str = "Current";
      }
      else
      {
        if (requirePasswordChange)
          str = "Forced";
        if (flag1)
          str = !(str == "") ? str + ", Expired" : "Expired";
      }
      changeAuditRecord.PriorStatus = str;
      if (newInfo.Password != null)
      {
        int serverSetting1 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.HistorySize");
        int serverSetting2 = (int) ClientContext.GetCurrent().Settings.GetServerSetting("Password.DaysToReuse");
        dbQueryBuilder.Append("declare @isCurrentPassword int\n");
        dbQueryBuilder.Append("declare @seqnum int\n");
        dbQueryBuilder.Append("declare @historySize int\n");
        dbQueryBuilder.Append("declare @pwdExpDate datetime\n");
        dbQueryBuilder.Append("select @historySize = " + (object) serverSetting1 + "\n");
        dbQueryBuilder.Append("select @pwdExpDate = DateAdd(d, -1 * " + (object) serverSetting2 + ", GetDate())\n");
        if (!User.compareUserPassword(newInfo.Userid, newInfo.Password))
        {
          dbQueryBuilder.Append("begin\n");
          dbQueryBuilder.Append("   select @seqnum = isnull(max(seqnum), 0) + 1 from password_history where userid = @userId\n");
          dbQueryBuilder.Append("   insert into password_history (userid, seqnum, password, isNewHashed) select u.userid, @seqnum, uc.password, 1 from users as u, usercredentials as uc where u.userid = @userId and uc.userid = @userId\n");
          dbQueryBuilder.Append("   delete from password_history where (userid = @userId) and (seqnum <= (@seqnum - @historySize)) and (date_changed < @pwdExpDate)\n");
          dbQueryBuilder.Append("   Insert into SysAT_UserPwdChanged (UserID, UserFullName, ActionType, DTTMStamp, UserAccountID, UserAccountName, ObjectType, PriorStatus) Values(" + EllieMae.EMLite.DataAccess.SQL.EncodeString(changeAuditRecord.UserID) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(changeAuditRecord.UserFullName) + ", " + (object) (int) changeAuditRecord.ActionType + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(changeAuditRecord.DateTime) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(changeAuditRecord.UserAccountID) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(changeAuditRecord.UserAccountName) + ", " + (object) (int) changeAuditRecord.ObjectType + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeString(changeAuditRecord.PriorStatus) + ")");
          dbQueryBuilder.Append("end\n");
        }
      }
      return dbQueryBuilder.ToString();
    }

    public void UpdateUserOnAuthentication(User user, bool doReset, string encompassVersion = null)
    {
      BackgroundActionRunner.EnqueueAction("UpdateUserOnAuthentication-" + user.UserInfo.Userid, (IClientContext) ClientContext.GetCurrent(), true, (Action<IClientContext>) (context =>
      {
        UserInfo userInfo = user.UserInfo;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine(string.Format("DECLARE @userId varchar(16) = {0}", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) userInfo.Userid)));
        if (doReset)
        {
          if (context.Cache.CacheSetting != CacheSetting.Disabled)
          {
            using (context.Cache.Lock(userInfo.Userid + "_reset-login", timeout: 600, supressWarning: true))
            {
              using (User user1 = UserStore.CheckOut(userInfo.Userid))
              {
                user1.FailedLoginAttempts = 0;
                user1.UserInfo.LastLogin = DateTime.Now;
                user1.UserInfo.LastLockedOutDateTime = new DateTime?();
                if ((encompassVersion ?? "") != "")
                  user1.UserInfo.EncompassVersion = encompassVersion;
                user1.ResetLoginInfo();
              }
            }
          }
          else
          {
            dbQueryBuilder.AppendLine(string.Format("UPDATE Users SET [failed_login_attempts] = {0}, last_login = '{1}', enc_version = {2}, locked = 0, lastLockedOutDateTime = null", (object) 0, (object) DateTime.Now, (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) encompassVersion)));
            if (userInfo.Locked)
              dbQueryBuilder.AppendLine(string.Format(", LastModifiedDate = '{0}', LastModifiedBy = '{1}'", (object) DateTime.UtcNow, (object) user.UserID));
            dbQueryBuilder.AppendLine("WHERE ([userid] = @userId)");
          }
        }
        else
        {
          dbQueryBuilder.AppendLine(string.Format("DECLARE @Failed_login_attempts SMALLINT\r\n                        DECLARE @MaxLoginFailures SMALLINT\r\n                        SELECT @MaxLoginFailures = Value FROM company_settings WHERE attribute = '{0}' and category = '{1}'\r\n                        SELECT @Failed_login_attempts = [failed_login_attempts] FROM users WHERE ([userid] = @userId)\r\n                        IF @MaxLoginFailures > 0 AND (@FAILED_LOGIN_ATTEMPTS + 1) >= @MaxLoginFailures\r\n                        BEGIN\r\n\t                        UPDATE Users SET [failed_login_attempts] = 0, [locked] = 1, LastModifiedDate = GETUTCDATE(), LastModifiedBy = '<system>', [LastLockedOutDateTime]='{2}' WHERE ([userid] = @userId)\r\n                        END\r\n                        ELSE\r\n\t                        UPDATE Users SET [failed_login_attempts] = [failed_login_attempts] + 1 WHERE ([userid] = @userId)", (object) "MaxLoginFailures", (object) "Password", (object) DateTime.Now));
          UserStore.RemoveCache(userInfo.Userid);
        }
        dbQueryBuilder.ExecuteNonQuery(TimeSpan.FromMilliseconds(500.0), DbTransactionType.Default);
      }));
    }

    public static bool CheckOAuthCilentIdExists(string oAuthClientId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string str = EllieMae.EMLite.DataAccess.SQL.Encode((object) oAuthClientId);
        dbQueryBuilder.AppendFormat("select count(*) from [users] where oAuthClientId = {0}", (object) str);
        return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return false;
      }
    }

    public static UserInfo[] GetPaginatedAccessibleUserRecords(
      UserInfo user,
      string orgId,
      int offset,
      int recordCount,
      out int totalRecords,
      out List<Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>> userDetails,
      GetUsersResultOption getUsersResultOption = GetUsersResultOption.None,
      bool isRecursive = false,
      IEnumerable<SearchFilter> searchFilters = null)
    {
      totalRecords = 0;
      userDetails = new List<Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>>();
      if (user == (UserInfo) null)
        return (UserInfo[]) null;
      SqlCommand sqlCommand = new SqlCommand();
      sqlCommand.CommandText = "GetAccessibleUsers";
      sqlCommand.CommandType = CommandType.StoredProcedure;
      SqlCommand sqlCmd = sqlCommand;
      bool flag1 = getUsersResultOption.HasFlag((Enum) GetUsersResultOption.CCSiteInfo);
      bool flag2 = getUsersResultOption.HasFlag((Enum) GetUsersResultOption.Licenses);
      bool flag3 = getUsersResultOption.HasFlag((Enum) GetUsersResultOption.UserGroups);
      bool flag4 = getUsersResultOption.HasFlag((Enum) GetUsersResultOption.LoCompPlans);
      DbAccessManager dbAccessManager = new DbAccessManager();
      DataTable dataTable = (DataTable) null;
      if (searchFilters != null && searchFilters.Any<SearchFilter>())
      {
        dataTable = new DataTable();
        dataTable.Columns.AddRange(new DataColumn[3]
        {
          new DataColumn("Operator", typeof (string)),
          new DataColumn("Operand", typeof (string)),
          new DataColumn("Val", typeof (string))
        });
        dataTable.Columns["Operand"].Unique = true;
        foreach (SearchFilter searchFilter in searchFilters)
        {
          StringQuery stringQuery = new StringQuery(searchFilter);
          DataRow row = dataTable.NewRow();
          row["Operator"] = (object) stringQuery.GetSqlOperator();
          row["Operand"] = (object) stringQuery.GetColumnWithTableAlias();
          row["Val"] = (object) stringQuery.GetValue(false);
          dataTable.Rows.Add(row);
        }
      }
      SqlParameterCollection parameters1 = sqlCmd.Parameters;
      SqlParameter sqlParameter1 = new SqlParameter();
      sqlParameter1.ParameterName = "CurUserId";
      sqlParameter1.Value = user.IsVirtualUser() ? (object) "admin" : (object) user.Userid;
      parameters1.Add(sqlParameter1);
      if (!string.IsNullOrEmpty(orgId))
      {
        SqlParameterCollection parameters2 = sqlCmd.Parameters;
        SqlParameter sqlParameter2 = new SqlParameter();
        sqlParameter2.ParameterName = "OrgId";
        sqlParameter2.Value = (object) orgId;
        parameters2.Add(sqlParameter2);
      }
      SqlParameterCollection parameters3 = sqlCmd.Parameters;
      SqlParameter sqlParameter3 = new SqlParameter();
      sqlParameter3.ParameterName = "StartRecordNumber";
      sqlParameter3.Value = (object) offset;
      parameters3.Add(sqlParameter3);
      SqlParameterCollection parameters4 = sqlCmd.Parameters;
      SqlParameter sqlParameter4 = new SqlParameter();
      sqlParameter4.ParameterName = "RecordCount";
      sqlParameter4.Value = (object) recordCount;
      parameters4.Add(sqlParameter4);
      if (flag1)
      {
        SqlParameterCollection parameters5 = sqlCmd.Parameters;
        SqlParameter sqlParameter5 = new SqlParameter();
        sqlParameter5.ParameterName = "PopulateCCSiteInfo";
        sqlParameter5.Value = (object) flag1;
        parameters5.Add(sqlParameter5);
      }
      if (flag2)
      {
        SqlParameterCollection parameters6 = sqlCmd.Parameters;
        SqlParameter sqlParameter6 = new SqlParameter();
        sqlParameter6.ParameterName = "PopulateLicensInfo";
        sqlParameter6.Value = (object) flag2;
        parameters6.Add(sqlParameter6);
      }
      if (flag3)
      {
        SqlParameterCollection parameters7 = sqlCmd.Parameters;
        SqlParameter sqlParameter7 = new SqlParameter();
        sqlParameter7.ParameterName = "PopulateUserGroups";
        sqlParameter7.Value = (object) flag3;
        parameters7.Add(sqlParameter7);
      }
      if (flag4)
      {
        SqlParameterCollection parameters8 = sqlCmd.Parameters;
        SqlParameter sqlParameter8 = new SqlParameter();
        sqlParameter8.ParameterName = "PopulateCompPlans";
        sqlParameter8.Value = (object) flag4;
        parameters8.Add(sqlParameter8);
      }
      if (isRecursive)
      {
        SqlParameterCollection parameters9 = sqlCmd.Parameters;
        SqlParameter sqlParameter9 = new SqlParameter();
        sqlParameter9.ParameterName = "IsRecursive";
        sqlParameter9.Value = (object) isRecursive;
        parameters9.Add(sqlParameter9);
      }
      if (searchFilters != null && searchFilters.Any<SearchFilter>())
      {
        SqlParameter sqlParameter10 = new SqlParameter("@FilterVariable", (object) dataTable)
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_Filter"
        };
        sqlCmd.Parameters.Add(sqlParameter10);
      }
      SqlParameter sqlParameter11 = new SqlParameter("TotalNumberOfRecords", SqlDbType.Int);
      sqlParameter11.Value = (object) 0;
      sqlParameter11.Direction = ParameterDirection.Output;
      sqlCmd.Parameters.Add(sqlParameter11);
      DataSet dataSet = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd, DbTransactionType.Default);
      if (Convert.ToInt32(sqlParameter11.Value) == 0)
        return (UserInfo[]) null;
      DataTable table1 = dataSet.Tables[0];
      DataTable table2 = dataSet.Tables[1];
      dataSet.Relations.Add("Personas", table1.Columns["userid"], table2.Columns["userid"], false);
      int index1 = 1;
      if (flag1)
      {
        ++index1;
        DataTable table3 = dataSet.Tables[index1];
        dataSet.Relations.Add("CCSiteInfo", table1.Columns["userid"], table3.Columns["userid"], false);
      }
      if (flag3)
      {
        ++index1;
        DataTable table4 = dataSet.Tables[index1];
        dataSet.Relations.Add("UserGroups", table1.Columns["userid"], table4.Columns["userid"], false);
      }
      if (flag2)
      {
        ++index1;
        DataTable table5 = dataSet.Tables[index1];
        dataSet.Relations.Add("LoLicenseInfo", table1.Columns["userid"], table5.Columns["userid"], false);
      }
      if (flag4)
      {
        int index2 = index1 + 1;
        DataTable table6 = dataSet.Tables[index2];
        dataSet.Relations.Add("LoCompPlans", table1.Columns["userid"], table6.Columns["userid"], false);
      }
      totalRecords = sqlParameter11.Value == DBNull.Value ? 0 : Convert.ToInt32(sqlParameter11.Value);
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      List<UserInfo> userInfoList = new List<UserInfo>(table1.Rows.Count);
      foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
      {
        DataRow[] childRows1 = row.GetChildRows("Personas");
        UserInfo userInfo = User.databaseRowToUserInfo(row, User.databaseRowsToPersonas(childRows1, personaLookup), "");
        userInfoList.Add(userInfo);
        LoanCompHistoryList loanCompHistoryList = (LoanCompHistoryList) null;
        CCSiteInfo ccSiteInfo = (CCSiteInfo) null;
        AclGroup[] aclGroupArray = (AclGroup[]) null;
        DataRow[] childRows2 = row.GetChildRows("LoLicenseInfo");
        LOLicenseInfo[] loLicenseInfoArray = new LOLicenseInfo[childRows2.Length];
        for (int index3 = 0; index3 < childRows2.Length; ++index3)
          loLicenseInfoArray[index3] = User.dataRowToLOLicense(childRows2[index3]);
        DataRow[] childRows3 = row.GetChildRows("CCSiteInfo");
        if (childRows3.Length != 0)
          ccSiteInfo = CCSiteInfoAccessor.getUserCCSiteInfoFromDatarow(childRows3[0]);
        DataRow[] childRows4 = row.GetChildRows("LoCompPlans");
        if (childRows4.Length != 0)
          loanCompHistoryList = User.databaseRowsToLoanCompHistoryList(childRows4, userInfo.Userid);
        DataRow[] childRows5 = row.GetChildRows("UserGroups");
        if (childRows5.Length != 0)
          aclGroupArray = AclGroupAccessor.DatabaseRowsToUserGroups(childRows5);
        userDetails.Add(new Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList, AclGroup[]>(userInfo, loLicenseInfoArray, ccSiteInfo, loanCompHistoryList, aclGroupArray));
      }
      return userInfoList.ToArray();
    }

    public static string UpsertUserInfo(
      UserInfo userInfo,
      UserEntry priorData,
      UserInfo loggedInUser,
      bool isUpdate,
      bool requirePasswordChange,
      LOLicenseInfo[] userLOLicenses,
      CCSiteInfo userCCSiteInfo,
      LoanCompHistoryList useloComplans,
      bool returnUpdatedResult,
      out Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList> updatedUser)
    {
      TraceLog.WriteInfo(nameof (User), "User.UpsertUserInfo: Creating SQL commands for table Users.");
      User.currentLoginUserID = loggedInUser.Userid;
      updatedUser = (Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList>) null;
      DbQueryBuilder sql = new DbQueryBuilder();
      DbAccessManager dbAccessManager = new DbAccessManager();
      List<SqlParameter> sqlParameterList = new List<SqlParameter>();
      DataSet userUpsertDataSet = User.GetUserUpsertDataSet(userInfo, loggedInUser, isUpdate, requirePasswordChange, userLOLicenses, userCCSiteInfo, useloComplans, sql);
      SqlParameter sqlParameter1 = new SqlParameter("@User", (object) userUpsertDataSet.Tables[0])
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_User"
      };
      sql.AppendLine("EXEC UpsertUsers @User");
      sqlParameterList.Add(sqlParameter1);
      SqlParameter sqlParameter2 = new SqlParameter("@userPersonas", (object) userUpsertDataSet.Tables[1])
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_CompositeIds"
      };
      sql.AppendLine("EXEC UpsertUserPersonas @userPersonas");
      sqlParameterList.Add(sqlParameter2);
      SqlParameter sqlParameter3 = new SqlParameter("@userSiteInfo", (object) userUpsertDataSet.Tables[2])
      {
        SqlDbType = SqlDbType.Structured,
        TypeName = "typ_UserCCSiteInfo"
      };
      sql.AppendLine("EXEC UpsertUserCCSiteInfo @userSiteInfo");
      sqlParameterList.Add(sqlParameter3);
      if (userUpsertDataSet.Tables[3].Rows.Count > 0)
      {
        SqlParameter sqlParameter4 = new SqlParameter("@userLOStateLicenses", (object) userUpsertDataSet.Tables[3])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_User_LO_StateLicense"
        };
        sql.AppendLine("EXEC UpsertUserLOStateLicenses @userLOStateLicenses");
        sqlParameterList.Add(sqlParameter4);
      }
      if (userUpsertDataSet.Tables[4].Rows.Count > 0)
      {
        SqlParameter sqlParameter5 = new SqlParameter("@userCompPlans", (object) userUpsertDataSet.Tables[4])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "Typ_UserCompPlans"
        };
        sql.AppendLine("EXEC UpsertUsersCompPlans @userCompPlans");
        sqlParameterList.Add(sqlParameter5);
      }
      if (userUpsertDataSet.Tables[5].Rows.Count > 0)
      {
        SqlParameter sqlParameter6 = new SqlParameter("@auditRecords", (object) userUpsertDataSet.Tables[5])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_UserAuditRecord"
        };
        sqlParameterList.Add(sqlParameter6);
        sql.AppendLine("EXEC InsertUserAuditRecord @auditRecords");
      }
      if (returnUpdatedResult)
      {
        SqlParameter sqlParameter7 = new SqlParameter("@uId", (object) userInfo.Userid);
        sql.AppendLine("EXEC [GetUserDetails] @uId");
        sqlParameterList.Add(sqlParameter7);
      }
      if (isUpdate && !userInfo.ApiUser && !string.IsNullOrEmpty(userInfo.Password))
        sql.AppendLine(User.GetPasswordUpdateHistoryQuery(priorData, userInfo));
      try
      {
        TraceLog.WriteInfo(nameof (User), sql.ToString());
        using (SqlCommand sqlCmd = new SqlCommand())
        {
          sqlCmd.CommandText = sql.ToString();
          sqlCmd.Parameters.AddRange(sqlParameterList.ToArray());
          DataSet userDetailsDataSet = dbAccessManager.ExecuteSetQuery((IDbCommand) sqlCmd);
          if (returnUpdatedResult)
            updatedUser = User.GetUserDetails(userDetailsDataSet, userInfo.Userid);
          if (!userInfo.ApiUser)
          {
            if (!string.IsNullOrEmpty(userInfo.Password))
              User.writePasswordToUserCredentials(userInfo.Userid, userInfo.Password);
          }
        }
        if (isUpdate)
          UserStore.RemoveCache(userInfo.Userid);
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (User), ex);
        throw new Exception("UpsertUserInfo: Cannot Create new User due to the following issue:\r\n" + ex.Message);
      }
      finally
      {
        sql.Reset();
      }
      string eventType = isUpdate ? "update" : "create";
      User.PublishUserKafkaEvent((IEnumerable<string>) new List<string>()
      {
        userInfo.Userid
      }, UserWebhookMessage.UserType.InternalUsers, eventType, loggedInUser);
      return userInfo.Userid;
    }

    private static bool PublishUserKafkaEvent(
      IEnumerable<string> contactIds,
      UserWebhookMessage.UserType userType,
      string eventType,
      UserInfo loggedInUser)
    {
      ClientContext current = ClientContext.GetCurrent();
      bool flag = false;
      try
      {
        UserWebhookEvent queueEvent = new UserWebhookEvent(current.InstanceName, loggedInUser.Userid, EncompassServer.ServerMode != EncompassServerMode.Service ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS, DateTime.UtcNow);
        queueEvent.CreateUserMessage(queueEvent.StandardMessage.EntityId, ClientContext.CurrentRequest.CorrelationId, current.InstanceName, loggedInUser.Userid, eventType, userType, current.ClientID, contactIds);
        if (queueEvent.QueueMessages.Count > 0)
        {
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (User), string.Format("Exception publishing userEvent to kafka for userId - {0}. Exception details {1}", (object) loggedInUser, (object) ex.StackTrace));
      }
      return flag;
    }

    private static DataSet GetUserUpsertDataSet(
      UserInfo userInfo,
      UserInfo loggedInUser,
      bool isUpdate,
      bool requirePasswordChange,
      LOLicenseInfo[] userLOLicenses,
      CCSiteInfo userCCSiteInfo,
      LoanCompHistoryList userCompPlans,
      DbQueryBuilder sql)
    {
      DataSet userUpsertDataSet = new DataSet();
      DataTable dtTable1 = new DataTable();
      dtTable1.Columns.AddRange(new DataColumn[43]
      {
        new DataColumn("Userid", typeof (string)),
        new DataColumn("OrgID", typeof (string)),
        new DataColumn("First_name", typeof (string)),
        new DataColumn("Middle_name", typeof (string)),
        new DataColumn("Last_name", typeof (string)),
        new DataColumn("Suffix_name", typeof (string)),
        new DataColumn("JobTitle", typeof (string)),
        new DataColumn("Status", typeof (int)),
        new DataColumn("Email", typeof (string)),
        new DataColumn("Phone", typeof (string)),
        new DataColumn("Cell_phone", typeof (string)),
        new DataColumn("Fax", typeof (string)),
        new DataColumn("Employee_Id", typeof (string)),
        new DataColumn("Working_Folder", typeof (string)),
        new DataColumn("Access_mode", typeof (int)),
        new DataColumn("No_Pwd_Expiration", typeof (bool)),
        new DataColumn("Require_Pwd_Change", typeof (bool)),
        new DataColumn("Locked", typeof (bool)),
        new DataColumn("PeerView", typeof (int)),
        new DataColumn("Data_Services_Opt", typeof (string)),
        new DataColumn("Chumid", typeof (string)),
        new DataColumn("NMLSOriginatorID", typeof (string)),
        new DataColumn("NMLSExpirationDate", typeof (DateTime)),
        new DataColumn("Last_Login", typeof (DateTime)),
        new DataColumn("Enc_Version", typeof (string)),
        new DataColumn("Email_Signature", typeof (string)),
        new DataColumn("ProfileURL", typeof (string)),
        new DataColumn("PersonalStatusOnline", typeof (bool)),
        new DataColumn("InheritParentCompPlan", typeof (bool)),
        new DataColumn("ApiUser", typeof (bool)),
        new DataColumn("OAuthClientId", typeof (string)),
        new DataColumn("AllowImpersonation", typeof (bool)),
        new DataColumn("InheritParentCCSite", typeof (bool)),
        new DataColumn("PersonaAccessComments", typeof (string)),
        new DataColumn("LastLockedOutDateTime", typeof (DateTime)),
        new DataColumn("Failed_Login_Attempts", typeof (int)),
        new DataColumn("SsoOnly", typeof (bool)),
        new DataColumn("SsoDisconnectedFromOrg", typeof (bool)),
        new DataColumn("CreatedDate", typeof (DateTime)),
        new DataColumn("CreatedBy", typeof (string)),
        new DataColumn("LastModifiedDate", typeof (DateTime)),
        new DataColumn("LastModifiedBy", typeof (string)),
        new DataColumn("Password", typeof (string))
      });
      DataTable dttblUserPersonas = new DataTable();
      dttblUserPersonas.Columns.AddRange(new DataColumn[2]
      {
        new DataColumn("id_1", typeof (int)),
        new DataColumn("id_2", typeof (string))
      });
      DataTable dtTable2 = new DataTable();
      dtTable2.Columns.AddRange(new DataColumn[10]
      {
        new DataColumn("UserId", typeof (string)),
        new DataColumn("State", typeof (string)),
        new DataColumn("Enabled", typeof (bool)),
        new DataColumn("LicenseNumber", typeof (string)),
        new DataColumn("ExpirationDate", typeof (DateTime)),
        new DataColumn("IssueDate", typeof (DateTime)),
        new DataColumn("StartDate", typeof (DateTime)),
        new DataColumn("Status", typeof (string)),
        new DataColumn("StatusDate", typeof (DateTime)),
        new DataColumn("LastCheckedDate", typeof (DateTime))
      });
      DataTable dtTable3 = new DataTable();
      dtTable3.Columns.AddRange(new DataColumn[3]
      {
        new DataColumn("UserId", typeof (string)),
        new DataColumn("SiteId", typeof (string)),
        new DataColumn("Url", typeof (string))
      });
      DataTable dtTable4 = new DataTable();
      dtTable4.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("UserId", typeof (string)),
        new DataColumn("CompPlanId", typeof (int)),
        new DataColumn("StartDate", typeof (DateTime)),
        new DataColumn("EndDate", typeof (DateTime))
      });
      DataTable dtTable5 = new DataTable();
      dtTable5.Columns.AddRange(new DataColumn[7]
      {
        new DataColumn("UserId", typeof (string)),
        new DataColumn("UserFullName", typeof (string)),
        new DataColumn("ActionType", typeof (int)),
        new DataColumn("DTTMStamp", typeof (DateTime)),
        new DataColumn("UserAccountID", typeof (string)),
        new DataColumn("UserAccountName", typeof (string)),
        new DataColumn("ObjectType", typeof (int))
      });
      DbTableInfo table1 = DbAccessManager.GetTable("Users");
      DbValueList values1 = new DbValueList();
      values1.Add("Userid", (object) userInfo.Userid);
      values1.Add("OrgID", (object) userInfo.OrgId);
      values1.Add("First_name", (object) userInfo.FirstName);
      values1.Add("Middle_name", (object) userInfo.MiddleName);
      values1.Add("Last_name", (object) userInfo.LastName);
      values1.Add("Suffix_name", (object) userInfo.SuffixName);
      values1.Add("JobTitle", (object) userInfo.JobTitle);
      values1.Add("Status", (object) userInfo.Status);
      values1.Add("Email", (object) userInfo.Email);
      values1.Add("Phone", (object) userInfo.Phone);
      values1.Add("Cell_phone", (object) userInfo.CellPhone);
      values1.Add("Fax", (object) userInfo.Fax);
      values1.Add("Employee_Id", (object) userInfo.EmployeeID);
      values1.Add("Working_Folder", (object) userInfo.WorkingFolder);
      values1.Add("Access_mode", (object) userInfo.AccessMode);
      values1.Add("No_Pwd_Expiration", (object) userInfo.PasswordNeverExpires);
      values1.Add("Require_Pwd_Change", (object) userInfo.RequirePasswordChange);
      values1.Add("Locked", (object) userInfo.Locked);
      values1.Add("PeerView", (object) userInfo.PeerView);
      values1.Add("Data_Services_Opt", (object) userInfo.DataServicesOptOut);
      values1.Add("Chumid", (object) userInfo.CHUMId);
      values1.Add("NMLSOriginatorID", (object) userInfo.NMLSOriginatorID);
      if (userInfo.NMLSExpirationDate != DateTime.MaxValue)
        values1.Add("NMLSExpirationDate", (object) userInfo.NMLSExpirationDate);
      else
        values1.Add("NMLSExpirationDate", (object) DBNull.Value);
      if (userInfo.LastLogin != DateTime.MinValue)
        values1.Add("Last_Login", (object) userInfo.LastLogin);
      else
        values1.Add("Last_Login", (object) DBNull.Value);
      DateTime? nullable = userInfo.LastLockedOutDateTime;
      if (nullable.GetValueOrDefault() != DateTime.MinValue)
        values1.Add("LastLockedOutDateTime", (object) userInfo.LastLockedOutDateTime);
      else
        values1.Add("LastLockedOutDateTime", (object) DBNull.Value);
      values1.Add("Enc_Version", (object) userInfo.EncompassVersion);
      values1.Add("Email_Signature", (object) userInfo.EmailSignature);
      values1.Add("ProfileURL", (object) userInfo.ProfileURL);
      values1.Add("PersonalStatusOnline", (object) userInfo.PersonalStatusOnline);
      values1.Add("InheritParentCompPlan", (object) userInfo.InheritParentCompPlan);
      values1.Add("ApiUser", (object) userInfo.ApiUser);
      if (!string.IsNullOrEmpty(userInfo.OAuthClientId))
        values1.Add("OAuthClientId", (object) userInfo.OAuthClientId);
      values1.Add("AllowImpersonation", (object) userInfo.AllowImpersonation);
      values1.Add("InheritParentCCSite", (object) userInfo.InheritParentCCSite);
      values1.Add("PersonaAccessComments", (object) userInfo.PersonaAccessComments);
      values1.Add("Failed_Login_Attempts", (object) userInfo.failed_login_attempts);
      values1.Add("SsoOnly", (object) userInfo.SSOOnly);
      values1.Add("SsoDisconnectedFromOrg", (object) userInfo.SSODisconnectedFromOrg);
      if (!string.IsNullOrEmpty(userInfo.Password))
        values1.Add("Password", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) User.getCaseSensitivePassword(userInfo.Password)), (IDbEncoder) DbEncoding.None);
      if (isUpdate)
      {
        nullable = userInfo.createdDate;
        DateTime minValue = DateTime.MinValue;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != minValue ? 1 : 0) : 0) : 1) != 0)
          values1.Add("CreatedDate", (object) userInfo.createdDate);
        else
          values1.Add("CreatedDate", (object) DBNull.Value);
        values1.Add("CreatedBy", (object) userInfo.createdBy);
        values1.Add("LastModifiedDate", (object) DateTime.UtcNow);
        values1.Add("LastModifiedBy", (object) loggedInUser.Userid);
      }
      else
      {
        DateTime utcNow = DateTime.UtcNow;
        values1.Add("CreatedDate", (object) utcNow);
        values1.Add("CreatedBy", (object) loggedInUser.Userid);
        values1.Add("LastModifiedDate", (object) utcNow);
        values1.Add("LastModifiedBy", (object) loggedInUser.Userid);
      }
      sql.InsertIntoDataTable(dtTable1, table1, values1);
      ((IEnumerable<Persona>) userInfo.UserPersonas).ToList<Persona>().ForEach((Action<Persona>) (p => dttblUserPersonas.Rows.Add((object) p.ID, (object) userInfo.Userid)));
      DbTableInfo table2 = DbAccessManager.GetTable("users_ccsite");
      if (userCCSiteInfo != null)
        sql.InsertIntoDataTable(dtTable3, table2, new DbValueList()
        {
          {
            "UserId",
            (object) userInfo.Userid
          },
          {
            "SiteId",
            (object) userCCSiteInfo.SiteId
          },
          {
            "Url",
            (object) userCCSiteInfo.Url
          }
        });
      DbTableInfo table3 = DbAccessManager.GetTable("users_CompPlans");
      if (userCompPlans != null)
      {
        userCompPlans.SortPlans(true);
        for (int i = 0; i < userCompPlans.Count; ++i)
        {
          DbValueList values2 = new DbValueList();
          LoanCompHistory historyAt = userCompPlans.GetHistoryAt(i);
          object endDate = (object) DBNull.Value;
          if (EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(historyAt.EndDate, DateTime.MaxValue) != "NULL")
            endDate = (object) historyAt.EndDate;
          values2.Add("UserId", (object) historyAt.Id);
          values2.Add("CompPlanId", (object) historyAt.CompPlanId);
          values2.Add("StartDate", (object) historyAt.StartDate);
          values2.Add("EndDate", endDate);
          sql.InsertIntoDataTable(dtTable4, table3, values2);
        }
      }
      if (userLOLicenses != null && userLOLicenses.Length != 0)
      {
        DbTableInfo table4 = DbAccessManager.GetTable("user_lo_licenses");
        foreach (LOLicenseInfo userLoLicense in userLOLicenses)
        {
          DbValueList values3 = new DbValueList();
          values3.Add("Userid", (object) userInfo.Userid);
          values3.Add("State", (object) userLoLicense.StateAbbrevation);
          values3.Add("Enabled", (object) (userLoLicense.Enabled ? 1 : 0));
          values3.Add("LicenseNumber", (object) userLoLicense.LicenseNo);
          if (userLoLicense.IssueDate != DateTime.MinValue)
            values3.Add("IssueDate", (object) userLoLicense.IssueDate);
          else
            values3.Add("IssueDate", (object) DBNull.Value);
          if (userLoLicense.StartDate != DateTime.MinValue)
            values3.Add("StartDate", (object) userLoLicense.StartDate);
          else
            values3.Add("StartDate", (object) DBNull.Value);
          if (userLoLicense.EndDate != DateTime.MinValue && userLoLicense.EndDate != DateTime.MaxValue)
            values3.Add("ExpirationDate", (object) userLoLicense.EndDate);
          else
            values3.Add("ExpirationDate", (object) DBNull.Value);
          values3.Add("Status", (object) userLoLicense.LicenseStatus);
          if (userLoLicense.StatusDate != DateTime.MinValue)
            values3.Add("StatusDate", (object) userLoLicense.StatusDate);
          else
            values3.Add("StatusDate", (object) DBNull.Value);
          if (userLoLicense.LastChecked != DateTime.MinValue)
            values3.Add("LastCheckedDate", (object) userLoLicense.LastChecked);
          else
            values3.Add("LastCheckedDate", (object) DBNull.Value);
          sql.InsertIntoDataTable(dtTable2, table4, values3);
        }
      }
      DbTableInfo table5 = DbAccessManager.GetTable("SysAT_UserProfile");
      sql.InsertIntoDataTable(dtTable5, table5, new DbValueList()
      {
        {
          "UserId",
          (object) loggedInUser.Userid
        },
        {
          "UserFullName",
          (object) loggedInUser.FullName
        },
        {
          "ActionType",
          (object) (ActionType) (isUpdate ? 8 : 7)
        },
        {
          "DTTMStamp",
          (object) DateTime.Now
        },
        {
          "UserAccountID",
          (object) userInfo.Userid
        },
        {
          "UserAccountName",
          (object) userInfo.FullName
        },
        {
          "ObjectType",
          (object) AuditObjectType.User
        }
      });
      if (requirePasswordChange)
        sql.InsertIntoDataTable(dtTable5, table5, new DbValueList()
        {
          {
            "UserId",
            (object) loggedInUser.Userid
          },
          {
            "UserFullName",
            (object) loggedInUser.FullName
          },
          {
            "ActionType",
            (object) ActionType.UserPasswordChangeForced
          },
          {
            "DTTMStamp",
            (object) DateTime.Now
          },
          {
            "UserAccountID",
            (object) userInfo.Userid
          },
          {
            "UserAccountName",
            (object) userInfo.FullName
          },
          {
            "ObjectType",
            (object) AuditObjectType.User
          }
        });
      userUpsertDataSet.Tables.AddRange(new DataTable[6]
      {
        dtTable1,
        dttblUserPersonas,
        dtTable3,
        dtTable2,
        dtTable4,
        dtTable5
      });
      return userUpsertDataSet;
    }

    private static Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList> GetUserDetails(
      DataSet userDetailsDataSet,
      string userId)
    {
      UserInfo userInfo = (UserInfo) null;
      LOLicenseInfo[] loLicenseInfoArray = (LOLicenseInfo[]) null;
      Persona[] personas = (Persona[]) null;
      Hashtable personaLookup = PersonaAccessor.GetPersonaLookup();
      CCSiteInfo ccSiteInfo = (CCSiteInfo) null;
      LoanCompHistoryList loanCompHistoryList = (LoanCompHistoryList) null;
      if (userDetailsDataSet.Tables[1].Rows.Count > 0)
        personas = User.databaseRowsToPersonas(userDetailsDataSet.Tables[1].Rows.Cast<DataRow>().ToArray<DataRow>(), personaLookup);
      if (userDetailsDataSet.Tables[0].Rows.Count > 0)
        userInfo = User.databaseRowToUserInfo(userDetailsDataSet.Tables[0].Rows[0], personas, "");
      if (userDetailsDataSet.Tables[2].Rows.Count > 0)
      {
        loLicenseInfoArray = new LOLicenseInfo[userDetailsDataSet.Tables[2].Rows.Count];
        for (int index = 0; index < userDetailsDataSet.Tables[2].Rows.Count; ++index)
          loLicenseInfoArray[index] = User.dataRowToLOLicense(userDetailsDataSet.Tables[2].Rows[index]);
      }
      if (userDetailsDataSet.Tables[3].Rows.Count > 0)
        ccSiteInfo = CCSiteInfoAccessor.getUserCCSiteInfoFromDatarow(userDetailsDataSet.Tables[3].Rows[0]);
      if (userDetailsDataSet.Tables[4].Rows.Count > 0)
        loanCompHistoryList = User.databaseRowsToLoanCompHistoryList(userDetailsDataSet.Tables[4].Rows.Cast<DataRow>().ToArray<DataRow>(), userId);
      return new Tuple<UserInfo, LOLicenseInfo[], CCSiteInfo, LoanCompHistoryList>(userInfo, loLicenseInfoArray, ccSiteInfo, loanCompHistoryList);
    }

    private static LoanCompHistoryList databaseRowsToLoanCompHistoryList(
      DataRow[] rows,
      string userId)
    {
      LoanCompHistoryList loanCompHistoryList = new LoanCompHistoryList(userId);
      foreach (DataRow row in rows)
      {
        LoanCompHistory history = new LoanCompHistory((string) row["userid"], (string) EllieMae.EMLite.DataAccess.SQL.Decode(row["name"], (object) ""), (int) EllieMae.EMLite.DataAccess.SQL.Decode(row["compplanid"], (object) -1), Utils.ParseDate(EllieMae.EMLite.DataAccess.SQL.Decode(row["startDate"], (object) DateTime.MinValue.ToString("MM/dd/yyyy"))), Utils.ParseDate(EllieMae.EMLite.DataAccess.SQL.Decode(row["endDate"], (object) DateTime.MaxValue.ToString("MM/dd/yyyy"))));
        loanCompHistoryList.AddHistory(history);
      }
      return loanCompHistoryList;
    }

    public static bool CheckIfLoanExistsForInternalUser(string userId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendFormat("select top 1 1 from [LoanAssociates] where userId = {0}", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return false;
      }
    }

    public static bool CheckIfContactsExistsForInternalUser(string userId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendFormat("select top 1 1 from [Borrower] where OwnerID = {0}", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        dbQueryBuilder.AppendFormat("select top 1 1 from [BizPartner] where OwnerID = {0}", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) userId));
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        return dataSet.Tables[0].Rows.Count > 0 || dataSet.Tables[1].Rows.Count > 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (User), ex);
        return false;
      }
    }

    private static string getDescendentWithInheritParentSSOSql(int orgId)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("WITH DescendentsWithInheritParentSSO(oid, parent) AS");
      stringBuilder.AppendLine("(SELECT oid , NULL");
      stringBuilder.AppendLine("FROM org_chart");
      stringBuilder.AppendLine(string.Format("WHERE oid = {0}", (object) orgId));
      stringBuilder.AppendLine("UNION ALL");
      stringBuilder.AppendLine("SELECT o.oid, p.oid");
      stringBuilder.AppendLine("FROM org_chart o");
      stringBuilder.AppendLine("INNER JOIN DescendentsWithInheritParentSSO p");
      stringBuilder.AppendLine("ON o.parent = p.oid AND o.inheritParentSSO = 1 AND o.oid <> o.parent)");
      return stringBuilder.ToString();
    }

    public enum UserRoleGroupEnum
    {
      RoleOnly = 0,
      RoleGroup = 1,
      GroupOnly = 3,
    }

    private class PasswordHistory
    {
      public object LastDate;
      public int PasswordsSince;

      public PasswordHistory(object lastDate, int passwordSince)
      {
        this.LastDate = lastDate;
        this.PasswordsSince = passwordSince;
      }
    }

    private class TemplateAccessEncoder : IDbEncoder
    {
      public string Encode(object value, DbColumnInfo columnInfo)
      {
        switch ((UserInfo.TemplateAccessEnum) value)
        {
          case UserInfo.TemplateAccessEnum.Public:
            return "'U'";
          case UserInfo.TemplateAccessEnum.Personal:
            return "'F'";
          case UserInfo.TemplateAccessEnum.All:
            return "'T'";
          default:
            return "'N'";
        }
      }

      public object Decode(object value)
      {
        if (value.ToString() == "T")
          return (object) UserInfo.TemplateAccessEnum.All;
        if (value.ToString() == "N")
          return (object) UserInfo.TemplateAccessEnum.None;
        return value.ToString() == "U" ? (object) UserInfo.TemplateAccessEnum.Public : (object) UserInfo.TemplateAccessEnum.Personal;
      }
    }

    private class Helper
    {
      public Helper() => a.a("32X9z798dyxk90b");
    }
  }
}
