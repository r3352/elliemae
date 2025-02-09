// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IPersona
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("EB38C370-E55B-4157-8C74-478C1954CA54")]
  public interface IPersona
  {
    int ID { get; }

    string Name { get; }

    int DisplayOrder { get; }
  }
}
