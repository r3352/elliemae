// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Workflow.MilestoneTemplateMilestoneUserInfo
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Workflow
{
  public class MilestoneTemplateMilestoneUserInfo
  {
    public string UserId { get; set; }

    public string UserName { get; set; }

    public Dictionary<int, string> RoleInfo { get; set; }

    public bool SendNotification { get; set; }
  }
}
