// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IFeeManagementRecords
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("5D48F60A-B698-4C0A-BC4C-AAA98E7ECF99")]
  public interface IFeeManagementRecords
  {
    FeeManagementRecord this[string name] { get; }
  }
}
