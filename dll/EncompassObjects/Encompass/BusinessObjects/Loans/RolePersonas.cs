// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.RolePersonas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class RolePersonas : IRolePersonas, IEnumerable
  {
    private List<Persona> personas = new List<Persona>();

    internal RolePersonas(Session session, int[] personaIds)
    {
      if (personaIds == null)
        return;
      foreach (int personaId in personaIds)
      {
        Persona personaById = session.Users.Personas.GetPersonaByID(personaId);
        if (personaById != (Persona) null)
          this.personas.Add(personaById);
      }
    }

    public int Count => this.personas.Count;

    public Persona this[int index] => this.personas[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.personas.GetEnumerator();
  }
}
