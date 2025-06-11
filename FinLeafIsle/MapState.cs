using FinLeafIsle.Collisions;
using FinLeafIsle.Components;
using MonoGame.Extended.ECS;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Autofac;
using System.IO;
using FinLeafIsle.Components.Inventory;

namespace FinLeafIsle
{
    public enum MapLoaderState
    {
        Idle,
        SaveAndUnload,
        LoadMap,
        LoadFinish,
        Unload,
    }
    public class MapState
    {
        public MapLoaderState _state;

        public MapState()
        {
            _state = MapLoaderState.Idle;
        }
       
    }
}
