// Decompiled with JetBrains decompiler
// Type: Elli.Common.EmList`1
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.Common
{
  [CollectionDataContract(ItemName = "Item", Namespace = "http://www.elliemae.com/encompass/platform")]
  public class EmList<T> : List<T>
  {
    public EmList(IEnumerable<T> enumerable)
      : base(enumerable)
    {
    }

    public EmList()
    {
    }

    public static EmList<T> ConvertToEmList(IEnumerable<T> typedList)
    {
      EmList<T> emList = new EmList<T>();
      foreach (T typed in typedList)
        emList.Add(typed);
      return emList;
    }
  }
}
