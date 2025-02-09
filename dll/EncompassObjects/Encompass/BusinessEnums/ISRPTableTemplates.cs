// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ISRPTableTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("6b47dbfb-12d0-4f27-9c6f-9347eef9e8e6")]
  public interface ISRPTableTemplates
  {
    SRPTableTemplate this[int index] { get; }

    SRPTableTemplate this[string name] { get; }

    int Count { get; }

    IEnumerator GetEnumerator();
  }
}
