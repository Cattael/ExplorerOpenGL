﻿using ExplorerOpenGL.Model.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerOpenGL.Controlers
{
    public class Controler
    {
        public KeyboardUtils KeyboardUtils;
        public DebugManager DebugManager; //instantiate on load
        public TextureManager TextureManager; //instantiate on load 

        List<Sprite> _sprites;
        Dictionary<string, SpriteFont> fonts;
        GraphicsDeviceManager graphics;

        public Controler(Dictionary<string, SpriteFont> Fonts, GraphicsDeviceManager Graphics, ContentManager content)
        {
            
            KeyboardUtils = new KeyboardUtils();
            DebugManager = new DebugManager(Fonts);
            TextureManager = new TextureManager(Graphics, content);
            
            fonts = Fonts;
            graphics = Graphics; 
        }

        public void Update(List<Sprite> sprites)
        {
            if(KeyboardUtils != null && TextureManager != null && DebugManager != null)
            {
                KeyboardUtils.Update();
                DebugManager.Update(sprites);
            }
            else
            {
                throw new NullReferenceException("Toutes les instances des controllers doivent être initialisées"); 
            }

        }
    }
}