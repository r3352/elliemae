// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ExtensionInvocationEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class ExtensionInvocationEventArgs : EventArgs
  {
    public Type Target { get; set; }

    public ExtensionInvocationType InvocationType { get; set; }

    public TimeSpan? Elapsed { get; set; }
  }
}
