namespace DemoDotNetCoreApplication.Contracts
{
    /// <summary>
    /// Class IUserContext.
    /// </summary>
    public class IUserContext
    {
        public int EmployeeID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>The region.</value>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the job level.
        /// </summary>
        /// <value>The job level.</value>
        public int JobLevel { get; set; }

        /// <summary>
        /// Gets or sets the type of the user.
        /// </summary>
        /// <value>The type of the user.</value>
        public string UserType { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is approver.
        /// </summary>
        /// <value><c>true</c> if this instance is approver; otherwise, <c>false</c>.</value>
        public bool IsApprover { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is admin.
        /// </summary>
        /// <value><c>true</c> if this instance is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin { get; set; }
    }

    /// <summary>
    /// Class UserContext.
    /// Implements the <see cref="DLR.Inventory.Concerns.IUserContext" />
    /// </summary>
    /// <seealso cref="DLR.Inventory.Concerns.IUserContext" />
}
