// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.CallbackSet
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Represents the set of serialization callbacks to be used when serializing/deserializing a type.
  /// </summary>
  public class CallbackSet
  {
    private readonly MetaType metaType;
    private MethodInfo beforeSerialize;
    private MethodInfo afterSerialize;
    private MethodInfo beforeDeserialize;
    private MethodInfo afterDeserialize;

    internal CallbackSet(MetaType metaType)
    {
      this.metaType = metaType ?? throw new ArgumentNullException(nameof (metaType));
    }

    internal MethodInfo this[TypeModel.CallbackType callbackType]
    {
      get
      {
        switch (callbackType)
        {
          case TypeModel.CallbackType.BeforeSerialize:
            return this.beforeSerialize;
          case TypeModel.CallbackType.AfterSerialize:
            return this.afterSerialize;
          case TypeModel.CallbackType.BeforeDeserialize:
            return this.beforeDeserialize;
          case TypeModel.CallbackType.AfterDeserialize:
            return this.afterDeserialize;
          default:
            throw new ArgumentException("Callback type not supported: " + callbackType.ToString(), nameof (callbackType));
        }
      }
    }

    internal static bool CheckCallbackParameters(TypeModel model, MethodInfo method)
    {
      foreach (ParameterInfo parameter in method.GetParameters())
      {
        Type parameterType = parameter.ParameterType;
        if (!(parameterType == model.MapType(typeof (SerializationContext))) && !(parameterType == model.MapType(typeof (Type))) && !(parameterType == model.MapType(typeof (StreamingContext))))
          return false;
      }
      return true;
    }

    private MethodInfo SanityCheckCallback(TypeModel model, MethodInfo callback)
    {
      this.metaType.ThrowIfFrozen();
      if (callback == (MethodInfo) null)
        return callback;
      if (callback.IsStatic)
        throw new ArgumentException("Callbacks cannot be static", nameof (callback));
      if (callback.ReturnType != model.MapType(typeof (void)) || !CallbackSet.CheckCallbackParameters(model, callback))
        throw CallbackSet.CreateInvalidCallbackSignature(callback);
      return callback;
    }

    internal static Exception CreateInvalidCallbackSignature(MethodInfo method)
    {
      return (Exception) new NotSupportedException("Invalid callback signature in " + method.DeclaringType.FullName + "." + method.Name);
    }

    /// <summary>Called before serializing an instance</summary>
    public MethodInfo BeforeSerialize
    {
      get => this.beforeSerialize;
      set => this.beforeSerialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    /// <summary>Called before deserializing an instance</summary>
    public MethodInfo BeforeDeserialize
    {
      get => this.beforeDeserialize;
      set => this.beforeDeserialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    /// <summary>Called after serializing an instance</summary>
    public MethodInfo AfterSerialize
    {
      get => this.afterSerialize;
      set => this.afterSerialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    /// <summary>Called after deserializing an instance</summary>
    public MethodInfo AfterDeserialize
    {
      get => this.afterDeserialize;
      set => this.afterDeserialize = this.SanityCheckCallback(this.metaType.Model, value);
    }

    /// <summary>True if any callback is set, else False</summary>
    public bool NonTrivial
    {
      get
      {
        return this.beforeSerialize != (MethodInfo) null || this.beforeDeserialize != (MethodInfo) null || this.afterSerialize != (MethodInfo) null || this.afterDeserialize != (MethodInfo) null;
      }
    }
  }
}
