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
        //public int hits;

        public Tiles(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.tileTexture = new Texture("textures/tile.png");
            tileTexture.Smooth = true;

            this.xTab = (int)Math.Floor((double)((Settings.WIDTH - 20) / sizeX));
            this.yTab = (int)Math.Floor((double)((Settings.HEIGHT / 2) / sizeY));

            this.tileMap = new int[xTab, yTab];
            this.tiles = new RectangleShape[xTab, yTab];
        }
//-----------------------------------------------------------------------------------------------
        // Read tiles from file 
        public void readTiles()
        {
            char ch;
            int posX = 0, posY = 10;
            StreamReader reader = new StreamReader("map.txt");
           
            for (int y = 0; y < yTab; y++) 
            {
                posY += sizeY;
                posX = 10;

                for (int x = 0; x < xTab; x++)
                {
                    ch = (char)reader.Read();
                    int temp = Convert.ToInt32(ch);
                    tileMap[x, y] = temp;

                    RectangleShape tile = new RectangleShape(new Vector2f(sizeX, sizeY));
                    tile.Position = new Vector2f(posX, posY);

                    if (temp == 49)   
                        tile.Texture = tileTexture;

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
