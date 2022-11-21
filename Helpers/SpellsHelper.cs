using CurePlease.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class SpellsHelper
    {

        static string[] shell_spells = { "Shell", "Shell II", "Shell III", "Shell IV", "Shell V" };
        static string[] protect_spells = { "Protect", "Protect II", "Protect III", "Protect IV", "Protect V" };
        static string[] refresh_spells = { "Refresh", "Refresh II", "Refresh III" };
        static string[] regen_spells = { "Regen", "Regen II", "Regen III", "Regen IV", "Regen V", "Regen VI" };
        static string[] raise_spells = { "Arise", "Raise III", "Raise II", "Raise" };

        private PlayerHelper _PlayerHelper;

        public SpellsHelper(PlayerHelper playerHelper)
        {
            _PlayerHelper = playerHelper;
        }

        public static string GetShellSpell()
        {
            return shell_spells[OptionsForm.config.autoShell_Spell];
        }
        public static string GetProtectSpell()
        {
            return protect_spells[OptionsForm.config.autoProtect_Spell];
        }
        public static string GetRefreshSpell()
        {
            return refresh_spells[OptionsForm.config.autoRefresh_Spell];
        }
        public static string GetRegenSpell()
        {
            return regen_spells[OptionsForm.config.autoRegen_Spell];
        }

        public static string GetStormVersion(string name)
        {
            var storms = new Dictionary<string, string[]>()
            {
                { "Sandstorm", new string[]{ "Sandstorm" , "Sandstorm II" } },
                { "Windstorm", new string[]{ "Windstorm", "Windstorm II" } },
                { "Firestorm", new string[]{ "Firestorm", "Firestorm II" } },
                { "Rainstorm", new string[]{ "Rainstorm", "Rainstorm II" } },
                { "Hailstorm", new string[]{ "Hailstorm", "Hailstorm II" } },
                { "Thunderstorm", new string[]{ "Thunderstorm", "Thunderstorm II" } },
                { "Voidstorm", new string[]{ "Voidstorm", "Voidstorm II" } },
                { "Aurorastorm", new string[]{ "Aurorastorm", "Aurorastorm II" } },
                { "Dummy", new string[]{ "Dummy", "Dummy" } }, 
            };

            return storms[name][OptionsForm.config.autoStorm_Spell];
        }

        public string GetRaiseSpell()
        {
            foreach(var spell in raise_spells)
            {
                if (_PlayerHelper.CanCastSpellNow(spell))
                {
                    return spell;
                }
            }
            return null;
        }

        public static string GetEnabledStormSpell(string name)
        {
            if (InternalHelper.getAutoEnable(GetStormVersion("Sandstorm"), name))
            {
                return "Sandstorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Windstorm"), name))
            {
                return "Windstorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Firestorm"), name))
            {
                return "Firestorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Rainstorm"), name))
            {
                return "Rainstorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Hailstorm"), name))
            {
                return "Hailstorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Thunderstorm"), name))
            {
                return "Thunderstorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Voidstorm"), name))
            {
                return "Voidstorm";
            }
            else if (InternalHelper.getAutoEnable(GetStormVersion("Aurorastorm"), name))
            {
                return "Aurorastorm";
            }
            else
            {
                return "Dummy";
            }
        }
    }
}
