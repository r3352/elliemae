// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Calculation.IElliDomainCalculation
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.Domain;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.BusinessRules.Calculation
{
  public interface IElliDomainCalculation
  {
    void CalculateTargetFieldValue(object modifiedObject, string modelFieldName);

    void CalculateTargetFieldValue(
      object modifiedObject,
      string modelFieldName,
      bool sendLockLists);

    object GetCalculatedValue(object modifiedObject, string modelFieldName);

    void LoanCalculateAllFields();

    void LoanCalculateForModifiedFieldCollection(
      IEnumerable<KeyValuePair<Entity.MemberTrackingKey, Entity.MemberTrackingValue>> modifiedFields);

    void LoanCalculateForAprFields(
      IEnumerable<KeyValuePair<Entity.MemberTrackingKey, Entity.MemberTrackingValue>> modifiedFields);

    void DisposeCalcEngine();

    void UpdatePreCalculationFields();

    IDisposable NewCalculationSession();
  }
}
