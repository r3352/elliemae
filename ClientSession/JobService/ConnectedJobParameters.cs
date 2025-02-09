// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.JobService.ConnectedJobParameters
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.JobService
{
  public class ConnectedJobParameters : IXmlSerializable
  {
    private const string keyString = "fj2jd%1Nd-ek2Jp;32BNS<##0dslX=d2kds'lgdmsi24mvsd";
    private string userId;
    private string password;
    private string serverUri;

    public ConnectedJobParameters(string userId, string password, string serverUri)
    {
      this.userId = userId;
      this.password = password;
      this.serverUri = serverUri;
    }

    public ConnectedJobParameters(XmlSerializationInfo info)
    {
      this.userId = info.GetString("uid");
      this.password = this.decrypt(info.GetString("pwd"));
      this.serverUri = info.GetString(nameof (serverUri));
    }

    public string UserID => this.userId;

    public string Password => this.password;

    public string ServerUri => this.serverUri;

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("uid", (object) this.userId);
      info.AddValue("pwd", (object) this.encrypt(this.password));
      info.AddValue("serverUri", (object) this.serverUri);
    }

    public override string ToString() => new XmlSerializer().Serialize((object) this);

    private string encrypt(string plainText)
    {
      byte[] bytes = Encoding.ASCII.GetBytes("fj2jd%1Nd-ek2Jp;32BNS<##0dslX=d2kds'lgdmsi24mvsd");
      byte[] numArray = new byte[Encoding.ASCII.GetByteCount(plainText) + 1];
      using (CryptoRandom cryptoRandom = new CryptoRandom())
        numArray[0] = (byte) ((uint) Encoding.ASCII.GetBytes("A")[0] + (uint) cryptoRandom.Next(25));
      Encoding.ASCII.GetBytes(plainText, 0, plainText.Length, numArray, 1);
      for (int index = 1; index < numArray.Length; ++index)
        numArray[index] = (byte) ((uint) numArray[index] ^ (uint) numArray[index - 1] ^ (uint) bytes[index % bytes.Length]);
      return Convert.ToBase64String(numArray);
    }

    private string decrypt(string cipher)
    {
      byte[] bytes1 = Encoding.ASCII.GetBytes("fj2jd%1Nd-ek2Jp;32BNS<##0dslX=d2kds'lgdmsi24mvsd");
      byte[] bytes2 = Convert.FromBase64String(cipher);
      for (int index = bytes2.Length - 1; index > 0; --index)
        bytes2[index] = (byte) ((uint) bytes2[index] ^ (uint) bytes2[index - 1] ^ (uint) bytes1[index % bytes1.Length]);
      return Encoding.ASCII.GetString(bytes2, 1, bytes2.Length - 1);
    }
  }
}
