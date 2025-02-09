// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.BorrowerPair
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class BorrowerPair
  {
    public static BorrowerPair All = new BorrowerPair(new Borrower(nameof (All), string.Empty, nameof (All)), new Borrower(string.Empty, string.Empty, string.Empty));
    private Borrower brw;
    private Borrower coBrw;

    public Borrower Borrower => this.brw;

    public Borrower CoBorrower => this.coBrw;

    public BorrowerPair(Borrower brw, Borrower coBrw)
    {
      this.brw = brw;
      this.coBrw = coBrw;
    }

    public string Id => this.brw.Id;

    public override string ToString()
    {
      if (this.coBrw.FirstName == string.Empty)
        return this.brw.ToString();
      return this.brw.LastName != this.coBrw.LastName ? this.brw.ToString() + " and " + this.coBrw.ToString() : this.brw.FirstName + " and " + this.coBrw.ToString();
    }

    public override int GetHashCode()
    {
      return this.coBrw == null ? this.brw.GetHashCode() : this.brw.GetHashCode() ^ this.coBrw.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      return obj is BorrowerPair borrowerPair && object.Equals((object) borrowerPair.Borrower, (object) this.Borrower) && object.Equals((object) borrowerPair.CoBorrower, (object) this.CoBorrower);
    }
  }
}
