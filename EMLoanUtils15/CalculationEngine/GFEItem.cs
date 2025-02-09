// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.GFEItem
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class GFEItem
  {
    public int LineNumber;
    public string ComponentID = "";
    public string Description = "";
    public string BorrowerFieldID = "";
    public string SellerFieldID = "";
    public string PayeeFieldID = "";
    public string POCFieldID = "";
    public string PaidByFieldID = "";
    public string PTBFieldID = "";
    public string CheckBorrowerFieldID = "";
    public string CheckSellerFieldID = "";
    public string For2015 = "";

    public GFEItem(
      int lineNumber,
      string payeeFieldID,
      string borrowerFieldID,
      string sellerFieldID,
      string paidByFieldID,
      string pocFieldID,
      string ptbFieldID,
      string checkBorrowerFieldID,
      string checkSellerFieldID,
      string description,
      string for2015)
      : this(lineNumber, "", payeeFieldID, borrowerFieldID, sellerFieldID, paidByFieldID, pocFieldID, ptbFieldID, checkBorrowerFieldID, checkSellerFieldID, description, for2015)
    {
    }

    public GFEItem(
      int lineNumber,
      string componentID,
      string payeeFieldID,
      string borrowerFieldID,
      string sellerFieldID,
      string paidByFieldID,
      string pocFieldID,
      string ptbFieldID,
      string checkBorrowerFieldID,
      string checkSellerFieldID,
      string description,
      string for2015)
    {
      this.LineNumber = lineNumber;
      this.ComponentID = componentID;
      this.Description = description;
      this.BorrowerFieldID = borrowerFieldID;
      this.SellerFieldID = sellerFieldID;
      this.POCFieldID = pocFieldID;
      this.PayeeFieldID = payeeFieldID;
      this.PaidByFieldID = paidByFieldID;
      this.PTBFieldID = ptbFieldID;
      this.CheckBorrowerFieldID = checkBorrowerFieldID;
      this.CheckSellerFieldID = checkSellerFieldID;
      this.For2015 = for2015;
    }

    public GFEItem Clone()
    {
      return new GFEItem(this.LineNumber, this.ComponentID, this.PayeeFieldID, this.BorrowerFieldID, this.SellerFieldID, this.PaidByFieldID, this.POCFieldID, this.PTBFieldID, this.CheckBorrowerFieldID, this.CheckSellerFieldID, this.Description, this.For2015);
    }
  }
}
