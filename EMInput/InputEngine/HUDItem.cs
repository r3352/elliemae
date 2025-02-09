// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUDItem
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class HUDItem
  {
    private string disburseDate;
    private string tax;
    private string haz;
    private string mtg;
    private string flood;
    private string cityTax;
    private string user1;
    private string user2;
    private string user3;
    private string usdaAnnualFee;

    public string DisburseDate => this.disburseDate;

    public string Tax
    {
      get => this.tax;
      set => this.tax = value;
    }

    public string Haz
    {
      get => this.haz;
      set => this.haz = value;
    }

    public string Mtg
    {
      get => this.mtg;
      set => this.mtg = value;
    }

    public string Flood
    {
      get => this.flood;
      set => this.flood = value;
    }

    public string CityTax
    {
      get => this.cityTax;
      set => this.cityTax = value;
    }

    public string User1
    {
      get => this.user1;
      set => this.user1 = value;
    }

    public string User2
    {
      get => this.user2;
      set => this.user2 = value;
    }

    public string User3
    {
      get => this.user3;
      set => this.user3 = value;
    }

    public string USDAAnnualFee
    {
      get => this.usdaAnnualFee;
      set => this.usdaAnnualFee = value;
    }

    public HUDItem(
      string disburseDate,
      string tax,
      string haz,
      string mtg,
      string flood,
      string cityTax,
      string user1,
      string user2,
      string user3,
      string usdaAnnualFee)
    {
      this.disburseDate = disburseDate;
      this.tax = tax;
      this.haz = haz;
      this.mtg = mtg;
      this.flood = flood;
      this.cityTax = cityTax;
      this.user1 = user1;
      this.user2 = user2;
      this.user3 = user3;
      this.usdaAnnualFee = usdaAnnualFee;
    }

    public string GetCellValue(int impoundCol)
    {
      switch (impoundCol)
      {
        case 1:
          return this.tax;
        case 2:
          return this.haz;
        case 3:
          return this.mtg;
        case 4:
          return this.flood;
        case 5:
          return this.cityTax;
        case 6:
          return this.user1;
        case 7:
          return this.user2;
        case 8:
          return this.user3;
        case 9:
          return this.USDAAnnualFee;
        default:
          return string.Empty;
      }
    }

    public void SetCellValue(int impoundCol, string val)
    {
      switch (impoundCol)
      {
        case 1:
          this.tax = val;
          break;
        case 2:
          this.haz = val;
          break;
        case 3:
          this.mtg = val;
          break;
        case 4:
          this.flood = val;
          break;
        case 5:
          this.cityTax = val;
          break;
        case 6:
          this.user1 = val;
          break;
        case 7:
          this.user2 = val;
          break;
        case 8:
          this.user3 = val;
          break;
        case 9:
          this.USDAAnnualFee = val;
          break;
      }
    }
  }
}
