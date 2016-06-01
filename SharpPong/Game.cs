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

        int level;
        public Player player1, playerR;

        // Ball and paddles
        public Ball ball;       
        public float paddleSpeed = 380f;
        
        // Time
        Clock clock, timer;
        public float deltaTime;

        // Game over
        public bool loose;
        Text counting;
        public int seconds;

        //----------------------------------
        public Game()
        {

            this.level = 1;

            this.clock = new Clock();
            this.timer = new Clock();

            this.ball = new Ball(14, 300);

            // Setting players
            this.player1 = new Player("PlayerA", 0);
            this.playerR = new Player("PlayerB", 0);

            this.loose = false;
            this.seconds = DateTime.Now.Second;

            // Counting
            this.counting = new Text("", new Font("resources/robotastic.ttf"));
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
                                  
                if (!loose) // Move player paddle and ball
                    move();
                else // Time delaying when player loose  
                {

                    counting.DisplayedString = "3";

                    if (DateTime.Now.Second - seconds == 1)
                        counting.DisplayedString = "2";

                    else if (DateTime.Now.Second - seconds == 2)
                        counting.DisplayedString = "1";

                    else if (DateTime.Now.Second - seconds >= 3 || DateTime.Now.Second - seconds < 0)
                    {
                        counting.DisplayedString = "0";
                        loose = false;
                    }
   
                }

                //**********************************************************  
                // Increase the level after 15 seconds 

                if (getTime() % 15 == 0)
                    LevelUp();
   
                //**********************************************************
                // Display everything on screen

                // Background
                window.Draw(background);
                
                // Time                                                            
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
                if (loose)
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

            Time temp = timer.ElapsedTime;
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
