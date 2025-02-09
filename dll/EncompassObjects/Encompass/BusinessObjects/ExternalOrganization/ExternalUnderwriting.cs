// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUnderwriting
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  [Flags]
  public enum ExternalUnderwriting
  {
    Delegated = 0,
    NonDeletgated = 1,
    ConditionallyDelegated = 2,
  }
}
