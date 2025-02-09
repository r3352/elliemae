// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICalendarManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICalendarManager
  {
    AppointmentInfo GetAppointment(string dataKey);

    AppointmentInfo[] GetAllAppointmentsForUser(string UserID);

    AppointmentInfo[] GetAppointmentsForUser(string UserID, DateTime startTime, DateTime endTime);

    AppointmentInfo[] GetAppointmentsForUser(
      string UserID,
      DateTime startTime,
      DateTime endTime,
      QueryCriterion filter);

    AppointmentInfo[] GetAllAppointmentsForContact(ContactInfo cInfo, string UserID);

    AppointmentInfo[] GetAllAppointmentsForContact(ContactInfo cInfo);

    AppointmentInfo[] GetAppointmentAndVariances(string dataKey);

    int AddNewBlankRecordForAppointment(string userID);

    void DeleteAppointment(string DataKey);

    void UpdateAppointment(AppointmentInfo apptInfo);

    void UpdateAppointment(AppointmentInfo apptInfo, ContactInfo[] contacts);

    void UpdateAppointmentAndDeletedOccurrences(
      AppointmentInfo rootAppt,
      AppointmentInfo[] deletedInfo);

    ContactInfo[] GetContactInfo(string DataKey);

    AppointmentInfo[] QueryAppointments(QueryCriterion criteria);

    void AddContact(
      string sharerID,
      string userID,
      CSMessage.AccessLevel accessLevel,
      CSControlMessage msg);

    void DeleteContact(string sharerID, string userID);

    void UpdateContact(
      string sharerID,
      string userID,
      CSMessage.AccessLevel accessLevel,
      CSControlMessage ackMsg);

    CSListInfo GetCalenderSharingContactList(string UserID, bool fromOwner);

    CSContactInfo GetCalendarContactInfo(string ownerID, string userID);

    bool ContainsContact(string sharerID, string userID);

    CSControlMessage[] GetStoredCSMessages(string userID);

    void SaveCSMessage(CSControlMessage msg);
  }
}
