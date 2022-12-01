
using System.Xml.Serialization;
namespace Domain.Entities.People;

[Serializable]
public class User : Human
{
    public string Login { get => _login; set => _login = value; }
    public string Password { get => _password; set => _password = value; }
    protected string _login;
    protected string _password;
    public int Id; 

    public User(string name, string surname,  string login, string password, int id) : base(name, surname)
    {
        _login = new string(login);
        _password = new string(password);
        Id = id;
    }

    public User() : base()
    {
        _login = new string("login");
        _password = new string("password");
        Id = 0;
    }


}
