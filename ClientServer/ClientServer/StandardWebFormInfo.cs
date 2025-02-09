// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.StandardWebFormInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class StandardWebFormInfo
  {
    private int formID;
    private bool access;
    private string formName;
    private string lastmodifiedby;

    public StandardWebFormInfo()
    {
    }

    public StandardWebFormInfo(int formID, bool access, string formName)
    {
      this.formID = formID;
      this.access = access;
      this.formName = formName;
    }

    public string FormName
    {
      get => this.formName;
      set => this.formName = value;
    }

    public bool Access
    {
      get => this.access;
      set => this.access = value;
    }

    public int FormID
    {
      get => this.formID;
      set => this.formID = value;
    }

    public string LastModifiedBy
    {
      get => this.lastmodifiedby;
      set => this.lastmodifiedby = value;
    }
  }
}
