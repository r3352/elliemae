// Decompiled with JetBrains decompiler
// Type: Export.Fannie32.HmdaExport.Factories.CoBorrowerFactory
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using Export.Fannie32.HmdaExport.Borrowers;

#nullable disable
namespace Export.Fannie32.HmdaExport.Factories
{
  internal class CoBorrowerFactory : IBorrowerFactory
  {
    public Borrower CreateBorrower(string coBorrowerFirstName, string coBorrowerSsn)
    {
      return (Borrower) new CoBorrower(coBorrowerFirstName, coBorrowerSsn);
    }
  }
}
