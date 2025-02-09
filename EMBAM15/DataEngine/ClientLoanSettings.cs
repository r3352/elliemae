// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ClientLoanSettings
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
  [Serializable]
  public class ClientLoanSettings : ILoanSettings
  {
    public ClientLoanSettings()
    {
      this.MilestoneDateTimeType = DateTimeType.Calendar;
      this.DocumentDateTimeType = DateTimeType.Calendar;
    }

    public string SystemID { get; set; }

    public FieldSettings FieldSettings { get; set; }

    public AlertSetupData AlertSetupData { get; set; }

    public IDictionary ComplianceSettings { get; set; }

    public IDictionary AlertSettings { get; set; }

    public DateTimeType MilestoneDateTimeType { get; set; }

    public DateTimeType DocumentDateTimeType { get; set; }

    public LoanMigrationData MigrationData { get; set; }

    public RoleInfo[] AllRoles { get; set; }

    public bool Use10DigitLockSecondaryTradeFields { get; set; }

    public IMilestoneDateCalculator MilestoneDateCalculator { get; set; }

    public HMDAInformation HMDAInfo { get; set; }

    public DateTime DDMLastModifiedDateTime { get; set; }

    public bool EnableTempBuyDown { get; set; }

    public bool LoanAmountRounding { get; set; }

    public bool AllowHybridWithENoteClosing { get; set; }

    public bool Use5DecimalsForIndexRates { get; set; }

    public Func<string, Dictionary<string, string>> LoanExternalFields { get; set; }

    public RoleInfo GetRole(int roleID)
    {
      foreach (RoleInfo allRole in this.AllRoles)
      {
        if (allRole.RoleID == roleID)
          return allRole;
      }
      return RoleInfo.Others.ID == roleID ? RoleInfo.Others : (RoleInfo) null;
    }
  }
}
