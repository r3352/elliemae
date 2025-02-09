// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.Screen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Provides the base class for all top-level screens within the Encompass application.
  /// </summary>
  /// <remarks>
  /// Many of the screens in Encompass provide screen-specific functionality which can be accessed
  /// by casting the screen to the appropriat derived type, such as the <see cref="T:EllieMae.Encompass.Automation.PipelineScreen" />.
  /// To access a Screen object, use the <see cref="P:EllieMae.Encompass.Automation.EncompassApplication.Screens" /> property.
  /// </remarks>
  public class Screen
  {
    private EncompassScreen screenType;

    internal Screen(EncompassScreen screenType) => this.screenType = screenType;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.Automation.EncompassScreen" /> enumeration value for this screen.
    /// </summary>
    public EncompassScreen ScreenType => this.screenType;

    /// <summary>
    /// Makes this screen the current;y visible screen in the Encompass UI.
    /// </summary>
    /// <remarks>This screen must be available in order to be made current. If the user
    /// does not have access to this screen or the screen is otherwise unavailable,
    /// an exception will occur.</remarks>
    public void MakeCurrent()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        Session.Application.GetService<IEncompassApplication>().SetCurrentActivity((EncompassActivity) this.screenType);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    /// <summary>
    /// Indicates if this screen is the currently active screen in the Encompass UI.
    /// </summary>
    /// <returns>Returns <c>true</c> is the screen is active, <c>false</c> otherwise.</returns>
    public bool IsCurrent()
    {
      return Session.Application.GetService<IEncompassApplication>().GetCurrentActivity().ToString() == this.screenType.ToString();
    }
  }
}
