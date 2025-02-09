// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.EnumItem
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [ComVisible(false)]
  public abstract class EnumItem
  {
    private int id;
    private string name;

    protected internal EnumItem(int id, string name)
    {
      this.id = id;
      this.name = name;
    }

    public int ID => this.id;

    public string Name => this.name;

    public override string ToString() => this.name;

    public override bool Equals(object obj)
    {
      return (object) (obj as EnumItem) != null && this.GetType().Equals(obj.GetType()) && this.ID == ((EnumItem) obj).ID;
    }

    public override int GetHashCode() => this.ID;

    public static bool operator ==(EnumItem a, EnumItem b) => object.Equals((object) a, (object) b);

    public static bool operator !=(EnumItem a, EnumItem b)
    {
      return !object.Equals((object) a, (object) b);
    }
  }
}
