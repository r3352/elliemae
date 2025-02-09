// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.Functions.PartialUpdate
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using MongoDB.Bson;
using System;

#nullable disable
namespace Elli.Data.MongoDB.Functions
{
  public class PartialUpdate
  {
    public static BsonDocument MergeDocument(BsonDocument Source, BsonDocument Target)
    {
      foreach (BsonElement bsonElement in Source)
      {
        string name = bsonElement.Name;
        BsonValue bsonValue = bsonElement.Value;
        switch (bsonValue.GetType().ToString())
        {
          case "MongoDB.Bson.BsonArray":
            Target = PartialUpdate.updateArray(Target, name, bsonValue);
            continue;
          case "MongoDB.Bson.BsonDocument":
            Target = PartialUpdate.UpdateDocument(Target, name, bsonValue);
            continue;
          default:
            Target = PartialUpdate.UpdateValue(Target, name, bsonValue);
            continue;
        }
      }
      return Target;
    }

    private static BsonDocument updateArray(BsonDocument Target, string name, BsonValue value)
    {
      if (!Target.Contains(name))
      {
        Target.InsertAt(0, new BsonElement(name, (BsonValue) value.AsBsonArray));
      }
      else
      {
        int num = 0;
        foreach (BsonValue asBson1 in value.AsBsonArray)
        {
          switch (asBson1.GetType().ToString())
          {
            case "MongoDB.Bson.BsonArray":
              Target = PartialUpdate.updateArray(Target, name, value);
              break;
            case "MongoDB.Bson.BsonDocument":
              if (asBson1.AsBsonDocument.Contains(Constants.Id))
              {
                string str = asBson1.AsBsonDocument[Constants.Id].ToString();
                bool result = false;
                if (asBson1.AsBsonDocument.Contains(Constants.EntityDeleted))
                {
                  bool.TryParse(asBson1.AsBsonDocument[Constants.EntityDeleted].ToString(), out result);
                  if (result && Target.Contains(name) && Target[name].GetType().ToString() == "MongoDB.Bson.BsonArray")
                  {
                    int index = 0;
                    foreach (BsonValue asBson2 in Target.DeepClone().AsBsonDocument[name].AsBsonArray)
                    {
                      if (asBson2.AsBsonDocument.Contains(Constants.Id) && asBson2[Constants.Id].ToString() == str)
                      {
                        Target[name].AsBsonArray.Remove(Target[name][index]);
                        break;
                      }
                      ++index;
                    }
                  }
                }
                if (!result && Target.Contains(name) && Target[name].GetType().ToString() == "MongoDB.Bson.BsonArray")
                {
                  int index = 0;
                  BsonDocument asBsonDocument = Target.DeepClone().AsBsonDocument;
                  bool flag = false;
                  foreach (BsonValue asBson3 in asBsonDocument[name].AsBsonArray)
                  {
                    if (asBson3.AsBsonDocument.Contains(Constants.Id) && asBson3[Constants.Id].ToString() == str)
                    {
                      Target[name][index] = (BsonValue) PartialUpdate.MergeDocument(asBson1.AsBsonDocument, asBsonDocument[name][index].AsBsonDocument);
                      flag = true;
                      break;
                    }
                    ++index;
                  }
                  if (!flag)
                  {
                    Target[name].AsBsonArray.Add((BsonValue) asBson1.AsBsonDocument);
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            default:
              Target = PartialUpdate.UpdateValue(Target, name, value);
              break;
          }
          ++num;
        }
      }
      return Target;
    }

    private static BsonDocument UpdateDocument(BsonDocument Target, string name, BsonValue value)
    {
      BsonDocument asBsonDocument = value.AsBsonDocument;
      if (Target.Contains(name))
      {
        BsonValue bsonValue = Target[name];
        if (!(bsonValue.GetType().ToString() == "MongoDB.Bson.BsonDocument"))
          throw new ApplicationException("The value of key '" + name + "' does not match with the type in the existing record.");
        Target[name] = PartialUpdate.MergeDocument(asBsonDocument, bsonValue.AsBsonDocument).AsBsonValue;
      }
      else
        Target.InsertAt(0, new BsonElement(name, value));
      return Target;
    }

    private static BsonDocument UpdateValue(BsonDocument Target, string name, BsonValue value)
    {
      if (Target.Contains(name))
      {
        if (string.IsNullOrEmpty(value.ToString()))
          Target.Remove(name);
        else if (!string.IsNullOrEmpty(value.ToString()))
          Target[name] = value;
      }
      else if (!string.IsNullOrEmpty(value.ToString()))
        Target.InsertAt(0, new BsonElement(name, value));
      return Target;
    }

    private static class BsonValueType
    {
      public const string BsonArray = "MongoDB.Bson.BsonArray";
      public const string BsonDocument = "MongoDB.Bson.BsonDocument";
    }
  }
}
