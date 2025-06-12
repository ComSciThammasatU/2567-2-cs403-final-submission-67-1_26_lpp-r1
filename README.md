[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/w8H8oomW)
**<ins>Note</ins>: Students must update this `README.md` file to be an installation manual or a README file for their own CS403 projects.**

**รหัสโครงงาน:** 67-1_26_lpp-r1

**ชื่อโครงงาน (ไทย):** ฟินลีฟ ไอส์ล เดโม

**Project Title (Eng):** FINLEAF ISLE DEMO 

**อาจารย์ที่ปรึกษาโครงงาน:** ผศ. ดร. ลัมพาพรรณ พันธุ์ชูจิตร์

**ผู้จัดทำโครงงาน:** นายขจรเกียรติ แสงสุริย์  6409610588  kajornkiet.sea@dome.tu.ac.th


   
Manual / Instructions for your projects starts here !
# Directory Tree
```
2567-2-cs403-final-submission-67-1_26_lpp-r1/  
├── README.md  
├── demo  
│   └── 67-1_26_lpp-r1_demo.mp4  
├── final_reports  
│   ├── 67-1_26_lpp-r1.pdf  
│   ├── 67-1_26_lpp-r1_abstract_en.txt  
│   └── 67-1_26_lpp-r1_abstract_th.txt
├── FinLeafIsle
│   ├── .config
│   │   └── dotnet-tools.json
│   ├── Collisions
│   │   ├── AABB.cs
│   │   ├── Body.cs
│   │   ├── CameraBlock.cs
│   │   ├── CollisionTester.cs
│   │   ├── GameWorld.cs
│   │   ├── GateArea.cs
│   │   ├── Manifold.cs
│   │   └── WaterArea.cs
│   ├── Components
│   │   ├── Inventory
│   │   │   ├── InventoryComponent.cs
│   │   │   └── InventorySlot.cs
│   │   ├── ItemComponent
│   │   │   ├── FishBehaviorDefinition.cs
│   │   │   └── Item.cs
│   │   ├── MiniGame
│   │   │   └── FishingMiniGameComponent.cs
│   │   ├── UI
│   │   │   └── Button.cs
│   │   ├── Bed.cs
│   │   ├── FishingComponent.cs
│   │   ├── Offset.cs
│   │   ├── OnMap.cs
│   │   ├── Player.cs
│   │   └── Wall.cs
│   ├── Content
│   │   ├── datas
│   │   │   └── Fish.json
│   │   ├── Guide
│   │   │   ├── CloseB.png
│   │   │   └── Guide.png
│   │   ├── Items
│   │   │   └── FishingItems
│   │   │       └── Bober.png
│   │   ├── Light
│   │   │   └── radial.png
│   │   ├── MainMenu
│   │   │   ├── BackB.png
│   │   │   ├── ExitB.png
│   │   │   ├── Logo.png
│   │   │   ├── MainMenuBG.png
│   │   │   ├── PlayB.png
│   │   │   ├── Save1.png
│   │   │   ├── Save2.png
│   │   │   └── Save3.png
│   │   ├── Maps
│   │   │   ├── Decorations
│   │   │   │   ├── bed.png
│   │   │   │   ├── bigtree.png
│   │   │   │   ├── bush_t1_01.png
│   │   │   │   ├── bush_t1_02.png
│   │   │   │   ├── bush_t1_03.png
│   │   │   │   ├── bush_t1_04.png
│   │   │   │   └── tent.png
│   │   │   ├── TiledTemplate
│   │   │   │   └── WallCollision.tx
│   │   │   ├── Tilesets
│   │   │   │   ├── BigTree.tsx
│   │   │   │   ├── Dark_temp.tsx
│   │   │   │   ├── Decoration.tsx
│   │   │   │   ├── home.png
│   │   │   │   ├── InsideTent.tsx
│   │   │   │   ├── land_spacing-export.png
│   │   │   │   ├── land_spacing-export.tsx
│   │   │   │   ├── tent.tsx
│   │   │   │   ├── tiles.png
│   │   │   │   └── Tileset.tsx
│   │   │   ├── Home.tmx
│   │   │   ├── Tent.tmx
│   │   │   ├── Town.tmx
│   │   │   └── town2.tmx
│   │   ├── OST
│   │   │   └── MysticGrove.wav
│   │   ├── Player
│   │   │   └── Fishing
│   │   │   │   ├── bigCircle.png
│   │   │   │   ├── smallCircle.png
│   │   │   │   └── smallCircleFilled.png
│   │   │   └── player.png
│   │   ├── PlayerMenu
│   │   │   ├── DecreaseB.png
│   │   │   ├── ExitB.png
│   │   │   ├── HelpB.png
│   │   │   ├── IncreaseB.png
│   │   │   ├── InventoryB.png
│   │   │   ├── MainB.png
│   │   │   ├── MenuB.png
│   │   │   ├── MusicLogo.png
│   │   │   ├── SettingB.png
│   │   │   └── SFXLogo.png
│   │   ├── SaveDay
│   │   │   └── NextB.png
│   │   ├── SFX
│   │   │   ├── Casting.wav
│   │   │   ├── Catch.wav
│   │   │   ├── Charge.wav
│   │   │   ├── Hooked.wav
│   │   │   ├── PressB.wav
│   │   │   ├── Progess.wav
│   │   │   ├── ReelIn.wav
│   │   │   ├── ReelOut.wav
│   │   │   └── Step.wav
│   │   ├── Content.mgcb
│   │   ├── DebugFont.spritefont
│   │   ├── hero.png
│   │   ├── ItemIcon.png
│   │   ├── pixel.png
│   │   └── tent.png
│   ├── Datas
│   │   ├── FishJsonData.cs
│   │   └── SavedMapData.cs
│   ├── DayTimeWeather
│   │   ├── DayTime.cs
│   │   └── TimePeriod.cs
│   ├── pipeline-references
│   │   ├── MonoGGame.Extended.Content.Pipeline.dll
│   │   └── MonoGGame.Extended.dll
│   ├── Systems
│   │   ├── BodyRenderSystem.cs
│   │   ├── CameraSystem.cs
│   │   ├── FishingMiniGameSystem.cs
│   │   ├── FishingRenderSystem.cs
│   │   ├── FishingSystem.cs
│   │   ├── GateSystem.cs
│   │   ├── HUDRenderSystem.cs
│   │   ├── LightingRenderSystem.cs
│   │   ├── MapLoaderSystem.cs
│   │   ├── MapRenderSystem.cs
│   │   ├── PlayerMenuRenderSystem.cs
│   │   ├── PlayerMenuSystem.cs
│   │   ├── PlayerSystem.cs
│   │   ├── RenderSystem.cs
│   │   ├── WaterAreaSystem.cs
│   │   └── WorldSystem.cs
│   ├── app.manifest
│   ├── AudioManager.cs
│   ├── DebugOverlay.cs
│   ├── EntityFactory.cs
│   ├── FinLeafIsle.tiled-project
│   ├── FinLeafIsle.tiled-session
│   ├── Game1.cs
│   ├── GameMain.cs
│   ├── GameState.cs
│   ├── Guide.cs
│   ├── Icon.bmp
│   ├── Icon.ico
│   ├── MainMenu.cs
│   ├── Map.cs
│   ├── MapLocation.cs
│   ├── MapState.cs
│   ├── PlayerMenu.cs
│   ├── Program.cs
│   ├── SaveDayPage.cs
│   ├── SaveManager.cs
│   └── SoundtrackManager.cs  
└── FinLeafIsle.sln
```
# Prerequisites
- [.NET SDK 8.0 or higher](https://dotnet.microsoft.com/en-us/download)  
  
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)  with ".NET Desktop Development" workload  

- [Git](https://git-scm.com/downloads/win)  

# Topic 3
