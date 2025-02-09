// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.IgnorePropertySerializer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class IgnorePropertySerializer : DefaultContractResolver
  {
    private readonly Dictionary<System.Type, HashSet<string>> _ignores;

    public IgnorePropertySerializer() => this._ignores = new Dictionary<System.Type, HashSet<string>>();

    public void IgnoreProperty(System.Type type, params string[] jsonPropertyNames)
    {
      if (!this._ignores.ContainsKey(type))
        this._ignores[type] = new HashSet<string>();
      foreach (string jsonPropertyName in jsonPropertyNames)
        this._ignores[type].Add(jsonPropertyName);
    }

    protected override JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty property = base.CreateProperty(member, memberSerialization);
      if (this.IsIgnored(property.DeclaringType, property.PropertyName))
      {
        property.ShouldSerialize = (Predicate<object>) (i => false);
        property.Ignored = true;
      }
      return property;
    }

    private bool IsIgnored(System.Type type, string jsonPropertyName)
    {
      return this._ignores.ContainsKey(type) && this._ignores[type].Contains(jsonPropertyName);
    }
  }
}
