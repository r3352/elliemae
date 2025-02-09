// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.DataSource.EntityParameter
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.DataSource
{
  [DataContract(Namespace = "")]
  public class EntityParameter
  {
    private int _hashCode;

    [DataMember]
    public string ParameterName { get; set; }

    [DataMember]
    public string ParameterValue { get; set; }

    public EntityParameter(string name, string value)
    {
      this.ParameterName = name;
      this.ParameterValue = value;
    }

    public EntityParameter(string fullParameter)
    {
      try
      {
        string[] strArray = fullParameter.Split('=');
        this.ParameterName = strArray[0];
        this.ParameterValue = strArray[1];
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Error creating EntityParameter '{0}'"), ex);
      }
    }

    public static bool operator ==(EntityParameter descriptor1, EntityParameter descriptor2)
    {
      return (object) descriptor1 == null ? (object) descriptor2 == null : descriptor1.Equals((object) descriptor2);
    }

    public static bool operator !=(EntityParameter descriptor1, EntityParameter descriptor2)
    {
      return (object) descriptor1 == null ? descriptor2 != null : !descriptor1.Equals((object) descriptor2);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      EntityParameter entityParameter = obj as EntityParameter;
      return (object) entityParameter != null && this.ParameterName == entityParameter.ParameterName && this.ParameterValue == entityParameter.ParameterValue;
    }

    public override int GetHashCode()
    {
      if (this._hashCode == 0)
        this._hashCode = (string.IsNullOrWhiteSpace(this.ParameterName) ? 0 : this.ParameterName.GetHashCode()) + (string.IsNullOrWhiteSpace(this.ParameterValue) ? 0 : 92821 * this.ParameterValue.GetHashCode());
      return this._hashCode;
    }
  }
}
