// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.UserPersonas
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class UserPersonas : IUserPersonas, IEnumerable
  {
    private User user;
    private ArrayList personas = new ArrayList();

    internal UserPersonas(User user, Persona[] personaList)
    {
      this.user = user;
      foreach (Persona persona in personaList)
      {
        Persona personaById = user.Session.Users.Personas.GetPersonaByID(persona.ID);
        if (personaById != (Persona) null)
          this.personas.Add((object) personaById);
      }
    }

    public int Count => this.personas.Count;

    public Persona this[int index] => (Persona) this.personas[index];

    public void Add(Persona p)
    {
      this.user.EnsureAdmin();
      if (this.personas.Contains((object) p))
        return;
      this.personas.Add((object) p);
    }

    public void AddRange(PersonaList personas)
    {
      foreach (Persona persona in (CollectionBase) personas)
        this.Add(persona);
    }

    public void Replace(PersonaList personas)
    {
      this.Clear();
      this.AddRange(personas);
    }

    public void Remove(Persona p)
    {
      this.user.EnsureAdmin();
      if (this.user.ID == "admin" && this.user.Session.Users.Personas.SuperAdministrator.Equals((object) p))
        throw new InvalidOperationException("The admin user cannot have the Super Administrator persona removed");
      this.personas.Remove((object) p);
    }

    public void Clear()
    {
      this.user.EnsureAdmin();
      this.personas.Clear();
      if (!(this.user.ID == "admin"))
        return;
      this.personas.Add((object) this.user.Session.Users.Personas.SuperAdministrator);
    }

    public bool Contains(Persona p) => this.personas.Contains((object) p);

    public IEnumerator GetEnumerator() => this.personas.GetEnumerator();

    public override string ToString()
    {
      string[] strArray = new string[this.Count];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = this[index].Name;
      return string.Join(" + ", strArray);
    }

    internal Persona[] Unwrap()
    {
      Persona[] personaArray = new Persona[this.Count];
      for (int index = 0; index < this.Count; ++index)
        personaArray[index] = this[index].Unwrap();
      return personaArray;
    }
  }
}
