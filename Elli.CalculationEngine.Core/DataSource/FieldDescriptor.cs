// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.FieldDescriptor
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  [DataContract(IsReference = true, Namespace = "")]
  public class FieldDescriptor
  {
    private const string _separator = ".";
    private FieldDescriptor _baseDescriptor;
    private int _hashCode;
    [DataMember]
    private Tuple<EntityDescriptor, string> descriptor;

    public FieldDescriptor(EntityDescriptor parentEntityType, string fieldId)
    {
      this.descriptor = new Tuple<EntityDescriptor, string>(parentEntityType, fieldId);
    }

    public FieldDescriptor(string parentEntityTypeString, string fieldId)
    {
      this.descriptor = new Tuple<EntityDescriptor, string>(FieldReplacementRegex.ParseEntityDescriptor(parentEntityTypeString), fieldId);
    }

    public static FieldDescriptor Create(string fullEntityType, string fieldId)
    {
      return new FieldDescriptor(EntityDescriptor.Create(fullEntityType), fieldId);
    }

    public static FieldDescriptor Create(EntityDescriptor parentEntityType, string fieldId)
    {
      return new FieldDescriptor(parentEntityType, fieldId);
    }

    public static FieldDescriptor Create(string descriptorString)
    {
      FieldDescriptor fieldDescriptor = (FieldDescriptor) null;
      if (!string.IsNullOrWhiteSpace(descriptorString))
      {
        string[] strArray = descriptorString.Split(new string[1]
        {
          "."
        }, 2, StringSplitOptions.None);
        fieldDescriptor = new FieldDescriptor(strArray[0], strArray[1]);
      }
      return fieldDescriptor;
    }

    private FieldDescriptor()
    {
    }

    public EntityDescriptor ParentEntityType
    {
      get => this.descriptor.Item1;
      private set
      {
      }
    }

    public string FieldId
    {
      get => this.descriptor.Item2;
      private set
      {
      }
    }

    public override string ToString()
    {
      return ((object) this.descriptor.Item1 == null ? string.Empty : this.descriptor.Item1.ToString()) + "." + this.descriptor.Item2;
    }

    public FieldDescriptor GetBaseDescriptor()
    {
      if ((object) this._baseDescriptor == null)
        this._baseDescriptor = FieldDescriptor.Create(this.ParentEntityType.GetBaseDescriptor(), this.FieldId);
      return this._baseDescriptor;
    }

    public static bool operator ==(FieldDescriptor descriptor1, FieldDescriptor descriptor2)
    {
      return (object) descriptor1 == null ? (object) descriptor2 == null : descriptor1.Equals((object) descriptor2);
    }

    public static bool operator !=(FieldDescriptor descriptor1, FieldDescriptor descriptor2)
    {
      return (object) descriptor1 == null ? descriptor2 != null : !descriptor1.Equals((object) descriptor2);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      FieldDescriptor fieldDescriptor = obj as FieldDescriptor;
      return (object) fieldDescriptor != null && this.FieldId == fieldDescriptor.FieldId && this.ParentEntityType == fieldDescriptor.ParentEntityType;
    }

    public override int GetHashCode()
    {
      if (this._hashCode == 0)
        this._hashCode = (string.IsNullOrWhiteSpace(this.FieldId) ? 0 : 92821 * this.FieldId.GetHashCode()) + ((object) this.ParentEntityType == null ? 0 : this.ParentEntityType.GetHashCode());
      return this._hashCode;
    }
  }
}
