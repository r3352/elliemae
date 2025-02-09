// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.StateCounty
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class StateCounty : IComparable<StateCounty>
  {
    private string state;
    private string county;

    public StateCounty(string state, string county)
    {
      this.state = state;
      this.county = county;
    }

    public string State => this.state;

    public string County => this.county;

    public override bool Equals(object obj)
    {
      StateCounty stateCounty = obj as StateCounty;
      return obj != null && string.Compare(this.state, stateCounty.state, true) == 0 && string.Compare(this.county, stateCounty.county, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.state) ^ StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.county);
    }

    public int CompareTo(StateCounty other)
    {
      if (other == null)
        return 1;
      int num = string.Compare(this.state, other.state, true);
      return num != 0 ? num : string.Compare(this.county, other.county, true);
    }
  }
}
