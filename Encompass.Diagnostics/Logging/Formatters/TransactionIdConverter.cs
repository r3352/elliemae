// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Logging.Formatters.TransactionIdConverter
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace Encompass.Diagnostics.Logging.Formatters
{
  public class TransactionIdConverter : JsonConverter
  {
    private const byte ForwardSlashByte = 47;
    private const byte PlusByte = 43;
    private const byte EqualsByte = 61;
    private const char Underscore = '_';
    private const char Dash = '-';

    public override bool CanConvert(Type objectType) => typeof (TransactionId).Equals(objectType);

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      string str = reader.Value?.ToString();
      return string.IsNullOrEmpty(str) ? (object) null : (object) new TransactionId(TransactionIdConverter.DecodeFromBase64String(str));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      string base64String = TransactionIdConverter.EncodeToBase64String(((TransactionId) value).Id);
      writer.WriteValue(base64String);
    }

    public static string EncodeToBase64String(Guid guid)
    {
      Span<byte> span1 = stackalloc byte[16];
      Span<byte> utf8_1 = stackalloc byte[24];
      MemoryMarshal.TryWrite<Guid>(span1, ref guid);
      int utf8_2 = (int) Base64.EncodeToUtf8((ReadOnlySpan<byte>) span1, utf8_1, out int _, out int _);
      Span<char> span2 = stackalloc char[22];
      for (int index = 0; index < 22; ++index)
      {
        switch (utf8_1[index])
        {
          case 43:
            span2[index] = '_';
            break;
          case 47:
            span2[index] = '-';
            break;
          default:
            span2[index] = (char) utf8_1[index];
            break;
        }
      }
      return span2.ToString();
    }

    public static Guid DecodeFromBase64String(string str)
    {
      Span<char> array = (Span<char>) str.ToArray<char>();
      Span<byte> utf8 = stackalloc byte[24];
      for (int index = 0; index < 22; ++index)
      {
        switch (array[index])
        {
          case '-':
            utf8[index] = (byte) 47;
            break;
          case '_':
            utf8[index] = (byte) 43;
            break;
          default:
            utf8[index] = (byte) array[index];
            break;
        }
      }
      utf8[22] = (byte) 61;
      utf8[23] = (byte) 61;
      Span<byte> span = stackalloc byte[16];
      int num = (int) Base64.DecodeFromUtf8((ReadOnlySpan<byte>) utf8, span, out int _, out int _);
      Guid guid;
      MemoryMarshal.TryRead<Guid>((ReadOnlySpan<byte>) span, out guid);
      return guid;
    }
  }
}
