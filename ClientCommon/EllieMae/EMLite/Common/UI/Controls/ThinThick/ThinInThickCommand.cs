// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Controls.ThinThick.ThinInThickCommand
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.ThinThick;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Controls.ThinThick
{
  public class ThinInThickCommand
  {
    private CommandContext commandContext;
    private bool modified;

    public ThinInThickCommand(CommandContext commandContext)
    {
      this.commandContext = commandContext;
    }

    public ICommand CreateCommand(string commandType)
    {
      return new CommandFactory(this.commandContext).CreateCommand(commandType);
    }

    public void SomethingChanged() => this.modified = true;

    public bool IsSomethingChanged() => this.modified;

    public void ResetTheChange() => this.modified = false;
  }
}
