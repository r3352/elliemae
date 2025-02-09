// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Persona
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class Persona : SessionBoundObject, IPersona
  {
    private Persona persona;

    internal Persona(Session session, Persona persona)
      : base(session)
    {
      this.persona = persona;
    }

    public int ID => this.persona.ID;

    public string Name => this.persona.Name;

    public int DisplayOrder => this.persona.DisplayOrder;

    public override string ToString() => this.Name;

    public override int GetHashCode() => this.ID;

    public override bool Equals(object obj)
    {
      Persona persona = obj as Persona;
      return obj != null && persona.ID == this.ID;
    }

    public static bool operator ==(Persona p1, Persona p2)
    {
      return object.Equals((object) p1, (object) p2);
    }

    public static bool operator !=(Persona p1, Persona p2) => !(p1 == p2);

    internal Persona Unwrap() => this.persona;
  }
}
