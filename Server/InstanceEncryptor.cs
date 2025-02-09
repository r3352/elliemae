// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.InstanceEncryptor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Encompass.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class InstanceEncryptor
  {
    private Encoding Encoding { get; }

    private DataProtection DataProtection { get; }

    internal string InstanceName { get; }

    public string Decrypt(byte[] bytes)
    {
      return bytes != null && bytes.Length != 0 ? this.Encoding.GetString(this.DataProtection.Decrypt(bytes)) : "";
    }

    public byte[] Encrypt(string clearText)
    {
      return !string.IsNullOrEmpty(clearText) ? this.DataProtection.Encrypt(this.Encoding.GetBytes(clearText)) : new byte[0];
    }

    public InstanceEncryptor(string instanceName)
    {
      this.Encoding = Encoding.UTF8;
      this.DataProtection = new DataProtection((IKeyProvider) new BaseKeyProvider(this.Encoding.GetBytes(this.InstanceName = instanceName.ToUpperInvariant())));
    }
  }
}
