using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace SharpPong
{
    class Tiles
    {
        public int[,] tileMap;
        public int xTab, yTab;
        public RectangleShape[,] tiles;
        public int sizeX, sizeY; // tile size
        Texture tileTexture; 

        public Tiles(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.xTab = (int)Math.Floor((double)((Settings.WIDTH - 20) / sizeX));
            this.yTab = (int)Math.Floor((double)((Settings.HEIGHT / 2) / sizeY));

            this.tileMap = new int[xTab, yTab];
            this.tiles = new RectangleShape[xTab, yTab];
        }
//-----------------------------------------------------------------------------------------------
        // Read tiles from file 
        public void readTiles()
        {

            int posX = 0, posY = 10;
            StreamReader reader = new StreamReader("map.txt");
           
            for (int y = 0; y < yTab; y++) 
            {
                posY += sizeY;
                posX = 10;

                for (int x = 0; x < xTab; x++)
                {
                    char ch = (char)reader.Read();
                    int temp = Convert.ToInt32(ch);
                    tileMap[x, y] = temp;

                    RectangleShape tile = new RectangleShape(new Vector2f(sizeX - 5, sizeY - 5));
                    tile.Position = new Vector2f(posX, posY);

                    if (temp == 49)
                    {
                        tileTexture = new Texture("textures/brick0.png");
                        tile.Texture = tileTexture;
                    }
                    else if (temp == 50)
                    {
                        tileTexture = new Texture("textures/brick1.png");
                        tile.Texture = tileTexture;
                    }

                    tiles[x, y] = tile;

                    posX += sizeX;
                }
            }

            reader.Close();
            reader.Dispose();
        }
//-----------------------------------------------------------------------------------------------
        // Generate new map with tiles
        public void generateTiles()
        {

        }
//-----------------------------------------------------------------------------------------------
    }
}
