﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.org
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
namespace Roguelancer.Particle.ParticleSystem {
    public interface IParticleSystem {
        Texture2D Texture { get; }
        Vector2 TextureOrigin { get; }
        bool Enabled { get; }
        int Capacity { get; }
        int ParticleCount { get; }
        IParticle this[int index] { get; }
        bool RemoveAt(int index);
        void Clear();
    }
}