// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.UserPersonas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> types that are assigned to a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" />.
  /// </summary>
  public class UserPersonas : IUserPersonas, IEnumerable
  {
    private User user;
    private ArrayList personas = new ArrayList();

    internal UserPersonas(User user, EllieMae.EMLite.Common.Persona[] personaList)
    {
      this.user = user;
      foreach (EllieMae.EMLite.Common.Persona persona in personaList)
      {
        Persona personaById = user.Session.Users.Personas.GetPersonaByID(persona.ID);
        if (personaById != (Persona) null)
          this.personas.Add((object) personaById);
      }
    }

    /// <summary>
    /// Returns the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> objects in the collection.
    /// </summary>
    public int Count => this.personas.Count;

    /// <summary>
    /// Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> from the collection by index.
    /// </summary>
    public Persona this[int index] => (Persona) this.personas[index];

    /// <summary>Adds a persona to the collection.</summary>
    /// <param name="p">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> to be added.</param>
    /// <remarks>Each persona can be added at most once to this collection.</remarks>
    public void Add(Persona p)
    {
      this.user.EnsureAdmin();
      if (this.personas.Contains((object) p))
        return;
      this.personas.Add((object) p);
    }

    /// <summary>
    /// Adds a collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> objects to the user's persona list.
    /// </summary>
    /// <param name="personas">A enumerable collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> objects.</param>
    public void AddRange(PersonaList personas)
    {
      foreach (Persona persona in (CollectionBase) personas)
        this.Add(persona);
    }

    /// <summary>Replaces the set of personas with a new set.</summary>
    /// <param name="personas">The set of personas used to replace the current personas.</param>
    public void Replace(PersonaList personas)
    {
      this.Clear();
      this.AddRange(personas);
    }

    /// <summary>
    /// Removes a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> from the collection.
    /// </summary>
    /// <param name="p">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> to be removed.</param>
    public void Remove(Persona p)
    {
      this.user.EnsureAdmin();
      if (this.user.ID == "admin" && this.user.Session.Users.Personas.SuperAdministrator.Equals((object) p))
        throw new InvalidOperationException("The admin user cannot have the Super Administrator persona removed");
      this.personas.Remove((object) p);
    }

    /// <summary>Clears the list of assigned personas.</summary>
    public void Clear()
    {
      this.user.EnsureAdmin();
      this.personas.Clear();
      if (!(this.user.ID == "admin"))
        return;
      this.personas.Add((object) this.user.Session.Users.Personas.SuperAdministrator);
    }

    /// <summary>
    /// Determines if a persona is contained in the collection.
    /// </summary>
    /// <param name="p">The persona to check against.</param>
    /// <returns>Returns <c>true</c> if the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> is in the collection,
    /// <c>false</c> otherwise.</returns>
    public bool Contains(Persona p) => this.personas.Contains((object) p);

    /// <summary>Provides a enumerator for the collection.</summary>
    /// <returns></returns>
    public IEnumerator GetEnumerator() => this.personas.GetEnumerator();

    /// <summary>
    /// Provides a string representation of the set of personas
    ///  </summary>
    /// <returns></returns>
    public override string ToString()
    {
      string[] strArray = new string[this.Count];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = this[index].Name;
      return string.Join(" + ", strArray);
    }

    internal EllieMae.EMLite.Common.Persona[] Unwrap()
    {
      EllieMae.EMLite.Common.Persona[] personaArray = new EllieMae.EMLite.Common.Persona[this.Count];
      for (int index = 0; index < this.Count; ++index)
        personaArray[index] = this[index].Unwrap();
      return personaArray;
    }
  }
}
