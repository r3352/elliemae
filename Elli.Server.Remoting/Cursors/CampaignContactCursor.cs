// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.CampaignContactCursor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Server;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public class CampaignContactCursor : CursorBase
  {
    private const string className = "CampaignContactCursor";

    public CampaignContactCursor Initialize(ISession session, CampaignContactInfo[] contactInfos)
    {
      this.InitializeInternal(session);
      if (contactInfos == null || contactInfos.Length == 0)
        return this;
      for (int index = 0; index < contactInfos.Length; ++index)
        this.Items.Add((object) contactInfos[index]);
      return this;
    }

    public override object[] GetItems(int startIndex, int count)
    {
      TraceLog.WriteApi(nameof (CampaignContactCursor), nameof (GetItems), (object) startIndex, (object) count);
      return base.GetItems(startIndex, count);
    }

    public override object GetItem(int index) => this.GetItems(index, 1)[0];
  }
}
