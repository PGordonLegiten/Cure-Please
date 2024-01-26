using CurePlease.DataStructures;
using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    class DebufHelper
    {
        EliteAPI _ELITEAPIPL;
        EliteAPI _ELITEAPIMonitored;
        CastingHelper _CastingManager;
        CurePleaseForm _Form;
        PlayerHelper _PlayerHelper;
        CureHelper _CureHelper;
        public DebufHelper(CurePleaseForm form, EliteAPI plApi, EliteAPI monitoredApi, CastingHelper helper, PlayerHelper pHelper, CureHelper cHelper)
        {
            _PlayerHelper = pHelper;
            _ELITEAPIPL = plApi;
            _ELITEAPIMonitored = monitoredApi;
            _CastingManager = helper;
            _CureHelper = cHelper;
            _Form = form;
        }
        public void Run()
        {
            // PL and Monitored Player Debuff Removal Starting with PL

            var wakeSleepSpellName = string.Empty;

            if (OptionsForm.config.wakeSleepSpell == 0)
            {
                wakeSleepSpellName = "Cure";
            }
            else if (OptionsForm.config.wakeSleepSpell == 1)
            {
                wakeSleepSpellName = "Cura";
            }
            else if (OptionsForm.config.wakeSleepSpell == 2)
            {
                wakeSleepSpellName = "Curaga";
            }

            foreach (StatusEffect plEffect in _ELITEAPIPL.Player.Buffs)
            {
                #region "pl_debuffs"
                if ((plEffect == StatusEffect.Doom) && (OptionsForm.config.plDoom) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                {
                    _CastingManager.QueueSpell(SpellType.Prio, _ELITEAPIPL.Player.Name, "Cursna");
                }
                if ((plEffect == StatusEffect.Paralysis) && (OptionsForm.config.plParalysis) && _PlayerHelper.IsAbleToCastSpell("Paralyna"))
                {
                    _CastingManager.QueueSpell(SpellType.Prio, _ELITEAPIPL.Player.Name, "Paralyna");
                }
                if ((plEffect == StatusEffect.Amnesia) && (OptionsForm.config.plAmnesia) && _PlayerHelper.IsAbleToCastSpell("Esuna") && BuffChecker(0, 418))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Esuna");
                }
                if ((plEffect == StatusEffect.Poison) && (OptionsForm.config.plPoison) && _PlayerHelper.IsAbleToCastSpell("Poisona"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Poisona");
                }
                if ((plEffect == StatusEffect.Attack_Down) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Blindness) && (OptionsForm.config.plBlindness) && _PlayerHelper.IsAbleToCastSpell("Blindna"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Blindna");
                }
                if ((plEffect == StatusEffect.Bind) && (OptionsForm.config.plBind) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Weight) && (OptionsForm.config.plWeight) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Slow) && (OptionsForm.config.plSlow) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Curse) && (OptionsForm.config.plCurse) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Cursna", SpellPrio.High);
                }
                if ((plEffect == StatusEffect.Curse2) && (OptionsForm.config.plCurse2) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Cursna", SpellPrio.High);
                }
                if ((plEffect == StatusEffect.Addle) && (OptionsForm.config.plAddle) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Bane) && (OptionsForm.config.plBane) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Cursna");
                }
                if ((plEffect == StatusEffect.Plague) && (OptionsForm.config.plPlague) && _PlayerHelper.IsAbleToCastSpell("Viruna"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Viruna");
                }
                if ((plEffect == StatusEffect.Disease) && (OptionsForm.config.plDisease) && _PlayerHelper.IsAbleToCastSpell("Viruna"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Viruna");
                }
                if ((plEffect == StatusEffect.Burn) && (OptionsForm.config.plBurn) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Frost) && (OptionsForm.config.plFrost) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Choke) && (OptionsForm.config.plChoke) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Rasp) && (OptionsForm.config.plRasp) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Shock) && (OptionsForm.config.plShock) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Drown) && (OptionsForm.config.plDrown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Dia) && (OptionsForm.config.plDia) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Bio) && (OptionsForm.config.plBio) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.STR_Down) && (OptionsForm.config.plStrDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.DEX_Down) && (OptionsForm.config.plDexDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.VIT_Down) && (OptionsForm.config.plVitDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.AGI_Down) && (OptionsForm.config.plAgiDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.INT_Down) && (OptionsForm.config.plIntDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.MND_Down) && (OptionsForm.config.plMndDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.CHR_Down) && (OptionsForm.config.plChrDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Max_HP_Down) && (OptionsForm.config.plMaxHpDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Max_MP_Down) && (OptionsForm.config.plMaxMpDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Accuracy_Down) && (OptionsForm.config.plAccuracyDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Evasion_Down) && (OptionsForm.config.plEvasionDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Defense_Down) && (OptionsForm.config.plDefenseDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Flash) && (OptionsForm.config.plFlash) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Magic_Acc_Down) && (OptionsForm.config.plMagicAccDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Magic_Atk_Down) && (OptionsForm.config.plMagicAtkDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Helix) && (OptionsForm.config.plHelix) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Max_TP_Down) && (OptionsForm.config.plMaxTpDown) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Requiem) && (OptionsForm.config.plRequiem) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Elegy) && (OptionsForm.config.plElegy) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Threnody) && (OptionsForm.config.plThrenody) && (OptionsForm.config.plAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase"))
                {
                    _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                #endregion
            }
            #region "monitored_player"
            // Next, we check monitored player
            if (_PlayerHelper.CastingPossible(_ELITEAPIMonitored.Player.Name))
            {
                foreach (StatusEffect monitoredEffect in _ELITEAPIMonitored.Player.Buffs)
                {
                    if ((monitoredEffect == StatusEffect.Doom) && (OptionsForm.config.monitoredDoom) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna", SpellPrio.Top);
                    }
                    if ((monitoredEffect == StatusEffect.Sleep) && (OptionsForm.config.monitoredSleep) && (OptionsForm.config.wakeSleepEnabled))
                    {
                        _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Player.Name, wakeSleepSpellName, CurePrio.CureIV);
                    }
                    if ((monitoredEffect == StatusEffect.Sleep2) && (OptionsForm.config.monitoredSleep2) && (OptionsForm.config.wakeSleepEnabled))
                    {
                        _CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Player.Name, wakeSleepSpellName, CurePrio.CureIV);
                    }
                    if ((monitoredEffect == StatusEffect.Silence) && (OptionsForm.config.monitoredSilence) && _PlayerHelper.IsAbleToCastSpell("Silena"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Silena");
                    }
                    if ((monitoredEffect == StatusEffect.Petrification) && (OptionsForm.config.monitoredPetrification) && _PlayerHelper.IsAbleToCastSpell("Stona"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Stona", SpellPrio.High);
                    }
                    if ((monitoredEffect == StatusEffect.Paralysis) && (OptionsForm.config.monitoredParalysis) && _PlayerHelper.IsAbleToCastSpell("Paralyna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Paralyna");
                    }
                    if ((monitoredEffect == StatusEffect.Amnesia) && (OptionsForm.config.monitoredAmnesia) && _PlayerHelper.IsAbleToCastSpell("Esuna") && BuffChecker(0, 418))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Esuna");
                    }
                    if ((monitoredEffect == StatusEffect.Poison) && (OptionsForm.config.monitoredPoison) && _PlayerHelper.IsAbleToCastSpell("Poisona"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Poisona");
                    }
                    if ((monitoredEffect == StatusEffect.Attack_Down) && (OptionsForm.config.monitoredAttackDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Blindness) && (OptionsForm.config.monitoredBlindness) && _PlayerHelper.IsAbleToCastSpell("Blindna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Blindna");
                    }
                    if ((monitoredEffect == StatusEffect.Bind) && (OptionsForm.config.monitoredBind) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Weight) && (OptionsForm.config.monitoredWeight) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Slow) && (OptionsForm.config.monitoredSlow) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Curse) && (OptionsForm.config.monitoredCurse) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna", SpellPrio.High);
                    }
                    if ((monitoredEffect == StatusEffect.Curse2) && (OptionsForm.config.monitoredCurse2) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna", SpellPrio.High);
                    }
                    if ((monitoredEffect == StatusEffect.Addle) && (OptionsForm.config.monitoredAddle) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Bane) && (OptionsForm.config.monitoredBane) && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna");
                    }
                    if ((monitoredEffect == StatusEffect.Plague) && (OptionsForm.config.monitoredPlague) && _PlayerHelper.IsAbleToCastSpell("Viruna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Viruna");
                    }
                    if ((monitoredEffect == StatusEffect.Disease) && (OptionsForm.config.monitoredDisease) && _PlayerHelper.IsAbleToCastSpell("Viruna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Viruna");
                    }
                    if ((monitoredEffect == StatusEffect.Burn) && (OptionsForm.config.monitoredBurn) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Frost) && (OptionsForm.config.monitoredFrost) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Choke) && (OptionsForm.config.monitoredChoke) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Rasp) && (OptionsForm.config.monitoredRasp) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Shock) && (OptionsForm.config.monitoredShock) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Drown) && (OptionsForm.config.monitoredDrown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Dia) && (OptionsForm.config.monitoredDia) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Bio) && (OptionsForm.config.monitoredBio) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.STR_Down) && (OptionsForm.config.monitoredStrDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.DEX_Down) && (OptionsForm.config.monitoredDexDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.VIT_Down) && (OptionsForm.config.monitoredVitDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.AGI_Down) && (OptionsForm.config.monitoredAgiDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.INT_Down) && (OptionsForm.config.monitoredIntDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.MND_Down) && (OptionsForm.config.monitoredMndDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.CHR_Down) && (OptionsForm.config.monitoredChrDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Max_HP_Down) && (OptionsForm.config.monitoredMaxHpDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Max_MP_Down) && (OptionsForm.config.monitoredMaxMpDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Accuracy_Down) && (OptionsForm.config.monitoredAccuracyDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Evasion_Down) && (OptionsForm.config.monitoredEvasionDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Defense_Down) && (OptionsForm.config.monitoredDefenseDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Flash) && (OptionsForm.config.monitoredFlash) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Magic_Acc_Down) && (OptionsForm.config.monitoredMagicAccDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Magic_Atk_Down) && (OptionsForm.config.monitoredMagicAtkDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Helix) && (OptionsForm.config.monitoredHelix) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Max_TP_Down) && (OptionsForm.config.monitoredMaxTpDown) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Requiem) && (OptionsForm.config.monitoredRequiem) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Elegy) && (OptionsForm.config.monitoredElegy) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Threnody) && (OptionsForm.config.monitoredThrenody) && _PlayerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                }
            }
            // End MONITORED Debuff Removal
            #endregion
          
        }
        public bool plMonitoredSameParty()
        {
            int PT_Structutre_NO = _CureHelper.GetPLPartyNumber();

            // Now generate the party
            IEnumerable<EliteAPI.PartyMember> cParty = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Active != 0 && p.Zone == _ELITEAPIPL.Player.ZoneId);

            // Make sure member number is not 0 (null) or 4 (void)
            if (PT_Structutre_NO != 0 && PT_Structutre_NO != 4)
            {
                // Run through Each party member as we're looking for either a specific name or if set
                // otherwise anyone with the MP criteria in the current party.
                foreach (EliteAPI.PartyMember pData in cParty)
                {
                    if (PT_Structutre_NO == 1 && pData.MemberNumber >= 0 && pData.MemberNumber <= 5 && pData.Name == _ELITEAPIMonitored.Player.Name)
                    {
                        return true;
                    }
                    else if (PT_Structutre_NO == 2 && pData.MemberNumber >= 6 && pData.MemberNumber <= 11 && pData.Name == _ELITEAPIMonitored.Player.Name)
                    {
                        return true;
                    }
                    else if (PT_Structutre_NO == 3 && pData.MemberNumber >= 12 && pData.MemberNumber <= 17 && pData.Name == _ELITEAPIMonitored.Player.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private bool DebuffContains(List<string> Debuff_list, string Checked_id)
        {
            if (Debuff_list != null)
            {
                if (Debuff_list.Any(x => x == Checked_id))
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
        public bool BuffChecker(int buffID, int checkedPlayer)
        {
            if (checkedPlayer == 1)
            {
                if (_ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Any(b => b == buffID))
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
                if (_ELITEAPIPL.Player.GetPlayerInfo().Buffs.Any(b => b == buffID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void UpdateBuffs(string charName, string characterBuffs)
        {
            if (!_PlayerHelper.IsAlive(charName)) { return; }; //no love for the dead

            var wakeSleepSpellName = string.Empty;

            if (OptionsForm.config.wakeSleepSpell == 0)
            {
                wakeSleepSpellName = "Cure";
            }
            else if (OptionsForm.config.wakeSleepSpell == 1)
            {
                wakeSleepSpellName = "Cura";
            }
            else if (OptionsForm.config.wakeSleepSpell == 2)
            {
                wakeSleepSpellName = "Curaga";
            }
            List<string> named_Debuffs = characterBuffs.Split(',').ToList();

            if (named_Debuffs != null && named_Debuffs.Count() != 0)
            {
                named_Debuffs = named_Debuffs.Select(t => t.Trim()).ToList();

                #region "resetting_debuff_timers"

                // IF SLOW IS NOT ACTIVE, YET NEITHER IS HASTE / FLURRY DESPITE BEING ENABLED
                // RESET THE TIMER TO FORCE IT TO BE CAST
                // 13 slow
                // 33 haste
                // 580 haste - twice in buff table
                if (!DebuffContains(named_Debuffs, "13") && !DebuffContains(named_Debuffs, "33") && !DebuffContains(named_Debuffs, "580") && !_PlayerHelper.plStatusCheck(StatusEffect.Slow))
                {
                    if (charName == "Helmaru")
                    {
                        Console.WriteLine($"Helmaru Reseting cooldown Haste II");
                    }
                    InternalHelper.resetCooldown("Haste", charName);
                    InternalHelper.resetCooldown("Haste II", charName);
                }
                //slow 13
                //flurry 265
                //flurry 581 - twice in buff table
                if (!DebuffContains(named_Debuffs, "13") && !DebuffContains(named_Debuffs, "265") && !DebuffContains(named_Debuffs, "581"))
                {
                    InternalHelper.resetCooldown("Flurry", charName);
                    InternalHelper.resetCooldown("Flurry II", charName);
                }
                // IF SUBLIMATION IS NOT ACTIVE, YET NEITHER IS REFRESH DESPITE BEING
                // ENABLED RESET THE TIMER TO FORCE IT TO BE CAST
                if (!DebuffContains(named_Debuffs, "187") && !DebuffContains(named_Debuffs, "188") && !DebuffContains(named_Debuffs, "43") && !DebuffContains(named_Debuffs, "541"))
                {
                    if (charName == "Helmaru")
                    {
                        Console.WriteLine($"Helmaru Reseting cooldown Phalanx II");
                    }
                    InternalHelper.resetCooldown(SpellsHelper.GetRefreshSpell(), charName);
                }
                else
                {
                    InternalHelper.setCooldown(SpellsHelper.GetRefreshSpell(), charName);
                }
                // IF REGEN IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER TO
                // FORCE IT TO BE CAST
                if (!DebuffContains(named_Debuffs, "42"))
                {
                    InternalHelper.resetCooldown(SpellsHelper.GetRegenSpell(), charName);
                }
                // IF PROTECT IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER TO
                // FORCE IT TO BE CAST
                if (!DebuffContains(named_Debuffs, "40"))
                {
                    InternalHelper.resetCooldown(SpellsHelper.GetProtectSpell(), charName);
                }

                // IF SHELL IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER TO
                // FORCE IT TO BE CAST
                if (!DebuffContains(named_Debuffs, "41"))
                {
                    InternalHelper.resetCooldown(SpellsHelper.GetShellSpell(), charName);
                }
                // IF PHALANX II IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER
                // TO FORCE IT TO BE CAST
                if (!DebuffContains(named_Debuffs, "116"))
                {
                    if (charName == "Helmaru")
                    {
                        Console.WriteLine($"Helmaru Reseting cooldown Phalanx II");
                    }
                    InternalHelper.resetCooldown("Phalanx II", charName);

                }
                // IF NO STORM SPELL IS ACTIVE DESPITE BEING ENABLED RESET THE TIMER
                // TO FORCE IT TO BE CAST
                if (SpellsHelper.GetEnabledStormSpell(charName) != "Dummy" && !DebuffContains(named_Debuffs, "178") && !DebuffContains(named_Debuffs, "179") && !DebuffContains(named_Debuffs, "180") && !DebuffContains(named_Debuffs, "181") &&
                    !DebuffContains(named_Debuffs, "182") && !DebuffContains(named_Debuffs, "183") && !DebuffContains(named_Debuffs, "184") && !DebuffContains(named_Debuffs, "185") &&
                    !DebuffContains(named_Debuffs, "589") && !DebuffContains(named_Debuffs, "590") && !DebuffContains(named_Debuffs, "591") && !DebuffContains(named_Debuffs, "592") &&
                    !DebuffContains(named_Debuffs, "593") && !DebuffContains(named_Debuffs, "594") && !DebuffContains(named_Debuffs, "595") && !DebuffContains(named_Debuffs, "596"))
                {
                    InternalHelper.resetCooldown(SpellsHelper.GetStormVersion(SpellsHelper.GetEnabledStormSpell(charName)), charName);
                }

                // ==============================================================================================================================================================================
                // PARTY DEBUFF REMOVAL

                if (OptionsForm.config.enablePartyDebuffRemoval && !string.IsNullOrEmpty(charName) && (_Form.characterNames_naRemoval.Contains(charName) || OptionsForm.config.SpecifiednaSpellsenable == false))
                {
                    //DOOM
                    if (OptionsForm.config.naCurse && DebuffContains(named_Debuffs, "15") && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Cursna", SpellPrio.Top);
                    }
                    //SLEEP
                    if (DebuffContains(named_Debuffs, "2") && _PlayerHelper.HasSpell(wakeSleepSpellName))
                    {
                        _CastingManager.QueueSpell(SpellType.Healing, charName, wakeSleepSpellName, CurePrio.CureIV);
                        //removeDebuff(charName, 2);
                    }
                    //SLEEP 2
                    if (DebuffContains(named_Debuffs, "19") && _PlayerHelper.HasSpell(wakeSleepSpellName))
                    {
                        _CastingManager.QueueSpell(SpellType.Healing, charName, wakeSleepSpellName, CurePrio.CureIV);
                        //removeDebuff(charName, 19);
                    }
                    //PETRIFICATION
                    if (OptionsForm.config.naPetrification && DebuffContains(named_Debuffs, "7") && _PlayerHelper.IsAbleToCastSpell("Stona"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Stona", SpellPrio.High);
                        //removeDebuff(charName, 7);
                    }
                    //SILENCE
                    if (OptionsForm.config.naSilence && DebuffContains(named_Debuffs, "6") && _PlayerHelper.IsAbleToCastSpell("Silena"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Silena");
                        //removeDebuff(charName, 6);
                    }
                    //PARALYSIS
                    if (OptionsForm.config.naParalysis && DebuffContains(named_Debuffs, "4") && _PlayerHelper.IsAbleToCastSpell("Paralyna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Paralyna");
                        //removeDebuff(charName, 4);
                    }
                    // PLAGUE
                    if (OptionsForm.config.naDisease && DebuffContains(named_Debuffs, "31") && _PlayerHelper.IsAbleToCastSpell("Viruna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Viruna");
                        //removeDebuff(charName, 31);
                    }
                    //DISEASE
                    if (OptionsForm.config.naDisease && DebuffContains(named_Debuffs, "8") && _PlayerHelper.IsAbleToCastSpell("Viruna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Viruna");
                        //removeDebuff(charName, 8);
                    }
                    // AMNESIA
                    if (OptionsForm.config.Esuna && DebuffContains(named_Debuffs, "16") && _PlayerHelper.IsAbleToCastSpell("Esuna") && BuffChecker(1, 418))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Esuna");
                        //removeDebuff(charName, 16);
                    }
                    //CURSE
                    if (OptionsForm.config.naCurse && DebuffContains(named_Debuffs, "9") && _PlayerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Cursna", SpellPrio.High);
                        //removeDebuff(charName, 9);
                    }
                    //BLINDNESS
                    if (OptionsForm.config.naBlindness && DebuffContains(named_Debuffs, "5") && _PlayerHelper.IsAbleToCastSpell("Blindna"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Blindna");
                        //removeDebuff(charName, 5);
                    }
                    //POISON
                    if (OptionsForm.config.naPoison && DebuffContains(named_Debuffs, "3") && _PlayerHelper.IsAbleToCastSpell("Poisona"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Poisona");
                        //removeDebuff(charName, 3);
                    }


                    // SLOW
                    if (OptionsForm.config.naErase == true && OptionsForm.config.na_Slow && DebuffContains(named_Debuffs, "13") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 13);
                    }
                    // BIO
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Bio && DebuffContains(named_Debuffs, "135") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 135);
                    }
                    // BIND
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Bind && DebuffContains(named_Debuffs, "11") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 11);
                    }
                    // GRAVITY
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Weight && DebuffContains(named_Debuffs, "12") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 12);
                    }
                    // ACCURACY DOWN
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_AccuracyDown && DebuffContains(named_Debuffs, "146") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 146);
                    }
                    // DEFENSE DOWN
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_DefenseDown && DebuffContains(named_Debuffs, "149") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 149);
                    }
                    // MAGIC DEF DOWN
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MagicDefenseDown && DebuffContains(named_Debuffs, "167") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 167);
                    }
                    // ATTACK DOWN
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_AttackDown && DebuffContains(named_Debuffs, "147") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 147);
                    }
                    // HP DOWN
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MaxHpDown && DebuffContains(named_Debuffs, "144") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 144);
                    }
                    // VIT Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_VitDown && DebuffContains(named_Debuffs, "138") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 138);

                    }
                    // Threnody
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Threnody && DebuffContains(named_Debuffs, "217") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 217);

                    }
                    // Shock
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Shock && DebuffContains(named_Debuffs, "132") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 132);

                    }
                    // StrDown
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_StrDown && DebuffContains(named_Debuffs, "136") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 136);

                    }
                    // Requiem
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Requiem && DebuffContains(named_Debuffs, "192") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 192);

                    }
                    // Rasp
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Rasp && DebuffContains(named_Debuffs, "131") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 131);

                    }
                    // Max TP Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MaxTpDown && DebuffContains(named_Debuffs, "189") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 189);

                    }
                    // Max MP Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MaxMpDown && DebuffContains(named_Debuffs, "145") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 145);

                    }
                    // Magic Attack Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MagicAttackDown && DebuffContains(named_Debuffs, "175") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 175);

                    }
                    // Magic Acc Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MagicAccDown && DebuffContains(named_Debuffs, "174") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 174);

                    }
                    // Mind Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MndDown && DebuffContains(named_Debuffs, "141") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 141);

                    }
                    // Int Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_IntDown && DebuffContains(named_Debuffs, "140") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 140);

                    }
                    // Helix
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Helix && DebuffContains(named_Debuffs, "186") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 186);

                    }
                    // Frost
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Frost && DebuffContains(named_Debuffs, "129") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 129);

                    }
                    // EvasionDown
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_EvasionDown && DebuffContains(named_Debuffs, "148") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 148);

                    }
                    // ELEGY
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Elegy && DebuffContains(named_Debuffs, "194") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 194);

                    }
                    // Drown
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Drown && DebuffContains(named_Debuffs, "133") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 133);

                    }
                    // Dia
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Dia && DebuffContains(named_Debuffs, "134") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 134);

                    }
                    // DexDown
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_DexDown && DebuffContains(named_Debuffs, "137") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 137);

                    }
                    // Choke
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Choke && DebuffContains(named_Debuffs, "130") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 130);

                    }
                    // ChrDown
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_ChrDown && DebuffContains(named_Debuffs, "142") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 142);

                    }
                    // Burn
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Burn && DebuffContains(named_Debuffs, "128") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 128);

                    }
                    // Addle
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Addle && DebuffContains(named_Debuffs, "21") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 21);
                    }
                    // AGI Down
                    else if (OptionsForm.config.naErase == true && OptionsForm.config.na_AgiDown && DebuffContains(named_Debuffs, "139") && _PlayerHelper.IsAbleToCastSpell("Erase"))
                    {
                        _CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                        //removeDebuff(charName, 139);
                    }
                }
                #endregion
            }
        }
    }
}
