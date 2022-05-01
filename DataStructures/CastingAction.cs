using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.DataStructures
{
    public class CastingAction : IEquatable<CastingAction>
    {
        public string ActionId { get; set; }
        public string SpellName { get; set; }

        public string Target { get; set; }
        public string DisplayText { get; set; }
        public DateTime Invoked { get; set; }

        public CastingAction(string spell, string target, [Optional] string OptionalExtras)
        {
            SpellName = spell;
            Target = target;
            ActionId = target + spell;
            Invoked = DateTime.Now;
            DisplayText = OptionalExtras;
        }

        public override string ToString()
        {
            return "[" + Invoked.ToString("hh:mm:ss") + "][" + SpellName + "][" + Target + "]";
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
