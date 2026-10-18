using System.Security.Cryptography;
using System.Text;
using TaskFlow.Domain.Services;

namespace TaskFlow.Infrastructure.Security;

public class Sha256PasswordHasher : IPasswordHasher
{
    public string Hash(string senha)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
        return Convert.ToBase64String(bytes);
    }

    public bool Verificar(string senha, string hash) 
        => Hash(senha) == hash;
}
