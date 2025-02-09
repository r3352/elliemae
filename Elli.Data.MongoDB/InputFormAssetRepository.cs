// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.InputFormAssetRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.InputForm;
using Microsoft.CSharp.RuntimeBinder;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class InputFormAssetRepository : IInputFormAssetRepository
  {
    static InputFormAssetRepository() => MongoInitializer.Initialize();

    public string CreateInputFormAsset(object form)
    {
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__0, form, Constants.InputFormAssetIdField);
      object obj2 = target2((CallSite) p1, obj1, (object) null);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__3.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__3, form, Constants.InputFormAssetIdField);
      }
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__4.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__4, form, Constants.InputFormAssetVersionField);
      object obj4 = target4((CallSite) p5, obj3, (object) null);
      if (target3((CallSite) p6, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__7.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__7, form, Constants.InputFormAssetVersionField);
      }
      IMongoCollection<BsonDocument> collection = InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets);
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target5 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p9 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__9;
      Type type = typeof (BsonDocument);
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__8.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__8, form);
      object obj6 = target5((CallSite) p9, type, obj5);
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__10.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__10, obj6, 0, new BsonElement(Constants.InputFormAssetVersionField, (BsonValue) Constants.InputFormAssetVersion));
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__11 = CallSite<Action<CallSite, IMongoCollection<BsonDocument>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertOne", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__11.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__11, collection, obj6);
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__14 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (InputFormAssetRepository)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target6 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__14.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p14 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__14;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__13 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target7 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__13.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p13 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__13;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__12 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__12.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__1.\u003C\u003Ep__12, obj6, "_id");
      object obj8 = target7((CallSite) p13, obj7);
      return target6((CallSite) p14, obj8);
    }

    public object GetInputFormAsset(string assetId)
    {
      BsonDocument inputFormAsset = InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(assetId))).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      if (inputFormAsset != (BsonDocument) null)
      {
        inputFormAsset.InsertAt(0, new BsonElement(Constants.InputFormAssetIdField, (BsonValue) inputFormAsset["_id"].ToString()));
        inputFormAsset.Remove("_id");
        inputFormAsset.Remove(Constants.InputFormAssetVersionField);
      }
      return (object) inputFormAsset;
    }

    public void UpdateInputFormAsset(string assetId, object asset)
    {
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__0, asset, Constants.InputFormAssetIdField);
      object obj2 = target2((CallSite) p1, obj1, (object) null);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__3.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__3, asset, Constants.InputFormAssetIdField);
      }
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__4.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__4, asset, Constants.InputFormAssetVersionField);
      object obj4 = target4((CallSite) p5, obj3, (object) null);
      if (target3((CallSite) p6, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__7 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__7.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__7, asset, Constants.InputFormAssetVersionField);
      }
      IMongoCollection<BsonDocument> collection = InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets);
      FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(assetId));
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, Type, object, object> target5 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__9.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, Type, object, object>> p9 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__9;
      Type type = typeof (BsonDocument);
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__8.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__8, asset);
      object obj6 = target5((CallSite) p9, type, obj5);
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__10 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__10.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__10, obj6, 0, new BsonElement(Constants.InputFormAssetVersionField, (BsonValue) Constants.InputFormAssetVersion));
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__11 = CallSite<Func<CallSite, IMongoCollection<BsonDocument>, FilterDefinition<BsonDocument>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReplaceOne", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__11.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__3.\u003C\u003Ep__11, collection, filterDefinition, obj6);
    }

    public void DeleteInputFormAsset(string assetId)
    {
      if (InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets).DeleteMany(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(assetId))).DeletedCount == 0L)
        throw new ApplicationException("Selected asset could not be found");
    }

    public IList<object> GetInputFormAssetsByType(string type)
    {
      IFindFluent<BsonDocument, BsonDocument> source = InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) (Constants.Metadata + "." + Constants.InputFormTypeField), type));
      List<object> formAssetsByType = new List<object>();
      CancellationToken cancellationToken = new CancellationToken();
      foreach (object obj1 in source.ToListAsync<BsonDocument>(cancellationToken).Result)
      {
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Action<CallSite, object, int, BsonElement> target1 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Action<CallSite, object, int, BsonElement>> p3 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__3;
        object obj2 = obj1;
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, Type, string, object, BsonElement>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, string, object, BsonElement> target2 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, string, object, BsonElement>> p2 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__2;
        Type type1 = typeof (BsonElement);
        string formAssetIdField = Constants.InputFormAssetIdField;
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target3 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p1 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__0, obj1, "_id");
        object obj4 = target3((CallSite) p1, obj3);
        BsonElement bsonElement = target2((CallSite) p2, type1, formAssetIdField, obj4);
        target1((CallSite) p3, obj2, 0, bsonElement);
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__4.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__4, obj1, "_id");
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__5 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__5.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__5, obj1, Constants.InputFormAssetVersionField);
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__6 = CallSite<Action<CallSite, List<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__6.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__5.\u003C\u003Ep__6, formAssetsByType, obj1);
      }
      return (IList<object>) formAssetsByType;
    }

    public IList<object> GetInputFormAssets()
    {
      List<BsonDocument> result = InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets).Find<BsonDocument>((Expression<Func<BsonDocument, bool>>) (_ => true)).ToListAsync<BsonDocument>().Result;
      object obj = (object) new List<object>();
      foreach (BsonDocument bsonDocument in result)
      {
        bsonDocument.InsertAt(0, new BsonElement(Constants.InputFormAssetIdField, (BsonValue) bsonDocument["_id"].ToString()));
        bsonDocument.Remove("_id");
        bsonDocument.Remove(Constants.InputFormAssetVersionField);
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, BsonDocument>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__0, obj, bsonDocument);
      }
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, IList<object>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IList<object>), typeof (InputFormAssetRepository)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__1.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__6.\u003C\u003Ep__1, obj);
    }

    public IList<object> GetInputFormAssetsByAssetIds(IList<string> assetIdList)
    {
      IMongoCollection<BsonDocument> collection = InputFormAssetRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.InputFormAssets);
      ObjectId objectId = new ObjectId();
      FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.In<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) "_id", (IEnumerable<BsonValue>) assetIdList.Select(s => new
      {
        s = s,
        ret = ObjectId.TryParse(s, out objectId)
      }).Select(_param1 => !_param1.ret ? (BsonValue) null : (BsonValue) objectId).ToList<BsonValue>());
      IFindFluent<BsonDocument, BsonDocument> source = collection.Find<BsonDocument>(filter);
      object obj = (object) new List<object>();
      CancellationToken cancellationToken = new CancellationToken();
      foreach (BsonDocument bsonDocument in source.ToList<BsonDocument>(cancellationToken))
      {
        bsonDocument.InsertAt(0, new BsonElement(Constants.InputFormAssetIdField, (BsonValue) bsonDocument["_id"].ToString()));
        bsonDocument.Remove("_id");
        bsonDocument.Remove(Constants.InputFormAssetVersionField);
        // ISSUE: reference to a compiler-generated field
        if (InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, BsonDocument>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (InputFormAssetRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__0.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__0, obj, bsonDocument);
      }
      // ISSUE: reference to a compiler-generated field
      if (InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, IList<object>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IList<object>), typeof (InputFormAssetRepository)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__1.Target((CallSite) InputFormAssetRepository.\u003C\u003Eo__7.\u003C\u003Ep__1, obj);
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ActiveConnection();
  }
}
