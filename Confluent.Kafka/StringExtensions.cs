// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.StringExtensions
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Text;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Extension methods for the <see cref="T:System.String" /> class.
  /// </summary>
  internal static class StringExtensions
  {
    internal static Encoding ToEncoding(this string encodingName)
    {
      switch (encodingName.ToLower())
      {
        case "utf8":
          return Encoding.UTF8;
        case "utf7":
          return Encoding.UTF7;
        case "utf32":
          return Encoding.UTF32;
        case "ascii":
          return Encoding.ASCII;
        default:
          return Encoding.GetEncoding(encodingName);
      }
    }
  }
}
