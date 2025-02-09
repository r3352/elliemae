// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EDisclosure2015TrackingField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class EDisclosure2015TrackingField : VirtualField
  {
    public const string FieldPrefix = "EDISCLOSED2015TRK";
    public const string FieldDescPrefix = "Last eDisclosed 2015 Field";
    private EDisclosureProperty2015 property;
    private FieldOptionCollection options = FieldOptionCollection.Empty;
    private int index = 1;

    public EDisclosureProperty2015 Property => this.property;

    public int Index => this.index;

    public EDisclosure2015TrackingField(
      EDisclosureProperty2015 property,
      string description,
      FieldFormat format)
      : base("EDISCLOSED2015TRK." + property.ToString(), description, format, FieldInstanceSpecifierType.Index)
    {
      this.property = property;
    }

    public EDisclosure2015TrackingField(
      EDisclosureProperty2015 property,
      string description,
      FieldFormat format,
      FieldInstanceSpecifierType instanceType)
      : base("EDISCLOSED2015TRK." + property.ToString(), description, format, instanceType)
    {
      this.property = property;
    }

    public EDisclosure2015TrackingField(
      EDisclosureProperty2015 property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public EDisclosure2015TrackingField(EDisclosure2015TrackingField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.property = parent.property;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.EDisclosure2015TrackingFields;
    }

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new EDisclosure2015TrackingField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    protected override string Evaluate(LoanData loan)
    {
      IDisclosureTracking2015Log[] tracking2015EnhancedLog = loan.GetLogList().GetAllEDisclosureTracking2015EnhancedLog(true);
      if (this.property == EDisclosureProperty2015.DisclosureCount)
        return tracking2015EnhancedLog != null ? string.Concat((object) tracking2015EnhancedLog.Length) : "0";
      if (tracking2015EnhancedLog == null || tracking2015EnhancedLog.Length == 0 || tracking2015EnhancedLog.Length < this.index)
        return "";
      IDisclosureTracking2015Log disclosureTracking2015Log = tracking2015EnhancedLog[tracking2015EnhancedLog.Length - this.index];
      switch (this.property)
      {
        case EDisclosureProperty2015.eDisclosurePresumedReceivedDate:
          return disclosureTracking2015Log.PresumedFulfillmentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
        case EDisclosureProperty2015.eDisclosureActualReceivedDate:
          return disclosureTracking2015Log.ActualFulfillmentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
        case EDisclosureProperty2015.eDisclosureSentDate:
          return disclosureTracking2015Log.FullfillmentProcessedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
        case EDisclosureProperty2015.eDisclosureBorrowerConsent:
          return disclosureTracking2015Log.EDisclosureBorrowerLoanLevelConsent;
        case EDisclosureProperty2015.eDisclosureCoBorrowerConsent:
          return disclosureTracking2015Log.EDisclosureCoBorrowerLoanLevelConsent;
        default:
          return "";
      }
    }
  }
}
