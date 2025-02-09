// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.MongoInitializer
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Mortgage;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;

#nullable disable
namespace Elli.Data.MongoDB
{
  public static class MongoInitializer
  {
    private static readonly object LockObject = new object();

    static MongoInitializer()
    {
      lock (MongoInitializer.LockObject)
      {
        ConventionRegistry.Register("Elli.Core Defaults", (IConventionPack) new ConventionPack()
        {
          (IConvention) new IgnoreExtraElementsConvention(true),
          (IConvention) new IgnoreIfDefaultConvention(true),
          (IConvention) new EnumRepresentationConvention(BsonType.String)
        }, (Func<Type, bool>) (t => t.FullName.StartsWith("Elli")));
        BsonSerializer.RegisterIdGenerator(typeof (Guid), (IIdGenerator) CombGuidGenerator.Instance);
        BsonSerializer.RegisterIdGenerator(typeof (string), (IIdGenerator) StringObjectIdGenerator.Instance);
        BsonSerializer.RegisterSerializer(typeof (Decimal?), (IBsonSerializer) new CustomDecimalSerializer(DecimalPlace.Six));
        BsonSerializer.RegisterSerializer(typeof (DateTime), (IBsonSerializer) new DateTimeSerializer(DateTimeKind.Utc));
        BsonSerializer.RegisterGenericSerializerDefinition(typeof (NA<>), typeof (CustomNaSerializer<>));
      }
    }

    public static void Initialize()
    {
    }
  }
}
