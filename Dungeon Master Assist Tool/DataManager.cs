using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Dungeon_Master_Assist_Tool
{
    
    public class DNDState
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("meta")]
        public string Metadata { get; set; }

        [JsonProperty("Armor Class")]
        public string ArmorClass { get; set; }

        [JsonProperty("Hit Points")]
        public string HitPoints { get; set; }

        [JsonProperty("Speed")]
        public string Speed { get; set; }

        [JsonProperty("STR")]
        public string Strength { get; set; }

        [JsonProperty("STR_mod")]
        public string StrengthMod { get; set; }

        [JsonProperty("DEX")]
        public string Dexterity { get; set; }

        [JsonProperty("DEX_mod")]
        public string DexterityMod { get; set; }

        [JsonProperty("CON")]
        public string Constitution { get; set; }

        [JsonProperty("CON_mod")]
        public string ConstitutionMod { get; set; }

        [JsonProperty("INT")]
        public string Intelligence { get; set; }

        [JsonProperty("INT_mod")]
        public string IntelligenceMod { get; set; }

        [JsonProperty("WIS")]
        public string Wisdon { get; set; }

        [JsonProperty("WIS_mod")]
        public string WisdomMod { get; set; }

        [JsonProperty("CHA")]
        public string Charisma { get; set; }

        [JsonProperty("CHA_mod")]
        public string CharismaMod { get; set; }

        [JsonProperty("Saving Throws")]
        public string SavingThrows { get; set; }

        [JsonProperty("Skills")]
        public string Skills { get; set; }

        [JsonProperty("Senses")]
        public string Senses { get; set; }

        [JsonProperty("Languages")]
        public string Languages { get; set; }

        [JsonProperty("Challenge")]
        public string Challenge { get; set; }

        [JsonProperty("Traits")]
        public string Traits { get; set; }

        [JsonProperty("Actions")]
        public string Actions { get; set; }

        [JsonProperty("Legendary Actions")]
        public string LegendaryActions { get; set; }

        [JsonProperty("img_url")]
        public string ImageURL { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

    public class StateSelectorManager
    {
        public DNDState[] States { get; private set; }

        public int CurrentIndex;

        public DNDState CurrentState => States[CurrentIndex];

        public StateSelectorManager(DNDState[] states)
        {
            States = states;
        }

        public DNDState this[int index]
        {
            get { return States[index]; }
        }

    }

    public class DataManager
    {

        public StateSelectorManager MonsterData;
        


        public void DeserializeMonsters()
        {

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\srd_5e_monsters.json");

            string jasonData = File.ReadAllText(path);

            MonsterData = new StateSelectorManager(JsonConvert.DeserializeObject<DNDState[]>(jasonData));

        }


    }
}
