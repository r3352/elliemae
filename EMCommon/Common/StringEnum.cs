// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.StringEnum
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class StringEnum
  {
    private static readonly Hashtable StringValues = new Hashtable();

    public StringEnum(Type enumType)
    {
      this.EnumType = enumType.IsEnum ? enumType : throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", (object) enumType));
    }

    public string GetStringValue(string valueName)
    {
      string stringValue = (string) null;
      try
      {
        stringValue = StringEnum.GetStringValue((Enum) Enum.Parse(this.EnumType, valueName));
      }
      catch (Exception ex)
      {
      }
      return stringValue;
    }

    public Array GetStringValues()
    {
      ArrayList arrayList = new ArrayList();
      foreach (StringValueAttribute[] stringValueAttributeArray in ((IEnumerable<FieldInfo>) this.EnumType.GetFields()).Select<FieldInfo, StringValueAttribute[]>((Func<FieldInfo, StringValueAttribute[]>) (fi => fi.GetCustomAttributes(typeof (StringValueAttribute), false) as StringValueAttribute[])).Where<StringValueAttribute[]>((Func<StringValueAttribute[], bool>) (attrs => attrs != null && attrs.Length != 0)))
        arrayList.Add((object) stringValueAttributeArray[0].Value);
      return (Array) arrayList.ToArray();
    }

    public IList GetListValues()
    {
      Type underlyingType = Enum.GetUnderlyingType(this.EnumType);
      ArrayList listValues = new ArrayList();
      foreach (FieldInfo field in this.EnumType.GetFields())
      {
        if (field.GetCustomAttributes(typeof (StringValueAttribute), false) is StringValueAttribute[] customAttributes && customAttributes.Length != 0)
          listValues.Add((object) new DictionaryEntry(Convert.ChangeType(Enum.Parse(this.EnumType, field.Name), underlyingType), (object) customAttributes[0].Value));
      }
      return (IList) listValues;
    }

    public bool IsStringDefined(string stringValue)
    {
      return StringEnum.Parse(this.EnumType, stringValue) != null;
    }

    public bool IsStringDefined(string stringValue, bool ignoreCase)
    {
      return StringEnum.Parse(this.EnumType, stringValue, ignoreCase) != null;
    }

    public Type EnumType { get; private set; }

    public static string GetStringValue(Enum value)
    {
      string stringValue1 = (string) null;
      Type type = value.GetType();
      lock (StringEnum.StringValues.SyncRoot)
      {
        if (StringEnum.StringValues.ContainsKey((object) value))
        {
          if (StringEnum.StringValues[(object) value] is StringValueAttribute stringValue2)
            stringValue1 = stringValue2.Value;
        }
        else if (type.GetField(value.ToString()).GetCustomAttributes(typeof (StringValueAttribute), false) is StringValueAttribute[] customAttributes)
        {
          if (customAttributes.Length != 0)
          {
            StringEnum.StringValues.Add((object) value, (object) customAttributes[0]);
            stringValue1 = customAttributes[0].Value;
          }
        }
      }
      return stringValue1;
    }

    public static object Parse(Type type, string stringValue)
    {
      return StringEnum.Parse(type, stringValue, false);
    }

    public static object Parse(Type type, string stringValue, bool ignoreCase)
    {
      object obj = (object) null;
      string strA = (string) null;
      if (!type.IsEnum)
        throw new ArgumentException(string.Format("Supplied type must be an Enum.  Type was {0}", (object) type));
      foreach (FieldInfo field in type.GetFields())
      {
        if (field.GetCustomAttributes(typeof (StringValueAttribute), false) is StringValueAttribute[] customAttributes && customAttributes.Length != 0)
          strA = customAttributes[0].Value;
        if (string.Compare(strA, stringValue, ignoreCase) == 0)
        {
          obj = Enum.Parse(type, field.Name);
          break;
        }
      }
      return obj;
    }

    public static bool IsStringDefined(Type enumType, string stringValue)
    {
      return StringEnum.Parse(enumType, stringValue) != null;
    }

    public static bool IsStringDefined(Type enumType, string stringValue, bool ignoreCase)
    {
      return StringEnum.Parse(enumType, stringValue, ignoreCase) != null;
    }
  }
}
