// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CustomEnumNameProvider
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class CustomEnumNameProvider : DefaultEnumNameProvider
  {
    private Hashtable valueToNameMap;
    private Hashtable nameToValueMap;

    public CustomEnumNameProvider(Type enumType, Hashtable valueToNameMap)
      : base(enumType)
    {
      this.valueToNameMap = valueToNameMap;
      this.nameToValueMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (DictionaryEntry valueToName in valueToNameMap)
        this.nameToValueMap[valueToName.Value] = valueToName.Key;
    }

    public override string GetName(object value)
    {
      return this.valueToNameMap.Contains(value) ? this.valueToNameMap[value].ToString() : base.GetName(value);
    }

    public override string[] GetNames()
    {
      string[] names = new string[this.nameToValueMap.Count];
      int num = 0;
      foreach (string key in (IEnumerable) this.nameToValueMap.Keys)
        names[num++] = key;
      return names;
    }

    public override object GetValue(string name)
    {
      return this.nameToValueMap.Contains((object) name) ? this.nameToValueMap[(object) name] : base.GetValue(name);
    }
  }
}
