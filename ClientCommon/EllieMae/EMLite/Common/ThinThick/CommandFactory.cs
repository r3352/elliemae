// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.CommandFactory
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick
{
  [ComVisible(true)]
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public class CommandFactory
  {
    private static List<Type> _allCommands = ((IEnumerable<Type>) typeof (CommandFactory).Assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => type != typeof (ICommand) && typeof (ICommand).IsAssignableFrom(type))).ToList<Type>();

    private CommandContext CurrentContext { get; set; }

    public CommandFactory(CommandContext context) => this.CurrentContext = context;

    public ICommand CreateCommand(string commandType) => this.CreateInstanceOf(commandType);

    public ICommand CreateInstanceOf(string commandType)
    {
      ICommand instanceOf = (ICommand) null;
      Type type = CommandFactory._allCommands.FirstOrDefault<Type>((Func<Type, bool>) (x => x.Name == commandType));
      if (type != (Type) null)
        instanceOf = (ICommand) Activator.CreateInstance(type, (object) this.CurrentContext);
      return instanceOf;
    }
  }
}
