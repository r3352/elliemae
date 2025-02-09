// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.Borrower2
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://dataservices.elliemaeservices.com/")]
  [Serializable]
  public class Borrower2
  {
    private string borrowerIndexField;
    private string currAddressCityField;
    private string currAddressStateField;
    private string currAddressZipField;
    private string residencyBasisField;
    private string incomeField;
    private string middleFICOField;
    private string experianField;
    private string transUnionField;
    private string equifaxField;
    private string minimumFICOField;
    private string dOBField;
    private string ethnicityField;
    private string raceField;
    private string sexField;
    private string uSCitizenField;
    private string permResAlienField;
    private string emailPresentField;
    private string sSNPresentField;
    private string lastNamePresentField;
    private string selfEmployedField;
    private string maritalStatusField;
    private string noOfDependantsField;
    private string dependantsAgesField;
    private string intendToOccupyField;
    private string ownershipInterestField;
    private string propertyTypeOwnedField;
    private string titleOwnedAsField;

    public string BorrowerIndex
    {
      get => this.borrowerIndexField;
      set => this.borrowerIndexField = value;
    }

    public string CurrAddressCity
    {
      get => this.currAddressCityField;
      set => this.currAddressCityField = value;
    }

    public string CurrAddressState
    {
      get => this.currAddressStateField;
      set => this.currAddressStateField = value;
    }

    public string CurrAddressZip
    {
      get => this.currAddressZipField;
      set => this.currAddressZipField = value;
    }

    public string ResidencyBasis
    {
      get => this.residencyBasisField;
      set => this.residencyBasisField = value;
    }

    public string Income
    {
      get => this.incomeField;
      set => this.incomeField = value;
    }

    public string MiddleFICO
    {
      get => this.middleFICOField;
      set => this.middleFICOField = value;
    }

    public string Experian
    {
      get => this.experianField;
      set => this.experianField = value;
    }

    public string TransUnion
    {
      get => this.transUnionField;
      set => this.transUnionField = value;
    }

    public string Equifax
    {
      get => this.equifaxField;
      set => this.equifaxField = value;
    }

    public string MinimumFICO
    {
      get => this.minimumFICOField;
      set => this.minimumFICOField = value;
    }

    public string DOB
    {
      get => this.dOBField;
      set => this.dOBField = value;
    }

    public string Ethnicity
    {
      get => this.ethnicityField;
      set => this.ethnicityField = value;
    }

    public string Race
    {
      get => this.raceField;
      set => this.raceField = value;
    }

    public string Sex
    {
      get => this.sexField;
      set => this.sexField = value;
    }

    public string USCitizen
    {
      get => this.uSCitizenField;
      set => this.uSCitizenField = value;
    }

    public string PermResAlien
    {
      get => this.permResAlienField;
      set => this.permResAlienField = value;
    }

    public string EmailPresent
    {
      get => this.emailPresentField;
      set => this.emailPresentField = value;
    }

    public string SSNPresent
    {
      get => this.sSNPresentField;
      set => this.sSNPresentField = value;
    }

    public string LastNamePresent
    {
      get => this.lastNamePresentField;
      set => this.lastNamePresentField = value;
    }

    public string SelfEmployed
    {
      get => this.selfEmployedField;
      set => this.selfEmployedField = value;
    }

    public string MaritalStatus
    {
      get => this.maritalStatusField;
      set => this.maritalStatusField = value;
    }

    public string NoOfDependants
    {
      get => this.noOfDependantsField;
      set => this.noOfDependantsField = value;
    }

    public string DependantsAges
    {
      get => this.dependantsAgesField;
      set => this.dependantsAgesField = value;
    }

    public string IntendToOccupy
    {
      get => this.intendToOccupyField;
      set => this.intendToOccupyField = value;
    }

    public string OwnershipInterest
    {
      get => this.ownershipInterestField;
      set => this.ownershipInterestField = value;
    }

    public string PropertyTypeOwned
    {
      get => this.propertyTypeOwnedField;
      set => this.propertyTypeOwnedField = value;
    }

    public string TitleOwnedAs
    {
      get => this.titleOwnedAsField;
      set => this.titleOwnedAsField = value;
    }
  }
}
