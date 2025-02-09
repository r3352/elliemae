// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Interception.RemotedMethodParameterConverter
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.VersionInterface15;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.Interception
{
  public class RemotedMethodParameterConverter : JsonConverter
  {
    public override bool CanRead => false;

    public override bool CanWrite => true;

    public override bool CanConvert(Type objectType)
    {
      return typeof (RemoteMethodParameter).Equals(objectType);
    }

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      throw new NotSupportedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      RemotedMethodParameterConverter.DictionaryConverter dictionaryConverter = new RemotedMethodParameterConverter.DictionaryConverter();
      RemotedMethodParameterConverter.CollectionConverter collectionConverter = new RemotedMethodParameterConverter.CollectionConverter();
      RemotedMethodParameterConverter.ObjectConverter objectConverter = new RemotedMethodParameterConverter.ObjectConverter();
      serializer.Converters.Add((JsonConverter) dictionaryConverter);
      serializer.Converters.Add((JsonConverter) collectionConverter);
      serializer.Converters.Add((JsonConverter) objectConverter);
      serializer.Serialize(writer, (value as RemoteMethodParameter).Parameter);
      serializer.Converters.Remove((JsonConverter) objectConverter);
      serializer.Converters.Remove((JsonConverter) collectionConverter);
      serializer.Converters.Remove((JsonConverter) dictionaryConverter);
    }

    public class ObjectConverter : JsonConverter
    {
      public override bool CanRead => false;

      public override bool CanWrite => true;

      public override bool CanConvert(Type objectType)
      {
        return RemotedMethodParameterConverter.ObjectConverter.IsComplexType(objectType);
      }

      public static bool IsComplexType(Type objectType)
      {
        Type underlyingType = Nullable.GetUnderlyingType(objectType);
        Type type = !(underlyingType == (Type) null) ? underlyingType : objectType;
        return !typeof (JObject).Equals(type) && !typeof (string).Equals(type) && !typeof (DateTime).Equals(type) && !typeof (Guid).Equals(type) && !type.IsPrimitive && !type.IsEnum && !typeof (IEnumerable).IsAssignableFrom(type);
      }

      public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
      {
        throw new NotSupportedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        switch (value)
        {
          case null:
            writer.WriteNull();
            break;
          case IWhiteListRemoteMethodParam _:
            (value as IWhiteListRemoteMethodParam).SerializeForLog(writer, serializer);
            break;
          case JedVersion _:
            serializer.Serialize(writer, (object) value.ToString());
            break;
          default:
            serializer.Serialize(writer, (object) string.Format("[{0}]", (object) value.GetType()));
            break;
        }
      }
    }

    public class DictionaryConverter : JsonConverter
    {
      public override bool CanRead => false;

      public override bool CanWrite => true;

      public override bool CanConvert(Type objectType)
      {
        if (objectType.IsGenericType)
        {
          if (typeof (Dictionary<,>).IsAssignableFrom(objectType.GetGenericTypeDefinition()))
          {
            Type genericTypeArgument = objectType.GenericTypeArguments[1];
            return !typeof (IWhiteListRemoteMethodParam).IsAssignableFrom(genericTypeArgument) && RemotedMethodParameterConverter.ObjectConverter.IsComplexType(genericTypeArgument);
          }
        }
        return false;
      }

      public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
      {
        throw new NotSupportedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        if (value == null)
          writer.WriteNull();
        else
          serializer.Serialize(writer, (object) new JObject()
          {
            {
              "dictionarySize",
              (JToken) (value as ICollection).Count
            }
          });
      }
    }

    public class CollectionConverter : JsonConverter
    {
      public override bool CanRead => false;

      public override bool CanWrite => true;

      public override bool CanConvert(Type objectType)
      {
        Type elementType;
        return this.IsCollectionType(objectType, out elementType) && !typeof (IWhiteListRemoteMethodParam).IsAssignableFrom(elementType) && RemotedMethodParameterConverter.ObjectConverter.IsComplexType(elementType);
      }

      private bool IsCollectionType(Type objectType, out Type elementType)
      {
        if (objectType.IsArray)
        {
          elementType = objectType.GetElementType();
          return true;
        }
        if (objectType.IsGenericType)
        {
          if (typeof (ICollection<>).IsAssignableFrom(objectType.GetGenericTypeDefinition()))
          {
            elementType = objectType.GenericTypeArguments[0];
            return true;
          }
        }
        elementType = (Type) null;
        return false;
      }

      public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer)
      {
        throw new NotSupportedException();
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
      {
        if (value == null)
          writer.WriteNull();
        else
          serializer.Serialize(writer, (object) new JObject()
          {
            {
              "collectionSize",
              (JToken) (value as ICollection).Count
            }
          });
      }
    }
  }
}
