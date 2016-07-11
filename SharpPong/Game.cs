using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;


namespace SharpPong
{
    class Game
    {
        // Game
        public RenderWindow window;
        public bool gameOn;
        public bool running;

        // Time
        private Clock clock;
        protected Clock timer;
        private Time timeAdd;
        private bool pause;
        public int seconds;
        public float deltaTime;

        // Players and ball
        public Player playerL, playerR;
        public float paddleSpeed = 550f;
        public Ball ball;       

        // Objects to display
        protected Text counting, time, score;
        RectangleShape background;

        //----------------------------------
        public Game(RenderWindow rw)
        {
            // Game
            this.window = rw;
            this.gameOn = true;
            this.running = false;            

            // Time
            this.clock = new Clock();
            this.timer = new Clock();
            this.timeAdd = Time.Zero;
            this.pause = false;
            this.seconds = DateTime.Now.Second;

            // Setting players and ball
            this.playerL = new Player("PlayerA", 0);
            this.playerR = new Player("PlayerB", 0);
            this.ball = new Ball(14, 300);


            Font robotasticF = ResourceManager.GetFont("resources/robotastic.ttf");
            // Text - Counting
            this.counting = new Text("", robotasticF);
            counting.Position = new Vector2f(Settings.WIDTH / 2 - 15, Settings.HEIGHT / 2 - 50);
            counting.CharacterSize = 50;
            counting.Color = new Color(255, 255, 255, 170);

            // Text - Time
            this.time = new Text("0.0", robotasticF);
            time.Position = new Vector2f(Settings.WIDTH - 90, 10);
            time.CharacterSize = 18;
            time.Color = new Color(255, 255, 255, 170);

             // Text - Score
            score = new Text("0:0", robotasticF);
            score.Position = new Vector2f(Settings.WIDTH / 2 - 30, 10);
            score.CharacterSize = 22;
            score.Color = new Color(255, 255, 255, 170);

            // Background
            this.background = new RectangleShape(new Vector2f(Settings.WIDTH, Settings.HEIGHT));
            background.Texture = ResourceManager.GetTexture("resources/textures/background.png");
            background.Texture.Repeated = true;
            background.TextureRect = new IntRect(0, 0, (int)Settings.WIDTH, (int)Settings.HEIGHT);

        }
   
        // Handling paddles and ball movement.
        protected virtual void Move() { }

        // Displaying objects for specific game mode. 
        protected virtual void PostRender() { }
        //----------------------------------
        // Running the game.
        public void RunGame()
        {
                      
            // Game main loop.
            while (window.IsOpen)
            {                

                // Process window events.
                window.DispatchEvents();

                // Back to the menu if Escape was pressed. 
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) && window.HasFocus())
                {
                    EscMenu();
                }

                deltaTime = clock.Restart().AsSeconds();
                //**********************************************************              
                if (gameOn)
                {

                    if (running)
                    {
                        // Move player paddle and ball.
                        Move();
                        pause = false;
                        seconds = DateTime.Now.Second;

                    }
                    // Time delaying when player loose.
                    else
                    {
                        
                        // Remember the time when the counting starts.
                        if (!pause)
                        {
                            timeAdd = timer.ElapsedTime + timeAdd;
                            pause = true;
                        }
                        
                        int countingNumber = 3 - Math.Abs(DateTime.Now.Second - seconds);
                        counting.DisplayedString = countingNumber.ToString();

                        if (countingNumber == 0 || countingNumber < 0)
                        {
                            counting.DisplayedString = "0";
                            running = true;
                            timer.Restart();
                        }

                    }

                }
                else
                {

                    timer.Restart();
                    timeAdd = Time.Zero;
                    seconds = DateTime.Now.Second;

                }

                RenderGame();

            }       

        }
        //----------------------------------
        // Returning game time. 
        protected double GetTime()
        {

            Time temp = timer.ElapsedTime + timeAdd;
            double time = Math.Round(temp.AsSeconds(), 2);
             
            return time;

        }
        //----------------------------------
        // Display everything on screen.
        private void RenderGame()
        {
            // Background.
            window.Draw(background);

            // Ball.
            window.Draw(ball.ballShape);

            // Objects for specific game mode. 
            PostRender();

            // Counting. 
            if (!running && gameOn) 
                window.Draw(counting);

            // Update the window.
            window.Display();

        }
        //----------------------------------
        protected virtual void EscMenu()
        {

            Menu menu = new Menu(window);

        }
        //----------------------------------
    }
}
