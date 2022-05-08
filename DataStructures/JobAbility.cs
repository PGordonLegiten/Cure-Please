using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.DataStructures
{
    public class JobAbility : IEquatable<JobAbility>
    {
        public string ActionId { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public DateTime Invoked { get; set; }
        public int Priority { get; set; }
        public StatusEffect Status { get; set; }

        public JobAbility()
        {
        }
        public JobAbility(string name, string target, StatusEffect effect)
        {
            Name = name;
            Target = target;
            ActionId = target + name;
            Invoked = DateTime.Now;
            Status = effect;
            //Priority = Convert.ToInt32(priority);
        }

        public override string ToString()
        {
            return "[" + Invoked.ToString("hh:mm:ss") + "][" + Name + "][" + Target + "][" + Priority + "]";
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            JobAbility objAsPart = obj as JobAbility;
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
        public bool Equals(JobAbility other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Name.Equals(other.Name) && this.Target.Equals(other.Target);
        }
    }
}
