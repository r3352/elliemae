// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ICOCCollection
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for COCFieldsForDisclosure2015 class.</summary>
  [Guid("DC7EAE90-600D-4DA4-BAB2-B01ECAAC4A6D")]
  public interface ICOCCollection
  {
    string AlertTriggerFielDID { get; }

    DateTime ChangedCircumstanceDate { get; }

    DateTime RevisedDueDate { get; }

    string ChangedCircumstanceDescription { get; }

    string ChangedCircumstanceComments { get; }

    string ChangedCircumstanceReason { get; }

    string ChangedCircumstanceReasonOther { get; }

    string ChangedCircumstanceCategory { get; }

    string AlertID { get; }

    string Description { get; }

    long Amount { get; }

    int OrderId { get; }
  }
}
