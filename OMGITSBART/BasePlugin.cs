using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.ObjectCreation;
using MTM101BaldAPI.Registers;
using System.Collections;
using UnityEngine;

namespace OMGITSBART
{
    [BepInPlugin("starrie.bbplus.omgitsbart", "Bart In BB+", "2.0.0.0")]

    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi")]

    public class BasePlugin : BaseUnityPlugin
    {
        public AssetManager assetMan = new AssetManager();
        private static string npcSubDirectory = "Bart";

        private IEnumerator RegisterAssets()
        {
            yield return 1;

            yield return "Building NPCs...";

            Bart SimpsonGamer = new NPCBuilder<Bart>(Info)
              .SetName("Bart")
              .SetEnum("SimpsonGamer")
              .SetMinMaxAudioDistance(1, 300)
              .IgnorePlayerOnSpawn()
              .AddSpawnableRoomCategories(new RoomCategory[] { RoomCategory.Hall, RoomCategory.Class, RoomCategory.Faculty })
              .SetPoster(assetMan.Get<Texture2D>("BartPoster"), "Bart", "Omg its bart wait a minute? What the fuck bart!!!")
              .Build();

            yield return "Doing some miscellaneous stuff...";

            SimpsonGamer.bartSprite = assetMan.Get<Sprite>("BartSprite");
            SimpsonGamer.simpsonGamer = assetMan.Get<SoundObject>("SimpsonGamer");

            assetMan.Add<NPC>("Bart", SimpsonGamer);

            yield break;
        }

        private void GetAssets()
        {
            assetMan.Add<Texture2D>("Bart", AssetLoader.TextureFromMod(this, npcSubDirectory, "Me.png"));
            assetMan.Add<Texture2D>("BartPoster", AssetLoader.TextureFromMod(this, npcSubDirectory, "POS_me.png"));
            assetMan.Add<Sprite>("BartSprite", AssetLoader.SpriteFromTexture2D(assetMan.Get<Texture2D>("Bart"), 30));
            assetMan.Add<SoundObject>("SimpsonGamer", ObjectCreators.CreateSoundObject(AssetLoader.AudioClipFromMod(this, npcSubDirectory, "Sound/Simpson_Gamer.ogg"), "*SIMPSON GAMER*", SoundType.Music, Color.yellow));
        }

        public void Awake()
        {
            Harmony harmony = new Harmony("starrie.bbplus.omgitsbart");

            harmony.PatchAll();

            GetAssets();

            LoadingEvents.RegisterOnAssetsLoaded(Info, RegisterAssets(), LoadingEventOrder.Pre);
            GeneratorManagement.Register(this, GenerationModType.Addend, AddObjects);
        }

        private void AddObjects(string floor, int floorNumber, SceneObject floorObject)
        {
            if (floor.StartsWith("F"))
            {
                floorObject.potentialNPCs.Add(new WeightedNPC()
                {
                    selection = assetMan.Get<NPC>("Bart"),
                    weight = floorNumber < 2 ? 200 * floorNumber : 450
                }
                );
            }
            else if (floor == "END")
            {
                floorObject.potentialNPCs.Add(new WeightedNPC()
                {
                    selection = assetMan.Get<NPC>("Bart"),
                    weight = 450
                }
                );
            }

            floorObject.MarkAsNeverUnload();
        }
    }
}