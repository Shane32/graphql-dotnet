using System;
using System.Collections.Generic;

namespace GraphQL.StarWars.Types
{
    public abstract class StarWarsCharacter
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Friends { get; set; }
        public int[] AppearsIn { get; set; }
        public AppearsInEnum AppearsInFlags { get; set; }
        public string Cursor { get; set; }
    }

    public class Human : StarWarsCharacter
    {
        public string HomePlanet { get; set; }
    }

    public class Droid : StarWarsCharacter
    {
        public string PrimaryFunction { get; set; }
    }

    [Flags]
    public enum AppearsInEnum
    {
        Episode1 = 0x1,
        Episode2 = 0x2,
        Episode3 = 0x4,
        Episode4 = 0x8,
        Episode5 = 0x10,
        Episode6 = 0x20,
    }
}
