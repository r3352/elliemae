// Decompiled with JetBrains decompiler
// Type: Export.Fannie32.HmdaFacade
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using Export.Fannie32.HmdaExport.Borrowers;
using Export.Fannie32.HmdaExport.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Export.Fannie32
{
  internal class HmdaFacade
  {
    private readonly Encompass.Export.Fannie32 _fannie32;
    private readonly Borrower _primaryBorrower;
    private readonly Borrower _coBorrower;

    public HmdaFacade(
      string primaryBorrowerFirstName,
      string primaryBorrowerSsn,
      string coBorrowerFirstName,
      string coBorrowerSsn,
      Encompass.Export.Fannie32 fannie32)
    {
      this._fannie32 = fannie32;
      this._primaryBorrower = HmdaFacade.LoadFactory(true).CreateBorrower(primaryBorrowerFirstName, primaryBorrowerSsn);
      this._coBorrower = HmdaFacade.LoadFactory(false).CreateBorrower(coBorrowerFirstName, coBorrowerSsn);
    }

    private static IBorrowerFactory LoadFactory(bool isPrimary)
    {
      return !isPrimary ? (IBorrowerFactory) new CoBorrowerFactory() : (IBorrowerFactory) new PrimaryBorrowerFactory();
    }

    public List<Tuple<string, string>> GetHmdaDataPointsDictionary()
    {
      this._primaryBorrower.FieldIdValueDictionary = this.LoadHmdaFieldsValues(this._primaryBorrower.FieldIdList);
      List<Tuple<string, string>> hmdatDataPoints1 = HmdaFacade.GetHmdatDataPoints(this._primaryBorrower);
      this._coBorrower.FieldIdValueDictionary = this.LoadHmdaFieldsValues(this._coBorrower.FieldIdList);
      List<Tuple<string, string>> hmdatDataPoints2 = HmdaFacade.GetHmdatDataPoints(this._coBorrower);
      List<Tuple<string, string>> pointsDictionary = new List<Tuple<string, string>>();
      pointsDictionary.AddRange((IEnumerable<Tuple<string, string>>) hmdatDataPoints1);
      pointsDictionary.AddRange((IEnumerable<Tuple<string, string>>) hmdatDataPoints2);
      return pointsDictionary;
    }

    private static List<Tuple<string, string>> GetHmdatDataPoints(Borrower borrower)
    {
      List<Tuple<string, string>> hmdatDataPoints = new List<Tuple<string, string>>();
      if (string.IsNullOrWhiteSpace(borrower.BorrowerSsn))
        return hmdatDataPoints;
      if (!string.IsNullOrWhiteSpace(borrower.GetGenderType()))
        hmdatDataPoints.Add(new Tuple<string, string>("HMDAGenderType", borrower.BorrowerSsn + ":" + borrower.GetGenderType()));
      if (!string.IsNullOrWhiteSpace(borrower.GetGenderRefusalIndicator()))
        hmdatDataPoints.Add(new Tuple<string, string>("HMDAGenderRefusalIndicator", borrower.BorrowerSsn + ":" + borrower.GetGenderRefusalIndicator()));
      List<string> ethnicityType = borrower.GetEthnicityType();
      hmdatDataPoints.AddRange(ethnicityType.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (ethnicity => new Tuple<string, string>("HMDAEthnicityType", borrower.BorrowerSsn + ":" + ethnicity))));
      List<string> ethnicityOriginType = borrower.GetEthnicityOriginType();
      hmdatDataPoints.AddRange(ethnicityOriginType.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDAEthnicityOriginType", borrower.BorrowerSsn + ":" + variable))));
      List<string> otherDescription = borrower.GetEthnicityOriginTypeOtherDescription();
      hmdatDataPoints.AddRange(otherDescription.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDAEthnicityOriginTypeOtherDesc", borrower.BorrowerSsn + ":" + variable))));
      if (!string.IsNullOrWhiteSpace(borrower.GetEthnicityRefusalIndicator()))
        hmdatDataPoints.Add(new Tuple<string, string>("HMDAEthnicityRefusalIndicator", borrower.BorrowerSsn + ":" + borrower.GetEthnicityRefusalIndicator()));
      List<string> raceType = borrower.GetRaceType();
      hmdatDataPoints.AddRange(raceType.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDARaceType", borrower.BorrowerSsn + ":" + variable))));
      if (!string.IsNullOrWhiteSpace(borrower.GetRaceRefusalIndicator()))
        hmdatDataPoints.Add(new Tuple<string, string>("HMDARaceRefusalIndicator", borrower.BorrowerSsn + ":" + borrower.GetRaceRefusalIndicator()));
      List<string> raceDesignationType = borrower.GetRaceDesignationType();
      hmdatDataPoints.AddRange(raceDesignationType.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDARaceDesignationType", borrower.BorrowerSsn + ":" + variable))));
      List<string> additionalDescription = borrower.GetRaceTypeAdditionalDescription();
      hmdatDataPoints.AddRange(additionalDescription.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDARaceTypeAdditionalDescription", borrower.BorrowerSsn + ":" + variable))));
      List<string> designationOtherAsnDesc = borrower.GetRaceDesignationOtherAsnDesc();
      hmdatDataPoints.AddRange(designationOtherAsnDesc.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDARaceDesignationOtherAsnDesc", borrower.BorrowerSsn + ":" + variable))));
      List<string> designationOtherPiDesc = borrower.GetRaceDesignationOtherPiDesc();
      hmdatDataPoints.AddRange(designationOtherPiDesc.Select<string, Tuple<string, string>>((Func<string, Tuple<string, string>>) (variable => new Tuple<string, string>("HMDARaceDesignationOtherPIDesc", borrower.BorrowerSsn + ":" + variable))));
      string str = borrower.ApplicationTakenMethodType();
      if (!string.IsNullOrWhiteSpace(str) && str.ToLower().Equals("facetoface"))
      {
        hmdatDataPoints.Add(new Tuple<string, string>("HMDAEthnicityCollectedBasedOnVisual", borrower.BorrowerSsn + ":" + HmdaFacade.GetCollectedValueBasedOnVisual(borrower.GetEthnicityCollectedBasedOnVisual())));
        hmdatDataPoints.Add(new Tuple<string, string>("HMDAGenderCollectedBasedOnVisual", borrower.BorrowerSsn + ":" + HmdaFacade.GetCollectedValueBasedOnVisual(borrower.GetGenderCollectedBasedOnVisual())));
        hmdatDataPoints.Add(new Tuple<string, string>("HMDARaceCollectedBasedOnVisual", borrower.BorrowerSsn + ":" + HmdaFacade.GetCollectedValueBasedOnVisual(borrower.GetRaceCollectedBasedOnVisual())));
      }
      if (!string.IsNullOrWhiteSpace(str))
        hmdatDataPoints.Add(new Tuple<string, string>("ApplicationTakenMethodType", borrower.BorrowerSsn + ":" + str));
      return hmdatDataPoints;
    }

    private Dictionary<string, string> LoadHmdaFieldsValues(List<string> fieldIdList)
    {
      return fieldIdList.ToDictionary<string, string, string>((Func<string, string>) (fieldId => fieldId), (Func<string, string>) (fieldId => this._fannie32.GetField(fieldId)));
    }

    private static string GetCollectedValueBasedOnVisual(string fieldValue)
    {
      switch (fieldValue.ToLower())
      {
        case "y":
          return "Y";
        case "n":
          return "N";
        default:
          return string.Empty;
      }
    }
  }
}
