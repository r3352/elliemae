// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineDataEntityCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineDataEntityCollection : IEnumerable<DocEngineDataEntity>, IEnumerable
  {
    private DocEngineMetadata metadata;
    private List<DocEngineDataEntity> entities = new List<DocEngineDataEntity>();

    internal DocEngineDataEntityCollection(DocEngineMetadata metadata) => this.metadata = metadata;

    public DocEngineMetadata Metadata => this.metadata;

    public void Add(DocEngineDataEntity entity) => this.entities.Add(entity);

    public void Remove(DocEngineDataEntity entity) => this.entities.Remove(entity);

    public DocEngineDataEntity Find(string encFieldID, string value, bool ignoreCase)
    {
      foreach (DocEngineDataEntity entity in this.entities)
      {
        if (string.Compare(entity.GetField(encFieldID), value, ignoreCase) == 0)
          return entity;
      }
      return (DocEngineDataEntity) null;
    }

    public int Count => this.entities.Count;

    public DocEngineDataEntity this[int index] => this.entities[index];

    public IEnumerator<DocEngineDataEntity> GetEnumerator()
    {
      return (IEnumerator<DocEngineDataEntity>) this.entities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
