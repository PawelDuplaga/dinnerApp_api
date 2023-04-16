using HotDinner.Application.Common.Interfaces.Persistence;
using HotDinner.Domain.Entities;

namespace HotDinner.Infrastructure.Persistence;


public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetByEmail(string email)
    {
        return _users.SingleOrDefault(x => x.Email == email);
    }
}