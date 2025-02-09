// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MessageServices.Event.MembersContract
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.MessageServices.Event
{
  public class MembersContract
  {
    public IEnumerable<MemberContract> Added { get; set; }

    public IEnumerable<MemberContract> Updated { get; set; }

    public IEnumerable<MemberContract> Deleted { get; set; }
  }
}
