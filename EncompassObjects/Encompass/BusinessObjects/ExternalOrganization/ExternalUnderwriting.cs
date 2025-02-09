// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUnderwriting
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Defines the possible upderwriting options for an external organization
  /// </summary>
  [Flags]
  public enum ExternalUnderwriting
  {
    /// <summary>Delegated</summary>
    Delegated = 0,
    /// <summary>Non-Deletgated</summary>
    NonDeletgated = 1,
    /// <summary>Conditionally Delegated</summary>
    ConditionallyDelegated = 2,
  }
}
