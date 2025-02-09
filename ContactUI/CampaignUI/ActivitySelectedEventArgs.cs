// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ActivitySelectedEventArgs
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ActivitySelectedEventArgs : EventArgs
  {
    public bool IsActivitySelected;

    public ActivitySelectedEventArgs(bool isActivitySelected)
    {
      this.IsActivitySelected = isActivitySelected;
    }
  }
}
