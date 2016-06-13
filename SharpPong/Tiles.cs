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
        public char[,] tileMap;
        public int xTab, yTab;
        public RectangleShape[,] tiles;
        public int sizeX, sizeY; // tile size

        public Tiles(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.xTab = (int)(Settings.WIDTH - 20) / sizeX;
            this.yTab = (int)(Settings.HEIGHT / 2) / sizeY;

            this.tileMap = new char[xTab, yTab];
            this.tiles = new RectangleShape[xTab, yTab];
        }
//-----------------------------------------------------------------------------------------------
        // Read tiles from file 
        public void readTiles()
        {

            int posX = 0, posY = 10;
            StreamReader reader = new StreamReader("resources/map.txt");
           
            for (int y = 0; y < yTab; y++) 
            {
                posY += sizeY;
                posX = 10;

                for (int x = 0; x < xTab; x++)
                {
                    char ch = (char)reader.Read();
                    tileMap[x, y] = ch;

                    RectangleShape tile = new RectangleShape(new Vector2f(sizeX - 5, sizeY - 5));
                    tile.Position = new Vector2f(posX, posY);

                    if (ch == '1')
                    {                       
                        tile.Texture = ResourceManager.getTexture("resources/textures/brick0.png");
                    }
                    else if (ch == '2')
                    {
                        tile.Texture = ResourceManager.getTexture("resources/textures/brick1.png");
                    }

                    tiles[x, y] = tile;

                    posX += sizeX;
                }
            }

            reader.Close();
            reader.Dispose();
        }
//-----------------------------------------------------------------------------------------------
    }
}
