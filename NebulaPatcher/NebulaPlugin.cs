﻿using BepInEx;
using HarmonyLib;
using Mirror;
using NebulaModel;
using NebulaModel.Logger;
using NebulaPatcher.Logger;
using NebulaPatcher.MonoBehaviours;
using System;
#if DEBUG
using System.IO;
#endif
using System.Reflection;
using UnityEngine;

namespace NebulaPatcher
{
    [BepInPlugin(PluginInfo.PLUGIN_ID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("dsp.galactic-scale.2", BepInDependency.DependencyFlags.SoftDependency)] // to load after GS2
    [BepInProcess("DSPGAME.exe")]
    public class NebulaPlugin : BaseUnityPlugin
    {
        void Awake()
        {
            Log.Init(new BepInExLogger(Logger));

            NebulaModel.Config.ModInfo = Info;
            NebulaModel.Config.LoadOptions();

            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                Log.Error("Unhandled exception occurred while initializing Nebula:", ex);
            }
        }

        void Initialize()
        {
            InitPatches();
            AddNebulaBootstrapper();
        }

        private void InitPatches()
        {
            Log.Info("Patching Dyson Sphere Program...");

            try
            {
                Log.Info($"Applying patches from {PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION_WITH_SHORT_SHA}");
#if DEBUG
                if (Directory.Exists("./mmdump"))
                {
                    foreach (FileInfo file in new DirectoryInfo("./mmdump").GetFiles())
                    {
                        file.Delete();
                    }

                    Environment.SetEnvironmentVariable("MONOMOD_DMD_TYPE", "cecil");
                    Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", "./mmdump");
                }
#endif
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_ID);
#if DEBUG
                Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", "");
#endif

                Log.Info("Patching completed successfully");
            }
            catch (Exception ex)
            {
                Log.Error("Unhandled exception occurred while patching the game:", ex);
            }
        }

        void AddNebulaBootstrapper()
        {
            Log.Info("Applying Nebula behaviours..");

            GameObject nebulaRoot = new GameObject();
            nebulaRoot.name = "Nebula Multiplayer Mod";
            nebulaRoot.AddComponent<NebulaBootstrapper>();

            // Needed for Mirror Networking
            var assembly = Assembly.GetAssembly(typeof(Config));
            var type = assembly.GetType("Mirror.GeneratedNetworkCode");
            var method = type.GetMethod("InitReadWriters");
            method.Invoke(null, null);

            assembly = Assembly.GetAssembly(typeof(NetworkManager));
            type = assembly.GetType("Mirror.NetworkLoop");
            AccessTools.Method(type, "RuntimeInitializeOnLoad").Invoke(null, null);

            assembly = Assembly.GetAssembly(typeof(NetworkManager));
            type = assembly.GetType("Mirror.GeneratedNetworkCode");
            AccessTools.Method(type, "InitReadWriters").Invoke(null, null);

            assembly = Assembly.GetAssembly(typeof(DistanceInterestManagement));
            type = assembly.GetType("Mirror.GeneratedNetworkCode");
            AccessTools.Method(type, "InitReadWriters").Invoke(null, null);

            Log.Info("Behaviours applied.");
        }
    }
}
