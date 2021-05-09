using System.Collections.Generic;
using StardewModdingAPI;

namespace AutoTrash
{
    class ModConfig
    {
        public bool MinesOnly { get; set; } = true;
        public IList<int> DeleteItems { get; set; } = new List<int>();
        public SButton ToggleTrash { get; set; } = SButton.RightAlt;
    }
}