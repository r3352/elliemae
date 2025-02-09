// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.Milestone
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  [Serializable]
  public class Milestone : IComparable<Milestone>, IWhiteListRemoteMethodParam
  {
    public const string FileStartedMilestoneID = "1";
    public const string CompletionMilestoneID = "7";
    private string milestoneId;
    private string name;
    private string tpoConnectStatus;
    private string consumerStatus;
    private bool archived;
    private int roleId;
    private int sortIndex;
    private Color displayColor;
    private string alertMessage;
    private string summaryFormId;
    private bool roleRequired;
    private string descTextBefore;
    private string descTextAfter;
    private int defaultDays;
    private string roleName;
    private string formName;

    public Milestone(string name)
    {
      this.milestoneId = Guid.NewGuid().ToString();
      this.name = name;
      this.tpoConnectStatus = "";
      this.consumerStatus = "";
      this.archived = false;
      this.roleId = -1;
      this.displayColor = Color.White;
      this.alertMessage = "";
      this.summaryFormId = (string) null;
      this.sortIndex = 9999;
      this.roleRequired = false;
      this.descTextBefore = "";
      this.descTextAfter = "";
      this.defaultDays = 0;
    }

    public Milestone(string msid, int sortIndex, int roleId)
    {
      this.milestoneId = msid;
      this.sortIndex = sortIndex;
      this.roleId = roleId;
      this.roleRequired = false;
    }

    public Milestone(
      string msid,
      string name,
      string tpoConnectStatus,
      string consumerStatus,
      string displayName,
      bool archived,
      int roleId,
      int sortIndex,
      Color displayColor,
      string alertMessage,
      string summaryFormId,
      bool roleRequired,
      string descTextBefore,
      string descTextAfter,
      int defaultDays)
    {
      this.milestoneId = msid;
      this.name = name;
      this.tpoConnectStatus = tpoConnectStatus;
      this.consumerStatus = consumerStatus;
      this.archived = archived;
      this.roleId = roleId;
      this.displayColor = displayColor;
      this.sortIndex = sortIndex;
      this.alertMessage = alertMessage;
      this.summaryFormId = summaryFormId;
      this.roleRequired = roleRequired;
      this.descTextBefore = descTextBefore;
      this.descTextAfter = descTextAfter;
      this.defaultDays = defaultDays;
    }

    public string MilestoneID => this.milestoneId;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public string TPOConnectStatus
    {
      get => this.tpoConnectStatus;
      set => this.tpoConnectStatus = value;
    }

    public string ConsumerStatus
    {
      get => this.consumerStatus;
      set => this.consumerStatus = value;
    }

    public bool Archived
    {
      get => this.archived;
      set => this.archived = value;
    }

    public int RoleID
    {
      get => this.roleId;
      set => this.roleId = value;
    }

    public int SortIndex => this.sortIndex;

    public Color DisplayColor
    {
      get => this.displayColor;
      set => this.displayColor = value;
    }

    public string AlertMessage
    {
      get => this.alertMessage;
      set => this.alertMessage = value;
    }

    public string SummaryFormID
    {
      get => this.summaryFormId;
      set => this.summaryFormId = value;
    }

    public bool RoleRequired
    {
      get => this.roleRequired;
      set => this.roleRequired = value;
    }

    public string DescTextBefore
    {
      get => this.descTextBefore;
      set => this.descTextBefore = value;
    }

    public string DescTextAfter
    {
      get => this.descTextAfter;
      set => this.descTextAfter = value;
    }

    public int DefaultDays
    {
      get => this.defaultDays;
      set => this.defaultDays = value;
    }

    public int CompareTo(Milestone other) => other == null ? -1 : this.sortIndex - other.sortIndex;

    public override bool Equals(object obj)
    {
      return obj is Milestone milestone && string.Compare(this.MilestoneID, milestone.MilestoneID, true) == 0;
    }

    public override int GetHashCode() => this.MilestoneID.ToLower().GetHashCode();

    public void SerializeForLog(JsonWriter writer, JsonSerializer serializer)
    {
      serializer.Serialize(writer, (object) new JObject()
      {
        {
          "name",
          (JToken) this.name
        }
      });
    }

    public override string ToString() => this.Name;

    public string RoleName
    {
      get => this.roleName;
      set => this.roleName = value;
    }

    public string FormName
    {
      get => this.formName;
      set => this.formName = value;
    }
  }
}
