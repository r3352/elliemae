// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.EntityDescriptor
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  [DataContract(IsReference = true, Namespace = "")]
  public class EntityDescriptor
  {
    private static ConcurrentDictionary<EntityDescriptor, EntityDescriptor> _internedDescriptors = new ConcurrentDictionary<EntityDescriptor, EntityDescriptor>();
    private int _hashCode;
    private string _descriptorString;
    private bool _isInterned;

    [DataMember]
    public string EntityType { get; set; }

    [DataMember]
    public List<EntityParameter> EntityParameters { get; set; }

    public EntityDescriptor(string type, IEnumerable<EntityParameter> parameters)
    {
      this.EntityType = type;
      this.EntityParameters = parameters == null ? new List<EntityParameter>() : (parameters is List<EntityParameter> entityParameterList ? entityParameterList : parameters.ToList<EntityParameter>());
    }

    public EntityDescriptor(
      string type,
      IEnumerable<EntityParameter> parameters,
      string fullEntityType)
    {
      this.EntityType = type;
      this.EntityParameters = parameters == null ? new List<EntityParameter>() : (parameters is List<EntityParameter> entityParameterList ? entityParameterList : parameters.ToList<EntityParameter>());
      this._descriptorString = fullEntityType;
    }

    public EntityDescriptor() => this.EntityParameters = new List<EntityParameter>();

    public static EntityDescriptor Create(string fullEntityType)
    {
      return FieldReplacementRegex.ParseEntityDescriptor(fullEntityType);
    }

    public static EntityDescriptor Create(string type, IEnumerable<EntityParameter> parameters)
    {
      return new EntityDescriptor(type, parameters);
    }

    public static EntityDescriptor Create(Type type)
    {
      return new EntityDescriptor(type == (Type) null ? string.Empty : type.Name, (IEnumerable<EntityParameter>) null);
    }

    public bool IsA(EntityDescriptor descriptor)
    {
      return (object) descriptor != null && (descriptor.EntityParameters == null || descriptor.EntityParameters.Count <= 0) && this.EntityType == descriptor.EntityType;
    }

    public bool IsA(string baseType)
    {
      return !string.IsNullOrWhiteSpace(baseType) && this.EntityType == baseType;
    }

    public static bool IsNullOrEmpty(EntityDescriptor descriptor)
    {
      return (object) descriptor == null || string.IsNullOrWhiteSpace(descriptor.EntityType);
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this._descriptorString))
      {
        this._descriptorString = this.EntityType;
        if (this.EntityParameters != null && this.EntityParameters.Count > 0)
        {
          this._descriptorString += "(";
          for (int index = 0; index < this.EntityParameters.Count<EntityParameter>(); ++index)
          {
            if (index > 0)
              this._descriptorString += ",";
            this._descriptorString = this._descriptorString + this.EntityParameters[index].ParameterName + "=" + this.EntityParameters[index].ParameterValue;
          }
          this._descriptorString += ")";
        }
      }
      return this._descriptorString;
    }

    public EntityDescriptor ToInterned()
    {
      EntityDescriptor interned = this;
      if (!this._isInterned && !EntityDescriptor._internedDescriptors.TryGetValue(this, out interned))
      {
        interned = this;
        this._isInterned = true;
        EntityDescriptor._internedDescriptors.TryAdd(this, this);
      }
      return interned;
    }

    public bool IsInterned
    {
      get => this._isInterned;
      private set
      {
      }
    }

    public static bool operator ==(EntityDescriptor descriptor1, EntityDescriptor descriptor2)
    {
      return (object) descriptor1 == null ? (object) descriptor2 == null : descriptor1.Equals((object) descriptor2);
    }

    public static bool operator !=(EntityDescriptor descriptor1, EntityDescriptor descriptor2)
    {
      return (object) descriptor1 == null ? descriptor2 != null : !descriptor1.Equals((object) descriptor2);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      EntityDescriptor entityDescriptor = obj as EntityDescriptor;
      if ((object) entityDescriptor == null)
        return false;
      if (this.IsInterned && entityDescriptor.IsInterned)
        return (object) this == obj;
      try
      {
        int count1 = this.EntityParameters == null ? 0 : this.EntityParameters.Count;
        int count2 = entityDescriptor.EntityParameters == null ? 0 : entityDescriptor.EntityParameters.Count;
        if (count1 == 0 && count2 == 0)
          return string.CompareOrdinal(this.EntityType, entityDescriptor.EntityType) == 0;
        if (count1 != count2)
          return false;
        for (int index = 0; index < this.EntityParameters.Count; ++index)
        {
          if (this.EntityParameters[index] != entityDescriptor.EntityParameters[index])
            return false;
        }
      }
      catch (Exception ex)
      {
        throw new Exception(this.EntityType + " == " + entityDescriptor.EntityType, ex);
      }
      return string.CompareOrdinal(this.EntityType, entityDescriptor.EntityType) == 0;
    }

    public bool IsBaseType() => this.EntityParameters == null || this.EntityParameters.Count < 1;

    public EntityDescriptor GetBaseDescriptor()
    {
      return this.IsBaseType() ? this : EntityDescriptor.Create(this.EntityType);
    }

    public override int GetHashCode()
    {
      if (this._hashCode == 0)
      {
        this._hashCode = this.EntityType == null ? 0 : 92821 * this.EntityType.GetHashCode();
        foreach (object entityParameter in this.EntityParameters)
          this._hashCode = 92821 * this._hashCode + entityParameter.GetHashCode();
      }
      return this._hashCode;
    }
  }
}
