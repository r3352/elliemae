// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IContactStatus
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("F9BD1459-685E-412f-BF74-D5AFB1152FC0")]
  public interface IContactStatus
  {
    int ID { get; }

    string Name { get; }

    string ToString();

    bool Equals(object o);
  }
}
