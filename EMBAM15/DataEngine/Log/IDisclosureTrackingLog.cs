// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.IDisclosureTrackingLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public interface IDisclosureTrackingLog
  {
    bool eDisclosureApplicationPackage { get; set; }

    bool eDisclosureThreeDayPackage { get; set; }

    bool eDisclosureLockPackage { get; set; }

    bool eDisclosureApprovalPackage { get; set; }

    string BorrowerPairID { get; }

    string eDisclosureDisclosedMessage { get; }

    string eDisclosureManuallyFulfilledBy { get; set; }

    DateTime eDisclosureManualFulfillmentDate { get; set; }

    string eDisclosureManualFulfillmentComment { get; set; }

    DisclosureTrackingBase.DisclosedMethod eDisclosureManualFulfillmentMethod { get; set; }

    DisclosureTrackingBase.DisclosedMethod eDisclosureAutomatedFulfillmentMethod { get; set; }

    string FulfillmentTrackingNumber { get; set; }

    string GetDisclosedField(string fieldId);

    Dictionary<string, string> GetDisclosedFields(List<string> fieldIDs);

    bool IsFieldLocked(string fieldId);
  }
}
