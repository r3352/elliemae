// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.PropertyType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Defines the possible property types for a loan.</summary>
  [Guid("78B9FDC7-FD9D-4854-818B-C8A09E015221")]
  public enum PropertyType
  {
    /// <summary>No property type is specified </summary>
    Unspecified,
    /// <summary>Property is attached to another unit</summary>
    Attached,
    /// <summary>Property is part of a multi-unit condominium</summary>
    Condominium,
    /// <summary>Property is part of a Co-op</summary>
    Coop,
    /// <summary>Property is a detached residence</summary>
    Detached,
    /// <summary>Property is part of a high-rise condo</summary>
    HighRiseCondo,
    /// <summary>Property is a manufactured home</summary>
    MfdHousing,
    /// <summary>Property is part of a PUD</summary>
    PUD,
    /// <summary>Property is a detached condominium</summary>
    DetachedCondo,
    /// <summary>Property is a Manufactured/Condo/PUD/Co-op</summary>
    MfdCondoPUDCoop,
    /// <summary>Property is a MH Select</summary>
    MHSelect,
  }
}
