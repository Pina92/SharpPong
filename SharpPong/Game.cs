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
        private int level;

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
        Text counting, time, score;
        RectangleShape background;

        //----------------------------------
        public Game(RenderWindow rw)
        {
            // Game
            this.window = rw;
            this.gameOn = true;
            this.running = false;
            this.level = 1;

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
            background.Texture = ResourceManager.getTexture("resources/textures/background.png");
            background.Texture.Repeated = true;
            background.TextureRect = new IntRect(0, 0, (int)Settings.WIDTH, (int)Settings.HEIGHT);

        }
        //----------------------------------
        // Increase game level
        public void LevelUp()
        {

            level++;
            ball.speed += 5;

        }
        //----------------------------------     
        // Handling paddles and ball movement
        public virtual void move() { }

        // Displaying paddles, objects for specific game, etc. 
        public virtual void postRender() { }
        //----------------------------------
        // Running game
        public void run()
        {
                      
            // Game loop
            while (window.IsOpen)
            {
                
                deltaTime = clock.Restart().AsSeconds();

               // Back to the menu if Escape was pressed 
               // TO-DO: Esc -> Resume, Menu, Exit
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    Menu menu = new Menu(window);
                }

                // Process events
                window.DispatchEvents();

                //**********************************************************              
                if (gameOn)
                {
                    if (running)
                    { // Move player paddle and ball
                        move();
                        pause = false;
                        seconds = DateTime.Now.Second;
                    }
                    else
                    { // Time delaying when player loose
                        if (!pause)
                        {
                            timeAdd = timer.ElapsedTime + timeAdd;
                            pause = true;
                        }

                        int countingNumber = 3 - Math.Abs(DateTime.Now.Second - seconds);
                        counting.DisplayedString = countingNumber.ToString();
                        
                        if(countingNumber == 0 || countingNumber < 0)
                        {
                            counting.DisplayedString = "0";
                            running = true;
                            timer.Restart();
                        }
                    }
                }
                else
                {
                    if (!pause)
                    {
                        timeAdd = timer.ElapsedTime + timeAdd;
                        pause = true;
                    }
                    seconds = DateTime.Now.Second;
                }
                //**********************************************************  
                // Increase the level after 15 seconds 

                if (getTime() % 15 == 0 && running)
                    LevelUp();

                //**********************************************************
                // Display everything on screen
                renderGame();
            }

        }
        //----------------------------------
        // Returning game time 
        public double getTime()
        {

            Time temp = timer.ElapsedTime + timeAdd;
            double time = Math.Round(temp.AsSeconds(), 2);
             
            return time;

        }
        //----------------------------------
        private void renderGame()
        {
            // Background
            window.Draw(background);

            // Time    
            if (running)
                time.DisplayedString = getTime().ToString();

            window.Draw(time);

            // Ball
            window.Draw(ball.ballShape);

            // Player's score
            score.DisplayedString = playerL.score.ToString() + " : " + playerR.score.ToString();
            window.Draw(score);

            // Paddles, objects for specific game, etc. 
            postRender();

            // Counting 
            if (!running)
                window.Draw(counting);

            // Update the window
            window.Display();

        }
        //----------------------------------
    }
}
