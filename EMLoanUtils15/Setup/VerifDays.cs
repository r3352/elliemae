// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.VerifDays
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  [Serializable]
  public class VerifDays : IXmlSerializable
  {
    private int vorRecvDays;
    private int voeRecvDays;
    private int vodRecvDays;
    private int volRecvDays;
    private int vomRecvDays;
    private int vorExpDays;
    private int voeExpDays;
    private int vodExpDays;
    private int volExpDays;
    private int vomExpDays;

    public VerifDays()
    {
    }

    public VerifDays(XmlSerializationInfo info)
    {
      this.vorRecvDays = info.GetInteger(nameof (vorRecvDays));
      this.voeRecvDays = info.GetInteger(nameof (voeRecvDays));
      this.vodRecvDays = info.GetInteger(nameof (vodRecvDays));
      this.volRecvDays = info.GetInteger(nameof (volRecvDays));
      this.vomRecvDays = info.GetInteger(nameof (vomRecvDays));
      this.vorExpDays = info.GetInteger(nameof (vorExpDays));
      this.voeExpDays = info.GetInteger(nameof (voeExpDays));
      this.vodExpDays = info.GetInteger(nameof (vodExpDays));
      this.volExpDays = info.GetInteger(nameof (volExpDays));
      this.vomExpDays = info.GetInteger(nameof (vomExpDays));
    }

    public int GetRecvDays(string verifType)
    {
      int recvDays = 0;
      switch (verifType)
      {
        case "VOR":
          recvDays = this.vorRecvDays;
          break;
        case "VOE":
          recvDays = this.voeRecvDays;
          break;
        case "VOD":
          recvDays = this.vodRecvDays;
          break;
        case "VOL":
          recvDays = this.volRecvDays;
          break;
        case "VOM":
          recvDays = this.vomRecvDays;
          break;
      }
      return recvDays;
    }

    public int GetExpDays(string verifType)
    {
      int expDays = 0;
      switch (verifType)
      {
        case "VOR":
          expDays = this.vorExpDays;
          break;
        case "VOE":
          expDays = this.voeExpDays;
          break;
        case "VOD":
          expDays = this.vodExpDays;
          break;
        case "VOL":
          expDays = this.volExpDays;
          break;
        case "VOM":
          expDays = this.vomExpDays;
          break;
      }
      return expDays;
    }

    public void SetRecvDays(string verifType, int days)
    {
      switch (verifType)
      {
        case "VOR":
          this.vorRecvDays = days;
          break;
        case "VOE":
          this.voeRecvDays = days;
          break;
        case "VOD":
          this.vodRecvDays = days;
          break;
        case "VOL":
          this.volRecvDays = days;
          break;
        case "VOM":
          this.vomRecvDays = days;
          break;
      }
    }

    public void SetExpDays(string verifType, int days)
    {
      switch (verifType)
      {
        case "VOR":
          this.vorExpDays = days;
          break;
        case "VOE":
          this.voeExpDays = days;
          break;
        case "VOD":
          this.vodExpDays = days;
          break;
        case "VOL":
          this.volExpDays = days;
          break;
        case "VOM":
          this.vomExpDays = days;
          break;
      }
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("vorRecvDays", (object) this.vorRecvDays);
      info.AddValue("voeRecvDays", (object) this.voeRecvDays);
      info.AddValue("vodRecvDays", (object) this.vodRecvDays);
      info.AddValue("volRecvDays", (object) this.volRecvDays);
      info.AddValue("vomRecvDays", (object) this.vomRecvDays);
      info.AddValue("vorExpDays", (object) this.vorExpDays);
      info.AddValue("voeExpDays", (object) this.voeExpDays);
      info.AddValue("vodExpDays", (object) this.vodExpDays);
      info.AddValue("volExpDays", (object) this.volExpDays);
      info.AddValue("vomExpDays", (object) this.vomExpDays);
    }
  }
}
