// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPassMessageCommandAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class EPassMessageCommandAction : EPassMessageAction
  {
    private string command;
    private Dictionary<string, string> parameters;

    public EPassMessageCommandAction(
      string command,
      string description,
      Dictionary<string, string> parameters)
      : base(EPassMessageActionType.Command, description)
    {
      this.command = command;
      this.parameters = parameters;
    }

    public string Command => this.command;

    public Dictionary<string, string> Parameters => this.parameters;
  }
}
