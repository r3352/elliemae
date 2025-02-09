// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineEntityCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineEntityCollection : IEnumerable<DocEngineEntity>, IEnumerable
  {
    private List<DocEngineEntity> entities = new List<DocEngineEntity>();

    internal DocEngineEntityCollection()
    {
    }

    internal void Add(DocEngineEntity entity) => this.entities.Add(entity);

    public int Count => this.entities.Count;

    public DocEngineEntity this[int index] => this.entities[index];

    public IEnumerator<DocEngineEntity> GetEnumerator()
    {
      return (IEnumerator<DocEngineEntity>) this.entities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
