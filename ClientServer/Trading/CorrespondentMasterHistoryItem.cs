// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterHistoryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentMasterHistoryItem
  {
    public const string TradeNameProperty = "CorrespondentMasterName�";
    public const string UserNameProperty = "UserName�";
    public const string CommentProperty = "Comment�";
    public const string LoanNumberProperty = "LoanNumber�";
    private int correspondentMasterHistoryId = -1;
    private int correspondentMasterId = -1;
    private string userId = "";
    private CorrespondentMasterHistoryAction action;
    private int status = -1;
    private DateTime timestamp = DateTime.MinValue;
    private XmlDictionary<string> data = new XmlDictionary<string>();
    private string dataXml = string.Empty;
    private string loanGuid;

    public CorrespondentMasterHistoryItem(
      int correspondentMasterHistoryId,
      int correspondentMasterId,
      string loanGuid,
      CorrespondentMasterHistoryAction action,
      int status,
      DateTime timestamp,
      string userId,
      string dataXml)
    {
      this.CorrespondentMasterHistoryID = correspondentMasterHistoryId;
      this.correspondentMasterId = correspondentMasterId;
      this.loanGuid = loanGuid;
      this.action = action;
      this.status = status;
      this.timestamp = timestamp;
      this.userId = userId;
      this.dataXml = dataXml;
      try
      {
        this.data = XmlDictionary<string>.Parse(this.dataXml);
      }
      catch
      {
      }
      if (this.data != null)
        return;
      this.data = new XmlDictionary<string>();
    }

    private CorrespondentMasterHistoryItem(
      CorrespondentMasterInfo correspondentMaster,
      PipelineInfo loanInfo,
      CorrespondentMasterHistoryAction action,
      int status,
      UserInfo user)
    {
      this.action = action;
      this.status = status;
      if (correspondentMaster != null)
      {
        this.correspondentMasterId = correspondentMaster.ID;
        this.Data["CorrespondentMasterName"] = correspondentMaster.Name;
      }
      if (user != (UserInfo) null)
      {
        this.UserID = user.Userid;
        this.Data[nameof (UserName)] = user.FullName;
      }
      if (loanInfo == null)
        return;
      this.loanGuid = loanInfo.GUID;
      this.Data["LoanNumber"] = string.Concat(loanInfo.GetField("LoanNumber"));
    }

    public CorrespondentMasterHistoryItem(
      CorrespondentMasterInfo correspondentMaster,
      CorrespondentMasterHistoryAction action,
      UserInfo user)
      : this(correspondentMaster, (PipelineInfo) null, action, 0, user)
    {
    }

    public CorrespondentMasterHistoryItem(
      CorrespondentMasterInfo correspondentMaster,
      MasterCommitmentStatus status,
      UserInfo user)
      : this(correspondentMaster, (PipelineInfo) null, CorrespondentMasterHistoryAction.CorrespondentMasterStatusChanged, (int) status, user)
    {
    }

    public CorrespondentMasterHistoryItem(
      CorrespondentMasterInfo correspondentMaster,
      PipelineInfo loanInfo,
      CorrespondentMasterLoanStatus status,
      UserInfo user)
      : this(correspondentMaster, loanInfo, CorrespondentMasterHistoryAction.LoanStatusChanged, (int) status, user)
    {
      if (status == CorrespondentMasterLoanStatus.Assigned)
      {
        this.Action = CorrespondentMasterHistoryAction.LoanAssigned;
      }
      else
      {
        if (status != CorrespondentMasterLoanStatus.Unassigned)
          return;
        this.Action = CorrespondentMasterHistoryAction.LoanRemoved;
      }
    }

    public CorrespondentMasterHistoryItem(
      CorrespondentMasterInfo correspondentMaster,
      PipelineInfo loanInfo,
      CorrespondentMasterLoanStatus status,
      string comment,
      UserInfo user)
      : this(correspondentMaster, loanInfo, status, user)
    {
      this.Comment = comment;
    }

    public int CorrespondentMasterHistoryID
    {
      get => this.correspondentMasterHistoryId;
      set => this.correspondentMasterHistoryId = value;
    }

    public int CorrespondentMasterID
    {
      get => this.correspondentMasterId;
      set => this.correspondentMasterId = value;
    }

    public string UserID
    {
      get => this.userId;
      set => this.userId = value;
    }

    public string TradeName
    {
      get => this.data["CorrespondentMasterName"];
      set => this.data["CorrespondentMasterName"] = value;
    }

    public string Comment
    {
      get
      {
        try
        {
          return this.data[nameof (Comment)];
        }
        catch
        {
          return "";
        }
      }
      set => this.data[nameof (Comment)] = value;
    }

    public CorrespondentMasterHistoryAction Action
    {
      get => this.action;
      set => this.action = value;
    }

    public int Status
    {
      get => this.status;
      set => this.status = value;
    }

    public DateTime Timestamp
    {
      get => this.timestamp;
      set => this.timestamp = value;
    }

    public string UserName
    {
      get => this.data[nameof (UserName)];
      set => this.data[nameof (UserName)] = value;
    }

    public string LoanGuid => this.loanGuid;

    public XmlDictionary<string> Data => this.data;

    public virtual string Description
    {
      get
      {
        string subDescription = this.getSubDescription();
        if (!string.IsNullOrEmpty(subDescription))
          return subDescription;
        switch (this.action)
        {
          case CorrespondentMasterHistoryAction.CorrespondentMasterCreated:
            return "Correspondent Master created";
          case CorrespondentMasterHistoryAction.CorrespondentMasterArchived:
            return "Correspondent Master archived";
          case CorrespondentMasterHistoryAction.CorrespondentMasterActivated:
            return "Correspondent Master activated";
          case CorrespondentMasterHistoryAction.CorrespondentMasterStatusChanged:
            return "Correspondent Master marked as " + new MasterCommitmentStatusEnumNameProvider().GetName((object) (MasterCommitmentStatus) this.status);
          default:
            return "No description available";
        }
      }
    }

    protected virtual string getSubDescription() => string.Empty;

    public string StatusDescription
    {
      get
      {
        switch (this.action)
        {
          case CorrespondentMasterHistoryAction.CorrespondentMasterCreated:
            return "Created";
          case CorrespondentMasterHistoryAction.CorrespondentMasterArchived:
            return "Archived";
          case CorrespondentMasterHistoryAction.CorrespondentMasterActivated:
            return "Activated";
          case CorrespondentMasterHistoryAction.CorrespondentTradeAssigned:
          case CorrespondentMasterHistoryAction.LoanAssigned:
            return "Assigned";
          case CorrespondentMasterHistoryAction.CorrespondentTradeUnassigned:
          case CorrespondentMasterHistoryAction.LoanRemoved:
            return "Removed";
          default:
            return "No description available";
        }
      }
    }

    public override string ToString() => this.Description;
  }
}
