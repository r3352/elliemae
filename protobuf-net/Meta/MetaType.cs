// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.MetaType
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Represents a type at runtime for use with protobuf, allowing the field mappings (etc) to be defined
  /// </summary>
  public class MetaType : ISerializerProxy
  {
    private MetaType baseType;
    private BasicList subTypes;
    internal static readonly Type ienumerable = typeof (IEnumerable);
    private CallbackSet callbacks;
    private string name;
    private MethodInfo factory;
    private readonly RuntimeTypeModel model;
    private readonly Type type;
    private IProtoTypeSerializer serializer;
    private Type constructType;
    private Type surrogate;
    private readonly BasicList fields = new BasicList();
    private const ushort OPTIONS_Pending = 1;
    private const ushort OPTIONS_EnumPassThru = 2;
    private const ushort OPTIONS_Frozen = 4;
    private const ushort OPTIONS_PrivateOnApi = 8;
    private const ushort OPTIONS_SkipConstructor = 16;
    private const ushort OPTIONS_AsReferenceDefault = 32;
    private const ushort OPTIONS_AutoTuple = 64;
    private const ushort OPTIONS_IgnoreListHandling = 128;
    private const ushort OPTIONS_IsGroup = 256;
    private volatile ushort flags;

    /// <summary>Get the name of the type being represented</summary>
    public override string ToString() => this.type.ToString();

    IProtoSerializer ISerializerProxy.Serializer => (IProtoSerializer) this.Serializer;

    /// <summary>Gets the base-type for this type</summary>
    public MetaType BaseType => this.baseType;

    internal TypeModel Model => (TypeModel) this.model;

    /// <summary>
    /// When used to compile a model, should public serialization/deserialzation methods
    /// be included for this type?
    /// </summary>
    public bool IncludeSerializerMethod
    {
      get => !this.HasFlag((ushort) 8);
      set => this.SetFlag((ushort) 8, !value, true);
    }

    /// <summary>
    /// Should this type be treated as a reference by default?
    /// </summary>
    public bool AsReferenceDefault
    {
      get => this.HasFlag((ushort) 32);
      set => this.SetFlag((ushort) 32, value, true);
    }

    private bool IsValidSubType(Type subType) => this.type.IsAssignableFrom(subType);

    /// <summary>Adds a known sub-type to the inheritance model</summary>
    public MetaType AddSubType(int fieldNumber, Type derivedType)
    {
      return this.AddSubType(fieldNumber, derivedType, DataFormat.Default);
    }

    /// <summary>Adds a known sub-type to the inheritance model</summary>
    public MetaType AddSubType(int fieldNumber, Type derivedType, DataFormat dataFormat)
    {
      if (derivedType == (Type) null)
        throw new ArgumentNullException(nameof (derivedType));
      if (fieldNumber < 1)
        throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      if (!this.type.IsClass && !this.type.IsInterface || this.type.IsSealed)
        throw new InvalidOperationException("Sub-types can only be added to non-sealed classes");
      MetaType derivedType1 = this.IsValidSubType(derivedType) ? this.model[derivedType] : throw new ArgumentException(derivedType.Name + " is not a valid sub-type of " + this.type.Name, nameof (derivedType));
      this.ThrowIfFrozen();
      derivedType1.ThrowIfFrozen();
      SubType subType = new SubType(fieldNumber, derivedType1, dataFormat);
      this.ThrowIfFrozen();
      derivedType1.SetBaseType(this);
      if (this.subTypes == null)
        this.subTypes = new BasicList();
      this.subTypes.Add((object) subType);
      this.model.ResetKeyCache();
      return this;
    }

    private void SetBaseType(MetaType baseType)
    {
      if (baseType == null)
        throw new ArgumentNullException(nameof (baseType));
      if (this.baseType == baseType)
        return;
      if (this.baseType != null)
        throw new InvalidOperationException("Type '" + this.baseType.Type.FullName + "' can only participate in one inheritance hierarchy");
      for (MetaType metaType = baseType; metaType != null; metaType = metaType.baseType)
      {
        if (metaType == this)
          throw new InvalidOperationException("Cyclic inheritance of '" + this.baseType.Type.FullName + "' is not allowed");
      }
      this.baseType = baseType;
    }

    /// <summary>
    /// Indicates whether the current type has defined callbacks
    /// </summary>
    public bool HasCallbacks => this.callbacks != null && this.callbacks.NonTrivial;

    /// <summary>
    /// Indicates whether the current type has defined subtypes
    /// </summary>
    public bool HasSubtypes => this.subTypes != null && this.subTypes.Count != 0;

    /// <summary>Returns the set of callbacks defined for this type</summary>
    public CallbackSet Callbacks
    {
      get
      {
        if (this.callbacks == null)
          this.callbacks = new CallbackSet(this);
        return this.callbacks;
      }
    }

    private bool IsValueType => this.type.IsValueType;

    /// <summary>
    /// Assigns the callbacks to use during serialiation/deserialization.
    /// </summary>
    /// <param name="beforeSerialize">The method (or null) called before serialization begins.</param>
    /// <param name="afterSerialize">The method (or null) called when serialization is complete.</param>
    /// <param name="beforeDeserialize">The method (or null) called before deserialization begins (or when a new instance is created during deserialization).</param>
    /// <param name="afterDeserialize">The method (or null) called when deserialization is complete.</param>
    /// <returns>The set of callbacks.</returns>
    public MetaType SetCallbacks(
      MethodInfo beforeSerialize,
      MethodInfo afterSerialize,
      MethodInfo beforeDeserialize,
      MethodInfo afterDeserialize)
    {
      CallbackSet callbacks = this.Callbacks;
      callbacks.BeforeSerialize = beforeSerialize;
      callbacks.AfterSerialize = afterSerialize;
      callbacks.BeforeDeserialize = beforeDeserialize;
      callbacks.AfterDeserialize = afterDeserialize;
      return this;
    }

    /// <summary>
    /// Assigns the callbacks to use during serialiation/deserialization.
    /// </summary>
    /// <param name="beforeSerialize">The name of the method (or null) called before serialization begins.</param>
    /// <param name="afterSerialize">The name of the method (or null) called when serialization is complete.</param>
    /// <param name="beforeDeserialize">The name of the method (or null) called before deserialization begins (or when a new instance is created during deserialization).</param>
    /// <param name="afterDeserialize">The name of the method (or null) called when deserialization is complete.</param>
    /// <returns>The set of callbacks.</returns>
    public MetaType SetCallbacks(
      string beforeSerialize,
      string afterSerialize,
      string beforeDeserialize,
      string afterDeserialize)
    {
      if (this.IsValueType)
        throw new InvalidOperationException();
      CallbackSet callbacks = this.Callbacks;
      callbacks.BeforeSerialize = this.ResolveMethod(beforeSerialize, true);
      callbacks.AfterSerialize = this.ResolveMethod(afterSerialize, true);
      callbacks.BeforeDeserialize = this.ResolveMethod(beforeDeserialize, true);
      callbacks.AfterDeserialize = this.ResolveMethod(afterDeserialize, true);
      return this;
    }

    internal string GetSchemaTypeName()
    {
      if (this.surrogate != (Type) null)
        return this.model[this.surrogate].GetSchemaTypeName();
      if (!string.IsNullOrEmpty(this.name))
        return this.name;
      string name = this.type.Name;
      if (!this.type.IsGenericType)
        return name;
      StringBuilder stringBuilder = new StringBuilder(name);
      int num = name.IndexOf('`');
      if (num >= 0)
        stringBuilder.Length = num;
      foreach (Type genericArgument in this.type.GetGenericArguments())
      {
        stringBuilder.Append('_');
        Type type = genericArgument;
        MetaType metaType;
        if (this.model.GetKey(ref type) >= 0 && (metaType = this.model[type]) != null && metaType.surrogate == (Type) null)
          stringBuilder.Append(metaType.GetSchemaTypeName());
        else
          stringBuilder.Append(type.Name);
      }
      return stringBuilder.ToString();
    }

    /// <summary>Gets or sets the name of this contract.</summary>
    public string Name
    {
      get => this.name;
      set
      {
        this.ThrowIfFrozen();
        this.name = value;
      }
    }

    /// <summary>
    /// Designate a factory-method to use to create instances of this type
    /// </summary>
    public MetaType SetFactory(MethodInfo factory)
    {
      this.model.VerifyFactory(factory, this.type);
      this.ThrowIfFrozen();
      this.factory = factory;
      return this;
    }

    /// <summary>
    /// Designate a factory-method to use to create instances of this type
    /// </summary>
    public MetaType SetFactory(string factory)
    {
      return this.SetFactory(this.ResolveMethod(factory, false));
    }

    private MethodInfo ResolveMethod(string name, bool instance)
    {
      if (string.IsNullOrEmpty(name))
        return (MethodInfo) null;
      return !instance ? Helpers.GetStaticMethod(this.type, name) : Helpers.GetInstanceMethod(this.type, name);
    }

    internal static Exception InbuiltType(Type type)
    {
      return (Exception) new ArgumentException("Data of this type has inbuilt behaviour, and cannot be added to a model in this way: " + type.FullName);
    }

    internal MetaType(RuntimeTypeModel model, Type type, MethodInfo factory)
    {
      this.factory = factory;
      if (model == null)
        throw new ArgumentNullException(nameof (model));
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (type.IsArray)
        throw MetaType.InbuiltType(type);
      this.type = model.TryGetBasicTypeSerializer(type) == null ? type : throw MetaType.InbuiltType(type);
      this.model = model;
      if (!Helpers.IsEnum(type))
        return;
      this.EnumPassthru = type.IsDefined(model.MapType(typeof (FlagsAttribute)), false);
    }

    /// <summary>
    /// Throws an exception if the type has been made immutable
    /// </summary>
    protected internal void ThrowIfFrozen()
    {
      if (((int) this.flags & 4) != 0)
        throw new InvalidOperationException("The type cannot be changed once a serializer has been generated for " + this.type.FullName);
    }

    /// <summary>The runtime type that the meta-type represents</summary>
    public Type Type => this.type;

    internal IProtoTypeSerializer Serializer
    {
      get
      {
        if (this.serializer == null)
        {
          int opaqueToken = 0;
          try
          {
            this.model.TakeLock(ref opaqueToken);
            if (this.serializer == null)
            {
              this.SetFlag((ushort) 4, true, false);
              this.serializer = this.BuildSerializer();
              if (this.model.AutoCompile)
                this.CompileInPlace();
            }
          }
          finally
          {
            this.model.ReleaseLock(opaqueToken);
          }
        }
        return this.serializer;
      }
    }

    internal bool IsList
    {
      get
      {
        return (this.IgnoreListHandling ? (Type) null : TypeModel.GetListItemType((TypeModel) this.model, this.type)) != (Type) null;
      }
    }

    private IProtoTypeSerializer BuildSerializer()
    {
      if (Helpers.IsEnum(this.type))
        return (IProtoTypeSerializer) new TagDecorator(1, WireType.Variant, false, (IProtoSerializer) new EnumSerializer(this.type, this.GetEnumMap()));
      Type listItemType = this.IgnoreListHandling ? (Type) null : TypeModel.GetListItemType((TypeModel) this.model, this.type);
      if (listItemType != (Type) null)
      {
        if (this.surrogate != (Type) null)
          throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot use a surrogate");
        if (this.subTypes != null && this.subTypes.Count != 0)
          throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be subclassed");
        Type defaultType = (Type) null;
        MetaType.ResolveListTypes((TypeModel) this.model, this.type, ref listItemType, ref defaultType);
        return (IProtoTypeSerializer) new TypeSerializer((TypeModel) this.model, this.type, new int[1]
        {
          1
        }, new IProtoSerializer[1]
        {
          new ValueMember(this.model, 1, this.type, listItemType, defaultType, DataFormat.Default).Serializer
        }, (MethodInfo[]) null, true, true, (CallbackSet) null, this.constructType, this.factory);
      }
      if (this.surrogate != (Type) null)
      {
        MetaType metaType = this.model[this.surrogate];
        MetaType baseType;
        while ((baseType = metaType.baseType) != null)
          metaType = baseType;
        return (IProtoTypeSerializer) new SurrogateSerializer((TypeModel) this.model, this.type, this.surrogate, metaType.Serializer);
      }
      if (this.IsAutoTuple)
      {
        MemberInfo[] mappedMembers;
        ConstructorInfo ctor = MetaType.ResolveTupleConstructor(this.type, out mappedMembers);
        if (ctor == (ConstructorInfo) null)
          throw new InvalidOperationException();
        return (IProtoTypeSerializer) new TupleSerializer(this.model, ctor, mappedMembers);
      }
      this.fields.Trim();
      int count1 = this.fields.Count;
      int count2 = this.subTypes == null ? 0 : this.subTypes.Count;
      int[] fieldNumbers = new int[count1 + count2];
      IProtoSerializer[] serializers = new IProtoSerializer[count1 + count2];
      int index = 0;
      if (count2 != 0)
      {
        foreach (SubType subType in this.subTypes)
        {
          if (!subType.DerivedType.IgnoreListHandling && this.model.MapType(MetaType.ienumerable).IsAssignableFrom(subType.DerivedType.Type))
            throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be used as a subclass");
          fieldNumbers[index] = subType.FieldNumber;
          serializers[index++] = subType.Serializer;
        }
      }
      if (count1 != 0)
      {
        foreach (ValueMember field in this.fields)
        {
          fieldNumbers[index] = field.FieldNumber;
          serializers[index++] = field.Serializer;
        }
      }
      BasicList basicList = (BasicList) null;
      for (MetaType baseType = this.BaseType; baseType != null; baseType = baseType.BaseType)
      {
        MethodInfo beforeDeserialize = baseType.HasCallbacks ? baseType.Callbacks.BeforeDeserialize : (MethodInfo) null;
        if (beforeDeserialize != (MethodInfo) null)
        {
          if (basicList == null)
            basicList = new BasicList();
          basicList.Add((object) beforeDeserialize);
        }
      }
      MethodInfo[] baseCtorCallbacks = (MethodInfo[]) null;
      if (basicList != null)
      {
        baseCtorCallbacks = new MethodInfo[basicList.Count];
        basicList.CopyTo((Array) baseCtorCallbacks, 0);
        Array.Reverse((Array) baseCtorCallbacks);
      }
      return (IProtoTypeSerializer) new TypeSerializer((TypeModel) this.model, this.type, fieldNumbers, serializers, baseCtorCallbacks, this.baseType == null, this.UseConstructor, this.callbacks, this.constructType, this.factory);
    }

    private static Type GetBaseType(MetaType type) => type.type.BaseType;

    internal static bool GetAsReferenceDefault(RuntimeTypeModel model, Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (Helpers.IsEnum(type))
        return false;
      AttributeMap[] attributeMapArray = AttributeMap.Create((TypeModel) model, type, false);
      for (int index = 0; index < attributeMapArray.Length; ++index)
      {
        object referenceDefault;
        if (attributeMapArray[index].AttributeType.FullName == "ProtoBuf.ProtoContractAttribute" && attributeMapArray[index].TryGet("AsReferenceDefault", out referenceDefault))
          return (bool) referenceDefault;
      }
      return false;
    }

    internal void ApplyDefaultBehaviour()
    {
      Type baseType = MetaType.GetBaseType(this);
      if (baseType != (Type) null && this.model.FindWithoutAdd(baseType) == null && MetaType.GetContractFamily(this.model, baseType, (AttributeMap[]) null) != MetaType.AttributeFamily.None)
        this.model.FindOrAddAuto(baseType, true, false, false);
      AttributeMap[] attributes1 = AttributeMap.Create((TypeModel) this.model, this.type, false);
      MetaType.AttributeFamily contractFamily = MetaType.GetContractFamily(this.model, this.type, attributes1);
      if (contractFamily == MetaType.AttributeFamily.AutoTuple)
        this.SetFlag((ushort) 64, true, true);
      bool isEnum = !this.EnumPassthru && Helpers.IsEnum(this.type);
      if (contractFamily == MetaType.AttributeFamily.None && !isEnum)
        return;
      bool flag = isEnum;
      BasicList basicList = (BasicList) null;
      BasicList partialMembers = (BasicList) null;
      int dataMemberOffset = 0;
      int num1 = 1;
      bool inferTagByName = this.model.InferTagFromNameDefault;
      ImplicitFields implicitMode = ImplicitFields.None;
      string str1 = (string) null;
      for (int index = 0; index < attributes1.Length; ++index)
      {
        AttributeMap attributeMap = attributes1[index];
        string fullName = attributeMap.AttributeType.FullName;
        object obj;
        if (!isEnum && fullName == "ProtoBuf.ProtoIncludeAttribute")
        {
          int fieldNumber = 0;
          if (attributeMap.TryGet("tag", out obj))
            fieldNumber = (int) obj;
          DataFormat dataFormat = DataFormat.Default;
          if (attributeMap.TryGet("DataFormat", out obj))
            dataFormat = (DataFormat) obj;
          Type type = (Type) null;
          try
          {
            if (attributeMap.TryGet("knownTypeName", out obj))
              type = this.model.GetType((string) obj, this.type.Assembly);
            else if (attributeMap.TryGet("knownType", out obj))
              type = (Type) obj;
          }
          catch (Exception ex)
          {
            throw new InvalidOperationException("Unable to resolve sub-type of: " + this.type.FullName, ex);
          }
          if (type == (Type) null)
            throw new InvalidOperationException("Unable to resolve sub-type of: " + this.type.FullName);
          if (this.IsValidSubType(type))
            this.AddSubType(fieldNumber, type, dataFormat);
        }
        if (fullName == "ProtoBuf.ProtoPartialIgnoreAttribute" && attributeMap.TryGet("MemberName", out obj) && obj != null)
        {
          if (basicList == null)
            basicList = new BasicList();
          basicList.Add((object) (string) obj);
        }
        if (!isEnum && fullName == "ProtoBuf.ProtoPartialMemberAttribute")
        {
          if (partialMembers == null)
            partialMembers = new BasicList();
          partialMembers.Add((object) attributeMap);
        }
        if (fullName == "ProtoBuf.ProtoContractAttribute")
        {
          if (attributeMap.TryGet("Name", out obj))
            str1 = (string) obj;
          if (Helpers.IsEnum(this.type))
          {
            if (attributeMap.TryGet("EnumPassthruHasValue", false, out obj) && (bool) obj && attributeMap.TryGet("EnumPassthru", out obj))
            {
              this.EnumPassthru = (bool) obj;
              flag = false;
              if (this.EnumPassthru)
                isEnum = false;
            }
          }
          else
          {
            if (attributeMap.TryGet("DataMemberOffset", out obj))
              dataMemberOffset = (int) obj;
            if (attributeMap.TryGet("InferTagFromNameHasValue", false, out obj) && (bool) obj && attributeMap.TryGet("InferTagFromName", out obj))
              inferTagByName = (bool) obj;
            if (attributeMap.TryGet("ImplicitFields", out obj) && obj != null)
              implicitMode = (ImplicitFields) obj;
            if (attributeMap.TryGet("SkipConstructor", out obj))
              this.UseConstructor = !(bool) obj;
            if (attributeMap.TryGet("IgnoreListHandling", out obj))
              this.IgnoreListHandling = (bool) obj;
            if (attributeMap.TryGet("AsReferenceDefault", out obj))
              this.AsReferenceDefault = (bool) obj;
            if (attributeMap.TryGet("ImplicitFirstTag", out obj) && (int) obj > 0)
              num1 = (int) obj;
            if (attributeMap.TryGet("IsGroup", out obj))
              this.IsGroup = (bool) obj;
            if (attributeMap.TryGet("Surrogate", out obj))
              this.SetSurrogate((Type) obj);
          }
        }
        if (fullName == "System.Runtime.Serialization.DataContractAttribute" && str1 == null && attributeMap.TryGet("Name", out obj))
          str1 = (string) obj;
        if (fullName == "System.Xml.Serialization.XmlTypeAttribute" && str1 == null && attributeMap.TryGet("TypeName", out obj))
          str1 = (string) obj;
      }
      if (!string.IsNullOrEmpty(str1))
        this.Name = str1;
      if (implicitMode != ImplicitFields.None)
        contractFamily &= MetaType.AttributeFamily.ProtoBuf;
      MethodInfo[] callbacks = (MethodInfo[]) null;
      BasicList members1 = new BasicList();
      MemberInfo[] members2 = this.type.GetMembers(isEnum ? BindingFlags.Static | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      bool hasConflictingEnumValue = false;
      foreach (MemberInfo member in members2)
      {
        if (!(member.DeclaringType != this.type) && !member.IsDefined(this.model.MapType(typeof (ProtoIgnoreAttribute)), true) && (basicList == null || !basicList.Contains((object) member.Name)))
        {
          bool forced = false;
          Type effectiveType;
          switch (member)
          {
            case PropertyInfo property:
              if (!isEnum)
              {
                MemberInfo backingMember = (MemberInfo) null;
                if (!property.CanWrite)
                {
                  string str2 = "<" + property.Name + ">k__BackingField";
                  foreach (MemberInfo memberInfo in members2)
                  {
                    if (memberInfo as FieldInfo != (FieldInfo) null && memberInfo.Name == str2)
                    {
                      backingMember = memberInfo;
                      break;
                    }
                  }
                }
                effectiveType = property.PropertyType;
                bool isPublic = Helpers.GetGetMethod(property, false, false) != (MethodInfo) null;
                bool isField = false;
                MetaType.ApplyDefaultBehaviour_AddMembers((TypeModel) this.model, contractFamily, isEnum, partialMembers, dataMemberOffset, inferTagByName, implicitMode, members1, member, ref forced, isPublic, isField, ref effectiveType, ref hasConflictingEnumValue, backingMember);
                continue;
              }
              continue;
            case FieldInfo fieldInfo:
              effectiveType = fieldInfo.FieldType;
              bool isPublic1 = fieldInfo.IsPublic;
              bool isField1 = true;
              if (!isEnum || fieldInfo.IsStatic)
              {
                MetaType.ApplyDefaultBehaviour_AddMembers((TypeModel) this.model, contractFamily, isEnum, partialMembers, dataMemberOffset, inferTagByName, implicitMode, members1, member, ref forced, isPublic1, isField1, ref effectiveType, ref hasConflictingEnumValue);
                continue;
              }
              continue;
            case MethodInfo methodInfo:
              if (!isEnum)
              {
                AttributeMap[] attributes2 = AttributeMap.Create((TypeModel) this.model, (MemberInfo) methodInfo, false);
                if (attributes2 != null && attributes2.Length != 0)
                {
                  MetaType.CheckForCallback(methodInfo, attributes2, "ProtoBuf.ProtoBeforeSerializationAttribute", ref callbacks, 0);
                  MetaType.CheckForCallback(methodInfo, attributes2, "ProtoBuf.ProtoAfterSerializationAttribute", ref callbacks, 1);
                  MetaType.CheckForCallback(methodInfo, attributes2, "ProtoBuf.ProtoBeforeDeserializationAttribute", ref callbacks, 2);
                  MetaType.CheckForCallback(methodInfo, attributes2, "ProtoBuf.ProtoAfterDeserializationAttribute", ref callbacks, 3);
                  MetaType.CheckForCallback(methodInfo, attributes2, "System.Runtime.Serialization.OnSerializingAttribute", ref callbacks, 4);
                  MetaType.CheckForCallback(methodInfo, attributes2, "System.Runtime.Serialization.OnSerializedAttribute", ref callbacks, 5);
                  MetaType.CheckForCallback(methodInfo, attributes2, "System.Runtime.Serialization.OnDeserializingAttribute", ref callbacks, 6);
                  MetaType.CheckForCallback(methodInfo, attributes2, "System.Runtime.Serialization.OnDeserializedAttribute", ref callbacks, 7);
                  continue;
                }
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      if (isEnum & flag && !hasConflictingEnumValue)
        this.EnumPassthru = true;
      ProtoMemberAttribute[] array = new ProtoMemberAttribute[members1.Count];
      members1.CopyTo((Array) array, 0);
      if (inferTagByName || implicitMode != ImplicitFields.None)
      {
        Array.Sort<ProtoMemberAttribute>(array);
        int num2 = num1;
        foreach (ProtoMemberAttribute protoMemberAttribute in array)
        {
          if (!protoMemberAttribute.TagIsPinned)
            protoMemberAttribute.Rebase(num2++);
        }
      }
      foreach (ProtoMemberAttribute normalizedAttribute in array)
      {
        ValueMember member = this.ApplyDefaultBehaviour(isEnum, normalizedAttribute);
        if (member != null)
          this.Add(member);
      }
      if (callbacks == null)
        return;
      this.SetCallbacks(MetaType.Coalesce(callbacks, 0, 4), MetaType.Coalesce(callbacks, 1, 5), MetaType.Coalesce(callbacks, 2, 6), MetaType.Coalesce(callbacks, 3, 7));
    }

    private static void ApplyDefaultBehaviour_AddMembers(
      TypeModel model,
      MetaType.AttributeFamily family,
      bool isEnum,
      BasicList partialMembers,
      int dataMemberOffset,
      bool inferTagByName,
      ImplicitFields implicitMode,
      BasicList members,
      MemberInfo member,
      ref bool forced,
      bool isPublic,
      bool isField,
      ref Type effectiveType,
      ref bool hasConflictingEnumValue,
      MemberInfo backingMember = null)
    {
      switch (implicitMode)
      {
        case ImplicitFields.AllPublic:
          if (isPublic)
          {
            forced = true;
            break;
          }
          break;
        case ImplicitFields.AllFields:
          if (isField)
          {
            forced = true;
            break;
          }
          break;
      }
      if (effectiveType.IsSubclassOf(model.MapType(typeof (Delegate))))
        effectiveType = (Type) null;
      if (!(effectiveType != (Type) null))
        return;
      ProtoMemberAttribute protoMemberAttribute = MetaType.NormalizeProtoMember(model, member, family, forced, isEnum, partialMembers, dataMemberOffset, inferTagByName, ref hasConflictingEnumValue, backingMember);
      if (protoMemberAttribute == null)
        return;
      members.Add((object) protoMemberAttribute);
    }

    private static MethodInfo Coalesce(MethodInfo[] arr, int x, int y)
    {
      MethodInfo methodInfo = arr[x];
      if (methodInfo == (MethodInfo) null)
        methodInfo = arr[y];
      return methodInfo;
    }

    internal static MetaType.AttributeFamily GetContractFamily(
      RuntimeTypeModel model,
      Type type,
      AttributeMap[] attributes)
    {
      MetaType.AttributeFamily contractFamily = MetaType.AttributeFamily.None;
      if (attributes == null)
        attributes = AttributeMap.Create((TypeModel) model, type, false);
      for (int index = 0; index < attributes.Length; ++index)
      {
        switch (attributes[index].AttributeType.FullName)
        {
          case "ProtoBuf.ProtoContractAttribute":
            bool flag = false;
            MetaType.GetFieldBoolean(ref flag, attributes[index], "UseProtoMembersOnly");
            if (flag)
              return MetaType.AttributeFamily.ProtoBuf;
            contractFamily |= MetaType.AttributeFamily.ProtoBuf;
            break;
          case "System.Xml.Serialization.XmlTypeAttribute":
            if (!model.AutoAddProtoContractTypesOnly)
            {
              contractFamily |= MetaType.AttributeFamily.XmlSerializer;
              break;
            }
            break;
          case "System.Runtime.Serialization.DataContractAttribute":
            if (!model.AutoAddProtoContractTypesOnly)
            {
              contractFamily |= MetaType.AttributeFamily.DataContractSerialier;
              break;
            }
            break;
        }
      }
      if (contractFamily == MetaType.AttributeFamily.None && MetaType.ResolveTupleConstructor(type, out MemberInfo[] _) != (ConstructorInfo) null)
        contractFamily |= MetaType.AttributeFamily.AutoTuple;
      return contractFamily;
    }

    internal static ConstructorInfo ResolveTupleConstructor(
      Type type,
      out MemberInfo[] mappedMembers)
    {
      mappedMembers = (MemberInfo[]) null;
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (type.IsAbstract)
        return (ConstructorInfo) null;
      ConstructorInfo[] constructors = Helpers.GetConstructors(type, false);
      if (constructors.Length == 0 || constructors.Length == 1 && constructors[0].GetParameters().Length == 0)
        return (ConstructorInfo) null;
      MemberInfo[] fieldsAndProperties = Helpers.GetInstanceFieldsAndProperties(type, true);
      BasicList basicList = new BasicList();
      bool flag1 = type.Name.IndexOf("Tuple", StringComparison.OrdinalIgnoreCase) < 0;
      for (int index = 0; index < fieldsAndProperties.Length; ++index)
      {
        if (fieldsAndProperties[index] is PropertyInfo property)
        {
          if (!property.CanRead)
            return (ConstructorInfo) null;
          if (flag1 && property.CanWrite && Helpers.GetSetMethod(property, false, false) != (MethodInfo) null)
            return (ConstructorInfo) null;
          basicList.Add((object) property);
        }
        else if (fieldsAndProperties[index] is FieldInfo fieldInfo)
        {
          if (flag1 && !fieldInfo.IsInitOnly)
            return (ConstructorInfo) null;
          basicList.Add((object) fieldInfo);
        }
      }
      if (basicList.Count == 0)
        return (ConstructorInfo) null;
      MemberInfo[] memberInfoArray = new MemberInfo[basicList.Count];
      basicList.CopyTo((Array) memberInfoArray, 0);
      int[] numArray = new int[memberInfoArray.Length];
      int num = 0;
      ConstructorInfo constructorInfo = (ConstructorInfo) null;
      mappedMembers = new MemberInfo[numArray.Length];
      for (int index1 = 0; index1 < constructors.Length; ++index1)
      {
        ParameterInfo[] parameters = constructors[index1].GetParameters();
        if (parameters.Length == memberInfoArray.Length)
        {
          for (int index2 = 0; index2 < numArray.Length; ++index2)
            numArray[index2] = -1;
          for (int index3 = 0; index3 < parameters.Length; ++index3)
          {
            for (int index4 = 0; index4 < memberInfoArray.Length; ++index4)
            {
              if (string.Compare(parameters[index3].Name, memberInfoArray[index4].Name, StringComparison.OrdinalIgnoreCase) == 0 && !(Helpers.GetMemberType(memberInfoArray[index4]) != parameters[index3].ParameterType))
                numArray[index3] = index4;
            }
          }
          bool flag2 = false;
          for (int index5 = 0; index5 < numArray.Length; ++index5)
          {
            if (numArray[index5] < 0)
            {
              flag2 = true;
              break;
            }
            mappedMembers[index5] = memberInfoArray[numArray[index5]];
          }
          if (!flag2)
          {
            ++num;
            constructorInfo = constructors[index1];
          }
        }
      }
      return num != 1 ? (ConstructorInfo) null : constructorInfo;
    }

    private static void CheckForCallback(
      MethodInfo method,
      AttributeMap[] attributes,
      string callbackTypeName,
      ref MethodInfo[] callbacks,
      int index)
    {
      for (int index1 = 0; index1 < attributes.Length; ++index1)
      {
        if (attributes[index1].AttributeType.FullName == callbackTypeName)
        {
          if (callbacks == null)
            callbacks = new MethodInfo[8];
          else if (callbacks[index] != (MethodInfo) null)
          {
            Type reflectedType = method.ReflectedType;
            throw new ProtoException("Duplicate " + callbackTypeName + " callbacks on " + reflectedType.FullName);
          }
          callbacks[index] = method;
        }
      }
    }

    private static bool HasFamily(MetaType.AttributeFamily value, MetaType.AttributeFamily required)
    {
      return (value & required) == required;
    }

    private static ProtoMemberAttribute NormalizeProtoMember(
      TypeModel model,
      MemberInfo member,
      MetaType.AttributeFamily family,
      bool forced,
      bool isEnum,
      BasicList partialMembers,
      int dataMemberOffset,
      bool inferByTagName,
      ref bool hasConflictingEnumValue,
      MemberInfo backingMember = null)
    {
      if (member == (MemberInfo) null || family == MetaType.AttributeFamily.None && !isEnum)
        return (ProtoMemberAttribute) null;
      int tag = int.MinValue;
      int num1 = inferByTagName ? -1 : 1;
      string name = (string) null;
      bool flag1 = false;
      bool ignore = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      DataFormat dataFormat = DataFormat.Default;
      if (isEnum)
        forced = true;
      AttributeMap[] attribs = AttributeMap.Create(model, member, true);
      if (isEnum)
      {
        if (MetaType.GetAttribute(attribs, "ProtoBuf.ProtoIgnoreAttribute") != null)
        {
          ignore = true;
        }
        else
        {
          AttributeMap attribute = MetaType.GetAttribute(attribs, "ProtoBuf.ProtoEnumAttribute");
          tag = Convert.ToInt32(((FieldInfo) member).GetRawConstantValue());
          if (attribute != null)
          {
            MetaType.GetFieldName(ref name, attribute, "Name");
            object obj;
            if ((bool) Helpers.GetInstanceMethod(attribute.AttributeType, "HasValue").Invoke(attribute.Target, (object[]) null) && attribute.TryGet("Value", out obj))
            {
              if (tag != (int) obj)
                hasConflictingEnumValue = true;
              tag = (int) obj;
            }
          }
        }
        flag2 = true;
      }
      if (!ignore && !flag2)
      {
        AttributeMap attribute = MetaType.GetAttribute(attribs, "ProtoBuf.ProtoMemberAttribute");
        MetaType.GetIgnore(ref ignore, attribute, attribs, "ProtoBuf.ProtoIgnoreAttribute");
        if (!ignore && attribute != null)
        {
          MetaType.GetFieldNumber(ref tag, attribute, "Tag");
          MetaType.GetFieldName(ref name, attribute, "Name");
          MetaType.GetFieldBoolean(ref flag3, attribute, "IsRequired");
          MetaType.GetFieldBoolean(ref flag1, attribute, "IsPacked");
          MetaType.GetFieldBoolean(ref flag8, attribute, "OverwriteList");
          MetaType.GetDataFormat(ref dataFormat, attribute, "DataFormat");
          MetaType.GetFieldBoolean(ref flag5, attribute, "AsReferenceHasValue", false);
          if (flag5)
            flag5 = MetaType.GetFieldBoolean(ref flag4, attribute, "AsReference", true);
          MetaType.GetFieldBoolean(ref flag6, attribute, "DynamicType");
          flag2 = flag7 = tag > 0;
        }
        if (!flag2 && partialMembers != null)
        {
          foreach (AttributeMap partialMember in partialMembers)
          {
            object obj;
            if (partialMember.TryGet("MemberName", out obj) && (string) obj == member.Name)
            {
              MetaType.GetFieldNumber(ref tag, partialMember, "Tag");
              MetaType.GetFieldName(ref name, partialMember, "Name");
              MetaType.GetFieldBoolean(ref flag3, partialMember, "IsRequired");
              MetaType.GetFieldBoolean(ref flag1, partialMember, "IsPacked");
              MetaType.GetFieldBoolean(ref flag8, attribute, "OverwriteList");
              MetaType.GetDataFormat(ref dataFormat, partialMember, "DataFormat");
              MetaType.GetFieldBoolean(ref flag5, attribute, "AsReferenceHasValue", false);
              if (flag5)
                flag5 = MetaType.GetFieldBoolean(ref flag4, partialMember, "AsReference", true);
              MetaType.GetFieldBoolean(ref flag6, partialMember, "DynamicType");
              int num2;
              flag7 = (num2 = tag > 0 ? 1 : 0) != 0;
              flag2 = num2 != 0;
              if (num2 != 0)
                break;
            }
          }
        }
      }
      if (!ignore && !flag2 && MetaType.HasFamily(family, MetaType.AttributeFamily.DataContractSerialier))
      {
        AttributeMap attribute = MetaType.GetAttribute(attribs, "System.Runtime.Serialization.DataMemberAttribute");
        if (attribute != null)
        {
          MetaType.GetFieldNumber(ref tag, attribute, "Order");
          MetaType.GetFieldName(ref name, attribute, "Name");
          MetaType.GetFieldBoolean(ref flag3, attribute, "IsRequired");
          flag2 = tag >= num1;
          if (flag2)
            tag += dataMemberOffset;
        }
      }
      if (!ignore && !flag2 && MetaType.HasFamily(family, MetaType.AttributeFamily.XmlSerializer))
      {
        AttributeMap attrib = MetaType.GetAttribute(attribs, "System.Xml.Serialization.XmlElementAttribute") ?? MetaType.GetAttribute(attribs, "System.Xml.Serialization.XmlArrayAttribute");
        MetaType.GetIgnore(ref ignore, attrib, attribs, "System.Xml.Serialization.XmlIgnoreAttribute");
        if (attrib != null && !ignore)
        {
          MetaType.GetFieldNumber(ref tag, attrib, "Order");
          MetaType.GetFieldName(ref name, attrib, "ElementName");
          flag2 = tag >= num1;
        }
      }
      if (!ignore && !flag2 && MetaType.GetAttribute(attribs, "System.NonSerializedAttribute") != null)
        ignore = true;
      if (ignore || tag < num1 && !forced)
        return (ProtoMemberAttribute) null;
      return new ProtoMemberAttribute(tag, forced | inferByTagName)
      {
        AsReference = flag4,
        AsReferenceHasValue = flag5,
        DataFormat = dataFormat,
        DynamicType = flag6,
        IsPacked = flag1,
        OverwriteList = flag8,
        IsRequired = flag3,
        Name = string.IsNullOrEmpty(name) ? member.Name : name,
        Member = member,
        BackingMember = backingMember,
        TagIsPinned = flag7
      };
    }

    private ValueMember ApplyDefaultBehaviour(bool isEnum, ProtoMemberAttribute normalizedAttribute)
    {
      MemberInfo member;
      if (normalizedAttribute == null || (member = normalizedAttribute.Member) == (MemberInfo) null)
        return (ValueMember) null;
      Type memberType = Helpers.GetMemberType(member);
      Type itemType = (Type) null;
      Type defaultType = (Type) null;
      MetaType.ResolveListTypes((TypeModel) this.model, memberType, ref itemType, ref defaultType);
      bool flag = false;
      if (itemType != (Type) null && this.model.FindOrAddAuto(memberType, false, true, false) >= 0 && (flag = this.model[memberType].IgnoreListHandling))
      {
        itemType = (Type) null;
        defaultType = (Type) null;
      }
      AttributeMap[] attribs = AttributeMap.Create((TypeModel) this.model, member, true);
      object defaultValue = (object) null;
      if (this.model.UseImplicitZeroDefaults)
      {
        switch (Helpers.GetTypeCode(memberType))
        {
          case ProtoTypeCode.Boolean:
            defaultValue = (object) false;
            break;
          case ProtoTypeCode.Char:
            defaultValue = (object) char.MinValue;
            break;
          case ProtoTypeCode.SByte:
            defaultValue = (object) (sbyte) 0;
            break;
          case ProtoTypeCode.Byte:
            defaultValue = (object) (byte) 0;
            break;
          case ProtoTypeCode.Int16:
            defaultValue = (object) (short) 0;
            break;
          case ProtoTypeCode.UInt16:
            defaultValue = (object) (ushort) 0;
            break;
          case ProtoTypeCode.Int32:
            defaultValue = (object) 0;
            break;
          case ProtoTypeCode.UInt32:
            defaultValue = (object) 0U;
            break;
          case ProtoTypeCode.Int64:
            defaultValue = (object) 0L;
            break;
          case ProtoTypeCode.UInt64:
            defaultValue = (object) 0UL;
            break;
          case ProtoTypeCode.Single:
            defaultValue = (object) 0.0f;
            break;
          case ProtoTypeCode.Double:
            defaultValue = (object) 0.0;
            break;
          case ProtoTypeCode.Decimal:
            defaultValue = (object) 0M;
            break;
          case ProtoTypeCode.TimeSpan:
            defaultValue = (object) TimeSpan.Zero;
            break;
          case ProtoTypeCode.Guid:
            defaultValue = (object) Guid.Empty;
            break;
        }
      }
      AttributeMap attribute1;
      object obj1;
      if ((attribute1 = MetaType.GetAttribute(attribs, "System.ComponentModel.DefaultValueAttribute")) != null && attribute1.TryGet("Value", out obj1))
        defaultValue = obj1;
      ValueMember valueMember = isEnum || normalizedAttribute.Tag > 0 ? new ValueMember(this.model, this.type, normalizedAttribute.Tag, member, memberType, itemType, defaultType, normalizedAttribute.DataFormat, defaultValue) : (ValueMember) null;
      if (valueMember != null)
      {
        valueMember.BackingMember = normalizedAttribute.BackingMember;
        Type type = this.type;
        PropertyInfo property = Helpers.GetProperty(type, member.Name + "Specified", true);
        MethodInfo getMethod = Helpers.GetGetMethod(property, true, true);
        if (getMethod == (MethodInfo) null || getMethod.IsStatic)
          property = (PropertyInfo) null;
        if (property != (PropertyInfo) null)
        {
          valueMember.SetSpecified(getMethod, Helpers.GetSetMethod(property, true, true));
        }
        else
        {
          MethodInfo instanceMethod = Helpers.GetInstanceMethod(type, "ShouldSerialize" + member.Name, Helpers.EmptyTypes);
          if (instanceMethod != (MethodInfo) null && instanceMethod.ReturnType == this.model.MapType(typeof (bool)))
            valueMember.SetSpecified(instanceMethod, (MethodInfo) null);
        }
        if (!string.IsNullOrEmpty(normalizedAttribute.Name))
          valueMember.SetName(normalizedAttribute.Name);
        valueMember.IsPacked = normalizedAttribute.IsPacked;
        valueMember.IsRequired = normalizedAttribute.IsRequired;
        valueMember.OverwriteList = normalizedAttribute.OverwriteList;
        if (normalizedAttribute.AsReferenceHasValue)
          valueMember.AsReference = normalizedAttribute.AsReference;
        valueMember.DynamicType = normalizedAttribute.DynamicType;
        valueMember.IsMap = !flag && valueMember.ResolveMapTypes(out Type _, out Type _, out Type _);
        AttributeMap attribute2;
        if (valueMember.IsMap && (attribute2 = MetaType.GetAttribute(attribs, "ProtoBuf.ProtoMapAttribute")) != null)
        {
          object obj2;
          if (attribute2.TryGet("DisableMap", out obj2) && (bool) obj2)
          {
            valueMember.IsMap = false;
          }
          else
          {
            if (attribute2.TryGet("KeyFormat", out obj2))
              valueMember.MapKeyFormat = (DataFormat) obj2;
            if (attribute2.TryGet("ValueFormat", out obj2))
              valueMember.MapValueFormat = (DataFormat) obj2;
          }
        }
      }
      return valueMember;
    }

    private static void GetDataFormat(ref DataFormat value, AttributeMap attrib, string memberName)
    {
      object obj;
      if (attrib == null || value != DataFormat.Default || !attrib.TryGet(memberName, out obj) || obj == null)
        return;
      value = (DataFormat) obj;
    }

    private static void GetIgnore(
      ref bool ignore,
      AttributeMap attrib,
      AttributeMap[] attribs,
      string fullName)
    {
      if (ignore || attrib == null)
        return;
      ignore = MetaType.GetAttribute(attribs, fullName) != null;
    }

    private static void GetFieldBoolean(ref bool value, AttributeMap attrib, string memberName)
    {
      MetaType.GetFieldBoolean(ref value, attrib, memberName, true);
    }

    private static bool GetFieldBoolean(
      ref bool value,
      AttributeMap attrib,
      string memberName,
      bool publicOnly)
    {
      if (attrib == null)
        return false;
      if (value)
        return true;
      object obj;
      if (!attrib.TryGet(memberName, publicOnly, out obj) || obj == null)
        return false;
      value = (bool) obj;
      return true;
    }

    private static void GetFieldNumber(ref int value, AttributeMap attrib, string memberName)
    {
      object obj;
      if (attrib == null || value > 0 || !attrib.TryGet(memberName, out obj) || obj == null)
        return;
      value = (int) obj;
    }

    private static void GetFieldName(ref string name, AttributeMap attrib, string memberName)
    {
      object obj;
      if (attrib == null || !string.IsNullOrEmpty(name) || !attrib.TryGet(memberName, out obj) || obj == null)
        return;
      name = (string) obj;
    }

    private static AttributeMap GetAttribute(AttributeMap[] attribs, string fullName)
    {
      for (int index = 0; index < attribs.Length; ++index)
      {
        AttributeMap attrib = attribs[index];
        if (attrib != null && attrib.AttributeType.FullName == fullName)
          return attrib;
      }
      return (AttributeMap) null;
    }

    /// <summary>Adds a member (by name) to the MetaType</summary>
    public MetaType Add(int fieldNumber, string memberName)
    {
      this.AddField(fieldNumber, memberName, (Type) null, (Type) null, (object) null);
      return this;
    }

    /// <summary>
    /// Adds a member (by name) to the MetaType, returning the ValueMember rather than the fluent API.
    /// This is otherwise identical to Add.
    /// </summary>
    public ValueMember AddField(int fieldNumber, string memberName)
    {
      return this.AddField(fieldNumber, memberName, (Type) null, (Type) null, (object) null);
    }

    /// <summary>
    /// Gets or sets whether the type should use a parameterless constructor (the default),
    /// or whether the type should skip the constructor completely. This option is not supported
    /// on compact-framework.
    /// </summary>
    public bool UseConstructor
    {
      get => !this.HasFlag((ushort) 16);
      set => this.SetFlag((ushort) 16, !value, true);
    }

    /// <summary>
    /// The concrete type to create when a new instance of this type is needed; this may be useful when dealing
    /// with dynamic proxies, or with interface-based APIs
    /// </summary>
    public Type ConstructType
    {
      get => this.constructType;
      set
      {
        this.ThrowIfFrozen();
        this.constructType = value;
      }
    }

    /// <summary>Adds a member (by name) to the MetaType</summary>
    public MetaType Add(string memberName)
    {
      this.Add(this.GetNextFieldNumber(), memberName);
      return this;
    }

    /// <summary>
    /// Performs serialization of this type via a surrogate; all
    /// other serialization options are ignored and handled
    /// by the surrogate's configuration.
    /// </summary>
    public void SetSurrogate(Type surrogateType)
    {
      if (surrogateType == this.type)
        surrogateType = (Type) null;
      if (surrogateType != (Type) null && surrogateType != (Type) null && Helpers.IsAssignableFrom(this.model.MapType(typeof (IEnumerable)), surrogateType))
        throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be used as a surrogate");
      this.ThrowIfFrozen();
      this.surrogate = surrogateType;
    }

    internal MetaType GetSurrogateOrSelf()
    {
      return this.surrogate != (Type) null ? this.model[this.surrogate] : this;
    }

    internal MetaType GetSurrogateOrBaseOrSelf(bool deep)
    {
      if (this.surrogate != (Type) null)
        return this.model[this.surrogate];
      MetaType baseType = this.baseType;
      if (baseType == null)
        return this;
      if (!deep)
        return baseType;
      MetaType surrogateOrBaseOrSelf;
      do
      {
        surrogateOrBaseOrSelf = baseType;
        baseType = baseType.baseType;
      }
      while (baseType != null);
      return surrogateOrBaseOrSelf;
    }

    private int GetNextFieldNumber()
    {
      int num = 0;
      foreach (ValueMember field in this.fields)
      {
        if (field.FieldNumber > num)
          num = field.FieldNumber;
      }
      if (this.subTypes != null)
      {
        foreach (SubType subType in this.subTypes)
        {
          if (subType.FieldNumber > num)
            num = subType.FieldNumber;
        }
      }
      return num + 1;
    }

    /// <summary>Adds a set of members (by name) to the MetaType</summary>
    public MetaType Add(params string[] memberNames)
    {
      if (memberNames == null)
        throw new ArgumentNullException(nameof (memberNames));
      int nextFieldNumber = this.GetNextFieldNumber();
      for (int index = 0; index < memberNames.Length; ++index)
        this.Add(nextFieldNumber++, memberNames[index]);
      return this;
    }

    /// <summary>Adds a member (by name) to the MetaType</summary>
    public MetaType Add(int fieldNumber, string memberName, object defaultValue)
    {
      this.AddField(fieldNumber, memberName, (Type) null, (Type) null, defaultValue);
      return this;
    }

    /// <summary>
    /// Adds a member (by name) to the MetaType, including an itemType and defaultType for representing lists
    /// </summary>
    public MetaType Add(int fieldNumber, string memberName, Type itemType, Type defaultType)
    {
      this.AddField(fieldNumber, memberName, itemType, defaultType, (object) null);
      return this;
    }

    /// <summary>
    /// Adds a member (by name) to the MetaType, including an itemType and defaultType for representing lists, returning the ValueMember rather than the fluent API.
    /// This is otherwise identical to Add.
    /// </summary>
    public ValueMember AddField(
      int fieldNumber,
      string memberName,
      Type itemType,
      Type defaultType)
    {
      return this.AddField(fieldNumber, memberName, itemType, defaultType, (object) null);
    }

    private ValueMember AddField(
      int fieldNumber,
      string memberName,
      Type itemType,
      Type defaultType,
      object defaultValue)
    {
      MemberInfo memberInfo1 = (MemberInfo) null;
      MemberInfo[] member1 = this.type.GetMember(memberName, Helpers.IsEnum(this.type) ? BindingFlags.Static | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (member1 != null && member1.Length == 1)
        memberInfo1 = member1[0];
      if (memberInfo1 == (MemberInfo) null)
        throw new ArgumentException("Unable to determine member: " + memberName, nameof (memberName));
      PropertyInfo propertyInfo = (PropertyInfo) null;
      Type type1;
      switch (memberInfo1.MemberType)
      {
        case MemberTypes.Field:
          type1 = ((FieldInfo) memberInfo1).FieldType;
          break;
        case MemberTypes.Property:
          propertyInfo = (PropertyInfo) memberInfo1;
          type1 = propertyInfo.PropertyType;
          break;
        default:
          throw new NotSupportedException(memberInfo1.MemberType.ToString());
      }
      MetaType.ResolveListTypes((TypeModel) this.model, type1, ref itemType, ref defaultType);
      MemberInfo memberInfo2 = (MemberInfo) null;
      if ((object) propertyInfo != null && !propertyInfo.CanWrite)
      {
        string str = "<" + memberInfo1.Name + ">k__BackingField";
        MemberInfo[] member2 = this.type.GetMember("<" + memberInfo1.Name + ">k__BackingField", Helpers.IsEnum(this.type) ? BindingFlags.Static | BindingFlags.Public : BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (member2 != null && member2.Length == 1 && member2[0] as FieldInfo != (FieldInfo) null)
          memberInfo2 = member2[0];
      }
      RuntimeTypeModel model = this.model;
      Type type2 = this.type;
      int fieldNumber1 = fieldNumber;
      MemberInfo member3 = memberInfo2;
      if ((object) member3 == null)
        member3 = memberInfo1;
      Type memberType = type1;
      Type itemType1 = itemType;
      Type defaultType1 = defaultType;
      object defaultValue1 = defaultValue;
      ValueMember member4 = new ValueMember(model, type2, fieldNumber1, member3, memberType, itemType1, defaultType1, DataFormat.Default, defaultValue1);
      if (memberInfo2 != (MemberInfo) null)
        member4.SetName(memberInfo1.Name);
      this.Add(member4);
      return member4;
    }

    internal static void ResolveListTypes(
      TypeModel model,
      Type type,
      ref Type itemType,
      ref Type defaultType)
    {
      if (type == (Type) null)
        return;
      if (type.IsArray)
      {
        itemType = type.GetArrayRank() == 1 ? type.GetElementType() : throw new NotSupportedException("Multi-dimensional arrays are not supported");
        defaultType = !(itemType == model.MapType(typeof (byte))) ? type : (itemType = (Type) null);
      }
      if (itemType == (Type) null)
        itemType = TypeModel.GetListItemType(model, type);
      if (itemType != (Type) null)
      {
        Type itemType1 = (Type) null;
        Type defaultType1 = (Type) null;
        MetaType.ResolveListTypes(model, itemType, ref itemType1, ref defaultType1);
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
        if (type.IsGenericType && type.GetGenericTypeDefinition() == model.MapType(typeof (IDictionary<,>)) && itemType == model.MapType(typeof (KeyValuePair<,>)).MakeGenericType(genericArguments = type.GetGenericArguments()))
          defaultType = model.MapType(typeof (Dictionary<,>)).MakeGenericType(genericArguments);
        else
          defaultType = model.MapType(typeof (List<>)).MakeGenericType(itemType);
      }
      if (!(defaultType != (Type) null) || Helpers.IsAssignableFrom(type, defaultType))
        return;
      defaultType = (Type) null;
    }

    private void Add(ValueMember member)
    {
      int opaqueToken = 0;
      try
      {
        this.model.TakeLock(ref opaqueToken);
        this.ThrowIfFrozen();
        this.fields.Add((object) member);
      }
      finally
      {
        this.model.ReleaseLock(opaqueToken);
      }
    }

    /// <summary>
    /// Returns the ValueMember that matchs a given field number, or null if not found
    /// </summary>
    public ValueMember this[int fieldNumber]
    {
      get
      {
        foreach (ValueMember field in this.fields)
        {
          if (field.FieldNumber == fieldNumber)
            return field;
        }
        return (ValueMember) null;
      }
    }

    /// <summary>
    /// Returns the ValueMember that matchs a given member (property/field), or null if not found
    /// </summary>
    public ValueMember this[MemberInfo member]
    {
      get
      {
        if (member == (MemberInfo) null)
          return (ValueMember) null;
        foreach (ValueMember field in this.fields)
        {
          if (field.Member == member || field.BackingMember == member)
            return field;
        }
        return (ValueMember) null;
      }
    }

    /// <summary>
    /// Returns the ValueMember instances associated with this type
    /// </summary>
    public ValueMember[] GetFields()
    {
      ValueMember[] array = new ValueMember[this.fields.Count];
      this.fields.CopyTo((Array) array, 0);
      Array.Sort<ValueMember>(array, (IComparer<ValueMember>) ValueMember.Comparer.Default);
      return array;
    }

    /// <summary>
    /// Returns the SubType instances associated with this type
    /// </summary>
    public SubType[] GetSubtypes()
    {
      if (this.subTypes == null || this.subTypes.Count == 0)
        return new SubType[0];
      SubType[] array = new SubType[this.subTypes.Count];
      this.subTypes.CopyTo((Array) array, 0);
      Array.Sort<SubType>(array, (IComparer<SubType>) SubType.Comparer.Default);
      return array;
    }

    internal IEnumerable<Type> GetAllGenericArguments()
    {
      return MetaType.GetAllGenericArguments(this.type);
    }

    private static IEnumerable<Type> GetAllGenericArguments(Type type)
    {
      Type[] typeArray = type.GetGenericArguments();
      for (int index = 0; index < typeArray.Length; ++index)
      {
        Type arg = typeArray[index];
        yield return arg;
        foreach (Type allGenericArgument in MetaType.GetAllGenericArguments(arg))
          yield return allGenericArgument;
        arg = (Type) null;
      }
      typeArray = (Type[]) null;
    }

    /// <summary>
    /// Compiles the serializer for this type; this is *not* a full
    /// standalone compile, but can significantly boost performance
    /// while allowing additional types to be added.
    /// </summary>
    /// <remarks>An in-place compile can access non-public types / members</remarks>
    public void CompileInPlace()
    {
      this.serializer = (IProtoTypeSerializer) CompiledSerializer.Wrap(this.Serializer, (TypeModel) this.model);
    }

    internal bool IsDefined(int fieldNumber)
    {
      foreach (ValueMember field in this.fields)
      {
        if (field.FieldNumber == fieldNumber)
          return true;
      }
      return false;
    }

    internal int GetKey(bool demand, bool getBaseKey)
    {
      return this.model.GetKey(this.type, demand, getBaseKey);
    }

    internal EnumSerializer.EnumPair[] GetEnumMap()
    {
      if (this.HasFlag((ushort) 2))
        return (EnumSerializer.EnumPair[]) null;
      EnumSerializer.EnumPair[] enumMap = new EnumSerializer.EnumPair[this.fields.Count];
      for (int index = 0; index < enumMap.Length; ++index)
      {
        ValueMember field = (ValueMember) this.fields[index];
        int fieldNumber = field.FieldNumber;
        object rawEnumValue = field.GetRawEnumValue();
        enumMap[index] = new EnumSerializer.EnumPair(fieldNumber, rawEnumValue, field.MemberType);
      }
      return enumMap;
    }

    /// <summary>
    /// Gets or sets a value indicating that an enum should be treated directly as an int/short/etc, rather
    /// than enforcing .proto enum rules. This is useful *in particul* for [Flags] enums.
    /// </summary>
    public bool EnumPassthru
    {
      get => this.HasFlag((ushort) 2);
      set => this.SetFlag((ushort) 2, value, true);
    }

    /// <summary>
    /// Gets or sets a value indicating that this type should NOT be treated as a list, even if it has
    /// familiar list-like characteristics (enumerable, add, etc)
    /// </summary>
    public bool IgnoreListHandling
    {
      get => this.HasFlag((ushort) 128);
      set => this.SetFlag((ushort) 128, value, true);
    }

    internal bool Pending
    {
      get => this.HasFlag((ushort) 1);
      set => this.SetFlag((ushort) 1, value, false);
    }

    private bool HasFlag(ushort flag) => ((int) this.flags & (int) flag) == (int) flag;

    private void SetFlag(ushort flag, bool value, bool throwIfFrozen)
    {
      if (throwIfFrozen && this.HasFlag(flag) != value)
        this.ThrowIfFrozen();
      if (value)
        this.flags |= flag;
      else
        this.flags &= ~flag;
    }

    internal static MetaType GetRootType(MetaType source)
    {
      MetaType baseType1;
      for (; source.serializer != null; source = baseType1)
      {
        baseType1 = source.baseType;
        if (baseType1 == null)
          return source;
      }
      RuntimeTypeModel model = source.model;
      int opaqueToken = 0;
      try
      {
        model.TakeLock(ref opaqueToken);
        MetaType baseType2;
        while ((baseType2 = source.baseType) != null)
          source = baseType2;
        return source;
      }
      finally
      {
        model.ReleaseLock(opaqueToken);
      }
    }

    internal bool IsPrepared() => this.serializer is CompiledSerializer;

    internal IEnumerable Fields => (IEnumerable) this.fields;

    internal static StringBuilder NewLine(StringBuilder builder, int indent)
    {
      return Helpers.AppendLine(builder).Append(' ', indent * 3);
    }

    internal bool IsAutoTuple => this.HasFlag((ushort) 64);

    /// <summary>
    /// Indicates whether this type should always be treated as a "group" (rather than a string-prefixed sub-message)
    /// </summary>
    public bool IsGroup
    {
      get => this.HasFlag((ushort) 256);
      set => this.SetFlag((ushort) 256, value, true);
    }

    internal void WriteSchema(
      StringBuilder builder,
      int indent,
      ref RuntimeTypeModel.CommonImports imports,
      ProtoSyntax syntax)
    {
      if (this.surrogate != (Type) null)
        return;
      ValueMember[] array1 = new ValueMember[this.fields.Count];
      this.fields.CopyTo((Array) array1, 0);
      Array.Sort<ValueMember>(array1, (IComparer<ValueMember>) ValueMember.Comparer.Default);
      if (this.IsList)
      {
        string schemaTypeName = this.model.GetSchemaTypeName(TypeModel.GetListItemType((TypeModel) this.model, this.type), DataFormat.Default, false, false, ref imports);
        MetaType.NewLine(builder, indent).Append("message ").Append(this.GetSchemaTypeName()).Append(" {");
        MetaType.NewLine(builder, indent + 1).Append("repeated ").Append(schemaTypeName).Append(" items = 1;");
        MetaType.NewLine(builder, indent).Append('}');
      }
      else if (this.IsAutoTuple)
      {
        MemberInfo[] mappedMembers;
        if (!(MetaType.ResolveTupleConstructor(this.type, out mappedMembers) != (ConstructorInfo) null))
          return;
        MetaType.NewLine(builder, indent).Append("message ").Append(this.GetSchemaTypeName()).Append(" {");
        for (int index = 0; index < mappedMembers.Length; ++index)
        {
          Type effectiveType;
          if (mappedMembers[index] is PropertyInfo propertyInfo)
          {
            effectiveType = propertyInfo.PropertyType;
          }
          else
          {
            if (!(mappedMembers[index] is FieldInfo fieldInfo))
              throw new NotSupportedException("Unknown member type: " + mappedMembers[index].GetType().Name);
            effectiveType = fieldInfo.FieldType;
          }
          MetaType.NewLine(builder, indent + 1).Append(syntax == ProtoSyntax.Proto2 ? "optional " : "").Append(this.model.GetSchemaTypeName(effectiveType, DataFormat.Default, false, false, ref imports).Replace('.', '_')).Append(' ').Append(mappedMembers[index].Name).Append(" = ").Append(index + 1).Append(';');
        }
        MetaType.NewLine(builder, indent).Append('}');
      }
      else if (Helpers.IsEnum(this.type))
      {
        MetaType.NewLine(builder, indent).Append("enum ").Append(this.GetSchemaTypeName()).Append(" {");
        if (array1.Length == 0 && this.EnumPassthru)
        {
          if (this.type.IsDefined(this.model.MapType(typeof (FlagsAttribute)), false))
            MetaType.NewLine(builder, indent + 1).Append("// this is a composite/flags enumeration");
          else
            MetaType.NewLine(builder, indent + 1).Append("// this enumeration will be passed as a raw value");
          foreach (FieldInfo field in this.type.GetFields())
          {
            if (field.IsStatic && field.IsLiteral)
            {
              object rawConstantValue = field.GetRawConstantValue();
              MetaType.NewLine(builder, indent + 1).Append(field.Name).Append(" = ").Append(rawConstantValue).Append(";");
            }
          }
        }
        else
        {
          Dictionary<int, int> dictionary = new Dictionary<int, int>(array1.Length);
          bool flag1 = false;
          foreach (ValueMember valueMember in array1)
          {
            if (dictionary.ContainsKey(valueMember.FieldNumber))
            {
              flag1 = true;
              break;
            }
            dictionary.Add(valueMember.FieldNumber, 1);
          }
          if (flag1)
            MetaType.NewLine(builder, indent + 1).Append("option allow_alias = true;");
          bool flag2 = false;
          foreach (ValueMember valueMember in array1)
          {
            if (valueMember.FieldNumber == 0)
            {
              MetaType.NewLine(builder, indent + 1).Append(valueMember.Name).Append(" = ").Append(valueMember.FieldNumber).Append(';');
              flag2 = true;
            }
          }
          if (syntax == ProtoSyntax.Proto3 && !flag2)
            MetaType.NewLine(builder, indent + 1).Append("ZERO = 0; // proto3 requires a zero value as the first item (it can be named anything)");
          foreach (ValueMember valueMember in array1)
          {
            if (valueMember.FieldNumber != 0)
              MetaType.NewLine(builder, indent + 1).Append(valueMember.Name).Append(" = ").Append(valueMember.FieldNumber).Append(';');
          }
        }
        MetaType.NewLine(builder, indent).Append('}');
      }
      else
      {
        MetaType.NewLine(builder, indent).Append("message ").Append(this.GetSchemaTypeName()).Append(" {");
        foreach (ValueMember valueMember in array1)
        {
          bool hasOption = false;
          string schemaTypeName1;
          if (valueMember.IsMap)
          {
            Type keyType;
            Type valueType;
            valueMember.ResolveMapTypes(out Type _, out keyType, out valueType);
            string schemaTypeName2 = this.model.GetSchemaTypeName(keyType, valueMember.MapKeyFormat, false, false, ref imports);
            schemaTypeName1 = this.model.GetSchemaTypeName(valueType, valueMember.MapKeyFormat, valueMember.AsReference, valueMember.DynamicType, ref imports);
            MetaType.NewLine(builder, indent + 1).Append("map<").Append(schemaTypeName2).Append(",").Append(schemaTypeName1).Append("> ").Append(valueMember.Name).Append(" = ").Append(valueMember.FieldNumber).Append(";");
          }
          else
          {
            string str = valueMember.ItemType != (Type) null ? "repeated " : (syntax == ProtoSyntax.Proto2 ? (valueMember.IsRequired ? "required " : "optional ") : "");
            MetaType.NewLine(builder, indent + 1).Append(str);
            if (valueMember.DataFormat == DataFormat.Group)
              builder.Append("group ");
            schemaTypeName1 = valueMember.GetSchemaTypeName(true, ref imports);
            builder.Append(schemaTypeName1).Append(" ").Append(valueMember.Name).Append(" = ").Append(valueMember.FieldNumber);
            if (syntax == ProtoSyntax.Proto2 && valueMember.DefaultValue != null && !valueMember.IsRequired)
            {
              if (valueMember.DefaultValue is string)
                MetaType.AddOption(builder, ref hasOption).Append("default = \"").Append(valueMember.DefaultValue).Append("\"");
              else if (!(valueMember.DefaultValue is TimeSpan))
              {
                if (valueMember.DefaultValue is bool)
                  MetaType.AddOption(builder, ref hasOption).Append((bool) valueMember.DefaultValue ? "default = true" : "default = false");
                else
                  MetaType.AddOption(builder, ref hasOption).Append("default = ").Append(valueMember.DefaultValue);
              }
            }
            if (MetaType.CanPack(valueMember.ItemType))
            {
              if (syntax == ProtoSyntax.Proto2)
              {
                if (valueMember.IsPacked)
                  MetaType.AddOption(builder, ref hasOption).Append("packed = true");
              }
              else if (!valueMember.IsPacked)
                MetaType.AddOption(builder, ref hasOption).Append("packed = false");
            }
            if (valueMember.AsReference)
            {
              imports |= RuntimeTypeModel.CommonImports.Protogen;
              MetaType.AddOption(builder, ref hasOption).Append("(.protobuf_net.fieldopt).asRef = true");
            }
            if (valueMember.DynamicType)
            {
              imports |= RuntimeTypeModel.CommonImports.Protogen;
              MetaType.AddOption(builder, ref hasOption).Append("(.protobuf_net.fieldopt).dynamicType = true");
            }
            MetaType.CloseOption(builder, ref hasOption).Append(';');
            if (syntax != ProtoSyntax.Proto2 && valueMember.DefaultValue != null && !valueMember.IsRequired && !MetaType.IsImplicitDefault(valueMember.DefaultValue))
              builder.Append(" // default value could not be applied: ").Append(valueMember.DefaultValue);
          }
          if (schemaTypeName1 == ".bcl.NetObjectProxy" && valueMember.AsReference && !valueMember.DynamicType)
            builder.Append(" // reference-tracked ").Append(valueMember.GetSchemaTypeName(false, ref imports));
        }
        if (this.subTypes != null && this.subTypes.Count != 0)
        {
          SubType[] array2 = new SubType[this.subTypes.Count];
          this.subTypes.CopyTo((Array) array2, 0);
          Array.Sort<SubType>(array2, (IComparer<SubType>) SubType.Comparer.Default);
          string[] array3 = new string[array2.Length];
          for (int index = 0; index < array2.Length; ++index)
            array3[index] = array2[index].DerivedType.GetSchemaTypeName();
          string str1 = "subtype";
          while (Array.IndexOf<string>(array3, str1) >= 0)
            str1 = "_" + str1;
          MetaType.NewLine(builder, indent + 1).Append("oneof ").Append(str1).Append(" {");
          for (int index = 0; index < array2.Length; ++index)
          {
            string str2 = array3[index];
            MetaType.NewLine(builder, indent + 2).Append(str2).Append(" ").Append(str2).Append(" = ").Append(array2[index].FieldNumber).Append(';');
          }
          MetaType.NewLine(builder, indent + 1).Append("}");
        }
        MetaType.NewLine(builder, indent).Append('}');
      }
    }

    private static StringBuilder AddOption(StringBuilder builder, ref bool hasOption)
    {
      if (hasOption)
        return builder.Append(", ");
      hasOption = true;
      return builder.Append(" [");
    }

    private static StringBuilder CloseOption(StringBuilder builder, ref bool hasOption)
    {
      if (!hasOption)
        return builder;
      hasOption = false;
      return builder.Append("]");
    }

    private static bool IsImplicitDefault(object value)
    {
      try
      {
        if (value == null)
          return false;
        switch (Helpers.GetTypeCode(value.GetType()))
        {
          case ProtoTypeCode.Boolean:
            return !(bool) value;
          case ProtoTypeCode.Char:
            return (char) value == char.MinValue;
          case ProtoTypeCode.SByte:
            return (sbyte) value == (sbyte) 0;
          case ProtoTypeCode.Byte:
            return (byte) value == (byte) 0;
          case ProtoTypeCode.Int16:
            return (short) value == (short) 0;
          case ProtoTypeCode.UInt16:
            return (ushort) value == (ushort) 0;
          case ProtoTypeCode.Int32:
            return (int) value == 0;
          case ProtoTypeCode.UInt32:
            return (uint) value == 0U;
          case ProtoTypeCode.Int64:
            return (long) value == 0L;
          case ProtoTypeCode.UInt64:
            return (ulong) value == 0UL;
          case ProtoTypeCode.Single:
            return (double) (float) value == 0.0;
          case ProtoTypeCode.Double:
            return (double) value == 0.0;
          case ProtoTypeCode.Decimal:
            return (Decimal) value == 0M;
          case ProtoTypeCode.DateTime:
            return (DateTime) value == new DateTime();
          case ProtoTypeCode.String:
            return (string) value == "";
          case ProtoTypeCode.TimeSpan:
            return (TimeSpan) value == TimeSpan.Zero;
        }
      }
      catch
      {
      }
      return false;
    }

    private static bool CanPack(Type type)
    {
      return !(type == (Type) null) && (uint) (Helpers.GetTypeCode(type) - 3) <= 11U;
    }

    internal sealed class Comparer : IComparer, IComparer<MetaType>
    {
      public static readonly MetaType.Comparer Default = new MetaType.Comparer();

      public int Compare(object x, object y) => this.Compare(x as MetaType, y as MetaType);

      public int Compare(MetaType x, MetaType y)
      {
        if (x == y)
          return 0;
        if (x == null)
          return -1;
        return y == null ? 1 : string.Compare(x.GetSchemaTypeName(), y.GetSchemaTypeName(), StringComparison.Ordinal);
      }
    }

    [Flags]
    internal enum AttributeFamily
    {
      None = 0,
      ProtoBuf = 1,
      DataContractSerialier = 2,
      XmlSerializer = 4,
      AutoTuple = 8,
    }
  }
}
