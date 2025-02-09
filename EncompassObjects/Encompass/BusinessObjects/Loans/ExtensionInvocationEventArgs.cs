// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ExtensionInvocationEventArgs
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Describes event information of the extension that is being fired.
  /// </summary>
  public class ExtensionInvocationEventArgs : EventArgs
  {
    /// <summary>Type information of the extension being fired.</summary>
    public Type Target { get; set; }

    /// <summary>Gets or sets the invocation type being invoked.</summary>
    public ExtensionInvocationType InvocationType { get; set; }

    /// <summary>Gets or sets the elapsed time if provided.</summary>
    public TimeSpan? Elapsed { get; set; }
  }
}
