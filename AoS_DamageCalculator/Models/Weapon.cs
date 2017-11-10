using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoS_DamageCalculator.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string WeaponName { get; set; }
        public int NumberOfAttacks { get; set; }
        public int ToHitNumber { get; set; }
        public int ToWoundNumber { get; set; }
        public int Rend { get; set; }
        public string Damage { get; set; }

        public int SpecialDamageNumber { get; set; }
        public int SpecialDamageDamage { get; set; }

        public int HitMortalWoundNumber { get; set; }
        public int HitMortalWoundDamage { get; set; }

        public int WoundMortalWoundNumber { get; set; }
        public int WoundMortalWoundDamage { get; set; }

        public int ModelId { get; set; }
        public Model Model { get; set; }
    }
}
