// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CDDisclosureField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class CDDisclosureField : VirtualField
  {
    public const string FieldPrefix = "DISCLOSEDCD";
    public const string FieldDescPrefix = "Last Disclosed CD Field";
    private DisclosureProperty2015 property;
    private FieldOptionCollection options = FieldOptionCollection.Empty;
    private int index = 1;

    public DisclosureProperty2015 Property => this.property;

    public int Index => this.index;

    public CDDisclosureField(
      DisclosureProperty2015 property,
      string description,
      FieldFormat format)
      : base("DISCLOSEDCD." + property.ToString(), description, format, FieldInstanceSpecifierType.Index)
    {
      this.property = property;
    }

    public CDDisclosureField(
      DisclosureProperty2015 property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public CDDisclosureField(CDDisclosureField parent, int index)
      : base((VirtualField) parent, (object) index)
    {
      this.property = parent.property;
      this.index = index;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.LastDisclosedCDFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new CDDisclosureField(this, Utils.ParseInt(instanceSpecifier, true));
    }

    protected override string Evaluate(LoanData loan)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = loan.GetLogList().GetAllIDisclosureTracking2015Log(true, DisclosureTracking2015Log.DisclosureTrackingType.CD);
      if (idisclosureTracking2015Log == null || idisclosureTracking2015Log.Length == 0 || idisclosureTracking2015Log.Length < this.index)
        return "";
      IDisclosureTracking2015Log disclosureTracking2015Log = idisclosureTracking2015Log[idisclosureTracking2015Log.Length - this.index];
      try
      {
        switch (this.property)
        {
          case DisclosureProperty2015.DisclosureType:
            return disclosureTracking2015Log.DisclosureType.ToString();
          case DisclosureProperty2015.SentDate:
            return disclosureTracking2015Log.DisclosedDate.ToString("MM/dd/yyyy");
          case DisclosureProperty2015.SentBy:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? (((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).DisclosedBy.UseUserValue ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).DisclosedBy.UserValue : disclosureTracking2015Log.DisclosedByFullName + "(" + ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).DisclosedBy.ComputedValue + ")") : ((DisclosureTracking2015Log) disclosureTracking2015Log).DisclosedByReportFullName;
          case DisclosureProperty2015.SentMethod:
            return disclosureTracking2015Log.DisclosedMethodName;
          case DisclosureProperty2015.OtherMethod:
            return disclosureTracking2015Log.DisclosedMethodOther;
          case DisclosureProperty2015.BorrowerReceivedMethod:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).DisclosedMethodNameByType(disclosureTracking2015Log.BorrowerDisclosedMethod) : ((DisclosureTracking2015Log) disclosureTracking2015Log).DisclosedMethodNameByType(disclosureTracking2015Log.BorrowerDisclosedMethod);
          case DisclosureProperty2015.BorrowerPresumedReceivedDate:
            return disclosureTracking2015Log.IsBorrowerPresumedDateLocked ? disclosureTracking2015Log.LockedBorrowerPresumedReceivedDate.ToString("MM/dd/yyyy") : disclosureTracking2015Log.BorrowerPresumedReceivedDate.ToString("MM/dd/yyyy");
          case DisclosureProperty2015.BorrowerActualReceivedDate:
            return disclosureTracking2015Log.BorrowerActualReceivedDate.ToString("MM/dd/yyyy");
          case DisclosureProperty2015.BorrowerType:
            return disclosureTracking2015Log.IsBorrowerTypeLocked ? disclosureTracking2015Log.LockedBorrowerType : disclosureTracking2015Log.BorrowerType;
          case DisclosureProperty2015.CoBorrowerReceivedMethod:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).DisclosedMethodNameByType(disclosureTracking2015Log.CoBorrowerDisclosedMethod) : ((DisclosureTracking2015Log) disclosureTracking2015Log).DisclosedMethodNameByType(disclosureTracking2015Log.CoBorrowerDisclosedMethod);
          case DisclosureProperty2015.CoBorrowerPresumedReceivedDate:
            return disclosureTracking2015Log.IsCoBorrowerPresumedDateLocked ? disclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate.ToString("MM/dd/yyyy") : disclosureTracking2015Log.CoBorrowerPresumedReceivedDate.ToString("MM/dd/yyyy");
          case DisclosureProperty2015.CoBorrowerActualReceivedDate:
            return disclosureTracking2015Log.CoBorrowerActualReceivedDate.ToString("MM/dd/yyyy");
          case DisclosureProperty2015.CoBorrowerType:
            return disclosureTracking2015Log.IsCoBorrowerTypeLocked ? disclosureTracking2015Log.LockedCoBorrowerType : disclosureTracking2015Log.CoBorrowerType;
          case DisclosureProperty2015.LoanSnapshot_DisclosedAPR:
            return disclosureTracking2015Log.DisclosedAPR;
          case DisclosureProperty2015.LoanSnapshot_DisclosedRate:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).DisclosedApr.ComputedValue : ((DisclosureTracking2015Log) disclosureTracking2015Log).DisclosedAPRValue;
          case DisclosureProperty2015.LoanSnapshot_LoanProgram:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).LoanProgram : ((DisclosureTrackingBase) disclosureTracking2015Log).LoanProgram;
          case DisclosureProperty2015.LoanSnapshot_LoanAmount:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).LoanAmount.ToString() : ((DisclosureTrackingBase) disclosureTracking2015Log).LoanAmount;
          case DisclosureProperty2015.LoanSnapshot_FinanceCharge:
            return disclosureTracking2015Log.FinanceCharge;
          case DisclosureProperty2015.LoanSnapshot_ApplicationDate:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).ApplicationDate.ToString("MM/dd/yyyy") : ((DisclosureTrackingBase) disclosureTracking2015Log).ApplicationDate.ToString("MM/dd/yyyy");
          case DisclosureProperty2015.Reason_RevisionsrequestedbytheConsumer:
            return disclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer.ToString();
          case DisclosureProperty2015.Reason_InterestRatedependentcharges:
            return disclosureTracking2015Log.CDReasonIsInterestRateDependentCharges.ToString();
          case DisclosureProperty2015.Reason_Other:
            return disclosureTracking2015Log.CDReasonIsOther.ToString();
          case DisclosureProperty2015.OtherReason:
            return disclosureTracking2015Log is EnhancedDisclosureTracking2015Log ? ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).ClosingDisclosure.OtherDescription : ((DisclosureTracking2015Log) disclosureTracking2015Log).CDReasonOther;
          case DisclosureProperty2015.ChangedCircumstanceDetails:
            return disclosureTracking2015Log is DisclosureTracking2015Log ? ((DisclosureTracking2015Log) disclosureTracking2015Log).ChangeInCircumstance : ((EnhancedDisclosureTracking2015Log) disclosureTracking2015Log).ChangeInCircumstance;
          case DisclosureProperty2015.ReasonComments:
            return disclosureTracking2015Log.ChangeInCircumstanceComments;
          case DisclosureProperty2015.ReasonChangeinAPR:
            return disclosureTracking2015Log.CDReasonIsChangeInAPR.ToString();
          case DisclosureProperty2015.ReasonChangeinLoanProduct:
            return disclosureTracking2015Log.CDReasonIsChangeInLoanProduct.ToString();
          case DisclosureProperty2015.ReasonPrepaymentPenaltyAdded:
            return disclosureTracking2015Log.CDReasonIsPrepaymentPenaltyAdded.ToString();
          case DisclosureProperty2015.ReasonChangeinSettlementCharges:
            return disclosureTracking2015Log.CDReasonIsChangeInSettlementCharges.ToString();
          case DisclosureProperty2015.ReasonChangedCircumstanceEligibility:
            return disclosureTracking2015Log.CDReasonIsChangedCircumstanceEligibility.ToString();
          case DisclosureProperty2015.Reason24hourAdvancedPreview:
            return disclosureTracking2015Log.CDReasonIs24HourAdvancePreview.ToString();
          case DisclosureProperty2015.ReasonToleranceCure:
            return disclosureTracking2015Log.CDReasonIsToleranceCure.ToString();
          case DisclosureProperty2015.ReasonClericalErrorCorrection:
            return disclosureTracking2015Log.CDReasonIsClericalErrorCorrection.ToString();
          case DisclosureProperty2015.BorrowerReceivedOtherMethod:
            return disclosureTracking2015Log.BorrowerDisclosedMethodOther;
          case DisclosureProperty2015.CoBorrowerReceivedOtherMethod:
            return disclosureTracking2015Log.CoBorrowerDisclosedMethodOther;
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
}
