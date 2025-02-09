// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.EMListBox
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  public class EMListBox : ListBoxEx
  {
    public event EMListBox.ElectronicFormEventHandler ElectronicFormEvent;

    protected virtual void OnElectronicFormEvent(string formName)
    {
      this.ElectronicFormEvent(formName);
    }

    protected override void OnSelectedIndicesChanged(IndexChangedEventArg e)
    {
      if (this.GetSelected(e.ChangedIndex))
      {
        string str = this.Items[e.ChangedIndex].ToString().Trim();
        if (str == string.Empty || str.StartsWith("----") || str == "Appraisal Request (electronic only)" || str == "Title Commitment Request (electronic only)")
        {
          this.SetSelected(e.ChangedIndex, false);
          switch (str)
          {
            case "Appraisal Request (electronic only)":
              this.OnElectronicFormEvent("Appraisal Request");
              return;
            case "Title Commitment Request (electronic only)":
              this.OnElectronicFormEvent("Title Commitment Request");
              return;
            default:
              return;
          }
        }
      }
      base.OnSelectedIndicesChanged(e);
    }

    public delegate void ElectronicFormEventHandler(string formName);
  }
}
