using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class InternalHelper
    {
        private static Dictionary<string, Dictionary<string, DateTime>> playerCooldowns = new Dictionary<string, Dictionary<string, DateTime>>();
        private static Dictionary<string, Dictionary<string, bool>> autoEnabled = new Dictionary<string, Dictionary<string, bool>>();

        public static void resetCooldown(string spell, string player)
        {
            if(!playerCooldowns.ContainsKey(spell))
            {
                playerCooldowns[spell] = new Dictionary<string, DateTime>();
            }
            playerCooldowns[spell][player] = new DateTime(1970, 1, 1, 0, 0, 0);
        }
        public static void setCooldown(string spell, string player)
        {
            if (!playerCooldowns.ContainsKey(spell))
            {
                playerCooldowns[spell] = new Dictionary<string, DateTime>();
            }
            playerCooldowns[spell][player] = DateTime.Now;
        }

        public static DateTime getCooldown(string spell, string player)
        {
            if (!playerCooldowns.ContainsKey(spell))
            {
                resetCooldown(spell, player);
            }
            return playerCooldowns[spell][player];
        }

        public static int getTimeSpanInMinutes(string spell, string player)
        {
            var currentTime = DateTime.Now;
            return (currentTime.Subtract(getCooldown(spell, player))).Minutes;
        }

        public static void setAutoEnable(string spell, string player, bool enable)
        {
            if (!autoEnabled.ContainsKey(spell))
            {
                autoEnabled[spell] = new Dictionary<string, bool>();
            }
            autoEnabled[spell][player] = enable;
        }
        public static bool getAutoEnable(string spell, string player)
        {
            if (!autoEnabled.ContainsKey(spell))
            {
                setAutoEnable(spell, player, false);
            }
            return autoEnabled[spell][player];
        }
    }
}
