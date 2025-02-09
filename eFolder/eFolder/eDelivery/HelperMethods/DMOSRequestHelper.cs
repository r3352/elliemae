// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.HelperMethods.DMOSRequestHelper
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.HelperMethods
{
  public static class DMOSRequestHelper
  {
    public static void SetPartyEntityType(
      Party party,
      string recipientRole,
      string borrowerId,
      string userId)
    {
      party.loanEntity = new LoanEntity();
      switch (recipientRole.Trim().ToLower())
      {
        case "appraiser":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Appraiser.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "assign to":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.AssignTo.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "borrower":
          party.loanEntity.entityId = borrowerId;
          party.loanEntity.entityType = "urn:elli:encompass:loan:borrower";
          break;
        case "broker":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Broker.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "builder":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Builder.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "buyer's agent":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.BuyerAgent.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "buyer's attorney":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.BuyerAttorney.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "coborrower":
          party.loanEntity.entityId = borrowerId;
          party.loanEntity.entityType = "urn:elli:encompass:loan:coborrower";
          break;
        case "credit company":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.CreditCompany.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "custom category #1":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Custom1.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "custom category #2":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Custom2.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "custom category #3":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Custom3.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "custom category #4":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Custom4.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "doc signing":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.DocSigning.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "docs prepared by":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.DocsPrepared.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "escrow company":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Escrow.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "financial planner":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.FinancialPlanner.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "flood insurance":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.FloodInsurance.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "hazard insurance":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.HazardInsurance.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "investor":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Investor.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "lender":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Lender.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "loan officer":
          party.loanEntity.entityId = Guid.NewGuid().ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:unknown";
          break;
        case "loan processor":
          party.loanEntity.entityId = Guid.NewGuid().ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:unknown";
          break;
        case "mortgage insurance":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.MortgageInsurance.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "nonborrowingowner":
          party.loanEntity.entityId = borrowerId;
          party.loanEntity.entityType = "urn:elli:encompass:loan:nonBorrowingOwner";
          break;
        case "notary":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Notary.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "originator":
          party.loanEntity.entityId = string.IsNullOrEmpty(userId) ? Session.UserID : userId;
          party.loanEntity.entityType = "urn:elli:encompass:user";
          break;
        case "seller":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Seller.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "seller's agent":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.SellerAgent.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "seller's attorney":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.SellerAttorney.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "seller2":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Seller2.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "seller3":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Seller3.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "seller4":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Seller4.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "servicing":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Servicing.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "settlement agent":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.SettlementAgent.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "surveyor":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Surveyor.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "title insurance":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Title.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "underwriter":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Underwriter.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        case "warehouse":
          party.loanEntity.entityId = FileContactRecord.ContactTypes.Warehouse.ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:fileContact";
          break;
        default:
          party.loanEntity.entityId = Guid.NewGuid().ToString();
          party.loanEntity.entityType = "urn:elli:encompass:loan:unknown";
          break;
      }
    }
  }
}
