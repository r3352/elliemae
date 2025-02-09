// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.TypeFormatEventArgs
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Event arguments needed to perform type-formatting functions; this could be resolving a Type to a string suitable for serialization, or could
  /// be requesting a Type from a string. If no changes are made, a default implementation will be used (from the assembly-qualified names).
  /// </summary>
  public class TypeFormatEventArgs : EventArgs
  {
    private Type type;
    private string formattedName;
    private readonly bool typeFixed;

    /// <summary>
    /// The type involved in this map; if this is initially null, a Type is expected to be provided for the string in FormattedName.
    /// </summary>
    public Type Type
    {
      get => this.type;
      set
      {
        if (!(this.type != value))
          return;
        if (this.typeFixed)
          throw new InvalidOperationException("The type is fixed and cannot be changed");
        this.type = value;
      }
    }

    /// <summary>
    /// The formatted-name involved in this map; if this is initially null, a formatted-name is expected from the type in Type.
    /// </summary>
    public string FormattedName
    {
      get => this.formattedName;
      set
      {
        if (!(this.formattedName != value))
          return;
        if (!this.typeFixed)
          throw new InvalidOperationException("The formatted-name is fixed and cannot be changed");
        this.formattedName = value;
      }
    }

    internal TypeFormatEventArgs(string formattedName)
    {
      this.formattedName = !string.IsNullOrEmpty(formattedName) ? formattedName : throw new ArgumentNullException(nameof (formattedName));
    }

    internal TypeFormatEventArgs(Type type)
    {
      this.type = type ?? throw new ArgumentNullException(nameof (type));
      this.typeFixed = true;
    }
  }
}
