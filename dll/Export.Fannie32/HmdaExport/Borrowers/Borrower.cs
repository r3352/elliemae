// Decompiled with JetBrains decompiler
// Type: Export.Fannie32.HmdaExport.Borrowers.Borrower
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Export.Fannie32.HmdaExport.Borrowers
{
  internal abstract class Borrower
  {
    public string BorrowerSsn { get; set; }

    public string BorrowerFirstName { get; set; }

    public List<string> FieldIdList { get; set; }

    public Dictionary<string, string> FieldIdValueDictionary { get; set; }

    public abstract string GetGenderType();

    public abstract string GetGenderRefusalIndicator();

    public abstract List<string> GetEthnicityType();

    public abstract List<string> GetEthnicityOriginType();

    public abstract List<string> GetEthnicityOriginTypeOtherDescription();

    public abstract string GetEthnicityRefusalIndicator();

    public abstract List<string> GetRaceType();

    public abstract string GetRaceRefusalIndicator();

    public abstract List<string> GetRaceDesignationType();

    public abstract List<string> GetRaceDesignationOtherAsnDesc();

    public abstract List<string> GetRaceDesignationOtherPiDesc();

    public abstract List<string> GetRaceTypeAdditionalDescription();

    public abstract string GetEthnicityCollectedBasedOnVisual();

    public abstract string GetGenderCollectedBasedOnVisual();

    public abstract string GetRaceCollectedBasedOnVisual();

    public abstract string ApplicationTakenMethodType();

    public static List<string> SplitDescription(string fieldValue, int size = 40)
    {
      List<string> stringList = new List<string>();
      for (int startIndex = 0; startIndex < fieldValue.Length; startIndex += size)
        stringList.Add(fieldValue.Substring(startIndex, Math.Min(size, fieldValue.Length - startIndex)));
      return stringList;
    }
  }
}
