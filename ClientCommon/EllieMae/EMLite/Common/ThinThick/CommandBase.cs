// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.CommandBase
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick
{
  [ComVisible(true)]
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public abstract class CommandBase : ICommand
  {
    protected CommandContext CurrentContext { get; private set; }

    public CommandBase()
    {
    }

    public CommandBase(CommandContext context)
      : this()
    {
      this.CurrentContext = context;
    }

    public abstract string Execute(string routine, string jsonParams);

    protected string ConvertToJson(IResponse resp)
    {
      return resp == null ? "{}" : new JavaScriptSerializer().Serialize((object) resp);
    }

    protected T DeserializeRequest<T>(string jsonParams)
    {
      IRequest request = (IRequest) (object) (!string.IsNullOrEmpty(jsonParams) ? new JavaScriptSerializer().Deserialize<T>(jsonParams) : Activator.CreateInstance<T>());
      request.CommandContext = this.CurrentContext;
      return (T) request;
    }
  }
}
