// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.SelectedFileListEventArgs
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class SelectedFileListEventArgs : EventArgs
  {
    public List<FileSystemEntry> FSEntryList;

    public SelectedFileListEventArgs(List<FileSystemEntry> fsEntryList)
    {
      this.FSEntryList = fsEntryList;
    }
  }
}
