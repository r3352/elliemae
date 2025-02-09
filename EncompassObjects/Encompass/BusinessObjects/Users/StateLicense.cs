// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.StateLicense
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Represents a Loan Officers license to originate loans in a particular state.
  /// </summary>
  /// <example>
  /// The following code enumerates the loans in which a particular Loan Officer
  /// is licensed to originate.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class UserManager
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server. We will need to be logged
  ///       // in as an Administrator to modify the user accounts.
  ///       Session session = new Session();
  ///       session.Start("myserver", "admin", "adminpwd");
  /// 
  ///       // Retrieve the "officer" user from the server
  ///       User officer = session.Users.GetUser("officer");
  /// 
  ///       // Iterate over the officer's licenses
  ///       foreach (StateLicense license in officer.StateLicenses)
  ///       {
  ///          // Only display enabled licenses
  ///          if (license.Enabled)
  ///             Console.WriteLine(license.State + ": " + license.LicenseNumber);
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class StateLicense : IStateLicense
  {
    private LOLicenseInfo license;

    internal StateLicense(LOLicenseInfo license) => this.license = license;

    /// <summary>
    /// The abbreviated state name in which the license applies.
    /// </summary>
    /// <example>
    /// The following code enumerates the loans in which a particular Loan Officer
    /// is licensed to originate.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Retrieve the "officer" user from the server
    ///       User officer = session.Users.GetUser("officer");
    /// 
    ///       // Iterate over the officer's licenses
    ///       foreach (StateLicense license in officer.StateLicenses)
    ///       {
    ///          // Only display enabled licenses
    ///          if (license.Enabled)
    ///             Console.WriteLine(license.State + ": " + license.LicenseNumber);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string State => this.license.StateAbbr;

    /// <summary>
    /// Gets or sets the license number for the current license.
    /// </summary>
    /// <example>
    /// The following code enumerates the loans in which a particular Loan Officer
    /// is licensed to originate.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Retrieve the "officer" user from the server
    ///       User officer = session.Users.GetUser("officer");
    /// 
    ///       // Iterate over the officer's licenses
    ///       foreach (StateLicense license in officer.StateLicenses)
    ///       {
    ///          // Only display enabled licenses
    ///          if (license.Enabled)
    ///             Console.WriteLine(license.State + ": " + license.LicenseNumber);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string LicenseNumber
    {
      get => this.license.License;
      set => this.license.License = value ?? "";
    }

    /// <summary>Gets or sets the expiration date for the license.</summary>
    /// <remarks>If no license expiration date is set, </remarks>
    public object ExpirationDate
    {
      get
      {
        return !(this.license.ExpirationDate == DateTime.MaxValue) ? (object) this.license.ExpirationDate : (object) null;
      }
      set
      {
        if (value == null)
          this.license.ExpirationDate = DateTime.MaxValue;
        else
          this.license.ExpirationDate = Convert.ToDateTime(value);
      }
    }

    /// <summary>Sets the expiration date for the license.</summary>
    /// <param name="value">The expiration date for the license or <c>null</c> if the
    /// license does not expire.</param>
    /// <remarks>This method is provided primarilly for COM-based clients that cannot
    /// set the ExpirationDate directly thru the <see cref="P:EllieMae.Encompass.BusinessObjects.Users.StateLicense.ExpirationDate" /> property.</remarks>
    void IStateLicense.SetExpirationDate(object value) => this.ExpirationDate = value;

    /// <summary>
    /// Gets or sets whether the current license is enabled in Encompass.
    /// </summary>
    /// <example>
    /// The following code enumerates the loans in which a particular Loan Officer
    /// is licensed to originate.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Retrieve the "officer" user from the server
    ///       User officer = session.Users.GetUser("officer");
    /// 
    ///       // Iterate over the officer's licenses
    ///       foreach (StateLicense license in officer.StateLicenses)
    ///       {
    ///          // Only display enabled licenses
    ///          if (license.Enabled)
    ///             Console.WriteLine(license.State + ": " + license.LicenseNumber);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool Enabled
    {
      get => this.license.Enabled;
      set => this.license.Enabled = value;
    }

    /// <summary>
    /// Gets or sets whether the current license is selected in Encompass.
    /// </summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.Selected&quot;]/*" />
    public bool Selected
    {
      get => this.license.Selected;
      set => this.license.Selected = value;
    }

    /// <summary>
    /// Gets or sets whether the current license is exempt in Encompass.
    /// </summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.Exempt&quot;]/*" />
    public bool Exempt
    {
      get => this.license.Exempt;
      set => this.license.Exempt = value;
    }

    /// <summary>Gets or sets the issue date for the current license.</summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.IssueDate&quot;]/*" />
    public object IssueDate
    {
      get
      {
        return !(this.license.IssueDate == DateTime.MaxValue) ? (object) this.license.IssueDate : (object) null;
      }
      set
      {
        if (value == null)
          this.license.IssueDate = DateTime.MaxValue;
        else
          this.license.IssueDate = Convert.ToDateTime(value);
      }
    }

    /// <summary>Gets or sets the start date for the current license.</summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.StartDate&quot;]/*" />
    public object StartDate
    {
      get
      {
        return !(this.license.StartDate == DateTime.MaxValue) ? (object) this.license.StartDate : (object) null;
      }
      set
      {
        if (value == null)
          this.license.StartDate = DateTime.MaxValue;
        else
          this.license.StartDate = Convert.ToDateTime(value);
      }
    }

    /// <summary>
    /// Gets or sets the license status for the current license.
    /// </summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.LicenseStatus&quot;]/*" />
    public string LicenseStatus
    {
      get => this.license.LicenseStatus;
      set => this.license.LicenseStatus = value;
    }

    /// <summary>Gets or sets the status date for the current license.</summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.StatusDate&quot;]/*" />
    public object StatusDate
    {
      get
      {
        return !(this.license.StatusDate == DateTime.MaxValue) ? (object) this.license.StatusDate : (object) null;
      }
      set
      {
        if (value == null)
          this.license.StatusDate = DateTime.MaxValue;
        else
          this.license.StatusDate = Convert.ToDateTime(value);
      }
    }

    /// <summary>
    /// Gets or sets the last checked date for the current license.
    /// </summary>
    /// <include file="StateLicense.xml" path="Examples/Example[@name=&quot;StateLicense.LastChecked&quot;]/*" />
    public object LastChecked
    {
      get
      {
        return !(this.license.LastChecked == DateTime.MaxValue) ? (object) this.license.LastChecked : (object) null;
      }
      set
      {
        if (value == null)
          this.license.LastChecked = DateTime.MaxValue;
        else
          this.license.LastChecked = Convert.ToDateTime(value);
      }
    }

    internal LOLicenseInfo Unwrap() => this.license;
  }
}
