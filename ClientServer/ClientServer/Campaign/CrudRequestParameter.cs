// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CrudRequestParameter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CrudRequestParameter
  {
    public CrudAction CrudAction;
    public int[] ContactIds;

    public CrudRequestParameter(CrudAction crudAction, int[] contactIds)
    {
      this.CrudAction = crudAction;
      this.ContactIds = contactIds;
    }
  }
}
