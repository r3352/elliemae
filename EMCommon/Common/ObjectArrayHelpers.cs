// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ObjectArrayHelpers
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class ObjectArrayHelpers
  {
    private static IHashableContents[] NullObjects = (IHashableContents[]) new NullHashable[0];

    public static int GetAggregateHash(params object[] objects)
    {
      return ObjectArrayHelpers.GetAggregateHash(ObjectArrayHelpers.NullHash, objects);
    }

    private static int GetAggregateHash(int seed, params object[] objects)
    {
      return ((IEnumerable<object>) (objects ?? (object[]) ObjectArrayHelpers.NullObjects)).Aggregate<object, int>(seed, new Func<int, object, int>(ObjectArrayHelpers.AggregateHash));
    }

    public static int AggregateHash(int a, object toHash)
    {
      int num1 = ObjectArrayHelpers.RotateLeft(a, 19);
      int a1 = ObjectArrayHelpers.NullHash;
      if (toHash == null)
      {
        a1 = ObjectArrayHelpers.NullHash;
      }
      else
      {
        object obj;
        if ((obj = toHash) is bool)
        {
          a1 = (bool) obj ? 1195150630 : 514133915;
        }
        else
        {
          switch (toHash)
          {
            case int num7:
              a1 = num7;
              break;
            case string str:
              uint num2 = 0;
              for (int index = 0; index < str.Length; ++index)
              {
                char ch = str[index];
                uint num3 = num2 + (uint) ch;
                uint num4 = num3 + (num3 << 10);
                num2 = num4 ^ num4 >> 6;
              }
              uint num5 = num2 + (num2 << 3);
              uint num6 = num5 ^ num5 >> 11;
              a1 = (int) (num6 + (num6 << 15));
              break;
            case DateTime dateTime:
              long ticks = dateTime.Ticks;
              a1 = (int) ticks ^ (int) (ticks >> 32);
              break;
            case IHashableContents hashableContents:
              a1 = hashableContents.GetContentsHashCode();
              break;
            case IEnumerable<IHashableContents> hashableContentses:
              using (IEnumerator<IHashableContents> enumerator = hashableContentses.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  IHashableContents current = enumerator.Current;
                  a1 = ObjectArrayHelpers.AggregateHash(a1, (object) current);
                }
                break;
              }
            case IEnumerable<object> objects:
              using (IEnumerator<object> enumerator = objects.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  object current = enumerator.Current;
                  a1 = ObjectArrayHelpers.AggregateHash(a1, current);
                }
                break;
              }
            default:
              throw new NotSupportedException("Objects must implement IHashableContents or be a supported primitive");
          }
        }
      }
      return num1 ^ a1;
    }

    private static int RotateLeft(int value, int bits)
    {
      uint num1 = (uint) value;
      int num2 = bits % 32;
      return (int) num1 << num2 | (int) (num1 >> 32 - num2);
    }

    private static int NullHash => '_'.GetHashCode();
  }
}
