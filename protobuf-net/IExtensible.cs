// Decompiled with JetBrains decompiler
// Type: ProtoBuf.IExtensible
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Indicates that the implementing type has support for protocol-buffer
  /// <see cref="T:ProtoBuf.IExtension">extensions</see>.
  /// </summary>
  /// <remarks>Can be implemented by deriving from Extensible.</remarks>
  public interface IExtensible
  {
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
    IExtension GetExtensionObject(bool createIfMissing);
  }
}
