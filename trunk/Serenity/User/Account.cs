using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.User
{
    public class Account
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public byte WorldId { get; set; }
        public byte ChannelId { get; set; }

        public string LastIP { get; set; }

        public byte Gender { get; set; }

        public byte LoggedIn { get; set; }
        public bool GM { get; set; }

        public bool Banned { get; set; }
        public string BanReason { get; set; }

        public List<Character> Characters { get; set; }

        public Account()
        {
            
        }

        public void Load()
        {
            Master.Instance.Accessor.LoadAccount(this);
        }

        public void Save()
        {
            //Program.Accessor.SaveAccount(this);
        }

        public void LoadCharacters()
        {
            Master.Instance.Accessor.LoadCharacters(this);
        }
    }
}
