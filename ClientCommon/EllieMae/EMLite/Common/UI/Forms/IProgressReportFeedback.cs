// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Forms.IProgressReportFeedback
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Forms
{
  public interface IProgressReportFeedback : ISynchronizeInvoke, IWin32Window
  {
    int MaxValue { get; set; }

    bool Cancel { get; }

    bool Increment(int count);

    bool ResetCounter(int maxValue);

    void UpdateStatus(string newSetting);

    Form ParentForm { get; }

    DialogResult ShowDialog(System.Type formType, params object[] args);
  }
}
