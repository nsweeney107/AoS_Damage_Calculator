using AoS_DamageCalculator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoS_DamageCalculator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ModelsContext Context { get; } = new ModelsContext();

        public ObservableCollection<Model> Models { get; } = new ObservableCollection<Model>();

        // Properties to hold wounds values
        private double threePlusAmor;

        public double ThreePlusArmor
        {
            get { return threePlusAmor; }
            set
            {
                threePlusAmor = value;
                OnPropertyChanged();
            }
        }

        private double fourPlusArmor;

        public double FourPlusArmor
        {
            get { return fourPlusArmor; }
            set
            {
                fourPlusArmor = value;
                OnPropertyChanged();
            }
        }

        private double fivePlusArmor;

        public double FivePlusArmor
        {
            get { return fivePlusArmor; }
            set
            {
                fivePlusArmor = value;
                OnPropertyChanged();
            }
        }

        private double sixPlusArmor;

        public double SixPlusArmor
        {
            get { return sixPlusArmor; }
            set
            {
                sixPlusArmor = value;
                OnPropertyChanged();
            }
        }

        private double averageWounds;

        public double AverageWounds
        {
            get { return averageWounds; }
            set
            {
                averageWounds = value;
                OnPropertyChanged();
            }
        }

        // Properties to hold checkbox values
        private string hitRerolls = "None";

        public string HitRerolls
        {
            get { return hitRerolls; }
            set
            {
                hitRerolls = value;
                OnPropertyChanged();
            }
        }

        private string woundRerolls = "None";

        public string WoundRerolls
        {
            get { return woundRerolls; }
            set
            {
                woundRerolls = value;
                OnPropertyChanged();
            }
        }

        private string hitMortalWounds = "No";

        public string HitMortalWounds
        {
            get { return hitMortalWounds; }
            set
            {
                hitMortalWounds = value;
                OnPropertyChanged();
            }
        }

        private string woundMortalWounds = "No";

        public string WoundMortalWounds
        {
            get { return woundMortalWounds; }
            set
            {
                woundMortalWounds = value;
                OnPropertyChanged();
            }
        }



        private Model selectedModel = new Model();

        public Model SelectedModel
        {
            get { return selectedModel; }
            set
            {
                if (value == null)
                {
                    value = new Model();
                }
                selectedModel = value;
                OnPropertyChanged();
            }
        }

        private Weapon selectedWeapon = new Weapon();

        public Weapon SelectedWeapon
        {
            get { return selectedWeapon; }
            set
            {
                if (value == null)
                {
                    value = new Weapon();
                }
                selectedWeapon = value;
                OnPropertyChanged();
            }
        }


        public async Task Init()
        {
            await Context.Database.MigrateAsync();
        }

        public async Task Load()
        {
            Models.Clear();
            await Context.Models.Include(m => m.Weapons).ForEachAsync(Models.Add);
            //if (Context.Models.FirstOrDefault() != null)
            //{
            //    SelectedModel = Context.Models.First();
            //    if (SelectedModel.Weapons.FirstOrDefault() != null)
            //    {
            //        SelectedWeapon = SelectedModel.Weapons.First();
            //        CalculateExpectedDamage();
            //    }
            //}
            //else
            //{
            //    SelectedModel = new Model();
            //}
            SelectedModel = new Model();
        }

        public async Task SaveModel()
        {
            if (SelectedModel.Id == 0)
            {
                Context.Models.Add(SelectedModel);
            }
            await Context.SaveChangesAsync();
            await Load();
        }

        public async Task CancelModel()
        {
            RejectChanges();
            await Load();
        }

        public void RejectChanges()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; // Revert changes made to deleted entity
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void AddWeapon()
        {
            if (SelectedWeapon.Id == 0)
            {
                SelectedModel.Weapons.Add(SelectedWeapon);
            }
            SelectedWeapon = new Weapon();
        }

        public void RemoveWeapon()
        {
            if (SelectedModel == null) return;

            if (SelectedModel.Id > 0)
            {
                Context.Weapons.Remove(SelectedWeapon);
            }

            if (SelectedModel.Weapons.Contains(SelectedWeapon))
            {
                SelectedModel.Weapons.Remove(SelectedWeapon);
            }

            SelectedWeapon = new Weapon();
        }

        public void CalculateExpectedDamage()
        {
            // Set up initial variables
            // Pull in attributes of selected weapon
            var numberOfAttacks = SelectedWeapon.NumberOfAttacks;
            var toHit = SelectedWeapon.ToHitNumber;
            var toWound = SelectedWeapon.ToWoundNumber;
            var rend = SelectedWeapon.Rend;
            var damage = SelectedWeapon.Damage;
            var specialDamageNumber = SelectedWeapon.SpecialDamageNumber;
            var specialDamageDamage = SelectedWeapon.SpecialDamageDamage;
            var hitMortalWoundsNumber = SelectedWeapon.HitMortalWoundNumber;
            var hitMortalWoundsDamage = SelectedWeapon.HitMortalWoundDamage;
            var woundMortalWoundsNumber = SelectedWeapon.WoundMortalWoundNumber;
            var woundMortalWoundsDamage = SelectedWeapon.WoundMortalWoundDamage;

            // Prepare the armor save initial values
            var threePlus = 3;
            var fourPlus = 4;
            var fivePlus = 5;
            var sixPlus = 6;
            
            // Check for checkbox values
            //var hitMortalWounds = HitMortalWounds;
            //var woundMortalWounds = WoundMortalWounds;
            var hitRerolls = HitRerolls;
            var woundRerolls = WoundRerolls;

            // Create variable for holding mortal wounds damage inflicted
            double totalMortalWoundsInflicted = 0;

            // Calculate Hit Mortal Wounds
            if (/*HitMortalWounds == "Yes" && */hitMortalWoundsNumber != 0)
            {
                toHit += 1;
                totalMortalWoundsInflicted += numberOfAttacks * CalculateChance(6, HitRerolls)*hitMortalWoundsDamage;
            }

            // Calculate Wound Mortal Wounds
            if (/*WoundMortalWounds == "Yes" && */woundMortalWoundsNumber != 0)
            {
                toWound += 1;
                totalMortalWoundsInflicted += numberOfAttacks * CalculateChance(toHit, HitRerolls) * CalculateChance(6, WoundRerolls) * hitMortalWoundsDamage;
            }

            // Calculate raw hit and wound chances
            var toHitChance = CalculateChance(toHit, HitRerolls);
            var toWoundChance = CalculateChance(toWound, WoundRerolls);
            
            // Apply the weapon's rend to the initial armor saves
            var rendedThreePlus = ApplyRend(threePlus, rend);
            var rendedFourPlus = ApplyRend(fourPlus, rend);
            var rendedFivePlus = ApplyRend(fivePlus, rend);
            var rendedSixPlus = ApplyRend(sixPlus, rend);
            
            // Convert the damage string to a double
            // Note, it is a string because D3 and D6 are valid choices
            // D3 on average is 2, D6 on average is 3.5
            var numericDamage = ConvertDamageStringToDouble(damage);

            // Calculate armor save chances
            var threePlusSaves = CalculateArmorSave(rendedThreePlus);
            var fourPlusSaves = CalculateArmorSave(rendedFourPlus);
            var fivePlusSaves = CalculateArmorSave(rendedFivePlus);
            var sixPlusSaves = CalculateArmorSave(rendedSixPlus);

            // Create variables for special damage
            double specialThreePlusDamage = 0;
            double specialFourPlusDamage = 0;
            double specialFivePlusDamage = 0;
            double specialSixPlusDamage = 0;

            // Calculate special damage values
            if (specialDamageNumber != 0 && specialDamageDamage != 0)
            {
                var specialToWound = CalculateChance(specialDamageNumber, WoundRerolls);
                specialThreePlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, specialToWound, threePlusSaves, specialDamageDamage);
                specialFourPlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, specialToWound, fourPlusSaves, specialDamageDamage);
                specialFivePlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, specialToWound, fivePlusSaves, specialDamageDamage);
                specialSixPlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, specialToWound, sixPlusSaves, specialDamageDamage);
                toWound += 1;
                toWoundChance = CalculateChance(toWound, WoundRerolls);
            }

            // Calculate the total damage output
            var threePlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, toWoundChance, threePlusSaves, numericDamage ) + specialThreePlusDamage + totalMortalWoundsInflicted;
            var fourPlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, toWoundChance, fourPlusSaves, numericDamage) + specialFourPlusDamage + totalMortalWoundsInflicted;
            var fivePlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, toWoundChance, fivePlusSaves, numericDamage) + specialFivePlusDamage +totalMortalWoundsInflicted;
            var sixPlusDamage = CalculateTotalDamageOutput(numberOfAttacks, toHitChance, toWoundChance, sixPlusSaves, numericDamage) + specialSixPlusDamage + totalMortalWoundsInflicted;

            // Calculate average damage
            var averageDamage = (threePlusDamage + fourPlusDamage + fivePlusDamage + sixPlusDamage) / 4;

            // Round damage outputs to tenths place
            var roundedThreePlusDamage = Math.Round(threePlusDamage, 1);
            var roundedFourPlusDamage = Math.Round(fourPlusDamage, 1);
            var roundedFivePlusDamage = Math.Round(fivePlusDamage, 1);
            var roundedSixPlusDamage = Math.Round(sixPlusDamage, 1);
            var roundedAverageDamage = Math.Round(averageDamage, 1);

            // Output to respective TextBoxes
            ThreePlusArmor = roundedThreePlusDamage;
            FourPlusArmor = roundedFourPlusDamage;
            FivePlusArmor = roundedFivePlusDamage;
            SixPlusArmor = roundedSixPlusDamage;
            AverageWounds = roundedAverageDamage;            
        }

        public double CalculateChance(int diceRoll, string rerollType)
        {
            double doubleDiceRoll = (double)diceRoll;
            const double oneSixth = (double)1/(double)6;
            double percentChance = CalculatePercentage(doubleDiceRoll);
            double result;

            if (diceRoll >= 7)
            {
                result = 1;
            }
            else
            {
                switch (rerollType)
                {
                    case "None":
                        result = percentChance;                        
                        break;
                    case "One":
                        result = percentChance + (oneSixth * percentChance);
                        break;
                    case "All":
                        result = percentChance + ((1-percentChance) * percentChance);
                        break;
                    default:
                        result = 0;
                        break;
                }

                //double doubleDiceRoll;
                //doubleDiceRoll = (double)diceRoll;
                //double result = ((6 - doubleDiceRoll)+1)/6;
                //return result;
            }

            return result;
        }

        public double CalculatePercentage(double diceRoll)
        {
            return ((6 - diceRoll) + 1) / 6;
        }

        public int ApplyRend(int armorSave, int rend)
        {
            int newArmorSave;
            if (rend == 0)
            {
                newArmorSave = armorSave;
            }
            else
            {
                newArmorSave = armorSave - rend;
            }

            return newArmorSave;
         }

        public double ConvertDamageStringToDouble(string damage)
        {
            if (damage == "D3")
            {
                damage = "2";
            }
            if (damage == "D6")
            {
                damage = "3.5";
            }

            double numericDamage;
            double.TryParse(damage, out numericDamage);
            return numericDamage;
        }

        public double CalculateArmorSave(int save)
        {
            if (save >= 7)
            {
                return 1;
            }
            else
            {
                var armorSave = 1 - CalculateChance(save, "None");
                return armorSave;
            }
        }

        public double CalculateTotalDamageOutput(int attacks, double toHit, double toWound, double armorSaveChance, double damage)
        {
            var damageOutput = attacks * toHit * toWound * armorSaveChance * damage;
            return damageOutput;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool HasChanges
        {
            get
            {
                Context.ChangeTracker.DetectChanges();
                return Context.ChangeTracker.Entries().Any(entry => entry.State != EntityState.Unchanged);
            }
        }
    }
}
