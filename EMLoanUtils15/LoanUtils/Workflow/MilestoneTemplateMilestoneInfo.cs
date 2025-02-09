// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.MilestoneTemplateMilestoneInfo
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class MilestoneTemplateMilestoneInfo
  {
    public string MilestoneId { get; set; }

    public string MilestoneName { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public int DaysToComplete { get; set; }

    public DateTime ExpectedDate { get; set; }

    public bool IsCompeted { get; set; }

    public bool IsEffected { get; set; }
  }
}
