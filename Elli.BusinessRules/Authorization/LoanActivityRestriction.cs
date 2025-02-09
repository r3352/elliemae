// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Authorization.LoanActivityRestriction
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.ElliEnum;
using System.Collections.Generic;

#nullable disable
namespace Elli.BusinessRules.Authorization
{
  public class LoanActivityRestriction
  {
    public LoanActivityRestriction() => this.MissingFields = (IList<string>) new List<string>();

    public LoanActivityType LoanActivity { get; set; }

    public string Message { get; set; }

    public IList<string> MissingFields { get; set; }

    public static LoanActivityRestriction FromFieldValidationError(FieldValidationError error)
    {
      LoanActivityRestriction activityRestriction = new LoanActivityRestriction()
      {
        LoanActivity = LoanActivityType.Unknown,
        Message = error.Message,
        MissingFields = (IList<string>) error.PrerequisiteFieldsNotCompleted
      };
      switch (error.FieldId)
      {
        case "BUTTON_ADDREGISTRATION":
          activityRestriction.LoanActivity = LoanActivityType.AddRegistration;
          break;
        case "BUTTON_AIQANALYZERS":
          activityRestriction.LoanActivity = LoanActivityType.AiqAnalyzers;
          break;
        case "BUTTON_COPYADDR":
          activityRestriction.LoanActivity = LoanActivityType.CopyAddress;
          break;
        case "BUTTON_COPYBRW":
          activityRestriction.LoanActivity = LoanActivityType.CopyFromBorrower;
          break;
        case "BUTTON_EDITREGISTRATION":
          activityRestriction.LoanActivity = LoanActivityType.EditRegistration;
          break;
        case "BUTTON_GETPRICING":
          activityRestriction.LoanActivity = LoanActivityType.GetPricing;
          break;
        case "BUTTON_IMPORTLIAB":
          activityRestriction.LoanActivity = LoanActivityType.ImportLiabilities;
          break;
        case "BUTTON_OCREDIT":
        case "BUTTON_ORDERCREDIT":
          activityRestriction.LoanActivity = LoanActivityType.OrderCredit;
          break;
        case "BUTTON_ORDERAPPRAISAL":
          activityRestriction.LoanActivity = LoanActivityType.OrderAppraisal;
          break;
        case "BUTTON_ORDERFLOOD":
          activityRestriction.LoanActivity = LoanActivityType.OrderFlood;
          break;
        case "BUTTON_ORDERTITLE":
          activityRestriction.LoanActivity = LoanActivityType.OrderTitle;
          break;
        case "BUTTON_PRODUCTANDPRICING":
          activityRestriction.LoanActivity = LoanActivityType.ProductAndPricing;
          break;
        case "BUTTON_REQUESTLOCK":
          activityRestriction.LoanActivity = LoanActivityType.RequestLock;
          break;
        case "BUTTON_SUBFIN":
          activityRestriction.LoanActivity = LoanActivityType.SubFinancing;
          break;
        case "BUTTON_VCREDIT":
        case "BUTTON_VIEWCREDIT":
          activityRestriction.LoanActivity = LoanActivityType.ViewCredit;
          break;
      }
      return activityRestriction;
    }
  }
}
