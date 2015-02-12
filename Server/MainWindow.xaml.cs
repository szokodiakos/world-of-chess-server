using Chessgine;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Linq;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IDisposable SignalR { get; set; }
        const string ServerURI = "http://+:8080";

        private bool serverOnline;
        public bool ServerOnline
        {
            get
            {
                return serverOnline;
            }
            set
            {
                startSwitch.IsEnabled = false;
                serverOnline = value;

                if (serverOnline)
                {
                    log("Starting server...");
                    startSwitch.Content = "Stop server";
                    Task.Run(() => StartServer());
                }
                else
                {
                    log("Stopping server...");
                    Task.Run(() => StopServer());
                    startSwitch.IsEnabled = true;
                    startSwitch.Content = "Start server";
                    log("Server stopped.");
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            log("World of Chess GameServer");
            log("Version: " + version.ToString());
            log("---------------------");
        }

        private void startSwitch_Click(object sender, RoutedEventArgs e)
        {
            ServerOnline = !ServerOnline;
        }

        public void log(string message)
        {
            if (!(console.CheckAccess()))
            {
                this.Dispatcher.Invoke(() => log(message));
                return;
            }
            console.AppendText(message + Environment.NewLine);
            console.ScrollToEnd();
        }

        private void StartServer()
        {
            try
            {
                SignalR = WebApp.Start(ServerURI);
            }
            catch (TargetInvocationException)
            {
                log("A server is already listening at " + ServerURI + ".");
                this.Dispatcher.Invoke(() => startSwitch.IsEnabled = true);
                return;
            }
            this.Dispatcher.Invoke(() => startSwitch.IsEnabled = true);
            log("Server listening at " + ServerURI + ".");
        }

        private void StopServer()
        {
            SignalR.Dispose();
        }

        private void clearLog_Click(object sender, RoutedEventArgs e)
        {
            console.Clear();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
    /// <summary>
    /// Echoes messages sent using the Send message by calling the
    /// addMessage method on the client. Also reports to the console
    /// when clients connect and disconnect.
    /// </summary>
    public class MyHub : Hub
    {
        private Timer matchMakeTimer;

        /// <summary>
        /// Registers name in server.
        /// 
        /// If the user is not present in the DB, it will be added.
        /// </summary>
        /// <param name="name">player's name</param>
        public void RegisterName(string name)
        {
            bool playerInDb = DataManager.IsPlayerInDb(name);

            int rating = Rules.DefaultRanking;

            if (playerInDb)
            {
                rating = DataManager.GetUserRating(name);
            }
            else
            {
                DataManager.AddUser(name);
            }

            WriteToServerConsole(
            "Added player (" + name + ") with rating " + rating
            + " to pool. (ConnectionId: " + Context.ConnectionId + ")");

            GameManager.Instance.AddPlayer(new Player(name, rating, Context.ConnectionId));
        }

        public void LeaveQueue()
        {
            Player player = GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId);
            player.InQueue = false;

            WriteToServerConsole("Player (" + player.Name + ") left the queue.");
        }

        public void MatchMake()
        {
            Player player = GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId);
            player.InQueue = true;

            WriteToServerConsole(
            "Player (" + player.Name + ") joined the queue. (Total people in queue: "
            + GameManager.Instance.GetQueuedPlayersCount() + ")");

            Player enemy = GameManager.Instance.MatchMake(player);
            DoMatchMake(player);
            matchMakeTimer = new System.Timers.Timer(5000);
            matchMakeTimer.Elapsed += (sender, e) => DoMatchMake(player);
            matchMakeTimer.Enabled = true;
        }

        // x,y = 0,0 -> H1
        // x,y = 7,0 -> A1
        // x,y = 0,7 -> H8
        // x,y = 7,7 -> A8
        public void RequestMove(int fromx, int fromy, int tox, int toy)
        {
            Player currentPlayer = GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId);
            GameSession currentSession = GameManager.Instance.GetSessionByPlayer(currentPlayer);
            string fromBoardFieldCode = Board.ArrayPosition2FieldCode(7 - fromx, 7 - fromy);
            string toBoardFieldCode = Board.ArrayPosition2FieldCode(7 - tox, 7 - toy);

            WriteToServerConsole(currentPlayer.Name + " requested move: from " + fromBoardFieldCode + " to " + toBoardFieldCode + ".");

            try
            {
                if (currentPlayer == currentSession.WhitePlayer)
                {
                    currentSession.Game.WhitePlayer.SelectField(fromBoardFieldCode);
                    currentSession.Game.WhitePlayer.MoveSelectedFigureTo(toBoardFieldCode);

                }
                else
                {
                    currentSession.Game.BlackPlayer.SelectField(fromBoardFieldCode);
                    currentSession.Game.BlackPlayer.MoveSelectedFigureTo(toBoardFieldCode);
                }
                Clients.Client(currentPlayer.ConnectionId).FigureMoved(fromx, fromy, tox, toy);
                Clients.Client(currentSession.GetEnemy(currentPlayer).ConnectionId).FigureMoved(fromx, fromy, tox, toy);
            }
            catch (EnemyFigureSelectedException)
            {
                Clients.Caller.ExceptionEvent("Enemy figure was selected. Please select one of your figures.");
            }
            catch (InvalidFigureMoveException)
            {
                Clients.Caller.ExceptionEvent("This is an invalid move for this figure. Please select another field.");
            }
            catch (NoSelectedFigureForPlayerException)
            {
                Clients.Caller.ExceptionEvent("Please select a figure before moving.");
            }
            catch (OtherPlayerTurnsException)
            {
                Clients.Caller.ExceptionEvent("The other player is in turn. Please wait for your turn.");
            }
            catch (GameIsNotStartedException)
            {
                Clients.Caller.ExceptionEvent("There is no active game session at the moment.");
            }
            catch (NoFigureOnFieldException)
            {
                Clients.Caller.ExceptionEvent("There is no figure on the selected field. Please select another field.");
            }
        }

        public void RequestHighscoreData(string username)
        {
            string UsersScore = DataManager.GetUserRating(username).ToString();
            List<String> ret = DataManager.GetHighscore();
            ret.Insert(0, UsersScore);
            ret.Insert(0, username);
            Clients.Caller.HigshscoreDataArrived(ret.ToArray());
        }

        public void RequestAvailableMoves(int posx, int posy)
        {
            Player currentPlayer = GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId);
            GameSession currentSession = GameManager.Instance.GetSessionByPlayer(currentPlayer);
            string fieldCode = Board.ArrayPosition2FieldCode(7 - posx, 7 - posy);

            List<string> validFields = currentSession.Game.Rules.GetValidFields(currentSession.Game.Board.GetFigureAtFieldCode(fieldCode));

            int[,] validFieldsArrayCodes = new int[validFields.Count, 2];
            for (int i = 0; i < validFields.Count; i++)
            {
                validFieldsArrayCodes[i, 0] = 7 - Board.FieldCode2ArrayPosition(validFields[i]).Item1;
                validFieldsArrayCodes[i, 1] = 7 - Board.FieldCode2ArrayPosition(validFields[i]).Item2;
            }

            Clients.Caller.AvailableMoves(validFieldsArrayCodes);
        }

        private void DoMatchMake(Player player)
        {
            Player enemy = GameManager.Instance.MatchMake(player);

            if (GameManager.Instance.PlayerPool.Contains(player) && player.InQueue && enemy != null)
            {
                WriteToServerConsole("Game found: " + player.Name + " versus " + enemy.Name + ".");

                // start the game
                Clients.Client(player.ConnectionId).GameStarts("WhitePlayer");
                Clients.Client(enemy.ConnectionId).GameStarts("BlackPlayer");
                GameManager.Instance.StartGameFor(player, enemy);

                // stop timer if exists
                if (matchMakeTimer != null)
                {
                    matchMakeTimer.Enabled = false;
                }
            }
            else if (GameManager.Instance.PlayerPool.Contains(player) && player.InQueue && enemy == null)
            {
                // recall DoMatchMake (No operation)
            }
            else if (!GameManager.Instance.PlayerPool.Contains(player) || !player.InQueue)
            {
                // stop timer
                matchMakeTimer.Enabled = false;
            }
        }

        public void Yield()
        {
            Player yielder = GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId);
            GameSession finishedSession = GameManager.Instance.GetSessionByPlayer(yielder);

            Player winner = null;
            Player loser = null;

            if (yielder == finishedSession.BlackPlayer)
            {
                winner = finishedSession.WhitePlayer;
                loser = finishedSession.BlackPlayer;
            }
            else
            {
                winner = finishedSession.BlackPlayer;
                loser = finishedSession.WhitePlayer;
            }

            if (finishedSession.Game.GameStatus)
            {
                finishedSession.Game.GameStatus = false;
                GameManager.Instance.AdjustRatings(winner, loser);
                Clients.Client(winner.ConnectionId).YieldWinner();
            }
        }

        public override Task OnConnected()
        {
            WriteToServerConsole("Client connected: " + Context.ConnectionId);


            bool playerAlreadyOnline = GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId) != null;

            if (!playerAlreadyOnline)
            {
                Clients.Caller.Connected();
            }

            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            WriteToServerConsole("Client disconnected: " + Context.ConnectionId);

            GameManager.Instance.DropPlayer(GameManager.Instance.GetPlayerByConnectionId(Context.ConnectionId));

            return base.OnDisconnected(stopCalled);
        }

        private void WriteToServerConsole(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            ((MainWindow)Application.Current.MainWindow).log(message));
        }
    }
}
