using BaseMod;
using System;
using Contingecy_Contract;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractReward
{
    public class PassiveAbility_1880103: PassiveAbilityBase, GetRecovery
    {
        public int GetRecoveryBonus(int v)
        {
            return v;
        }
    }
}
