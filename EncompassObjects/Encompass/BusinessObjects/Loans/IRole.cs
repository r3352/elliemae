// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IRole
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Interface for Role class.</summary>
  /// <exclude />
  [Guid("C71F6021-C7E5-4281-A15E-D41A8F7BDDD6")]
  public interface IRole
  {
    int ID { get; }

    string Name { get; }

    string Abbreviation { get; }

    bool Protected { get; }

    RolePersonas EligiblePersonas { get; }

    RoleUserGroups EligibleGroups { get; }
  }
}
