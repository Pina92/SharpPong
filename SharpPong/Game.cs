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
        public bool gameOn;
        int level;
        public Player player1, playerR;
        public bool running;
       
        // Ball and paddles
        public Ball ball;       
        public float paddleSpeed = 380f;
        
        // Time
        Clock clock, timer;
        Time timeAdd;
        public float deltaTime;
        bool pause;

        // Game over
        Text counting;
        public int seconds;

        //----------------------------------
        public Game()
        {
            this.gameOn = true;
            this.level = 1;
            this.running = false;

            this.clock = new Clock();
            this.timer = new Clock();
            this.timeAdd = Time.Zero;
            this.pause = false;

            this.ball = new Ball(14, 300);

            // Setting players
            this.player1 = new Player("PlayerA", 0);
            this.playerR = new Player("PlayerB", 0);

            this.seconds = DateTime.Now.Second;

            // Counting
            this.counting = new Text("", Settings.robotasticF);
            counting.Position = new Vector2f(Settings.WIDTH / 2 - 15, Settings.HEIGHT / 2 - 50);
            counting.CharacterSize = 50;
            counting.Color = new Color(255, 255, 255, 170);

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
        public virtual void displayRest(RenderWindow window) { }
        //----------------------------------
        // Running game
        public void run(RenderWindow window, Text time, Text score, RectangleShape background, int type)
        {
            
            // Game loop
            while (window.IsOpen)
            {
                
                deltaTime = clock.Restart().AsSeconds();

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

                // Background
                window.Draw(background);

                // Time    
                if (running)
                   time.DisplayedString = getTime().ToString();

                window.Draw(time);

                // Ball
                CircleShape ballObject = getBall();
                window.Draw(ballObject);

                // Player's score
                score.DisplayedString = player1.score.ToString() + " : " + playerR.score.ToString();
                window.Draw(score);

                // Paddles, objects for specific game, etc. 
                displayRest(window);

                // Counting 
                if (!running)
                    window.Draw(counting);

                //**********************************************************

                // Update the window
                window.Display();

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
        // Returning ball
        public CircleShape getBall()
        {
            
            return ball.ballShape;

        }
        //----------------------------------
    }
}
