using CurePlease.DataStructures;
using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurePlease.Helpers
{
    internal class CureHelper
    {
        PlayerHelper _PlayerHelper;
        EliteAPI _ELITEAPIPL;
        EliteAPI _ELITEAPIMonitored;
        CastingHelper _CastingManager;
        CurePleaseForm _Form;
        public CureHelper(CurePleaseForm form, EliteAPI plApi, EliteAPI monitoredApi, CastingHelper helper)
        {
            _PlayerHelper = new PlayerHelper(plApi, monitoredApi);
            _ELITEAPIPL = plApi;
            _ELITEAPIMonitored = monitoredApi;
            _CastingManager = helper;
            _Form = form;
        }


        public void Run()
        {
            // Set array values for GUI "Enabled" checkboxes
            CheckBox[] enabledBoxes = new CheckBox[18];
            enabledBoxes[0] = _Form.player0enabled;
            enabledBoxes[1] = _Form.player1enabled;
            enabledBoxes[2] = _Form.player2enabled;
            enabledBoxes[3] = _Form.player3enabled;
            enabledBoxes[4] = _Form.player4enabled;
            enabledBoxes[5] = _Form.player5enabled;
            enabledBoxes[6] = _Form.player6enabled;
            enabledBoxes[7] = _Form.player7enabled;
            enabledBoxes[8] = _Form.player8enabled;
            enabledBoxes[9] = _Form.player9enabled;
            enabledBoxes[10] = _Form.player10enabled;
            enabledBoxes[11] = _Form.player11enabled;
            enabledBoxes[12] = _Form.player12enabled;
            enabledBoxes[13] = _Form.player13enabled;
            enabledBoxes[14] = _Form.player14enabled;
            enabledBoxes[15] = _Form.player15enabled;
            enabledBoxes[16] = _Form.player16enabled;
            enabledBoxes[17] = _Form.player17enabled;

            // Set array values for GUI "High Priority" checkboxes
            CheckBox[] highPriorityBoxes = new CheckBox[18];
            highPriorityBoxes[0] = _Form.player0priority;
            highPriorityBoxes[1] = _Form.player1priority;
            highPriorityBoxes[2] = _Form.player2priority;
            highPriorityBoxes[3] = _Form.player3priority;
            highPriorityBoxes[4] = _Form.player4priority;
            highPriorityBoxes[5] = _Form.player5priority;
            highPriorityBoxes[6] = _Form.player6priority;
            highPriorityBoxes[7] = _Form.player7priority;
            highPriorityBoxes[8] = _Form.player8priority;
            highPriorityBoxes[9] = _Form.player9priority;
            highPriorityBoxes[10] = _Form.player10priority;
            highPriorityBoxes[11] = _Form.player11priority;
            highPriorityBoxes[12] = _Form.player12priority;
            highPriorityBoxes[13] = _Form.player13priority;
            highPriorityBoxes[14] = _Form.player14priority;
            highPriorityBoxes[15] = _Form.player15priority;
            highPriorityBoxes[16] = _Form.player16priority;
            highPriorityBoxes[17] = _Form.player17priority;
            bool cureCasted = false;
            List<byte> cures_required = new List<byte>();

            /////////////////////////// PL CURE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (_ELITEAPIPL.Player.HP > 0 && (_ELITEAPIPL.Player.HPP <= OptionsForm.config.monitoredCurePercentage) && OptionsForm.config.enableOutOfPartyHealing == true && !_PlayerHelper.PLInParty())
            {
                cureCasted = CureCalculator_PL();
            }

            /////////////////////////// CURAGA //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            IOrderedEnumerable<EliteAPI.PartyMember> cParty_curaga = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Active != 0 && p.Zone == _ELITEAPIPL.Player.ZoneId).OrderBy(p => p.CurrentHPP);

            int plPartyNumber = GetPLPartyNumber();

            if (plPartyNumber != 0 && plPartyNumber != 4)
            {
                foreach (EliteAPI.PartyMember pData in cParty_curaga)
                {
                    if (plPartyNumber == 1 && pData.MemberNumber >= 0 && pData.MemberNumber <= 5) //pl in first party
                    {
                        if (_PlayerHelper.CastingPossible(pData.Name) && (_ELITEAPIMonitored.Party.GetPartyMembers()[pData.MemberNumber].Active >= 1) && enabledBoxes[pData.MemberNumber].Checked)
                        {
                            if (_ELITEAPIMonitored.Party.GetPartyMembers()[pData.MemberNumber].CurrentHPP <= OptionsForm.config.curagaCurePercentage)
                            {
                                cures_required.Add(pData.MemberNumber);
                            }
                        }
                    }
                    else if (plPartyNumber == 2 && pData.MemberNumber >= 6 && pData.MemberNumber <= 11) //pl in second party
                    {
                        if (_PlayerHelper.CastingPossible(pData.Name) && (_ELITEAPIMonitored.Party.GetPartyMembers()[pData.MemberNumber].Active >= 1) && enabledBoxes[pData.MemberNumber].Checked)
                        {
                            if (_ELITEAPIMonitored.Party.GetPartyMembers()[pData.MemberNumber].CurrentHPP <= OptionsForm.config.curagaCurePercentage)
                            {
                                cures_required.Add(pData.MemberNumber);
                            }
                        }
                    }
                    else if (plPartyNumber == 3 && pData.MemberNumber >= 12 && pData.MemberNumber <= 17) //pl in third party
                    {
                        if (_PlayerHelper.CastingPossible(pData.Name) && (_ELITEAPIMonitored.Party.GetPartyMembers()[pData.MemberNumber].Active >= 1) && enabledBoxes[pData.MemberNumber].Checked)
                        {
                            if (_ELITEAPIMonitored.Party.GetPartyMembers()[pData.MemberNumber].CurrentHPP <= OptionsForm.config.curagaCurePercentage)
                            {
                                cures_required.Add(pData.MemberNumber);
                            }
                        }
                    }
                }

                if (cures_required.Count >= OptionsForm.config.curagaRequiredMembers)
                {
                    int lowestHP_id = cures_required.First();
                    cureCasted = CuragaCalculatorsAsync(lowestHP_id);
                }
            }

            /////////////////////////// CURE //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //var playerHpOrder = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Active >= 1).OrderBy(p => p.CurrentHPP).Select(p => p.Index);
            IEnumerable<byte> playerHpOrder = _ELITEAPIMonitored.Party.GetPartyMembers().OrderBy(p => p.CurrentHPP).Where(p => p.Active >= 1).Select(p => p.MemberNumber);

            // First run a check on the monitored target
            byte playerMonitoredHp = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Name == _ELITEAPIMonitored.Player.Name).OrderBy(p => p.Active == 0).Select(p => p.MemberNumber).FirstOrDefault();

            if (OptionsForm.config.enableMonitoredPriority && _ELITEAPIMonitored.Party.GetPartyMembers()[playerMonitoredHp].Name == _ELITEAPIMonitored.Player.Name && _ELITEAPIMonitored.Party.GetPartyMembers()[playerMonitoredHp].CurrentHP > 0 && (_ELITEAPIMonitored.Party.GetPartyMembers()[playerMonitoredHp].CurrentHPP <= OptionsForm.config.monitoredCurePercentage))
            {
                CureCalculator(playerMonitoredHp);
                cureCasted = true;
            }
            else
            {
                // Now run a scan to check all targets in the High Priority Threshold
                foreach (byte id in playerHpOrder)
                {
                    if ((highPriorityBoxes[id].Checked) && _ELITEAPIMonitored.Party.GetPartyMembers()[id].CurrentHP > 0 && (_ELITEAPIMonitored.Party.GetPartyMembers()[id].CurrentHPP <= OptionsForm.config.priorityCurePercentage))
                    {
                        CureCalculator(id);
                        cureCasted = true;
                        break;
                    }
                }

                // Now run everyone else
                foreach (byte id in playerHpOrder)
                {
                    // Cures First, is casting possible, and enabled?
                    if (_PlayerHelper.CastingPossible(_ELITEAPIMonitored.Party.GetPartyMembers()[id].Name) && (_ELITEAPIMonitored.Party.GetPartyMembers()[id].Active >= 1) && (enabledBoxes[id].Checked) && (_ELITEAPIMonitored.Party.GetPartyMembers()[id].CurrentHP > 0))
                    {
                        if ((_ELITEAPIMonitored.Party.GetPartyMembers()[id].CurrentHPP <= OptionsForm.config.curePercentage) && (_PlayerHelper.CastingPossible(_ELITEAPIMonitored.Party.GetPartyMembers()[id].Name)))
                        {
                            CureCalculator(id);
                            cureCasted = true;
                            break;
                        }
                    }
                }
            }
        }
        private bool CuragaCalculatorsAsync(int partyMemberId)
        {
            if (_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHP > 0)
            {
                if (CuragaCalculatorAsync(partyMemberId, "Cure V", "Curaga V", 380, OptionsForm.config.curaga5Amount, OptionsForm.config.curaga5enabled, CurePrio.CuragaV)) { return true; }
                if (CuragaCalculatorAsync(partyMemberId, "Cure IV", "Curaga IV", 260, OptionsForm.config.curaga4Amount, OptionsForm.config.curaga4enabled, CurePrio.CuragaIV)) { return true; }
                if (CuragaCalculatorAsync(partyMemberId, "Cure III", "Curaga III", 180, OptionsForm.config.curaga3Amount, OptionsForm.config.curaga3enabled, CurePrio.CuragaIII)) { return true; }
                if (CuragaCalculatorAsync(partyMemberId, "Cure II", "Curaga II", 120, OptionsForm.config.curaga2Amount, OptionsForm.config.curaga2enabled, CurePrio.CuragaII)) { return true; }
                if (CuragaCalculatorAsync(partyMemberId, "Cure", "Curaga", 60, OptionsForm.config.curagaAmount, OptionsForm.config.curagaEnabled, CurePrio.CuragaI)) { return true; }
            }
            return false;
        }

        private bool CuragaCalculatorAsync(int partyMemberId, string singleSpell, string aoeSpell, uint mp, int threshHold, bool enabled, CurePrio prio)
        {
            string lowestHP_Name = _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name;
            if (
                enabled
                && (((_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHP * 100 / _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHPP) - _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHP) >= threshHold)
                && _ELITEAPIPL.Player.MP > mp
                &&
                    (
                        _PlayerHelper.IsAbleToCastSpell(aoeSpell)
                     || (OptionsForm.config.Accession && OptionsForm.config.accessionCure && _PlayerHelper.IsAbleToCastSpell(singleSpell))
                    )
            )
            {
                string cureSpell = aoeSpell;
                if (OptionsForm.config.Accession && OptionsForm.config.accessionCure && _PlayerHelper.HasAbility("Accession") && _Form.currentSCHCharges >= 1 && (_ELITEAPIPL.Player.MainJob == 20 || _ELITEAPIPL.Player.SubJob == 20))
                {
                    cureSpell = singleSpell;
                }

                if ((_PlayerHelper.plStatusCheck(StatusEffect.Light_Arts) || _PlayerHelper.plStatusCheck(StatusEffect.Addendum_White)))
                {
                    if (!_PlayerHelper.plStatusCheck(StatusEffect.Accession))
                    {
                        _Form.JobAbility_Wait($"{aoeSpell}, Accession", "Accession");
                    }
                }

                if (OptionsForm.config.curagaTargetType == 0)
                {
                    _CastingManager.QueueSpell(SpellType.Healing, lowestHP_Name, cureSpell, prio);
                    return true;
                }
                else
                {
                    _CastingManager.QueueSpell(SpellType.Healing, OptionsForm.config.curagaTargetName, cureSpell, prio);
                    return true;
                }
            }
            return false;
        }
        public bool CureCalculator_PL()
        {
            // FIRST GET HOW MUCH HP IS MISSING FROM THE CURRENT PARTY MEMBER
            var cureCasted = false;
            if (_ELITEAPIPL.Player.HP > 0)
            {
                uint HP_Loss = (_ELITEAPIPL.Player.HP * 100) / (_ELITEAPIPL.Player.HPP) - (_ELITEAPIPL.Player.HP);

                if (OptionsForm.config.cure6enabled && HP_Loss >= OptionsForm.config.cure6amount && _ELITEAPIPL.Player.MP > 227 && _PlayerHelper.HasSpell("Cure VI"))
                {
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIPL.Player.Name, "Cure VI", CurePrio.CureVI);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure5enabled && HP_Loss >= OptionsForm.config.cure5amount && _ELITEAPIPL.Player.MP > 125 && _PlayerHelper.HasSpell("Cure V"))
                {
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIPL.Player.Name, "Cure V", CurePrio.CureV);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure4enabled && HP_Loss >= OptionsForm.config.cure4amount && _ELITEAPIPL.Player.MP > 88 && _PlayerHelper.HasSpell("Cure IV"))
                {
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIPL.Player.Name, "Cure IV", CurePrio.CureIV);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure3enabled && HP_Loss >= OptionsForm.config.cure3amount && _ELITEAPIPL.Player.MP > 46 && _PlayerHelper.HasSpell("Cure III"))
                {
                    //if (OptionsForm.config.PrioritiseOverLowerTier == true) { RunDebuffChecker(); } TODO MOVE THIS TO CASTING HELPER
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIPL.Player.Name, "Cure III", CurePrio.CureIII);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure2enabled && HP_Loss >= OptionsForm.config.cure2amount && _ELITEAPIPL.Player.MP > 24 && _PlayerHelper.HasSpell("Cure II"))
                {
                    //if (OptionsForm.config.PrioritiseOverLowerTier == true) { RunDebuffChecker(); } TODO MOVE THIS TO CASTING HELPER
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIPL.Player.Name, "Cure II", CurePrio.CureII);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure1enabled && HP_Loss >= OptionsForm.config.cure1amount && _ELITEAPIPL.Player.MP > 8 && _PlayerHelper.HasSpell("Cure"))
                {
                    //if (OptionsForm.config.PrioritiseOverLowerTier == true) { RunDebuffChecker(); } TODO MOVE THIS TO CASTING HELPER
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIPL.Player.Name, "Cure", CurePrio.CureI);
                    cureCasted = true;
                }
            }
            return cureCasted;
        }

        public bool CureCalculator(byte partyMemberId)
        {
            bool cureCasted = false;
            // FIRST GET HOW MUCH HP IS MISSING FROM THE CURRENT PARTY MEMBER
            if (_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHP > 0)
            {
                uint HP_Loss = (_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHP * 100) / (_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHPP) - (_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].CurrentHP);

                if (OptionsForm.config.cure6enabled && HP_Loss >= OptionsForm.config.cure6amount && _ELITEAPIPL.Player.MP > 227 && _PlayerHelper.HasSpell("Cure VI"))
                {
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name, "Cure VI", CurePrio.CureVI);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure5enabled && HP_Loss >= OptionsForm.config.cure5amount && _ELITEAPIPL.Player.MP > 125 && _PlayerHelper.HasSpell("Cure V"))
                {
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name, "Cure V", CurePrio.CureV);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure4enabled && HP_Loss >= OptionsForm.config.cure4amount && _ELITEAPIPL.Player.MP > 88 && _PlayerHelper.HasSpell("Cure IV"))
                {
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name, "Cure IV", CurePrio.CureIV);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure3enabled && HP_Loss >= OptionsForm.config.cure3amount && _ELITEAPIPL.Player.MP > 46 && _PlayerHelper.HasSpell("Cure III"))
                {
                   // if (OptionsForm.config.PrioritiseOverLowerTier == true) { RunDebuffChecker(); } TODO MOVE THIS TO CASTING HELPER
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name, "Cure III", CurePrio.CureIII);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure2enabled && HP_Loss >= OptionsForm.config.cure2amount && _ELITEAPIPL.Player.MP > 24 && _PlayerHelper.HasSpell("Cure II"))
                {
                    //if (OptionsForm.config.PrioritiseOverLowerTier == true) { RunDebuffChecker(); } TODO MOVE THIS TO CASTING HELPER
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name, "Cure II", CurePrio.CureII);
                    cureCasted = true;
                }
                else if (OptionsForm.config.cure1enabled && HP_Loss >= OptionsForm.config.cure1amount && _ELITEAPIPL.Player.MP > 8 && _PlayerHelper.HasSpell("Cure"))
                {
                    //if (OptionsForm.config.PrioritiseOverLowerTier == true) { RunDebuffChecker(); } TODO MOVE THIS TO CASTING HELPER
                    _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Name, "Cure", CurePrio.CureI);
                    cureCasted = true;
                }
            }
            return cureCasted;
        }

        public string GetCureSpell(string cureSpell)
        {
            if (cureSpell.ToLower() == "cure vi")
            {
                if (_PlayerHelper.CanCastSpellNow("Cure VI"))
                {
                    return "Cure VI";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure V") && OptionsForm.config.Undercure)
                {
                    return "Cure V";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure IV") && OptionsForm.config.Undercure)
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
                if (_PlayerHelper.CanCastSpellNow("Cure V"))
                {
                    return "Cure V";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure IV") && OptionsForm.config.Undercure)
                {
                    return "Cure IV";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure VI") && OptionsForm.config.Overcure)
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
                if (_PlayerHelper.CanCastSpellNow("Cure IV"))
                {
                    return "Cure IV";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure III") && OptionsForm.config.Undercure)
                {
                    return "Cure III";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure V") && OptionsForm.config.Overcure)
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
                if (_PlayerHelper.CanCastSpellNow("Cure III"))
                {
                    return "Cure III";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure IV") && OptionsForm.config.Overcure)
                {
                    return "Cure IV";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure II") && OptionsForm.config.Undercure)
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
                if (_PlayerHelper.CanCastSpellNow("Cure II"))
                {
                    return "Cure II";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure") && OptionsForm.config.Undercure)
                {
                    return "Cure";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure III") && OptionsForm.config.Overcure)
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
                if (_PlayerHelper.CanCastSpellNow("Cure"))
                {
                    return "Cure";
                }
                else if (_PlayerHelper.CanCastSpellNow("Cure II") && OptionsForm.config.Overcure)
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
                if (_PlayerHelper.CanCastSpellNow("Curaga V"))
                {
                    return "Curaga V";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga IV") && OptionsForm.config.Undercure)
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
                if (_PlayerHelper.CanCastSpellNow("Curaga IV"))
                {
                    return "Curaga IV";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga V") && OptionsForm.config.Overcure)
                {
                    return "Curaga V";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga III") && OptionsForm.config.Undercure)
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
                if (_PlayerHelper.CanCastSpellNow("Curaga III"))
                {
                    return "Curaga III";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga IV") && OptionsForm.config.Overcure)
                {
                    return "Curaga IV";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga II") && OptionsForm.config.Undercure)
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
                if (_PlayerHelper.CanCastSpellNow("Curaga II"))
                {
                    return "Curaga II";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga") && OptionsForm.config.Undercure)
                {
                    return "Curaga";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga III") && OptionsForm.config.Overcure)
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
                if (_PlayerHelper.CanCastSpellNow("Curaga"))
                {
                    return "Curaga";
                }
                else if (_PlayerHelper.CanCastSpellNow("Curaga II")  && OptionsForm.config.Overcure)
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
        public int GetPLPartyNumber()
        {
            // FIRST CHECK THAT BOTH THE PL AND MONITORED PLAYER ARE IN THE SAME PT/ALLIANCE
            List<EliteAPI.PartyMember> currentPT = _ELITEAPIMonitored.Party.GetPartyMembers();

            int partyChecker = 0;

            foreach (EliteAPI.PartyMember PTMember in currentPT)
            {
                if (PTMember.Name == _ELITEAPIPL.Player.Name)
                {
                    partyChecker++;
                }
                if (PTMember.Name == _ELITEAPIMonitored.Player.Name)
                {
                    partyChecker++;
                }
            }

            if (partyChecker >= 2)
            {
                int plParty = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Name == _ELITEAPIPL.Player.Name).Select(p => p.MemberNumber).FirstOrDefault();

                if (plParty <= 5)
                {
                    return 1;
                }
                else if (plParty <= 11 && plParty >= 6)
                {
                    return 2;
                }
                else if (plParty <= 17 && plParty >= 12)
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 4;
            }
        }
    }
}
