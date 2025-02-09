// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Personas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Provides access to the set of Personas defined in the Encompass system.
  /// </summary>
  public class Personas : SessionBoundObject, IPersonas, IEnumerable
  {
    private ArrayList personas;
    private Persona superAdmin;
    private Persona admin;

    internal Personas(Session session)
      : base(session)
    {
    }

    /// <summary>Gets the fixed Super Administrator persona.</summary>
    public Persona SuperAdministrator
    {
      get
      {
        this.ensureLoaded();
        return this.superAdmin;
      }
    }

    /// <summary>Gets the fixed Administrator persona.</summary>
    public Persona Administrator
    {
      get
      {
        this.ensureLoaded();
        return this.admin;
      }
    }

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> objects in the collection.
    /// </summary>
    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.personas.Count;
      }
    }

    /// <summary>
    /// Gets a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> from the collection by index.
    /// </summary>
    public Persona this[int index]
    {
      get
      {
        this.ensureLoaded();
        return (Persona) this.personas[index];
      }
    }

    /// <summary>
    /// Gets a Persona from the collection using the unique Persona ID.
    /// </summary>
    /// <param name="personaId">The ID of the persona to retrieve.</param>
    /// <returns>Returns the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> if one is found,
    /// <c>null</c> otherwise.</returns>
    public Persona GetPersonaByID(int personaId)
    {
      this.ensureLoaded();
      foreach (Persona persona in this.personas)
      {
        if (persona.ID == personaId)
          return persona;
      }
      return (Persona) null;
    }

    /// <summary>
    /// Gets a Persona from the collection using the persona's name.
    /// </summary>
    /// <param name="personaName">The name of the persona being retrieved.</param>
    /// <returns>Returns the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> if one is found,
    /// <c>null</c> otherwise.</returns>
    public Persona GetPersonaByName(string personaName)
    {
      this.ensureLoaded();
      foreach (Persona persona in this.personas)
      {
        if (string.Compare(persona.Name, personaName, true) == 0)
          return persona;
      }
      return (Persona) null;
    }

    /// <summary>
    /// Provides for enumeration of the collection of personas.
    /// </summary>
    /// <returns>Returns an enumerator for the collection.</returns>
    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.personas.GetEnumerator();
    }

    private void ensureLoaded()
    {
      if (this.personas != null)
        return;
      this.personas = new ArrayList();
      foreach (EllieMae.EMLite.Common.Persona allPersona in ((IPersonaManager) this.Session.GetObject("PersonaManager")).GetAllPersonas())
      {
        Persona persona = new Persona(this.Session, allPersona);
        this.personas.Add((object) persona);
        if (allPersona.ID == 0)
          this.superAdmin = persona;
        else if (allPersona.ID == 1)
          this.admin = persona;
      }
    }
  }
}
