// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.SelectionOptions
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public static class SelectionOptions
  {
    public static SelectionOptions.SelectionOption[] TemplateTypeOptions = new SelectionOptions.SelectionOption[3]
    {
      new SelectionOptions.SelectionOption("Loan Template", 0),
      new SelectionOptions.SelectionOption("Borrower Contact Template", 1),
      new SelectionOptions.SelectionOption("Business Contact Template", 2)
    };
    public static SelectionOptions.SelectionOption[] ChartTypeOptions = new SelectionOptions.SelectionOption[4]
    {
      new SelectionOptions.SelectionOption("Bar Chart", 0),
      new SelectionOptions.SelectionOption("Trend Chart", 1),
      new SelectionOptions.SelectionOption("Loan Table", 2),
      new SelectionOptions.SelectionOption("User Table", 3)
    };
    public static SelectionOptions.SelectionOption[] SummaryTypeOptions = new SelectionOptions.SelectionOption[2]
    {
      new SelectionOptions.SelectionOption("Number of Files", 0),
      new SelectionOptions.SelectionOption("Dollor Amount", 1)
    };
    public static SelectionOptions.SelectionOption[] TimeFrameOptions = new SelectionOptions.SelectionOption[17]
    {
      new SelectionOptions.SelectionOption("Current Week", 0),
      new SelectionOptions.SelectionOption("Current Month", 1),
      new SelectionOptions.SelectionOption("Current Year", 2),
      new SelectionOptions.SelectionOption("Previous Week", 3),
      new SelectionOptions.SelectionOption("Previous Month", 4),
      new SelectionOptions.SelectionOption("Previous Year", 5),
      new SelectionOptions.SelectionOption("Last 7 Days", 6),
      new SelectionOptions.SelectionOption("Last 30 Days", 7),
      new SelectionOptions.SelectionOption("Last 90 Days", 8),
      new SelectionOptions.SelectionOption("Last 365 Days", 9),
      new SelectionOptions.SelectionOption("Next Week", 10),
      new SelectionOptions.SelectionOption("Next Month", 11),
      new SelectionOptions.SelectionOption("Next Year", 12),
      new SelectionOptions.SelectionOption("Next 7 Days", 13),
      new SelectionOptions.SelectionOption("Next 30 Days", 14),
      new SelectionOptions.SelectionOption("Next 90 Days", 15),
      new SelectionOptions.SelectionOption("Next 365 Days", 16)
    };
    public static SelectionOptions.SelectionOption[] TimeFrameOptions2 = new SelectionOptions.SelectionOption[10]
    {
      new SelectionOptions.SelectionOption("Current Week", 0),
      new SelectionOptions.SelectionOption("Current Month", 1),
      new SelectionOptions.SelectionOption("Current Year", 2),
      new SelectionOptions.SelectionOption("Previous Week", 3),
      new SelectionOptions.SelectionOption("Previous Month", 4),
      new SelectionOptions.SelectionOption("Previous Year", 5),
      new SelectionOptions.SelectionOption("Last 7 Days", 6),
      new SelectionOptions.SelectionOption("Last 30 Days", 7),
      new SelectionOptions.SelectionOption("Last 90 Days", 8),
      new SelectionOptions.SelectionOption("Last 365 Days", 9)
    };
    public static SelectionOptions.SelectionOption[] TimePeriodOptions = new SelectionOptions.SelectionOption[11]
    {
      new SelectionOptions.SelectionOption("Previous 4 weeks", 0),
      new SelectionOptions.SelectionOption("Previous 4 months", 1),
      new SelectionOptions.SelectionOption("Previous 6 months", 2),
      new SelectionOptions.SelectionOption("Previous 12 months", 3),
      new SelectionOptions.SelectionOption("Previous 24 months", 4),
      new SelectionOptions.SelectionOption("Last 7 Days", 5),
      new SelectionOptions.SelectionOption("Next 7 Days", 6),
      new SelectionOptions.SelectionOption("Next 4 Weeks", 7),
      new SelectionOptions.SelectionOption("Next 4 Months", 8),
      new SelectionOptions.SelectionOption("Next 6 Months", 9),
      new SelectionOptions.SelectionOption("Next 12 Months", 10)
    };
    public static SelectionOptions.SelectionOption[] TimePeriodOptions2 = new SelectionOptions.SelectionOption[5]
    {
      new SelectionOptions.SelectionOption("Previous 4 weeks", 0),
      new SelectionOptions.SelectionOption("Previous 4 months", 1),
      new SelectionOptions.SelectionOption("Previous 6 months", 2),
      new SelectionOptions.SelectionOption("Previous 12 months", 3),
      new SelectionOptions.SelectionOption("Previous 24 months", 4)
    };
    public static SelectionOptions.SelectionOption[] SubsetTypeOptions = new SelectionOptions.SelectionOption[2]
    {
      new SelectionOptions.SelectionOption("Highest", 0),
      new SelectionOptions.SelectionOption("Lowest", 1)
    };
    public static SelectionOptions.SelectionOption[] SummaryTypeOptions2 = new SelectionOptions.SelectionOption[2]
    {
      new SelectionOptions.SelectionOption("Average", 4),
      new SelectionOptions.SelectionOption("Total", 3)
    };
    public const int DB_APPRAISAL = 0;
    public const int DB_CREDIT = 1;
    public const int DB_DOCUMENT_PREPARATION = 2;
    public const int DB_ESCROW = 3;
    public const int DB_FLOOD = 4;
    public const int DB_HAZARD_INSURANCE = 5;
    public const int DB_MORTGAGE_INSURANCE = 6;
    public const int DB_TITLE = 7;
    public const int DB_UNDERWRITER = 8;
    public static SelectionOptions.SelectionOption[] LenderVendorOptions = new SelectionOptions.SelectionOption[9]
    {
      new SelectionOptions.SelectionOption("Appraisal", 0),
      new SelectionOptions.SelectionOption("Credit", 1),
      new SelectionOptions.SelectionOption("Document Preparation", 2),
      new SelectionOptions.SelectionOption("Escrow", 3),
      new SelectionOptions.SelectionOption("Flood", 4),
      new SelectionOptions.SelectionOption("Hazard Insurance", 5),
      new SelectionOptions.SelectionOption("Mortgage Insurance", 6),
      new SelectionOptions.SelectionOption("Title", 7),
      new SelectionOptions.SelectionOption("Underwriter", 8)
    };
    public const int DB_COMPLETED = 0;
    public const int DB_FUNDED = 1;
    public const int DB_DOCUMENTED = 2;
    public const int DB_APPROVED = 3;
    public static SelectionOptions.SelectionOption[] CommissionTypeOptions = new SelectionOptions.SelectionOption[4]
    {
      new SelectionOptions.SelectionOption("Completed", 0),
      new SelectionOptions.SelectionOption("Funded", 1),
      new SelectionOptions.SelectionOption("Documented", 2),
      new SelectionOptions.SelectionOption("Approved", 3)
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

      public override string ToString() => this.name;
    }
  }
}
