// Decompiled with JetBrains decompiler
// Type: Export.Fannie32.HmdaExport.Borrowers.PrimaryBorrower
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Export.Fannie32.HmdaExport.Borrowers
{
  internal class PrimaryBorrower : Borrower
  {
    private readonly List<string> _fiedIdList = new List<string>()
    {
      "4193",
      "4194",
      "4245",
      "4196",
      "4195",
      "4210",
      "4211",
      "4243",
      "4212",
      "4144",
      "4145",
      "4146",
      "4147",
      "4125",
      "4205",
      "1524",
      "1525",
      "1526",
      "1527",
      "1528",
      "4244",
      "1530",
      "4252",
      "4148",
      "4149",
      "4150",
      "4151",
      "4152",
      "4153",
      "4154",
      "4155",
      "4156",
      "4157",
      "4158",
      "4128",
      "4130",
      "4126",
      "4121",
      "4123",
      "4122",
      "4143"
    };
    private int _asianSegment;
    private int _hawaiianSegment;
    private int _americanIndianOrAlaskaNative;

    public PrimaryBorrower(string primaryBorrowerFirstName, string primaryBorrowerSsn)
    {
      this.BorrowerSsn = primaryBorrowerSsn;
      this.BorrowerFirstName = primaryBorrowerFirstName;
      this.FieldIdList = this._fiedIdList;
    }

    public override string GetGenderType()
    {
      string genderType = string.Empty;
      if (this.FieldIdValueDictionary["4193"].Equals("Y"))
        genderType = "Female";
      if (this.FieldIdValueDictionary["4194"].Equals("Y"))
        genderType = "Male";
      if (this.FieldIdValueDictionary["4245"].Equals("Y"))
        genderType = "InformationNotProvidedUnknown";
      if (this.FieldIdValueDictionary["4196"].Equals("Y"))
        genderType = "NotApplicable";
      if (this.FieldIdValueDictionary["4193"].Equals("Y") && this.FieldIdValueDictionary["4194"].Equals("Y"))
        genderType = "ApplicantSelectedBothMaleAndFemale";
      return genderType;
    }

    public override string GetGenderRefusalIndicator()
    {
      return !this.FieldIdValueDictionary["4195"].Equals("Y") ? "N" : "Y";
    }

    public override List<string> GetEthnicityType()
    {
      List<string> ethnicityType = new List<string>();
      if (this.FieldIdValueDictionary["4210"].Equals("Y"))
        ethnicityType.Add("HispanicOrLatino");
      if (this.FieldIdValueDictionary["4211"].Equals("Y"))
        ethnicityType.Add("NotHispanicOrLatino");
      if (this.FieldIdValueDictionary["4243"].Equals("Y"))
        ethnicityType.Add("InformationNotProvidedByApplicantInMIT");
      if (this.FieldIdValueDictionary["4212"].Equals("Y"))
        ethnicityType.Add("NotApplicable");
      return ethnicityType;
    }

    public override List<string> GetEthnicityOriginType()
    {
      List<string> ethnicityOriginType = new List<string>();
      if (this.FieldIdValueDictionary["4144"].Equals("Y"))
        ethnicityOriginType.Add("Mexican");
      if (this.FieldIdValueDictionary["4145"].Equals("Y"))
        ethnicityOriginType.Add("PuertoRican");
      if (this.FieldIdValueDictionary["4146"].Equals("Y"))
        ethnicityOriginType.Add("Cuban");
      if (this.FieldIdValueDictionary["4147"].Equals("Y"))
        ethnicityOriginType.Add("Other");
      return ethnicityOriginType;
    }

    public override List<string> GetEthnicityOriginTypeOtherDescription()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4125"]);
    }

    public override string GetEthnicityRefusalIndicator()
    {
      return !this.FieldIdValueDictionary["4205"].Equals("Y") ? "N" : "Y";
    }

    public override List<string> GetRaceType()
    {
      List<string> raceType = new List<string>();
      if (this.FieldIdValueDictionary["1524"].Equals("Y"))
      {
        this._americanIndianOrAlaskaNative = raceType.Count + 1;
        raceType.Add((raceType.Count + 1).ToString() + ":AmericanIndianOrAlaskaNative");
      }
      if (this.FieldIdValueDictionary["1525"].Equals("Y"))
      {
        this._asianSegment = raceType.Count + 1;
        raceType.Add(this._asianSegment.ToString() + ":Asian");
      }
      if (this.FieldIdValueDictionary["1526"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":BlackOrAfricanAmerican");
      if (this.FieldIdValueDictionary["1527"].Equals("Y"))
      {
        this._hawaiianSegment = raceType.Count + 1;
        raceType.Add(this._hawaiianSegment.ToString() + ":NativeHawaiianOrOtherPacificIslander");
      }
      if (this.FieldIdValueDictionary["1528"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":White");
      if (this.FieldIdValueDictionary["4244"].Equals("Y"))
        raceType.Add((raceType.Count + 1).ToString() + ":InformationNotProvidedByApplicantInMIT");
      if (this.FieldIdValueDictionary["1530"].Equals("Y"))
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
      return !this.FieldIdValueDictionary["4252"].Equals("Y") ? "N" : "Y";
    }

    public override List<string> GetRaceDesignationType()
    {
      List<string> raceDesignationType = new List<string>();
      if (this.FieldIdValueDictionary["4148"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":AsianIndian");
      if (this.FieldIdValueDictionary["4149"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Chinese");
      if (this.FieldIdValueDictionary["4150"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Filipino");
      if (this.FieldIdValueDictionary["4151"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Japanese");
      if (this.FieldIdValueDictionary["4152"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Korean");
      if (this.FieldIdValueDictionary["4153"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":Vietnamese");
      if (this.FieldIdValueDictionary["4154"].Equals("Y"))
        raceDesignationType.Add(this._asianSegment.ToString() + ":OtherAsian");
      if (this.FieldIdValueDictionary["4155"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":NativeHawaiian");
      if (this.FieldIdValueDictionary["4156"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":GuamanianOrChamorro");
      if (this.FieldIdValueDictionary["4157"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":Samoan");
      if (this.FieldIdValueDictionary["4158"].Equals("Y"))
        raceDesignationType.Add(this._hawaiianSegment.ToString() + ":OtherPacificIslander");
      return raceDesignationType;
    }

    public override List<string> GetRaceDesignationOtherAsnDesc()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4128"], 38).Select<string, string>((Func<string, string>) (value => this._asianSegment.ToString() + ":" + value)).ToList<string>();
    }

    public override List<string> GetRaceDesignationOtherPiDesc()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4130"], 38).Select<string, string>((Func<string, string>) (value => this._hawaiianSegment.ToString() + ":" + value)).ToList<string>();
    }

    public override List<string> GetRaceTypeAdditionalDescription()
    {
      return Borrower.SplitDescription(this.FieldIdValueDictionary["4126"], 38).Select<string, string>((Func<string, string>) (value => this._americanIndianOrAlaskaNative.ToString() + ":" + value)).ToList<string>();
    }

    public override string GetEthnicityCollectedBasedOnVisual()
    {
      return this.FieldIdValueDictionary["4121"];
    }

    public override string GetGenderCollectedBasedOnVisual() => this.FieldIdValueDictionary["4123"];

    public override string GetRaceCollectedBasedOnVisual() => this.FieldIdValueDictionary["4122"];

    public override string ApplicationTakenMethodType()
    {
      return string.IsNullOrWhiteSpace(this.FieldIdValueDictionary["4143"]) ? string.Empty : this.FieldIdValueDictionary["4143"];
    }
  }
}
