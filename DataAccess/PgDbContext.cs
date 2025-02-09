// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.PgDbContext
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class PgDbContext : DbContext
  {
    private string _instanceId = "";
    private bool _RLSEnabled = ServerGlobals.PGRLSEnabled;
    private string _role = "";

    public override string InstanceId
    {
      get => !string.IsNullOrEmpty(this._instanceId) ? this._instanceId : "local";
      set => this._instanceId = value;
    }

    public bool RLSEnabled
    {
      get => this._RLSEnabled;
      set => this._RLSEnabled = value;
    }

    public string Role
    {
      get
      {
        if (string.IsNullOrEmpty(this._role))
          this._role = this.InstanceId;
        return this._role;
      }
      set => this._role = value;
    }
  }
}
