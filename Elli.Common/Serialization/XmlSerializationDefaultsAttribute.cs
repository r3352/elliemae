// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.XmlSerializationDefaultsAttribute
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.Serialization
{
  [AttributeUsage(AttributeTargets.Class)]
  public class XmlSerializationDefaultsAttribute : Attribute
  {
    public string DefaultContext { get; set; }

    public bool AppliesToContext(string context)
    {
      return context == null || this.DefaultContext == null || object.Equals((object) this.DefaultContext, (object) context);
    }
  }
}
