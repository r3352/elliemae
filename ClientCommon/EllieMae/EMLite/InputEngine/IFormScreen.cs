// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IFormScreen
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.UI;
using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public interface IFormScreen : IRefreshContents
  {
    void RefreshContents(string id);

    void RefreshControl(string controlId);

    void RefreshAllControls(bool force);

    bool SetGoToFieldFocus(string fieldID, int count);

    void SelectAllFields();

    void DeselectAllFields();

    void RefreshToolTips();

    object GetAutomationFormObject();

    void ExecAction(string action);

    void Popup(string formName, string title, int width, int height);

    event EventHandler OnFieldChanged;

    void UpdateCurrentField();

    void Dispose();
  }
}
