// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LoanFieldSelection
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  internal class LoanFieldSelection
  {
    public static LoanFieldSelection.SelectionOption[] SortOrderOptions = new LoanFieldSelection.SelectionOption[3]
    {
      new LoanFieldSelection.SelectionOption("None", 0),
      new LoanFieldSelection.SelectionOption("Ascending", 1),
      new LoanFieldSelection.SelectionOption("Descending", 2)
    };
    public static LoanFieldSelection.SelectionOption[] SummaryTypeOptions = new LoanFieldSelection.SelectionOption[6]
    {
      new LoanFieldSelection.SelectionOption("None", 0),
      new LoanFieldSelection.SelectionOption("Page", 1),
      new LoanFieldSelection.SelectionOption("Group", 2),
      new LoanFieldSelection.SelectionOption("Total", 3),
      new LoanFieldSelection.SelectionOption("Average", 4),
      new LoanFieldSelection.SelectionOption("Count", 5)
    };
    public static LoanFieldSelection.SelectionOption[] SummaryTypeOptions2 = new LoanFieldSelection.SelectionOption[4]
    {
      new LoanFieldSelection.SelectionOption("None", 0),
      new LoanFieldSelection.SelectionOption("Total", 3),
      new LoanFieldSelection.SelectionOption("Average", 4),
      new LoanFieldSelection.SelectionOption("Count", 5)
    };
    public static LoanFieldSelection.SelectionOption[] SummaryTypeOptions3 = new LoanFieldSelection.SelectionOption[2]
    {
      new LoanFieldSelection.SelectionOption("None", 0),
      new LoanFieldSelection.SelectionOption("Count", 5)
    };

    public class SelectionOption
    {
      private string name;
      private int id;

      public string Name => this.name;

      public int Id => this.id;

      public SelectionOption(string name, int id)
      {
        this.name = name;
        this.id = id;
      }
    }
  }
}
