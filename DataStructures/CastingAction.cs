using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.DataStructures
{
    public enum SpellType : byte
    {
        Prio = 0,
        Healing = 1,
        Buff = 2,
        Debuff = 3,
        GEO = 4,
        Raise = 5,
    }

    public enum SpellPrio : byte
    {
        Low = 1,
        Middle = 2,
        High = 3,
        Top = 10,
    }

    public enum CurePrio : byte
    {
        CureI = 10,
        CureII = 11,
        CureIII = 12,
        CureIV = 13,
        CureV = 14,
        CureVI = 15,
        CuragaI = 20,
        CuragaII = 21,
        CuragaIII = 22,
        CuragaIV = 23,
        CuragaV = 24,
    }
    public class CastingAction : IEquatable<CastingAction>
    {
        public string ActionId { get; set; }
        public string SpellName { get; set; }
        public string Target { get; set; }
        public SpellType Type { get; set; }
        public DateTime Invoked { get; set; }
        public int Priority { get; set; }
        public List<JobAbility> JobAbilities { get; set; }

        public CastingAction()
        {
        }
        public CastingAction(SpellType type, string spell, string target, Enum priority, List<JobAbility> jas)
        {
            SpellName = spell;
            Target = target;
            ActionId = target + spell;
            Invoked = DateTime.Now;
            Priority = Convert.ToInt32(priority);
            Type = type;
            JobAbilities = jas;
        }

        public override string ToString()
        {
            return "[" + Invoked.ToString("hh:mm:ss") + "][" + SpellName + "][" + Target + "][" + Priority + "]";
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            CastingAction objAsPart = obj as CastingAction;
            if (objAsPart == null)
            {
                return false;
            }
            else
            {
                return Equals(objAsPart);
            }
        }
        public override int GetHashCode()
        {
            return ActionId.GetHashCode();
        }
        public bool Equals(CastingAction other)
        {
            if (other == null)
            {
                return false;
            }
            return this.SpellName.Equals(other.SpellName) && this.Target.Equals(other.Target);
        }
    }
}
