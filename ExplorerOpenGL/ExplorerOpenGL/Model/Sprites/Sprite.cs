﻿using ExplorerOpenGL.Controlers;
using ExplorerOpenGL.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerOpenGL.Model.Sprites
{
    public class Sprite
    {
        public bool IsRemove { get; set; }
        public Vector2 Position;
        protected Texture2D _texture;
        public float Radian { get; set; }
        public Rectangle HitBox { get {
                if (_texture != null) return new Rectangle((int)Position.X - (int)origin.X, (int)Position.Y - (int)origin.Y, _texture.Width, _texture.Height);
                else return new Rectangle((int)Position.X, (int)Position.Y, 1, 1); 
            } }
        protected Vector2 origin;
        public SpriteEffects Effects { get; set; }
        public float  layerDepth { get; set; }
        protected float scale { get; set; }
        protected float opacity { get; set; }

        public Sprite()
        {
            scale = 1;
            opacity = 1f; 
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites, Controler controler)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if(_texture != null && !(this is MousePointer))
                spriteBatch.Draw(_texture, Position, null, Color.White * opacity, Radian, origin, scale, Effects, layerDepth);
        }
    }
}
