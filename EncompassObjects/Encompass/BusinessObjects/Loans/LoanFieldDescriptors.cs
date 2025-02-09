// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFieldDescriptors
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for FieldDescriptors.</summary>
  public class LoanFieldDescriptors : SessionBoundObject, ILoanFieldDescriptors, IEnumerable
  {
    private FieldDescriptors customFields;

    internal LoanFieldDescriptors(Session session)
      : base(session)
    {
    }

    /// <summary>Looks up a field using its unique field ID.</summary>
    /// <param name="fieldId"></param>
    /// <returns></returns>
    /// <remarks>This method will find the field whether it is a custom field or a standard
    /// Encompass field.</remarks>
    public FieldDescriptor this[string fieldId]
    {
      get
      {
        return this.CustomFields[fieldId] ?? FieldDescriptors.StandardFields[fieldId] ?? FieldDescriptors.VirtualFields[fieldId] ?? (FieldDescriptor) null;
      }
    }

    /// <summary>
    /// Returns the list of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" /> objects for the defined standard fields.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method provided for users of non-.NET-based environments. Users of
    /// .NET- can retrieve this list though the static StandardFields property of the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptors" /> class.</remarks>
    public FieldDescriptors StandardFields => FieldDescriptors.StandardFields;

    /// <summary>
    /// Returns the list of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" /> objects for the defined virtual fields.
    /// </summary>
    /// <returns></returns>
    /// <remarks>This method provided for users of non-.NET-based environments. Users of
    /// .NET- can retrieve this list though the static VirtualFields property of the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptors" /> class.</remarks>
    public FieldDescriptors VirtualFields => FieldDescriptors.VirtualFields;

    /// <summary>
    /// Returns the list of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" /> objects for the defined custom fields.
    /// </summary>
    /// <returns></returns>
    public FieldDescriptors CustomFields
    {
      get
      {
        this.ensureCustomFieldsLoaded();
        return this.customFields;
      }
    }

    /// <summary>
    /// Refreshes the list of custom fields from the Encompass Server.
    /// </summary>
    public void Refresh() => this.customFields = (FieldDescriptors) null;

    /// <summary>
    /// Provides an enumerator for both standard and custom fields.
    /// </summary>
    /// <returns>Returns an enumerator which first iterates over standard fields and then
    /// iterates over custom fields.</returns>
    public IEnumerator GetEnumerator()
    {
      ArrayList arrayList = new ArrayList();
      foreach (FieldDescriptor standardField in this.StandardFields)
        arrayList.Add((object) standardField);
      foreach (FieldDescriptor virtualField in this.VirtualFields)
        arrayList.Add((object) virtualField);
      foreach (FieldDescriptor customField in this.CustomFields)
        arrayList.Add((object) customField);
      return arrayList.GetEnumerator();
    }

    private void ensureCustomFieldsLoaded()
    {
      if (this.customFields != null)
        return;
      FieldSettings fieldSettings = this.Session.SessionObjects.LoanManager.GetFieldSettings();
      this.customFields = new FieldDescriptors();
      foreach (FieldDefinition definedFieldDefinition in EncompassFields.GetAllUserDefinedFieldDefinitions(fieldSettings))
        this.customFields.Add(new FieldDescriptor(definedFieldDefinition));
    }

    private IConfigurationManager getConfigurationManager()
    {
      return this.Session.SessionObjects.ConfigurationManager;
    }

    internal FieldDescriptor GetOrCreate(string fieldId)
    {
      return this[fieldId] ?? FieldDescriptor.CreateUndefined(fieldId);
    }
  }
}
