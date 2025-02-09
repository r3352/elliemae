// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ITemplateMilestone
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>Interface for Milestone class.</summary>
  /// <exclude />
  [Guid("AE5B37C2-54E2-4b41-94AE-9B77B5DBFC0B")]
  public interface ITemplateMilestone
  {
    int ID { get; }

    string Name { get; }

    TemplateMilestone Next { get; }

    TemplateMilestone Previous { get; }

    bool Equals(object o);
  }
}
