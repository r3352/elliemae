// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IPersonas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("F9904A1B-1ECE-4d14-934D-340A1181C7D1")]
  public interface IPersonas
  {
    int Count { get; }

    Persona this[int index] { get; }

    Persona GetPersonaByID(int personaId);

    Persona GetPersonaByName(string personaName);

    Persona SuperAdministrator { get; }

    Persona Administrator { get; }

    IEnumerator GetEnumerator();
  }
}
