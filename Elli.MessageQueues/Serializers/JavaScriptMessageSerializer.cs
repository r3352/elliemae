// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Serializers.JavaScriptMessageSerializer
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System.Text;
using System.Web.Script.Serialization;

#nullable disable
namespace Elli.MessageQueues.Serializers
{
  public class JavaScriptMessageSerializer : ISerializer
  {
    private readonly JavaScriptSerializer _serializer;
    private readonly UTF8Encoding _encoding;

    public JavaScriptMessageSerializer()
    {
      this._serializer = new JavaScriptSerializer();
      this._encoding = new UTF8Encoding();
    }

    public byte[] Serialize<T>(T message)
    {
      return (object) message == null ? (byte[]) null : this._encoding.GetBytes(this._serializer.Serialize((object) message));
    }

    public T Deserialize<T>(byte[] bytes)
    {
      return bytes == null ? default (T) : this._serializer.Deserialize<T>(this._encoding.GetString(bytes));
    }
  }
}
