﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace FerrumModules.Engine
{
    public static class FE_Assets
    {
        public static Dictionary<string, Texture2D> Textures { get; private set; } = new Dictionary<string, Texture2D>();

        public static void AddTexture(Texture2D texture, string name)
        {
            if (Textures.ContainsKey(name)) throw new Exception("Two or more textures had the same name.");
            Textures[name] = texture;
        }
    }
}
