namespace ResumeTech.Common.Auth; 

public interface IUserDetailsProvider {
    public UserDetails CurrentUser { get; }
    public UserId CurrentUserId { get; }
    public void Set(UserDetails userDetails);
}