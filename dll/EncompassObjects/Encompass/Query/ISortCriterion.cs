// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.ISortCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Query
{
  [Guid("DE581669-D46E-4669-9252-CA860FACE17F")]
  public interface ISortCriterion
  {
    string FieldName { get; set; }

    SortOrder SortOrder { get; set; }

    DataConversion Conversion { get; set; }
  }
}
