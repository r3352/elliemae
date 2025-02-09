// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Serializers.TypeNameSerializer
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;

#nullable disable
namespace Elli.MessageQueues.Serializers
{
  internal class TypeNameSerializer : ITypeNameSerializer
  {
    public string Serialize(Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("TypeNameSerializer.Serialize(type)");
      if (type.FullName == null)
        throw new ArgumentNullException("TypeNameSerializer.Serialize(type.FullName)");
      return type.FullName.Replace('.', '_') + ":" + type.Assembly.GetName().Name.Replace('.', '_');
    }
  }
}
