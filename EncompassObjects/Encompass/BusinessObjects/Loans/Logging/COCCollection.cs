// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.COCCollection
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// The class to expose COCFields in Disclosure Tracking log 2015 to SDK.
  /// </summary>
  public class COCCollection : ICOCCollection
  {
    private readonly string alertTriggerFielDID;
    private readonly DateTime changedCircumstanceDate;
    private readonly DateTime revisedDueDate;
    private readonly string changedCircumstanceDescription;
    private readonly string changedCircumstanceComments;
    private readonly string changedCircumstanceReason;
    private readonly string changedCircumstanceReasonOther;
    private readonly string changedCircumstanceCategory;
    private readonly string alertID;
    private readonly string description;
    private readonly long amount;
    private readonly int orderId;

    internal COCCollection(
      string AltTrgFldID,
      DateTime CocDate,
      DateTime RevDueDate,
      string CocDescription,
      string CocComments,
      string cocReason,
      string cocReasonOther,
      string cocCategory,
      string AlertID,
      string Description,
      long amt,
      int OrderId)
    {
      this.alertTriggerFielDID = AltTrgFldID;
      this.changedCircumstanceDate = CocDate;
      this.revisedDueDate = RevDueDate;
      this.changedCircumstanceDescription = CocDescription;
      this.changedCircumstanceComments = CocComments;
      this.changedCircumstanceReason = cocReason;
      this.changedCircumstanceReasonOther = cocReasonOther;
      this.changedCircumstanceCategory = cocCategory;
      this.alertID = AlertID;
      this.description = Description;
      this.amount = amt;
      this.orderId = OrderId;
    }

    /// <summary>Gets the Alert Trigger Field Id</summary>
    public string AlertTriggerFielDID => this.alertTriggerFielDID;

    /// <summary>Gets the Changed Circumstance Date</summary>
    public DateTime ChangedCircumstanceDate => this.changedCircumstanceDate;

    /// <summary>Gets the Revised Due Date</summary>
    public DateTime RevisedDueDate => this.revisedDueDate;

    /// <summary>Gets the Changed Circumstance Description</summary>
    public string ChangedCircumstanceDescription => this.changedCircumstanceDescription;

    /// <summary>Gets the Changed Circumstance Comments</summary>
    public string ChangedCircumstanceComments => this.changedCircumstanceComments;

    /// <summary>Gets the Changed Circumstance Reason</summary>
    public string ChangedCircumstanceReason => this.changedCircumstanceReason;

    /// <summary>Gets the Changed Circumstance Reason for Other</summary>
    public string ChangedCircumstanceReasonOther => this.changedCircumstanceReasonOther;

    /// <summary>Gets the Changed Circumstance Category</summary>
    public string ChangedCircumstanceCategory => this.changedCircumstanceCategory;

    /// <summary>Gets the AlertId</summary>
    public string AlertID => this.alertID;

    /// <summary>Gets the Description</summary>
    public string Description => this.description;

    /// <summary>Gets the Amount</summary>
    public long Amount => this.amount;

    /// <summary>Gets the Order Id</summary>
    public int OrderId => this.orderId;
  }
}
