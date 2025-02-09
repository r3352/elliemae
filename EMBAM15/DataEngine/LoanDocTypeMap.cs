// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDocTypeMap
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanDocTypeMap
  {
    private const int numOfCodes = 22;
    private const string letterCodes = "?ABCDEFGHIJKLMNOPQRSTU";
    public static string[] Descriptions = new string[22]
    {
      "",
      "Alternative",
      "Streamline Refinance",
      "No Documentation",
      "No Ratio",
      "Limited Documentation",
      "Full Documentation",
      "No Income and No Assets on 1003",
      "No Assets on 1003",
      "No Income and No Employment on 1003",
      "No Income on 1003",
      "No Verification of Stated Income, Employment, or Assets",
      "No Verification of Stated Income or Assets",
      "No Verification of Stated Assets",
      "No Verfication of Stated Income or Employment",
      "No Verification of Stated Income",
      "Verbal Verification of Employment(VVOE)",
      "One paystub",
      "Reduced",
      "One paystub and VVOE",
      "One paystub and one W-2 and VVOE or one year 1040",
      "No Income, No Employment, and No Assets on 1003"
    };
    public static string[] MismoCodes = new string[22]
    {
      "",
      "Alternative",
      "StreamlineRefinance",
      "NoDocumentation",
      "NoRatio",
      "LimitedDocumentation",
      "FullDocumentation",
      "NoDepositVerificationEmploymentVerificationOrIncomeVerification",
      "NoDepositVerification",
      "NoEmploymentVerificationOrIncomeVerification",
      "NoIncomeOn1003",
      "NoVerificationOfStatedIncomeEmploymentOrAssets",
      "NoVerificationOfStatedIncomeOrAssets",
      "NoVerificationOfStatedAssets",
      "NoVerificationOfStatedIncomeOrEmployment",
      "NoVerificationOfStatedIncome",
      "VerbalVerificationOfEmployment",
      "OnePaystub",
      "Reduced",
      "OnePaystubAndVerbalVerificationOfEmployment",
      "OnePaystubAndOneW2AndVerbalVerificationOfEmploymentOrOneYear1040",
      "NoIncomeNoEmploymentNoAssetsOn1003"
    };
    private static Hashtable mismoToCodeMap = new Hashtable();

    static LoanDocTypeMap()
    {
      LoanDocTypeMap.mismoToCodeMap[(object) "Alternative"] = (object) LoanDocTypeMap.Code.A;
      LoanDocTypeMap.mismoToCodeMap[(object) "StreamlineRefinance"] = (object) LoanDocTypeMap.Code.B;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoDocumentation"] = (object) LoanDocTypeMap.Code.C;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoRatio"] = (object) LoanDocTypeMap.Code.D;
      LoanDocTypeMap.mismoToCodeMap[(object) "LimitedDocumentation"] = (object) LoanDocTypeMap.Code.E;
      LoanDocTypeMap.mismoToCodeMap[(object) "FullDocumentation"] = (object) LoanDocTypeMap.Code.F;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoDepositVerificationEmploymentVerificationOrIncomeVerification"] = (object) LoanDocTypeMap.Code.G;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoDepositVerification"] = (object) LoanDocTypeMap.Code.H;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoEmploymentVerificationOrIncomeVerification"] = (object) LoanDocTypeMap.Code.I;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoIncomeOn1003"] = (object) LoanDocTypeMap.Code.J;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoVerificationOfStatedIncomeEmploymentOrAssets"] = (object) LoanDocTypeMap.Code.K;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoVerificationOfStatedIncomeOrAssets"] = (object) LoanDocTypeMap.Code.L;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoVerificationOfStatedAssets"] = (object) LoanDocTypeMap.Code.M;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoVerificationOfStatedIncomeOrEmployment"] = (object) LoanDocTypeMap.Code.N;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoVerificationOfStatedIncome"] = (object) LoanDocTypeMap.Code.O;
      LoanDocTypeMap.mismoToCodeMap[(object) "VerbalVerificationOfEmployment"] = (object) LoanDocTypeMap.Code.P;
      LoanDocTypeMap.mismoToCodeMap[(object) "OnePaystub"] = (object) LoanDocTypeMap.Code.Q;
      LoanDocTypeMap.mismoToCodeMap[(object) "Reduced"] = (object) LoanDocTypeMap.Code.R;
      LoanDocTypeMap.mismoToCodeMap[(object) "OnePaystubAndVerbalVerificationOfEmployment"] = (object) LoanDocTypeMap.Code.S;
      LoanDocTypeMap.mismoToCodeMap[(object) "OnePaystubAndOneW2AndVerbalVerificationOfEmploymentOrOneYear1040"] = (object) LoanDocTypeMap.Code.T;
      LoanDocTypeMap.mismoToCodeMap[(object) "NoIncomeNoEmploymentNoAssetsOn1003"] = (object) LoanDocTypeMap.Code.U;
    }

    private LoanDocTypeMap()
    {
    }

    public static LoanDocTypeMap.Code GetCodeForCodeString(string codeLetter)
    {
      LoanDocTypeMap.Code codeForCodeString = LoanDocTypeMap.Code.NotSpecified;
      switch (codeLetter.ToUpper())
      {
        case "A":
          codeForCodeString = LoanDocTypeMap.Code.A;
          break;
        case "B":
          codeForCodeString = LoanDocTypeMap.Code.B;
          break;
        case "C":
          codeForCodeString = LoanDocTypeMap.Code.C;
          break;
        case "D":
          codeForCodeString = LoanDocTypeMap.Code.D;
          break;
        case "E":
          codeForCodeString = LoanDocTypeMap.Code.E;
          break;
        case "F":
          codeForCodeString = LoanDocTypeMap.Code.F;
          break;
        case "G":
          codeForCodeString = LoanDocTypeMap.Code.G;
          break;
        case "H":
          codeForCodeString = LoanDocTypeMap.Code.H;
          break;
        case "I":
          codeForCodeString = LoanDocTypeMap.Code.I;
          break;
        case "J":
          codeForCodeString = LoanDocTypeMap.Code.J;
          break;
        case "K":
          codeForCodeString = LoanDocTypeMap.Code.K;
          break;
        case "L":
          codeForCodeString = LoanDocTypeMap.Code.L;
          break;
        case "M":
          codeForCodeString = LoanDocTypeMap.Code.M;
          break;
        case "N":
          codeForCodeString = LoanDocTypeMap.Code.N;
          break;
        case "O":
          codeForCodeString = LoanDocTypeMap.Code.O;
          break;
        case "P":
          codeForCodeString = LoanDocTypeMap.Code.P;
          break;
        case "Q":
          codeForCodeString = LoanDocTypeMap.Code.Q;
          break;
        case "R":
          codeForCodeString = LoanDocTypeMap.Code.R;
          break;
        case "S":
          codeForCodeString = LoanDocTypeMap.Code.S;
          break;
        case "T":
          codeForCodeString = LoanDocTypeMap.Code.T;
          break;
        case "U":
          codeForCodeString = LoanDocTypeMap.Code.U;
          break;
      }
      return codeForCodeString;
    }

    public static LoanDocTypeMap.Code GetCode(string mismoCode)
    {
      if ((mismoCode ?? "").Trim() == "")
        return LoanDocTypeMap.Code.NotSpecified;
      return LoanDocTypeMap.mismoToCodeMap[(object) mismoCode] == null ? LoanDocTypeMap.Code.Unknown : (LoanDocTypeMap.Code) LoanDocTypeMap.mismoToCodeMap[(object) mismoCode];
    }

    public static string GetLetterCode(LoanDocTypeMap.Code code)
    {
      return code == LoanDocTypeMap.Code.NotSpecified ? "" : "?ABCDEFGHIJKLMNOPQRSTU".Substring((int) code, 1);
    }

    public static string GetLetterCode(string mismoCode)
    {
      return LoanDocTypeMap.GetLetterCode(LoanDocTypeMap.GetCode(mismoCode));
    }

    public enum Code
    {
      Unknown = 0,
      A = 1,
      B = 2,
      C = 3,
      D = 4,
      E = 5,
      F = 6,
      G = 7,
      H = 8,
      I = 9,
      J = 10, // 0x0000000A
      K = 11, // 0x0000000B
      L = 12, // 0x0000000C
      M = 13, // 0x0000000D
      N = 14, // 0x0000000E
      O = 15, // 0x0000000F
      P = 16, // 0x00000010
      Q = 17, // 0x00000011
      R = 18, // 0x00000012
      S = 19, // 0x00000013
      T = 20, // 0x00000014
      U = 21, // 0x00000015
      NotSpecified = 100, // 0x00000064
    }
  }
}
