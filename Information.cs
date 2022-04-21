using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimpleSettingsReader
{
    /// <summary>
    /// Functions for information collection and retrieval.
    /// </summary>
    public class Information
    {
        /// <summary>
        /// All objectified settings in the INI file.
        /// </summary>
        private List<Setting> allSettings = new List<Setting>();

        /// <summary>
        /// All objectified categories in the INI file.
        /// </summary>
        private List<Category> allCategories = new List<Category>();

        /// <summary>
        /// The INI file reader functions.
        /// </summary>
        private Parser parser;

        /// <summary>
        /// Initializes <see cref="allSettings"/> using the <see cref="BuildCategoryList(string[])"/> method.
        /// </summary>
        /// <param name="parser">The file reader/parser you used.</param>
        /// <exception cref="FileNotFoundException"></exception>
        public Information(Parser parser)
        {
            this.parser = parser;
            if(parser.allLines == null)
            {
                throw new FileNotFoundException();
            }
            BuildCategoryList(parser.allLines);
        }

        /// <summary>
        /// Single setting in the INI file that exists under the category.
        /// </summary>
        /// <param name="category">The category to look for.</param>
        /// <param name="key">The key to look for under the category.</param>
        /// <returns>A string that matches the value of the key passed in under the category passed in. Blank if it can't be found.</returns>
        public string GetSetting(string category, string key)
        {
            Setting setting = allSettings.FirstOrDefault(x => x.Category.Name.Equals(category) && x.Key.Equals(key));
            if (setting != null)
                return setting.Value;
            else
                return "";
        }

        /// <summary>
        /// All settings in the INI file that exist under the category.
        /// </summary>
        /// <param name="category">The category used to find all settings.</param>
        /// <returns>A list of objects that exist under the string passed.</returns>
        public List<object> GetSettings(string category)
        {
            IEnumerable<Setting> settings = allSettings.Where(x => x.Category.Name.Contains(category));
            List<object> toReturn = new List<object>();
            foreach (Setting stng in (settings != null) ? settings : new List<Setting>())
            {
                toReturn.Add(stng.Value);
            }
            return toReturn;
        }

        /// <summary>
        /// All categories in the INI file.
        /// </summary>
        /// <returns>A list of strings that match all the categories.</returns>
        public List<string> GetCategories()
        {
            List<string> toReturn = new List<string>();
            foreach (Category catg in allCategories)
            {
                toReturn.Add(catg.Name);
            }
            return toReturn;
        }

        /// <summary>
        /// All categories in the INI file that match the passed parameter.
        /// </summary>
        /// <param name="possCategory">The value to search for. Uses a contains search.</param>
        /// <returns>A list of strings that match the category contains search.</returns>
        public List<string> GetCategories(string possCategory)
        {
            List<string> toReturn = new List<string>();
            foreach (Category catg in allCategories.Where(x => x.Name.Contains(possCategory)))
            {
                toReturn.Add(catg.Name);
            }
            return toReturn;
        }

        /// <summary>
        /// Builds a category list finding brackets '[' to figure out categories that INI settings will be in.
        /// <br></br>
        /// <br></br>
        /// Uses <see cref="BuildSettings(string[], string, int)"/> to set up new model data <see cref="Setting"/>.
        /// </summary>
        /// <param name="allLines">All the lines in a text file.</param>
        internal void BuildCategoryList(string[] allLines)
        {
            for (int i = 0; i < allLines.Length; i++)
            {
                //Category Found
                if (!string.IsNullOrEmpty(allLines[i]) && allLines[i].StartsWith(@"["))
                {
                    string category = allLines[i].Replace("[", "").Replace("]", "");
                    BuildSettings(allLines, category, i);
                }
            }
        }

        /// <summary>
        /// Creates the <see cref="Category"/> and <see cref="Setting"/> model data.
        /// </summary>
        /// <param name="allLines">Every line from the INI file.</param>
        /// <param name="category"><see cref="Setting.Category"/> information.</param>
        /// <param name="i">Index to start on from this category.</param>
        internal void BuildSettings(string[] allLines, string category, int i)
        {
            i++;
            while (i < allLines.Length && !allLines[i].StartsWith(@"["))
            {
                if (!string.IsNullOrEmpty(allLines[i]))
                {
                    AddNewSettingAndCategory(
                        allLines[i].Substring(0, allLines[i].LastIndexOf(@"=")),
                        allLines[i].Substring(allLines[i].LastIndexOf(@"=") + 1),
                        category);
                }
                i++;
            }
        }

        /// <summary>
        /// Adds the new <see cref="Category"/> and <see cref="Setting"/> information along 
        /// with an index to the <see cref="allCategories"/> and <see cref="allSettings"/>
        /// appropriately.
        /// </summary>
        /// <param name="key"><see cref="Setting.Key"/></param>
        /// <param name="value"><see cref="Setting.Value"/></param>
        /// <param name="category"><see cref="Category.Name"/></param>
        internal void AddNewSettingAndCategory(string key, string value, string category)
        {
            Category toCat = new Category();
            toCat.Id = allCategories.Count;
            toCat.Name = category;
            allCategories.Add(toCat);

            Setting toAdd = new Setting();
            toAdd.Id = allSettings.Count;
            toAdd.Key = key;
            toAdd.Value = value;
            toAdd.Category = toCat;
            allSettings.Add(toAdd);
        }
    }
}
