using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factories_And_Guns
{
    class General()
    {
        static public Dictionary<string, SunSystem> SunSystems { get; set; } = [];
        static public SunSystem CurrentSunSystem { get; set; } = null;

        static public void Update(float dt, Keys[] keys, MouseState mouse)
        {
            CurrentSunSystem?.Update(dt, keys, mouse);
        }
    }

    class SunSystem(string name)
    {
        public Dictionary<string, Planet> Planets { get; set; } = [];
        public string Name { get; set; } = name;
        public Planet CurrentPlanet { get; set; } = null;

        public void Update(float dt, Keys[] keys, MouseState mouse)
        {
            CurrentPlanet?.Update(dt, keys, mouse);
        }
    }

    class Planet(int orbit, Dictionary<string, Planet> satellites, float heightOrbit)
    {
        public Dictionary<string, Field> Fields { get; set; } = [];
        public Dictionary<string, Planet> Satellites { get; set; } = satellites;
        public int Orbit { get; set; } = orbit;
        public float HeightOrbit { get; set; } = heightOrbit;
        public Field CurrentField { get; set; } = null;

        public void Update(float dt, Keys[] keys, MouseState mouse)
        {
            CurrentField?.Update(dt, keys, mouse);
        }
    }
}
