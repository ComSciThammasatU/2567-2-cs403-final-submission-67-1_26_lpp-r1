using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinLeafIsle
{
    public enum GState
    {
        MainMenu = 0,
        GamePlay = 1,
        Playermenu = 2,
        SaveDay = 3,
    }

    public enum PMenuState
    {
        Inventory = 0,
        Menu = 1,
        Setting = 2,
        
    }
    public class GameState
    {
        public GState State { get; set; }
        public PMenuState PMenu { get; set; }
    }
}
