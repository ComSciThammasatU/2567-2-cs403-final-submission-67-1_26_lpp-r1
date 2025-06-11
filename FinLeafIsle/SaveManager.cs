using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using FinLeafIsle.Components.Inventory;
using FinLeafIsle.DayTimeWeather;

namespace FinLeafIsle
{
    public class SaveManager
    {

        public string _savePath = "";
        private DayTime _daytime;
        public SaveManager(DayTime dayTime) 
        {
            _daytime = dayTime;        
        }
        

        public void CopyTempToSaveSlot()
        {
            System.Diagnostics.Debug.WriteLine($"Saved one!!! ");
            string sourceDir = Path.Combine(Environment.CurrentDirectory, "Content", "datas", "Temp");
            

            if (!Directory.Exists(_savePath))
                Directory.CreateDirectory(_savePath);
           
            // Copy game data
            string gameDataSrc = Path.Combine(sourceDir, "gameData.json");
            string gameDataDst = Path.Combine(_savePath, "gameData.json");
            if (File.Exists(gameDataSrc))
                File.Copy(gameDataSrc, gameDataDst, true);
            
            // Copy player data
            string playerDataSrc = Path.Combine(sourceDir, "playerData.json");
            string playerDataDst = Path.Combine(_savePath, "playerData.json");
            if (File.Exists(playerDataSrc))
                File.Copy(playerDataSrc, playerDataDst, true);

            // Copy all files from mapData
            string mapDataSrcDir = Path.Combine(sourceDir, "mapData");
            string mapDataDstDir = Path.Combine(_savePath, "mapData");

            if (Directory.Exists(mapDataSrcDir))
            {
                if (!Directory.Exists(mapDataDstDir))
                    Directory.CreateDirectory(mapDataDstDir);

                foreach (var file in Directory.GetFiles(mapDataSrcDir, "*.json"))
                {
                    var fileName = Path.GetFileName(file);
                    var destPath = Path.Combine(mapDataDstDir, fileName);
                    File.Copy(file, destPath, true);
                }
            }
           
        }

        public void CopySaveSlotToTemp()
        {
            string destinationDir = Path.Combine(Environment.CurrentDirectory, "Content", "datas", "Temp");


            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);

            // Copy player data
            string gameDataSrc = Path.Combine(_savePath, "gameData.json");
            string gameDataDst = Path.Combine(destinationDir, "gameData.json");
            if (File.Exists(gameDataSrc))
                File.Copy(gameDataSrc, gameDataDst, true);

            // Copy player data
            string playerDataSrc = Path.Combine(_savePath, "playerData.json"); 
            string playerDataDst = Path.Combine(destinationDir, "playerData.json");
            if (File.Exists(playerDataSrc))
                File.Copy(playerDataSrc, playerDataDst, true);

            // Copy all files from mapData
            string mapDataSrcDir = Path.Combine(_savePath, "mapData");
            string mapDataDstDir = Path.Combine(destinationDir, "mapData");

            if (Directory.Exists(mapDataSrcDir))
            {
                if (!Directory.Exists(mapDataDstDir))
                    Directory.CreateDirectory(mapDataDstDir);

                foreach (var file in Directory.GetFiles(mapDataSrcDir, "*.json"))
                {
                    var fileName = Path.GetFileName(file);
                    var destPath = Path.Combine(mapDataDstDir, fileName);
                    File.Copy(file, destPath, true);
                }
            }
        }

        public void ClearTemp()
        {
            System.Diagnostics.Debug.WriteLine($"{_daytime.Time}");
            string tempDir = Path.Combine(Environment.CurrentDirectory, "Content", "datas", "Temp");
            string playerDataPath = Path.Combine(tempDir, "playerData.json");
            string gameDataPath = Path.Combine(tempDir, "gameData.json");
            string mapDataDir = Path.Combine(tempDir, "mapData");

            // Delete playerData.json
            if (File.Exists(playerDataPath))
                File.Delete(playerDataPath);

            // Delete gameData.json
            if (File.Exists(gameDataPath))
                File.Delete(gameDataPath);

            // Delete all mapData files and directory
            if (Directory.Exists(mapDataDir))
            {
                foreach (var file in Directory.GetFiles(mapDataDir))
                {
                    File.Delete(file);
                }

                // Delete mapData folder itself
                //Directory.Delete(mapDataDir);
            }

           
        }
    }
}
