// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BizCategory
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BizCategory : IComparable
  {
    private int id;
    private string name;

    public BizCategory(int id, string name)
    {
      this.id = id;
      this.name = name;
    }

    public int CategoryID => this.id;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public override string ToString() => this.name;

    public static bool operator ==(BizCategory o1, BizCategory o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(BizCategory o1, BizCategory o2) => !(o1 == o2);

    public override bool Equals(object obj)
    {
      BizCategory bizCategory = obj as BizCategory;
      return !(bizCategory == (BizCategory) null) && bizCategory.CategoryID == this.CategoryID;
    }

    public override int GetHashCode() => this.CategoryID;

    public int CompareTo(object obj) => this.name.CompareTo(((BizCategory) obj).name);
  }
}
