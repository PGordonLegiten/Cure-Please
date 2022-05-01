using EliteMMO.API;
using Microsoft.Build.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class PlayerHelper
    {
        private static EliteAPI _ELITEAPIPL;
        private static EliteAPI _ELITEAPIMonitored;
        public PlayerHelper(EliteAPI plApi, EliteAPI monitoredApi)
        {
            _ELITEAPIPL = plApi;
            _ELITEAPIMonitored = monitoredApi;
        }

        public bool isAbleToCastSpellWithMp(string spell, string name)
        {
            return IsAbleToCastSpell(spell) && _ELITEAPIPL.Player.MP > OptionsForm.config.mpMinCastValue;
        }

        public bool IsAbleToCastSpell(string spell)
        {
            return HasSpell(spell) && JobChecker(spell) == true;
        }

        public bool isAbleToBuff(string spell, string player)
        {
            return isAbleToCastSpellWithMp(spell, player) && InternalHelper.getAutoEnable(spell, player);
        }

        public bool CastingPossible(string memberName)
        {
            var member = _ELITEAPIMonitored.Party.GetPartyMembers().Where(x => x.Name == memberName).FirstOrDefault();
            if (member == null) { return false; }
            if ((_ELITEAPIPL.Entity.GetEntity((int)member.TargetIndex).Distance < 21) && (_ELITEAPIPL.Entity.GetEntity((int)member.TargetIndex).Distance > 0) && (member.CurrentHP > 0) || (_ELITEAPIPL.Party.GetPartyMember(0).ID == member.ID) && (member.CurrentHP > 0))
            {
                return true;
            }
            return false;
        }

        public bool raisePossible(string memberName)
        {
            var member = _ELITEAPIMonitored.Party.GetPartyMembers().Where(x => x.Name == memberName).FirstOrDefault();
            if (member == null) { return false; }
            if ((_ELITEAPIPL.Entity.GetEntity((int)member.TargetIndex).Distance < 21) && (_ELITEAPIPL.Entity.GetEntity((int)member.TargetIndex).Distance > 0) || (_ELITEAPIPL.Party.GetPartyMember(0).ID == member.ID))
            {
                return true;
            }
            return false;
        }

        public bool JobChecker(string SpellName)
        {

            string checked_spellName = SpellName.Trim().ToLower();

            EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(checked_spellName, 0); // GRAB THE REQUESTED SPELL DATA

            int mainjobLevelRequired = magic.LevelRequired[(_ELITEAPIPL.Player.MainJob)]; // GRAB SPELL LEVEL FOR THE MAIN JOB
            int subjobLevelRequired = magic.LevelRequired[(_ELITEAPIPL.Player.SubJob)]; // GRAB SPELL LEVEL FOR THE SUB JOB

            if (checked_spellName == "honor march")
            {
                return true;
            }

            if (mainjobLevelRequired <= _ELITEAPIPL.Player.MainJobLevel && mainjobLevelRequired != -1)
            { // IF THE MAIN JOB DOES NOT EQUAl -1 (Meaning the JOB can't use the spell) AND YOUR LEVEL IS EQUAL TO OR LOVER THAN THE REQUIRED LEVEL RETURN true
                return true;
            }
            else if (subjobLevelRequired <= _ELITEAPIPL.Player.SubJobLevel && subjobLevelRequired != -1)
            { // IF THE SUB JOB DOES NOT EQUAl -1 (Meaning the JOB can't use the spell) AND YOUR LEVEL IS EQUAL TO OR LOVER THAN THE REQUIRED LEVEL RETURN true
                return true;
            }
            else if (mainjobLevelRequired > 99 && mainjobLevelRequired != -1)
            { // IF THE MAIN JOB LEVEL IS GREATER THAN 99 BUT DOES NOT EQUAL -1 THEN IT IS A JOB POINT REQUIRED SPELL AND SO FURTHER CHECKS MUST BE MADE SO GRAB CURRENT JOB POINT TABLE
                EliteAPI.PlayerJobPoints JobPoints = _ELITEAPIPL.Player.GetJobPoints(_ELITEAPIPL.Player.MainJob);

                // Spell is a JP spell so check this works correctly and that you possess the spell
                if (checked_spellName == "refresh iii" || checked_spellName == "temper ii")
                {
                    if (_ELITEAPIPL.Player.MainJob == 5 && _ELITEAPIPL.Player.MainJobLevel == 99 && JobPoints.SpentJobPoints >= 1200) // IF MAIN JOB IS RDM, AND JOB LEVEL IS AT MAX WITH REQUIRED JOB POINTS
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (checked_spellName == "distract iii" || checked_spellName == "frazzle iii")
                {
                    if (_ELITEAPIPL.Player.MainJob == 5 && _ELITEAPIPL.Player.MainJobLevel == 99 && JobPoints.SpentJobPoints >= 550) // IF MAIN JOB IS RDM, AND JOB LEVEL IS AT MAX WITH REQUIRED JOB POINTS
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (checked_spellName.Contains("storm ii"))
                {
                    if (_ELITEAPIPL.Player.MainJob == 20 && _ELITEAPIPL.Player.MainJobLevel == 99 && JobPoints.SpentJobPoints >= 100) // IF MAIN JOB IS SCH, AND JOB LEVEL IS AT MAX WITH REQUIRED JOB POINTS
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (checked_spellName == "reraise iv")
                {
                    if (_ELITEAPIPL.Player.MainJob == 3 && _ELITEAPIPL.Player.MainJobLevel == 99 && JobPoints.SpentJobPoints >= 100) // IF MAIN JOB IS WHM, AND JOB LEVEL IS AT MAX WITH REQUIRED JOB POINTS
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (checked_spellName == "full cure")
                {
                    if (_ELITEAPIPL.Player.MainJob == 3 && _ELITEAPIPL.Player.MainJobLevel == 99 && JobPoints.SpentJobPoints >= 1200) // IF MAIN JOB IS WHM, AND JOB LEVEL IS AT MAX WITH REQUIRED JOB POINTS
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool HasSpell(string checked_spellName)
        {

            checked_spellName = checked_spellName.Trim().ToLower();

            if (checked_spellName == "honor march")
            {
                return true;
            }

            EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(checked_spellName, 0);

            if (_ELITEAPIPL.Player.GetPlayerInfo().Buffs.Any(b => b == 262)) // IF YOU HAVE OMERTA THEN BLOCK MAGIC CASTING
            {
                return false;
            }
            else if (_ELITEAPIPL.Player.HasSpell(magic.Index) && JobChecker(checked_spellName) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // TODO
        // REMOVE THIS FROM UNIVERSE
        // SPELL RECAST SHOULD BE HANDLED IN CASTING HELPER
        public int CheckSpellRecast(string checked_recastspellName)
        {
            return 0;
            checked_recastspellName = checked_recastspellName.Trim().ToLower();

            if (checked_recastspellName == "honor march")
            {
                return 0;
            }

            if (checked_recastspellName != "blank")
            {
                EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(checked_recastspellName, 0);

                if (magic == null)
                {
                    throw new Exception("Error detected, please Report Error: #SpellRecastError #" + checked_recastspellName);
                }
                else
                {
                    if (_ELITEAPIPL.Recast.GetSpellRecast(magic.Index) == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
