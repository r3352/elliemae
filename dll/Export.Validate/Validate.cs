// Decompiled with JetBrains decompiler
// Type: Encompass.Export.Validate
// Assembly: Export.Validate, Version=1.0.7933.30763, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 617E5049-06C8-448B-B2D8-44769B16A732
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Validate.dll

using EllieMae.EMLite.Export;

#nullable disable
namespace Encompass.Export
{
  public class Validate : IValidate
  {
    private IBam loan;

    public IBam Bam
    {
      get => this.loan;
      set => this.loan = value;
    }

    public bool ValidateData(string format, bool allowContinue)
    {
      switch (format.ToUpper())
      {
        case "LP":
          return new FreddieValidate(this.loan).ValidateData(allowContinue);
        case "FREDDIE30":
          return new Freddie30Validate(this.loan).ValidateData(allowContinue);
        case "FREDDIE42":
          return new Freddie42Validate(this.loan).ValidateData(allowContinue);
        case "CWLEDA":
          return new CWLedaValidate(this.loan).ValidateData(allowContinue);
        case "FNMAEC":
          return new FannieEarlyCheckValidate(this.loan).ValidateData(allowContinue);
        case "FNMAECULAD":
          return new FannieEarlyCheckValidate(this.loan, true).ValidateData(allowContinue);
        default:
          return new FannieValidate(this.loan).ValidateData(allowContinue);
      }
    }

    public string ValidateData_List(string format)
    {
      switch (format.ToUpper())
      {
        case "LP":
          return new FreddieValidate(this.loan).ValidateDataList();
        case "FREDDIE30":
          return new Freddie30Validate(this.loan).ValidateDataList();
        case "FREDDIE42":
          return new Freddie42Validate(this.loan).ValidateDataList();
        case "CWLEDA":
          return new CWLedaValidate(this.loan).ValidateDataList();
        case "FNMAEC":
          return new FannieEarlyCheckValidate(this.loan).ValidateDataList();
        default:
          return new FannieValidate(this.loan).ValidateDataList();
      }
    }
  }
}
