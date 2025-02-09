// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.ApplicationScreens
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Provides access to the different top-level screens which make up the Encompass application.
  /// </summary>
  public class ApplicationScreens : IEnumerable, ISynchronizeInvoke, IWin32Window
  {
    private Hashtable screens = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal ApplicationScreens()
    {
      this.createScreen(EncompassScreen.BorrowerContacts, (Screen) new BorrowerContactsScreen());
      this.createScreen(EncompassScreen.BusinessContacts, (Screen) new BusinessContactsScreen());
      this.createScreen(EncompassScreen.ePASS, (Screen) new EPassScreen());
      this.createScreen(EncompassScreen.Loans, (Screen) new LoansScreen());
      this.createScreen(EncompassScreen.Pipeline, (Screen) new PipelineScreen());
      foreach (EncompassScreen encompassScreen in Enum.GetValues(typeof (EncompassScreen)))
      {
        if (encompassScreen != EncompassScreen.Unknown && !this.screens.Contains((object) encompassScreen.ToString()))
          this.createScreen(encompassScreen, new Screen(encompassScreen));
      }
    }

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.Automation.Screen" /> object for the specified value of the
    /// <see cref="T:EllieMae.Encompass.Automation.EncompassScreen" /> enumeration.
    /// </summary>
    public Screen this[EncompassScreen screen] => (Screen) this.screens[(object) screen.ToString()];

    /// <summary>Gets or sets the current screen.</summary>
    /// <remarks>If an attempt is made to make a <see cref="T:EllieMae.Encompass.Automation.Screen" /> current to which
    /// the user does not have access, an exception will occur.</remarks>
    public Screen Current
    {
      get
      {
        return (Screen) this.screens[(object) Session.Application.GetService<IEncompassApplication>().GetCurrentActivity().ToString()];
      }
      set => value.MakeCurrent();
    }

    /// <summary>
    /// Provides a enumerator to iterate over the Encompass screens.
    /// </summary>
    /// <returns>Returns an enumerator for iterating over the screens.</returns>
    public IEnumerator GetEnumerator() => this.screens.Values.GetEnumerator();

    private void createScreen(EncompassScreen type, Screen instance)
    {
      this.screens[(object) type.ToString()] = (object) instance;
    }

    /// <summary>
    /// Indicates if the call must be marshalled to the UI thread of the screen.
    /// </summary>
    public bool InvokeRequired => Session.MainScreen.InvokeRequired;

    /// <summary>
    /// Begins the invocation of an asynchronous task on the UI thread of the screen.
    /// </summary>
    /// <param name="method">The delegate for the method to be executed</param>
    /// <param name="args">The arguments passed to the delegate.</param>
    /// <returns>Returns an IAsyncResult which can be used to retrieve the results of the call.</returns>
    public IAsyncResult BeginInvoke(Delegate method, object[] args)
    {
      return Session.MainScreen.BeginInvoke(method, args);
    }

    /// <summary>
    /// Ends the invocation of an asynchronous method started using the <see cref="M:EllieMae.Encompass.Automation.ApplicationScreens.BeginInvoke(System.Delegate,System.Object[])" /> method.
    /// </summary>
    /// <param name="result">The AsyncResult object returned by the call to BeginInvoke.</param>
    /// <returns>Returns the output of the original method.</returns>
    public object EndInvoke(IAsyncResult result) => Session.MainScreen.EndInvoke(result);

    /// <summary>
    /// Synchronously invokes a delegate on the UI thread of the application.
    /// </summary>
    /// <param name="method">The method to be invoked.</param>
    /// <param name="args">The aruments supplied to the method.</param>
    /// <returns>Returns the results of the method invocation.</returns>
    public object Invoke(Delegate method, object[] args) => Session.MainScreen.Invoke(method, args);

    /// <summary>
    /// Provides a Windows handle for use in displaying message boxes.
    /// </summary>
    IntPtr IWin32Window.Handle => Session.MainScreen.Handle;
  }
}
