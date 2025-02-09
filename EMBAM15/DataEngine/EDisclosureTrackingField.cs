// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EDisclosureTrackingField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class EDisclosureTrackingField : VirtualField
  {
    public const string FieldPrefix = "EDISCLOSEDTRK";
    public const string FieldDescPrefix = "Last eDisclosed Field";
    private EDisclosureProperty property;
    private FieldOptionCollection options = FieldOptionCollection.Empty;
    private int index = 1;

    public EDisclosureTrackingField(
      EDisclosureProperty property,
      string description,
      FieldFormat format)
      : base("EDISCLOSEDTRK." + property.ToString(), description, format, FieldInstanceSpecifierType.Index)
    {
      this.property = property;
    }

    public EDisclosureTrackingField(
      EDisclosureProperty property,
      string description,
      FieldFormat format,
      FieldInstanceSpecifierType instanceType)
      : base("EDISCLOSEDTRK." + property.ToString(), description, format, instanceType)
    {
      this.property = property;
    }

    public EDisclosureTrackingField(
      EDisclosureProperty property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public EDisclosureTrackingField(EDisclosureTrackingField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.property = parent.property;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.EDisclosureTrackingFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new EDisclosureTrackingField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    protected override string Evaluate(LoanData loan)
    {
      if (!Utils.CheckIf2015RespaTila(loan.GetField("3969")))
      {
        DisclosureTrackingLog[] edisclosureTrackingLog = loan.GetLogList().GetAllEDisclosureTrackingLog(true);
        if (this.property == EDisclosureProperty.DisclosureCount)
          return edisclosureTrackingLog != null ? string.Concat((object) edisclosureTrackingLog.Length) : "0";
        if (edisclosureTrackingLog == null || edisclosureTrackingLog.Length == 0 || edisclosureTrackingLog.Length < this.index)
          return "";
        DisclosureTrackingLog disclosureTrackingLog = edisclosureTrackingLog[edisclosureTrackingLog.Length - this.index];
        switch (this.property)
        {
          case EDisclosureProperty.SentDate:
            return disclosureTrackingLog.eDisclosurePackageCreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.BorrowerDateViewMessage:
            return disclosureTrackingLog.eDisclosureBorrowerViewMessageDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.BorrowerDateAccepted:
            return disclosureTrackingLog.eDisclosureBorrowerAcceptConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.BorrowerDateRejected:
            return disclosureTrackingLog.eDisclosureBorrowerRejectConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.BorrowerDateESigned:
            return disclosureTrackingLog.eDisclosureBorrowereSignedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.CoBorrowerDateViewMessage:
            return disclosureTrackingLog.eDisclosureCoBorrowerViewMessageDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.CoBorrowerDateAccepted:
            return disclosureTrackingLog.eDisclosureCoBorrowerAcceptConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.CoBorrowerDateRejected:
            return disclosureTrackingLog.eDisclosureCoBorrowerRejectConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.CoBorrowerDateESigned:
            return disclosureTrackingLog.eDisclosureCoBorrowereSignedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.FulfilledBy:
            return disclosureTrackingLog.DisclosedByFullName;
          case EDisclosureProperty.DateTimeFulfilled:
            return !(disclosureTrackingLog.eDisclosureManualFulfillmentDate < disclosureTrackingLog.FullfillmentProcessedDate) ? disclosureTrackingLog.eDisclosureManualFulfillmentDate.ToString("MM/dd/yyyy hh:mm:ss tt") : disclosureTrackingLog.FullfillmentProcessedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.FulfillmentMethod:
            if (disclosureTrackingLog.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.ByMail)
              return "U.S. Mail";
            if (disclosureTrackingLog.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
              return "eDisclosure";
            if (disclosureTrackingLog.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.Fax)
              return "Fax";
            if (disclosureTrackingLog.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
              return "In Person";
            if (disclosureTrackingLog.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.Other)
              return "Other";
            return disclosureTrackingLog.eDisclosureManualFulfillmentDate == DateTime.MinValue && disclosureTrackingLog.FullfillmentProcessedDate != DateTime.MinValue ? "Encompass Fulfillment Service" : string.Empty;
          case EDisclosureProperty.Comments:
            return disclosureTrackingLog.Comments;
          case EDisclosureProperty.BorrowerName:
            return disclosureTrackingLog.BorrowerName;
          case EDisclosureProperty.BorrowerEmailAddress:
            return disclosureTrackingLog.eDisclosureBorrowerEmail;
          case EDisclosureProperty.BorrowerIPAddressAccepted:
            return disclosureTrackingLog.eDisclosureBorrowerAcceptConsentIP;
          case EDisclosureProperty.BorrowerIPAddressRejected:
            return disclosureTrackingLog.eDisclosureBorrowerRejectConsentIP;
          case EDisclosureProperty.BorrowerDateAuthenticated:
            return disclosureTrackingLog.eDisclosureBorrowerAuthenticatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.BorrowerIPAddressAuthenticated:
            return disclosureTrackingLog.eDisclosureBorrowerAuthenticatedIP;
          case EDisclosureProperty.BorrowerIPAddressESigned:
            return disclosureTrackingLog.eDisclosureBorrowereSignedIP;
          case EDisclosureProperty.CoBorrowerName:
            return disclosureTrackingLog.CoBorrowerName;
          case EDisclosureProperty.CoBorrowerEmailAddress:
            return disclosureTrackingLog.eDisclosureCoBorrowerEmail;
          case EDisclosureProperty.CoBorrowerIPAddressAccepted:
            return disclosureTrackingLog.eDisclosureCoBorrowerAcceptConsentIP;
          case EDisclosureProperty.CoBorrowerIPAddressRejected:
            return disclosureTrackingLog.eDisclosureCoBorrowerRejectConsentIP;
          case EDisclosureProperty.CoBorrowerDateAuthenticated:
            return disclosureTrackingLog.eDisclosureCoBorrowerAuthenticatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.CoBorrowerIPAddressAuthenticated:
            return disclosureTrackingLog.eDisclosureCoBorrowerAuthenticatedIP;
          case EDisclosureProperty.CoBorrowerIPAddressESigned:
            return disclosureTrackingLog.eDisclosureCoBorrowereSignedIP;
          case EDisclosureProperty.LoanOriginatorName:
            return disclosureTrackingLog.eDisclosureLOName;
          case EDisclosureProperty.LoanOriginatorDateViewMessage:
            return disclosureTrackingLog.eDisclosureLOViewMessageDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.LoanOriginatorDateESigned:
            return disclosureTrackingLog.eDisclosureLOeSignedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.LoanOriginatorIPAddressESigned:
            return disclosureTrackingLog.eDisclosureLOeSignedIP;
          case EDisclosureProperty.FulfillmentComment:
            return disclosureTrackingLog.eDisclosureManualFulfillmentComment;
          case EDisclosureProperty.DisclosureMethod:
            return disclosureTrackingLog.DisclosedMethodName;
          case EDisclosureProperty.FulfillmentOrderedBy:
            return disclosureTrackingLog.eDisclosureManuallyFulfilledBy;
          case EDisclosureProperty.BorrowerDocumentViewedDate:
            return disclosureTrackingLog.EDisclosureBorrowerDocumentViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          case EDisclosureProperty.CoBorrowerDocumentViewedDate:
            return disclosureTrackingLog.EDisclosureCoborrowerDocumentViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
          default:
            return "";
        }
      }
      else
      {
        IDisclosureTracking2015Log[] idisclosureTracking2015Log = loan.GetLogList().GetAllIDisclosureTracking2015Log(true);
        if (this.property == EDisclosureProperty.DisclosureCount)
          return idisclosureTracking2015Log != null ? string.Concat((object) idisclosureTracking2015Log.Length) : "0";
        if (idisclosureTracking2015Log == null || idisclosureTracking2015Log.Length == 0 || idisclosureTracking2015Log.Length < this.index)
          return "";
        IDisclosureTracking2015Log disclosureTracking2015Log = idisclosureTracking2015Log[idisclosureTracking2015Log.Length - this.index];
        try
        {
          switch (this.property)
          {
            case EDisclosureProperty.SentDate:
              return disclosureTracking2015Log.eDisclosurePackageCreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerDateViewMessage:
              return disclosureTracking2015Log.eDisclosureBorrowerViewMessageDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerDateAccepted:
              return disclosureTracking2015Log.eDisclosureBorrowerAcceptConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerDateRejected:
              return disclosureTracking2015Log.eDisclosureBorrowerRejectConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerDateESigned:
              return disclosureTracking2015Log.eDisclosureBorrowereSignedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerDateViewMessage:
              return disclosureTracking2015Log.eDisclosureCoBorrowerViewMessageDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerDateAccepted:
              return disclosureTracking2015Log.eDisclosureCoBorrowerAcceptConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerDateRejected:
              return disclosureTracking2015Log.eDisclosureCoBorrowerRejectConsentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerDateESigned:
              return disclosureTracking2015Log.eDisclosureCoBorrowereSignedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.FulfilledBy:
              return disclosureTracking2015Log.DisclosedByFullName;
            case EDisclosureProperty.DateTimeFulfilled:
              return disclosureTracking2015Log.eDisclosureManualFulfillmentDate < disclosureTracking2015Log.FullfillmentProcessedDate ? disclosureTracking2015Log.FullfillmentProcessedDate.ToString("MM/dd/yyyy hh:mm:ss tt") : disclosureTracking2015Log.eDisclosureManualFulfillmentDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.FulfillmentMethod:
              if (disclosureTracking2015Log.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.ByMail)
                return "U.S. Mail";
              if (disclosureTracking2015Log.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure)
                return "eDisclosure";
              if (disclosureTracking2015Log.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.Fax)
                return "Fax";
              if (disclosureTracking2015Log.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.InPerson)
                return "In Person";
              if (disclosureTracking2015Log.eDisclosureManualFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.Other)
                return "Other";
              if (!(disclosureTracking2015Log.eDisclosureManualFulfillmentDate == DateTime.MinValue) || !(disclosureTracking2015Log.FullfillmentProcessedDate != DateTime.MinValue))
                return string.Empty;
              return disclosureTracking2015Log.eDisclosureAutomatedFulfillmentMethod == DisclosureTrackingBase.DisclosedMethod.OvernightShipping ? "Overnight Shipping" : "Encompass Fulfillment Service";
            case EDisclosureProperty.Comments:
              return disclosureTracking2015Log.Comments;
            case EDisclosureProperty.BorrowerName:
              return disclosureTracking2015Log.BorrowerName;
            case EDisclosureProperty.BorrowerEmailAddress:
              return disclosureTracking2015Log.eDisclosureBorrowerEmail;
            case EDisclosureProperty.BorrowerIPAddressAccepted:
              return disclosureTracking2015Log.eDisclosureBorrowerAcceptConsentIP;
            case EDisclosureProperty.BorrowerIPAddressRejected:
              return disclosureTracking2015Log.eDisclosureBorrowerRejectConsentIP;
            case EDisclosureProperty.BorrowerDateAuthenticated:
              return disclosureTracking2015Log.eDisclosureBorrowerAuthenticatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerIPAddressAuthenticated:
              return disclosureTracking2015Log.eDisclosureBorrowerAuthenticatedIP;
            case EDisclosureProperty.BorrowerIPAddressESigned:
              return disclosureTracking2015Log.eDisclosureBorrowereSignedIP;
            case EDisclosureProperty.CoBorrowerName:
              return disclosureTracking2015Log.CoBorrowerName;
            case EDisclosureProperty.CoBorrowerEmailAddress:
              return disclosureTracking2015Log.eDisclosureCoBorrowerEmail;
            case EDisclosureProperty.CoBorrowerIPAddressAccepted:
              return disclosureTracking2015Log.eDisclosureCoBorrowerAcceptConsentIP;
            case EDisclosureProperty.CoBorrowerIPAddressRejected:
              return disclosureTracking2015Log.eDisclosureCoBorrowerRejectConsentIP;
            case EDisclosureProperty.CoBorrowerDateAuthenticated:
              return disclosureTracking2015Log.eDisclosureCoBorrowerAuthenticatedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerIPAddressAuthenticated:
              return disclosureTracking2015Log.eDisclosureCoBorrowerAuthenticatedIP;
            case EDisclosureProperty.CoBorrowerIPAddressESigned:
              return disclosureTracking2015Log.eDisclosureCoBorrowereSignedIP;
            case EDisclosureProperty.LoanOriginatorName:
              return disclosureTracking2015Log.eDisclosureLOName;
            case EDisclosureProperty.LoanOriginatorDateViewMessage:
              return disclosureTracking2015Log.eDisclosureLOViewMessageDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.LoanOriginatorDateESigned:
              return disclosureTracking2015Log.eDisclosureLOeSignedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.LoanOriginatorIPAddressESigned:
              return disclosureTracking2015Log.eDisclosureLOeSignedIP;
            case EDisclosureProperty.FulfillmentComment:
              return disclosureTracking2015Log.eDisclosureManualFulfillmentComment;
            case EDisclosureProperty.DisclosureMethod:
              return disclosureTracking2015Log.DisclosedMethodName;
            case EDisclosureProperty.FulfillmentOrderedBy:
              return disclosureTracking2015Log.eDisclosureManuallyFulfilledBy;
            case EDisclosureProperty.BorrowerDocumentViewedDate:
              return disclosureTracking2015Log.EDisclosureBorrowerDocumentViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerDocumentViewedDate:
              return disclosureTracking2015Log.EDisclosureCoborrowerDocumentViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.FulfillmentTrackingNumber:
              return disclosureTracking2015Log.FulfillmentTrackingNumber;
            case EDisclosureProperty.BorrowerInformationalViewedDate:
              return disclosureTracking2015Log.eDisclosureBorrowerInformationalViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerInformationalViewedIP:
              return disclosureTracking2015Log.eDisclosureBorrowerInformationalViewedIP;
            case EDisclosureProperty.BorrowerInformationalCompletedDate:
              return disclosureTracking2015Log.eDisclosureBorrowerInformationalCompletedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.BorrowerInformationalCompletedIP:
              return disclosureTracking2015Log.eDisclosureBorrowerInformationalCompletedIP;
            case EDisclosureProperty.CoBorrowerInformationalViewedDate:
              return disclosureTracking2015Log.eDisclosureCoBorrowerInformationalViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerInformationalViewedIP:
              return disclosureTracking2015Log.eDisclosureCoBorrowerInformationalViewedIP;
            case EDisclosureProperty.CoBorrowerInformationalCompletedDate:
              return disclosureTracking2015Log.eDisclosureCoBorrowerInformationalCompletedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.CoBorrowerInformationalCompletedIP:
              return disclosureTracking2015Log.eDisclosureCoBorrowerInformationalCompletedIP;
            case EDisclosureProperty.LoanOriginatorInformationalViewedDate:
              return disclosureTracking2015Log.eDisclosureLOInformationalViewedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.LoanOriginatorInformationalViewedIP:
              return disclosureTracking2015Log.eDisclosureLOInformationalViewedIP;
            case EDisclosureProperty.LoanOriginatorInformationalCompletedDate:
              return disclosureTracking2015Log.eDisclosureLOInformationalCompletedDate.ToString("MM/dd/yyyy hh:mm:ss tt");
            case EDisclosureProperty.LoanOriginatorInformationalCompletedIP:
              return disclosureTracking2015Log.eDisclosureLOInformationalCompletedIP;
            default:
              return "";
          }
        }
        catch (Exception ex)
        {
          return "";
        }
      }
    }

    public EDisclosureProperty Property => this.property;

    public int Index => this.index;
  }
}
