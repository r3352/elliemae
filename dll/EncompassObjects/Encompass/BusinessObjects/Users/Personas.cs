// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Personas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class Personas : SessionBoundObject, IPersonas, IEnumerable
  {
    private ArrayList personas;
    private Persona superAdmin;
    private Persona admin;

    internal Personas(Session session)
      : base(session)
    {
    }

    public Persona SuperAdministrator
    {
      get
      {
        this.ensureLoaded();
        return this.superAdmin;
      }
    }

    public Persona Administrator
    {
      get
      {
        this.ensureLoaded();
        return this.admin;
      }
    }

    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.personas.Count;
      }
    }

    public Persona this[int index]
    {
      get
      {
        this.ensureLoaded();
        return (Persona) this.personas[index];
      }
    }

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
      foreach (Persona allPersona in ((IPersonaManager) this.Session.GetObject("PersonaManager")).GetAllPersonas())
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
