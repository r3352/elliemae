// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IAddress
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for Address class.</summary>
  /// <exclude />
  [Guid("899028F8-FC70-46f1-80D2-3CED72171EA8")]
  public interface IAddress
  {
    string Street1 { get; set; }

    string Street2 { get; set; }

    string City { get; set; }

    string State { get; set; }

    string Zip { get; set; }

    string UnitType { get; set; }
  }
}
