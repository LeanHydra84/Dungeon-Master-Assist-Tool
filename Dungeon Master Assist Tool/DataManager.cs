using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections;

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
        public string Wisdom { get; set; }

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

    // This is an ugly wrapper for a beautiful solution, let it be known
    // Maybe make this generic so it can be used by other classes
    public class StateEnumerator<T> : IEnumerable<T>
    {

        T[] array;
        string searchQuery;

        public StateEnumerator(T[] array, string search = null)
        {
            this.array = array;
            this.searchQuery = search;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(array, searchQuery);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<T>
        {

            T[] array;

            int currentIndex;

            string query = null;

            public Enumerator(T[] stateArray)
            {
                array = stateArray;
                currentIndex = -1;
            }

            public Enumerator(T[] stateArray, string query) : this(stateArray)
            {
                this.query = query;
            }

            public T Current => array[currentIndex];

            object IEnumerator.Current => array[currentIndex];


            public bool MoveNext()
            {

                currentIndex++;

                if (!string.IsNullOrWhiteSpace(query))
                {
                    while (currentIndex < array.Length && !Current.ToString().ToLower().Contains(query))
                    {
                        currentIndex++;
                    }
                }



                if (currentIndex >= array.Length) return false;
                return true;
            }

            public void Reset() { }
            public void Dispose() { }

        }
    }

    public class StateSelectorManager : INotifyPropertyChanged
    {


        public DNDState CurrentState => States[CurrentIndex];
        public DNDState[] States { get; set; }

        public int CurrentIndex;

        public StateSelectorManager(DNDState[] states)
        {
            States = states;
        }

        public DNDState this[int index]
        {
            get { return States[index]; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateIndex(object sender, SelectionChangedEventArgs e)
        {
            CurrentIndex = ((ListBox)sender).SelectedIndex;
            PropertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
        }

        public string searchQuery;
        public void UpdateSearchQuery(object sender, EventArgs e)
        {
            searchQuery = ((TextBox)sender).Text;
            PropertyChanged(this, new PropertyChangedEventArgs("Enumerator"));
        }

        public StateEnumerator<DNDState> Enumerator
        {
            get
            {
                return new StateEnumerator<DNDState>(States, searchQuery);
            }
        }

    }

    public class DataManager : INotifyPropertyChanged
    {

        public StateSelectorManager MonsterData { get; set; }

        public DataManager()
        {
            DeserializeMonsters();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyMonster()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("MonsterData"));
        }

        public void DeserializeMonsters()
        {

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\srd_5e_monsters.json");

            string jasonData = File.ReadAllText(path);

            MonsterData = new StateSelectorManager(JsonConvert.DeserializeObject<DNDState[]>(jasonData));

        }


    }
}
