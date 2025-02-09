// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanFieldDescriptors
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanFieldDescriptors : SessionBoundObject, ILoanFieldDescriptors, IEnumerable
  {
    private FieldDescriptors customFields;

    internal LoanFieldDescriptors(Session session)
      : base(session)
    {
    }

    public FieldDescriptor this[string fieldId]
    {
      get
      {
        return this.CustomFields[fieldId] ?? FieldDescriptors.StandardFields[fieldId] ?? FieldDescriptors.VirtualFields[fieldId] ?? (FieldDescriptor) null;
      }
    }

    public FieldDescriptors StandardFields => FieldDescriptors.StandardFields;

    public FieldDescriptors VirtualFields => FieldDescriptors.VirtualFields;

    public FieldDescriptors CustomFields
    {
      get
      {
        this.ensureCustomFieldsLoaded();
        return this.customFields;
      }
    }

    public void Refresh() => this.customFields = (FieldDescriptors) null;

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
