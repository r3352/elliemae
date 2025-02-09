// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.EPassScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>Represents the ePASS screen.</summary>
  /// <remarks>This screen is only available when a loan is open in the loan editor.</remarks>
  public class EPassScreen : Screen
  {
    internal EPassScreen()
      : base(EncompassScreen.ePASS)
    {
    }

    /// <summary>Navigates the ePASS browser to a particular URL.</summary>
    /// <param name="url">The destination URL.</param>
    public void Navigate(string url) => Session.Application.GetService<IEPass>().Navigate(url);

    /// <summary>Executes an ePASS signature.</summary>
    /// <param name="emnSignature">The Ellie Mae Network signature to be executed.</param>
    /// <remarks>Ellie Mae Network Signatures are semi-colon delimited strings that provide the Services window with
    /// instructions to perform a certain prescribed action. This function is provided for use only
    /// by customers who understand and need use of existing EMN signatures.</remarks>
    public void Execute(string emnSignature)
    {
      Session.Application.GetService<IEPass>().ProcessURL(emnSignature);
    }
  }
}
