using UdemyAppProject.Entities;

namespace UdemyAppProject.Interfaces
{
    public interface ITokenService
    {
        string CreateTokoen(AppUser user);
    }
}
