// Decompiled with JetBrains decompiler
// Type: Export.Fannie32.HmdaExport.Borrowers.CoBorrower
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Export.Fannie32.HmdaExport.Borrowers
{
  internal class CoBorrower : Borrower
  {
    private readonly List<string> _fiedIdList = new List<string>()
    {
      "4197",
      "4198",
      "4248",
      "4200",
      "4199",
      "4213",
      "4214",
      "4246",
      "4215",
      "4159",
      "4160",
      "4161",
      "4162",
      "4136",
      "4206",
      "1532",
      "1533",
      "1534",
      "1535",
      "1536",
      "4247",
      "1538",
      "4253",
      "4163",
      "4164",
      "4165",
      "4166",
      "4167",
      "4168",
      "4169",
      "4170",
      "4171",
      "4172",
      "4173",
      "4139",
      "4141",
      "4137",
      "4132",
      "4134",
      "4133",
      "4131"
    };
    private int _asianSegment;
    private int _hawaiianSegment;
    private int _americanIndianOrAlaskaNative;

    public CoBorrower(string coBorrowerFirstName, string coBorrowerSsn)
    {
      this.BorrowerSsn = coBorrowerSsn;
      this.BorrowerFirstName = coBorrowerFirstName;
      this.FieldIdList = this._fiedIdList;
    }

    public override string GetGenderType()
    {
      string genderType = string.Empty;
      if (this.FieldIdValueDictionary["4197"].Equals("Y"))
        genderType = "Female";
      if (this.FieldIdValueDictionary["4198"].Equals("Y"))
        genderType = "Male";
      if (this.FieldIdValueDictionary["4248"].Equals("Y"))
        genderType = "InformationNotProvidedUnknown";
      if (this.FieldIdValueDictionary["4200"].Equals("Y"))
        genderType = "NotApplicable";
      if (this.FieldIdValueDictionary["4197"].Equals("Y") && this.FieldIdValueDictionary["4198"].Equals("Y"))
        genderType = "ApplicantSelectedBothMaleAndFemale";
      return genderType;
    }

    public override string GetGenderRefusalIndicator()
    {
      return !this.FieldIdValueDictionary["4199"].Equals("Y") ? "N" : "Y";
    }

    public override List<string> GetEthnicityType()
    {
      List<string> ethnicityType = new List<string>();
      if (this.FieldIdValueDictionary["4213"].Equals("Y"))
        ethnicityType.Add("HispanicOrLatino");
      if (this.FieldIdValueDictionary["4214"].Equals("Y"))
        ethnicityType.Add("NotHispanicOrLatino");
      if (this.FieldIdValueDictionary["4246"].Equals("Y"))
        ethnicityType.Add("InformationNotProvidedByApplicantInMIT");
      if (this.FieldIdValueDictionary["4215"].Equals("Y"))
        ethnicityType.Add("NotApplicable");
      return ethnicityType;
    }

    public override List<string> GetEthnicityOriginType()
    {
      List<string> ethnicityOriginType = new List<string>();
      if (this.FieldIdValueDictionary["4159"].Equals("Y"))
        ethnicityOriginType.Add("Mexican");
      if (this.FieldIdValueDictionary["4160"].Equals("Y"))
        ethnicityOriginType.Add("PuertoRican");
      if (this.FieldIdValueDictionary["4161"].Equals("Y"))
        ethnicityOriginType.Add("Cuban");
      if (this.FieldIdValueDictionary["4162"].Equals("Y"))
        ethnicityOriginType.Add("Other");
      return ethnicityOriginType;
    }

    public override List<string> GetEthnicityOriginTypeOtherDescription()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4136"]);
    }

    public override string GetEthnicityRefusalIndicator()
    {
      return !this.FieldIdValueDictionary["4206"].Equals("Y") ? "N" : "Y";
    }

    public override List<string> GetRaceType()
    {
      List<string> raceType = new List<string>();
      if (this.FieldIdValueDictionary["1532"].Equals("Y"))
      {
        this._americanIndianOrAlaskaNative = raceType.Count + 1;
        raceType.Add((raceType.Count + 1).ToString() + ":AmericanIndianOrAlaskaNative");
      }
      if (this.FieldIdValueDictionary["1533"].Equals("Y"))
      {
        this._asianSegment = raceType.Count + 1;
        raceType.Add(this._asianSegment.ToString() + ":Asian");
      }
      if (this.FieldIdValueDictionary["1534"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":BlackOrAfricanAmerican");
      if (this.FieldIdValueDictionary["1535"].Equals("Y"))
      {
        this._hawaiianSegment = raceType.Count + 1;
        raceType.Add(this._hawaiianSegment.ToString() + ":NativeHawaiianOrOtherPacificIslander");
      }
      if (this.FieldIdValueDictionary["1536"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":White");
      if (this.FieldIdValueDictionary["4247"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":InformationNotProvidedByApplicantInMIT");
      if (this.FieldIdValueDictionary["1538"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":NotApplicable");
      if (this._americanIndianOrAlaskaNative == 0)
        this._americanIndianOrAlaskaNative = raceType.Count + 1;
      if (this._asianSegment == 0)
        this._asianSegment = this._americanIndianOrAlaskaNative + 1;
      if (this._hawaiianSegment == 0)
        this._hawaiianSegment = this._asianSegment + 1;
      return raceType;
    }

    public override string GetRaceRefusalIndicator()
    {
      return !this.FieldIdValueDictionary["4253"].Equals("Y") ? "N" : "Y";
    }

    public override List<string> GetRaceDesignationType()
    {
      List<string> raceDesignationType = new List<string>();
      if (this.FieldIdValueDictionary["4163"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":AsianIndian");
      if (this.FieldIdValueDictionary["4164"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Chinese");
      if (this.FieldIdValueDictionary["4165"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Filipino");
      if (this.FieldIdValueDictionary["4166"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Japanese");
      if (this.FieldIdValueDictionary["4167"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Korean");
      if (this.FieldIdValueDictionary["4168"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Vietnamese");
      if (this.FieldIdValueDictionary["4169"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":OtherAsian");
      if (this.FieldIdValueDictionary["4170"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":NativeHawaiian");
      if (this.FieldIdValueDictionary["4171"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":GuamanianOrChamorro");
      if (this.FieldIdValueDictionary["4172"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":Samoan");
      if (this.FieldIdValueDictionary["4173"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":OtherPacificIslander");
      return raceDesignationType;
    }

    public override List<string> GetRaceDesignationOtherAsnDesc()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4139"], 38).Select<string, string>((Func<string, string>) (value => this._asianSegment.ToString() + ":" + value)).ToList<string>();
    }

    public override List<string> GetRaceDesignationOtherPiDesc()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4141"], 38).Select<string, string>((Func<string, string>) (value => this._hawaiianSegment.ToString() + ":" + value)).ToList<string>();
    }

    public override List<string> GetRaceTypeAdditionalDescription()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4137"], 38).Select<string, string>((Func<string, string>) (value => this._americanIndianOrAlaskaNative.ToString() + ":" + value)).ToList<string>();
    }

    public override string GetEthnicityCollectedBasedOnVisual()
    {
      return this.FieldIdValueDictionary["4132"];
    }

    public override string GetGenderCollectedBasedOnVisual() => this.FieldIdValueDictionary["4134"];

    public override string GetRaceCollectedBasedOnVisual() => this.FieldIdValueDictionary["4133"];

    public override string ApplicationTakenMethodType()
    {
      return string.IsNullOrWhiteSpace(this.FieldIdValueDictionary["4131"]) ? string.Empty : this.FieldIdValueDictionary["4131"];
    }
  }
}
