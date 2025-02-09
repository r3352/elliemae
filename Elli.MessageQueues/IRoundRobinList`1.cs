// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.IRoundRobinList`1
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Collections.Generic;

#nullable disable
namespace Elli.MessageQueues
{
  public interface IRoundRobinList<T>
  {
    IEnumerable<T> All { get; }

    void ClearAll();

    void Add(T item);

    T Current { get; }

    T GetNext();
  }
}
