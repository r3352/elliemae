// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.USPS
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class USPS
  {
    public static readonly Hashtable StateCodes = CollectionsUtil.CreateCaseInsensitiveHashtable();
    public static readonly Hashtable StateAbbrs = new Hashtable();
    public static readonly Hashtable StateNames = new Hashtable();
    public static readonly USPS.State[] States = (USPS.State[]) null;

    static USPS()
    {
      USPS.StateCodes[(object) "Unknown"] = (object) USPS.StateCode.Unknown;
      USPS.StateAbbrs[(object) USPS.StateCode.Unknown] = (object) "Unknown";
      USPS.StateNames[(object) USPS.StateCode.Unknown] = (object) "Unknown";
      USPS.StateCodes[(object) "AL"] = (object) USPS.StateCode.AL;
      USPS.StateAbbrs[(object) USPS.StateCode.AL] = (object) "AL";
      USPS.StateNames[(object) USPS.StateCode.AL] = (object) "Alabama";
      USPS.StateCodes[(object) "AK"] = (object) USPS.StateCode.AK;
      USPS.StateAbbrs[(object) USPS.StateCode.AK] = (object) "AK";
      USPS.StateNames[(object) USPS.StateCode.AK] = (object) "Alaska";
      USPS.StateCodes[(object) "AS"] = (object) USPS.StateCode.AS;
      USPS.StateAbbrs[(object) USPS.StateCode.AS] = (object) "AS";
      USPS.StateNames[(object) USPS.StateCode.AS] = (object) "American Samoa";
      USPS.StateCodes[(object) "AZ"] = (object) USPS.StateCode.AZ;
      USPS.StateAbbrs[(object) USPS.StateCode.AZ] = (object) "AZ";
      USPS.StateNames[(object) USPS.StateCode.AZ] = (object) "Arizona";
      USPS.StateCodes[(object) "AR"] = (object) USPS.StateCode.AR;
      USPS.StateAbbrs[(object) USPS.StateCode.AR] = (object) "AR";
      USPS.StateNames[(object) USPS.StateCode.AR] = (object) "Arkansas";
      USPS.StateCodes[(object) "CA"] = (object) USPS.StateCode.CA;
      USPS.StateAbbrs[(object) USPS.StateCode.CA] = (object) "CA";
      USPS.StateNames[(object) USPS.StateCode.CA] = (object) "California";
      USPS.StateCodes[(object) "CO"] = (object) USPS.StateCode.CO;
      USPS.StateAbbrs[(object) USPS.StateCode.CO] = (object) "CO";
      USPS.StateNames[(object) USPS.StateCode.CO] = (object) "Colorado";
      USPS.StateCodes[(object) "CT"] = (object) USPS.StateCode.CT;
      USPS.StateAbbrs[(object) USPS.StateCode.CT] = (object) "CT";
      USPS.StateNames[(object) USPS.StateCode.CT] = (object) "Connecticut";
      USPS.StateCodes[(object) "DE"] = (object) USPS.StateCode.DE;
      USPS.StateAbbrs[(object) USPS.StateCode.DE] = (object) "DE";
      USPS.StateNames[(object) USPS.StateCode.DE] = (object) "Delaware";
      USPS.StateCodes[(object) "DC"] = (object) USPS.StateCode.DC;
      USPS.StateAbbrs[(object) USPS.StateCode.DC] = (object) "DC";
      USPS.StateNames[(object) USPS.StateCode.DC] = (object) "District of Columbia";
      USPS.StateCodes[(object) "FM"] = (object) USPS.StateCode.FM;
      USPS.StateAbbrs[(object) USPS.StateCode.FM] = (object) "FM";
      USPS.StateNames[(object) USPS.StateCode.FM] = (object) "Federated States of Micronesia";
      USPS.StateCodes[(object) "FL"] = (object) USPS.StateCode.FL;
      USPS.StateAbbrs[(object) USPS.StateCode.FL] = (object) "FL";
      USPS.StateNames[(object) USPS.StateCode.FL] = (object) "Florida";
      USPS.StateCodes[(object) "GA"] = (object) USPS.StateCode.GA;
      USPS.StateAbbrs[(object) USPS.StateCode.GA] = (object) "GA";
      USPS.StateNames[(object) USPS.StateCode.GA] = (object) "Georgia";
      USPS.StateCodes[(object) "GU"] = (object) USPS.StateCode.GU;
      USPS.StateAbbrs[(object) USPS.StateCode.GU] = (object) "GU";
      USPS.StateNames[(object) USPS.StateCode.GU] = (object) "Guam";
      USPS.StateCodes[(object) "HI"] = (object) USPS.StateCode.HI;
      USPS.StateAbbrs[(object) USPS.StateCode.HI] = (object) "HI";
      USPS.StateNames[(object) USPS.StateCode.HI] = (object) "Hawaii";
      USPS.StateCodes[(object) "ID"] = (object) USPS.StateCode.ID;
      USPS.StateAbbrs[(object) USPS.StateCode.ID] = (object) "ID";
      USPS.StateNames[(object) USPS.StateCode.ID] = (object) "Idaho";
      USPS.StateCodes[(object) "IL"] = (object) USPS.StateCode.IL;
      USPS.StateAbbrs[(object) USPS.StateCode.IL] = (object) "IL";
      USPS.StateNames[(object) USPS.StateCode.IL] = (object) "Illinois";
      USPS.StateCodes[(object) "IN"] = (object) USPS.StateCode.IN;
      USPS.StateAbbrs[(object) USPS.StateCode.IN] = (object) "IN";
      USPS.StateNames[(object) USPS.StateCode.IN] = (object) "Indiana";
      USPS.StateCodes[(object) "IA"] = (object) USPS.StateCode.IA;
      USPS.StateAbbrs[(object) USPS.StateCode.IA] = (object) "IA";
      USPS.StateNames[(object) USPS.StateCode.IA] = (object) "Iowa";
      USPS.StateCodes[(object) "KS"] = (object) USPS.StateCode.KS;
      USPS.StateAbbrs[(object) USPS.StateCode.KS] = (object) "KS";
      USPS.StateNames[(object) USPS.StateCode.KS] = (object) "Kansas";
      USPS.StateCodes[(object) "KY"] = (object) USPS.StateCode.KY;
      USPS.StateAbbrs[(object) USPS.StateCode.KY] = (object) "KY";
      USPS.StateNames[(object) USPS.StateCode.KY] = (object) "Kentucky";
      USPS.StateCodes[(object) "LA"] = (object) USPS.StateCode.LA;
      USPS.StateAbbrs[(object) USPS.StateCode.LA] = (object) "LA";
      USPS.StateNames[(object) USPS.StateCode.LA] = (object) "Louisiana";
      USPS.StateCodes[(object) "ME"] = (object) USPS.StateCode.ME;
      USPS.StateAbbrs[(object) USPS.StateCode.ME] = (object) "ME";
      USPS.StateNames[(object) USPS.StateCode.ME] = (object) "Maine";
      USPS.StateCodes[(object) "MH"] = (object) USPS.StateCode.MH;
      USPS.StateAbbrs[(object) USPS.StateCode.MH] = (object) "MH";
      USPS.StateNames[(object) USPS.StateCode.MH] = (object) "Marshall Islands";
      USPS.StateCodes[(object) "MD"] = (object) USPS.StateCode.MD;
      USPS.StateAbbrs[(object) USPS.StateCode.MD] = (object) "MD";
      USPS.StateNames[(object) USPS.StateCode.MD] = (object) "Maryland";
      USPS.StateCodes[(object) "MA"] = (object) USPS.StateCode.MA;
      USPS.StateAbbrs[(object) USPS.StateCode.MA] = (object) "MA";
      USPS.StateNames[(object) USPS.StateCode.MA] = (object) "Massachusetts";
      USPS.StateCodes[(object) "MI"] = (object) USPS.StateCode.MI;
      USPS.StateAbbrs[(object) USPS.StateCode.MI] = (object) "MI";
      USPS.StateNames[(object) USPS.StateCode.MI] = (object) "Michigan";
      USPS.StateCodes[(object) "MN"] = (object) USPS.StateCode.MN;
      USPS.StateAbbrs[(object) USPS.StateCode.MN] = (object) "MN";
      USPS.StateNames[(object) USPS.StateCode.MN] = (object) "Minnesota";
      USPS.StateCodes[(object) "MS"] = (object) USPS.StateCode.MS;
      USPS.StateAbbrs[(object) USPS.StateCode.MS] = (object) "MS";
      USPS.StateNames[(object) USPS.StateCode.MS] = (object) "Mississippi";
      USPS.StateCodes[(object) "MO"] = (object) USPS.StateCode.MO;
      USPS.StateAbbrs[(object) USPS.StateCode.MO] = (object) "MO";
      USPS.StateNames[(object) USPS.StateCode.MO] = (object) "Missouri";
      USPS.StateCodes[(object) "MT"] = (object) USPS.StateCode.MT;
      USPS.StateAbbrs[(object) USPS.StateCode.MT] = (object) "MT";
      USPS.StateNames[(object) USPS.StateCode.MT] = (object) "Montana";
      USPS.StateCodes[(object) "NE"] = (object) USPS.StateCode.NE;
      USPS.StateAbbrs[(object) USPS.StateCode.NE] = (object) "NE";
      USPS.StateNames[(object) USPS.StateCode.NE] = (object) "Nebraska";
      USPS.StateCodes[(object) "NV"] = (object) USPS.StateCode.NV;
      USPS.StateAbbrs[(object) USPS.StateCode.NV] = (object) "NV";
      USPS.StateNames[(object) USPS.StateCode.NV] = (object) "Nevada";
      USPS.StateCodes[(object) "NH"] = (object) USPS.StateCode.NH;
      USPS.StateAbbrs[(object) USPS.StateCode.NH] = (object) "NH";
      USPS.StateNames[(object) USPS.StateCode.NH] = (object) "New Hampshire";
      USPS.StateCodes[(object) "NJ"] = (object) USPS.StateCode.NJ;
      USPS.StateAbbrs[(object) USPS.StateCode.NJ] = (object) "NJ";
      USPS.StateNames[(object) USPS.StateCode.NJ] = (object) "New Jersey";
      USPS.StateCodes[(object) "NM"] = (object) USPS.StateCode.NM;
      USPS.StateAbbrs[(object) USPS.StateCode.NM] = (object) "NM";
      USPS.StateNames[(object) USPS.StateCode.NM] = (object) "New Mexico";
      USPS.StateCodes[(object) "NY"] = (object) USPS.StateCode.NY;
      USPS.StateAbbrs[(object) USPS.StateCode.NY] = (object) "NY";
      USPS.StateNames[(object) USPS.StateCode.NY] = (object) "New York";
      USPS.StateCodes[(object) "NC"] = (object) USPS.StateCode.NC;
      USPS.StateAbbrs[(object) USPS.StateCode.NC] = (object) "NC";
      USPS.StateNames[(object) USPS.StateCode.NC] = (object) "North Carolina";
      USPS.StateCodes[(object) "ND"] = (object) USPS.StateCode.ND;
      USPS.StateAbbrs[(object) USPS.StateCode.ND] = (object) "ND";
      USPS.StateNames[(object) USPS.StateCode.ND] = (object) "North Dakota";
      USPS.StateCodes[(object) "MP"] = (object) USPS.StateCode.MP;
      USPS.StateAbbrs[(object) USPS.StateCode.MP] = (object) "MP";
      USPS.StateNames[(object) USPS.StateCode.MP] = (object) "Northern Mariana Islands";
      USPS.StateCodes[(object) "OH"] = (object) USPS.StateCode.OH;
      USPS.StateAbbrs[(object) USPS.StateCode.OH] = (object) "OH";
      USPS.StateNames[(object) USPS.StateCode.OH] = (object) "Ohio";
      USPS.StateCodes[(object) "OK"] = (object) USPS.StateCode.OK;
      USPS.StateAbbrs[(object) USPS.StateCode.OK] = (object) "OK";
      USPS.StateNames[(object) USPS.StateCode.OK] = (object) "Oklahoma";
      USPS.StateCodes[(object) "OR"] = (object) USPS.StateCode.OR;
      USPS.StateAbbrs[(object) USPS.StateCode.OR] = (object) "OR";
      USPS.StateNames[(object) USPS.StateCode.OR] = (object) "Oregon";
      USPS.StateCodes[(object) "PW"] = (object) USPS.StateCode.PW;
      USPS.StateAbbrs[(object) USPS.StateCode.PW] = (object) "PW";
      USPS.StateNames[(object) USPS.StateCode.PW] = (object) "Palau Island";
      USPS.StateCodes[(object) "PA"] = (object) USPS.StateCode.PA;
      USPS.StateAbbrs[(object) USPS.StateCode.PA] = (object) "PA";
      USPS.StateNames[(object) USPS.StateCode.PA] = (object) "Pennsylvania";
      USPS.StateCodes[(object) "PR"] = (object) USPS.StateCode.PR;
      USPS.StateAbbrs[(object) USPS.StateCode.PR] = (object) "PR";
      USPS.StateNames[(object) USPS.StateCode.PR] = (object) "Puerto Rico";
      USPS.StateCodes[(object) "RI"] = (object) USPS.StateCode.RI;
      USPS.StateAbbrs[(object) USPS.StateCode.RI] = (object) "RI";
      USPS.StateNames[(object) USPS.StateCode.RI] = (object) "Rhode Island";
      USPS.StateCodes[(object) "SC"] = (object) USPS.StateCode.SC;
      USPS.StateAbbrs[(object) USPS.StateCode.SC] = (object) "SC";
      USPS.StateNames[(object) USPS.StateCode.SC] = (object) "South Carolina";
      USPS.StateCodes[(object) "SD"] = (object) USPS.StateCode.SD;
      USPS.StateAbbrs[(object) USPS.StateCode.SD] = (object) "SD";
      USPS.StateNames[(object) USPS.StateCode.SD] = (object) "South Dakota";
      USPS.StateCodes[(object) "TN"] = (object) USPS.StateCode.TN;
      USPS.StateAbbrs[(object) USPS.StateCode.TN] = (object) "TN";
      USPS.StateNames[(object) USPS.StateCode.TN] = (object) "Tennessee";
      USPS.StateCodes[(object) "TX"] = (object) USPS.StateCode.TX;
      USPS.StateAbbrs[(object) USPS.StateCode.TX] = (object) "TX";
      USPS.StateNames[(object) USPS.StateCode.TX] = (object) "Texas";
      USPS.StateCodes[(object) "UT"] = (object) USPS.StateCode.UT;
      USPS.StateAbbrs[(object) USPS.StateCode.UT] = (object) "UT";
      USPS.StateNames[(object) USPS.StateCode.UT] = (object) "Utah";
      USPS.StateCodes[(object) "VT"] = (object) USPS.StateCode.VT;
      USPS.StateAbbrs[(object) USPS.StateCode.VT] = (object) "VT";
      USPS.StateNames[(object) USPS.StateCode.VT] = (object) "Vermont";
      USPS.StateCodes[(object) "VI"] = (object) USPS.StateCode.VI;
      USPS.StateAbbrs[(object) USPS.StateCode.VI] = (object) "VI";
      USPS.StateNames[(object) USPS.StateCode.VI] = (object) "Virgin Islands";
      USPS.StateCodes[(object) "VA"] = (object) USPS.StateCode.VA;
      USPS.StateAbbrs[(object) USPS.StateCode.VA] = (object) "VA";
      USPS.StateNames[(object) USPS.StateCode.VA] = (object) "Virginia";
      USPS.StateCodes[(object) "WA"] = (object) USPS.StateCode.WA;
      USPS.StateAbbrs[(object) USPS.StateCode.WA] = (object) "WA";
      USPS.StateNames[(object) USPS.StateCode.WA] = (object) "Washington";
      USPS.StateCodes[(object) "WV"] = (object) USPS.StateCode.WV;
      USPS.StateAbbrs[(object) USPS.StateCode.WV] = (object) "WV";
      USPS.StateNames[(object) USPS.StateCode.WV] = (object) "West Virginia";
      USPS.StateCodes[(object) "WI"] = (object) USPS.StateCode.WI;
      USPS.StateAbbrs[(object) USPS.StateCode.WI] = (object) "WI";
      USPS.StateNames[(object) USPS.StateCode.WI] = (object) "Wisconsin";
      USPS.StateCodes[(object) "WY"] = (object) USPS.StateCode.WY;
      USPS.StateAbbrs[(object) USPS.StateCode.WY] = (object) "WY";
      USPS.StateNames[(object) USPS.StateCode.WY] = (object) "Wyoming";
      ArrayList arrayList = new ArrayList();
      foreach (USPS.StateCode code in Enum.GetValues(typeof (USPS.StateCode)))
      {
        if (code != USPS.StateCode.Unknown)
          arrayList.Add((object) new USPS.State(code));
      }
      USPS.States = (USPS.State[]) arrayList.ToArray(typeof (USPS.State));
    }

    private USPS()
    {
    }

    public enum StateCode
    {
      Unknown,
      AL,
      AK,
      AS,
      AZ,
      AR,
      CA,
      CO,
      CT,
      DE,
      DC,
      FM,
      FL,
      GA,
      GU,
      HI,
      ID,
      IL,
      IN,
      IA,
      KS,
      KY,
      LA,
      ME,
      MH,
      MD,
      MA,
      MI,
      MN,
      MS,
      MO,
      MT,
      NE,
      NV,
      NH,
      NJ,
      NM,
      NY,
      NC,
      ND,
      MP,
      OH,
      OK,
      OR,
      PW,
      PA,
      PR,
      RI,
      SC,
      SD,
      TN,
      TX,
      UT,
      VT,
      VI,
      VA,
      WA,
      WV,
      WI,
      WY,
    }

    public class State : IComparable
    {
      public readonly USPS.StateCode Code;
      public readonly string Name;
      public readonly string Abbrev;

      public State(USPS.StateCode code)
      {
        this.Code = code;
        this.Name = string.Concat(USPS.StateNames[(object) code]);
        this.Abbrev = string.Concat(USPS.StateAbbrs[(object) code]);
      }

      public override string ToString() => this.Abbrev;

      public override bool Equals(object obj) => obj is USPS.State state && this.Code == state.Code;

      public override int GetHashCode() => this.Code.GetHashCode();

      public int CompareTo(object obj)
      {
        return !(obj is USPS.State state) ? 1 : string.Compare(this.Name, state.Name);
      }

      public static USPS.State FromAbbreviation(string abbr)
      {
        foreach (USPS.State state in USPS.States)
        {
          if (string.Compare(state.Abbrev, abbr, true) == 0)
            return state;
        }
        return (USPS.State) null;
      }

      public static USPS.State FromStateCode(USPS.StateCode code)
      {
        foreach (USPS.State state in USPS.States)
        {
          if (state.Code == code)
            return state;
        }
        return (USPS.State) null;
      }

      public static USPS.State FromName(string name)
      {
        foreach (USPS.State state in USPS.States)
        {
          if (string.Compare(state.Name, name, true) == 0)
            return state;
        }
        return (USPS.State) null;
      }
    }
  }
}
