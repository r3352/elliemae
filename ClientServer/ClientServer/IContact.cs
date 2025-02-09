// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IContact
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [CLSCompliant(true)]
  public interface IContact : IDisposable
  {
    bool Exists { get; }

    int ContactID { get; }

    ContactType ContactType { get; }

    string FullName { get; }

    void Delete();

    void UndoCheckout();

    ContactHistoryItem[] GetHistory();

    ContactHistoryItem[] GetHistory(DateTime startDate, DateTime endDate);

    ContactHistoryItem[] GetHistory(string eventType);

    ContactHistoryItem[] GetHistory(string eventType, DateTime startDate, DateTime endDate);

    ContactHistoryItem GetHistoryItem(int itemId);

    int AddHistoryItem(ContactHistoryItem item);

    void DeleteHistoryItem(int historyItemId);

    ContactCustomField[] GetCustomFields();

    void UpdateCustomFields(ContactCustomField[] fields);

    ContactNote[] GetNotes();

    ContactNote[] GetNotes(DateTime startDate, DateTime endDate);

    ContactNote GetNote(int noteId);

    int AddNote(ContactNote note);

    void UpdateNote(ContactNote note);

    void DeleteNote(int noteId);

    void AddCreditScoresForHistoryItem(int historyId, ContactCreditScores[] contactScoresList);

    ContactCreditScores[] GetCreditScoresForHistoryItem(int historyId);
  }
}
