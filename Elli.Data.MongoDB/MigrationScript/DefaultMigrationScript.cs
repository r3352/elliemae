// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.MigrationScript.DefaultMigrationScript
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using MongoDB.Bson;
using System;
using System.Linq;

#nullable disable
namespace Elli.Data.MongoDB.MigrationScript
{
  public class DefaultMigrationScript : IMigrationScript
  {
    public string ScriptVersion { get; set; }

    public DefaultMigrationScript(Version version) => this.ScriptVersion = version.ToString();

    public BsonDocument Migrate(BsonDocument sourceDocument, Version version = null)
    {
      if (version != (Version) null)
        this.ScriptVersion = version.ToString();
      if (sourceDocument.Elements.Any<BsonElement>((Func<BsonElement, bool>) (x => x.Name == "AssemblyVersion")))
      {
        sourceDocument["AssemblyVersion"] = (BsonValue) this.ScriptVersion;
      }
      else
      {
        BsonElement element = new BsonElement("AssemblyVersion", (BsonValue) this.ScriptVersion);
        sourceDocument.Add(element);
      }
      return sourceDocument;
    }
  }
}
