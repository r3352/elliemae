// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactLoanMatchType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// A enumeration used for performing queries against the Encompass contacts database
  /// that allows filtering the set of contacts retrieved based on closed loans related to
  /// each contact.
  /// </summary>
  [Guid("650CB8D7-8643-4a4b-B5B9-EA43B8305AE5")]
  public enum ContactLoanMatchType
  {
    /// <summary>Do not consider related loans when performing the query.</summary>
    None,
    /// <summary>Specified criteria must match at least one closed loan for each returned contact.</summary>
    AnyCompleted,
    /// <summary>Specified criteria must match the last closed loan for each returned contact.</summary>
    LastCompleted,
    /// <summary>Specified criteria must match at least one origniated loan for each returned contact.</summary>
    AnyOriginated,
    /// <summary>Specified criteria must match the most recently originated loan for each returned contact.</summary>
    LastOriginated,
  }
}
