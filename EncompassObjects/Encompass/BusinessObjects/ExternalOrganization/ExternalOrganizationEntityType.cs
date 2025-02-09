// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganizationEntityType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Defines the possible channel type for an external organization
  /// </summary>
  [Guid("C889B1FB-A135-4F64-8ED0-9EDCEADED183")]
  public enum ExternalOrganizationEntityType : byte
  {
    /// <summary>No External Organization Type selected</summary>
    None,
    /// <summary>Indicates an organization is broker</summary>
    Broker,
    /// <summary>Indicates an organization is correspondent</summary>
    Correspondent,
    /// <summary>
    /// Indicates an organization is both broker and correspondent
    /// </summary>
    Both,
  }
}
