// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.MapDecorator`3
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal class MapDecorator<TDictionary, TKey, TValue> : ProtoDecoratorBase where TDictionary : class, IDictionary<TKey, TValue>
  {
    private readonly Type concreteType;
    private readonly IProtoSerializer keyTail;
    private readonly int fieldNumber;
    private readonly WireType wireType;
    private static readonly MethodInfo indexerSet = MapDecorator<TDictionary, TKey, TValue>.GetIndexerSetter();
    private static readonly TKey DefaultKey = typeof (TKey) == typeof (string) ? (TKey) "" : default (TKey);
    private static readonly TValue DefaultValue = typeof (TValue) == typeof (string) ? (TValue) "" : default (TValue);

    internal MapDecorator(
      TypeModel model,
      Type concreteType,
      IProtoSerializer keyTail,
      IProtoSerializer valueTail,
      int fieldNumber,
      WireType wireType,
      WireType keyWireType,
      WireType valueWireType,
      bool overwriteList)
      : base((object) MapDecorator<TDictionary, TKey, TValue>.DefaultValue == null ? (IProtoSerializer) new TagDecorator(2, valueWireType, false, valueTail) : (IProtoSerializer) new DefaultValueDecorator(model, (object) MapDecorator<TDictionary, TKey, TValue>.DefaultValue, (IProtoSerializer) new TagDecorator(2, valueWireType, false, valueTail)))
    {
      this.wireType = wireType;
      this.keyTail = (IProtoSerializer) new DefaultValueDecorator(model, (object) MapDecorator<TDictionary, TKey, TValue>.DefaultKey, (IProtoSerializer) new TagDecorator(1, keyWireType, false, keyTail));
      this.fieldNumber = fieldNumber;
      Type type = concreteType;
      if ((object) type == null)
        type = typeof (TDictionary);
      this.concreteType = type;
      if (keyTail.RequiresOldValue)
        throw new InvalidOperationException("Key tail should not require the old value");
      if (!keyTail.ReturnsValue)
        throw new InvalidOperationException("Key tail should return a value");
      if (!valueTail.ReturnsValue)
        throw new InvalidOperationException("Value tail should return a value");
      this.AppendToCollection = !overwriteList;
    }

    private static MethodInfo GetIndexerSetter()
    {
      foreach (PropertyInfo property in typeof (TDictionary).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!(property.Name != "Item") && !(property.PropertyType != typeof (TValue)))
        {
          ParameterInfo[] indexParameters = property.GetIndexParameters();
          if (indexParameters != null && indexParameters.Length == 1 && !(indexParameters[0].ParameterType != typeof (TKey)))
          {
            MethodInfo setMethod = property.GetSetMethod(true);
            if (setMethod != (MethodInfo) null)
              return setMethod;
          }
        }
      }
      throw new InvalidOperationException("Unable to resolve indexer for map");
    }

    public override Type ExpectedType => typeof (TDictionary);

    public override bool ReturnsValue => true;

    public override bool RequiresOldValue => this.AppendToCollection;

    private bool AppendToCollection { get; }

    public override object Read(object untyped, ProtoReader source)
    {
      TDictionary dictionary = (this.AppendToCollection ? (TDictionary) untyped : default (TDictionary)) ?? (TDictionary) Activator.CreateInstance(this.concreteType);
      do
      {
        TKey key = MapDecorator<TDictionary, TKey, TValue>.DefaultKey;
        TValue obj = MapDecorator<TDictionary, TKey, TValue>.DefaultValue;
        SubItemToken token = ProtoReader.StartSubItem(source);
        int num;
        while ((num = source.ReadFieldHeader()) > 0)
        {
          switch (num)
          {
            case 1:
              key = (TKey) this.keyTail.Read((object) null, source);
              continue;
            case 2:
              obj = (TValue) this.Tail.Read(this.Tail.RequiresOldValue ? (object) obj : (object) null, source);
              continue;
            default:
              source.SkipField();
              continue;
          }
        }
        ProtoReader.EndSubItem(token, source);
        dictionary[key] = obj;
      }
      while (source.TryReadFieldHeader(this.fieldNumber));
      return (object) dictionary;
    }

    public override void Write(object untyped, ProtoWriter dest)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) untyped)
      {
        ProtoWriter.WriteFieldHeader(this.fieldNumber, this.wireType, dest);
        SubItemToken token = ProtoWriter.StartSubItem((object) null, dest);
        if ((object) keyValuePair.Key != null)
          this.keyTail.Write((object) keyValuePair.Key, dest);
        if ((object) keyValuePair.Value != null)
          this.Tail.Write((object) keyValuePair.Value, dest);
        ProtoWriter.EndSubItem(token, dest);
      }
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      Type type = typeof (KeyValuePair<TKey, TValue>);
      MethodInfo moveNext;
      MethodInfo current;
      MethodInfo enumeratorInfo = ListDecorator.GetEnumeratorInfo(ctx.Model, this.ExpectedType, type, out moveNext, out current);
      Type returnType = enumeratorInfo.ReturnType;
      MethodInfo getMethod1 = type.GetProperty("Key").GetGetMethod();
      MethodInfo getMethod2 = type.GetProperty("Value").GetGetMethod();
      using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
      {
        using (Local local1 = new Local(ctx, returnType))
        {
          using (Local local2 = new Local(ctx, typeof (SubItemToken)))
          {
            using (Local local3 = new Local(ctx, type))
            {
              ctx.LoadAddress(localWithValue, this.ExpectedType);
              ctx.EmitCall(enumeratorInfo, this.ExpectedType);
              ctx.StoreValue(local1);
              using (ctx.Using(local1))
              {
                CodeLabel label1 = ctx.DefineLabel();
                CodeLabel label2 = ctx.DefineLabel();
                ctx.Branch(label2, false);
                ctx.MarkLabel(label1);
                ctx.LoadAddress(local1, returnType);
                ctx.EmitCall(current, returnType);
                if (type != ctx.MapType(typeof (object)) && current.ReturnType == ctx.MapType(typeof (object)))
                  ctx.CastFromObject(type);
                ctx.StoreValue(local3);
                ctx.LoadValue(this.fieldNumber);
                ctx.LoadValue((int) this.wireType);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("WriteFieldHeader"));
                ctx.LoadNullRef();
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("StartSubItem"));
                ctx.StoreValue(local2);
                ctx.LoadAddress(local3, type);
                ctx.EmitCall(getMethod1, type);
                ctx.WriteNullCheckedTail(typeof (TKey), this.keyTail, (Local) null);
                ctx.LoadAddress(local3, type);
                ctx.EmitCall(getMethod2, type);
                ctx.WriteNullCheckedTail(typeof (TValue), this.Tail, (Local) null);
                ctx.LoadValue(local2);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("EndSubItem"));
                ctx.MarkLabel(label2);
                ctx.LoadAddress(local1, returnType);
                ctx.EmitCall(moveNext, returnType);
                ctx.BranchIfTrue(label1, false);
              }
            }
          }
        }
      }
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      using (Local local1 = this.AppendToCollection ? ctx.GetLocalWithValue(this.ExpectedType, valueFrom) : new Local(ctx, typeof (TDictionary)))
      {
        using (Local local2 = new Local(ctx, typeof (SubItemToken)))
        {
          using (Local local3 = new Local(ctx, typeof (TKey)))
          {
            using (Local local4 = new Local(ctx, typeof (TValue)))
            {
              using (Local local5 = new Local(ctx, ctx.MapType(typeof (int))))
              {
                if (!this.AppendToCollection)
                {
                  ctx.LoadNullRef();
                  ctx.StoreValue(local1);
                }
                if (this.concreteType != (Type) null)
                {
                  ctx.LoadValue(local1);
                  CodeLabel label = ctx.DefineLabel();
                  ctx.BranchIfTrue(label, true);
                  ctx.EmitCtor(this.concreteType);
                  ctx.StoreValue(local1);
                  ctx.MarkLabel(label);
                }
                CodeLabel label1 = ctx.DefineLabel();
                ctx.MarkLabel(label1);
                if (typeof (TKey) == typeof (string))
                {
                  ctx.LoadValue("");
                  ctx.StoreValue(local3);
                }
                else
                  ctx.InitLocal(typeof (TKey), local3);
                if (typeof (TValue) == typeof (string))
                {
                  ctx.LoadValue("");
                  ctx.StoreValue(local4);
                }
                else
                  ctx.InitLocal(typeof (TValue), local4);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("StartSubItem"));
                ctx.StoreValue(local2);
                CodeLabel label2 = ctx.DefineLabel();
                CodeLabel label3 = ctx.DefineLabel();
                ctx.Branch(label2, false);
                ctx.MarkLabel(label3);
                ctx.LoadValue(local5);
                CodeLabel label4 = ctx.DefineLabel();
                CodeLabel label5 = ctx.DefineLabel();
                CodeLabel label6 = ctx.DefineLabel();
                ctx.Switch(new CodeLabel[3]
                {
                  label4,
                  label5,
                  label6
                });
                ctx.MarkLabel(label4);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("SkipField"));
                ctx.Branch(label2, false);
                ctx.MarkLabel(label5);
                this.keyTail.EmitRead(ctx, (Local) null);
                ctx.StoreValue(local3);
                ctx.Branch(label2, false);
                ctx.MarkLabel(label6);
                this.Tail.EmitRead(ctx, this.Tail.RequiresOldValue ? local4 : (Local) null);
                ctx.StoreValue(local4);
                ctx.MarkLabel(label2);
                ctx.EmitBasicRead("ReadFieldHeader", ctx.MapType(typeof (int)));
                ctx.CopyValue();
                ctx.StoreValue(local5);
                ctx.LoadValue(0);
                ctx.BranchIfGreater(label3, false);
                ctx.LoadValue(local2);
                ctx.LoadReaderWriter();
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("EndSubItem"));
                ctx.LoadAddress(local1, this.ExpectedType);
                ctx.LoadValue(local3);
                ctx.LoadValue(local4);
                ctx.EmitCall(MapDecorator<TDictionary, TKey, TValue>.indexerSet);
                ctx.LoadReaderWriter();
                ctx.LoadValue(this.fieldNumber);
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("TryReadFieldHeader"));
                ctx.BranchIfTrue(label1, false);
                if (!this.ReturnsValue)
                  return;
                ctx.LoadValue(local1);
              }
            }
          }
        }
      }
    }
  }
}
