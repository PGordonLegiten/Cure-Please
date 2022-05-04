using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class CureHelper
    {
        PlayerHelper _playerHelper;
        public CureHelper(EliteAPI plApi, EliteAPI monitoredApi)
        {
            _playerHelper = new PlayerHelper(plApi, monitoredApi);
        }

        public string GetCureSpell(string cureSpell)
        {
            if (cureSpell.ToLower() == "cure vi")
            {
                if (_playerHelper.CanCastSpellNow("Cure VI"))
                {
                    return "Cure VI";
                }
                else if (_playerHelper.CanCastSpellNow("Cure V") && OptionsForm.config.Undercure)
                {
                    return "Cure V";
                }
                else if (_playerHelper.CanCastSpellNow("Cure IV") && OptionsForm.config.Undercure)
                {
                    return "Cure IV";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "cure v")
            {
                if (_playerHelper.CanCastSpellNow("Cure V"))
                {
                    return "Cure V";
                }
                else if (_playerHelper.CanCastSpellNow("Cure IV") && OptionsForm.config.Undercure)
                {
                    return "Cure IV";
                }
                else if (_playerHelper.CanCastSpellNow("Cure VI") && OptionsForm.config.Overcure)
                {
                    return "Cure VI";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "cure iv")
            {
                if (_playerHelper.CanCastSpellNow("Cure IV"))
                {
                    return "Cure IV";
                }
                else if (_playerHelper.CanCastSpellNow("Cure III") && OptionsForm.config.Undercure)
                {
                    return "Cure III";
                }
                else if (_playerHelper.CanCastSpellNow("Cure V") && OptionsForm.config.Overcure)
                {
                    return "Cure V";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "cure iii")
            {
                if (_playerHelper.CanCastSpellNow("Cure III"))
                {
                    return "Cure III";
                }
                else if (_playerHelper.CanCastSpellNow("Cure IV") && OptionsForm.config.Overcure)
                {
                    return "Cure IV";
                }
                else if (_playerHelper.CanCastSpellNow("Cure II") && OptionsForm.config.Undercure)
                {
                    return "Cure II";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "cure ii")
            {
                if (_playerHelper.CanCastSpellNow("Cure II"))
                {
                    return "Cure II";
                }
                else if (_playerHelper.CanCastSpellNow("Cure") && OptionsForm.config.Undercure)
                {
                    return "Cure";
                }
                else if (_playerHelper.CanCastSpellNow("Cure III") && OptionsForm.config.Overcure)
                {
                    return "Cure III";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "cure")
            {
                if (_playerHelper.CanCastSpellNow("Cure"))
                {
                    return "Cure";
                }
                else if (_playerHelper.CanCastSpellNow("Cure II") && OptionsForm.config.Overcure)
                {
                    return "Cure II";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "curaga v")
            {
                if (_playerHelper.CanCastSpellNow("Curaga V"))
                {
                    return "Curaga V";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga IV") && OptionsForm.config.Undercure)
                {
                    return "Curaga IV";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "curaga iv")
            {
                if (_playerHelper.CanCastSpellNow("Curaga IV"))
                {
                    return "Curaga IV";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga V") && OptionsForm.config.Overcure)
                {
                    return "Curaga V";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga III") && OptionsForm.config.Undercure)
                {
                    return "Curaga III";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "curaga iii")
            {
                if (_playerHelper.CanCastSpellNow("Curaga III"))
                {
                    return "Curaga III";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga IV") && OptionsForm.config.Overcure)
                {
                    return "Curaga IV";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga II") && OptionsForm.config.Undercure)
                {
                    return "Curaga II";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "curaga ii")
            {
                if (_playerHelper.CanCastSpellNow("Curaga II"))
                {
                    return "Curaga II";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga") && OptionsForm.config.Undercure)
                {
                    return "Curaga";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga III") && OptionsForm.config.Overcure)
                {
                    return "Curaga III";
                }
                else
                {
                    return "false";
                }
            }
            else if (cureSpell.ToLower() == "curaga")
            {
                if (_playerHelper.CanCastSpellNow("Curaga"))
                {
                    return "Curaga";
                }
                else if (_playerHelper.CanCastSpellNow("Curaga II")  && OptionsForm.config.Overcure)
                {
                    return "Curaga II";
                }
                else
                {
                    return "false";
                }
            }
            return "false";
        }
    }
}
