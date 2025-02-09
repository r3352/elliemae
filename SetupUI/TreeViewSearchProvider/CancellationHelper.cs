// Decompiled with JetBrains decompiler
// Type: TreeViewSearchProvider.CancellationHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Threading;

#nullable disable
namespace TreeViewSearchProvider
{
  internal class CancellationHelper
  {
    private static CancellationTokenSource _cts;
    private static CancellationToken _token;

    internal static CancellationToken Token
    {
      get
      {
        if (CancellationHelper._cts == null || CancellationHelper._cts.IsCancellationRequested)
        {
          CancellationHelper._cts = new CancellationTokenSource();
          CancellationHelper._token = CancellationHelper._cts.Token;
        }
        return CancellationHelper._token;
      }
    }

    internal static void CancelRequest()
    {
      if (CancellationHelper._cts == null || CancellationHelper._cts.IsCancellationRequested)
        return;
      CancellationHelper._cts.Cancel(true);
      CancellationHelper._cts.Dispose();
    }
  }
}
