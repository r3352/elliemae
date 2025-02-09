// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.SettingsRptJobInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class SettingsRptJobInfo
  {
    public Dictionary<string, string> reportParameters;
    public List<string> reportFilters;
    public string ReportID;
    public string ReportName;
    public SettingsRptJobInfo.jobType Type;
    public string ID;
    public string Description;
    public SettingsRptJobInfo.jobStatus Status;
    public string Message;
    public string CreatedBy;
    public string CreateDate;
    public string LastUpdateDt;
    public string CancelBy;
    public string CancelDt;
    public string DeleteBy;
    public string DeleteDt;
    public string FileGuid;

    public SettingsRptJobInfo(
      SettingsRptJobInfo.jobType type,
      string reportname,
      SettingsRptJobInfo.jobStatus status,
      string createdBy,
      string createDate)
      : this(type, reportname, status, createdBy, createDate, Guid.NewGuid().ToString())
    {
    }

    public SettingsRptJobInfo(
      SettingsRptJobInfo.jobType type,
      string reportname,
      SettingsRptJobInfo.jobStatus status,
      string createdBy,
      string createDate,
      string fileGuid)
    {
      this.Status = status;
      this.CreatedBy = createdBy;
      this.Type = type;
      this.ReportName = reportname;
      this.CreateDate = createDate;
      this.reportFilters = new List<string>();
      this.reportParameters = new Dictionary<string, string>();
      this.FileGuid = fileGuid;
    }

    public enum jobStatus
    {
      Submitted,
      InProgress,
      Completed,
      Failed,
      Canceled,
      Canceling,
      Deleted,
    }

    public enum jobType
    {
      Organization,
      Persona,
      UserGroups,
    }
  }
}
