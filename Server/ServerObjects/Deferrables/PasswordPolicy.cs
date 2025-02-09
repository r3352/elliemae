// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.PasswordPolicy
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Common.Security;
using Elli.MessageQueues;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables
{
  public class PasswordPolicy : IPasswordPolicy
  {
    private static Logger logger = new Logger();
    private static string ClassName = nameof (PasswordPolicy);

    public string Decrypt(string cypherText)
    {
      try
      {
        if (string.IsNullOrEmpty(cypherText))
          return "";
        cypherText = PasswordPolicy.ConvertHexToString(cypherText, Encoding.Unicode);
        return Rijndael256Util.Decrypt(cypherText);
      }
      catch (Exception ex)
      {
        PasswordPolicy.logger.Error(ex);
        TraceLog.WriteError(PasswordPolicy.ClassName, ex.ToString());
        throw;
      }
    }

    public string Encrypt(string plainText)
    {
      try
      {
        return string.IsNullOrEmpty(plainText) ? "" : PasswordPolicy.ConvertStringToHex(Rijndael256Util.Encrypt(plainText), Encoding.Unicode);
      }
      catch (Exception ex)
      {
        PasswordPolicy.logger.Error(ex);
        TraceLog.WriteError(PasswordPolicy.ClassName, ex.ToString());
        throw;
      }
    }

    private static string ConvertStringToHex(string input, Encoding encoding)
    {
      try
      {
        byte[] bytes = encoding.GetBytes(input);
        StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
        foreach (byte num in bytes)
          stringBuilder.AppendFormat("{0:X2}", (object) num);
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        PasswordPolicy.logger.Error(ex);
        TraceLog.WriteError(PasswordPolicy.ClassName, ex.ToString());
        throw;
      }
    }

    private static string ConvertHexToString(string hexInput, Encoding encoding)
    {
      try
      {
        int length = hexInput.Length;
        byte[] bytes = new byte[length / 2];
        for (int startIndex = 0; startIndex < length; startIndex += 2)
          bytes[startIndex / 2] = Convert.ToByte(hexInput.Substring(startIndex, 2), 16);
        return encoding.GetString(bytes);
      }
      catch (Exception ex)
      {
        PasswordPolicy.logger.Error(ex);
        TraceLog.WriteError(PasswordPolicy.ClassName, ex.ToString());
        throw;
      }
    }
  }
}
