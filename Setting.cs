using System;

namespace SimpleSettingsReader
{
    [Serializable]
    internal class Setting
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
