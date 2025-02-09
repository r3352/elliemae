// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IPipelineAlert
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("7ED8AF09-54A9-4e6b-A745-BD287A5C8DC7")]
  public interface IPipelineAlert
  {
    AlertType Type { get; }

    string Source { get; }

    AlertStatus Status { get; }

    Role WorkflowRole { get; }

    object Date { get; }
  }
}
