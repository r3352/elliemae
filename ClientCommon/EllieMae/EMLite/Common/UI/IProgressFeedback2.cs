// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IProgressFeedback2
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IProgressFeedback2 : ISynchronizeInvoke, IWin32Window, IServerProgressFeedback2
  {
    Form ParentForm { get; }

    DialogResult ShowDialog(System.Type formType, params object[] args);

    DialogResult MsgBox(string text, MessageBoxButtons buttons, MessageBoxIcon icon);
  }
}
