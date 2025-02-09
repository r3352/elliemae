// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.ThinThick.TradingScreens.Interfaces.ITradingList
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.ThinThick.Requests.Trading;
using EllieMae.EMLite.Common.ThinThick.Responses;

#nullable disable
namespace EllieMae.EMLite.MainUI.ThinThick.TradingScreens.Interfaces
{
  public interface ITradingList : ITradingScreen
  {
    int[] Ids { get; set; }

    void SetIds(int[] ids);

    void SetSelectedPipelineView(string xml);

    bool IsCurrentViewSet { get; }

    TableLayout Layout { get; }

    int ItemCount { get; }

    CurrentOrArchived CurrentOrArchived { get; set; }

    OpSimpleResponse ExportToExcel(bool exportAll, bool print);
  }
}
