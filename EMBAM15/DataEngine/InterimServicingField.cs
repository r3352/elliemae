// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicingField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine.InterimServicing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class InterimServicingField : VirtualField
  {
    private const string FieldPrefix = "ISPAY";
    private InterimServicingProperty property;
    private string virtualFieldID = string.Empty;
    private int payOrder = 1;

    public InterimServicingField(
      InterimServicingProperty property,
      int payOrder,
      string virtualFieldID,
      string description,
      FieldFormat format)
      : base("ISPAY." + property.ToString() + "." + (object) payOrder + "." + virtualFieldID, description, format)
    {
      this.property = property;
      this.payOrder = payOrder;
      this.virtualFieldID = virtualFieldID;
    }

    public InterimServicingField(
      InterimServicingProperty property,
      string virtualFieldID,
      string description,
      FieldFormat format)
      : base("ISPAY." + property.ToString() + "." + virtualFieldID, description, format)
    {
      this.property = property;
      this.payOrder = 0;
      this.virtualFieldID = virtualFieldID;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.InterimServicingFields;

    protected override string Evaluate(LoanData loan)
    {
      if (this.virtualFieldID.ToUpper().StartsWith("NEXTDISBURSEMENT"))
      {
        DateTime minValue = DateTime.MinValue;
        DateTime dateTime = DateTime.MaxValue;
        string str = string.Empty;
        Decimal num = 0M;
        for (int index = 59; index < 73; index += 2)
        {
          DateTime date = Utils.ParseDate((object) loan.GetField("SERVICE.X" + (object) index));
          if (!(date == DateTime.MinValue) && !(date > dateTime))
          {
            num = Utils.ParseDecimal((object) loan.GetField("SERVICE.X" + (object) (index - 1)));
            dateTime = date;
            switch (index)
            {
              case 59:
                str = "Taxes";
                continue;
              case 61:
                str = "Hazard Insurance";
                continue;
              case 63:
                str = "Mortgage Insurance";
                continue;
              case 65:
                str = "Flood Insurance";
                continue;
              case 67:
                str = "City Property Tax";
                continue;
              case 69:
                str = "Other 1";
                continue;
              case 71:
                str = "Other 2";
                continue;
              case 73:
                str = "Other 3";
                continue;
              default:
                continue;
            }
          }
        }
        switch (this.virtualFieldID.ToUpper())
        {
          case "NEXTDISBURSEMENTDATE":
            return !(dateTime == DateTime.MaxValue) ? dateTime.ToString("MM/dd/yyyy") : string.Empty;
          case "NEXTDISBURSEMENTTYPE":
            return str;
          case "NEXTDISBURSEMENTAMOUNT":
            return !(num > 0M) ? string.Empty : num.ToString("N2");
          default:
            return string.Empty;
        }
      }
      else
      {
        ServicingTransactionBase[] servicingTransactions = loan.GetServicingTransactions(true);
        if (servicingTransactions == null || servicingTransactions.Length == 0)
          return string.Empty;
        ServicingTransactionBase servicingTransactionBase = (ServicingTransactionBase) null;
        int num = 0;
        for (int index = servicingTransactions.Length - 1; index >= 0; --index)
        {
          if (this.property == InterimServicingProperty.Payment && servicingTransactions[index] is PaymentTransactionLog || this.property == InterimServicingProperty.Escrow && servicingTransactions[index] is EscrowDisbursementLog || this.property == InterimServicingProperty.Interest && servicingTransactions[index] is EscrowInterestLog || this.property == InterimServicingProperty.Other && servicingTransactions[index] is OtherTransactionLog || this.property == InterimServicingProperty.All)
          {
            if (this.payOrder == num)
            {
              servicingTransactionBase = servicingTransactions[index];
              break;
            }
            ++num;
          }
        }
        if (servicingTransactionBase == null)
          return string.Empty;
        if (this.property == InterimServicingProperty.Payment)
        {
          PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) servicingTransactionBase;
          switch (this.virtualFieldID)
          {
            case "ACCTHOLDER":
              return paymentTransactionLog.AccountHolder;
            case "ACCTNO":
              return paymentTransactionLog.AccountNumber;
            case "ADDESCROW":
              return paymentTransactionLog.AdditionalEscrow.ToString("N2");
            case "ADDPRINCIPAL":
              return paymentTransactionLog.AdditionalPrincipal.ToString("N2");
            case "AMOUNT":
              return paymentTransactionLog.CheckAmount.ToString("N2");
            case "AMTDUE":
              return paymentTransactionLog.TotalAmountDue.ToString("N3");
            case "AMTRECE":
              return paymentTransactionLog.TotalAmountReceived.ToString("N3");
            case "BUYDOWNSUBSIDYAMOUNT":
              return paymentTransactionLog.BuydownSubsidyAmount.ToString("N2");
            case "CHKDATE":
              return !(paymentTransactionLog.CheckDate != DateTime.MinValue) ? "" : paymentTransactionLog.CheckDate.ToString("MM/dd/yyyy");
            case "CHKNO":
              return paymentTransactionLog.CheckNumber;
            case "COMMENTS":
              return paymentTransactionLog.Comments;
            case "DEPODATE":
              return !(paymentTransactionLog.PaymentDepositedDate != DateTime.MinValue) ? "" : paymentTransactionLog.PaymentDepositedDate.ToString("MM/dd/yyyy");
            case "DUEDATE":
              return !(paymentTransactionLog.PaymentDueDate != DateTime.MinValue) ? "" : paymentTransactionLog.PaymentDueDate.ToString("MM/dd/yyyy");
            case "ESCROW":
              return paymentTransactionLog.Escrow.ToString("N2");
            case "INDEXRATE":
              return !(loan?.GetField("4912") == "FiveDecimals") ? paymentTransactionLog.IndexRate.ToString("N3") : paymentTransactionLog.IndexRate.ToString("N5");
            case "INTEREST":
              return paymentTransactionLog.Interest.ToString("N2");
            case "INTNAME":
              return paymentTransactionLog.InstitutionName;
            case "INTRATE":
              return paymentTransactionLog.InterestRate.ToString("N3");
            case "LATEDATE":
              return !(paymentTransactionLog.LatePaymentDate != DateTime.MinValue) ? "" : paymentTransactionLog.LatePaymentDate.ToString("MM/dd/yyyy");
            case "LATEFEE":
              return paymentTransactionLog.LateFee.ToString("N2");
            case "MISCFEE":
              return paymentTransactionLog.MiscFee.ToString("N2");
            case "PAYMETHOD":
              return paymentTransactionLog.PaymentMethod.ToString();
            case "PRINCIPAL":
              return paymentTransactionLog.Principal.ToString("N2");
            case "RDATE":
              return !(paymentTransactionLog.PaymentReceivedDate != DateTime.MinValue) ? "" : paymentTransactionLog.PaymentReceivedDate.ToString("MM/dd/yyyy");
            case "ROUTINE":
              return paymentTransactionLog.InstitutionRouting;
            case "SDATE":
              return !(paymentTransactionLog.StatementDate != DateTime.MinValue) ? "" : paymentTransactionLog.StatementDate.ToString("MM/dd/yyyy");
          }
        }
        else if (this.property == InterimServicingProperty.Escrow)
        {
          EscrowDisbursementLog escrowDisbursementLog = (EscrowDisbursementLog) servicingTransactionBase;
          switch (this.virtualFieldID)
          {
            case "ESCROWTYPE":
              return escrowDisbursementLog.DisbursementType.ToString();
            case "INTNAME":
              return escrowDisbursementLog.InstitutionName;
            case "DUEDATE":
              return !(escrowDisbursementLog.DisbursementDueDate != DateTime.MinValue) ? "" : escrowDisbursementLog.DisbursementDueDate.ToString("MM/dd/yyyy");
            case "DATE":
              return !(escrowDisbursementLog.TransactionDate != DateTime.MinValue) ? "" : escrowDisbursementLog.TransactionDate.ToString("MM/dd/yyyy");
            case "AMT":
              return escrowDisbursementLog.TransactionAmount.ToString("N2");
            case "COMMENTS":
              return escrowDisbursementLog.Comments;
          }
        }
        else if (this.property == InterimServicingProperty.Interest)
        {
          EscrowInterestLog escrowInterestLog = (EscrowInterestLog) servicingTransactionBase;
          switch (this.virtualFieldID)
          {
            case "DATE":
              return !(escrowInterestLog.IncurredDate != DateTime.MinValue) ? "" : escrowInterestLog.IncurredDate.ToString("MM/dd/yyyy");
            case "AMT":
              return escrowInterestLog.InterestAmount.ToString("N2");
            case "COMMENTS":
              return escrowInterestLog.Comments;
          }
        }
        else if (this.property == InterimServicingProperty.Other)
        {
          OtherTransactionLog otherTransactionLog = (OtherTransactionLog) servicingTransactionBase;
          switch (this.virtualFieldID)
          {
            case "ACCTNO":
              return otherTransactionLog.AccountNumber;
            case "AMT":
              return otherTransactionLog.TransactionAmount.ToString("N2");
            case "COMMENTS":
              return otherTransactionLog.Comments;
            case "DATE":
              return !(otherTransactionLog.TransactionDate != DateTime.MinValue) ? "" : otherTransactionLog.TransactionDate.ToString("MM/dd/yyyy");
            case "INTNAME":
              return otherTransactionLog.InstitutionName;
            case "REFNO":
              return otherTransactionLog.Reference;
            case "ROUTINE":
              return otherTransactionLog.InstitutionRouting;
          }
        }
        else if (this.property == InterimServicingProperty.All)
        {
          switch (this.virtualFieldID)
          {
            case "AMT":
              return servicingTransactionBase.TransactionAmount.ToString("N2");
            case "CREATEDBY":
              return servicingTransactionBase.CreatedByName;
            case "CREATEDDATETIME":
              return servicingTransactionBase.CreatedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
            case "DATE":
              return !(servicingTransactionBase.TransactionDate != DateTime.MinValue) ? "" : servicingTransactionBase.TransactionDate.ToString("MM/dd/yyyy");
            case "MODIFIEDBY":
              return servicingTransactionBase.ModifiedByName;
            case "MODIFIEDDATETIME":
              return !(servicingTransactionBase.ModifiedDateTime != DateTime.MinValue) ? "" : servicingTransactionBase.ModifiedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
            case "TYPE":
              return servicingTransactionBase.TransactionType.ToString();
          }
        }
        else if (this.property == InterimServicingProperty.Principal)
        {
          PrincipalDisbursementLog principalDisbursementLog = (PrincipalDisbursementLog) servicingTransactionBase;
          switch (this.virtualFieldID)
          {
            case "DATE":
              return !(principalDisbursementLog.TransactionDate != DateTime.MinValue) ? "" : principalDisbursementLog.TransactionDate.ToString("MM/dd/yyyy");
            case "AMT":
              return principalDisbursementLog.TransactionAmount.ToString("N2");
            case "INTNAME":
              return principalDisbursementLog.InstitutionName;
            case "COMMENTS":
              return principalDisbursementLog.Comments;
          }
        }
        return string.Empty;
      }
    }

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return edition == EncompassEdition.Banker;
    }

    public string VirtualFieldID => this.virtualFieldID;

    public InterimServicingProperty Property => this.property;

    public int PayOrder => this.payOrder;
  }
}
