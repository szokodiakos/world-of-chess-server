using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Chessgine
{
    public class ChessClock
    {
        public Timer Timer { get; private set; }
        public int BlackSecondsRemaining { get; private set; }
        public int WhiteSecondsRemaining { get; private set; }

        private Color currentColor;

        private Game game;

        public ChessClock(int minutes, Game game)
        {
            this.game = game;
            WhiteSecondsRemaining = BlackSecondsRemaining = minutes * 60;
            currentColor = Color.WHITE;
            Timer = new Timer();
            Timer.Interval = 1000;
            Timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            switch (currentColor)
            {
                case Color.BLACK:
                    BlackSecondsRemaining -= 1;
                    if (BlackSecondsRemaining < 0)
                    {
                        this.game.gameFinished(Color.WHITE);
                    }
                    break;
                case Color.WHITE:
                    WhiteSecondsRemaining -= 1;
                    if (WhiteSecondsRemaining < 0)
                    {
                        this.game.gameFinished(Color.BLACK);
                    }
                    break;
                default:
                    break;
            }
        }
        public void Start()
        {
            //Timer.Enabled = true;
        }

        public void Hit()
        {
            //Timer.Enabled = false;

            switch (currentColor)
            {
                case Color.BLACK:
                    currentColor = Color.WHITE;
                    break;
                case Color.WHITE:
                    currentColor = Color.BLACK;
                    break;
                default:
                    break;
            }

            //Timer.Enabled = true;
        }

        public void Stop()
        {
            //Timer.Enabled = false;
        }
    }
}
