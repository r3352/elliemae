// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PopulateGVItemEventArgs
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class PopulateGVItemEventArgs : EventArgs
  {
    private GVItem item;
    private object data;

    public PopulateGVItemEventArgs(GVItem item, object data)
    {
      this.item = item;
      this.data = data;
    }

    public GVItem ListItem => this.item;

    public object DataItem => this.data;
  }
}
