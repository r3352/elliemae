// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.RolePersonas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Provides the collection or <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> objects associated with a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Role" />.
  /// </summary>
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

    /// <summary>Gets the number of personas in the collection</summary>
    public int Count => this.personas.Count;

    /// <summary>Returns a persona from the collection by index.</summary>
    /// <param name="index">The index of the desired persona.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> at the specified index in the collection.</returns>
    public Persona this[int index] => this.personas[index];

    /// <summary>Provides a enumerator for the collection.</summary>
    /// <returns>Returns an IEnumerator for enumerating the collection.</returns>
    public IEnumerator GetEnumerator() => (IEnumerator) this.personas.GetEnumerator();
  }
}
