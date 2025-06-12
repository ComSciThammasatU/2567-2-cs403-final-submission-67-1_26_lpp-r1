[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/w8H8oomW)
**<ins>Note</ins>: Students must update this `README.md` file to be an installation manual or a README file for their own CS403 projects.**

**รหัสโครงงาน:** 67-1_26_lpp-r1

**ชื่อโครงงาน (ไทย):** ฟินลีฟ ไอส์ล เดโม

**Project Title (Eng):** FINLEAF ISLE DEMO 

**อาจารย์ที่ปรึกษาโครงงาน:** ผศ. ดร. ลัมพาพรรณ พันธุ์ชูจิตร์

**ผู้จัดทำโครงงาน:** นายขจรเกียรติ แสงสุริย์  6409610588  kajornkiet.sea@dome.tu.ac.th


   
# Directory Tree
```
2567-2-cs403-final-submission-67-1_26_lpp-r1/  
├── README.md  
├── demo  
│   ├── 67-1_26_lpp-r1_demo_1.mp4
│   ├── 67-1_26_lpp-r1_demo_2.mp4
│   └── 67-1_26_lpp-r1_demo_3.mp4
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

![image](https://github.com/user-attachments/assets/cd866366-2fb3-4bba-907f-4b6e32fe4cf7)  

- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)  with ".NET Desktop Development" workload  

![image](https://github.com/user-attachments/assets/3b839369-af50-4f59-a38d-407e943f5c0e)  
กดเลือก .NET Desktop Development แล้วจึงกดติดตั้ง  

- [Git](https://git-scm.com/downloads/win)
  
![image](https://github.com/user-attachments/assets/d0fe4a0f-3a99-42ff-8632-ea583d8f44a6)  
![image](https://github.com/user-attachments/assets/1c494528-6212-42ba-b41f-25955b913466)  
![image](https://github.com/user-attachments/assets/eca8525a-0dd4-4fff-9764-347b0d523dc9)  
![image](https://github.com/user-attachments/assets/6c8127e5-92f4-41a0-8573-3628767f5ab0)  
![image](https://github.com/user-attachments/assets/61348889-2ea4-482a-9f69-8c386253dcca)  
![image](https://github.com/user-attachments/assets/270f70b0-a199-43ba-adc9-43c4442a10bb)  
![image](https://github.com/user-attachments/assets/99f5605d-2cd6-48f8-9605-944bd128bf21)  
![image](https://github.com/user-attachments/assets/175678b3-14ab-4a2a-a8a6-6e0762a57926)  
![image](https://github.com/user-attachments/assets/18c3d22c-1544-41db-8d30-69657ad4aca0)  
![image](https://github.com/user-attachments/assets/873b0c5d-f168-4c7d-9cb9-a2281ee53802)  
![image](https://github.com/user-attachments/assets/c593f25c-5c75-47d5-90b0-04e6459178df)  
![image](https://github.com/user-attachments/assets/f875bdf6-79b0-4948-8eda-87be54ab961d)  
![image](https://github.com/user-attachments/assets/056a602d-5322-4e9a-8ecc-caef8ea313bb)  

# วิธีการติดตั้งโปรเจค  

![image](https://github.com/user-attachments/assets/0bfb1a97-691f-4c29-95cc-d8b9776dbc4b)  
เปิด Git Bash   


![image](https://github.com/user-attachments/assets/4b2d77ee-2494-4e39-a73a-db22ff7cac31)  
คัดลอก URL ของ git repo  


`git clone `  
ภายใน Git Bash ใช้ git clone ตามด้วย URL ที่คัดลอกไว้  


![image](https://github.com/user-attachments/assets/cfe8a889-b5b5-4875-9875-9f7683c7f2d9)  
ภายในโฟลเดอร์โปรเจค ทำการเปิดไฟล์ .sln ด้วย Visual Studio  


![image](https://github.com/user-attachments/assets/8bdef123-2368-4471-a881-17b1454b0cb5)  
เปิด Command Prompt ภายใน Visual Studio  


`dotnet restore `  
ใช้คำสั่งเพิ่อพื้นฟู package  


`dotnet list package `  
ใช้คำสั่งตรวจสอบรายการ package   
![image](https://github.com/user-attachments/assets/44e3c5b6-c947-4810-8331-7555ff08cd6e)  


![image](https://github.com/user-attachments/assets/8a0b1297-8334-4ede-a3c3-14b97db065e3)  
ทำการ Build Solution  


![image](https://github.com/user-attachments/assets/60887b6e-c47a-4965-9f9b-c9cfaec24eaa)  
แล้วจึงสามารถกดทำงานโปรแกรม  


# วิธีการใช้งานโปรเจค

![image](https://github.com/user-attachments/assets/0438dc27-162b-4b41-ab0d-9c0e6f2ab1b3)  
หน้าเริ่มเกม กด Play เพื่อเข้าเล่น  


![image](https://github.com/user-attachments/assets/16597077-1fcf-4b3c-9fee-5a728f296a1e)  
เลือกช่องบันทึก  


![image](https://github.com/user-attachments/assets/959d9102-a872-4a74-b271-704089afa264)  
การควบคุมภายในเกมมีได้แก่ การเคลื่อนที่ด้วย WASD, การคลิกซ้ายเพื่อตกปลา, กด E เปิดเมนู  


![image](https://github.com/user-attachments/assets/74899ea8-64f7-44ad-94c0-aedff98d77ad)  
ผู้เล่นบังคับปลาให้อยู่ภายในวงกลมขนาดเล็ก เมื่อวงกลมเพิ่มจนสมบูรณ์จึงได้รับปลา  


![image](https://github.com/user-attachments/assets/fb86d1fb-14bf-4325-a4f7-cd71091ff1f2)   
การปฏิสัมพันธ์กับที่นอน เป็นการบันทึกเกม  


![image](https://github.com/user-attachments/assets/41676a49-9dc4-4522-9e5a-2b952bc27321)  
การตั้งค่าภายในเกม  


![image](https://github.com/user-attachments/assets/c9448e35-b45f-4be9-8540-4e8cff7c1cb1)  
เมนูสำหรับออกจากเกม  







