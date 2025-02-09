// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FeeManagementPersonaInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FeeManagementPersonaInfo
  {
    private List<FeeManagementPersonaRights> personas = new List<FeeManagementPersonaRights>();

    public void AddPersonaRights(FeeManagementPersonaRights personaRights)
    {
      this.personas.Add(personaRights);
    }

    public FeeManagementPersonaRights GetPersonaRights(int personaID)
    {
      for (int index = 0; index < this.personas.Count; ++index)
      {
        if (this.personas[index].PersonaID == personaID)
          return this.personas[index];
      }
      return (FeeManagementPersonaRights) null;
    }

    public bool IsSectionEditable(FeeSectionEnum sectionEnum)
    {
      for (int index = 0; index < this.personas.Count; ++index)
      {
        bool flag = false;
        switch (sectionEnum)
        {
          case FeeSectionEnum.For700:
            flag = this.personas[index].Overwrite700;
            break;
          case FeeSectionEnum.For800:
            flag = this.personas[index].Overwrite800;
            break;
          case FeeSectionEnum.For900:
            flag = this.personas[index].Overwrite900;
            break;
          case FeeSectionEnum.For1000:
            flag = this.personas[index].Overwrite1000;
            break;
          case FeeSectionEnum.For1100:
            flag = this.personas[index].Overwrite1100;
            break;
          case FeeSectionEnum.For1200:
            flag = this.personas[index].Overwrite1200;
            break;
          case FeeSectionEnum.For1300:
            flag = this.personas[index].Overwrite1300;
            break;
          case FeeSectionEnum.ForPC:
            flag = this.personas[index].OverwritePC;
            break;
        }
        if (flag)
          return true;
      }
      return false;
    }

    public FeeManagementPersonaRights[] GetAllPersonaRights() => this.personas.ToArray();
  }
}
