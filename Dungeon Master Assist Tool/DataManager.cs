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
using System.Windows;

namespace Dungeon_Master_Assist_Tool
{

    public interface BindToListBox
    {
        string PrimarySource { get; }
        string SecondarySource { get; }
    }

    public class MonsterState : BindToListBox
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

        private string _traits = "None";
        [JsonProperty("Traits")]
        public string Traits
        {
            get => _traits;
            set
            {
                string temp = value.HtmlLineSplit().CleanHTML();
                if (!string.IsNullOrWhiteSpace(temp))
                    _traits = temp;
            }
        }

        private string _actions = "None";
        [JsonProperty("Actions")]
        public string Actions
        {
            get => _actions;
            set
            {
                string temp = value.HtmlLineSplit().CleanHTML();
                if (!string.IsNullOrWhiteSpace(temp))
                    _actions = temp;
            }
        }

        private string _legendaryActions = "None";
        [JsonProperty("Legendary Actions")]
        public string LegendaryActions
        {
            get => _legendaryActions;
            set
            {
                string temp = value.HtmlLineSplit().CleanHTML();
                if (!string.IsNullOrWhiteSpace(temp))
                    _legendaryActions = temp;
            }
        }

        [JsonProperty("img_url")]
        public string ImageURL { get; set; }

        public string PrimarySource => Name;
        public string SecondarySource => Metadata;

        public override string ToString()
        {
            return Name;
        }

    }

    public class SpellState : BindToListBox
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("page")]
        public string Page { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("components")]
        public string Components { get; set; }

        [JsonProperty("material")]
        public string Material { get; set; } = "None";

        [JsonProperty("ritual")]
        public string Ritual { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("concentration")]
        public string Concentration { get; set; }

        [JsonProperty("casting_time")]
        public string CastingTime { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        private string _description;
        [JsonProperty("desc")]
        public string Description
        {
            get => _description;
            set
            {
                string temp = value.HtmlLineSplit().CleanHTML();
                if (!string.IsNullOrWhiteSpace(temp))
                    _description = temp;
            }
        }

        public string PrimarySource => Name;
        public string SecondarySource => School;

        public override string ToString()
        {
            return Name;
        }

    }

    public class DamagePhrases
    {

        public Dictionary<string, Dictionary<string, string[]>> Phrases;

        public void Deserialize(string name)
        {
            string jsonData = null;

            var stream = Application.GetResourceStream(new Uri($@"pack://application:,,,/JsonFiles/{name}", UriKind.RelativeOrAbsolute)).Stream;

            using (StreamReader reader = new StreamReader(stream))
            {
                jsonData = reader.ReadToEnd();
            }
            stream.Close();

            Phrases = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string[]>>>(jsonData);
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

    public class StateSelectorManager<T> : INotifyPropertyChanged where T : class, BindToListBox
    {


        public T[] States { get; set; }
        public T CurrentState { get; private set; }

        public StateSelectorManager(T[] states)
        {
            States = states;
            CurrentState = States[0];
        }

        public T this[int index]
        {
            get { return States[index]; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateIndex(object sender, SelectionChangedEventArgs e)
        {
            CurrentState = (T)((ListBox)sender).SelectedItem ?? (T)((ListBox)sender).Items.GetItemAt(0);
            PropertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
        }

        public string searchQuery;
        public void UpdateSearchQuery(object sender, EventArgs e)
        {
            searchQuery = ((TextBox)sender).Text.ToLower();
            PropertyChanged(this, new PropertyChangedEventArgs("Enumerator"));
        }

        public ReloadableEnumerator<T> Enumerator => new ReloadableEnumerator<T>(States, searchQuery);

    }

    public class RandomSetSelector<T> : IEnumerable<T>
    {

        private static Random rand = new Random();

        T[] Set;

        public RandomSetSelector(T[] set)
        {
            Set = set;
        }

        public static RandomSetSelector<T> DeserializeJSONResource(string name)
        {
            string jsonData = null;

            var stream = Application.GetResourceStream(new Uri($@"pack://application:,,,/JsonFiles/{name}", UriKind.RelativeOrAbsolute)).Stream;

            using (StreamReader reader = new StreamReader(stream))
            {
                jsonData = reader.ReadToEnd();
            }
            stream.Close();

            return new RandomSetSelector<T>(JsonConvert.DeserializeObject<T[]>(jsonData));

        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in Set)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T this[int index]
        {
            get => Set[index];
        }

        public T GetRandom => Set[rand.Next() % Set.Length];

    }

    public class DataManager
    {

        public StateSelectorManager<MonsterState> MonsterData { get; }
        public StateSelectorManager<SpellState> SpellsList { get; }

        DamagePhrases phrases;

        public DataManager()
        {
            MonsterData = DeserializeJSONAsset<MonsterState>("srd_5e_monsters.json");
            SpellsList = DeserializeJSONAsset<SpellState>("spells.json");
        }

        public static StateSelectorManager<T> DeserializeJSONAsset<T>(string fileName) where T : class, BindToListBox
        {
            string jsonData = null;

            var stream = Application.GetResourceStream(new Uri($@"pack://application:,,,/JsonFiles/{fileName}", UriKind.RelativeOrAbsolute)).Stream;

            using (StreamReader reader = new StreamReader(stream))
            {
                jsonData = reader.ReadToEnd();
            }
            stream.Close();

            return new StateSelectorManager<T>(JsonConvert.DeserializeObject<T[]>(jsonData));

        }

    }

    public static partial class ExtensionMethods
    {

        public static string CleanAndValidate(this string input)
        {
            string temp = input.HtmlLineSplit().CleanHTML();
            if (!string.IsNullOrWhiteSpace(temp))
                return temp;
            else return input;
        }


        public static string Truncate(this string input, int length)
        {
            if (input.Length < length) return input;
            return input.Substring(0, length) + "...";
        }

        public static string CleanHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        public static string HtmlLineSplit(this string input)
        {
            return Regex.Replace(input, "</p>", "\n\n");
        }

    }

}
