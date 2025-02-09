// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ITaskSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>Interface for the TaskSet class.</summary>
  /// <exclude />
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
