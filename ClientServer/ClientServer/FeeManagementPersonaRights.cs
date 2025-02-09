// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FeeManagementPersonaRights
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FeeManagementPersonaRights
  {
    private int personaID;
    private string personaName;
    private bool overwrite700;
    private bool overwrite800;
    private bool overwrite900;
    private bool overwrite1000;
    private bool overwrite1100;
    private bool overwrite1200;
    private bool overwrite1300;
    private bool overwritePC;

    public FeeManagementPersonaRights(
      int personaID,
      bool overwrite700,
      bool overwrite800,
      bool overwrite900,
      bool overwrite1000,
      bool overwrite1100,
      bool overwrite1200,
      bool overwrite1300,
      bool overwritePC)
      : this(personaID, (string) null, overwrite700, overwrite800, overwrite900, overwrite1000, overwrite1100, overwrite1200, overwrite1300, overwritePC)
    {
    }

    public FeeManagementPersonaRights(
      int personaID,
      string personaName,
      bool overwrite700,
      bool overwrite800,
      bool overwrite900,
      bool overwrite1000,
      bool overwrite1100,
      bool overwrite1200,
      bool overwrite1300,
      bool overwritePC)
    {
      this.personaID = personaID;
      this.personaName = personaName;
      this.overwrite700 = overwrite700;
      this.overwrite800 = overwrite800;
      this.overwrite900 = overwrite900;
      this.overwrite1000 = overwrite1000;
      this.overwrite1100 = overwrite1100;
      this.overwrite1200 = overwrite1200;
      this.overwrite1300 = overwrite1300;
      this.overwritePC = overwritePC;
    }

    public int PersonaID => this.personaID;

    public string PersonaName => this.personaName;

    public bool Overwrite700 => this.overwrite700;

    public bool Overwrite800 => this.overwrite800;

    public bool Overwrite900 => this.overwrite900;

    public bool Overwrite1000 => this.overwrite1000;

    public bool Overwrite1100 => this.overwrite1100;

    public bool Overwrite1200 => this.overwrite1200;

    public bool Overwrite1300 => this.overwrite1300;

    public bool OverwritePC => this.overwritePC;

    public void SetRights(FeeSectionEnum sectionEnum, bool enabled)
    {
      switch (sectionEnum)
      {
        case FeeSectionEnum.For700:
          this.overwrite700 = enabled;
          break;
        case FeeSectionEnum.For800:
          this.overwrite800 = enabled;
          break;
        case FeeSectionEnum.For900:
          this.overwrite900 = enabled;
          break;
        case FeeSectionEnum.For1000:
          this.overwrite1000 = enabled;
          break;
        case FeeSectionEnum.For1100:
          this.overwrite1100 = enabled;
          break;
        case FeeSectionEnum.For1200:
          this.overwrite1200 = enabled;
          break;
        case FeeSectionEnum.For1300:
          this.overwrite1300 = enabled;
          break;
        case FeeSectionEnum.ForPC:
          this.overwritePC = enabled;
          break;
      }
    }
  }
}
