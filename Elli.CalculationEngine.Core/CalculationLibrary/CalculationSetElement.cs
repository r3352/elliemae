// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.CalculationSetElement
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  [DataContract]
  public class CalculationSetElement : LibraryElement, ICalculationSetElement
  {
    public CalculationSetElement() => this.Version = this.GetVersion();

    [DataMember]
    public string Version { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string DescriptiveName { get; set; }

    [DataMember]
    public bool Enabled { get; set; }

    [DataMember]
    public List<CalculationTest> CalculationTests { get; set; }

    [IgnoreDataMember]
    public bool IsValidEntityType { get; set; }

    [IgnoreDataMember]
    public bool IsTemplateVerified { get; set; }

    [IgnoreDataMember]
    public bool IsTransientVerified { get; set; }

    [IgnoreDataMember]
    public string ParentEntityType
    {
      get
      {
        if (this.Identity == null)
          return string.Empty;
        switch (this.Identity.Type)
        {
          case LibraryElementType.FieldExpressionCalculation:
            return ((FieldExpressionCalculation) this).Expression != null ? ((FieldExpressionCalculation) this).Expression.ParentEntityType : string.Empty;
          case LibraryElementType.CalculationTemplate:
            return ((CalculationTemplate) this).Expression != null ? ((CalculationTemplate) this).Expression.ParentEntityType : string.Empty;
          case LibraryElementType.Function:
            return ((Function) this).Expression != null ? ((Function) this).Expression.ParentEntityType : string.Empty;
          case LibraryElementType.TransientDataObject:
            return ((TransientDataObject) this).Expression != null ? ((TransientDataObject) this).Expression.ParentEntityType : string.Empty;
          default:
            return CalculationUtility.GetRootEntityType();
        }
      }
      set
      {
        if (this.Identity == null)
          return;
        switch (this.Identity.Type)
        {
          case LibraryElementType.FieldExpressionCalculation:
            if (((FieldExpressionCalculation) this).Expression == null)
              break;
            ((FieldExpressionCalculation) this).Expression.ParentEntityType = value;
            break;
          case LibraryElementType.CalculationTemplate:
            if (((CalculationTemplate) this).Expression == null)
              break;
            ((CalculationTemplate) this).Expression.ParentEntityType = value;
            break;
          case LibraryElementType.Function:
            if (((Function) this).Expression == null)
              break;
            ((Function) this).Expression.ParentEntityType = value;
            break;
          case LibraryElementType.TransientDataObject:
            if (((TransientDataObject) this).Expression == null)
              break;
            ((TransientDataObject) this).Expression.ParentEntityType = value;
            break;
        }
      }
    }

    public string GetVersion()
    {
      string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
      int num = version.IndexOf(".");
      if (num < 0)
        return "1.0";
      int length = version.Length;
      while (length > num)
      {
        length = version.LastIndexOf(".");
        if (length > num)
          version = version.Substring(0, length);
      }
      return version;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("For: {0} {1}\r\n", (object) this.Name, string.IsNullOrEmpty(this.DescriptiveName) ? (object) string.Empty : (object) string.Format("({0})", (object) this.DescriptiveName)));
      stringBuilder.Append(string.Format("Class: {0}\r\n", (object) this.Identity.ClassName));
      stringBuilder.Append(string.Format("Description: {0}\r\n", (object) this.Identity.Description));
      return stringBuilder.ToString();
    }
  }
}
