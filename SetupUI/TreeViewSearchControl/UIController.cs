// Decompiled with JetBrains decompiler
// Type: TreeViewSearchControl.UIController
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Windows.Forms;

#nullable disable
namespace TreeViewSearchControl
{
  public class UIController
  {
    internal Form ParentForm;
    internal Keys Key;

    public bool SearchLabel { get; set; }

    public string SearchLabelText { get; set; } = "&Search";

    public int SearchTextboxWitdh { get; set; }

    public bool SearchButton { get; set; }

    public bool SearchButtonImage { get; set; } = true;

    public string SearchButtonText { get; set; } = "&Find";

    public bool ClearButton { get; set; }

    public bool ClearButtonImage { get; set; } = true;

    public string ClearButtonText { get; set; } = "&Clear";

    public bool SettingsButton { get; set; }

    public MessageMedium MessageMedium { get; set; } = MessageMedium.Tooltip;

    public bool RealTime { get; set; }

    public static UIController Custom => new UIController();

    public static UIController Everything
    {
      get
      {
        return new UIController()
        {
          SearchLabel = true,
          SearchButton = true,
          ClearButton = true,
          SettingsButton = true
        };
      }
    }

    private UIController()
    {
    }

    public void AssignFunctionKey(Form parentForm, Keys key = Keys.F3)
    {
      this.ParentForm = parentForm;
      this.Key = key;
    }
  }
}
