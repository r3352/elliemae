// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IRole
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
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
