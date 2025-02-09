// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ShipInDarkValidation
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ShipInDarkValidation
  {
    public static List<string> URLA2020FormNames = (List<string>) null;
    private static List<string> URLA2020FormIDs = (List<string>) null;
    private static Dictionary<string, string> forms_URLA2020 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase)
    {
      {
        "D1003_2020P1",
        "1003 URLA Part 1"
      },
      {
        "D1003_2020P2",
        "1003 URLA Part 2"
      },
      {
        "D1003_2020P3",
        "1003 URLA Part 3"
      },
      {
        "D1003_2020P4",
        "1003 URLA Part 4"
      },
      {
        "D1003_2020P5",
        "1003 URLA - Lender"
      },
      {
        "D1003_2020P6",
        "1003 URLA Continuation"
      },
      {
        "VOOI",
        "Verification of Other Income"
      },
      {
        "VOOL",
        "Verification of Other Liability"
      },
      {
        "VOGG",
        "Verification of Gifts and Grants"
      },
      {
        "VOOA",
        "Verification of Other Assets"
      },
      {
        "VOAL",
        "Verification of Additional Loans"
      }
    };
    private static Dictionary<string, string> formIDLookup_URLATable = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase)
    {
      {
        "D10031",
        "D1003_2020P1"
      },
      {
        "D10032",
        "D1003_2020P2"
      },
      {
        "D10033",
        "D1003_2020P3"
      },
      {
        "D10034",
        "D1003_2020P4"
      }
    };

    static ShipInDarkValidation()
    {
      ShipInDarkValidation.URLA2020FormNames = new List<string>();
      ShipInDarkValidation.URLA2020FormIDs = new List<string>();
      foreach (KeyValuePair<string, string> keyValuePair in ShipInDarkValidation.forms_URLA2020)
      {
        ShipInDarkValidation.URLA2020FormNames.Add(keyValuePair.Value);
        ShipInDarkValidation.URLA2020FormIDs.Add(keyValuePair.Key);
      }
    }

    public static bool IsURLA2020Form(string formIDorName)
    {
      return ShipInDarkValidation.URLA2020FormIDs.Contains(formIDorName) || ShipInDarkValidation.URLA2020FormNames.Contains(formIDorName);
    }

    public static string GetNewURLAFormID(string oldFormID)
    {
      return ShipInDarkValidation.formIDLookup_URLATable.ContainsKey(oldFormID) ? ShipInDarkValidation.formIDLookup_URLATable[oldFormID] : (string) null;
    }

    public static bool IsURLA2020OutputForm(string formID)
    {
      return formID.StartsWith("2020 URLA (1003/65) -");
    }
  }
}
