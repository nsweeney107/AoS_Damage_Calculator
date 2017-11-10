using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoS_DamageCalculator.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int MinUnitModelCount { get; set; }
        public int MaxUnitModelCount { get; set; }

        public ObservableCollection<Weapon> Weapons { get; set; } = new ObservableCollection<Weapon>();
    }
}
