// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.EnumItem
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// Provides a base class for all Business Enumeration items in the Encompass Object Model.
  /// </summary>
  [ComVisible(false)]
  public abstract class EnumItem
  {
    private int id;
    private string name;

    /// <summary>Constructor</summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    protected internal EnumItem(int id, string name)
    {
      this.id = id;
      this.name = name;
    }

    /// <summary>
    /// Gets the unique ID of the current item within the enumeration.
    /// </summary>
    public int ID => this.id;

    /// <summary>Gets the descriptive name of the item.</summary>
    public string Name => this.name;

    /// <summary>Provides a string representation of the current item.</summary>
    /// <returns>The name of the current item.</returns>
    public override string ToString() => this.name;

    /// <summary>
    /// Compares two enumeration values to see if they represent the same value.
    /// </summary>
    /// <param name="obj">The EnumItem object to which to compare the current object.</param>
    /// <returns>Returns <c>true</c> if the objects are of the same EnumItem-derived
    /// type and they have the same ID, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return (object) (obj as EnumItem) != null && this.GetType().Equals(obj.GetType()) && this.ID == ((EnumItem) obj).ID;
    }

    /// <summary>
    /// Provides a hash value for the current object for use in a Hashtable.
    /// </summary>
    /// <returns>A hash value for the current object.</returns>
    public override int GetHashCode() => this.ID;

    /// <summary>
    /// Provides an equality operator for two EnumItem-derived objects.
    /// </summary>
    /// <param name="a">The first object to compare.</param>
    /// <param name="b">The second object to compare.</param>
    /// <returns>Returns <c>true</c> if the two instances represent the same
    /// enumeration item. That is, they must have the same derived type and
    /// the same ID. Returns <c>false</c> otherwise.</returns>
    public static bool operator ==(EnumItem a, EnumItem b) => object.Equals((object) a, (object) b);

    /// <summary>
    /// Provides an inequality operator for two EnumItem-derived objects.
    /// </summary>
    /// <param name="a">The first object to compare.</param>
    /// <param name="b">The second object to compare.</param>
    /// <returns>Returns <c>true</c> if the two instances represent different
    /// enumeration items. Two EnumItems are different if they are instances of
    /// different derived types or if they have different IDs.
    /// Returns <c>true</c> otherwise.</returns>
    public static bool operator !=(EnumItem a, EnumItem b)
    {
      return !object.Equals((object) a, (object) b);
    }
  }
}
