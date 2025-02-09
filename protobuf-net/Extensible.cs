// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Extensible
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Simple base class for supporting unexpected fields allowing
  /// for loss-less round-tips/merge, even if the data is not understod.
  /// The additional fields are (by default) stored in-memory in a buffer.
  /// </summary>
  /// <remarks>As an example of an alternative implementation, you might
  /// choose to use the file system (temporary files) as the back-end, tracking
  /// only the paths [such an object would ideally be IDisposable and use
  /// a finalizer to ensure that the files are removed].</remarks>
  /// <seealso cref="T:ProtoBuf.IExtensible" />
  public abstract class Extensible : IExtensible
  {
    private IExtension extensionObject;

    IExtension IExtensible.GetExtensionObject(bool createIfMissing)
    {
      return this.GetExtensionObject(createIfMissing);
    }

    /// <summary>
    /// Retrieves the <see cref="T:ProtoBuf.IExtension">extension</see> object for the current
    /// instance, optionally creating it if it does not already exist.
    /// </summary>
    /// <param name="createIfMissing">Should a new extension object be
    /// created if it does not already exist?</param>
    /// <returns>The extension object if it exists (or was created), or null
    /// if the extension object does not exist or is not available.</returns>
    /// <remarks>The <c>createIfMissing</c> argument is false during serialization,
    /// and true during deserialization upon encountering unexpected fields.</remarks>
    protected virtual IExtension GetExtensionObject(bool createIfMissing)
    {
      return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
    }

    /// <summary>
    /// Provides a simple, default implementation for <see cref="T:ProtoBuf.IExtension">extension</see> support,
    /// optionally creating it if it does not already exist. Designed to be called by
    /// classes implementing <see cref="T:ProtoBuf.IExtensible" />.
    /// </summary>
    /// <param name="createIfMissing">Should a new extension object be
    /// created if it does not already exist?</param>
    /// <param name="extensionObject">The extension field to check (and possibly update).</param>
    /// <returns>The extension object if it exists (or was created), or null
    /// if the extension object does not exist or is not available.</returns>
    /// <remarks>The <c>createIfMissing</c> argument is false during serialization,
    /// and true during deserialization upon encountering unexpected fields.</remarks>
    public static IExtension GetExtensionObject(
      ref IExtension extensionObject,
      bool createIfMissing)
    {
      if (createIfMissing && extensionObject == null)
        extensionObject = (IExtension) new BufferExtension();
      return extensionObject;
    }

    /// <summary>
    /// Appends the value as an additional (unexpected) data-field for the instance.
    /// Note that for non-repeated sub-objects, this equates to a merge operation;
    /// for repeated sub-objects this adds a new instance to the set; for simple
    /// values the new value supercedes the old value.
    /// </summary>
    /// <remarks>Note that appending a value does not remove the old value from
    /// the stream; avoid repeatedly appending values for the same field.</remarks>
    /// <typeparam name="TValue">The type of the value to append.</typeparam>
    /// <param name="instance">The extensible object to append the value to.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="value">The value to append.</param>
    public static void AppendValue<TValue>(IExtensible instance, int tag, TValue value)
    {
      Extensible.AppendValue<TValue>(instance, tag, DataFormat.Default, value);
    }

    /// <summary>
    /// Appends the value as an additional (unexpected) data-field for the instance.
    /// Note that for non-repeated sub-objects, this equates to a merge operation;
    /// for repeated sub-objects this adds a new instance to the set; for simple
    /// values the new value supercedes the old value.
    /// </summary>
    /// <remarks>Note that appending a value does not remove the old value from
    /// the stream; avoid repeatedly appending values for the same field.</remarks>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="format">The data-format to use when encoding the value.</param>
    /// <param name="instance">The extensible object to append the value to.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="value">The value to append.</param>
    public static void AppendValue<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      TValue value)
    {
      ExtensibleUtil.AppendExtendValue((TypeModel) RuntimeTypeModel.Default, instance, tag, format, (object) value);
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// The value returned is the composed value after merging any duplicated content; if the
    /// value is "repeated" (a list), then use GetValues instead.
    /// </summary>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <returns>The effective value of the field, or the default value if not found.</returns>
    public static TValue GetValue<TValue>(IExtensible instance, int tag)
    {
      return Extensible.GetValue<TValue>(instance, tag, DataFormat.Default);
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// The value returned is the composed value after merging any duplicated content; if the
    /// value is "repeated" (a list), then use GetValues instead.
    /// </summary>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="format">The data-format to use when decoding the value.</param>
    /// <returns>The effective value of the field, or the default value if not found.</returns>
    public static TValue GetValue<TValue>(IExtensible instance, int tag, DataFormat format)
    {
      TValue obj;
      Extensible.TryGetValue<TValue>(instance, tag, format, out obj);
      return obj;
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// The value returned (in "value") is the composed value after merging any duplicated content;
    /// if the value is "repeated" (a list), then use GetValues instead.
    /// </summary>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="value">The effective value of the field, or the default value if not found.</param>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <returns>True if data for the field was present, false otherwise.</returns>
    public static bool TryGetValue<TValue>(IExtensible instance, int tag, out TValue value)
    {
      return Extensible.TryGetValue<TValue>(instance, tag, DataFormat.Default, out value);
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// The value returned (in "value") is the composed value after merging any duplicated content;
    /// if the value is "repeated" (a list), then use GetValues instead.
    /// </summary>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="value">The effective value of the field, or the default value if not found.</param>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="format">The data-format to use when decoding the value.</param>
    /// <returns>True if data for the field was present, false otherwise.</returns>
    public static bool TryGetValue<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      out TValue value)
    {
      return Extensible.TryGetValue<TValue>(instance, tag, format, false, out value);
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// The value returned (in "value") is the composed value after merging any duplicated content;
    /// if the value is "repeated" (a list), then use GetValues instead.
    /// </summary>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="value">The effective value of the field, or the default value if not found.</param>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="format">The data-format to use when decoding the value.</param>
    /// <param name="allowDefinedTag">Allow tags that are present as part of the definition; for example, to query unknown enum values.</param>
    /// <returns>True if data for the field was present, false otherwise.</returns>
    public static bool TryGetValue<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format,
      bool allowDefinedTag,
      out TValue value)
    {
      value = default (TValue);
      bool flag = false;
      foreach (TValue extendedValue in ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, true, allowDefinedTag))
      {
        value = extendedValue;
        flag = true;
      }
      return flag;
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// Each occurrence of the field is yielded separately, making this usage suitable for "repeated"
    /// (list) fields.
    /// </summary>
    /// <remarks>The extended data is processed lazily as the enumerator is iterated.</remarks>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <returns>An enumerator that yields each occurrence of the field.</returns>
    public static IEnumerable<TValue> GetValues<TValue>(IExtensible instance, int tag)
    {
      return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, DataFormat.Default, false, false);
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// Each occurrence of the field is yielded separately, making this usage suitable for "repeated"
    /// (list) fields.
    /// </summary>
    /// <remarks>The extended data is processed lazily as the enumerator is iterated.</remarks>
    /// <typeparam name="TValue">The data-type of the field.</typeparam>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="format">The data-format to use when decoding the value.</param>
    /// <returns>An enumerator that yields each occurrence of the field.</returns>
    public static IEnumerable<TValue> GetValues<TValue>(
      IExtensible instance,
      int tag,
      DataFormat format)
    {
      return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, false, false);
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// The value returned (in "value") is the composed value after merging any duplicated content;
    /// if the value is "repeated" (a list), then use GetValues instead.
    /// </summary>
    /// <param name="type">The data-type of the field.</param>
    /// <param name="model">The model to use for configuration.</param>
    /// <param name="value">The effective value of the field, or the default value if not found.</param>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="format">The data-format to use when decoding the value.</param>
    /// <param name="allowDefinedTag">Allow tags that are present as part of the definition; for example, to query unknown enum values.</param>
    /// <returns>True if data for the field was present, false otherwise.</returns>
    public static bool TryGetValue(
      TypeModel model,
      Type type,
      IExtensible instance,
      int tag,
      DataFormat format,
      bool allowDefinedTag,
      out object value)
    {
      value = (object) null;
      bool flag = false;
      foreach (object extendedValue in ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, true, allowDefinedTag))
      {
        value = extendedValue;
        flag = true;
      }
      return flag;
    }

    /// <summary>
    /// Queries an extensible object for an additional (unexpected) data-field for the instance.
    /// Each occurrence of the field is yielded separately, making this usage suitable for "repeated"
    /// (list) fields.
    /// </summary>
    /// <remarks>The extended data is processed lazily as the enumerator is iterated.</remarks>
    /// <param name="model">The model to use for configuration.</param>
    /// <param name="type">The data-type of the field.</param>
    /// <param name="instance">The extensible object to obtain the value from.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="format">The data-format to use when decoding the value.</param>
    /// <returns>An enumerator that yields each occurrence of the field.</returns>
    public static IEnumerable GetValues(
      TypeModel model,
      Type type,
      IExtensible instance,
      int tag,
      DataFormat format)
    {
      return ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, false, false);
    }

    /// <summary>
    /// Appends the value as an additional (unexpected) data-field for the instance.
    /// Note that for non-repeated sub-objects, this equates to a merge operation;
    /// for repeated sub-objects this adds a new instance to the set; for simple
    /// values the new value supercedes the old value.
    /// </summary>
    /// <remarks>Note that appending a value does not remove the old value from
    /// the stream; avoid repeatedly appending values for the same field.</remarks>
    /// <param name="model">The model to use for configuration.</param>
    /// <param name="format">The data-format to use when encoding the value.</param>
    /// <param name="instance">The extensible object to append the value to.</param>
    /// <param name="tag">The field identifier; the tag should not be defined as a known data-field for the instance.</param>
    /// <param name="value">The value to append.</param>
    public static void AppendValue(
      TypeModel model,
      IExtensible instance,
      int tag,
      DataFormat format,
      object value)
    {
      ExtensibleUtil.AppendExtendValue(model, instance, tag, format, value);
    }
  }
}
