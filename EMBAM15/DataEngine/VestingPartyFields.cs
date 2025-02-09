// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.VestingPartyFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class VestingPartyFields
  {
    private static readonly string[] BorrowerFieldList = new string[13]
    {
      "1868",
      "1869",
      "65",
      "1871",
      null,
      "Borr.TrusteeOf",
      "1870",
      "Borr.POASigTxt",
      "1872",
      "2968",
      "1402",
      "1811",
      "Borr.OccupancyIntent"
    };
    private static readonly string[] CoBorrowerFieldList = new string[13]
    {
      "1873",
      "1874",
      "97",
      "1876",
      null,
      "CoBorr.TrusteeOf",
      "1875",
      "CoBorr.POASigTxt",
      "1877",
      "2969",
      "1403",
      "1811",
      "CoBorr.OccupancyIntent"
    };
    private static readonly string[] VestingPartyFieldList = new string[14]
    {
      "TR0001",
      "TR0002",
      "TR0003",
      "TR0004",
      "TR0005",
      "TR0006",
      "TR0007",
      "TR0011",
      "TR0008",
      "TR0009",
      "TR0012",
      "TR0013",
      "TR0014",
      "TR0099"
    };
    private string[] fieldList;
    private BorrowerPair pair;
    private VestingPartyType type;
    private int trusteeIndex = -1;

    private VestingPartyFields(string[] fieldList, BorrowerPair pair, VestingPartyType type)
    {
      this.fieldList = fieldList;
      this.pair = pair;
      this.type = type;
    }

    private VestingPartyFields(string[] fieldList, int trusteeIndex)
    {
      this.fieldList = fieldList;
      this.type = VestingPartyType.Other;
      this.trusteeIndex = trusteeIndex;
    }

    public VestingPartyType Type => this.type;

    public bool IsBorrowerOrCoborrower()
    {
      return this.type == VestingPartyType.Borrower || this.type == VestingPartyType.CoBorrower;
    }

    public int VestingPartyIndex => this.trusteeIndex;

    public BorrowerPair BorrowerPair => this.pair;

    public string NameField => this.fieldList[0];

    public string AliasField => this.fieldList[1];

    public string SSNField => this.fieldList[2];

    public string TypeField => this.fieldList[3];

    public string BorrowerPairField => this.fieldList[4];

    public string TrusteeOfField => this.fieldList[5];

    public string POAField => this.fieldList[6];

    public string POASignatureTextField => this.fieldList[7];

    public string VestingField => this.fieldList[8];

    public string AuthToSignField => this.fieldList[9];

    public string BorrowerDOB => this.fieldList.Length <= 10 ? "" : this.fieldList[10];

    public string OccupancyStatus => this.fieldList.Length <= 11 ? "" : this.fieldList[11];

    public string OccupancyIntent => this.fieldList.Length <= 12 ? "" : this.fieldList[12];

    public static VestingPartyFields GetBorrowerFields(BorrowerPair pair)
    {
      return new VestingPartyFields(VestingPartyFields.BorrowerFieldList, pair, VestingPartyType.Borrower);
    }

    public static VestingPartyFields GetCoBorrowerFields(BorrowerPair pair)
    {
      return new VestingPartyFields(VestingPartyFields.CoBorrowerFieldList, pair, VestingPartyType.CoBorrower);
    }

    public static VestingPartyFields GetAdditionalVestingPartyInstanceFields(int trusteeIndex)
    {
      string[] fieldList = new string[VestingPartyFields.VestingPartyFieldList.Length];
      for (int index = 0; index < fieldList.Length; ++index)
      {
        FieldDefinition field = (FieldDefinition) StandardFields.GetField(VestingPartyFields.VestingPartyFieldList[index]);
        fieldList[index] = field.CreateInstance((object) trusteeIndex).FieldID;
      }
      return new VestingPartyFields(fieldList, trusteeIndex);
    }

    public static bool IsBeneficiaryType(string partyType)
    {
      return partyType == "Trustee" || partyType == "Settlor Trustee" || partyType == "Title Only Trustee" || partyType == "Title Only Settlor Trustee";
    }

    public static bool IsTrusteeType(string partyType)
    {
      return partyType == "Trustee" || partyType == "Settlor Trustee" || partyType == "Title Only Trustee" || partyType == "Title Only Settlor Trustee";
    }

    public static bool IsNonTrusteeBorrower(string partyType)
    {
      return partyType != null && partyType.Length == 0 || partyType == "Individual" || partyType == "Title only";
    }

    public static bool IsCorporateOfficer(string partyType) => partyType == "Officer";
  }
}
