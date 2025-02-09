// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.CalendarManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Server;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class CalendarManager : SessionBoundObject, ICalendarManager
  {
    private const string className = "CalendarManager";
    private CalendarSO engine;

    public CalendarManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (CalendarManager).ToLower());
      this.engine = new CalendarSO(session.UserID);
      return this;
    }

    public virtual AppointmentInfo GetAppointment(string dataKey)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAppointment), new object[1]
      {
        (object) dataKey
      });
      try
      {
        return this.engine.GetAppointment(dataKey);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo) null;
      }
    }

    public virtual AppointmentInfo[] GetAppointmentAndVariances(string dataKey)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAppointmentAndVariances), new object[1]
      {
        (object) dataKey
      });
      try
      {
        return this.engine.GetAppointmentAndVariances(dataKey);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual AppointmentInfo[] GetAllAppointmentsForUser(string userID)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAllAppointmentsForUser), new object[1]
      {
        (object) userID
      });
      try
      {
        return this.engine.GetAllAppointmentsForUser(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual AppointmentInfo[] GetAppointmentsForUser(
      string userId,
      DateTime startTime,
      DateTime endTime)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAppointmentsForUser), new object[3]
      {
        (object) userId,
        (object) startTime,
        (object) endTime
      });
      try
      {
        return this.engine.GetAppointmentsForUser(userId, startTime, endTime);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual AppointmentInfo[] GetAppointmentsForUser(
      string userId,
      DateTime startTime,
      DateTime endTime,
      QueryCriterion filter)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAppointmentsForUser), new object[4]
      {
        (object) userId,
        (object) startTime,
        (object) endTime,
        (object) filter
      });
      try
      {
        return this.engine.GetAppointmentsForUser(userId, startTime, endTime, filter);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual AppointmentInfo[] GetAllAppointmentsForContact(ContactInfo cInfo, string userID)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAllAppointmentsForContact), new object[2]
      {
        (object) cInfo,
        (object) userID
      });
      try
      {
        return this.engine.GetAllAppointmentsForContact(cInfo, userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual AppointmentInfo[] GetAllAppointmentsForContact(ContactInfo cInfo)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetAllAppointmentsForContact), new object[1]
      {
        (object) cInfo
      });
      try
      {
        return this.engine.GetAllAppointmentsForContact(cInfo, this.GetCalenderSharingContactList(this.Session.UserID, false), this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual void DeleteAppointment(string dataKey)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (DeleteAppointment), new object[1]
      {
        (object) dataKey
      });
      try
      {
        AppointmentInfo appointment = this.engine.GetAppointment(dataKey);
        if (appointment == null)
          return;
        this.engine.DeleteAppointment(dataKey);
        foreach (Elli.Server.Remoting.SessionObjects.Session session in this.Session.Context.Sessions.GetSessionsForUser(appointment.UserID))
        {
          try
          {
            session.RaiseEvent((ServerEvent) new AppointmentEvent(session.SessionInfo, dataKey, AppointmentAction.Delete, appointment.UserID));
          }
          catch
          {
          }
        }
        this.NotifyAccessor(appointment, dataKey, AppointmentAction.Delete);
        if (!this.Session.CSEnabled)
          return;
        AppointmentEvent appointmentEvent = new AppointmentEvent((SessionInfo) null, dataKey, AppointmentAction.Delete, appointment.UserID);
        this.Session.Context.Sessions.RaiseServerEventOnRemoveServers((Message) new AppointmentEventMessage((SessionInfo) null, new string[1]
        {
          appointment.UserID
        }, appointmentEvent));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
      }
    }

    private void NotifyAccessor(AppointmentInfo appt, string dataKey, AppointmentAction action)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (NotifyAccessor), new object[3]
      {
        (object) appt,
        (object) dataKey,
        (object) action
      });
      try
      {
        CSListInfo sharingContactList = InterceptionUtils.NewInstance<CalendarManager>().Initialize(this.Session).GetCalenderSharingContactList(appt.UserID, true);
        List<string> stringList = new List<string>();
        foreach (CSContactInfo csContactInfo in sharingContactList.GetCSContactInfoList())
        {
          foreach (Elli.Server.Remoting.SessionObjects.Session session in this.Session.Context.Sessions.GetSessionsForUser(csContactInfo.UserID))
          {
            try
            {
              session.RaiseEvent((ServerEvent) new AppointmentEvent(session.SessionInfo, dataKey, action, appt.UserID));
            }
            catch
            {
            }
          }
          if (this.Session.CSEnabled && !stringList.Contains(csContactInfo.UserID))
            stringList.Add(csContactInfo.UserID);
        }
        if (!this.Session.CSEnabled || stringList.Count <= 0)
          return;
        AppointmentEvent appointmentEvent = new AppointmentEvent((SessionInfo) null, dataKey, action, appt.UserID);
        this.Session.Context.Sessions.RaiseServerEventOnRemoveServers((Message) new AppointmentEventMessage((SessionInfo) null, stringList.ToArray(), appointmentEvent));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
      }
    }

    public void UpdateAppointment(AppointmentInfo apptInfo)
    {
      this.UpdateAppointment(apptInfo, (ContactInfo[]) null);
    }

    public virtual void UpdateAppointment(AppointmentInfo apptInfo, ContactInfo[] contacts)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (UpdateAppointment), new object[2]
      {
        (object) apptInfo,
        (object) contacts
      });
      try
      {
        this.engine.UpdateAppointment(apptInfo, contacts);
        foreach (Elli.Server.Remoting.SessionObjects.Session session in this.Session.Context.Sessions.GetSessionsForUser(apptInfo.UserID))
        {
          try
          {
            session.RaiseEvent((ServerEvent) new AppointmentEvent(session.SessionInfo, apptInfo.DataKey, AppointmentAction.CreateUpdate, apptInfo.UserID));
          }
          catch
          {
          }
        }
        this.NotifyAccessor(apptInfo, apptInfo.DataKey, AppointmentAction.CreateUpdate);
        if (!this.Session.CSEnabled)
          return;
        AppointmentEvent appointmentEvent = new AppointmentEvent((SessionInfo) null, apptInfo.DataKey, AppointmentAction.CreateUpdate, apptInfo.UserID);
        this.Session.Context.Sessions.RaiseServerEventOnRemoveServers((Message) new AppointmentEventMessage((SessionInfo) null, new string[1]
        {
          apptInfo.UserID
        }, appointmentEvent));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateAppointmentAndDeletedOccurrences(
      AppointmentInfo rootAppt,
      AppointmentInfo[] deletedInfo)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (UpdateAppointmentAndDeletedOccurrences), new object[2]
      {
        (object) rootAppt,
        (object) deletedInfo
      });
      try
      {
        this.engine.UpdateAppointmentAndDeletedOccurrences(rootAppt, deletedInfo);
        foreach (Elli.Server.Remoting.SessionObjects.Session session in this.Session.Context.Sessions.GetSessionsForUser(rootAppt.UserID))
        {
          try
          {
            session.RaiseEvent((ServerEvent) new AppointmentEvent(session.SessionInfo, rootAppt.DataKey, AppointmentAction.CreateUpdate, rootAppt.UserID));
          }
          catch
          {
          }
        }
        this.NotifyAccessor(rootAppt, rootAppt.DataKey, AppointmentAction.CreateUpdate);
        if (!this.Session.CSEnabled)
          return;
        AppointmentEvent appointmentEvent = new AppointmentEvent((SessionInfo) null, rootAppt.DataKey, AppointmentAction.CreateUpdate, rootAppt.UserID);
        this.Session.Context.Sessions.RaiseServerEventOnRemoveServers((Message) new AppointmentEventMessage((SessionInfo) null, new string[1]
        {
          rootAppt.UserID
        }, appointmentEvent));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual int AddNewBlankRecordForAppointment(string userId)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (AddNewBlankRecordForAppointment), new object[1]
      {
        (object) userId
      });
      try
      {
        return this.engine.AddNewBlankRecordForAppointment(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual ContactInfo[] GetContactInfo(string dataKey)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetContactInfo), new object[1]
      {
        (object) dataKey
      });
      try
      {
        return this.engine.GetContactInfo(dataKey);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (ContactInfo[]) null;
      }
    }

    public virtual AppointmentInfo[] QueryAppointments(QueryCriterion criteria)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (QueryAppointments), new object[1]
      {
        (object) criteria
      });
      try
      {
        return this.engine.QueryAppointments("", criteria);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex, this.Session.SessionInfo);
        return (AppointmentInfo[]) null;
      }
    }

    public virtual void AddContact(
      string sharerID,
      string userID,
      CSMessage.AccessLevel accessLevel,
      CSControlMessage msg)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (AddContact), new object[4]
      {
        (object) sharerID,
        (object) userID,
        (object) accessLevel,
        (object) msg
      });
      try
      {
        CSDBAccessor.AddContact(sharerID, userID, accessLevel);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
      }
    }

    public virtual void DeleteContact(string sharerID, string userID)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (DeleteContact), new object[2]
      {
        (object) sharerID,
        (object) userID
      });
      try
      {
        CSDBAccessor.DeleteContact(sharerID, userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
      }
    }

    public virtual void UpdateContact(
      string sharerID,
      string userID,
      CSMessage.AccessLevel accessLevel,
      CSControlMessage ackMsg)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (UpdateContact), new object[4]
      {
        (object) sharerID,
        (object) userID,
        (object) accessLevel,
        (object) ackMsg
      });
      try
      {
        CSDBAccessor.UpdateContact(sharerID, userID, accessLevel);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
      }
    }

    public virtual CSListInfo GetCalenderSharingContactList(string UserID, bool fromOwner)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetCalenderSharingContactList), Array.Empty<object>());
      try
      {
        return CSDBAccessor.GetCalenderSharingContactList(UserID, fromOwner);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
        return (CSListInfo) null;
      }
    }

    public virtual CSContactInfo GetCalendarContactInfo(string ownerID, string userID)
    {
      CSContactInfo calendarContactInfo = (CSContactInfo) null;
      this.onApiCalled(nameof (CalendarManager), nameof (GetCalendarContactInfo), new object[2]
      {
        (object) ownerID,
        (object) userID
      });
      try
      {
        CSListInfo sharingContactList = this.GetCalenderSharingContactList(ownerID, true);
        if (sharingContactList != null)
        {
          CSContactInfo[] csContactInfoList = sharingContactList.GetCSContactInfoList();
          if (csContactInfoList != null)
          {
            if (csContactInfoList.Length != 0)
            {
              foreach (CSContactInfo csContactInfo in csContactInfoList)
              {
                if (csContactInfo.UserID == userID)
                {
                  calendarContactInfo = csContactInfo;
                  break;
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
      }
      return calendarContactInfo;
    }

    public virtual bool ContainsContact(string sharerID, string userID)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (ContainsContact), Array.Empty<object>());
      try
      {
        return CSDBAccessor.ContainsContact(sharerID, userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
        return false;
      }
    }

    public virtual CSControlMessage[] GetStoredCSMessages(string userID)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (GetStoredCSMessages), Array.Empty<object>());
      try
      {
        return CSDBAccessor.GetStoredCSMessages(userID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
        return (CSControlMessage[]) null;
      }
    }

    public virtual void SaveCSMessage(CSControlMessage msg)
    {
      this.onApiCalled(nameof (CalendarManager), nameof (SaveCSMessage), new object[1]
      {
        (object) msg
      });
      try
      {
        CSDBAccessor.SaveCSControlMessage(msg.FromUser, msg.ToUser, msg.MsgType, msg.Text);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (CalendarManager), ex);
      }
    }
  }
}
