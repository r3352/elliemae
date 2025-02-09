// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.TPORecoverCodeSettingRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Common.Security;
using Elli.Domain.Security;
using Microsoft.CSharp.RuntimeBinder;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class TPORecoverCodeSettingRepository : ITPORecoverCodeSettingRepository
  {
    static TPORecoverCodeSettingRepository() => MongoInitializer.Initialize();

    public string CreateTPORecoverCodeSettings(string tpoRecoverCodeSettingData)
    {
      try
      {
        IMongoCollection<BsonDocument> collection = TPORecoverCodeSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPORecoverCodeSettings);
        BsonDocument bsonDocument = BsonDocument.Parse(tpoRecoverCodeSettingData.ToString());
        BsonDocument document = bsonDocument;
        CancellationToken cancellationToken = new CancellationToken();
        collection.InsertOne(document, cancellationToken: cancellationToken);
        return Rijndael256Util.Encrypt(bsonDocument["_id"].ToString());
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ActiveConnection();

    public object ForgotTPOPasswordRecoverCodeSettings(
      string recoverCodeDecrypted,
      string recoverCode)
    {
      BsonDocument bsonDocument = TPORecoverCodeSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPORecoverCodeSettings).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(recoverCodeDecrypted))).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      return !((BsonDocument) null == bsonDocument) && bsonDocument.Contains("TPOLoginInfo") ? (object) bsonDocument : throw new ApplicationException("Password reset has expired, please reset.");
    }

    public void DeleteRecoverCodeSettingsData(string id)
    {
      if (TPORecoverCodeSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPORecoverCodeSettings).DeleteMany(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(id))).DeletedCount == 0L)
        throw new ApplicationException("Selected Recover Code Data could not be found");
    }

    private IList<object> GetRecoverCodeSettingData()
    {
      List<BsonDocument> result = TPORecoverCodeSettingRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.TPORecoverCodeSettings).Find<BsonDocument>((Expression<Func<BsonDocument, bool>>) (_ => true)).ToListAsync<BsonDocument>().Result;
      object obj = (object) new List<object>();
      foreach (BsonDocument bsonDocument in result)
      {
        // ISSUE: reference to a compiler-generated field
        if (TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Action<CallSite, object, BsonDocument>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__0, obj, bsonDocument);
      }
      // ISSUE: reference to a compiler-generated field
      if (TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, IList<object>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (IList<object>), typeof (TPORecoverCodeSettingRepository)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__5.\u003C\u003Ep__1, obj);
    }

    public void RemoveExpireOrSameUserEntries(string contactId)
    {
      DateTime now = DateTime.Now;
      foreach (object obj1 in (IEnumerable<object>) this.GetRecoverCodeSettingData())
      {
        // ISSUE: reference to a compiler-generated field
        if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__0, obj1, "TPOLoginInfo");
        if (target1((CallSite) p1, obj2))
        {
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__5 = CallSite<Func<CallSite, DateTime, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.GreaterThan, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, DateTime, object, object> target2 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, DateTime, object, object>> p5 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__5;
          DateTime dateTime = now;
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Parse", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target3 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p4 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__4;
          Type type1 = typeof (DateTime);
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target4 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p3 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__2, obj1, "Expiration");
          object obj4 = target4((CallSite) p3, obj3);
          object obj5 = target3((CallSite) p4, type1, obj4);
          object obj6 = target2((CallSite) p5, dateTime, obj5);
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__13 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__13.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__13, obj6))
          {
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__12 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__12 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, bool> target5 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__12.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, bool>> p12 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__12;
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__11 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__11 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.Or, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object, object> target6 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__11.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object, object>> p11 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__11;
            object obj7 = obj6;
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__10 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__10 = CallSite<Func<CallSite, int, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, int, object, object> target7 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__10.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, int, object, object>> p10 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__10;
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__9 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__9 = CallSite<Func<CallSite, Type, object, string, bool, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Compare", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, Type, object, string, bool, object> target8 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__9.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, Type, object, string, bool, object>> p9 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__9;
            Type type2 = typeof (string);
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__8 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target9 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__8.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p8 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__8;
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string, object> target10 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string, object>> p7 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj8 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__6.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__6, obj1, "TPOLoginInfo");
            object obj9 = target10((CallSite) p7, obj8, "ContactID");
            object obj10 = target9((CallSite) p8, obj9);
            string str = contactId;
            object obj11 = target8((CallSite) p9, type2, obj10, str, true);
            object obj12 = target7((CallSite) p10, 0, obj11);
            object obj13 = target6((CallSite) p11, obj7, obj12);
            if (!target5((CallSite) p12, obj13))
              continue;
          }
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__16 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__16 = CallSite<Action<CallSite, TPORecoverCodeSettingRepository, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "DeleteRecoverCodeSettingsData", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Action<CallSite, TPORecoverCodeSettingRepository, object> target11 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__16.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Action<CallSite, TPORecoverCodeSettingRepository, object>> p16 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__16;
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__15 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__15 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToString", (IEnumerable<Type>) null, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target12 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__15.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p15 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__15;
          // ISSUE: reference to a compiler-generated field
          if (TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__14 == null)
          {
            // ISSUE: reference to a compiler-generated field
            TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__14 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (TPORecoverCodeSettingRepository), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj14 = TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__14.Target((CallSite) TPORecoverCodeSettingRepository.\u003C\u003Eo__6.\u003C\u003Ep__14, obj1, "_id");
          object obj15 = target12((CallSite) p15, obj14);
          target11((CallSite) p16, this, obj15);
        }
      }
    }
  }
}
