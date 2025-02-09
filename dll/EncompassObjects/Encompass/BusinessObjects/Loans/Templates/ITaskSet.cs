// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ITaskSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("6E889061-3E77-4526-AF1B-23A510742F25")]
  public interface ITaskSet
  {
    string Name { get; }

    string Path { get; }

    TemplateType TemplateType { get; }

    string Description { get; }

    TaskTemplateList GetTasksForMilestone(Milestone ms);

    TaskTemplateList GetAllTasks();
  }
}
