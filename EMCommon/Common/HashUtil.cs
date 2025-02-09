// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.HashUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class HashUtil
  {
    private static MD5CryptoServiceProvider _md5;
    private static SHA1CryptoServiceProvider _sha1;

    private static MD5CryptoServiceProvider md5
    {
      get
      {
        if (HashUtil._md5 == null)
          HashUtil._md5 = new MD5CryptoServiceProvider();
        return HashUtil._md5;
      }
    }

    private static SHA1CryptoServiceProvider sha1
    {
      get
      {
        if (HashUtil._sha1 == null)
          HashUtil._sha1 = new SHA1CryptoServiceProvider();
        return HashUtil._sha1;
      }
    }

    public static byte[] ComputeHash(HashAlgorithm algorithm, byte[] buffer)
    {
      if (algorithm == HashAlgorithm.MD5)
        return HashUtil.md5.ComputeHash(buffer);
      return algorithm == HashAlgorithm.SHA1 ? HashUtil.sha1.ComputeHash(buffer) : (byte[]) null;
    }

    public static string ComputeHashB64(HashAlgorithm algorithm, byte[] buffer)
    {
      return Convert.ToBase64String(HashUtil.ComputeHash(algorithm, buffer));
    }

    public static string ComputeHashB64(string str)
    {
      return HashUtil.ComputeHashB64(HashAlgorithm.SHA1, Encoding.ASCII.GetBytes(str));
    }
  }
}
