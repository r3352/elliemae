// Decompiled with JetBrains decompiler
// Type: Elli.Common.Serialization.XmlSerializationAttribute
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.Serialization
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class XmlSerializationAttribute : Attribute
  {
    public XmlSerializationAttribute()
      : this(true)
    {
    }

    public XmlSerializationAttribute(bool serializable)
    {
      this.ContextName = (string) null;
      this.Serializable = serializable;
    }

    public XmlSerializationAttribute(string contextName)
    {
      this.ContextName = contextName;
      this.Serializable = true;
    }

    public string ContextName { get; private set; }

    public bool Serializable { get; private set; }

    public string XmlName { get; set; }

    public string XmlListItemName { get; set; }

    public bool AlwaysEmitGroupElement { get; set; }

    public bool AppliesToContext(string context)
    {
      return context == null || this.ContextName == null || object.Equals((object) this.ContextName, (object) context);
    }
  }
}
