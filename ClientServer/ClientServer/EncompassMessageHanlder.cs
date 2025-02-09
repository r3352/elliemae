// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EncompassMessageHanlder
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EncompassMessageHanlder : IONRPRuleHandler
  {
    private IWin32Window owner;

    public EncompassMessageHanlder(IWin32Window owner) => this.owner = owner;

    public void MessageHandler(string msg)
    {
      int num = (int) Utils.Dialog(this.owner, msg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
  }
}
