// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Trading.EntityReference
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

#nullable disable
namespace EllieMae.EMLite.ClientServer.Trading
{
  public class EntityReference
  {
    public string EntityId { get; set; }

    public string EntityName { get; set; }

    public EntityRefTypeContract EntityType { get; set; }

    public string EntityUri { get; set; }

    public override string ToString()
    {
      return string.Format("Id:{0},Name:{1},Uri:{2},Type:{3}", (object) this.EntityId, (object) this.EntityName, (object) this.EntityUri, (object) this.EntityType);
    }
  }
}
