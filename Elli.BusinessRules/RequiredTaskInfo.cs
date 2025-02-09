// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.RequiredTaskInfo
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using System;

#nullable disable
namespace Elli.BusinessRules
{
  public class RequiredTaskInfo
  {
    public string TaskGuid { get; set; }

    public bool IsRequired { get; set; }

    public string TaskType { get; set; }

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public int DaysToComplete { get; set; }

    public int TaskPriority { get; set; }

    public DateTime? ExpectedDate { get; set; }

    public string LoanTaskGuid { get; set; }
  }
}
