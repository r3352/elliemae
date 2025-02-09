// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.DeletedCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public class DeletedCollection : CollectionBase
  {
    public void Add(BusinessBase child) => this.List.Add((object) child);

    public void Remove(BusinessBase child) => this.List.Remove((object) child);

    public BusinessBase this[int index] => (BusinessBase) this.List[index];
  }
}
