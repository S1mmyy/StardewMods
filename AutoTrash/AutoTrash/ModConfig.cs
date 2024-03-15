using System.Collections.Generic;
using StardewModdingAPI;

namespace AutoTrash
{
    class ModConfig
    {
        public bool MinesOnly { get; set; } = true;
        public IList<string> DeleteItems { get; set; } = [];
        public SButton ToggleTrash { get; set; } = SButton.RightAlt;
    }
}