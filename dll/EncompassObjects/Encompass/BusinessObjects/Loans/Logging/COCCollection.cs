// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.COCCollection
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
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

    public string AlertTriggerFielDID => this.alertTriggerFielDID;

    public DateTime ChangedCircumstanceDate => this.changedCircumstanceDate;

    public DateTime RevisedDueDate => this.revisedDueDate;

    public string ChangedCircumstanceDescription => this.changedCircumstanceDescription;

    public string ChangedCircumstanceComments => this.changedCircumstanceComments;

    public string ChangedCircumstanceReason => this.changedCircumstanceReason;

    public string ChangedCircumstanceReasonOther => this.changedCircumstanceReasonOther;

    public string ChangedCircumstanceCategory => this.changedCircumstanceCategory;

    public string AlertID => this.alertID;

    public string Description => this.description;

    public long Amount => this.amount;

    public int OrderId => this.orderId;
  }
}
