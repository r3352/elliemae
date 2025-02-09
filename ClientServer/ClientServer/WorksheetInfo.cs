// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WorksheetInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class WorksheetInfo
  {
    public readonly string MilestoneID;
    public readonly int CoreMilestoneID = -1;
    public readonly string CustomMilestoneGuid;
    public readonly bool SetRoleFirst;
    public readonly string AlertMessage;
    public readonly RoleSummaryInfo Role;
    public readonly DateTime LastModTime = DateTime.MinValue;
    public readonly bool IsMsArchived;
    public readonly InputFormInfo FieldSummaryForm;

    public WorksheetInfo(
      string milestoneID,
      RoleSummaryInfo role,
      bool setRoleFirst,
      string alertMessage,
      DateTime lastModTime,
      bool isMsArchived,
      InputFormInfo fieldSummaryForm)
    {
      if (milestoneID == null)
        throw new Exception("Milestone ID cannot be null");
      try
      {
        int int32 = Convert.ToInt32(milestoneID);
        this.CoreMilestoneID = int32 > 0 ? int32 : throw new Exception(milestoneID + ": invalid core milestone id");
      }
      catch (FormatException ex)
      {
        this.CustomMilestoneGuid = milestoneID;
      }
      this.Role = role;
      this.SetRoleFirst = setRoleFirst;
      this.AlertMessage = alertMessage ?? "";
      this.MilestoneID = milestoneID;
      this.LastModTime = lastModTime;
      if (!this.IsCoreMilestone)
        this.IsMsArchived = isMsArchived;
      this.FieldSummaryForm = fieldSummaryForm;
    }

    public WorksheetInfo(
      string milestoneID,
      RoleSummaryInfo role,
      bool setRoleFirst,
      string alertMessage,
      bool isMsArchived,
      InputFormInfo fieldSummaryForm)
      : this(milestoneID, role, setRoleFirst, alertMessage, DateTime.MaxValue, isMsArchived, fieldSummaryForm)
    {
    }

    public bool ShowButton => this.Role != null && this.Role.RoleName != null;

    public bool IsCoreMilestone => this.CoreMilestoneID > 0;
  }
}
