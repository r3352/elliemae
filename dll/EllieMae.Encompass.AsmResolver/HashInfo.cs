// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.HashInfo
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Utils;
using System;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class HashInfo
  {
    private HashAlgorithm algorithm;
    private string digestValueB64;
    private byte[] digestValue;

    public HashAlgorithm Algorithm => this.algorithm;

    public string DigestValueB64
    {
      get
      {
        if (this.digestValueB64 == null)
          this.digestValueB64 = Convert.ToBase64String(this.digestValue);
        return this.digestValueB64;
      }
    }

    public byte[] DigestValue
    {
      get
      {
        if (this.digestValue == null)
          this.digestValue = Convert.FromBase64String(this.digestValueB64);
        return this.digestValue;
      }
    }

    public HashInfo(HashAlgorithm algorithm, string digestValueB64)
    {
      this.algorithm = algorithm;
      this.digestValueB64 = digestValueB64;
    }

    public HashInfo(HashAlgorithm algorithm, byte[] digestValue)
    {
      this.algorithm = algorithm;
      this.digestValue = digestValue;
    }

    public HashInfo(string algorithm, string digestValueB64)
    {
      this.setAlgorithm(algorithm);
      this.digestValueB64 = digestValueB64;
    }

    public HashInfo(string algorithm, byte[] digestValue)
    {
      this.setAlgorithm(algorithm);
      this.digestValue = digestValue;
    }

    private void setAlgorithm(string algorithm)
    {
      if (algorithm.ToLower().Contains("sha1"))
      {
        this.algorithm = HashAlgorithm.SHA1;
      }
      else
      {
        if (!algorithm.ToLower().Contains("md5"))
          throw new Exception(algorithm + ": unsupported hash algorithm");
        this.algorithm = HashAlgorithm.MD5;
      }
    }
  }
}
