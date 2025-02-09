// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IDisclosure2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public interface IDisclosure2015
  {
    string ID { get; }

    DateTime Date { get; }

    LogEntryType EntryType { get; }

    string Comments { get; set; }

    LogAlerts RoleAlerts { get; }

    bool IsAlert { get; }

    bool EnabledForCompliance { get; set; }

    StandardDisclosure2015Type DisclosureType { get; }

    string DisclosedBy { get; }

    DateTime DateAdded { get; }

    DeliveryMethod2015 DeliveryMethod { get; set; }

    string DeliveryMethodOther { get; set; }

    object ReceivedDate { get; set; }

    DisclosureFields Fields { get; }

    DisclosedDocuments Documents { get; }

    DateTime BorrowerActualReceivedDate { get; set; }

    DeliveryMethod2015 BorrowerReceivedMethod { get; set; }

    string BorrowerReceivedMethodOther { get; set; }

    DateTime CoBorrowerActualReceivedDate { get; set; }

    DeliveryMethod2015 CoBorrowerReceivedMethod { get; set; }

    string CoBorrowerReceivedMethodOther { get; set; }

    DisclosureRecordType DisclosureRecordType { get; set; }

    DateTime DisclosedDate { get; set; }

    string ManualFulfilledBy { get; set; }

    DateTime ManualFulfilledDateTime { get; set; }

    DeliveryMethod2015 ManualFulfillmentMethod { get; set; }

    string ManualFulfillmentCommets { get; set; }

    DateTime ManualFulfillmentActualReceivedDate { get; set; }

    DateTime ManualFulfillmentPresumedReceivedDate { get; }

    DataObject MergedDoc { get; }

    string BorrowerName { get; }

    string CoBorrowerName { get; }

    string eDisclosureBorrowerName { get; }

    string eDisclosureCoBorrowerName { get; }

    DisclosureRecordType DisclosureRecordType2015 { get; }

    DateTime DisclosedDate2015 { get; }

    DeliveryMethod2015 BorrowerReceivedMethod2015 { get; }

    DeliveryMethod2015 CoBorrowerReceivedMethod2015 { get; }

    DateTime BorrowerActualReceivedDate2015 { get; }

    DateTime CoBorrowerActualReceivedDate2015 { get; }

    int NumberOfNonBorrowingOwnerContact { get; }

    int NumberOfVestingParties { get; }

    int NumberOfGoodFaithChangeOfCircumstance { get; }

    string GetAttribute(string name);

    List<NonBorrowerOwner> GetAllNBOItems();

    ChangeOfCircumstanceItems GetAllCOCItems();

    List<DisclosedVestingFieldsForDisclosure2015> GetAllVestingItems();
  }
}
