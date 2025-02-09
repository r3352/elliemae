﻿// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.ParseableSerializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class ParseableSerializer : IProtoSerializer
  {
    private readonly MethodInfo parse;

    public static ParseableSerializer TryCreate(Type type, TypeModel model)
    {
      MethodInfo parse = !(type == (Type) null) ? type.GetMethod("Parse", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[1]
      {
        model.MapType(typeof (string))
      }, (ParameterModifier[]) null) : throw new ArgumentNullException(nameof (type));
      if (!(parse != (MethodInfo) null) || !(parse.ReturnType == type))
        return (ParseableSerializer) null;
      if (Helpers.IsValueType(type))
      {
        MethodInfo customToString = ParseableSerializer.GetCustomToString(type);
        if (customToString == (MethodInfo) null || customToString.ReturnType != model.MapType(typeof (string)))
          return (ParseableSerializer) null;
      }
      return new ParseableSerializer(parse);
    }

    private static MethodInfo GetCustomToString(Type type)
    {
      return type.GetMethod("ToString", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public, (Binder) null, Helpers.EmptyTypes, (ParameterModifier[]) null);
    }

    private ParseableSerializer(MethodInfo parse) => this.parse = parse;

    public Type ExpectedType => this.parse.DeclaringType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public object Read(object value, ProtoReader source)
    {
      return this.parse.Invoke((object) null, new object[1]
      {
        (object) source.ReadString()
      });
    }

    public void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteString(value.ToString(), dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      Type expectedType = this.ExpectedType;
      if (Helpers.IsValueType(expectedType))
      {
        using (Local localWithValue = ctx.GetLocalWithValue(expectedType, valueFrom))
        {
          ctx.LoadAddress(localWithValue, expectedType);
          ctx.EmitCall(ParseableSerializer.GetCustomToString(expectedType));
        }
      }
      else
        ctx.EmitCall(ctx.MapType(typeof (object)).GetMethod("ToString"));
      ctx.EmitBasicWrite("WriteString", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadString", ctx.MapType(typeof (string)));
      ctx.EmitCall(this.parse);
    }
  }
}
