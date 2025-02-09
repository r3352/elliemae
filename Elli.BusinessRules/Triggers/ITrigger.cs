// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Triggers.ITrigger
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.BusinessRules.Triggers
{
  public interface ITrigger
  {
    string TriggerId { get; }

    string TriggerName { get; }

    TriggerType TriggerType { get; }

    bool Execute(ITriggerRunner triggerRunner);

    IEnumerable<string> GetActivationFields();
  }
}
