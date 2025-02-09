// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.ValueMember
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Represents a member (property/field) that is mapped to a protobuf field
  /// </summary>
  public class ValueMember
  {
    private readonly int fieldNumber;
    private readonly MemberInfo originalMember;
    private MemberInfo backingMember;
    private readonly Type parentType;
    private readonly Type itemType;
    private readonly Type defaultType;
    private readonly Type memberType;
    private object defaultValue;
    private readonly RuntimeTypeModel model;
    private IProtoSerializer serializer;
    private DataFormat dataFormat;
    private DataFormat mapKeyFormat;
    private DataFormat mapValueFormat;
    private MethodInfo getSpecified;
    private MethodInfo setSpecified;
    private string name;
    private const byte OPTIONS_IsStrict = 1;
    private const byte OPTIONS_IsPacked = 2;
    private const byte OPTIONS_IsRequired = 4;
    private const byte OPTIONS_OverwriteList = 8;
    private const byte OPTIONS_SupportNull = 16;
    private const byte OPTIONS_AsReference = 32;
    private const byte OPTIONS_IsMap = 64;
    private const byte OPTIONS_DynamicType = 128;
    private byte flags;

    /// <summary>
    /// The number that identifies this member in a protobuf stream
    /// </summary>
    public int FieldNumber => this.fieldNumber;

    /// <summary>
    /// Gets the member (field/property) which this member relates to.
    /// </summary>
    public MemberInfo Member => this.originalMember;

    /// <summary>
    /// Gets the backing member (field/property) which this member relates to
    /// </summary>
    public MemberInfo BackingMember
    {
      get => this.backingMember;
      set
      {
        if (!(this.backingMember != value))
          return;
        this.ThrowIfFrozen();
        this.backingMember = value;
      }
    }

    /// <summary>
    /// Within a list / array / etc, the type of object for each item in the list (especially useful with ArrayList)
    /// </summary>
    public Type ItemType => this.itemType;

    /// <summary>The underlying type of the member</summary>
    public Type MemberType => this.memberType;

    /// <summary>
    /// For abstract types (IList etc), the type of concrete object to create (if required)
    /// </summary>
    public Type DefaultType => this.defaultType;

    /// <summary>The type the defines the member</summary>
    public Type ParentType => this.parentType;

    /// <summary>
    /// The default value of the item (members with this value will not be serialized)
    /// </summary>
    public object DefaultValue
    {
      get => this.defaultValue;
      set
      {
        if (this.defaultValue == value)
          return;
        this.ThrowIfFrozen();
        this.defaultValue = value;
      }
    }

    /// <summary>Creates a new ValueMember instance</summary>
    public ValueMember(
      RuntimeTypeModel model,
      Type parentType,
      int fieldNumber,
      MemberInfo member,
      Type memberType,
      Type itemType,
      Type defaultType,
      DataFormat dataFormat,
      object defaultValue)
      : this(model, fieldNumber, memberType, itemType, defaultType, dataFormat)
    {
      if (parentType == (Type) null)
        throw new ArgumentNullException(nameof (parentType));
      if (fieldNumber < 1 && !Helpers.IsEnum(parentType))
        throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      this.originalMember = member ?? throw new ArgumentNullException(nameof (member));
      this.parentType = parentType;
      if (fieldNumber < 1 && !Helpers.IsEnum(parentType))
        throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      if (defaultValue != null && model.MapType(defaultValue.GetType()) != memberType)
        defaultValue = ValueMember.ParseDefaultValue(memberType, defaultValue);
      this.defaultValue = defaultValue;
      MetaType withoutAdd = model.FindWithoutAdd(memberType);
      if (withoutAdd != null)
        this.AsReference = withoutAdd.AsReferenceDefault;
      else
        this.AsReference = MetaType.GetAsReferenceDefault(model, memberType);
    }

    /// <summary>Creates a new ValueMember instance</summary>
    internal ValueMember(
      RuntimeTypeModel model,
      int fieldNumber,
      Type memberType,
      Type itemType,
      Type defaultType,
      DataFormat dataFormat)
    {
      this.fieldNumber = fieldNumber;
      this.memberType = memberType ?? throw new ArgumentNullException(nameof (memberType));
      this.itemType = itemType;
      this.defaultType = defaultType;
      this.model = model ?? throw new ArgumentNullException(nameof (model));
      this.dataFormat = dataFormat;
    }

    internal object GetRawEnumValue() => ((FieldInfo) this.originalMember).GetRawConstantValue();

    private static object ParseDefaultValue(Type type, object value)
    {
      Type underlyingType = Helpers.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
        type = underlyingType;
      if (value is string defaultValue)
      {
        if (Helpers.IsEnum(type))
          return Helpers.ParseEnum(type, defaultValue);
        switch (Helpers.GetTypeCode(type))
        {
          case ProtoTypeCode.Boolean:
            return (object) bool.Parse(defaultValue);
          case ProtoTypeCode.Char:
            return defaultValue.Length == 1 ? (object) defaultValue[0] : throw new FormatException("Single character expected: \"" + defaultValue + "\"");
          case ProtoTypeCode.SByte:
            return (object) sbyte.Parse(defaultValue, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Byte:
            return (object) byte.Parse(defaultValue, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Int16:
            return (object) short.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.UInt16:
            return (object) ushort.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Int32:
            return (object) int.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.UInt32:
            return (object) uint.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Int64:
            return (object) long.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.UInt64:
            return (object) ulong.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Single:
            return (object) float.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Double:
            return (object) double.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.Decimal:
            return (object) Decimal.Parse(defaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.DateTime:
            return (object) DateTime.Parse(defaultValue, (IFormatProvider) CultureInfo.InvariantCulture);
          case ProtoTypeCode.String:
            return (object) defaultValue;
          case ProtoTypeCode.TimeSpan:
            return (object) TimeSpan.Parse(defaultValue);
          case ProtoTypeCode.Guid:
            return (object) new Guid(defaultValue);
          case ProtoTypeCode.Uri:
            return (object) defaultValue;
        }
      }
      return Helpers.IsEnum(type) ? Enum.ToObject(type, value) : Convert.ChangeType(value, type, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    internal IProtoSerializer Serializer
    {
      get => this.serializer ?? (this.serializer = this.BuildSerializer());
    }

    /// <summary>
    /// Specifies the rules used to process the field; this is used to determine the most appropriate
    /// wite-type, but also to describe subtypes <i>within</i> that wire-type (such as SignedVariant)
    /// </summary>
    public DataFormat DataFormat
    {
      get => this.dataFormat;
      set
      {
        if (value == this.dataFormat)
          return;
        this.ThrowIfFrozen();
        this.dataFormat = value;
      }
    }

    /// <summary>
    /// Indicates whether this field should follow strict encoding rules; this means (for example) that if a "fixed32"
    /// is encountered when "variant" is defined, then it will fail (throw an exception) when parsing. Note that
    /// when serializing the defined type is always used.
    /// </summary>
    public bool IsStrict
    {
      get => this.HasFlag((byte) 1);
      set => this.SetFlag((byte) 1, value, true);
    }

    /// <summary>
    /// Indicates whether this field should use packed encoding (which can save lots of space for repeated primitive values).
    /// This option only applies to list/array data of primitive types (int, double, etc).
    /// </summary>
    public bool IsPacked
    {
      get => this.HasFlag((byte) 2);
      set => this.SetFlag((byte) 2, value, true);
    }

    /// <summary>
    /// Indicates whether this field should *repace* existing values (the default is false, meaning *append*).
    /// This option only applies to list/array data.
    /// </summary>
    public bool OverwriteList
    {
      get => this.HasFlag((byte) 8);
      set => this.SetFlag((byte) 8, value, true);
    }

    /// <summary>Indicates whether this field is mandatory.</summary>
    public bool IsRequired
    {
      get => this.HasFlag((byte) 4);
      set => this.SetFlag((byte) 4, value, true);
    }

    /// <summary>Enables full object-tracking/full-graph support.</summary>
    public bool AsReference
    {
      get => this.HasFlag((byte) 32);
      set => this.SetFlag((byte) 32, value, true);
    }

    /// <summary>
    /// Embeds the type information into the stream, allowing usage with types not known in advance.
    /// </summary>
    public bool DynamicType
    {
      get => this.HasFlag((byte) 128);
      set => this.SetFlag((byte) 128, value, true);
    }

    /// <summary>
    /// Indicates that the member should be treated as a protobuf Map
    /// </summary>
    public bool IsMap
    {
      get => this.HasFlag((byte) 64);
      set => this.SetFlag((byte) 64, value, true);
    }

    /// <summary>
    /// Specifies the data-format that should be used for the key, when IsMap is enabled
    /// </summary>
    public DataFormat MapKeyFormat
    {
      get => this.mapKeyFormat;
      set
      {
        if (this.mapKeyFormat == value)
          return;
        this.ThrowIfFrozen();
        this.mapKeyFormat = value;
      }
    }

    /// <summary>
    /// Specifies the data-format that should be used for the value, when IsMap is enabled
    /// </summary>
    public DataFormat MapValueFormat
    {
      get => this.mapValueFormat;
      set
      {
        if (this.mapValueFormat == value)
          return;
        this.ThrowIfFrozen();
        this.mapValueFormat = value;
      }
    }

    /// <summary>
    /// Specifies methods for working with optional data members.
    /// </summary>
    /// <param name="getSpecified">Provides a method (null for none) to query whether this member should
    /// be serialized; it must be of the form "bool {Method}()". The member is only serialized if the
    /// method returns true.</param>
    /// <param name="setSpecified">Provides a method (null for none) to indicate that a member was
    /// deserialized; it must be of the form "void {Method}(bool)", and will be called with "true"
    /// when data is found.</param>
    public void SetSpecified(MethodInfo getSpecified, MethodInfo setSpecified)
    {
      if (!(this.getSpecified != getSpecified) && !(this.setSpecified != setSpecified))
        return;
      if (getSpecified != (MethodInfo) null && (getSpecified.ReturnType != this.model.MapType(typeof (bool)) || getSpecified.IsStatic || getSpecified.GetParameters().Length != 0))
        throw new ArgumentException("Invalid pattern for checking member-specified", nameof (getSpecified));
      ParameterInfo[] parameters;
      if (setSpecified != (MethodInfo) null && (setSpecified.ReturnType != this.model.MapType(typeof (void)) || setSpecified.IsStatic || (parameters = setSpecified.GetParameters()).Length != 1 || parameters[0].ParameterType != this.model.MapType(typeof (bool))))
        throw new ArgumentException("Invalid pattern for setting member-specified", nameof (setSpecified));
      this.ThrowIfFrozen();
      this.getSpecified = getSpecified;
      this.setSpecified = setSpecified;
    }

    private void ThrowIfFrozen()
    {
      if (this.serializer != null)
        throw new InvalidOperationException("The type cannot be changed once a serializer has been generated");
    }

    internal bool ResolveMapTypes(out Type dictionaryType, out Type keyType, out Type valueType)
    {
      dictionaryType = keyType = valueType = (Type) null;
      try
      {
        Type memberType = this.memberType;
        if (ImmutableCollectionDecorator.IdentifyImmutable((TypeModel) this.model, this.MemberType, out MethodInfo _, out PropertyInfo _, out PropertyInfo _, out MethodInfo _, out MethodInfo _, out MethodInfo _))
          return false;
        if (memberType.IsInterface && memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof (IDictionary<,>))
        {
          Type[] genericArguments = this.memberType.GetGenericArguments();
          if (ValueMember.IsValidMapKeyType(genericArguments[0]))
          {
            keyType = genericArguments[0];
            valueType = genericArguments[1];
            dictionaryType = this.memberType;
          }
          return false;
        }
        foreach (Type type1 in this.memberType.GetInterfaces())
        {
          Type type2 = type1;
          if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof (IDictionary<,>))
          {
            if (dictionaryType != (Type) null)
              throw new InvalidOperationException("Multiple dictionary interfaces implemented by type: " + this.memberType.FullName);
            Type[] genericArguments = type1.GetGenericArguments();
            if (ValueMember.IsValidMapKeyType(genericArguments[0]))
            {
              keyType = genericArguments[0];
              valueType = genericArguments[1];
              dictionaryType = this.memberType;
            }
          }
        }
        if (dictionaryType == (Type) null)
          return false;
        Type itemType = (Type) null;
        Type defaultType = (Type) null;
        this.model.ResolveListTypes(valueType, ref itemType, ref defaultType);
        return !(itemType != (Type) null) && dictionaryType != (Type) null;
      }
      catch
      {
        return false;
      }
    }

    private static bool IsValidMapKeyType(Type type)
    {
      if (type == (Type) null || Helpers.IsEnum(type))
        return false;
      switch (Helpers.GetTypeCode(type))
      {
        case ProtoTypeCode.Boolean:
        case ProtoTypeCode.Char:
        case ProtoTypeCode.SByte:
        case ProtoTypeCode.Byte:
        case ProtoTypeCode.Int16:
        case ProtoTypeCode.UInt16:
        case ProtoTypeCode.Int32:
        case ProtoTypeCode.UInt32:
        case ProtoTypeCode.Int64:
        case ProtoTypeCode.UInt64:
        case ProtoTypeCode.String:
          return true;
        default:
          return false;
      }
    }

    private IProtoSerializer BuildSerializer()
    {
      int opaqueToken = 0;
      try
      {
        this.model.TakeLock(ref opaqueToken);
        MemberInfo memberInfo = this.backingMember;
        if ((object) memberInfo == null)
          memberInfo = this.originalMember;
        MemberInfo member = memberInfo;
        IProtoSerializer tail;
        if (this.IsMap)
        {
          Type dictionaryType;
          Type keyType;
          Type valueType;
          this.ResolveMapTypes(out dictionaryType, out keyType, out valueType);
          if (dictionaryType == (Type) null)
            throw new InvalidOperationException("Unable to resolve map type for type: " + this.memberType.FullName);
          Type type = this.defaultType;
          if (type == (Type) null && Helpers.IsClass(this.memberType))
            type = this.memberType;
          WireType defaultWireType1;
          IProtoSerializer coreSerializer1 = ValueMember.TryGetCoreSerializer(this.model, this.MapKeyFormat, keyType, out defaultWireType1, false, false, false, false);
          if (!this.AsReference)
            this.AsReference = MetaType.GetAsReferenceDefault(this.model, valueType);
          WireType defaultWireType2;
          IProtoSerializer coreSerializer2 = ValueMember.TryGetCoreSerializer(this.model, this.MapValueFormat, valueType, out defaultWireType2, this.AsReference, this.DynamicType, false, true);
          ConstructorInfo[] constructors = typeof (MapDecorator<,,>).MakeGenericType(dictionaryType, keyType, valueType).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
          if (constructors.Length != 1)
            throw new InvalidOperationException("Unable to resolve MapDecorator constructor");
          tail = (IProtoSerializer) constructors[0].Invoke(new object[9]
          {
            (object) this.model,
            (object) type,
            (object) coreSerializer1,
            (object) coreSerializer2,
            (object) this.fieldNumber,
            (object) (WireType) (this.DataFormat == DataFormat.Group ? 3 : 2),
            (object) defaultWireType1,
            (object) defaultWireType2,
            (object) this.OverwriteList
          });
        }
        else
        {
          Type type1 = this.itemType;
          if ((object) type1 == null)
            type1 = this.memberType;
          Type type2 = type1;
          WireType defaultWireType;
          IProtoSerializer coreSerializer = ValueMember.TryGetCoreSerializer(this.model, this.dataFormat, type2, out defaultWireType, this.AsReference, this.DynamicType, this.OverwriteList, true);
          if (coreSerializer == null)
            throw new InvalidOperationException("No serializer defined for type: " + type2.FullName);
          if (this.itemType != (Type) null && this.SupportNull)
          {
            if (this.IsPacked)
              throw new NotSupportedException("Packed encodings cannot support null values");
            tail = (IProtoSerializer) new TagDecorator(this.fieldNumber, WireType.StartGroup, false, (IProtoSerializer) new NullDecorator((TypeModel) this.model, (IProtoSerializer) new TagDecorator(1, defaultWireType, this.IsStrict, coreSerializer)));
          }
          else
            tail = (IProtoSerializer) new TagDecorator(this.fieldNumber, defaultWireType, this.IsStrict, coreSerializer);
          if (this.itemType != (Type) null)
          {
            Type type3 = this.SupportNull ? this.itemType : Helpers.GetUnderlyingType(this.itemType) ?? this.itemType;
            tail = !this.memberType.IsArray ? (IProtoSerializer) ListDecorator.Create((TypeModel) this.model, this.memberType, this.defaultType, tail, this.fieldNumber, this.IsPacked, defaultWireType, member != (MemberInfo) null && PropertyDecorator.CanWrite((TypeModel) this.model, member), this.OverwriteList, this.SupportNull) : (IProtoSerializer) new ArrayDecorator((TypeModel) this.model, tail, this.fieldNumber, this.IsPacked, defaultWireType, this.memberType, this.OverwriteList, this.SupportNull);
          }
          else if (this.defaultValue != null && !this.IsRequired && this.getSpecified == (MethodInfo) null)
            tail = (IProtoSerializer) new DefaultValueDecorator((TypeModel) this.model, this.defaultValue, tail);
          if (this.memberType == this.model.MapType(typeof (Uri)))
            tail = (IProtoSerializer) new UriDecorator((TypeModel) this.model, tail);
        }
        if (member != (MemberInfo) null)
        {
          switch (member)
          {
            case PropertyInfo property:
              tail = (IProtoSerializer) new PropertyDecorator((TypeModel) this.model, this.parentType, property, tail);
              break;
            case FieldInfo field:
              tail = (IProtoSerializer) new FieldDecorator(this.parentType, field, tail);
              break;
            default:
              throw new InvalidOperationException();
          }
          if (this.getSpecified != (MethodInfo) null || this.setSpecified != (MethodInfo) null)
            tail = (IProtoSerializer) new MemberSpecifiedDecorator(this.getSpecified, this.setSpecified, tail);
        }
        return tail;
      }
      finally
      {
        this.model.ReleaseLock(opaqueToken);
      }
    }

    private static WireType GetIntWireType(DataFormat format, int width)
    {
      switch (format)
      {
        case DataFormat.Default:
        case DataFormat.TwosComplement:
          return WireType.Variant;
        case DataFormat.ZigZag:
          return WireType.SignedVariant;
        case DataFormat.FixedSize:
          return width != 32 ? WireType.Fixed64 : WireType.Fixed32;
        default:
          throw new InvalidOperationException();
      }
    }

    private static WireType GetDateTimeWireType(DataFormat format)
    {
      switch (format)
      {
        case DataFormat.Default:
        case DataFormat.WellKnown:
          return WireType.String;
        case DataFormat.FixedSize:
          return WireType.Fixed64;
        case DataFormat.Group:
          return WireType.StartGroup;
        default:
          throw new InvalidOperationException();
      }
    }

    internal static IProtoSerializer TryGetCoreSerializer(
      RuntimeTypeModel model,
      DataFormat dataFormat,
      Type type,
      out WireType defaultWireType,
      bool asReference,
      bool dynamicType,
      bool overwriteList,
      bool allowComplexTypes)
    {
      Type underlyingType = Helpers.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
        type = underlyingType;
      if (Helpers.IsEnum(type))
      {
        if (allowComplexTypes && model != null)
        {
          defaultWireType = WireType.Variant;
          return (IProtoSerializer) new EnumSerializer(type, model.GetEnumMap(type));
        }
        defaultWireType = WireType.None;
        return (IProtoSerializer) null;
      }
      switch (Helpers.GetTypeCode(type))
      {
        case ProtoTypeCode.Boolean:
          defaultWireType = WireType.Variant;
          return (IProtoSerializer) new BooleanSerializer((TypeModel) model);
        case ProtoTypeCode.Char:
          defaultWireType = WireType.Variant;
          return (IProtoSerializer) new CharSerializer((TypeModel) model);
        case ProtoTypeCode.SByte:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
          return (IProtoSerializer) new SByteSerializer((TypeModel) model);
        case ProtoTypeCode.Byte:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
          return (IProtoSerializer) new ByteSerializer((TypeModel) model);
        case ProtoTypeCode.Int16:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
          return (IProtoSerializer) new Int16Serializer((TypeModel) model);
        case ProtoTypeCode.UInt16:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
          return (IProtoSerializer) new UInt16Serializer((TypeModel) model);
        case ProtoTypeCode.Int32:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
          return (IProtoSerializer) new Int32Serializer((TypeModel) model);
        case ProtoTypeCode.UInt32:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
          return (IProtoSerializer) new UInt32Serializer((TypeModel) model);
        case ProtoTypeCode.Int64:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 64);
          return (IProtoSerializer) new Int64Serializer((TypeModel) model);
        case ProtoTypeCode.UInt64:
          defaultWireType = ValueMember.GetIntWireType(dataFormat, 64);
          return (IProtoSerializer) new UInt64Serializer((TypeModel) model);
        case ProtoTypeCode.Single:
          defaultWireType = WireType.Fixed32;
          return (IProtoSerializer) new SingleSerializer((TypeModel) model);
        case ProtoTypeCode.Double:
          defaultWireType = WireType.Fixed64;
          return (IProtoSerializer) new DoubleSerializer((TypeModel) model);
        case ProtoTypeCode.Decimal:
          defaultWireType = WireType.String;
          return (IProtoSerializer) new DecimalSerializer((TypeModel) model);
        case ProtoTypeCode.DateTime:
          defaultWireType = ValueMember.GetDateTimeWireType(dataFormat);
          return (IProtoSerializer) new DateTimeSerializer(dataFormat, (TypeModel) model);
        case ProtoTypeCode.String:
          defaultWireType = WireType.String;
          return asReference ? (IProtoSerializer) new NetObjectSerializer((TypeModel) model, model.MapType(typeof (string)), 0, BclHelpers.NetObjectOptions.AsReference) : (IProtoSerializer) new ProtoBuf.Serializers.StringSerializer((TypeModel) model);
        case ProtoTypeCode.TimeSpan:
          defaultWireType = ValueMember.GetDateTimeWireType(dataFormat);
          return (IProtoSerializer) new TimeSpanSerializer(dataFormat, (TypeModel) model);
        case ProtoTypeCode.ByteArray:
          defaultWireType = WireType.String;
          return (IProtoSerializer) new BlobSerializer((TypeModel) model, overwriteList);
        case ProtoTypeCode.Guid:
          defaultWireType = dataFormat == DataFormat.Group ? WireType.StartGroup : WireType.String;
          return (IProtoSerializer) new GuidSerializer((TypeModel) model);
        case ProtoTypeCode.Uri:
          defaultWireType = WireType.String;
          return (IProtoSerializer) new ProtoBuf.Serializers.StringSerializer((TypeModel) model);
        case ProtoTypeCode.Type:
          defaultWireType = WireType.String;
          return (IProtoSerializer) new SystemTypeSerializer((TypeModel) model);
        default:
          IProtoSerializer coreSerializer = model.AllowParseableTypes ? (IProtoSerializer) ParseableSerializer.TryCreate(type, (TypeModel) model) : (IProtoSerializer) null;
          if (coreSerializer != null)
          {
            defaultWireType = WireType.String;
            return coreSerializer;
          }
          if (allowComplexTypes && model != null)
          {
            int key = model.GetKey(type, false, true);
            MetaType proxy = (MetaType) null;
            if (key >= 0)
            {
              proxy = model[type];
              if (dataFormat == DataFormat.Default && proxy.IsGroup)
                dataFormat = DataFormat.Group;
            }
            if (asReference | dynamicType)
            {
              BclHelpers.NetObjectOptions options = BclHelpers.NetObjectOptions.None;
              if (asReference)
                options |= BclHelpers.NetObjectOptions.AsReference;
              if (dynamicType)
                options |= BclHelpers.NetObjectOptions.DynamicType;
              if (proxy != null)
              {
                if (asReference && Helpers.IsValueType(type))
                {
                  string str = "AsReference cannot be used with value-types";
                  throw new InvalidOperationException(!(type.Name == "KeyValuePair`2") ? str + ": " + type.FullName : str + "; please see https://stackoverflow.com/q/14436606/23354");
                }
                if (asReference && proxy.IsAutoTuple)
                  options |= BclHelpers.NetObjectOptions.LateSet;
                if (proxy.UseConstructor)
                  options |= BclHelpers.NetObjectOptions.UseConstructor;
              }
              defaultWireType = dataFormat == DataFormat.Group ? WireType.StartGroup : WireType.String;
              return (IProtoSerializer) new NetObjectSerializer((TypeModel) model, type, key, options);
            }
            if (key >= 0)
            {
              defaultWireType = dataFormat == DataFormat.Group ? WireType.StartGroup : WireType.String;
              return (IProtoSerializer) new SubItemSerializer(type, key, (ISerializerProxy) proxy, true);
            }
          }
          defaultWireType = WireType.None;
          return (IProtoSerializer) null;
      }
    }

    internal void SetName(string name)
    {
      if (!(name != this.name))
        return;
      this.ThrowIfFrozen();
      this.name = name;
    }

    /// <summary>
    /// Gets the logical name for this member in the schema (this is not critical for binary serialization, but may be used
    /// when inferring a schema).
    /// </summary>
    public string Name
    {
      get => !string.IsNullOrEmpty(this.name) ? this.name : this.originalMember.Name;
      set => this.SetName(value);
    }

    private bool HasFlag(byte flag) => ((int) this.flags & (int) flag) == (int) flag;

    private void SetFlag(byte flag, bool value, bool throwIfFrozen)
    {
      if (throwIfFrozen && this.HasFlag(flag) != value)
        this.ThrowIfFrozen();
      if (value)
        this.flags |= flag;
      else
        this.flags &= ~flag;
    }

    /// <summary>
    /// Should lists have extended support for null values? Note this makes the serialization less efficient.
    /// </summary>
    public bool SupportNull
    {
      get => this.HasFlag((byte) 16);
      set => this.SetFlag((byte) 16, value, true);
    }

    internal string GetSchemaTypeName(
      bool applyNetObjectProxy,
      ref RuntimeTypeModel.CommonImports imports)
    {
      Type effectiveType = this.ItemType;
      if (effectiveType == (Type) null)
        effectiveType = this.MemberType;
      return this.model.GetSchemaTypeName(effectiveType, this.DataFormat, applyNetObjectProxy && this.AsReference, applyNetObjectProxy && this.DynamicType, ref imports);
    }

    internal sealed class Comparer : IComparer, IComparer<ValueMember>
    {
      public static readonly ValueMember.Comparer Default = new ValueMember.Comparer();

      public int Compare(object x, object y) => this.Compare(x as ValueMember, y as ValueMember);

      public int Compare(ValueMember x, ValueMember y)
      {
        if (x == y)
          return 0;
        if (x == null)
          return -1;
        return y == null ? 1 : x.FieldNumber.CompareTo(y.FieldNumber);
      }
    }
  }
}
