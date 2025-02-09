// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.Commands.CloseDialogCommand
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Operation.Location;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick.Commands
{
  [ComVisible(true)]
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public class CloseDialogCommand(CommandContext context) : CommandBase(context)
  {
    public override string Execute(string routine, string jsonParams)
    {
      IResponse resp = (IResponse) null;
      if (routine == "OpCommon.CloseDialog")
      {
        using (IOpCommon opCommon = OperationLocator.GetInstance().Resolve<IOpCommon>())
          resp = (IResponse) opCommon.CloseDialog(this.DeserializeRequest<OpCloseDialogRequest>(jsonParams));
      }
      return this.ConvertToJson(resp);
    }
  }
}
