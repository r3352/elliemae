// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ILoanSettings
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [CLSCompliant(true)]
  public interface ILoanSettings
  {
    string SystemID { get; }

    FieldSettings FieldSettings { get; }

    AlertSetupData AlertSetupData { get; }

    IDictionary ComplianceSettings { get; }

    IDictionary AlertSettings { get; }

    DateTimeType MilestoneDateTimeType { get; }

    DateTimeType DocumentDateTimeType { get; }

    LoanMigrationData MigrationData { get; }

    RoleInfo[] AllRoles { get; }

    bool Use10DigitLockSecondaryTradeFields { get; }

    IMilestoneDateCalculator MilestoneDateCalculator { get; }

    RoleInfo GetRole(int roleID);

    HMDAInformation HMDAInfo { get; set; }

    DateTime DDMLastModifiedDateTime { get; set; }

    bool AllowHybridWithENoteClosing { get; set; }

    bool EnableTempBuyDown { get; set; }

    bool LoanAmountRounding { get; set; }

    bool Use5DecimalsForIndexRates { get; set; }

    Func<string, Dictionary<string, string>> LoanExternalFields { get; set; }
  }
}
