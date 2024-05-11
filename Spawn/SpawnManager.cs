using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoomboxController_Content.Spawn
{
    internal class SpawnManager : BoomboxController
    {
        [HarmonyPatch(typeof(RoundArtifactSpawner), "CreateArtifactSpawners")]
        [HarmonyPrefix]
        public static bool CreateArtifactSpawners_Patch(RoundArtifactSpawner __instance, List<Item> artifacts, ref GameObject ___artifactSpawnerPrefab)
        {
            for (int i = 0; i < artifacts.Count; i++)
            {
                if (artifacts[i].name != "Radio")
                {
                    ArtifactSpawner component = UnityEngine.Object.Instantiate(___artifactSpawnerPrefab, RoundArtifactSpawner.GetRandPointWithWeight(), Quaternion.identity).GetComponent<ArtifactSpawner>();
                    component.transform.parent = __instance.transform;
                    component.gameObject.SetActive(value: true);
                    component.artifactToSpawn = artifacts[i];
                }
            }
            return false;
        }
    }
}
