// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.ServicingEnum
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class ServicingEnum
  {
    public static string[] DisbursementTypes = new string[10]
    {
      "",
      "Taxes",
      "Hazard Insurance",
      "Mortgage Insurance",
      "Flood Insurance",
      "City Property Tax",
      "Other 1",
      "Other 2",
      "Other 3",
      "USDA Annual Fee"
    };
    public static string[] ServicingPaymentMethodsUI = new string[5]
    {
      "",
      "Check",
      "Automated Clearing House",
      "Lock Box",
      "Wire"
    };

    public static string DisbursementTypesToUI(ServicingDisbursementTypes disbursementType)
    {
      switch (disbursementType)
      {
        case ServicingDisbursementTypes.None:
          return string.Empty;
        case ServicingDisbursementTypes.HazardInsurance:
          return "Hazard Insurance";
        case ServicingDisbursementTypes.MortgageInsurance:
          return "Mortgage Insurance";
        case ServicingDisbursementTypes.FloodInsurance:
          return "Flood Insurance";
        case ServicingDisbursementTypes.CityPropertyTax:
          return "City Property Tax";
        case ServicingDisbursementTypes.Other1:
          return "Other 1";
        case ServicingDisbursementTypes.Other2:
          return "Other 2";
        case ServicingDisbursementTypes.Other3:
          return "Other 3";
        case ServicingDisbursementTypes.USDAMonthlyPremium:
          return "USDA Annual Fee";
        default:
          return disbursementType.ToString();
      }
    }

    public static ServicingDisbursementTypes DisbursementTypesToEnum(string disbursement)
    {
      switch (disbursement)
      {
        case "City Property Tax":
          return ServicingDisbursementTypes.CityPropertyTax;
        case "Flood Insurance":
          return ServicingDisbursementTypes.FloodInsurance;
        case "Hazard Insurance":
          return ServicingDisbursementTypes.HazardInsurance;
        case "Mortgage Insurance":
          return ServicingDisbursementTypes.MortgageInsurance;
        case "Other 1":
          return ServicingDisbursementTypes.Other1;
        case "Other 2":
          return ServicingDisbursementTypes.Other2;
        case "Other 3":
          return ServicingDisbursementTypes.Other3;
        case "Taxes":
          return ServicingDisbursementTypes.Taxes;
        case "USDA Annual Fee":
          return ServicingDisbursementTypes.USDAMonthlyPremium;
        default:
          return ServicingDisbursementTypes.None;
      }
    }

    public static ServicingTransactionTypes TransactionTypesToEnum(string transactionType)
    {
      switch (transactionType)
      {
        case "EscrowDisbursement":
          return ServicingTransactionTypes.EscrowDisbursement;
        case "EscrowDisbursementReversal":
          return ServicingTransactionTypes.EscrowDisbursementReversal;
        case "EscrowInterest":
          return ServicingTransactionTypes.EscrowInterest;
        case "Other":
          return ServicingTransactionTypes.Other;
        case "Payment":
          return ServicingTransactionTypes.Payment;
        case "PaymentReversal":
          return ServicingTransactionTypes.PaymentReversal;
        case "PurchaseAdvice":
          return ServicingTransactionTypes.PurchaseAdvice;
        case "SchedulePayment":
          return ServicingTransactionTypes.SchedulePayment;
        default:
          return ServicingTransactionTypes.None;
      }
    }

    public static string TransactionTypesToUI(ServicingTransactionTypes transactionType)
    {
      switch (transactionType)
      {
        case ServicingTransactionTypes.Payment:
          return "Payment";
        case ServicingTransactionTypes.PaymentReversal:
          return "Payment Reversal";
        case ServicingTransactionTypes.EscrowDisbursement:
          return "Escrow Disbursement";
        case ServicingTransactionTypes.EscrowInterest:
          return "Escrow Interest";
        case ServicingTransactionTypes.EscrowDisbursementReversal:
          return "Escrow Disbursement Reversal";
        case ServicingTransactionTypes.Other:
          return "Other";
        case ServicingTransactionTypes.SchedulePayment:
          return "Schedule Payment";
        case ServicingTransactionTypes.PurchaseAdvice:
          return "Loan Purchase";
        case ServicingTransactionTypes.PrincipalDisbursement:
          return "Principal Disbursement";
        default:
          return "";
      }
    }

    public static string ServicingPaymentMethodsToUI(string paymentMethod)
    {
      switch (paymentMethod)
      {
        case "AutomatedClearingHouse":
          return "Automated Clearing House";
        case "LockBox":
          return "Lock Box";
        default:
          return paymentMethod;
      }
    }

    public static string ServicingPaymentMethodsToUI(ServicingPaymentMethods paymentMethod)
    {
      switch (paymentMethod)
      {
        case ServicingPaymentMethods.AutomatedClearingHouse:
          return "Automated Clearing House";
        case ServicingPaymentMethods.LockBox:
          return "Lock Box";
        case ServicingPaymentMethods.Wire:
          return "Wire";
        default:
          return "Check";
      }
    }

    public static object ToEnum(string value, Type enumType)
    {
      try
      {
        return Enum.Parse(enumType, value, true);
      }
      catch
      {
        return (object) 0;
      }
    }
  }
}
