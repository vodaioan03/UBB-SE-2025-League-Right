using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duo.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }

    public User(int userId, string username)
    {
        UserId = userId;
        Username = username;
    }

    public User(string username)
    {
        Username = username;
    }
}