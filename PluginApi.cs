using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomboxController_Content
{
    internal class PluginApi : GetPlugin
    {
        internal override void Log(object message)
        {
            base.Log(message);
        }

        public override Configs GetConfig()
        {
            return base.GetConfig();
        }

        internal override BoomboxController GetBoombox()
        {
            return base.GetBoombox();
        }

        public override Harmony GetHarmony()
        {
            return base.GetHarmony();
        }
    }
}
