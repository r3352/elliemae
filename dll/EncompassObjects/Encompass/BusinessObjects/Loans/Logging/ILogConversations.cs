// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogConversations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("8850FE1E-6EF8-40dd-AA05-ED198174DA3C")]
  public interface ILogConversations
  {
    int Count { get; }

    Conversation this[int index] { get; }

    Conversation Add(DateTime conversationDate);

    Conversation AddForUser(DateTime conversationDate, User heldByUser);

    void Remove(Conversation conversation);

    IEnumerator GetEnumerator();
  }
}
