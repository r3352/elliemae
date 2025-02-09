// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ContactFieldRef
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ContactFieldRef
  {
    public FileContactRecord.ContactTypes ContactType;
    public string CategoryName = string.Empty;
    public string CompanyField = string.Empty;
    public string AddressField = string.Empty;
    public string CityField = string.Empty;
    public string StateField = string.Empty;
    public string ZipField = string.Empty;
    public string RelationshipField = string.Empty;
    public string PhoneField = string.Empty;
    public string LineItemField = string.Empty;
    public string ContactNameField = string.Empty;
    public string EmailField = string.Empty;
    public string FaxField = string.Empty;

    public ContactFieldRef(
      FileContactRecord.ContactTypes contactType,
      string categoryName,
      string companyField,
      string addressField,
      string cityField,
      string stateField,
      string zipField,
      string relationshipField,
      string phoneField,
      string lineItemField,
      string contactNameField,
      string emailField,
      string faxField)
    {
      this.ContactType = contactType;
      this.CategoryName = categoryName;
      this.CompanyField = companyField;
      this.AddressField = addressField;
      this.CityField = cityField;
      this.StateField = stateField;
      this.ZipField = zipField;
      this.RelationshipField = relationshipField;
      this.PhoneField = phoneField;
      this.LineItemField = lineItemField;
      this.ContactNameField = contactNameField;
      this.EmailField = emailField;
      this.FaxField = faxField;
    }
  }
}
