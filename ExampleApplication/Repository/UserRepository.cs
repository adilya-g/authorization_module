using ExampleApplication.Entity;

namespace ExampleApplication.Repository;

public class UserRepository
{
    private List<User> _users = new List<User>();
    private int? _increment = 0;

    public void AddUser(User user)
    {
        recountIncrement();            //инкремент пересчитывается, либо инициализируется
        user.UserId = _increment ?? 1; //на самом деле инкремент на этом этапе не может быть null,
                                      //пришлось захардкодить чтобы не ругался компилятор
        _users.Add(user);
    }

    public bool RemoveUser(User user)
    {
        if (_users.Contains(user))
        {
            _users.Remove(user);
            return true;
        }
        return false;   
    }

    public void UpdateUser(User user)
    {
        _users[_users.FindIndex(u => u.UserId == user.UserId)] = user;
    }

    public User? FindUserById(int userId)
    {
        return _users.Find(user => user.UserId == userId);
    }
    
    public User? FindUserByEmail(string email)
    {
        return _users.Find(user => user.Email == email);
    }

    private void recountIncrement()
    {
        if(_increment != null)
            _increment++;
        else
        {
            _users.Sort();
            _increment = _users.Last().UserId + 1;
        }
    }
}