using Prueba.Core.Entities;

namespace Prueba.Core.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}