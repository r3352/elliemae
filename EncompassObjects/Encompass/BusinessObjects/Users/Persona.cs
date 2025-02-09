// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Persona
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Represents a defined Persona in the Encompass system.</summary>
  public class Persona : SessionBoundObject, IPersona
  {
    private EllieMae.EMLite.Common.Persona persona;

    internal Persona(Session session, EllieMae.EMLite.Common.Persona persona)
      : base(session)
    {
      this.persona = persona;
    }

    /// <summary>Get the unique indenitifer for the Persona.</summary>
    public int ID => this.persona.ID;

    /// <summary>Gets the viewable name for the Persona.</summary>
    public string Name => this.persona.Name;

    /// <summary>Gets the index of the Persona in the display order.</summary>
    public int DisplayOrder => this.persona.DisplayOrder;

    /// <summary>Returns a string representation of the object.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.Persona.Name" /> of the Persona.</returns>
    public override string ToString() => this.Name;

    /// <summary>
    /// Provides a hash code implementation for the Persona class.
    /// </summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.Persona.ID" /> of the Persona.</returns>
    public override int GetHashCode() => this.ID;

    /// <summary>
    /// Provides an equality implementation for the Persona class.
    /// </summary>
    /// <param name="obj">The Persona object to which to compare the current object.</param>
    /// <returns>Returns <c>true</c> if the objects represent the same Persona, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      Persona persona = obj as Persona;
      return obj != null && persona.ID == this.ID;
    }

    /// <summary>Overrides the equality operator for a Persona object.</summary>
    public static bool operator ==(Persona p1, Persona p2)
    {
      return object.Equals((object) p1, (object) p2);
    }

    /// <summary>
    /// Overrides the inequality operator for a Persona object.
    /// </summary>
    public static bool operator !=(Persona p1, Persona p2) => !(p1 == p2);

    internal EllieMae.EMLite.Common.Persona Unwrap() => this.persona;
  }
}
