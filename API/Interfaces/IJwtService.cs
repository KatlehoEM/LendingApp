namespace API.Interfaces;

using API.Entities;


public interface IJwtService
{
    string GenerateToken(User user);
}

