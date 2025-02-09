// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.MigrationScript.ScriptProcessor
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using MongoDB.Bson;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Elli.Data.MongoDB.MigrationScript
{
  public class ScriptProcessor
  {
    private IMigrationScript _migrationScript;
    private static readonly Version _currentDocumentVersion = ScriptProcessor.GetAssemblyVersion();
    private const string EmptyVersion = "0.0.0.0";

    public BsonDocument Migrate(BsonDocument sourceDocument)
    {
      if (sourceDocument == (BsonDocument) null)
        return (BsonDocument) null;
      BsonDocument bsonDocument = sourceDocument;
      Version result = new Version();
      if (sourceDocument.Any<BsonElement>((Func<BsonElement, bool>) (x => x.Name == "AssemblyVersion")))
        Version.TryParse(sourceDocument.GetElement("AssemblyVersion").Value.AsString, out result);
      if (result == ScriptProcessor._currentDocumentVersion)
        return bsonDocument;
      for (; result < ScriptProcessor._currentDocumentVersion; result = new Version(bsonDocument.GetElement("AssemblyVersion").Value.ToString()))
      {
        result.ToString();
        this._migrationScript = (IMigrationScript) new DefaultMigrationScript(ScriptProcessor._currentDocumentVersion);
        bsonDocument = this._migrationScript.Migrate(sourceDocument);
      }
      return bsonDocument;
    }

    private static Version GetAssemblyVersion()
    {
      return new Version(FileVersionInfo.GetVersionInfo(Assembly.Load("Elli.Domain").Location).FileVersion);
    }
  }
}
