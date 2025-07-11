namespace AuditForge.Core.Application.Interfaces
{
    /// <summary>
    /// Represents a provider that supplies the current user's identity.
    /// </summary>
    public interface IUserProvider
    {
        /// <summary>
        /// Gets the unique identifier of the current user.
        /// </summary>
        /// <returns>User identifier as string (e.g. user ID or username).</returns>
        string GetUserId();
    }
}
