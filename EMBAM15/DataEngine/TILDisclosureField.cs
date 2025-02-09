// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.TILDisclosureField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class TILDisclosureField : VirtualField
  {
    public const string FieldPrefix = "DISCLOSEDTIL";
    public const string FieldDescPrefix = "Last Disclosed TIL Field";
    private DisclosureProperty property;
    private FieldOptionCollection options = FieldOptionCollection.Empty;
    private int index = 1;

    public TILDisclosureField(DisclosureProperty property, string description, FieldFormat format)
      : base("DISCLOSEDTIL." + property.ToString(), description, format, FieldInstanceSpecifierType.Index)
    {
      this.property = property;
    }

    public TILDisclosureField(
      DisclosureProperty property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public TILDisclosureField(TILDisclosureField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.property = parent.property;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LastDisclosedTILFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new TILDisclosureField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    protected override string Evaluate(LoanData loan)
    {
      DisclosureTrackingLog[] disclosureTrackingLog1 = loan.GetLogList().GetAllDisclosureTrackingLog(true, DisclosureTrackingLog.DisclosureTrackingType.TIL);
      if (disclosureTrackingLog1 == null || disclosureTrackingLog1.Length == 0 || disclosureTrackingLog1.Length < this.index)
        return "";
      DisclosureTrackingLog disclosureTrackingLog2 = disclosureTrackingLog1[disclosureTrackingLog1.Length - this.index];
      switch (this.property)
      {
        case DisclosureProperty.SentDate:
          return disclosureTrackingLog2.DisclosedDate.ToString("MM/dd/yyyy");
        case DisclosureProperty.DisclosedBy:
          return disclosureTrackingLog2.DisclosedByFullName;
        case DisclosureProperty.DeliveryMethod:
          return disclosureTrackingLog2.DisclosedMethodName;
        case DisclosureProperty.ReceivedDate:
          return disclosureTrackingLog2.ReceivedDate.ToString("MM/dd/yyyy");
        case DisclosureProperty.Comments:
          return disclosureTrackingLog2.Comments;
        case DisclosureProperty.BorrowerName:
          return disclosureTrackingLog2.BorrowerName;
        case DisclosureProperty.CoBorrowerName:
          return disclosureTrackingLog2.CoBorrowerName;
        case DisclosureProperty.PropertyAddress:
          return disclosureTrackingLog2.PropertyAddress;
        case DisclosureProperty.PropertyCity:
          return disclosureTrackingLog2.PropertyCity;
        case DisclosureProperty.PropertyState:
          return disclosureTrackingLog2.PropertyState;
        case DisclosureProperty.PropertyZip:
          return disclosureTrackingLog2.PropertyZip;
        case DisclosureProperty.DisclosedAPR:
          return disclosureTrackingLog2.DisclosedAPR;
        case DisclosureProperty.LoanProgram:
          return disclosureTrackingLog2.LoanProgram;
        case DisclosureProperty.LoanAmount:
          return disclosureTrackingLog2.LoanAmount;
        case DisclosureProperty.FinanceCharge:
          return disclosureTrackingLog2.FinanceCharge;
        case DisclosureProperty.ApplicationDate:
          return disclosureTrackingLog2.ApplicationDate.ToString("MM/dd/yyyy");
        default:
          return "";
      }
    }

    public int Index => this.index;

    public DisclosureProperty Property => this.property;
  }
}
