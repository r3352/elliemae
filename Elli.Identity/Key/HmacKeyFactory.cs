// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Key.HmacKeyFactory
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System;
using System.Configuration;

#nullable disable
namespace Elli.Identity.Key
{
  public class HmacKeyFactory : ITokenKeyFactory
  {
    private byte[] _keyBytes;

    public HmacKeyFactory()
    {
      this._keyBytes = Convert.FromBase64String(ConfigurationManager.AppSettings["elli.Identity.TokenProvider:Hmac512Key"] ?? throw new Exception("Missing Hmac512Key entry in configuration file."));
    }

    public HmacKeyFactory(byte[] keyBytes) => this._keyBytes = keyBytes;

    public HmacKeyFactory(string keyString) => this._keyBytes = Convert.FromBase64String(keyString);

    public object GetSignatureKey() => (object) this._keyBytes;

    public object GetValidationKey() => (object) this._keyBytes;

    public KeyAlgorithm Algorithm => KeyAlgorithm.HS512;
  }
}
