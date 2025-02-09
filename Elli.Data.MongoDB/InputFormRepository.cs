// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.InputFormRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.InputForm;
using Microsoft.CSharp.RuntimeBinder;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class InputFormRepository : IInputFormRepository
  {
    static InputFormRepository() => MongoInitializer.Initialize();

    public string CreateInputForm(object form)
    {
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__0, form, Constants.InputFormIdField);
      object obj2 = target2((CallSite) p1, obj1, (object) null);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__3.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__3, form, Constants.InputFormIdField);
      }
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__4.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__4, form, Constants.InputFormVersionField);
      object obj4 = target4((CallSite) p5, obj3, (object) null);
      if (target3((CallSite) p6, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__7.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__7, form, Constants.InputFormVersionField);
      }
      try
      {
        IMongoCollection<BsonDocument> collection = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target5 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p9 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__9;
        Type type = typeof (BsonDocument);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__8.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__8, form);
        object obj6 = target5((CallSite) p9, type, obj5);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__10.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__10, obj6, 0, new BsonElement(Constants.InputFormVersionField, (BsonValue) Constants.InputFormVersion));
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__11 = CallSite<Action<CallSite, IMongoCollection<BsonDocument>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertOne", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__11.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__11, collection, obj6);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (InputFormRepository)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target6 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__14.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p14 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__14;
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__13 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target7 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__13.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p13 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__13;
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__12 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__12.Target((CallSite) InputFormRepository.\u003C\u003Eo__1.\u003C\u003Ep__12, obj6, "_id");
        object obj8 = target7((CallSite) p13, obj7);
        return target6((CallSite) p14, obj8);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public void UpdateInputForm(string inputFormId, object form)
    {
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__0, form, Constants.InputFormIdField);
      object obj2 = target2((CallSite) p1, obj1, (object) null);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__3.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__3, form, Constants.InputFormIdField);
      }
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__4.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__4, form, Constants.InputFormVersionField);
      object obj4 = target4((CallSite) p5, obj3, (object) null);
      if (target3((CallSite) p6, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__7.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__7, form, Constants.InputFormVersionField);
      }
      try
      {
        IMongoCollection<BsonDocument> collection = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms);
        FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(inputFormId));
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target5 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p9 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__9;
        Type type = typeof (BsonDocument);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__8.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__8, form);
        object obj6 = target5((CallSite) p9, type, obj5);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__10.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__10, obj6, 0, new BsonElement(Constants.InputFormVersionField, (BsonValue) Constants.InputFormVersion));
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__11 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__11 = CallSite<Func<CallSite, IMongoCollection<BsonDocument>, FilterDefinition<BsonDocument>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReplaceOne", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj7 = InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__11.Target((CallSite) InputFormRepository.\u003C\u003Eo__2.\u003C\u003Ep__11, collection, filterDefinition, obj6);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public object GetInputForm(string inputFormId)
    {
      try
      {
        BsonDocument inputForm = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(inputFormId))).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if (inputForm != (BsonDocument) null)
        {
          inputForm.InsertAt(0, new BsonElement(Constants.InputFormIdField, (BsonValue) inputForm["_id"].ToString()));
          inputForm.Remove("_id");
          inputForm.Remove(Constants.InputFormVersionField);
        }
        return (object) inputForm;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public object GetInputFormByName(string inputFormName)
    {
      try
      {
        BsonDocument inputFormByName = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) (Constants.Metadata + "." + Constants.InputFormNameField), inputFormName)).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if (inputFormByName != (BsonDocument) null)
        {
          inputFormByName.InsertAt(0, new BsonElement(Constants.InputFormIdField, (BsonValue) inputFormByName["_id"].ToString()));
          inputFormByName.Remove("_id");
          inputFormByName.Remove(Constants.InputFormVersionField);
        }
        return (object) inputFormByName;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public IList<object> GetInputForms()
    {
      try
      {
        List<BsonDocument> result = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms).Find<BsonDocument>((Expression<Func<BsonDocument, bool>>) (_ => true)).ToListAsync<BsonDocument>().Result;
        List<object> inputForms = new List<object>();
        foreach (object obj1 in result)
        {
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, int, BsonElement> target1 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, int, BsonElement>> p3 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__3;
          object obj2 = obj1;
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, Type, string, object, BsonElement>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, string, object, BsonElement> target2 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, string, object, BsonElement>> p2 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__2;
          Type type = typeof (BsonElement);
          string inputFormIdField = Constants.InputFormIdField;
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target3 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p1 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__0, obj1, "_id");
          object obj4 = target3((CallSite) p1, obj3);
          BsonElement bsonElement = target2((CallSite) p2, type, inputFormIdField, obj4);
          target1((CallSite) p3, obj2, 0, bsonElement);
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__4.Target((CallSite) InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__4, obj1, "_id");
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__5.Target((CallSite) InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__5, obj1, Constants.InputFormVersionField);
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__6 = CallSite<Action<CallSite, List<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__6.Target((CallSite) InputFormRepository.\u003C\u003Eo__5.\u003C\u003Ep__6, inputForms, obj1);
        }
        return (IList<object>) inputForms;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public IList<object> GetInputForms(string assetId)
    {
      try
      {
        List<BsonDocument> result = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) (Constants.InputFormAssetField + "." + Constants.InputFormAssetIdField), assetId)).ToListAsync<BsonDocument>().Result;
        List<object> inputForms = new List<object>();
        foreach (object obj1 in result)
        {
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, object, int, BsonElement> target1 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, object, int, BsonElement>> p3 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__3;
          object obj2 = obj1;
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Func<CallSite, Type, string, object, BsonElement>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, string, object, BsonElement> target2 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, string, object, BsonElement>> p2 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__2;
          Type type = typeof (BsonElement);
          string inputFormIdField = Constants.InputFormIdField;
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target3 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p1 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__0, obj1, "_id");
          object obj4 = target3((CallSite) p1, obj3);
          BsonElement bsonElement = target2((CallSite) p2, type, inputFormIdField, obj4);
          target1((CallSite) p3, obj2, 0, bsonElement);
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__4.Target((CallSite) InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__4, obj1, "_id");
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__5.Target((CallSite) InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__5, obj1, Constants.InputFormVersionField);
          // ISSUE: reference to a compiler-generated field
          if (InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__6 = CallSite<Action<CallSite, List<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__6.Target((CallSite) InputFormRepository.\u003C\u003Eo__6.\u003C\u003Ep__6, inputForms, obj1);
        }
        return (IList<object>) inputForms;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public void DeleteInputForm(string inputFormId)
    {
      try
      {
        if (InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms).DeleteMany(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(inputFormId))).DeletedCount == 0L)
          throw new ApplicationException("The selected Input Form cannot be found.");
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public IList<string> GetInputFormAssetIdsByInputForm(string inputFormId)
    {
      BsonDocument bsonDocument = InputFormRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputForms).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(inputFormId))).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      List<string> assetIdsByInputForm = new List<string>();
      object obj1 = bsonDocument != (BsonDocument) null && bsonDocument[Constants.InputFormAssetField] != (BsonValue) null ? (object) bsonDocument[Constants.InputFormAssetField] : throw new ApplicationException(string.Format("An input form with Id {0} does not exist.", (object) inputFormId));
      // ISSUE: reference to a compiler-generated field
      if (InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (InputFormRepository)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj2 in InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__3.Target((CallSite) InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__3, obj1))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target = InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p1 = InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__0.Target((CallSite) InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__0, obj2, Constants.InputFormAssetIdField);
        object obj4 = target((CallSite) p1, obj3);
        // ISSUE: reference to a compiler-generated field
        if (InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__2 = CallSite<Action<CallSite, List<string>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (InputFormRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__2.Target((CallSite) InputFormRepository.\u003C\u003Eo__8.\u003C\u003Ep__2, assetIdsByInputForm, obj4);
      }
      return (IList<string>) assetIdsByInputForm;
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ActiveConnection();
  }
}
