// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IApplicationScreen
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IApplicationScreen : IWin32Window, ISynchronizeInvoke
  {
    string Name { get; }

    void Enable();

    void Disable();

    void RefreshContents();

    string[] GetScreenNames(bool recursive);

    IApplicationScreen GetScreen(string name);

    IApplicationScreen GetCurrentScreen();

    bool SetCurrentScreen(string name);

    void RegisterService(object service, System.Type serviceType);

    T GetService<T>();

    void Navigate(string target, string type);

    void SetMenu(string menuName);

    ToolStripDropDown HelpDropDown { get; }

    bool IsModalDialogOpen();

    bool CloseModalDialogs();

    void DisplayHelp(string topicName);
  }
}
