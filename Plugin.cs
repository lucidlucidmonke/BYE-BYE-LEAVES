using System;
using BepInEx;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;
using Utilla;

namespace GorillaTagModTemplateProject
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	/* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		bool inRoom;
		private bool leavesFound = false;
        private bool leavesEnabled = true;
        

		void Start()
		{
			/* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

			Utilla.Events.GameInitialized += OnGameInitialized;
		}

        void OnEnable()
		{
            {

            }
            HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{

            HarmonyPatches.RemoveHarmonyPatches();
		}



        void OnGameInitialized(object sender, EventArgs e)
		{
			/* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
            leavesFound = false;
		}



        void Update()
        {
            if (inRoom && !leavesFound)
            {
                
                Transform forestTransform = null;

                forestTransform = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest")?.transform;
                

                foreach (Transform child in forestTransform)
                {
                    if (child.name == "leaves_green (combined by EdMeshCombinerSceneProcessor)")
                    {
                        child.gameObject.SetActive(false);
                        leavesFound = true;
                        leavesEnabled = true;
                        Logger.LogInfo("=== BYE BYE LEAVES === Leaves found and disabled.");
                    }
                }

                if (!leavesFound)
                {
                    Logger.LogInfo("=== BYE BYE LEAVES === No leaves found to disable.");
                }
            }
            
        }
        


        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
            
        }
        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			inRoom = false;
            Transform forestTransform = null;

                forestTransform = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest")?.transform;
                

                foreach (Transform child in forestTransform)
                {
                    if (child.name == "leaves_green (combined by EdMeshCombinerSceneProcessor)")
                    {
                        child.gameObject.SetActive(true);
                        leavesEnabled = true;
                        leavesFound = false;
                        Logger.LogInfo("=== BYE BYE LEAVES === Leaves re-enabled.");
                    }
                }
            
        }
	}
}
