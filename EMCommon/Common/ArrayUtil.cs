// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ArrayUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class ArrayUtil
  {
    private const string className = "ArrayUtil";

    public static Array Add(Array array, object value)
    {
      return new ArrayList((ICollection) array) { value }.ToArray(value.GetType());
    }

    public static Array AddRange(Array array, ICollection c, Type objType)
    {
      ArrayList arrayList = new ArrayList((ICollection) array);
      arrayList.AddRange(c);
      return arrayList.ToArray(objType);
    }

    public static Array Remove(Array array, object obj)
    {
      ArrayList arrayList = new ArrayList((ICollection) array);
      arrayList.Remove(obj);
      return arrayList.ToArray(obj.GetType());
    }

    public static Array Update(Array array, object obj)
    {
      return ArrayUtil.UpdateRange(array, (ICollection) new object[1]
      {
        obj
      }, obj.GetType());
    }

    public static Array UpdateRange(Array array, ICollection c, Type objType)
    {
      ArrayList arrayList = new ArrayList((ICollection) array);
      foreach (object obj in (IEnumerable) c)
      {
        int index = arrayList.IndexOf(obj);
        if (index != -1)
        {
          arrayList.RemoveAt(index);
          arrayList.Insert(index, obj);
        }
        else
          arrayList.Add(obj);
      }
      return arrayList.ToArray(objType);
    }
  }
}
