using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using SFML.Graphics;

namespace SharpPong
{
    static class ResourceManager
    {

        static Hashtable fonts = new Hashtable();
        static Hashtable textures = new Hashtable();
        
        //-------------------------------------------------
        static public Font GetFont(string font_path)
        {

            if (fonts.ContainsKey(font_path))
                return (Font)fonts[font_path];
            else
            {
                Font new_font = new Font(font_path);
                fonts.Add(font_path, new_font);

                return new_font;
            }

        }
        //-------------------------------------------------
        static public Texture GetTexture(string texture_path)
        {

            if (textures.ContainsKey(texture_path))
                return (Texture)textures[texture_path];
            else
            {
                Texture new_texture = new Texture(texture_path);
                textures.Add(texture_path, new_texture);

                return new_texture;
            }

        }
        //-------------------------------------------------

    }
}
