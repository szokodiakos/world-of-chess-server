using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Player
    {
        public string Name { get; set; }
        public int Rating { get; set; }
        public string ConnectionId { get; set; }

        public bool InQueue { get; set; }

        public Player(string name, int rating, string connectionId)
        {
            this.Name = name;
            this.Rating = rating;
            this.ConnectionId = connectionId;
            this.InQueue = false;
        }
    }
}
