// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.MilestoneBase
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using System;

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public class MilestoneBase
  {
    public readonly string MilestoneID;

    public MilestoneBase(string milestoneId) => this.MilestoneID = (milestoneId ?? "").Trim();

    public bool IsCoreMilestone => this.CoreMilestoneID > 0;

    public string CustomMilestoneGuid => !this.IsCoreMilestone ? this.MilestoneID : "";

    public int CoreMilestoneID
    {
      get
      {
        try
        {
          return Convert.ToInt32(this.MilestoneID);
        }
        catch
        {
          return -1;
        }
      }
    }
  }
}
