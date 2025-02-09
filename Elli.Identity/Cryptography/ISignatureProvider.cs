// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Cryptography.ISignatureProvider
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

#nullable disable
namespace Elli.Identity.Cryptography
{
  public interface ISignatureProvider
  {
    string GenerateSignature(string algorithm, string data);

    bool VerifySignature(string algorithm, string data, string signature);
  }
}
