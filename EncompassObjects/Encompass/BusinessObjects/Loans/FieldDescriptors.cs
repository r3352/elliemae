// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptors
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Summary description for FieldDescriptorCollection.</summary>
  public class FieldDescriptors : IFieldDescriptors, IEnumerable
  {
    private static FieldDescriptors standardFields = new FieldDescriptors((FieldDescriptors.IFieldSource) new FieldDescriptors.StandardFieldSource());
    private static FieldDescriptors virtualFields = new FieldDescriptors((FieldDescriptors.IFieldSource) new FieldDescriptors.VirtualFieldSource());
    private Hashtable fields = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable instances = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private FieldDescriptors.IFieldSource fieldSource;

    internal FieldDescriptors()
    {
    }

    private FieldDescriptors(FieldDescriptors.IFieldSource fieldSource)
    {
      this.fieldSource = fieldSource;
      foreach (FieldDefinition fieldDef in (IEnumerable) fieldSource)
        this.Add(new FieldDescriptor(fieldDef));
    }

    internal void Add(FieldDescriptor field)
    {
      if (this.fields.Contains((object) field.FieldID))
        return;
      this.fields.Add((object) field.FieldID, (object) field);
    }

    /// <summary>Returns the number of fields in the collection.</summary>
    /// <remarks>The count does not include instances of multi-instance fields since there is
    /// not set limit on the number of such fields.</remarks>
    public int Count => this.fields.Count;

    /// <summary>
    /// Retrieves a <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptor" /> using the field's Field ID.
    /// </summary>
    public FieldDescriptor this[string fieldId]
    {
      get
      {
        if (this.fields.Contains((object) fieldId))
          return (FieldDescriptor) this.fields[(object) fieldId];
        if (this.instances.Contains((object) fieldId))
          return (FieldDescriptor) this.instances[(object) fieldId];
        if (this.fieldSource == null)
          return (FieldDescriptor) null;
        FieldDefinition field = this.fieldSource.GetField(fieldId);
        if (field == null)
          return (FieldDescriptor) null;
        FieldDescriptor fieldDescriptor = new FieldDescriptor(field);
        this.instances.Add((object) fieldDescriptor.FieldID, (object) fieldDescriptor);
        return fieldDescriptor;
      }
    }

    /// <summary>Checks if a field ID is contained in the field list.</summary>
    /// <param name="fieldId">The Field ID to be checked</param>
    /// <returns>Returns <c>true</c> if the field is present, <c>false</c> otherwise.</returns>
    public bool Contains(string fieldId) => this[fieldId] != null;

    /// <summary>Provides an enumerator for the collection.</summary>
    /// <returns>An enumerator for interating over the collection of fields.</returns>
    /// <remarks>When iterating over the collection of fields, instance of multi-instance
    /// fields are not included.</remarks>
    public IEnumerator GetEnumerator() => this.fields.Values.GetEnumerator();

    /// <summary>Retrieves the list of all Standard Encompass fields.</summary>
    /// <returns>Returns the collection of standard fields.</returns>
    public static FieldDescriptors StandardFields => FieldDescriptors.standardFields;

    /// <summary>Retrieves the list of all Standard Encompass fields.</summary>
    /// <returns>Returns the collection of standard fields.</returns>
    public static FieldDescriptors VirtualFields => FieldDescriptors.virtualFields;

    private interface IFieldSource : IEnumerable
    {
      FieldDefinition GetField(string fieldId);
    }

    private class StandardFieldSource : FieldDescriptors.IFieldSource, IEnumerable
    {
      public FieldDefinition GetField(string fieldId)
      {
        return (FieldDefinition) EllieMae.EMLite.DataEngine.StandardFields.GetField(fieldId, true);
      }

      public IEnumerator GetEnumerator() => EllieMae.EMLite.DataEngine.StandardFields.All.GetEnumerator();
    }

    private class VirtualFieldSource : FieldDescriptors.IFieldSource, IEnumerable
    {
      public FieldDefinition GetField(string fieldId)
      {
        return (FieldDefinition) EllieMae.EMLite.DataEngine.VirtualFields.GetField(fieldId, true);
      }

      public IEnumerator GetEnumerator() => EllieMae.EMLite.DataEngine.VirtualFields.All.GetEnumerator();
    }
  }
}
