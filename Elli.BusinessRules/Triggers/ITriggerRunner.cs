// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Triggers.ITriggerRunner
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.BusinessRules.Actions;
using Elli.Domain;
using Elli.Domain.Mortgage;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.BusinessRules.Triggers
{
  public interface ITriggerRunner : Elli.BusinessRules.Triggers.Base.ITriggerRunner
  {
    HashSet<RuleActionType> CalcActionTypes { get; }

    Loan Loan { get; }

    string CurrentActivationFieldModelPath { get; }

    int CurrentApplicationIndex { get; }

    bool PreventMilestoneChangesByTriggers { get; }

    ITrigger CurrentExecutingTrigger { get; }

    List<Tuple<string, int>> ModifiedFields { get; }

    LoanEventLogList LoanEventLogList { get; }

    void ExecuteTriggerForField(
      string memberModelPath,
      int applicationIndex,
      IList<ITrigger> memberValueList = null);

    void ExecuteCalcAndTriggerForModifiedField(string memberModelPath);

    bool IsLockableField(string fieldName);

    void ExecuteCalcAndTriggerForModifiedFieldCollection(
      ITrigger trigger,
      IEnumerable<KeyValuePair<Entity.MemberTrackingKey, Entity.MemberTrackingValue>> modifiedFields);

    void LoadTriggers(IEnumerable<ITrigger> elliTriggers);

    string GetMemberValue(string fieldId, string modelPath, int applicationIndex = 0);
  }
}
