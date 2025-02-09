// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.ModuleLicenseCollection
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  [Serializable]
  public class ModuleLicenseCollection : IEnumerable
  {
    private ArrayList features = new ArrayList();

    public int Count => this.features.Count;

    public int this[int index] => (int) this.features[index];

    public void Add(int module)
    {
      if (this.features.Contains((object) module))
        return;
      this.features.Add((object) module);
    }

    public void Remove(int module) => this.features.Remove((object) module);

    public bool Contains(int module) => this.features.Contains((object) module);

    public void Clear() => this.features.Clear();

    public IEnumerator GetEnumerator() => this.features.GetEnumerator();

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < this.features.Count; ++index)
        str = str + (index > 0 ? (object) "," : (object) "") + (object) (int) this.features[index];
      return str;
    }

    public static ModuleLicenseCollection Parse(string featureStr)
    {
      ModuleLicenseCollection licenseCollection = new ModuleLicenseCollection();
      string str = featureStr;
      char[] chArray = new char[1]{ ',' };
      foreach (string s in str.Split(chArray))
      {
        try
        {
          licenseCollection.Add(int.Parse(s));
        }
        catch
        {
        }
      }
      return licenseCollection;
    }
  }
}
