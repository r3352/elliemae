// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.ApplicationScreens
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

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

    public Screen this[EncompassScreen screen] => (Screen) this.screens[(object) screen.ToString()];

    public Screen Current
    {
      get
      {
        return (Screen) this.screens[(object) Session.Application.GetService<IEncompassApplication>().GetCurrentActivity().ToString()];
      }
      set => value.MakeCurrent();
    }

    public IEnumerator GetEnumerator() => this.screens.Values.GetEnumerator();

    private void createScreen(EncompassScreen type, Screen instance)
    {
      this.screens[(object) type.ToString()] = (object) instance;
    }

    public bool InvokeRequired => ((ISynchronizeInvoke) Session.MainScreen).InvokeRequired;

    public IAsyncResult BeginInvoke(Delegate method, object[] args)
    {
      return ((ISynchronizeInvoke) Session.MainScreen).BeginInvoke(method, args);
    }

    public object EndInvoke(IAsyncResult result)
    {
      return ((ISynchronizeInvoke) Session.MainScreen).EndInvoke(result);
    }

    public object Invoke(Delegate method, object[] args)
    {
      return ((ISynchronizeInvoke) Session.MainScreen).Invoke(method, args);
    }

    IntPtr IWin32Window.Handle => ((IWin32Window) Session.MainScreen).Handle;
  }
}
