// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EDisclosure2015TrackingFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class EDisclosure2015TrackingFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static EDisclosure2015TrackingFields()
    {
      EDisclosure2015TrackingFields.All.Add((FieldDefinition) new EDisclosure2015TrackingField(EDisclosureProperty2015.eDisclosurePresumedReceivedDate, "2015 Fulfillment Presumed Received Date", FieldFormat.DATE));
      EDisclosure2015TrackingFields.All.Add((FieldDefinition) new EDisclosure2015TrackingField(EDisclosureProperty2015.eDisclosureActualReceivedDate, "2015 Fulfillment Actual Received Date", FieldFormat.DATE));
      EDisclosure2015TrackingFields.All.Add((FieldDefinition) new EDisclosure2015TrackingField(EDisclosureProperty2015.eDisclosureSentDate, "2015 Fulfillment Sent Date", FieldFormat.DATE));
      EDisclosure2015TrackingFields.All.Add((FieldDefinition) new EDisclosure2015TrackingField(EDisclosureProperty2015.eDisclosureBorrowerConsent, "2015 Borrower Consent when eDisclosure was sent", FieldFormat.STRING));
      EDisclosure2015TrackingFields.All.Add((FieldDefinition) new EDisclosure2015TrackingField(EDisclosureProperty2015.eDisclosureCoBorrowerConsent, "2015 Co-Borrower Consent when eDisclosure was sent", FieldFormat.STRING));
    }
  }
}
