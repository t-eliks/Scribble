namespace Scribble.Logic
{
    using System;
    using System.Collections.Generic;

    public class FieldGenerator
    {
        private static Random r;

        private FieldGenerator()
        {
            r = new Random();
        }

        public static FieldGenerator Instance { get; } = new FieldGenerator();

        public string GenerateRandomCharacterName()
        {
            var names = new List<string>()
            {
                "Kaladin",
                "Shallan",
                "Jaskier",
                "Deangelo Trumbull",
                "Blaine Sackett",
                "Elida Claycomb",
                "Madalene Oloughlin",
                "Jerald Garibay",
                "Ali Brunk",
                "Henrietta Vescio",
                "Tresa Cornman",
                "Ginger Meinecke",
                "Monroe Wooster"
            };

            return names[r.Next(0, names.Count)];
        }

        public string GenerateRandomDescription()
        {
            var descriptions = new List<string>()
            {
                "Azeroth is the name of the world in which the majority of the Warcraft series is set. At its core dwells a slumbering world-soul, " +
                "the nascent spirit of a titan. Long ago, it was invaded by the Old Gods, eldritch abominations from the Void. When the Pantheon arrived, " +
                "the titans imprisoned the Old Gods deep beneath the earth, before healing and ordering the world, and seeding life across the planet. " +
                "A great fount of magic that would nurture the land was placed at the center of Kalimdor, known as the Well of Eternity.",
                "The Summoner's Rift is the most commonly used Field of Justice. The map was given a graphical and technical update on May 23rd, " +
                "2012 and remade from scratch on November 12th, 2014.",
                "An accomplished spearman and a natural leader, he eventually becomes the captain of Elhokar Kholin's King's Guard, " +
                "formerly known as the Cobalt Guard, House Kholin's personal honor guard.",
                "Daughter of the recently deceased Brightlord Lin Davar of Jah Keved, Shallan pursued and received scholarly training as the ward of Jasnah Kholin.",
                "Julian Alfred Pankratz, Viscount de Lettenhove, better known as Dandelion/Jaskier, was a poet, minstrel, bard, and close friend of Geralt of Rivia."
            };

            return descriptions[r.Next(0, descriptions.Count)];
        }

        public string GenerateRandomLine()
        {
            var lines = new List<string>()
            {
                "Big place, full of nerds.",
                "Washed up players, wishing someone turned back time to 2013",
                "Warrior, Main",
                "Save the world, of course!",
                "Lightweaver, Main",
                "Solve puzzles",
                "Poet, Drunk, Secondary",
                "Get drunk and be an annoying stuck up cunt."
            };

            return lines[r.Next(0, lines.Count)];
        }
    }
}
