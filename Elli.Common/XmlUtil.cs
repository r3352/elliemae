// Decompiled with JetBrains decompiler
// Type: Elli.Common.XmlUtil
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Xml.Linq;

#nullable disable
namespace Elli.Common
{
  public static class XmlUtil
  {
    public static void AddElement(XElement parent, XElement child)
    {
      if (!child.HasAttributes && !child.HasElements)
        return;
      parent.Add((object) child);
    }

    public static void AddAttribute(XElement element, string name, short value)
    {
      if (value == (short) 0)
        return;
      element.Add((object) new XAttribute((XName) name, (object) value.ToString()));
    }

    public static void AddAttribute(XElement element, string name, Decimal value)
    {
      if (value == 0M)
        return;
      element.Add((object) new XAttribute((XName) name, (object) value.ToString("F2")));
    }

    public static void AddAttribute(XElement element, string name, Decimal value, byte scale)
    {
      if (value == 0M)
        return;
      element.Add((object) new XAttribute((XName) name, (object) value.ToString("F" + scale.ToString())));
    }

    public static void AddAttribute(XElement element, string name, int value)
    {
      if (value == 0)
        return;
      element.Add((object) new XAttribute((XName) name, (object) value));
    }

    public static void AddAttribute(XElement element, string name, long value)
    {
      if (value == 0L)
        return;
      element.Add((object) new XAttribute((XName) name, (object) value));
    }

    public static void AddAttribute(XElement element, string name, object value)
    {
      if (value == null)
        return;
      element.Add((object) new XAttribute((XName) name, value));
    }

    public static void AddAttribute(XElement element, string name, string value)
    {
      if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
        return;
      element.Add((object) new XAttribute((XName) name, (object) value));
    }

    public static void AddAttribute(
      XElement element,
      string name,
      DateTime? value,
      bool dateAndTime)
    {
      if (!value.HasValue)
        return;
      if (dateAndTime)
        element.Add((object) new XAttribute((XName) name, (object) string.Format("{0:yyyy-dd-MM HH:mm:ss}", (object) value)));
      else
        element.Add((object) new XAttribute((XName) name, (object) string.Format("{0:MM/dd/yyyy}", (object) value)));
    }

    public static string ReadString(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      return xattribute == null ? string.Empty : xattribute.Value;
    }

    public static DateTime? ReadDate(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return new DateTime?();
      DateTime result;
      return DateTime.TryParse(xattribute.Value, out result) ? new DateTime?(result) : new DateTime?();
    }

    public static Decimal ReadDecimal(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return 0M;
      Decimal result;
      if (!Decimal.TryParse(xattribute.Value, out result))
        result = 0M;
      return result;
    }

    public static long ReadLong(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return 0;
      long result;
      if (!long.TryParse(xattribute.Value, out result))
        result = 0L;
      return result;
    }

    public static int ReadInt(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return 0;
      int result;
      if (!int.TryParse(xattribute.Value, out result))
        result = 0;
      return result;
    }

    public static short ReadShort(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return 0;
      short result;
      if (!short.TryParse(xattribute.Value, out result))
        result = (short) 0;
      return result;
    }

    public static byte ReadByte(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return 0;
      byte result;
      if (!byte.TryParse(xattribute.Value, out result))
        result = (byte) 0;
      return result;
    }

    public static bool ReadNumericBool(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return false;
      byte result;
      if (!byte.TryParse(xattribute.Value, out result))
        result = (byte) 0;
      return result > (byte) 0;
    }

    public static bool? ReadNullableBool(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return new bool?();
      bool result;
      return !bool.TryParse(xattribute.Value, out result) ? new bool?() : new bool?(result);
    }

    public static Guid ReadGuid(XElement element, string attName)
    {
      XAttribute xattribute = element.Attribute((XName) attName);
      if (xattribute == null)
        return Guid.Empty;
      try
      {
        return new Guid(xattribute.Value);
      }
      catch
      {
        return Guid.Empty;
      }
    }
  }
}
