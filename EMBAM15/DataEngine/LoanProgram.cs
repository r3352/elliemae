// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanProgram
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanProgram : FieldDataTemplate
  {
    public static readonly string[] TemplateFields;
    private static Dictionary<string, string> legacyFieldMap = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Dictionary<string, string> loanCompFieldMap = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Dictionary<string, string> openingPlanCodeFieldMap = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private static Dictionary<string, FieldDefinition> lpFieldDefs = new Dictionary<string, FieldDefinition>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private LoanProgramFieldSettings fieldSettings;
    private static string[,] legacyFieldPairs = new string[123, 2]
    {
      {
        "LP01",
        "1172"
      },
      {
        "LP02",
        "1811"
      },
      {
        "LP03",
        "420"
      },
      {
        "LP05",
        "19"
      },
      {
        "LP07",
        "608"
      },
      {
        "LP08",
        "3"
      },
      {
        "LP09",
        "1014"
      },
      {
        "LP10",
        "4"
      },
      {
        "LP11",
        "325"
      },
      {
        "LP12",
        "1269"
      },
      {
        "LP13",
        "1613"
      },
      {
        "LP14",
        "1270"
      },
      {
        "LP15",
        "1614"
      },
      {
        "LP16",
        "1271"
      },
      {
        "LP17",
        "1615"
      },
      {
        "LP18",
        "1272"
      },
      {
        "LP19",
        "1616"
      },
      {
        "LP20",
        "1273"
      },
      {
        "LP21",
        "1617"
      },
      {
        "LP22",
        "1274"
      },
      {
        "LP23",
        "1618"
      },
      {
        "LP24",
        "423"
      },
      {
        "LP25",
        "1177"
      },
      {
        "LP26",
        "697"
      },
      {
        "LP27",
        "696"
      },
      {
        "LP28",
        "695"
      },
      {
        "LP29",
        "694"
      },
      {
        "LP30",
        "247"
      },
      {
        "LP31",
        "689"
      },
      {
        "LP32",
        "688"
      },
      {
        "LP33",
        "1699"
      },
      {
        "LP34",
        "1700"
      },
      {
        "LP35",
        "SYS.X1"
      },
      {
        "LP36",
        "691"
      },
      {
        "LP37",
        "690"
      },
      {
        "LP38",
        "1712"
      },
      {
        "LP39",
        "1713"
      },
      {
        "LP40",
        "698"
      },
      {
        "LP41",
        "1266"
      },
      {
        "LP42",
        "1267"
      },
      {
        "LP43",
        "312"
      },
      {
        "LP44",
        "1401"
      },
      {
        "LP45",
        "PREQUAL.X205"
      },
      {
        "LP46",
        "PREQUAL.X209"
      },
      {
        "LP47",
        "PREQUAL.X210"
      },
      {
        "LP48",
        "1484"
      },
      {
        "LP49",
        "1790"
      },
      {
        "LP50",
        "1791"
      },
      {
        "LP53",
        "1176"
      },
      {
        "LP54",
        "1677"
      },
      {
        "LP55",
        "1265"
      },
      {
        "LP56",
        "1404"
      },
      {
        "LP57",
        "1199"
      },
      {
        "LP58",
        "1198"
      },
      {
        "LP59",
        "1766"
      },
      {
        "LP60",
        "1201"
      },
      {
        "LP61",
        "1200"
      },
      {
        "LP62",
        "1770"
      },
      {
        "LP63",
        "1205"
      },
      {
        "LP64",
        "664"
      },
      {
        "LP65",
        "663"
      },
      {
        "LP66",
        "665"
      },
      {
        "LP67",
        "666"
      },
      {
        "LP68",
        "1603"
      },
      {
        "LP69",
        "671"
      },
      {
        "LP70",
        "675"
      },
      {
        "LP71",
        "670"
      },
      {
        "LP72",
        "672"
      },
      {
        "LP73",
        "674"
      },
      {
        "LP74",
        "1719"
      },
      {
        "LP75",
        "677"
      },
      {
        "LP79",
        "1290"
      },
      {
        "LP80",
        "1130"
      },
      {
        "LP81",
        "680"
      },
      {
        "LP82",
        "679"
      },
      {
        "LP83",
        "1005"
      },
      {
        "LP84",
        "1487"
      },
      {
        "LP85",
        "1063"
      },
      {
        "LP86",
        "995"
      },
      {
        "LP87",
        "994"
      },
      {
        "LP88",
        "1702"
      },
      {
        "LP89",
        "1697"
      },
      {
        "LP90",
        "1698"
      },
      {
        "LP91",
        "1242"
      },
      {
        "LP92",
        "676"
      },
      {
        "LP93",
        "1708"
      },
      {
        "LP94",
        "1709"
      },
      {
        "LP95",
        "1710"
      },
      {
        "LP96",
        "9"
      },
      {
        "LP97",
        "1785"
      },
      {
        "LP98",
        "1753"
      },
      {
        "LP99",
        "1775"
      },
      {
        "LP100",
        "1757"
      },
      {
        "LP101",
        "1107"
      },
      {
        "LP102",
        "1765"
      },
      {
        "LP103",
        "1760"
      },
      {
        "LP104",
        "969"
      },
      {
        "LP105",
        "SYS.X2"
      },
      {
        "LP106",
        "1853"
      },
      {
        "LP107",
        "MORNET.X67"
      },
      {
        "LP108",
        "723"
      },
      {
        "LP110",
        "1959"
      },
      {
        "LP109",
        "8"
      },
      {
        "LP111",
        "1962"
      },
      {
        "LP112",
        "SYS.X6"
      },
      {
        "LP113",
        "1964"
      },
      {
        "LP114",
        "1888"
      },
      {
        "LP115",
        "1891"
      },
      {
        "LP116",
        "1482"
      },
      {
        "LP117",
        "1965"
      },
      {
        "LP118",
        "1966"
      },
      {
        "LP119",
        "1892"
      },
      {
        "LP120",
        "1893"
      },
      {
        "LP121",
        "1413"
      },
      {
        "LP122",
        "1483"
      },
      {
        "LP123",
        "1967"
      },
      {
        "LP124",
        "1968"
      },
      {
        "LP126",
        "1986"
      },
      {
        "LP127",
        "1987"
      },
      {
        "LP128",
        "3248"
      },
      {
        "LP129",
        "LE2.X28"
      },
      {
        "LP130",
        "CD4.X2"
      },
      {
        "LP131",
        "CD4.X3"
      }
    };
    public static string[,] LockRequestFieldMap = new string[12, 2]
    {
      {
        "MORNET.X67",
        "2867"
      },
      {
        "1484",
        "2940"
      },
      {
        "1811",
        "2950"
      },
      {
        "19",
        "2951"
      },
      {
        "1172",
        "2952"
      },
      {
        "608",
        "2953"
      },
      {
        "995",
        "2956"
      },
      {
        "994",
        "2957"
      },
      {
        "420",
        "2958"
      },
      {
        "4",
        "2959"
      },
      {
        "325",
        "2960"
      },
      {
        "1130",
        "3041"
      }
    };
    public static string[,] OpeningClosingFieldMap = new string[7, 2]
    {
      {
        "Opening.PlanID",
        "PlanCode.ID"
      },
      {
        "DISCLOSUREPLANCODE",
        "1881"
      },
      {
        "Opening.PlanDesc",
        "PlanCode.Desc"
      },
      {
        "Opening.LoanProgTyp",
        "PlanCode.LoanProgTyp"
      },
      {
        "Opening.ProgCd",
        "PlanCode.ProgCd"
      },
      {
        "VEND.X263",
        "PlanCode.ProgSpnsrNm"
      },
      {
        "Opening.InvCd",
        "PlanCode.InvCd"
      }
    };
    public static readonly string[] PlanCodeMetadataFields = new string[7]
    {
      "PlanCode.ID",
      "1881",
      "PlanCode.Desc",
      "PlanCode.LoanProgTyp",
      "PlanCode.ProgCd",
      "PlanCode.ProgSpnsrNm",
      "PlanCode.InvCd"
    };
    public static readonly string[] PlanCodeFields = new string[45]
    {
      "995",
      "1172",
      "Terms.USDAGovtType",
      "3314",
      "420",
      "608",
      "4",
      "325",
      "1659",
      "677",
      "663",
      "423",
      "SYS.X2",
      "PlanCode.AdtlSgVbgTyp",
      "Terms.IntrOnly",
      "1177",
      "697",
      "696",
      "695",
      "694",
      "247",
      "ARM.ApplyLfCpLow",
      "ARM.FlrBasis",
      "ARM.FlrVerbgTyp",
      "1700",
      "SYS.X1",
      "ARM.IdxLkbckPrd",
      "696",
      "1290",
      "CnvrOpt.FeeAmt",
      "CnvrOpt.FeePct",
      "CnvrOpt.MaxRateAdj",
      "CnvrOpt.MinRateAdj",
      "LE1.X96",
      "LE1.X97",
      "675",
      "Terms.PrepyVrbgTyp",
      "1266",
      "1267",
      "Terms.GPMPmtIncr",
      "SYS.X6",
      "1962",
      "19",
      "Constr.FstIntChgAdj",
      "4793"
    };
    public static readonly string[] NonPlanCodeFields = new string[239]
    {
      "9",
      "1811",
      "1005",
      "1487",
      "1063",
      "994",
      "3",
      "1014",
      "1269",
      "1613",
      "1270",
      "1614",
      "1271",
      "1615",
      "1272",
      "1616",
      "1273",
      "1617",
      "1274",
      "1618",
      "1853",
      "670",
      "1176",
      "1677",
      "1265",
      "1404",
      "688",
      "689",
      "1699",
      "1959",
      "691",
      "690",
      "1712",
      "1713",
      "698",
      "1199",
      "1198",
      "1201",
      "1200",
      "1205",
      "1209",
      "2978",
      "1107",
      "1826",
      "1765",
      "1760",
      "1757",
      "1198",
      "1199",
      "1200",
      "1201",
      "1205",
      "3248",
      "1775",
      "1753",
      "3531",
      "3532",
      "3533",
      "3625",
      "3262",
      "SYS.X11",
      "1484",
      "PREQUAL.X205",
      "PREQUAL.X209",
      "PREQUAL.X210",
      "1790",
      "1791",
      "1130",
      "664",
      "663",
      "665",
      "666",
      "1702",
      "1698",
      "1697",
      "676",
      "1242",
      "723",
      "8",
      "1708",
      "1709",
      "1710",
      "672",
      "674",
      "1719",
      "2831",
      "2832",
      "1854",
      "677",
      "679",
      "680",
      "1401",
      "1785",
      "MORNET.X67",
      "LP125",
      "3560",
      "3566",
      "LE2.X28",
      "CD4.X2",
      "CD4.X3",
      "CD4.X42",
      "CD4.X43",
      "4689",
      "4746",
      "4747",
      "4748",
      "4749",
      "4985",
      "4595",
      "4596",
      "4597",
      "4599",
      "4602",
      "4600",
      "4601",
      "4603",
      "4604",
      "4605",
      "4606",
      "4607",
      "4608",
      "4609",
      "4610",
      "4611",
      "4614",
      "4615",
      "4617",
      "4618",
      "4619",
      "4620",
      "4621",
      "4622",
      "4623",
      "4624",
      "4625",
      "4626",
      "4627",
      "4628",
      "4612",
      "4613",
      "4616",
      "4592",
      "4593",
      "4594",
      "1891",
      "4588",
      "4590",
      "4591",
      "1965",
      "1966",
      "4589",
      "1986",
      "1987",
      "HELOC.WireFee",
      "HELOC.StopPmtChrg",
      "HELOC.OvrLmtChg",
      "HELOC.OvrLmtRtnChg",
      "HELOC.RlsRecgChg",
      "HELOC.TransactionFees",
      "HELOC.RtdChkChgAmt",
      "HELOC.RtdChkChgMax",
      "HELOC.RtdChkChgMin",
      "HELOC.RtdChkChgRat",
      "HELOC.ParticipationFees",
      "4577",
      "4578",
      "4579",
      "4581",
      "4582",
      "4580",
      "4583",
      "4584",
      "4585",
      "4573",
      "4574",
      "4575",
      "HELOC.MinRepmtPct",
      "1890",
      "4576",
      "4569",
      "4564",
      "4565",
      "HELOC.MinAdvPct",
      "4567",
      "1889",
      "4560",
      "4475",
      "4530",
      "4476",
      "4477",
      "4478",
      "4479",
      "4480",
      "4481",
      "4482",
      "1483",
      "4483",
      "4484",
      "4485",
      "4464",
      "4531",
      "4465",
      "4466",
      "4467",
      "4468",
      "4469",
      "4470",
      "4471",
      "4491",
      "4474",
      "4473",
      "4472",
      "4549",
      "4551",
      "4552",
      "4550",
      "4556",
      "4557",
      "HELOC.MinPmtUPB",
      "1482",
      "4492",
      "1892",
      "4553",
      "4554",
      "4555",
      "1413",
      "1893",
      "1967",
      "1968",
      "HELOC.MinPmtUnpdBalAmt",
      "HELOC.MinPmtLessThanAmt",
      "4630",
      "1985",
      "4586",
      "4587",
      "4665",
      "4671",
      "4661",
      "4913"
    };

    static LoanProgram()
    {
      List<string> stringList = new List<string>();
      foreach (string codeMetadataField in LoanProgram.PlanCodeMetadataFields)
        stringList.Add(codeMetadataField);
      foreach (string planCodeField in LoanProgram.PlanCodeFields)
        stringList.Add(planCodeField);
      foreach (string nonPlanCodeField in LoanProgram.NonPlanCodeFields)
        stringList.Add(nonPlanCodeField);
      LoanProgram.TemplateFields = stringList.ToArray();
      for (int index = 0; index < LoanProgram.legacyFieldPairs.GetLength(0); ++index)
      {
        LoanProgram.legacyFieldMap[LoanProgram.legacyFieldPairs[index, 0]] = LoanProgram.legacyFieldPairs[index, 1];
        LoanProgram.loanCompFieldMap[LoanProgram.legacyFieldPairs[index, 1]] = LoanProgram.legacyFieldPairs[index, 0];
      }
      foreach (string templateField in LoanProgram.TemplateFields)
        LoanProgram.lpFieldDefs[templateField] = (FieldDefinition) StandardFields.GetField(templateField);
      for (int index = 0; index < LoanProgram.OpeningClosingFieldMap.GetLength(0); ++index)
        LoanProgram.openingPlanCodeFieldMap[LoanProgram.OpeningClosingFieldMap[index, 1]] = LoanProgram.OpeningClosingFieldMap[index, 0];
    }

    public LoanProgram()
    {
    }

    public LoanProgram(XmlSerializationInfo info)
      : base(info)
    {
      JedVersion jedVersion = new JedVersion(1, 0, 0);
      try
      {
        jedVersion = JedVersion.Parse(info.GetString("Version"));
      }
      catch
      {
      }
      if (jedVersion < JedVersion.Parse("7.0.0"))
      {
        foreach (string key in new List<string>((IEnumerable<string>) this.FieldData.Keys))
        {
          if (LoanProgram.legacyFieldMap.ContainsKey(key))
          {
            if (LoanProgram.lpFieldDefs.ContainsKey(LoanProgram.legacyFieldMap[key]))
              this.FieldData[LoanProgram.legacyFieldMap[key]] = this.FieldData[key];
            this.FieldData.Remove(key);
          }
        }
        base.TemplateName = this.GetSimpleField("1401");
      }
      this.CalculateAll();
    }

    public override string TemplateName
    {
      get => base.TemplateName;
      set
      {
        base.TemplateName = value;
        this.SetField("1401", value);
      }
    }

    public string PlanID
    {
      get => this.GetSimpleField("PlanCode.ID");
      set => this.SetField("PlanCode.ID", value);
    }

    public string ClosingCostPath
    {
      get => this.GetSimpleField("1785");
      set => this.SetField("1785", value);
    }

    public string PlanDescription
    {
      get => this.GetSimpleField("PlanCode.Desc");
      set => this.SetField("PlanCode.Desc", value);
    }

    public string PlanInvestor
    {
      get => this.GetSimpleField("PlanCode.ProgSpnsrNm");
      set => this.SetField("PlanCode.ProgSpnsrNm", value);
    }

    public string PlanCode
    {
      get => this.GetSimpleField("1881");
      set => this.SetField("1881", value);
    }

    public DocumentOrderType DocumentOrderType
    {
      get
      {
        string simpleField = this.GetSimpleField("PlanCode.LoanProgTyp");
        if (simpleField == "Opening")
          return DocumentOrderType.Opening;
        return simpleField != "" ? DocumentOrderType.Closing : DocumentOrderType.None;
      }
      set
      {
        if (value == DocumentOrderType.Opening)
          this.SetField("PlanCode.LoanProgTyp", "Opening");
        else if (value == DocumentOrderType.Closing)
        {
          this.SetField("PlanCode.LoanProgTyp", "Closing");
        }
        else
        {
          if (value != DocumentOrderType.None)
            throw new ArgumentException("The specified document order type is invalid");
          this.SetField("PlanCode.LoanProgTyp", "");
        }
      }
    }

    public void ClearPlan()
    {
      foreach (string codeMetadataField in LoanProgram.PlanCodeMetadataFields)
        this.SetField(codeMetadataField, "");
    }

    public bool IsLinkedToDocEnginePlan => this.DocumentOrderType != 0;

    public override void SetField(string id, string val)
    {
      if (LoanProgram.legacyFieldMap.ContainsKey(id))
        base.SetField(LoanProgram.legacyFieldMap[id], val);
      else
        base.SetField(id, val);
      this.executeCalcs(id, val);
    }

    public override string ToLoanFieldID(string templateFieldID)
    {
      return this.DocumentOrderType == DocumentOrderType.Opening ? this.mapFieldIDForOpening(templateFieldID) : templateFieldID;
    }

    public override bool AlwaysApply(string templateFieldID)
    {
      return this.IsLinkedToDocEnginePlan && LoanProgram.IsPlanCodeMetadataField(templateFieldID) || base.AlwaysApply(templateFieldID);
    }

    public static bool IsPlanCodeMetadataField(string fieldId)
    {
      return Array.Exists<string>(LoanProgram.PlanCodeMetadataFields, (Predicate<string>) new StringPredicate(fieldId, true));
    }

    private string mapFieldIDForOpening(string fieldID)
    {
      return LoanProgram.openingPlanCodeFieldMap.ContainsKey(fieldID) ? LoanProgram.openingPlanCodeFieldMap[fieldID] : fieldID;
    }

    public override string[] GetAllowedFieldIDs()
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) LoanProgram.TemplateFields);
      if (this.fieldSettings != null && this.fieldSettings.LoanProgramAdditionalFields != null)
        stringList.AddRange((IEnumerable<string>) this.fieldSettings.LoanProgramAdditionalFields);
      return stringList.ToArray();
    }

    public LoanProgramFieldSettings FieldSettings => this.fieldSettings;

    public void ApplyFieldSettings(LoanProgramFieldSettings settings)
    {
      this.fieldSettings = settings;
    }

    public override FieldDefinition GetFieldDefinition(string id)
    {
      if (LoanProgram.legacyFieldMap.ContainsKey(id) && LoanProgram.lpFieldDefs.ContainsKey(LoanProgram.legacyFieldMap[id]))
        return LoanProgram.lpFieldDefs[LoanProgram.legacyFieldMap[id]];
      if (LoanProgram.lpFieldDefs.ContainsKey(id))
        return LoanProgram.lpFieldDefs[id];
      return this.fieldSettings != null && this.fieldSettings.IsLPAdditionalField(id) ? EncompassFields.GetField(id, (EllieMae.EMLite.DataEngine.FieldSettings) this.fieldSettings) : EncompassFields.GetField(id, (EllieMae.EMLite.DataEngine.FieldSettings) this.fieldSettings) ?? (FieldDefinition) null;
    }

    public override string GetSimpleField(string id)
    {
      return LoanProgram.legacyFieldMap.ContainsKey(id) ? base.GetSimpleField(LoanProgram.legacyFieldMap[id]) : base.GetSimpleField(id);
    }

    public override Hashtable GetProperties()
    {
      Hashtable properties = base.GetProperties();
      properties.Add((object) "PlanID", (object) this.PlanID);
      properties.Add((object) "PlanCode", (object) this.PlanCode);
      properties.Add((object) "PlanDescription", (object) this.PlanDescription);
      properties.Add((object) "PlanInvestor", (object) this.PlanInvestor);
      properties.Add((object) "LinkedToPlan", this.IsLinkedToDocEnginePlan ? (object) "Y" : (object) "N");
      if (this.DocumentOrderType == DocumentOrderType.Opening)
        properties.Add((object) "PlanType", (object) "eDisclosures");
      else if (this.DocumentOrderType == DocumentOrderType.Closing)
        properties.Add((object) "PlanType", (object) "Closing Docs");
      else
        properties.Add((object) "PlanType", (object) "");
      return properties;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Version", (object) "7.0.0");
      base.GetXmlObjectData(info);
    }

    public static explicit operator LoanProgram(BinaryObject obj)
    {
      return (LoanProgram) BinaryConvertibleObject.Parse(obj, typeof (LoanProgram));
    }

    public static string GetLoanComparisonFieldID(string loanFieldId, int comparisonIndex)
    {
      return !LoanProgram.loanCompFieldMap.ContainsKey(loanFieldId) ? "" : LoanProgram.loanCompFieldMap[loanFieldId].Insert(2, comparisonIndex.ToString("00"));
    }

    private void executeCalcs(string id, string val)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 350953279:
          if (!(id == "19"))
            return;
          if (val != "Other")
          {
            if (val == "ConstructionOnly" && this.GetField("Constr.Refi") != "Y" && this.GetField("1964") != "Y")
              this.SetField("9", "Construction Only");
            else
              this.SetField("9", "");
          }
          this.calculateBalloonIndicator();
          return;
        case 822911587:
          if (!(id == "4"))
            return;
          break;
        case 906799682:
          if (!(id == "3"))
            return;
          break;
        case 1045129471:
          if (!(id == "325"))
            return;
          break;
        case 2031986902:
          if (!(id == "1172"))
            return;
          if (val != "FarmersHomeAdministration")
            this.SetField("Terms.USDAGovtType", "");
          if (val == "FarmersHomeAdministration")
          {
            this.SetField("1063", "RHS");
            return;
          }
          if (!(val != "Other"))
            return;
          this.SetField("1063", "");
          return;
        case 2061624233:
          if (!(id == "4475"))
            return;
          switch (val)
          {
            case "Fraction of Balance":
            case "Percentage of Balance":
              this.SetField("4530", val);
              this.SetField("4475", string.Empty);
              return;
            case "Rate":
              if (this.GetField("4479") == "Y")
              {
                this.SetField("4530", string.Empty);
                return;
              }
              break;
          }
          if (!(val == string.Empty))
            return;
          this.SetField("4476", string.Empty);
          this.SetField("4477", string.Empty);
          this.SetField("4478", string.Empty);
          this.SetField("4479", "");
          return;
        case 2078548947:
          if (!(id == "4468") || !(this.GetField("4464") == "Rate") || !(val != "Y"))
            return;
          this.SetField("4531", string.Empty);
          return;
        case 2115874997:
          if (!(id == "1177"))
            return;
          this.SetField("Terms.IntrOnly", Utils.ParseInt((object) val) > 0 ? "Y" : "");
          return;
        case 2157693625:
          if (!(id == "608") || !(val != "OtherAmortizationType"))
            return;
          this.SetField("994", "");
          return;
        case 2262955661:
          if (!(id == "4479") || !(this.GetField("4475") == "Rate") || !(val == "Y"))
            return;
          this.SetField("4530", string.Empty);
          return;
        case 2279880375:
          if (!(id == "4464"))
            return;
          switch (val)
          {
            case "Fraction of Balance":
            case "Percentage of Balance":
              this.SetField("4531", val);
              return;
            case "Rate":
              if (this.GetField("4468") != "Y")
              {
                this.SetField("4531", string.Empty);
                return;
              }
              break;
          }
          if (!(val == string.Empty))
            return;
          this.SetField("4465", string.Empty);
          this.SetField("4466", string.Empty);
          this.SetField("4467", string.Empty);
          this.SetField("4468", "");
          return;
        case 2320489014:
          if (!(id == "4531"))
            return;
          if ((val == "Fraction of Balance" || val == "Percentage of Balance") && this.GetField("4468") != "Y")
            this.SetField("4464", string.Empty);
          if (val == string.Empty || val == "Fraction of Balance")
            this.SetField("4471", string.Empty);
          if (!(val == string.Empty) && !(val == "Percentage of Balance"))
            return;
          this.SetField("4469", string.Empty);
          this.SetField("4470", string.Empty);
          return;
        case 2337266633:
          if (!(id == "4530"))
            return;
          if ((val == "Fraction of Balance" || val == "Percentage of Balance") && this.GetField("4479") == "Y")
            this.SetField("4475", string.Empty);
          if (val == string.Empty || val == "Fraction of Balance")
            this.SetField("4482", string.Empty);
          if (!(val == string.Empty) && !(val == "Percentage of Balance"))
            return;
          this.SetField("4480", string.Empty);
          this.SetField("4481", string.Empty);
          return;
        case 3240857578:
          if (!(id == "4746"))
            return;
          break;
        default:
          return;
      }
      this.calculateBalloonIndicator();
    }

    public void CalculateAll()
    {
      foreach (string templateField in LoanProgram.TemplateFields)
        this.executeCalcs(templateField, this.GetSimpleField(templateField));
    }

    public void ClearMIPData()
    {
      this.SetField("1766", "");
      this.SetField("1760", "");
      this.SetField("1107", "");
      this.SetField("1199", "");
      this.SetField("1198", "");
      this.SetField("1205", "");
      this.SetField("1200", "");
      this.SetField("1201", "");
      this.SetField("1765", "");
      this.SetField("3531", "");
      this.SetField("3532", "");
      this.SetField("3533", "");
      this.SetField("3625", "");
      this.SetField("3262", "");
      this.SetField("SYS.X11", "");
      for (int index = 3560; index <= 3566; ++index)
        this.SetField(string.Concat((object) index), "");
      if (!(this.GetField("1172") == "FHA"))
        return;
      this.SetField("1209", "");
      this.SetField("2978", "");
    }

    private void calculateBalloonIndicator()
    {
      string field = this.GetField("3");
      if (!string.IsNullOrEmpty(field) && Utils.ParseDouble((object) field) == 0.0 && this.GetField("4746") == "NoPaymentwithBalloon")
        this.SetField("1659", "Y");
      else if (this.GetField("19") == "ConstructionOnly")
      {
        this.SetField("1659", "Y");
      }
      else
      {
        Decimal num1 = Utils.ParseDecimal((object) this.GetField("4"));
        Decimal num2 = Utils.ParseDecimal((object) this.GetField("325"));
        if (num2 > 0M && num2 < num1)
          this.SetField("1659", "Y");
        else
          this.SetField("1659", "N");
      }
    }

    public static string GetMappingFieldIdForLpField(string lpField)
    {
      return LoanProgram.legacyFieldMap[lpField.ToUpper()];
    }
  }
}
