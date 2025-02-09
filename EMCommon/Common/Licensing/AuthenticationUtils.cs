// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.AuthenticationUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  public static class AuthenticationUtils
  {
    private const string authPublicKey = "BgIAAACkAABSU0ExAAQAAAEAAQCPmOpl18gvJ/D18ieeK6gNUgZmFsd7KlAb6eYmWnzMj6t6Tv6bGkZCCtoA43j4K7Zl1mPu/OzEq+mYywumtuhAoIeWwJdBYPXjECOYvSlOCtmiW7zUHL7F1mOaNpECgWzSuWqlpyCYx+bzqtUmvAw2tIa9RijNNnEFe5maSJV/sw==";
    private const string preauthPublicKey = "BgIAAACkAABSU0ExAAQAAAEAAQDDW0XL9JH7I+OsZkTWYtCzMAw0IJvcuCKuyXNY8YOwmudZefzbbOE1DMYn+u15giD5C1ypCoN63xzBsVwa2sgzNkkm4aZ3wlrElhSuxP/11DcKv/aoyu4veVtLpIv87IojK9rUEviyOyKYEf1c9j1/hf4RwCN/RYzIsDTL/kKinw==";

    public static string ComputeCRC(string path)
    {
      try
      {
        using (FileStream inputStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          using (SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider())
            return Convert.ToBase64String(cryptoServiceProvider.ComputeHash((Stream) inputStream));
        }
      }
      catch
      {
        return "";
      }
    }

    public static string ComputeCRC(string text, Encoding encoding)
    {
      try
      {
        using (SHA1CryptoServiceProvider cryptoServiceProvider = new SHA1CryptoServiceProvider())
          return Convert.ToBase64String(cryptoServiceProvider.ComputeHash(encoding.GetBytes(text)));
      }
      catch
      {
        return "";
      }
    }

    public static bool CompareAuthorizationKey(string authKey, string passPhrase)
    {
      using (SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider())
      {
        using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
        {
          cryptoServiceProvider.ImportCspBlob(Convert.FromBase64String("BgIAAACkAABSU0ExAAQAAAEAAQCPmOpl18gvJ/D18ieeK6gNUgZmFsd7KlAb6eYmWnzMj6t6Tv6bGkZCCtoA43j4K7Zl1mPu/OzEq+mYywumtuhAoIeWwJdBYPXjECOYvSlOCtmiW7zUHL7F1mOaNpECgWzSuWqlpyCYx+bzqtUmvAw2tIa9RijNNnEFe5maSJV/sw=="));
          return cryptoServiceProvider.VerifyData(Encoding.ASCII.GetBytes(passPhrase), (object) halg, Convert.FromBase64String(authKey));
        }
      }
    }

    public static bool IsPreauthorized(
      string moduleCrc,
      PreauthorizedModuleType moduleType,
      PreauthorizedModule[] preauthorizedModules)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(moduleCrc);
      using (SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider())
      {
        using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
        {
          cryptoServiceProvider.ImportCspBlob(Convert.FromBase64String("BgIAAACkAABSU0ExAAQAAAEAAQDDW0XL9JH7I+OsZkTWYtCzMAw0IJvcuCKuyXNY8YOwmudZefzbbOE1DMYn+u15giD5C1ypCoN63xzBsVwa2sgzNkkm4aZ3wlrElhSuxP/11DcKv/aoyu4veVtLpIv87IojK9rUEviyOyKYEf1c9j1/hf4RwCN/RYzIsDTL/kKinw=="));
          foreach (PreauthorizedModule preauthorizedModule in preauthorizedModules)
          {
            if (preauthorizedModule.ModuleType == moduleType && cryptoServiceProvider.VerifyData(bytes, (object) halg, Convert.FromBase64String(preauthorizedModule.AuthorizationKey)))
              return true;
          }
          return false;
        }
      }
    }
  }
}
