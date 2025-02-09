// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanDuplicationTemplateAclInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanDuplicationTemplateAclInfo
  {
    private int personaID;
    private string templateName;
    private bool viewAccess;

    public LoanDuplicationTemplateAclInfo(int personaID, string templateName, bool viewAccess)
    {
      this.personaID = personaID;
      this.templateName = templateName;
      this.viewAccess = viewAccess;
    }

    public int PersonaID
    {
      get => this.personaID;
      set => this.personaID = value;
    }

    public string TemplateName
    {
      get => this.templateName;
      set => this.templateName = value;
    }

    public bool ViewAccess
    {
      get => this.viewAccess;
      set => this.viewAccess = value;
    }
  }
}
