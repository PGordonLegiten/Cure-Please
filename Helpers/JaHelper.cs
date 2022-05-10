using CurePlease.DataStructures;
using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class JaHelper
    {
        private EliteAPI _ELITEAPIPL;
        private EliteAPI _ELITEAPIMonitored;
        private CurePleaseForm _Form;
        private CastingHelper _CastingManager;

        private Dictionary<string,long> _AbilityCooldown = new Dictionary<string,long>();
        public long IsPerformingAction_Timer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public List<string> ScholarJas = new List<string>()
        {
            //LIGHT ARTS
            "Accession",
            "Penury",
            "Celerity",
            "Rapture",
            "Perpetuance",
            "Addendum: White",
            //DARK ARTS
            "Persimony",
            "Ebullience",
            "Manifestation",
            "Focalization",
            "Alacrity",
            "Immanence",
            "Addendum: Black",
            
        };
        
        private Dictionary<string, string> SpecialJas = new Dictionary<string, string>()
        {
           // {"Addendum: Black", "Stratagems"},
        };

        public JaHelper(CurePleaseForm form, EliteAPI pl, EliteAPI monitor, CastingHelper manager)
        {
            _Form = form;
            _ELITEAPIMonitored = monitor;
            _ELITEAPIPL = pl;
            _CastingManager = manager;
        }

        public void JobAbility_Wait(string JobAbilityName)
        {
            JobAbility_Wait(JobAbilityName, JobAbilityName, "<me>");
        }
        public void JobAbility_Wait(string JobabilityDATA, string JobAbilityName)
        {
            JobAbility_Wait(JobabilityDATA, JobAbilityName, "<me>");
        }
        public bool JobAbility_Wait(string JobabilityDATA, string JobAbilityName, string target)
        {
            if (_CastingManager.CanAct())
            {
                _CastingManager.AddLog(new LogEntry("/ja \"" + JobAbilityName + "\" " + target + "", Color.Black));
                var time = _CastingManager.GetLock();
                _Form.castingLockLabel.Text = "Casting is LOCKED for a JA.";
                _Form.currentAction.Text = "Using a Job Ability: " + JobabilityDATA;
                _ELITEAPIPL.ThirdParty.SendString("/ja \"" + JobAbilityName + "\" " + target + "");
                Thread.Sleep(TimeSpan.FromSeconds(2));
                _Form.castingLockLabel.Text = "Casting is UNLOCKED";
                _Form.currentAction.Text = string.Empty;
                _Form.castingSpell = string.Empty;
                _CastingManager.FreeLock("JobAbility_Wait", time);
                return true;
            }
            return false;
        }
        public void DoAbility(JobAbility ja)
        {
            _AbilityCooldown[ja.Name] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            _CastingManager.AddLog(new LogEntry("/ja \"" + ja.Name + "\" " + ja.Target + "", Color.Black));
            var time = _CastingManager.GetLock();
            _Form.castingLockLabel.Text = "Casting is LOCKED for a JA.";
            _Form.currentAction.Text = "Using a Job Ability: " + ja.Name;
            _ELITEAPIPL.ThirdParty.SendString("/ja \"" + ja.Name + "\" " + ja.Target + "");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _Form.castingLockLabel.Text = "Casting is UNLOCKED";
            _Form.currentAction.Text = string.Empty;
            _Form.castingSpell = string.Empty;
            _CastingManager.FreeLock("JobAbility_Wait", time);
        }

        public bool HasTotalStratagems(List<JobAbility> jas)
        {
            var strataCount = 0;
            foreach (JobAbility ja in jas)
            {
                if (ScholarJas.Contains(ja.Name))
                {
                    strataCount++;
                }
            }
            return strataCount <= _Form.currentSCHCharges;
        }

        public bool IsJaReady(string name)
        {
            return IsJaReady(new JobAbility() { Name = name });
        }

        public bool OnCooldown(string name)
        {
            if (_AbilityCooldown.ContainsKey(name) && DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - _AbilityCooldown[name] < 5000) //same ability spam prevention (causes problem with SCH accession)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsJaReady(JobAbility ja)
        {
            if (OnCooldown(ja.Name)) //same ability spam prevention (causes problem with SCH accession)1
            {
                return false;
            }
            if (ScholarJas.Contains(ja.Name)) //SCH Abilities show stragtagem recast as recast (they dont have a recast , check if stratagem exists)
            {
                return _Form.currentSCHCharges > 0;
            }
            if (SpecialJas.ContainsKey(ja.Name)) //Some abilites need to be translated 
            {
                return IsJaReady(SpecialJas[ja.Name]);
            }
            int id = _ELITEAPIPL.Resources.GetAbility(ja.Name, 0).TimerID;
            List<int> IDs = _ELITEAPIPL.Recast.GetAbilityIds();
            for (int x = 0; x < IDs.Count; x++)
            {
                if (IDs[x] == id)
                {
                    return _ELITEAPIPL.Recast.GetAbilityRecast(x) == 0;
                }
            }
            return true;
        }

        public int GetRecast(int id)
        {
            List<int> IDs = _ELITEAPIPL.Recast.GetAbilityIds();
            for (int x = 0; x < IDs.Count; x++)
            {
                if (IDs[x] == id)
                {
                    return _ELITEAPIPL.Recast.GetAbilityRecast(x);
                }
            }
            return 0;
        }
    }
}
