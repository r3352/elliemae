// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.RuntimeTypeModel
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Provides protobuf serialization support for a number of types that can be defined at runtime
  /// </summary>
  public sealed class RuntimeTypeModel : TypeModel
  {
    private ushort options;
    private const ushort OPTIONS_InferTagFromNameDefault = 1;
    private const ushort OPTIONS_IsDefaultModel = 2;
    private const ushort OPTIONS_Frozen = 4;
    private const ushort OPTIONS_AutoAddMissingTypes = 8;
    private const ushort OPTIONS_AutoCompile = 16;
    private const ushort OPTIONS_UseImplicitZeroDefaults = 32;
    private const ushort OPTIONS_AllowParseableTypes = 64;
    private const ushort OPTIONS_AutoAddProtoContractTypesOnly = 128;
    private const ushort OPTIONS_IncludeDateTimeKind = 256;
    private const ushort OPTIONS_DoNotInternStrings = 512;
    private static readonly BasicList.MatchPredicate MetaTypeFinder = new BasicList.MatchPredicate(RuntimeTypeModel.MetaTypeFinderImpl);
    private static readonly BasicList.MatchPredicate BasicTypeFinder = new BasicList.MatchPredicate(RuntimeTypeModel.BasicTypeFinderImpl);
    private BasicList basicTypes = new BasicList();
    private readonly BasicList types = new BasicList();
    private const int KnownTypes_Array = 1;
    private const int KnownTypes_Dictionary = 2;
    private const int KnownTypes_Hashtable = 3;
    private const int KnownTypes_ArrayCutoff = 20;
    private int metadataTimeoutMilliseconds = 5000;
    private int contentionCounter = 1;
    private MethodInfo defaultFactory;

    private bool GetOption(ushort option) => ((int) this.options & (int) option) == (int) option;

    private void SetOption(ushort option, bool value)
    {
      if (value)
        this.options |= option;
      else
        this.options &= ~option;
    }

    /// <summary>
    /// Global default that
    /// enables/disables automatic tag generation based on the existing name / order
    /// of the defined members. See <seealso cref="P:ProtoBuf.ProtoContractAttribute.InferTagFromName" />
    /// for usage and <b>important warning</b> / explanation.
    /// You must set the global default before attempting to serialize/deserialize any
    /// impacted type.
    /// </summary>
    public bool InferTagFromNameDefault
    {
      get => this.GetOption((ushort) 1);
      set => this.SetOption((ushort) 1, value);
    }

    /// <summary>
    /// Global default that determines whether types are considered serializable
    /// if they have [DataContract] / [XmlType]. With this enabled, <b>ONLY</b>
    /// types marked as [ProtoContract] are added automatically.
    /// </summary>
    public bool AutoAddProtoContractTypesOnly
    {
      get => this.GetOption((ushort) 128);
      set => this.SetOption((ushort) 128, value);
    }

    /// <summary>
    /// Global switch that enables or disables the implicit
    /// handling of "zero defaults"; meanning: if no other default is specified,
    /// it assumes bools always default to false, integers to zero, etc.
    /// 
    /// If this is disabled, no such assumptions are made and only *explicit*
    /// default values are processed. This is enabled by default to
    /// preserve similar logic to v1.
    /// </summary>
    public bool UseImplicitZeroDefaults
    {
      get => this.GetOption((ushort) 32);
      set
      {
        if (!value && this.GetOption((ushort) 2))
          throw new InvalidOperationException("UseImplicitZeroDefaults cannot be disabled on the default model");
        this.SetOption((ushort) 32, value);
      }
    }

    /// <summary>
    /// Global switch that determines whether types with a <c>.ToString()</c> and a <c>Parse(string)</c>
    /// should be serialized as strings.
    /// </summary>
    public bool AllowParseableTypes
    {
      get => this.GetOption((ushort) 64);
      set => this.SetOption((ushort) 64, value);
    }

    /// <summary>
    /// Global switch that determines whether DateTime serialization should include the <c>Kind</c> of the date/time.
    /// </summary>
    public bool IncludeDateTimeKind
    {
      get => this.GetOption((ushort) 256);
      set => this.SetOption((ushort) 256, value);
    }

    /// <summary>
    /// Global switch that determines whether a single instance of the same string should be used during deserialization.
    /// </summary>
    /// <remarks>Note this does not use the global .NET string interner</remarks>
    public bool InternStrings
    {
      get => !this.GetOption((ushort) 512);
      set => this.SetOption((ushort) 512, !value);
    }

    /// <summary>
    /// Should the <c>Kind</c> be included on date/time values?
    /// </summary>
    protected internal override bool SerializeDateTimeKind() => this.GetOption((ushort) 256);

    /// <summary>
    /// The default model, used to support ProtoBuf.Serializer
    /// </summary>
    public static RuntimeTypeModel Default => RuntimeTypeModel.Singleton.Value;

    /// <summary>
    /// Returns a sequence of the Type instances that can be
    /// processed by this model.
    /// </summary>
    public IEnumerable GetTypes() => (IEnumerable) this.types;

    /// <summary>Suggest a .proto definition for the given type</summary>
    /// <param name="type">The type to generate a .proto definition for, or <c>null</c> to generate a .proto that represents the entire model</param>
    /// <returns>The .proto definition as a string</returns>
    /// <param name="syntax">The .proto syntax to use</param>
    public override string GetSchema(Type type, ProtoSyntax syntax)
    {
      BasicList list = new BasicList();
      MetaType metaType1 = (MetaType) null;
      bool flag = false;
      if (type == (Type) null)
      {
        foreach (MetaType type1 in this.types)
        {
          MetaType surrogateOrBaseOrSelf = type1.GetSurrogateOrBaseOrSelf(false);
          if (!list.Contains((object) surrogateOrBaseOrSelf))
          {
            list.Add((object) surrogateOrBaseOrSelf);
            this.CascadeDependents(list, surrogateOrBaseOrSelf);
          }
        }
      }
      else
      {
        Type underlyingType = Helpers.GetUnderlyingType(type);
        if (underlyingType != (Type) null)
          type = underlyingType;
        flag = ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out WireType _, false, false, false, false) != null;
        if (!flag)
        {
          int orAddAuto = this.FindOrAddAuto(type, false, false, false);
          metaType1 = orAddAuto >= 0 ? ((MetaType) this.types[orAddAuto]).GetSurrogateOrBaseOrSelf(false) : throw new ArgumentException("The type specified is not a contract-type", nameof (type));
          list.Add((object) metaType1);
          this.CascadeDependents(list, metaType1);
        }
      }
      StringBuilder builder1 = new StringBuilder();
      string str1 = (string) null;
      if (!flag)
      {
        foreach (MetaType metaType2 in metaType1 == null ? (IEnumerable) this.types : (IEnumerable) list)
        {
          if (!metaType2.IsList)
          {
            string str2 = metaType2.Type.Namespace;
            if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("System."))
            {
              if (str1 == null)
                str1 = str2;
              else if (!(str1 == str2))
              {
                str1 = (string) null;
                break;
              }
            }
          }
        }
      }
      if (syntax != ProtoSyntax.Proto2)
      {
        if (syntax != ProtoSyntax.Proto3)
          throw new ArgumentOutOfRangeException(nameof (syntax));
        builder1.AppendLine("syntax = \"proto3\";");
      }
      else
        builder1.AppendLine("syntax = \"proto2\";");
      if (!string.IsNullOrEmpty(str1))
      {
        builder1.Append("package ").Append(str1).Append(';');
        Helpers.AppendLine(builder1);
      }
      RuntimeTypeModel.CommonImports imports = RuntimeTypeModel.CommonImports.None;
      StringBuilder builder2 = new StringBuilder();
      MetaType[] array = new MetaType[list.Count];
      list.CopyTo((Array) array, 0);
      Array.Sort<MetaType>(array, (IComparer<MetaType>) MetaType.Comparer.Default);
      if (flag)
      {
        Helpers.AppendLine(builder2).Append("message ").Append(type.Name).Append(" {");
        MetaType.NewLine(builder2, 1).Append(syntax == ProtoSyntax.Proto2 ? "optional " : "").Append(this.GetSchemaTypeName(type, DataFormat.Default, false, false, ref imports)).Append(" value = 1;");
        Helpers.AppendLine(builder2).Append('}');
      }
      else
      {
        for (int index = 0; index < array.Length; ++index)
        {
          MetaType metaType3 = array[index];
          if (!metaType3.IsList || metaType3 == metaType1)
            metaType3.WriteSchema(builder2, 0, ref imports, syntax);
        }
      }
      if ((imports & RuntimeTypeModel.CommonImports.Bcl) != RuntimeTypeModel.CommonImports.None)
      {
        builder1.Append("import \"protobuf-net/bcl.proto\"; // schema for protobuf-net's handling of core .NET types");
        Helpers.AppendLine(builder1);
      }
      if ((imports & RuntimeTypeModel.CommonImports.Protogen) != RuntimeTypeModel.CommonImports.None)
      {
        builder1.Append("import \"protobuf-net/protogen.proto\"; // custom protobuf-net options");
        Helpers.AppendLine(builder1);
      }
      if ((imports & RuntimeTypeModel.CommonImports.Timestamp) != RuntimeTypeModel.CommonImports.None)
      {
        builder1.Append("import \"google/protobuf/timestamp.proto\";");
        Helpers.AppendLine(builder1);
      }
      if ((imports & RuntimeTypeModel.CommonImports.Duration) != RuntimeTypeModel.CommonImports.None)
      {
        builder1.Append("import \"google/protobuf/duration.proto\";");
        Helpers.AppendLine(builder1);
      }
      return Helpers.AppendLine(builder1.Append((object) builder2)).ToString();
    }

    private void CascadeDependents(BasicList list, MetaType metaType)
    {
      if (metaType.IsList)
      {
        Type listItemType = TypeModel.GetListItemType((TypeModel) this, metaType.Type);
        this.TryGetCoreSerializer(list, listItemType);
      }
      else
      {
        if (metaType.IsAutoTuple)
        {
          MemberInfo[] mappedMembers;
          if (MetaType.ResolveTupleConstructor(metaType.Type, out mappedMembers) != (ConstructorInfo) null)
          {
            for (int index = 0; index < mappedMembers.Length; ++index)
            {
              Type itemType = (Type) null;
              if ((object) (mappedMembers[index] as PropertyInfo) != null)
                itemType = ((PropertyInfo) mappedMembers[index]).PropertyType;
              else if ((object) (mappedMembers[index] as FieldInfo) != null)
                itemType = ((FieldInfo) mappedMembers[index]).FieldType;
              this.TryGetCoreSerializer(list, itemType);
            }
          }
        }
        else
        {
          foreach (ValueMember field in metaType.Fields)
          {
            Type valueType = field.ItemType;
            if (field.IsMap)
              field.ResolveMapTypes(out Type _, out Type _, out valueType);
            if (valueType == (Type) null)
              valueType = field.MemberType;
            this.TryGetCoreSerializer(list, valueType);
          }
        }
        foreach (Type allGenericArgument in metaType.GetAllGenericArguments())
          this.TryGetCoreSerializer(list, allGenericArgument);
        if (metaType.HasSubtypes)
        {
          foreach (SubType subtype in metaType.GetSubtypes())
          {
            MetaType surrogateOrSelf = subtype.DerivedType.GetSurrogateOrSelf();
            if (!list.Contains((object) surrogateOrSelf))
            {
              list.Add((object) surrogateOrSelf);
              this.CascadeDependents(list, surrogateOrSelf);
            }
          }
        }
        MetaType metaType1 = metaType.BaseType;
        if (metaType1 != null)
          metaType1 = metaType1.GetSurrogateOrSelf();
        if (metaType1 == null || list.Contains((object) metaType1))
          return;
        list.Add((object) metaType1);
        this.CascadeDependents(list, metaType1);
      }
    }

    private void TryGetCoreSerializer(BasicList list, Type itemType)
    {
      if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, itemType, out WireType _, false, false, false, false) != null)
        return;
      int orAddAuto = this.FindOrAddAuto(itemType, false, false, false);
      if (orAddAuto < 0)
        return;
      MetaType surrogateOrBaseOrSelf = ((MetaType) this.types[orAddAuto]).GetSurrogateOrBaseOrSelf(false);
      if (list.Contains((object) surrogateOrBaseOrSelf))
        return;
      list.Add((object) surrogateOrBaseOrSelf);
      this.CascadeDependents(list, surrogateOrBaseOrSelf);
    }

    internal RuntimeTypeModel(bool isDefault)
    {
      this.AutoAddMissingTypes = true;
      this.UseImplicitZeroDefaults = true;
      this.SetOption((ushort) 2, isDefault);
      try
      {
        this.AutoCompile = RuntimeTypeModel.EnableAutoCompile();
      }
      catch
      {
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static bool EnableAutoCompile()
    {
      try
      {
        DynamicMethod dynamicMethod = new DynamicMethod("CheckCompilerAvailable", typeof (bool), new Type[1]
        {
          typeof (int)
        });
        ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldc_I4, 42);
        ilGenerator.Emit(OpCodes.Ceq);
        ilGenerator.Emit(OpCodes.Ret);
        return ((Predicate<int>) dynamicMethod.CreateDelegate(typeof (Predicate<int>)))(42);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    /// <summary>
    /// Obtains the MetaType associated with a given Type for the current model,
    /// allowing additional configuration.
    /// </summary>
    public MetaType this[Type type]
    {
      get => (MetaType) this.types[this.FindOrAddAuto(type, true, false, false)];
    }

    internal MetaType FindWithoutAdd(Type type)
    {
      foreach (MetaType type1 in this.types)
      {
        if (type1.Type == type)
        {
          if (type1.Pending)
            this.WaitOnLock(type1);
          return type1;
        }
      }
      Type type2 = TypeModel.ResolveProxies(type);
      return !(type2 == (Type) null) ? this.FindWithoutAdd(type2) : (MetaType) null;
    }

    private static bool MetaTypeFinderImpl(object value, object ctx)
    {
      return ((MetaType) value).Type == (Type) ctx;
    }

    private static bool BasicTypeFinderImpl(object value, object ctx)
    {
      return ((RuntimeTypeModel.BasicType) value).Type == (Type) ctx;
    }

    private void WaitOnLock(MetaType type)
    {
      int opaqueToken = 0;
      try
      {
        this.TakeLock(ref opaqueToken);
      }
      finally
      {
        this.ReleaseLock(opaqueToken);
      }
    }

    internal IProtoSerializer TryGetBasicTypeSerializer(Type type)
    {
      int index1 = this.basicTypes.IndexOf(RuntimeTypeModel.BasicTypeFinder, (object) type);
      if (index1 >= 0)
        return ((RuntimeTypeModel.BasicType) this.basicTypes[index1]).Serializer;
      lock (this.basicTypes)
      {
        int index2 = this.basicTypes.IndexOf(RuntimeTypeModel.BasicTypeFinder, (object) type);
        if (index2 >= 0)
          return ((RuntimeTypeModel.BasicType) this.basicTypes[index2]).Serializer;
        IProtoSerializer coreSerializer = MetaType.GetContractFamily(this, type, (AttributeMap[]) null) == MetaType.AttributeFamily.None ? ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out WireType _, false, false, false, false) : (IProtoSerializer) null;
        if (coreSerializer != null)
          this.basicTypes.Add((object) new RuntimeTypeModel.BasicType(type, coreSerializer));
        return coreSerializer;
      }
    }

    internal int FindOrAddAuto(
      Type type,
      bool demand,
      bool addWithContractOnly,
      bool addEvenIfAutoDisabled)
    {
      int index = this.types.IndexOf(RuntimeTypeModel.MetaTypeFinder, (object) type);
      if (index >= 0)
      {
        MetaType type1 = (MetaType) this.types[index];
        if (type1.Pending)
          this.WaitOnLock(type1);
        return index;
      }
      bool flag1 = this.AutoAddMissingTypes | addEvenIfAutoDisabled;
      if (!Helpers.IsEnum(type) && this.TryGetBasicTypeSerializer(type) != null)
      {
        if (flag1 && !addWithContractOnly)
          throw MetaType.InbuiltType(type);
        return -1;
      }
      Type ctx = TypeModel.ResolveProxies(type);
      if (ctx != (Type) null && ctx != type)
      {
        index = this.types.IndexOf(RuntimeTypeModel.MetaTypeFinder, (object) ctx);
        type = ctx;
      }
      if (index < 0)
      {
        int opaqueToken = 0;
        bool flag2 = false;
        try
        {
          this.TakeLock(ref opaqueToken);
          MetaType metaType;
          if ((metaType = this.RecogniseCommonTypes(type)) == null)
          {
            MetaType.AttributeFamily contractFamily = MetaType.GetContractFamily(this, type, (AttributeMap[]) null);
            if (contractFamily == MetaType.AttributeFamily.AutoTuple)
              flag1 = addEvenIfAutoDisabled = true;
            if (!flag1 || !Helpers.IsEnum(type) & addWithContractOnly && contractFamily == MetaType.AttributeFamily.None)
            {
              if (demand)
                TypeModel.ThrowUnexpectedType(type);
              return index;
            }
            metaType = this.Create(type);
          }
          metaType.Pending = true;
          int num = this.types.IndexOf(RuntimeTypeModel.MetaTypeFinder, (object) type);
          if (num < 0)
          {
            this.ThrowIfFrozen();
            index = this.types.Add((object) metaType);
            flag2 = true;
          }
          else
            index = num;
          if (flag2)
          {
            metaType.ApplyDefaultBehaviour();
            metaType.Pending = false;
          }
        }
        finally
        {
          this.ReleaseLock(opaqueToken);
          if (flag2)
            this.ResetKeyCache();
        }
      }
      return index;
    }

    private MetaType RecogniseCommonTypes(Type type) => (MetaType) null;

    private MetaType Create(Type type)
    {
      this.ThrowIfFrozen();
      return new MetaType(this, type, this.defaultFactory);
    }

    /// <summary>
    /// Adds support for an additional type in this model, optionally
    /// applying inbuilt patterns. If the type is already known to the
    /// model, the existing type is returned **without** applying
    /// any additional behaviour.
    /// </summary>
    /// <remarks>Inbuilt patterns include:
    /// [ProtoContract]/[ProtoMember(n)]
    /// [DataContract]/[DataMember(Order=n)]
    /// [XmlType]/[XmlElement(Order=n)]
    /// [On{Des|S}erializ{ing|ed}]
    /// ShouldSerialize*/*Specified
    /// </remarks>
    /// <param name="type">The type to be supported</param>
    /// <param name="applyDefaultBehaviour">Whether to apply the inbuilt configuration patterns (via attributes etc), or
    /// just add the type with no additional configuration (the type must then be manually configured).</param>
    /// <returns>The MetaType representing this type, allowing
    /// further configuration.</returns>
    public MetaType Add(Type type, bool applyDefaultBehaviour)
    {
      MetaType metaType = !(type == (Type) null) ? this.FindWithoutAdd(type) : throw new ArgumentNullException(nameof (type));
      if (metaType != null)
        return metaType;
      int opaqueToken = 0;
      if (type.IsInterface && this.MapType(MetaType.ienumerable).IsAssignableFrom(type) && TypeModel.GetListItemType((TypeModel) this, type) == (Type) null)
        throw new ArgumentException("IEnumerable[<T>] data cannot be used as a meta-type unless an Add method can be resolved");
      try
      {
        metaType = this.RecogniseCommonTypes(type);
        if (metaType != null)
          applyDefaultBehaviour = applyDefaultBehaviour ? false : throw new ArgumentException("Default behaviour must be observed for certain types with special handling; " + type.FullName, nameof (applyDefaultBehaviour));
        if (metaType == null)
          metaType = this.Create(type);
        metaType.Pending = true;
        this.TakeLock(ref opaqueToken);
        if (this.FindWithoutAdd(type) != null)
          throw new ArgumentException("Duplicate type", nameof (type));
        this.ThrowIfFrozen();
        this.types.Add((object) metaType);
        if (applyDefaultBehaviour)
          metaType.ApplyDefaultBehaviour();
        metaType.Pending = false;
      }
      finally
      {
        this.ReleaseLock(opaqueToken);
        this.ResetKeyCache();
      }
      return metaType;
    }

    /// <summary>
    /// Should serializers be compiled on demand? It may be useful
    /// to disable this for debugging purposes.
    /// </summary>
    public bool AutoCompile
    {
      get => this.GetOption((ushort) 16);
      set => this.SetOption((ushort) 16, value);
    }

    /// <summary>
    /// Should support for unexpected types be added automatically?
    /// If false, an exception is thrown when unexpected types
    /// are encountered.
    /// </summary>
    public bool AutoAddMissingTypes
    {
      get => this.GetOption((ushort) 8);
      set
      {
        if (!value && this.GetOption((ushort) 2))
          throw new InvalidOperationException("The default model must allow missing types");
        this.ThrowIfFrozen();
        this.SetOption((ushort) 8, value);
      }
    }

    /// <summary>
    /// Verifies that the model is still open to changes; if not, an exception is thrown
    /// </summary>
    private void ThrowIfFrozen()
    {
      if (this.GetOption((ushort) 4))
        throw new InvalidOperationException("The model cannot be changed once frozen");
    }

    /// <summary>Prevents further changes to this model</summary>
    public void Freeze()
    {
      if (this.GetOption((ushort) 2))
        throw new InvalidOperationException("The default model cannot be frozen");
      this.SetOption((ushort) 4, true);
    }

    /// <summary>
    /// Provides the key that represents a given type in the current model.
    /// </summary>
    protected override int GetKeyImpl(Type type) => this.GetKey(type, false, true);

    internal int GetKey(Type type, bool demand, bool getBaseKey)
    {
      try
      {
        int orAddAuto = this.FindOrAddAuto(type, demand, true, false);
        if (orAddAuto >= 0)
        {
          MetaType type1 = (MetaType) this.types[orAddAuto];
          if (getBaseKey)
            orAddAuto = this.FindOrAddAuto(MetaType.GetRootType(type1).Type, true, true, false);
        }
        return orAddAuto;
      }
      catch (NotSupportedException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.Message.IndexOf(type.FullName) < 0)
          throw new ProtoException(ex.Message + " (" + type.FullName + ")", ex);
        throw;
      }
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream.
    /// </summary>
    /// <param name="key">Represents the type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="dest">The destination stream to write to.</param>
    protected internal override void Serialize(int key, object value, ProtoWriter dest)
    {
      ((MetaType) this.types[key]).Serializer.Write(value, dest);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="key">Represents the type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    protected internal override object Deserialize(int key, object value, ProtoReader source)
    {
      IProtoSerializer serializer = (IProtoSerializer) ((MetaType) this.types[key]).Serializer;
      if (value != null || !Helpers.IsValueType(serializer.ExpectedType) || !serializer.RequiresOldValue)
        return serializer.Read(value, source);
      value = Activator.CreateInstance(serializer.ExpectedType);
      return serializer.Read(value, source);
    }

    internal ProtoSerializer GetSerializer(IProtoSerializer serializer, bool compiled)
    {
      if (serializer == null)
        throw new ArgumentNullException(nameof (serializer));
      return compiled ? CompilerContext.BuildSerializer(serializer, (TypeModel) this) : new ProtoSerializer(serializer.Write);
    }

    /// <summary>
    /// Compiles the serializers individually; this is *not* a full
    /// standalone compile, but can significantly boost performance
    /// while allowing additional types to be added.
    /// </summary>
    /// <remarks>An in-place compile can access non-public types / members</remarks>
    public void CompileInPlace()
    {
      foreach (MetaType type in this.types)
        type.CompileInPlace();
    }

    private void BuildAllSerializers()
    {
      for (int index = 0; index < this.types.Count; ++index)
      {
        MetaType type = (MetaType) this.types[index];
        if (type.Serializer == null)
          throw new InvalidOperationException("No serializer available for " + type.Type.Name);
      }
    }

    /// <summary>
    /// Fully compiles the current model into a static-compiled model instance
    /// </summary>
    /// <remarks>A full compilation is restricted to accessing public types / members</remarks>
    /// <returns>An instance of the newly created compiled type-model</returns>
    public TypeModel Compile() => this.Compile(new RuntimeTypeModel.CompilerOptions());

    private static ILGenerator Override(TypeBuilder type, string name)
    {
      MethodInfo method = type.BaseType.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
      ParameterInfo[] parameters = method.GetParameters();
      Type[] parameterTypes = new Type[parameters.Length];
      for (int index = 0; index < parameterTypes.Length; ++index)
        parameterTypes[index] = parameters[index].ParameterType;
      MethodBuilder methodInfoBody = type.DefineMethod(method.Name, method.Attributes & ~MethodAttributes.Abstract | MethodAttributes.Final, method.CallingConvention, method.ReturnType, parameterTypes);
      ILGenerator ilGenerator = methodInfoBody.GetILGenerator();
      type.DefineMethodOverride((MethodInfo) methodInfoBody, method);
      return ilGenerator;
    }

    /// <summary>
    /// Fully compiles the current model into a static-compiled serialization dll
    /// (the serialization dll still requires protobuf-net for support services).
    /// </summary>
    /// <remarks>A full compilation is restricted to accessing public types / members</remarks>
    /// <param name="name">The name of the TypeModel class to create</param>
    /// <param name="path">The path for the new dll</param>
    /// <returns>An instance of the newly created compiled type-model</returns>
    public TypeModel Compile(string name, string path)
    {
      return this.Compile(new RuntimeTypeModel.CompilerOptions()
      {
        TypeName = name,
        OutputPath = path
      });
    }

    /// <summary>
    /// Fully compiles the current model into a static-compiled serialization dll
    /// (the serialization dll still requires protobuf-net for support services).
    /// </summary>
    /// <remarks>A full compilation is restricted to accessing public types / members</remarks>
    /// <returns>An instance of the newly created compiled type-model</returns>
    public TypeModel Compile(RuntimeTypeModel.CompilerOptions options)
    {
      string typeName = options != null ? options.TypeName : throw new ArgumentNullException(nameof (options));
      string outputPath = options.OutputPath;
      this.BuildAllSerializers();
      this.Freeze();
      bool flag = !string.IsNullOrEmpty(outputPath);
      if (string.IsNullOrEmpty(typeName))
      {
        if (flag)
          throw new ArgumentNullException("typeName");
        typeName = Guid.NewGuid().ToString();
      }
      string assemblyName;
      string name;
      if (outputPath == null)
      {
        assemblyName = typeName;
        name = assemblyName + ".dll";
      }
      else
      {
        assemblyName = new FileInfo(Path.GetFileNameWithoutExtension(outputPath)).Name;
        name = assemblyName + Path.GetExtension(outputPath);
      }
      AssemblyBuilder asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName()
      {
        Name = assemblyName
      }, flag ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
      ModuleBuilder module = flag ? asm.DefineDynamicModule(name, outputPath) : asm.DefineDynamicModule(name);
      this.WriteAssemblyAttributes(options, assemblyName, asm);
      TypeBuilder type1 = this.WriteBasicTypeModel(options, typeName, module);
      int index;
      bool hasInheritance;
      RuntimeTypeModel.SerializerPair[] methodPairs;
      CompilerContext.ILVersion ilVersion;
      this.WriteSerializers(options, assemblyName, type1, out index, out hasInheritance, out methodPairs, out ilVersion);
      ILGenerator il;
      int knownTypesCategory;
      FieldBuilder knownTypes;
      Type knownTypesLookupType;
      this.WriteGetKeyImpl(type1, hasInheritance, methodPairs, ilVersion, assemblyName, out il, out knownTypesCategory, out knownTypes, out knownTypesLookupType);
      il = RuntimeTypeModel.Override(type1, "SerializeDateTimeKind");
      il.Emit(this.IncludeDateTimeKind ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
      il.Emit(OpCodes.Ret);
      CompilerContext ctx = this.WriteSerializeDeserialize(assemblyName, type1, methodPairs, ilVersion, ref il);
      this.WriteConstructors(type1, ref index, methodPairs, ref il, knownTypesCategory, knownTypes, knownTypesLookupType, ctx);
      Type type2 = type1.CreateType();
      if (!string.IsNullOrEmpty(outputPath))
      {
        try
        {
          asm.Save(outputPath);
        }
        catch (IOException ex)
        {
          throw new IOException(outputPath + ", " + ex.Message, (Exception) ex);
        }
      }
      return (TypeModel) Activator.CreateInstance(type2);
    }

    private void WriteConstructors(
      TypeBuilder type,
      ref int index,
      RuntimeTypeModel.SerializerPair[] methodPairs,
      ref ILGenerator il,
      int knownTypesCategory,
      FieldBuilder knownTypes,
      Type knownTypesLookupType,
      CompilerContext ctx)
    {
      type.DefineDefaultConstructor(MethodAttributes.Public);
      il = type.DefineTypeInitializer().GetILGenerator();
      switch (knownTypesCategory)
      {
        case 1:
          CompilerContext.LoadValue(il, this.types.Count);
          il.Emit(OpCodes.Newarr, ctx.MapType(typeof (Type)));
          index = 0;
          foreach (RuntimeTypeModel.SerializerPair methodPair in methodPairs)
          {
            il.Emit(OpCodes.Dup);
            CompilerContext.LoadValue(il, index);
            il.Emit(OpCodes.Ldtoken, methodPair.Type.Type);
            il.EmitCall(OpCodes.Call, ctx.MapType(typeof (Type)).GetMethod("GetTypeFromHandle"), (Type[]) null);
            il.Emit(OpCodes.Stelem_Ref);
            ++index;
          }
          il.Emit(OpCodes.Stsfld, (FieldInfo) knownTypes);
          il.Emit(OpCodes.Ret);
          break;
        case 2:
          CompilerContext.LoadValue(il, this.types.Count);
          il.Emit(OpCodes.Newobj, knownTypesLookupType.GetConstructor(new Type[1]
          {
            this.MapType(typeof (int))
          }));
          il.Emit(OpCodes.Stsfld, (FieldInfo) knownTypes);
          int num1 = 0;
          foreach (RuntimeTypeModel.SerializerPair methodPair in methodPairs)
          {
            il.Emit(OpCodes.Ldsfld, (FieldInfo) knownTypes);
            il.Emit(OpCodes.Ldtoken, methodPair.Type.Type);
            il.EmitCall(OpCodes.Call, ctx.MapType(typeof (Type)).GetMethod("GetTypeFromHandle"), (Type[]) null);
            int num2 = num1++;
            int baseKey = methodPair.BaseKey;
            if (baseKey != methodPair.MetaKey)
            {
              num2 = -1;
              for (int index1 = 0; index1 < methodPairs.Length; ++index1)
              {
                if (methodPairs[index1].BaseKey == baseKey && methodPairs[index1].MetaKey == baseKey)
                {
                  num2 = index1;
                  break;
                }
              }
            }
            CompilerContext.LoadValue(il, num2);
            il.EmitCall(OpCodes.Callvirt, knownTypesLookupType.GetMethod("Add", new Type[2]
            {
              this.MapType(typeof (Type)),
              this.MapType(typeof (int))
            }), (Type[]) null);
          }
          il.Emit(OpCodes.Ret);
          break;
        case 3:
          CompilerContext.LoadValue(il, this.types.Count);
          il.Emit(OpCodes.Newobj, knownTypesLookupType.GetConstructor(new Type[1]
          {
            this.MapType(typeof (int))
          }));
          il.Emit(OpCodes.Stsfld, (FieldInfo) knownTypes);
          int num3 = 0;
          foreach (RuntimeTypeModel.SerializerPair methodPair in methodPairs)
          {
            il.Emit(OpCodes.Ldsfld, (FieldInfo) knownTypes);
            il.Emit(OpCodes.Ldtoken, methodPair.Type.Type);
            il.EmitCall(OpCodes.Call, ctx.MapType(typeof (Type)).GetMethod("GetTypeFromHandle"), (Type[]) null);
            int num4 = num3++;
            int baseKey = methodPair.BaseKey;
            if (baseKey != methodPair.MetaKey)
            {
              num4 = -1;
              for (int index2 = 0; index2 < methodPairs.Length; ++index2)
              {
                if (methodPairs[index2].BaseKey == baseKey && methodPairs[index2].MetaKey == baseKey)
                {
                  num4 = index2;
                  break;
                }
              }
            }
            CompilerContext.LoadValue(il, num4);
            il.Emit(OpCodes.Box, this.MapType(typeof (int)));
            il.EmitCall(OpCodes.Callvirt, knownTypesLookupType.GetMethod("Add", new Type[2]
            {
              this.MapType(typeof (object)),
              this.MapType(typeof (object))
            }), (Type[]) null);
          }
          il.Emit(OpCodes.Ret);
          break;
        default:
          throw new InvalidOperationException();
      }
    }

    private CompilerContext WriteSerializeDeserialize(
      string assemblyName,
      TypeBuilder type,
      RuntimeTypeModel.SerializerPair[] methodPairs,
      CompilerContext.ILVersion ilVersion,
      ref ILGenerator il)
    {
      il = RuntimeTypeModel.Override(type, "Serialize");
      CompilerContext compilerContext1 = new CompilerContext(il, false, true, methodPairs, (TypeModel) this, ilVersion, assemblyName, this.MapType(typeof (object)), "Serialize " + type.Name);
      CodeLabel[] jumpTable = new CodeLabel[this.types.Count];
      for (int index = 0; index < jumpTable.Length; ++index)
        jumpTable[index] = compilerContext1.DefineLabel();
      il.Emit(OpCodes.Ldarg_1);
      compilerContext1.Switch(jumpTable);
      compilerContext1.Return();
      for (int index = 0; index < jumpTable.Length; ++index)
      {
        RuntimeTypeModel.SerializerPair methodPair = methodPairs[index];
        compilerContext1.MarkLabel(jumpTable[index]);
        il.Emit(OpCodes.Ldarg_2);
        compilerContext1.CastFromObject(methodPair.Type.Type);
        il.Emit(OpCodes.Ldarg_3);
        il.EmitCall(OpCodes.Call, (MethodInfo) methodPair.Serialize, (Type[]) null);
        compilerContext1.Return();
      }
      il = RuntimeTypeModel.Override(type, "Deserialize");
      CompilerContext compilerContext2 = new CompilerContext(il, false, false, methodPairs, (TypeModel) this, ilVersion, assemblyName, this.MapType(typeof (object)), "Deserialize " + type.Name);
      for (int index = 0; index < jumpTable.Length; ++index)
        jumpTable[index] = compilerContext2.DefineLabel();
      il.Emit(OpCodes.Ldarg_1);
      compilerContext2.Switch(jumpTable);
      compilerContext2.LoadNullRef();
      compilerContext2.Return();
      for (int i = 0; i < jumpTable.Length; ++i)
      {
        RuntimeTypeModel.SerializerPair methodPair = methodPairs[i];
        compilerContext2.MarkLabel(jumpTable[i]);
        Type type1 = methodPair.Type.Type;
        if (Helpers.IsValueType(type1))
        {
          il.Emit(OpCodes.Ldarg_2);
          il.Emit(OpCodes.Ldarg_3);
          il.EmitCall(OpCodes.Call, (MethodInfo) RuntimeTypeModel.EmitBoxedSerializer(type, i, type1, methodPairs, (TypeModel) this, ilVersion, assemblyName), (Type[]) null);
          compilerContext2.Return();
        }
        else
        {
          il.Emit(OpCodes.Ldarg_2);
          compilerContext2.CastFromObject(type1);
          il.Emit(OpCodes.Ldarg_3);
          il.EmitCall(OpCodes.Call, (MethodInfo) methodPair.Deserialize, (Type[]) null);
          compilerContext2.Return();
        }
      }
      return compilerContext2;
    }

    private void WriteGetKeyImpl(
      TypeBuilder type,
      bool hasInheritance,
      RuntimeTypeModel.SerializerPair[] methodPairs,
      CompilerContext.ILVersion ilVersion,
      string assemblyName,
      out ILGenerator il,
      out int knownTypesCategory,
      out FieldBuilder knownTypes,
      out Type knownTypesLookupType)
    {
      il = RuntimeTypeModel.Override(type, "GetKeyImpl");
      CompilerContext compilerContext = new CompilerContext(il, false, false, methodPairs, (TypeModel) this, ilVersion, assemblyName, this.MapType(typeof (Type), true), "GetKeyImpl");
      if (this.types.Count <= 20)
      {
        knownTypesCategory = 1;
        knownTypesLookupType = this.MapType(typeof (Type[]), true);
      }
      else
      {
        knownTypesLookupType = this.MapType(typeof (Dictionary<Type, int>), false);
        if (knownTypesLookupType == (Type) null)
        {
          knownTypesLookupType = this.MapType(typeof (Hashtable), true);
          knownTypesCategory = 3;
        }
        else
          knownTypesCategory = 2;
      }
      knownTypes = type.DefineField(nameof (knownTypes), knownTypesLookupType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);
      switch (knownTypesCategory)
      {
        case 1:
          il.Emit(OpCodes.Ldsfld, (FieldInfo) knownTypes);
          il.Emit(OpCodes.Ldarg_1);
          il.EmitCall(OpCodes.Callvirt, this.MapType(typeof (IList)).GetMethod("IndexOf", new Type[1]
          {
            this.MapType(typeof (object))
          }), (Type[]) null);
          if (hasInheritance)
          {
            il.DeclareLocal(this.MapType(typeof (int)));
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Stloc_0);
            BasicList basicList = new BasicList();
            int num1 = -1;
            for (int index = 0; index < methodPairs.Length && methodPairs[index].MetaKey != methodPairs[index].BaseKey; ++index)
            {
              if (num1 == methodPairs[index].BaseKey)
              {
                basicList.Add(basicList[basicList.Count - 1]);
              }
              else
              {
                basicList.Add((object) compilerContext.DefineLabel());
                num1 = methodPairs[index].BaseKey;
              }
            }
            CodeLabel[] jumpTable = new CodeLabel[basicList.Count];
            basicList.CopyTo((Array) jumpTable, 0);
            compilerContext.Switch(jumpTable);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);
            int num2 = -1;
            for (int index = jumpTable.Length - 1; index >= 0; --index)
            {
              if (num2 != methodPairs[index].BaseKey)
              {
                num2 = methodPairs[index].BaseKey;
                int num3 = -1;
                for (int length = jumpTable.Length; length < methodPairs.Length; ++length)
                {
                  if (methodPairs[length].BaseKey == num2 && methodPairs[length].MetaKey == num2)
                  {
                    num3 = length;
                    break;
                  }
                }
                compilerContext.MarkLabel(jumpTable[index]);
                CompilerContext.LoadValue(il, num3);
                il.Emit(OpCodes.Ret);
              }
            }
            break;
          }
          il.Emit(OpCodes.Ret);
          break;
        case 2:
          LocalBuilder local = il.DeclareLocal(this.MapType(typeof (int)));
          Label label1 = il.DefineLabel();
          il.Emit(OpCodes.Ldsfld, (FieldInfo) knownTypes);
          il.Emit(OpCodes.Ldarg_1);
          il.Emit(OpCodes.Ldloca_S, local);
          il.EmitCall(OpCodes.Callvirt, knownTypesLookupType.GetMethod("TryGetValue", BindingFlags.Instance | BindingFlags.Public), (Type[]) null);
          il.Emit(OpCodes.Brfalse_S, label1);
          il.Emit(OpCodes.Ldloc_S, local);
          il.Emit(OpCodes.Ret);
          il.MarkLabel(label1);
          il.Emit(OpCodes.Ldc_I4_M1);
          il.Emit(OpCodes.Ret);
          break;
        case 3:
          Label label2 = il.DefineLabel();
          il.Emit(OpCodes.Ldsfld, (FieldInfo) knownTypes);
          il.Emit(OpCodes.Ldarg_1);
          il.EmitCall(OpCodes.Callvirt, knownTypesLookupType.GetProperty("Item").GetGetMethod(), (Type[]) null);
          il.Emit(OpCodes.Dup);
          il.Emit(OpCodes.Brfalse_S, label2);
          if (ilVersion == CompilerContext.ILVersion.Net1)
          {
            il.Emit(OpCodes.Unbox, this.MapType(typeof (int)));
            il.Emit(OpCodes.Ldobj, this.MapType(typeof (int)));
          }
          else
            il.Emit(OpCodes.Unbox_Any, this.MapType(typeof (int)));
          il.Emit(OpCodes.Ret);
          il.MarkLabel(label2);
          il.Emit(OpCodes.Pop);
          il.Emit(OpCodes.Ldc_I4_M1);
          il.Emit(OpCodes.Ret);
          break;
        default:
          throw new InvalidOperationException();
      }
    }

    private void WriteSerializers(
      RuntimeTypeModel.CompilerOptions options,
      string assemblyName,
      TypeBuilder type,
      out int index,
      out bool hasInheritance,
      out RuntimeTypeModel.SerializerPair[] methodPairs,
      out CompilerContext.ILVersion ilVersion)
    {
      index = 0;
      hasInheritance = false;
      methodPairs = new RuntimeTypeModel.SerializerPair[this.types.Count];
      foreach (MetaType type1 in this.types)
      {
        MethodBuilder serialize = type.DefineMethod("Write", MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, this.MapType(typeof (void)), new Type[2]
        {
          type1.Type,
          this.MapType(typeof (ProtoWriter))
        });
        MethodBuilder deserialize = type.DefineMethod("Read", MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, type1.Type, new Type[2]
        {
          type1.Type,
          this.MapType(typeof (ProtoReader))
        });
        RuntimeTypeModel.SerializerPair serializerPair = new RuntimeTypeModel.SerializerPair(this.GetKey(type1.Type, true, false), this.GetKey(type1.Type, true, true), type1, serialize, deserialize, serialize.GetILGenerator(), deserialize.GetILGenerator());
        methodPairs[index++] = serializerPair;
        if (serializerPair.MetaKey != serializerPair.BaseKey)
          hasInheritance = true;
      }
      if (hasInheritance)
        Array.Sort<RuntimeTypeModel.SerializerPair>(methodPairs);
      ilVersion = CompilerContext.ILVersion.Net2;
      if (options.MetaDataVersion == 65536)
        ilVersion = CompilerContext.ILVersion.Net1;
      index = 0;
      while (index < methodPairs.Length)
      {
        RuntimeTypeModel.SerializerPair serializerPair = methodPairs[index];
        CompilerContext ctx1 = new CompilerContext(serializerPair.SerializeBody, true, true, methodPairs, (TypeModel) this, ilVersion, assemblyName, serializerPair.Type.Type, "SerializeImpl " + serializerPair.Type.Type.Name);
        MemberInfo returnType = (MemberInfo) serializerPair.Deserialize.ReturnType;
        ctx1.CheckAccessibility(ref returnType);
        serializerPair.Type.Serializer.EmitWrite(ctx1, ctx1.InputValue);
        ctx1.Return();
        CompilerContext ctx2 = new CompilerContext(serializerPair.DeserializeBody, true, false, methodPairs, (TypeModel) this, ilVersion, assemblyName, serializerPair.Type.Type, "DeserializeImpl " + serializerPair.Type.Type.Name);
        serializerPair.Type.Serializer.EmitRead(ctx2, ctx2.InputValue);
        if (!serializerPair.Type.Serializer.ReturnsValue)
          ctx2.LoadValue(ctx2.InputValue);
        ctx2.Return();
        ++index;
      }
    }

    private TypeBuilder WriteBasicTypeModel(
      RuntimeTypeModel.CompilerOptions options,
      string typeName,
      ModuleBuilder module)
    {
      Type parent = this.MapType(typeof (TypeModel));
      TypeAttributes attr = parent.Attributes & ~TypeAttributes.Abstract | TypeAttributes.Sealed;
      if (options.Accessibility == RuntimeTypeModel.Accessibility.Internal)
        attr &= ~TypeAttributes.Public;
      return module.DefineType(typeName, attr, parent);
    }

    private void WriteAssemblyAttributes(
      RuntimeTypeModel.CompilerOptions options,
      string assemblyName,
      AssemblyBuilder asm)
    {
      if (!string.IsNullOrEmpty(options.TargetFrameworkName))
      {
        Type type = (Type) null;
        try
        {
          type = this.GetType("System.Runtime.Versioning.TargetFrameworkAttribute", Helpers.GetAssembly(this.MapType(typeof (string))));
        }
        catch
        {
        }
        if (type != (Type) null)
        {
          PropertyInfo[] namedProperties;
          object[] propertyValues;
          if (string.IsNullOrEmpty(options.TargetFrameworkDisplayName))
          {
            namedProperties = new PropertyInfo[0];
            propertyValues = new object[0];
          }
          else
          {
            namedProperties = new PropertyInfo[1]
            {
              type.GetProperty("FrameworkDisplayName")
            };
            propertyValues = new object[1]
            {
              (object) options.TargetFrameworkDisplayName
            };
          }
          CustomAttributeBuilder customBuilder = new CustomAttributeBuilder(type.GetConstructor(new Type[1]
          {
            this.MapType(typeof (string))
          }), new object[1]
          {
            (object) options.TargetFrameworkName
          }, namedProperties, propertyValues);
          asm.SetCustomAttribute(customBuilder);
        }
      }
      Type type1 = (Type) null;
      try
      {
        type1 = this.MapType(typeof (InternalsVisibleToAttribute));
      }
      catch
      {
      }
      if (!(type1 != (Type) null))
        return;
      BasicList basicList1 = new BasicList();
      BasicList basicList2 = new BasicList();
      foreach (MetaType type2 in this.types)
      {
        Assembly assembly = Helpers.GetAssembly(type2.Type);
        if (basicList2.IndexOfReference((object) assembly) < 0)
        {
          basicList2.Add((object) assembly);
          AttributeMap[] attributeMapArray = AttributeMap.Create((TypeModel) this, assembly);
          for (int index = 0; index < attributeMapArray.Length; ++index)
          {
            if (!(attributeMapArray[index].AttributeType != type1))
            {
              object obj;
              attributeMapArray[index].TryGet("AssemblyName", out obj);
              string str = obj as string;
              if (!(str == assemblyName) && !string.IsNullOrEmpty(str) && basicList1.IndexOfString(str) < 0)
              {
                basicList1.Add((object) str);
                CustomAttributeBuilder customBuilder = new CustomAttributeBuilder(type1.GetConstructor(new Type[1]
                {
                  this.MapType(typeof (string))
                }), new object[1]{ (object) str });
                asm.SetCustomAttribute(customBuilder);
              }
            }
          }
        }
      }
    }

    private static MethodBuilder EmitBoxedSerializer(
      TypeBuilder type,
      int i,
      Type valueType,
      RuntimeTypeModel.SerializerPair[] methodPairs,
      TypeModel model,
      CompilerContext.ILVersion ilVersion,
      string assemblyName)
    {
      MethodInfo deserialize = (MethodInfo) methodPairs[i].Deserialize;
      MethodBuilder methodBuilder = type.DefineMethod("_" + i.ToString(), MethodAttributes.Static, CallingConventions.Standard, model.MapType(typeof (object)), new Type[2]
      {
        model.MapType(typeof (object)),
        model.MapType(typeof (ProtoReader))
      });
      CompilerContext ctx = new CompilerContext(methodBuilder.GetILGenerator(), true, false, methodPairs, model, ilVersion, assemblyName, model.MapType(typeof (object)), "BoxedSerializer " + valueType.Name);
      ctx.LoadValue(ctx.InputValue);
      CodeLabel label = ctx.DefineLabel();
      ctx.BranchIfFalse(label, true);
      Type type1 = valueType;
      ctx.LoadValue(ctx.InputValue);
      ctx.CastFromObject(type1);
      ctx.LoadReaderWriter();
      ctx.EmitCall(deserialize);
      ctx.CastToObject(type1);
      ctx.Return();
      ctx.MarkLabel(label);
      using (Local local = new Local(ctx, type1))
      {
        ctx.LoadAddress(local, type1);
        ctx.EmitCtor(type1);
        ctx.LoadValue(local);
        ctx.LoadReaderWriter();
        ctx.EmitCall(deserialize);
        ctx.CastToObject(type1);
        ctx.Return();
      }
      return methodBuilder;
    }

    internal bool IsPrepared(Type type)
    {
      MetaType withoutAdd = this.FindWithoutAdd(type);
      return withoutAdd != null && withoutAdd.IsPrepared();
    }

    internal EnumSerializer.EnumPair[] GetEnumMap(Type type)
    {
      int orAddAuto = this.FindOrAddAuto(type, false, false, false);
      return orAddAuto >= 0 ? ((MetaType) this.types[orAddAuto]).GetEnumMap() : (EnumSerializer.EnumPair[]) null;
    }

    /// <summary>
    /// The amount of time to wait if there are concurrent metadata access operations
    /// </summary>
    public int MetadataTimeoutMilliseconds
    {
      get => this.metadataTimeoutMilliseconds;
      set
      {
        this.metadataTimeoutMilliseconds = value > 0 ? value : throw new ArgumentOutOfRangeException(nameof (MetadataTimeoutMilliseconds));
      }
    }

    internal void TakeLock(ref int opaqueToken)
    {
      opaqueToken = 0;
      if (Monitor.TryEnter((object) this.types, this.metadataTimeoutMilliseconds))
      {
        opaqueToken = this.GetContention();
      }
      else
      {
        this.AddContention();
        throw new TimeoutException("Timeout while inspecting metadata; this may indicate a deadlock. This can often be avoided by preparing necessary serializers during application initialization, rather than allowing multiple threads to perform the initial metadata inspection; please also see the LockContended event");
      }
    }

    private int GetContention() => Interlocked.CompareExchange(ref this.contentionCounter, 0, 0);

    private void AddContention() => Interlocked.Increment(ref this.contentionCounter);

    internal void ReleaseLock(int opaqueToken)
    {
      if (opaqueToken == 0)
        return;
      Monitor.Exit((object) this.types);
      if (opaqueToken == this.GetContention())
        return;
      LockContentedEventHandler lockContended = this.LockContended;
      if (lockContended == null)
        return;
      string stackTrace;
      try
      {
        throw new ProtoException();
      }
      catch (Exception ex)
      {
        stackTrace = ex.StackTrace;
      }
      lockContended((object) this, new LockContentedEventArgs(stackTrace));
    }

    /// <summary>
    /// If a lock-contention is detected, this event signals the *owner* of the lock responsible for the blockage, indicating
    /// what caused the problem; this is only raised if the lock-owning code successfully completes.
    /// </summary>
    public event LockContentedEventHandler LockContended;

    internal void ResolveListTypes(Type type, ref Type itemType, ref Type defaultType)
    {
      if (type == (Type) null || Helpers.GetTypeCode(type) != ProtoTypeCode.Unknown)
        return;
      if (type.IsArray)
      {
        itemType = type.GetArrayRank() == 1 ? type.GetElementType() : throw new NotSupportedException("Multi-dimension arrays are supported");
        defaultType = !(itemType == this.MapType(typeof (byte))) ? type : (itemType = (Type) null);
      }
      else if (this[type].IgnoreListHandling)
        return;
      if (itemType == (Type) null)
        itemType = TypeModel.GetListItemType((TypeModel) this, type);
      if (itemType != (Type) null)
      {
        Type itemType1 = (Type) null;
        Type defaultType1 = (Type) null;
        this.ResolveListTypes(itemType, ref itemType1, ref defaultType1);
        if (itemType1 != (Type) null)
          throw TypeModel.CreateNestedListsNotSupported(type);
      }
      if (!(itemType != (Type) null) || !(defaultType == (Type) null))
        return;
      if (type.IsClass && !type.IsAbstract && Helpers.GetConstructor(type, Helpers.EmptyTypes, true) != (ConstructorInfo) null)
        defaultType = type;
      if (defaultType == (Type) null && type.IsInterface)
      {
        Type[] genericArguments;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == this.MapType(typeof (IDictionary<,>)) && itemType == this.MapType(typeof (KeyValuePair<,>)).MakeGenericType(genericArguments = type.GetGenericArguments()))
          defaultType = this.MapType(typeof (Dictionary<,>)).MakeGenericType(genericArguments);
        else
          defaultType = this.MapType(typeof (List<>)).MakeGenericType(itemType);
      }
      if (!(defaultType != (Type) null) || Helpers.IsAssignableFrom(type, defaultType))
        return;
      defaultType = (Type) null;
    }

    internal string GetSchemaTypeName(
      Type effectiveType,
      DataFormat dataFormat,
      bool asReference,
      bool dynamicType,
      ref RuntimeTypeModel.CommonImports imports)
    {
      Type underlyingType = Helpers.GetUnderlyingType(effectiveType);
      if (underlyingType != (Type) null)
        effectiveType = underlyingType;
      if (effectiveType == this.MapType(typeof (byte[])))
        return "bytes";
      IProtoSerializer coreSerializer = ValueMember.TryGetCoreSerializer(this, dataFormat, effectiveType, out WireType _, false, false, false, false);
      if (coreSerializer == null)
      {
        if (!(asReference | dynamicType))
          return this[effectiveType].GetSurrogateOrBaseOrSelf(true).GetSchemaTypeName();
        imports |= RuntimeTypeModel.CommonImports.Bcl;
        return ".bcl.NetObjectProxy";
      }
      if (coreSerializer is ParseableSerializer)
      {
        if (asReference)
          imports |= RuntimeTypeModel.CommonImports.Bcl;
        return !asReference ? "string" : ".bcl.NetObjectProxy";
      }
      switch (Helpers.GetTypeCode(effectiveType))
      {
        case ProtoTypeCode.Boolean:
          return "bool";
        case ProtoTypeCode.Char:
        case ProtoTypeCode.Byte:
        case ProtoTypeCode.UInt16:
        case ProtoTypeCode.UInt32:
          return dataFormat == DataFormat.FixedSize ? "fixed32" : "uint32";
        case ProtoTypeCode.SByte:
        case ProtoTypeCode.Int16:
        case ProtoTypeCode.Int32:
          if (dataFormat == DataFormat.ZigZag)
            return "sint32";
          return dataFormat == DataFormat.FixedSize ? "sfixed32" : "int32";
        case ProtoTypeCode.Int64:
          if (dataFormat == DataFormat.ZigZag)
            return "sint64";
          return dataFormat == DataFormat.FixedSize ? "sfixed64" : "int64";
        case ProtoTypeCode.UInt64:
          return dataFormat == DataFormat.FixedSize ? "fixed64" : "uint64";
        case ProtoTypeCode.Single:
          return "float";
        case ProtoTypeCode.Double:
          return "double";
        case ProtoTypeCode.Decimal:
          imports |= RuntimeTypeModel.CommonImports.Bcl;
          return ".bcl.Decimal";
        case ProtoTypeCode.DateTime:
          if (dataFormat == DataFormat.FixedSize)
            return "sint64";
          if (dataFormat == DataFormat.WellKnown)
          {
            imports |= RuntimeTypeModel.CommonImports.Timestamp;
            return ".google.protobuf.Timestamp";
          }
          imports |= RuntimeTypeModel.CommonImports.Bcl;
          return ".bcl.DateTime";
        case ProtoTypeCode.String:
          if (asReference)
            imports |= RuntimeTypeModel.CommonImports.Bcl;
          return !asReference ? "string" : ".bcl.NetObjectProxy";
        case ProtoTypeCode.TimeSpan:
          if (dataFormat == DataFormat.FixedSize)
            return "sint64";
          if (dataFormat == DataFormat.WellKnown)
          {
            imports |= RuntimeTypeModel.CommonImports.Duration;
            return ".google.protobuf.Duration";
          }
          imports |= RuntimeTypeModel.CommonImports.Bcl;
          return ".bcl.TimeSpan";
        case ProtoTypeCode.Guid:
          imports |= RuntimeTypeModel.CommonImports.Bcl;
          return ".bcl.Guid";
        case ProtoTypeCode.Type:
          return "string";
        default:
          throw new NotSupportedException("No .proto map found for: " + effectiveType.FullName);
      }
    }

    /// <summary>
    /// Designate a factory-method to use to create instances of any type; note that this only affect types seen by the serializer *after* setting the factory.
    /// </summary>
    public void SetDefaultFactory(MethodInfo methodInfo)
    {
      this.VerifyFactory(methodInfo, (Type) null);
      this.defaultFactory = methodInfo;
    }

    internal void VerifyFactory(MethodInfo factory, Type type)
    {
      if (!(factory != (MethodInfo) null))
        return;
      if (type != (Type) null && Helpers.IsValueType(type))
        throw new InvalidOperationException();
      if (!factory.IsStatic)
        throw new ArgumentException("A factory-method must be static", nameof (factory));
      if (type != (Type) null && factory.ReturnType != type && factory.ReturnType != this.MapType(typeof (object)))
        throw new ArgumentException("The factory-method must return object" + (type == (Type) null ? "" : " or " + type.FullName), nameof (factory));
      if (!CallbackSet.CheckCallbackParameters((TypeModel) this, factory))
        throw new ArgumentException("Invalid factory signature in " + factory.DeclaringType.FullName + "." + factory.Name, nameof (factory));
    }

    private sealed class Singleton
    {
      internal static readonly RuntimeTypeModel Value = new RuntimeTypeModel(true);

      private Singleton()
      {
      }
    }

    [Flags]
    internal enum CommonImports
    {
      None = 0,
      Bcl = 1,
      Timestamp = 2,
      Duration = 4,
      Protogen = 8,
    }

    private sealed class BasicType
    {
      private readonly Type type;
      private readonly IProtoSerializer serializer;

      public Type Type => this.type;

      public IProtoSerializer Serializer => this.serializer;

      public BasicType(Type type, IProtoSerializer serializer)
      {
        this.type = type;
        this.serializer = serializer;
      }
    }

    internal sealed class SerializerPair : IComparable
    {
      public readonly int MetaKey;
      public readonly int BaseKey;
      public readonly MetaType Type;
      public readonly MethodBuilder Serialize;
      public readonly MethodBuilder Deserialize;
      public readonly ILGenerator SerializeBody;
      public readonly ILGenerator DeserializeBody;

      int IComparable.CompareTo(object obj)
      {
        RuntimeTypeModel.SerializerPair serializerPair = obj != null ? (RuntimeTypeModel.SerializerPair) obj : throw new ArgumentException(nameof (obj));
        if (this.BaseKey == this.MetaKey)
          return serializerPair.BaseKey == serializerPair.MetaKey ? this.MetaKey.CompareTo(serializerPair.MetaKey) : 1;
        if (serializerPair.BaseKey == serializerPair.MetaKey)
          return -1;
        int num = this.BaseKey.CompareTo(serializerPair.BaseKey);
        if (num == 0)
          num = this.MetaKey.CompareTo(serializerPair.MetaKey);
        return num;
      }

      public SerializerPair(
        int metaKey,
        int baseKey,
        MetaType type,
        MethodBuilder serialize,
        MethodBuilder deserialize,
        ILGenerator serializeBody,
        ILGenerator deserializeBody)
      {
        this.MetaKey = metaKey;
        this.BaseKey = baseKey;
        this.Serialize = serialize;
        this.Deserialize = deserialize;
        this.SerializeBody = serializeBody;
        this.DeserializeBody = deserializeBody;
        this.Type = type;
      }
    }

    /// <summary>
    /// Represents configuration options for compiling a model to
    /// a standalone assembly.
    /// </summary>
    public sealed class CompilerOptions
    {
      private string targetFrameworkName;
      private string targetFrameworkDisplayName;
      private string typeName;
      private string outputPath;
      private string imageRuntimeVersion;
      private int metaDataVersion;
      private RuntimeTypeModel.Accessibility accessibility;

      /// <summary>Import framework options from an existing type</summary>
      public void SetFrameworkOptions(MetaType from)
      {
        if (from == null)
          throw new ArgumentNullException(nameof (from));
        foreach (AttributeMap attributeMap in AttributeMap.Create(from.Model, Helpers.GetAssembly(from.Type)))
        {
          if (attributeMap.AttributeType.FullName == "System.Runtime.Versioning.TargetFrameworkAttribute")
          {
            object obj;
            if (attributeMap.TryGet("FrameworkName", out obj))
              this.TargetFrameworkName = (string) obj;
            if (!attributeMap.TryGet("FrameworkDisplayName", out obj))
              break;
            this.TargetFrameworkDisplayName = (string) obj;
            break;
          }
        }
      }

      /// <summary>
      /// The TargetFrameworkAttribute FrameworkName value to burn into the generated assembly
      /// </summary>
      public string TargetFrameworkName
      {
        get => this.targetFrameworkName;
        set => this.targetFrameworkName = value;
      }

      /// <summary>
      /// The TargetFrameworkAttribute FrameworkDisplayName value to burn into the generated assembly
      /// </summary>
      public string TargetFrameworkDisplayName
      {
        get => this.targetFrameworkDisplayName;
        set => this.targetFrameworkDisplayName = value;
      }

      /// <summary>The name of the TypeModel class to create</summary>
      public string TypeName
      {
        get => this.typeName;
        set => this.typeName = value;
      }

      /// <summary>The path for the new dll</summary>
      public string OutputPath
      {
        get => this.outputPath;
        set => this.outputPath = value;
      }

      /// <summary>The runtime version for the generated assembly</summary>
      public string ImageRuntimeVersion
      {
        get => this.imageRuntimeVersion;
        set => this.imageRuntimeVersion = value;
      }

      /// <summary>The runtime version for the generated assembly</summary>
      public int MetaDataVersion
      {
        get => this.metaDataVersion;
        set => this.metaDataVersion = value;
      }

      /// <summary>The acecssibility of the generated serializer</summary>
      public RuntimeTypeModel.Accessibility Accessibility
      {
        get => this.accessibility;
        set => this.accessibility = value;
      }
    }

    /// <summary>Type accessibility</summary>
    public enum Accessibility
    {
      /// <summary>Available to all callers</summary>
      Public,
      /// <summary>
      /// Available to all callers in the same assembly, or assemblies specified via [InternalsVisibleTo(...)]
      /// </summary>
      Internal,
    }
  }
}
