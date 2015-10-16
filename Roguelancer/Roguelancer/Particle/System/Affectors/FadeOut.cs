﻿// Roguelancer 0.1 Pre Alpha by Leon Aiossa
// http://www.team-nexgen.org
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tranquillity;
using Roguelancer.Particle.ParticleSystem;
using Roguelancer.Interfaces;

namespace Tranquillity {
    public class Fadeout : IParticleAffector {
        public void Affect(GameTime gameTime, DynamicParticle particle) {
            if(particle.Age.HasValue) {
                particle.Color = Color.Lerp(particle.InitialColor, new Color(particle.InitialColor.R, particle.InitialColor.G, particle.InitialColor.B, 0), 1.0f - particle.Age.Value);
            }
        }
    }
}
