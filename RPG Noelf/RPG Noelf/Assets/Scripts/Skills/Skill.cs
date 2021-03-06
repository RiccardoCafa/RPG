using RPG_Noelf.Assets.Scripts.Ents;
using RPG_Noelf.Assets.Scripts.PlayerFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Noelf.Assets.Scripts.Skills
{
    public enum SkillType
    {
        passive,
        habilite,
        ultimate
    }
    public enum AtributBonus
    {
        For,
        Int,
        dex,
        agl
    }


    class Skill : SkillGenerics
    {
       //Skill que apenas causam dano

        public Skill(string pathImage, string name)
        {
            this.pathImage = pathImage;
            this.name = name;
            this.tipobuff = SkillTypeBuff.normal;
        }


        public override double UseSkill(Ent player, Ent Enemy)
        {
            if (!(player is Player)) return 0;
            if (manaCost <= (player as Player).Mp)
            {
               // CalcBonus(player);
               // Damage = Damage + Amplificator * Lvl;
                player.Mp -= manaCost;
                return DamageBonus + Damage;
            }
            return 0;
        }

        public override void RevertSkill(Ent ent)
        {
            
        }
    }
}
