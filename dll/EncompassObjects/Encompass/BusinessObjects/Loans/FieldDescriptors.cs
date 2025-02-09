// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.FieldDescriptors
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
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

    public int Count => this.fields.Count;

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

    public bool Contains(string fieldId) => this[fieldId] != null;

    public IEnumerator GetEnumerator() => this.fields.Values.GetEnumerator();

    public static FieldDescriptors StandardFields => FieldDescriptors.standardFields;

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
