// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EscrowSchedule
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class EscrowSchedule
  {
    public EscrowSchedule()
    {
      this.PayDate = DateTime.MinValue;
      this.PayDate_Tax = DateTime.MinValue;
      this.PayDate_Hazard = DateTime.MinValue;
      this.PayDate_Mortgage = DateTime.MinValue;
      this.PayDate_Flood = DateTime.MinValue;
      this.PayDate_City = DateTime.MinValue;
      this.PayDate_User1 = DateTime.MinValue;
      this.PayDate_User2 = DateTime.MinValue;
      this.PayDate_User3 = DateTime.MinValue;
      this.PayDate_USDAPremium = DateTime.MinValue;
      this.Tax = 0.0;
      this.HazardInsurance = 0.0;
      this.MortgageInsurance = 0.0;
      this.FloodInsurance = 0.0;
      this.CityTax = 0.0;
      this.UserTax1 = 0.0;
      this.UserTax2 = 0.0;
      this.UserTax3 = 0.0;
      this.USDAPremium = 0.0;
      this.TotalPayment = 0.0;
    }

    public DateTime PayDate { set; get; }

    public DateTime PayDate_Tax { set; get; }

    public DateTime PayDate_Hazard { set; get; }

    public DateTime PayDate_Mortgage { set; get; }

    public DateTime PayDate_Flood { set; get; }

    public DateTime PayDate_City { set; get; }

    public DateTime PayDate_User1 { set; get; }

    public DateTime PayDate_User2 { set; get; }

    public DateTime PayDate_User3 { set; get; }

    public DateTime PayDate_USDAPremium { set; get; }

    public double Tax { set; get; }

    public double HazardInsurance { set; get; }

    public double MortgageInsurance { set; get; }

    public double FloodInsurance { set; get; }

    public double CityTax { set; get; }

    public double UserTax1 { set; get; }

    public double UserTax2 { set; get; }

    public double UserTax3 { set; get; }

    public double USDAPremium { set; get; }

    public double TotalPayment { set; get; }
  }
}
