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
using System.Text.RegularExpressions;

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
        public string TraitsFormatted => string.IsNullOrWhiteSpace(Traits) ? "None" : Regex.Replace(Traits, "<.*?>", string.Empty).Truncate(DataManager.LengthyTextLimit);

        [JsonProperty("Actions")]
        public string Actions { get; set; }
        public string ActionsFormatted => string.IsNullOrWhiteSpace(Traits) ? "None" : Regex.Replace(Actions, "<.*?>", string.Empty).Truncate(DataManager.LengthyTextLimit);

        [JsonProperty("Legendary Actions")]
        public string LegendaryActions { get; set; }
        public string LegendaryActionsFormatted => string.IsNullOrWhiteSpace(Traits) ? "None" : Regex.Replace(LegendaryActions, "<.*?>", string.Empty).Truncate(DataManager.LengthyTextLimit);

        [JsonProperty("img_url")]
        public string ImageURL { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

    // This is an ugly wrapper for a beautiful solution, let it be known
    public class ReloadableEnumerator<T> : IEnumerable<T>
    {

        T[] array;
        string searchQuery;

        public ReloadableEnumerator(T[] array, string search = null)
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


        public DNDState[] States { get; set; }
        public DNDState CurrentState { get; private set; }

        public StateSelectorManager(DNDState[] states)
        {
            States = states;
            CurrentState = States[0];
        }

        public DNDState this[int index]
        {
            get { return States[index]; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateIndex(object sender, SelectionChangedEventArgs e)
        {
            CurrentState = (DNDState)((ListBox)sender).SelectedItem ?? (DNDState)((ListBox)sender).Items.GetItemAt(0);
            PropertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
        }

        public string searchQuery;
        public void UpdateSearchQuery(object sender, EventArgs e)
        {
            searchQuery = ((TextBox)sender).Text.ToLower();
            PropertyChanged(this, new PropertyChangedEventArgs("Enumerator"));
            //UpdateIndex(this, null);
        }

        public ReloadableEnumerator<DNDState> Enumerator => new ReloadableEnumerator<DNDState>(States, searchQuery);

    }

    public class DataManager
    {

        public StateSelectorManager MonsterData { get; private set; }

        public static int LengthyTextLimit = 100;

        public DataManager()
        {
            DeserializeMonsters();
        }

        public void DeserializeMonsters()
        {

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\srd_5e_monsters.json");

            string jasonData = File.ReadAllText(path);

            MonsterData = new StateSelectorManager(JsonConvert.DeserializeObject<DNDState[]>(jasonData));

        }

        

    }

    public static partial class ExtensionMethods
    {
        public static string Truncate(this string input, int length)
        {
            return input.Substring(0, length) + "...";
        }
    }

}
