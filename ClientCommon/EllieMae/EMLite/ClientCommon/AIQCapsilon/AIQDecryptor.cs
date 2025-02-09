// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.AIQCapsilon.AIQDecryptor
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientCommon.AIQCapsilon
{
  public class AIQDecryptor
  {
    public string RsaDecryptWithPrivate(string encryptedLaunchConfigurationId)
    {
      byte[] array = Enumerable.Range(0, encryptedLaunchConfigurationId.Length).Where<int>((Func<int, bool>) (x => x % 2 == 0)).Select<int, byte>((Func<int, byte>) (x => Convert.ToByte(encryptedLaunchConfigurationId.Substring(x, 2), 16))).ToArray<byte>();
      Pkcs1Encoding pkcs1Encoding = new Pkcs1Encoding((IAsymmetricBlockCipher) new RsaEngine());
      AsymmetricKeyParameter parameters = this.ReadPrivateKey();
      pkcs1Encoding.Init(false, (ICipherParameters) parameters);
      return Encoding.UTF8.GetString(pkcs1Encoding.ProcessBlock(array, 0, array.Length));
    }

    public string RsaEncryptWithPrivate(string clearText)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(clearText);
      AsymmetricKeyParameter parameters = this.ReadPrivateKey();
      Pkcs1Encoding pkcs1Encoding = new Pkcs1Encoding((IAsymmetricBlockCipher) new RsaEngine());
      pkcs1Encoding.Init(true, (ICipherParameters) parameters);
      return string.Join("", ((IEnumerable<byte>) pkcs1Encoding.ProcessBlock(bytes, 0, bytes.Length)).Select<byte, string>((Func<byte, string>) (c => string.Format("{0:X2}", (object) Convert.ToInt32(c)))));
    }

    private AsymmetricKeyParameter ReadPrivateKey()
    {
      AsymmetricCipherKeyPair asymmetricCipherKeyPair = (AsymmetricCipherKeyPair) null;
      try
      {
        string companySetting = Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.Private.PemFile");
        if (string.IsNullOrEmpty(companySetting))
        {
          Tracing.Log(Tracing.SwEFolder, TraceLevel.Error, nameof (AIQDecryptor), string.Format("Private key file not found"));
          return (AsymmetricKeyParameter) null;
        }
        using (TextReader textReader = (TextReader) new StringReader(companySetting))
          asymmetricCipherKeyPair = (AsymmetricCipherKeyPair) new PemReader(textReader).ReadObject();
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwEFolder, TraceLevel.Error, nameof (AIQDecryptor), string.Format("Error in reading the privatekey file: {0}", (object) ex.Message));
        return (AsymmetricKeyParameter) null;
      }
      if (asymmetricCipherKeyPair == null)
      {
        Tracing.Log(Tracing.SwEFolder, TraceLevel.Error, nameof (AIQDecryptor), string.Format("Error in reading the privatekey file, returning null"));
        return (AsymmetricKeyParameter) null;
      }
      Tracing.Log(Tracing.SwEFolder, TraceLevel.Verbose, nameof (AIQDecryptor), string.Format("returning private key"));
      return asymmetricCipherKeyPair.Private;
    }

    public string PrivateKeyFolderLocation
    {
      get
      {
        string str = (string) null;
        try
        {
          string companySetting = Session.ConfigurationManager.GetCompanySetting("AIQConfig", "AIQ.Private.PemFile");
          if (!string.IsNullOrWhiteSpace(companySetting))
            str = "AIQCapsilon\\PrivateKey\\" + companySetting.Trim();
          if (!AssemblyResolver.IsSmartClient)
            return Path.Combine(SystemSettings.LocalAppDir, str);
        }
        catch (Exception ex)
        {
          Tracing.Log(Tracing.SwEFolder, TraceLevel.Error, nameof (AIQDecryptor), string.Format("Error in getting privatekey pem file.File path:{0} Exception: {1}", (object) str, (object) ex.Message));
        }
        return AssemblyResolver.DownloadAndCacheResourceFile(str);
      }
    }
  }
}
