using Chessgine;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameManager
    {
        private static GameManager instance;
        public List<Player> PlayerPool { get; set; }

        public List<GameSession> Sessions { get; set; }

        private GameManager()
        {
            this.PlayerPool = new List<Player>();
            this.Sessions = new List<GameSession>();
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void AddPlayer(Player player)
        {
            this.PlayerPool.Add(player);
        }

        public void DropPlayer(Player player)
        {
            this.PlayerPool.Remove(player);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>returns player if it is online. returns null if player is not online.</returns>
        public Player GetPlayerByConnectionId(string connectionId)
        {
            Player player = null;
            foreach (var item in PlayerPool)
            {
                if (item.ConnectionId == connectionId)
                {
                    player = item;
                }
            }
            return player;
        }

        public Player MatchMake(Player challenger)
        {
            Player enemy = null;
            int challengerRating = challenger.Rating;
            int diff = challengerRating;

            foreach (var player in PlayerPool)
            {
                // get nearest rating player
                if (Math.Abs(player.Rating - challenger.Rating) < diff && player != challenger && player.InQueue)
                {
                    enemy = player;
                }
            }
            return enemy;
        }

        public Player GetPlayerByName(string name)
        {
            Player player = null;
            foreach (var item in PlayerPool)
            {
                if (item.Name == name)
                {
                    player = item;
                }
            }
            return player;
        }

        public void StartGameFor(Player player, Player enemy)
        {
            player.InQueue = false;
            enemy.InQueue = false;

            GameSession newSession = new GameSession(new Game(20), player, enemy);
            newSession.Game.OnGameEvent += Game_OnGameEvent;
            newSession.Game.Board.OnGameEvent += Game_OnGameEvent;
            Sessions.Add(newSession);

            newSession.Game.Start();
        }

        void Game_OnGameEvent(object myObject, GameArgs myArgs)
        {
            Game currentGame = (Game)myObject;
            GameSession currentSession = GetSessionByGame(currentGame);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();

            if (myArgs.Message == "CheckMate")
            {
                Player loser = myArgs.Extra.ToLower() == "white" ? currentSession.WhitePlayer : currentSession.BlackPlayer;
                Player winner = myArgs.Extra.ToLower() != "white" ? currentSession.WhitePlayer : currentSession.BlackPlayer;

                this.AdjustRatings(winner, loser);
            }
            else if (myArgs.Message == "Draw")
            {
                // ratings does not change upon draw
            }

            hubContext.Clients.Client(currentSession.WhitePlayer.ConnectionId).GameEvent(myArgs.Message, myArgs.Extra);
            hubContext.Clients.Client(currentSession.BlackPlayer.ConnectionId).GameEvent(myArgs.Message, myArgs.Extra);
        }

        public void AdjustRatings(Player winner, Player loser)
        {
            loser.Rating -= 10;
            winner.Rating += 10;

            // could use ELO rating system
            DataManager.SetUserRating(loser.Name, loser.Rating);
            DataManager.SetUserRating(winner.Name, winner.Rating);
        }

        public int GetQueuedPlayersCount()
        {
            int num = 0;
            foreach (var item in PlayerPool)
            {
                if (item.InQueue)
                {
                    num++;
                }
            }
            return num;
        }

        public GameSession GetSessionByPlayer(Player player)
        {
            GameSession requestedGameSession = null;

            foreach (var item in Sessions)
            {
                if (item.BlackPlayer == player || item.WhitePlayer == player)
                {
                    requestedGameSession = item;
                }
            }
            return requestedGameSession;
        }

        public GameSession GetSessionByGame(Game game)
        {
            GameSession requestedGameSession = null;

            foreach (var item in Sessions)
            {
                if (item.Game == game)
                {
                    requestedGameSession = item;
                }
            }
            return requestedGameSession;
        }
    }
}
