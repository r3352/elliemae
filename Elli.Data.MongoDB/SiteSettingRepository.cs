// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.SiteSettingRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Settings;
using Microsoft.CSharp.RuntimeBinder;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class SiteSettingRepository : ISiteSettingRepository
  {
    static SiteSettingRepository() => MongoInitializer.Initialize();

    public bool CreateSiteSetting(object siteSettingData)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__1 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, Type, object, object> target1 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, Type, object, object>> p1 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__1;
        Type type1 = typeof (JObject);
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__0.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__0, siteSettingData);
        object obj2 = target1((CallSite) p1, type1, obj1);
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p4 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target3 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p3 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__2.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__2, obj2, Constants.SiteGuidField);
        object obj4 = target3((CallSite) p3, obj3, (object) null);
        if (target2((CallSite) p4, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target4 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p7 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__7;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target5 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__6.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p6 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__6;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj5 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__5.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__5, obj2, Constants.SiteIdField);
          object obj6 = target5((CallSite) p6, obj5, (object) null);
          if (target4((CallSite) p7, obj6))
          {
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target6 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__12.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p12 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__12;
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target7 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__11.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p11 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__11;
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__10 = CallSite<Func<CallSite, SiteSettingRepository, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "GetSiteSetting", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, SiteSettingRepository, object, object> target8 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__10.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, SiteSettingRepository, object, object>> p10 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__10;
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target9 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__9.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p9 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__9;
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj7 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__8.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__8, obj2, Constants.SiteIdField);
            object obj8 = target9((CallSite) p9, obj7);
            object obj9 = target8((CallSite) p10, this, obj8);
            object obj10 = target7((CallSite) p11, obj9, (object) null);
            if (target6((CallSite) p12, obj10))
              throw new ApplicationException("Invalid input, SiteId already exists in the database.");
          }
          IMongoCollection<BsonDocument> collection = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__14 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target10 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__14.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p14 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__14;
          Type type2 = typeof (BsonDocument);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__13.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__13, siteSettingData);
          object obj12 = target10((CallSite) p14, type2, obj11);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__15 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__15.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__15, obj12, 0, new BsonElement(Constants.SiteSettingsVersionField, (BsonValue) Constants.SiteSettingsVersion));
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__16 = CallSite<Action<CallSite, IMongoCollection<BsonDocument>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertOne", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__16.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__16, collection, obj12);
        }
        else
        {
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          string empty3 = string.Empty;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__19 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target11 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__19.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p19 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__19;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target12 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__18.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p18 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__18;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj13 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__17.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__17, obj2, Constants.SiteIdField);
          object obj14 = target12((CallSite) p18, obj13, (object) null);
          if (target11((CallSite) p19, obj14))
            throw new ApplicationException("Invalid input, you need to pass a SiteId.");
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__21 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (SiteSettingRepository)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string> target13 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__21.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string>> p21 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__21;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__20 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj15 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__20.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__20, obj2, Constants.SiteIdField);
          string siteId = target13((CallSite) p21, obj15);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__22 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__22 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__22.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__22, obj2, Constants.SiteIdField);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__24 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (SiteSettingRepository)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, string> target14 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__24.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, string>> p24 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__24;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__23 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj16 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__23.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__23, obj2, Constants.SiteGuidField);
          string str = target14((CallSite) p24, obj16);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__25 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__25 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__25.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__25, obj2, Constants.SiteGuidField);
          IMongoCollection<BsonDocument> collection = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__27 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__27 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Deserialize", (IEnumerable<Type>) new Type[1]
            {
              typeof (BsonDocument)
            }, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target15 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__27.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p27 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__27;
          Type type3 = typeof (BsonSerializer);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__26 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj17 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__26.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__26, obj2);
          object obj18 = target15((CallSite) p27, type3, obj17);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__28 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__28 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__28.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__28, obj18, 0, new BsonElement(Constants.SiteGuidField, (BsonValue) str));
          object siteSetting = this.GetSiteSetting(siteId);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__30 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__30 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target16 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__30.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p30 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__30;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__29 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj19 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__29.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__29, siteSetting, (object) null);
          if (target16((CallSite) p30, obj19))
          {
            BsonDocument document = new BsonDocument();
            document.InsertAt(0, new BsonElement(Constants.SiteIdField, (BsonValue) siteId));
            document.InsertAt(1, new BsonElement(Constants.SiteSettingsVersionField, (BsonValue) Constants.SiteSettingsVersion));
            BsonDocument bsonDocument = document;
            string siteSettings = Constants.SiteSettings;
            BsonArray bsonArray1 = new BsonArray();
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__31 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__31 = CallSite<Action<CallSite, BsonArray, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__31.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__31, bsonArray1, obj18);
            BsonArray bsonArray2 = bsonArray1;
            BsonElement element = new BsonElement(siteSettings, (BsonValue) bsonArray2);
            bsonDocument.InsertAt(2, element);
            collection.InsertOne(document);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__33 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__33 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, Type, object, object> target17 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__33.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, Type, object, object>> p33 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__33;
            Type type4 = typeof (BsonDocument);
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__32 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__32 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj20 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__32.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__32, siteSetting);
            object obj21 = target17((CallSite) p33, type4, obj20);
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__34 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, BsonDocument>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (BsonDocument), typeof (SiteSettingRepository)));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__34.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__34, obj21).Contains(Constants.SiteSettings))
            {
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__36 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__36 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AsBsonArray", typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, object> target18 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__36.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, object>> p36 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__36;
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__35 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__35 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj22 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__35.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__35, obj21, Constants.SiteSettings);
              object obj23 = target18((CallSite) p36, obj22);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__37 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__37 = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__37.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__37, obj23, obj18);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__38 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__38 = CallSite<Func<CallSite, object, string, object, object>>.Create(Binder.SetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj24 = SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__38.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__38, obj21, Constants.SiteSettings, obj23);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__39 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__39 = CallSite<Action<CallSite, IMongoCollection<BsonDocument>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "ReplaceOne", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__39.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__1.\u003C\u003Ep__39, collection, siteSetting, obj21);
            }
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public bool UpdateSiteSetting(object siteSettingData)
    {
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target2 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p1 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__0, siteSettingData, Constants.SiteSettingsVersionField);
      object obj2 = target2((CallSite) p1, obj1, (object) null);
      if (target1((CallSite) p2, obj2))
      {
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__3.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__3, siteSettingData, Constants.SiteSettingsVersionField);
      }
      string empty = string.Empty;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__6.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p6 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__6;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target4 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__5.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p5 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__5;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__4.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__4, siteSettingData, Constants.SiteIdField);
      object obj4 = target4((CallSite) p5, obj3, (object) null);
      if (target3((CallSite) p6, obj4))
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (SiteSettingRepository)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string> target5 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string>> p8 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj5 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__7.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__7, siteSettingData, Constants.SiteIdField);
      string str1 = target5((CallSite) p8, obj5);
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__11 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target6 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__11.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p11 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__11;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__10 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object, object> target7 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__10.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object, object>> p10 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__10;
      // ISSUE: reference to a compiler-generated field
      if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj6 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__9.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__9, siteSettingData, Constants.SiteGuidField);
      object obj7 = target7((CallSite) p10, obj6, (object) null);
      if (target6((CallSite) p11, obj7))
      {
        try
        {
          IMongoCollection<BsonDocument> collection = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings);
          FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, str1);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__13 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target8 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__13.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p13 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__13;
          Type type = typeof (BsonDocument);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__12.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__12, siteSettingData);
          object obj9 = target8((CallSite) p13, type, obj8);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__14 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__14.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__14, obj9, 0, new BsonElement(Constants.SiteSettingsVersionField, (BsonValue) Constants.SiteSettingsVersion));
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__15 = CallSite<Func<CallSite, IMongoCollection<BsonDocument>, FilterDefinition<BsonDocument>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ReplaceOne", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj10 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__15.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__15, collection, filterDefinition, obj9);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__18 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__18 = CallSite<Func<CallSite, object, bool>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (bool), typeof (SiteSettingRepository)));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target9 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__18.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p18 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__18;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__17 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__17 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, int, object> target10 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__17.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, int, object>> p17 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__17;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__16 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ModifiedCount", typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__16.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__16, obj10);
          object obj12 = target10((CallSite) p17, obj11, 1);
          return target9((CallSite) p18, obj12);
        }
        catch (Exception ex)
        {
          throw new ApplicationException(ex.Message);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__20 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__20 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (SiteSettingRepository)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target11 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__20.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p20 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__20;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__19 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__19 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj13 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__19.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__19, siteSettingData, Constants.SiteGuidField);
        string str2 = target11((CallSite) p20, obj13);
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__23 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__23 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target12 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__23.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p23 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__23;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__22 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__22 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target13 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__22.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p22 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__22;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__21 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__21 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj14 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__21.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__21, siteSettingData, Constants.SiteIdField);
        object obj15 = target13((CallSite) p22, obj14, (object) null);
        if (target12((CallSite) p23, obj15))
        {
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__24 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__24 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__24.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__24, siteSettingData, Constants.SiteIdField);
        }
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__27 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__27 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target14 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__27.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p27 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__27;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__26 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__26 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target15 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__26.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> p26 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__26;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__25 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__25 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj16 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__25.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__25, siteSettingData, Constants.SiteGuidField);
        object obj17 = target15((CallSite) p26, obj16, (object) null);
        if (target14((CallSite) p27, obj17))
        {
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__28 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__28 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__28.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__28, siteSettingData, Constants.SiteGuidField);
        }
        try
        {
          IMongoCollection<BsonDocument> collection = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings);
          FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, str1);
          BsonDocument replacement = collection.Find<BsonDocument>(filter).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
          if ((BsonDocument) null == replacement)
            throw new ApplicationException("No document found with SiteId '" + str1 + "'");
          BsonArray asBsonArray = replacement[Constants.SiteSettings].AsBsonArray;
          int num = 0;
          bool flag = false;
          foreach (BsonValue bsonValue1 in asBsonArray)
          {
            if (((BsonDocument) bsonValue1).Contains(Constants.SiteGuidField) && bsonValue1[Constants.SiteGuidField].AsString == str2)
            {
              flag = true;
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__30 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__30 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, Type, object, object> target16 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__30.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, Type, object, object>> p30 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__30;
              Type type1 = typeof (JObject);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__29 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__29 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj18 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__29.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__29, siteSettingData);
              object obj19 = target16((CallSite) p30, type1, obj18);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__32 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__32 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Deserialize", (IEnumerable<Type>) new Type[1]
                {
                  typeof (BsonDocument)
                }, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, Type, object, object> target17 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__32.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, Type, object, object>> p32 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__32;
              Type type2 = typeof (BsonSerializer);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__31 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__31 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj20 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__31.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__31, obj19);
              object obj21 = target17((CallSite) p32, type2, obj20);
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__33 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__33 = CallSite<Action<CallSite, object, int, BsonElement>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InsertAt", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__33.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__33, obj21, 0, new BsonElement(Constants.SiteGuidField, (BsonValue) str2));
              BsonArray bsonArray = asBsonArray;
              int index = num;
              // ISSUE: reference to a compiler-generated field
              if (SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__34 == null)
              {
                // ISSUE: reference to a compiler-generated field
                SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__34 = CallSite<Func<CallSite, object, BsonValue>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (BsonValue), typeof (SiteSettingRepository)));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              BsonValue bsonValue2 = SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__34.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__2.\u003C\u003Ep__34, obj21);
              bsonArray[index] = bsonValue2;
              break;
            }
            ++num;
          }
          if (!flag)
            throw new ApplicationException("Invalid Guid '" + str2 + "' for SiteId '" + str1 + "'.");
          collection.ReplaceOne(filter, replacement);
          return true;
        }
        catch (Exception ex)
        {
          throw new ApplicationException(ex.Message);
        }
      }
    }

    public string GetSiteSettingLibraryId(string siteId)
    {
      try
      {
        if (string.IsNullOrEmpty(siteId))
          throw new ApplicationException("Invalid input, you need to pass a SiteId.");
        BsonDocument bsonDocument = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if (!(bsonDocument != (BsonDocument) null))
          throw new ApplicationException("The selected site setting cannot be found.");
        return bsonDocument[Constants.LibraryIdField] != (BsonValue) null ? bsonDocument[Constants.LibraryIdField].ToString() : throw new ApplicationException("The requested SiteSetting doesn't have a LibraryId attached before.");
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    private object GetSiteSetting(string siteId)
    {
      if (string.IsNullOrEmpty(siteId))
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      BsonDocument siteSetting = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      if (siteSetting != (BsonDocument) null)
      {
        siteSetting.Remove("_id");
        siteSetting.Remove(Constants.SiteSettingsVersionField);
      }
      return (object) siteSetting;
    }

    public object GetSiteSetting(string siteId, string guid = null)
    {
      try
      {
        object siteSetting = this.GetSiteSetting(siteId);
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0, siteSetting, (object) null);
        if (target1((CallSite) p1, obj1))
          return (object) siteId;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target2 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p5 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, Type, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, Type, object> target3 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, Type, object>> p4 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__4;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target4 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> p3 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__2.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__2, siteSetting, Constants.SiteSettings);
        object obj3 = target4((CallSite) p3, obj2);
        Type type = typeof (BsonArray);
        object obj4 = target3((CallSite) p4, obj3, type);
        if (target2((CallSite) p5, obj4))
          return siteSetting;
        if (string.IsNullOrEmpty(guid))
        {
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return (object) (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__6.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__6, siteSetting, Constants.SiteSettings) as BsonArray).ToJson<BsonArray>(new JsonWriterSettings()
          {
            OutputMode = JsonOutputMode.Strict
          });
        }
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__14 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IEnumerable), typeof (SiteSettingRepository)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, IEnumerable> target5 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__14.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, IEnumerable>> p14 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__14;
        // ISSUE: reference to a compiler-generated field
        if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__7 == null)
        {
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__7.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__7, siteSetting, Constants.SiteSettings);
        foreach (object obj6 in target5((CallSite) p14, obj5))
        {
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__8 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "AsBsonDocument", typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__8.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__8, obj6);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target6 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__13.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p13 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__13;
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, BsonDocument>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (BsonDocument), typeof (SiteSettingRepository)));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__9.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__9, obj7).Contains(Constants.SiteGuidField);
          object obj8;
          if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__12 = CallSite<Func<CallSite, bool, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, bool, object, object> target7 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__12.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, bool, object, object>> p12 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__12;
            int num = flag ? 1 : 0;
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string, object> target8 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__11.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string, object>> p11 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__11;
            // ISSUE: reference to a compiler-generated field
            if (SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__10 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj9 = SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__10.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__10, obj7, Constants.SiteGuidField);
            string str = guid;
            object obj10 = target8((CallSite) p11, obj9, str);
            obj8 = target7((CallSite) p12, num != 0, obj10);
          }
          else
            obj8 = (object) flag;
          if (target6((CallSite) p13, obj8))
            return (object) (obj7 as BsonDocument).ToJson<BsonDocument>(new JsonWriterSettings()
            {
              OutputMode = JsonOutputMode.Strict
            });
        }
        return (object) guid;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public IList<object> GetSiteSettings()
    {
      try
      {
        List<BsonDocument> result = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings).Find<BsonDocument>((Expression<Func<BsonDocument, bool>>) (_ => true)).ToListAsync<BsonDocument>().Result;
        List<object> siteSettings = new List<object>();
        foreach (object obj in result)
        {
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0, obj, "_id");
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Action<CallSite, object, string>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1, obj, Constants.SiteSettingsVersionField);
          // ISSUE: reference to a compiler-generated field
          if (SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Action<CallSite, List<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (SiteSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2.Target((CallSite) SiteSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2, siteSettings, obj);
        }
        return (IList<object>) siteSettings;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    private bool DeleteSiteSetting(string siteId)
    {
      if (string.IsNullOrEmpty(siteId))
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      if (SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings).DeleteMany(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).DeletedCount == 0L)
        throw new ApplicationException("The selected site setting cannot be found.");
      return false;
    }

    public bool DeleteSiteSetting(string siteId, string guid = null)
    {
      try
      {
        if (string.IsNullOrEmpty(guid))
          return this.DeleteSiteSetting(siteId);
        IMongoCollection<BsonDocument> collection = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings);
        BsonDocument bsonDocument = collection.Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if (bsonDocument == (BsonDocument) null)
          throw new ApplicationException("No document found with SiteId '" + siteId + "'.");
        BsonArray asBsonArray = bsonDocument[Constants.SiteSettings].AsBsonArray;
        bool flag = false;
        int index = 0;
        foreach (BsonValue bsonValue in asBsonArray)
        {
          BsonDocument asBsonDocument = bsonValue.AsBsonDocument;
          if (asBsonDocument.Contains(Constants.SiteGuidField) && asBsonDocument[Constants.SiteGuidField] == (BsonValue) guid)
          {
            flag = true;
            break;
          }
          ++index;
        }
        if (!flag)
          throw new ApplicationException("Invalid Guid '" + guid + "' for SiteId '" + siteId + "'.");
        asBsonArray.RemoveAt(index);
        bsonDocument[Constants.SiteSettings] = (BsonValue) asBsonArray;
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
        collection.ReplaceOne(filter, BsonDocument.Parse(bsonDocument.ToString()));
        return true;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public object GetSiteSettingByKey(string siteId, string Key)
    {
      try
      {
        List<object> objectList = new List<object>();
        BsonDocument bsonDocument = SiteSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPOSiteSettings).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if ((BsonDocument) null == bsonDocument)
          throw new ApplicationException("No document found with SiteId '" + siteId + "'");
        foreach (BsonDocument asBson in bsonDocument[Constants.SiteSettings].AsBsonArray)
        {
          foreach (BsonElement element in asBson.Elements)
          {
            if (string.Compare(element.Name, Key, StringComparison.OrdinalIgnoreCase) == 0)
              return (object) element;
          }
        }
        BsonElement siteSettingByKey;
        return (object) siteSettingByKey;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ActiveConnection();
  }
}
