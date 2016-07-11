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
        
        // Tile size.
        public int sizeX, sizeY;
        // Quantity of tiles in row and column.
        public int xTab, yTab;

        public char[,] tileMap;
        public RectangleShape[,] tiles;

        public Tiles(int sizeX, int sizeY)
        {

            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.xTab = (int)(Settings.WIDTH) / sizeX;
            this.yTab = (int)(Settings.HEIGHT / 2) / sizeY;

            this.tileMap = new char[xTab, yTab];
            this.tiles = new RectangleShape[xTab, yTab];

        }
//-----------------------------------------------------------------------------------------------
        // Read tiles from file. 
        public void ReadTiles()
        {

            StreamReader reader = new StreamReader("resources/map.txt");
            int posX, posY = 0;

            for (int y = 0; y < yTab; y++) 
            {

                posX = 0;

                for (int x = 0; x < xTab; x++)
                {

                    char ch = (char)reader.Read();
                    tileMap[x, y] = ch;

                    RectangleShape tile = new RectangleShape(new Vector2f(sizeX, sizeY));
                    tile.Position = new Vector2f(posX, posY);

                    if (ch == '1')
                    {                       
                        tile.Texture = ResourceManager.GetTexture("resources/textures/brick0.png");
                    }
                    else if (ch == '2')
                    {
                        tile.Texture = ResourceManager.GetTexture("resources/textures/brick3.png");
                    }

                    tiles[x, y] = tile;

                    posX += sizeX;

                }

                posY += sizeY;

            }

            reader.Close();
            reader.Dispose();

        }
//-----------------------------------------------------------------------------------------------
    }
}
