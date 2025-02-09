// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.INonBorrowerOwner
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for INonBorrowerOwner class.</summary>
  /// <exclude />
  [Guid("8C024F32-CA0E-47CD-A1B8-10FE7A6AC994")]
  public interface INonBorrowerOwner
  {
    DeliveryMethod2015 DisclosedMethod { get; set; }

    string DisclosedMethodOther { get; set; }

    DateTime PresumedReceivedDate { get; set; }

    DateTime lockedPresumedReceivedDate { get; set; }

    bool IsPresumedDateLocked { get; set; }

    DateTime ActualReceivedDate { get; set; }

    bool IsBorrowerTypeLocked { get; set; }

    string BorrowerType { get; }

    string LockedBorrowerType { get; }

    bool IseDisclosure { get; }

    EDisclosureStatusForNBO EDisclosureStatus { get; }
  }
}
