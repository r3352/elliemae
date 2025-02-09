// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ExtensibleUtil
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// This class acts as an internal wrapper allowing us to do a dynamic
  /// methodinfo invoke; an't put into Serializer as don't want on public
  /// API; can't put into Serializer&lt;T&gt; since we need to invoke
  /// across classes
  /// </summary>
  internal static class ExtensibleUtil
  {
    /// <summary>
    /// All this does is call GetExtendedValuesTyped with the correct type for "instance";
    /// this ensures that we don't get issues with subclasses declaring conflicting types -
    /// the caller must respect the fields defined for the type they pass in.
    /// </summary>
    internal static IEnumerable<TValue> GetExtendedValues<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      bool singleton,
      bool allowDefinedTag)
    {
      foreach (TValue extendedValue in ExtensibleUtil.GetExtendedValues((TypeModel) RuntimeTypeModel.Default, typeof (TValue), instance, tag, format, singleton, allowDefinedTag))
        yield return extendedValue;
    }

    /// <summary>
    /// All this does is call GetExtendedValuesTyped with the correct type for "instance";
    /// this ensures that we don't get issues with subclasses declaring conflicting types -
    /// the caller must respect the fields defined for the type they pass in.
    /// </summary>
    internal static IEnumerable GetExtendedValues(
      TypeModel model,
      Type type,
      IExtensible instance,
      int tag,
      DataFormat format,
      bool singleton,
      bool allowDefinedTag)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (tag <= 0)
        throw new ArgumentOutOfRangeException(nameof (tag));
      IExtension extn = instance.GetExtensionObject(false);
      if (extn != null)
      {
        Stream stream = extn.BeginQuery();
        object extendedValue = (object) null;
        ProtoReader reader = (ProtoReader) null;
        try
        {
          reader = ProtoReader.Create(stream, model, new SerializationContext());
          while (model.TryDeserializeAuxiliaryType(reader, format, tag, type, ref extendedValue, true, true, false, false, (object) null) && extendedValue != null)
          {
            if (!singleton)
            {
              yield return extendedValue;
              extendedValue = (object) null;
            }
          }
          if (singleton && extendedValue != null)
            yield return extendedValue;
        }
        finally
        {
          ProtoReader.Recycle(reader);
          extn.EndQuery(stream);
        }
      }
    }

    internal static void AppendExtendValue(
      TypeModel model,
      IExtensible instance,
      int tag,
      DataFormat format,
      object value)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      IExtension extensionObject = instance.GetExtensionObject(true);
      if (extensionObject == null)
        throw new InvalidOperationException("No extension object available; appended data would be lost.");
      bool commit = false;
      Stream stream = extensionObject.BeginAppend();
      try
      {
        using (ProtoWriter writer = ProtoWriter.Create(stream, model))
        {
          model.TrySerializeAuxiliaryType(writer, (Type) null, format, tag, value, false, (object) null);
          writer.Close();
        }
        commit = true;
      }
      finally
      {
        extensionObject.EndAppend(stream, commit);
      }
    }
  }
}
