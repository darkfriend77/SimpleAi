using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HsProtoSim.Model;

namespace SimpleAi.Meta
{
    internal class Decks
    {
        public static List<Card> ShamanAggro => new List<Card>()
        {
            Cards.FromName("Mirror Image"),
        };

        public static List<Card> WarriorPirate => new List<Card>()
        {
            Cards.FromName("N'Zoth's First Mate"),
            Cards.FromName("N'Zoth's First Mate"),
            Cards.FromName("Upgrade!"),
            Cards.FromName("Upgrade!"),
            Cards.FromName("Fiery War Axe"),
            Cards.FromName("Fiery War Axe"),
            Cards.FromName("Heroic Strike"),
            Cards.FromName("Heroic Strike"),
            Cards.FromName("Bloodsail Cultist"),
            Cards.FromName("Bloodsail Cultist"),
            Cards.FromName("Frothing Berserker"),
            Cards.FromName("Frothing Berserker"),
            Cards.FromName("Kor'kron Elite"),
            Cards.FromName("Kor'kron Elite"),
            Cards.FromName("Arcanite Reaper"),
            Cards.FromName("Arcanite Reaper"),
            Cards.FromName("Patches the Pirate"),
            // TODO Cards.FromName("Sir Finley Mrrgglton"),
            Cards.FromName("Small-Time Buccaneer"),
            Cards.FromName("Small-Time Buccaneer"),
            Cards.FromName("Southsea Deckhand"),
            Cards.FromName("Southsea Deckhand"),
            Cards.FromName("Bloodsail Raider"),
            Cards.FromName("Bloodsail Raider"),
            Cards.FromName("Southsea Captain"),
            Cards.FromName("Southsea Captain"),
            Cards.FromName("Dread Corsair"),
            Cards.FromName("Dread Corsair"),
            Cards.FromName("Naga Corsair"),
            Cards.FromName("Naga Corsair"),
        };

        public static List<Card> SimpleMage => new List<Card>()
        {
            Cards.FromName("Mirror Image"),
            Cards.FromName("Sorcerer's Apprentice"),
            Cards.FromName("Ice Lance"),
            Cards.FromName("Frostbolt"),
            Cards.FromName("Ironforge Rifleman"),
            Cards.FromName("Mirror Image"),
            Cards.FromName("Ironforge Rifleman"),
            Cards.FromName("Gnomish Inventor"),
            Cards.FromName("Fireball"),
            Cards.FromName("Fireball"),
            Cards.FromName("Gnomish Inventor"),
            Cards.FromName("Ice Lance"),
            Cards.FromName("Sorcerer's Apprentice"),
            Cards.FromName("Frostwolf Grunt"),
            Cards.FromName("Raid Leader"),
            Cards.FromName("Kobold Geomancer"),
            Cards.FromName("Stormpike Commando"),
            Cards.FromName("Sen'jin Shieldmasta"),
            Cards.FromName("Elven Archer"),
            Cards.FromName("Frostwolf Grunt"),
            Cards.FromName("Stormpike Commando"),
            Cards.FromName("Elven Archer"),
            Cards.FromName("Stormwind Champion"),
            Cards.FromName("Frostwolf Warlord"),
            Cards.FromName("Voodoo Doctor"),
            Cards.FromName("Raid Leader"),
            Cards.FromName("Kobold Geomancer"),
            Cards.FromName("Sen'jin Shieldmasta"),
            Cards.FromName("Voodoo Doctor"),
            Cards.FromName("Frostwolf Warlord")
        };
    }
}
