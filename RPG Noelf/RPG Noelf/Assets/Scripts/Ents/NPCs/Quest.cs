﻿using RPG_Noelf.Assets.Scripts.Inventory_Scripts;
using RPG_Noelf.Assets.Scripts.PlayerFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RPG_Noelf.Assets.Scripts.Ents.NPCs
{
    public abstract class Quest
    {
        public uint QUEST_ID { get; set; }
        public uint RequiredID { get; set; }//Id nescessario para a ativação da quest
        public int level { get; set; }//level ta int por que só quero comparar o level atual do player
        public string name { get; set; }//nome da quest
        public string Description { get; set; }//descrição da quest
        public int QuestPart { get; set; }//progresso da quest
        public int GainedXP { get; set; }//EXP recebido
        public int GainedGold { get; set; }//ouro recebido
        public Slot GainedItem { get; set; }
        public bool isComplete { get; set; }//está completa sim ou não
        public string RewardDescription { get; set; }

        public Quest()
        {
           
            
        }

        //completar a quest
        public void CompleteQuest(/*Player p*/)
        {
            this.isComplete = true;
            //p.level.GainEXP(this.GainedXP);
            //p._Inventory.AddGold(this.GainedGold);
        }

    }

    //quest de matar ou coletar um ID
    public class CountQuest:Quest
    {
        public uint mobToKill { get; set; }//esse ID está para mob, mas pode ser usado para item
        public int countMobs { get; set; }//contagem ne itens/monstros necessaria
        public CountQuest(int mobs)
        {
            countMobs = mobs;
        }

        //decrementar o numero de mobs após matar um, e completar caso seja o ultimo
        public void decreaseID(uint genericID)
        {
            if(genericID == mobToKill)
            {
                if(countMobs == 0)
                {
                    CompleteQuest();
                }
                else
                {
                    this.countMobs--;
                }
            }

        }

      
    }
    // quests de falar com um cara em especifico ou entregar um item
    class SpeakQuest : Quest
    {
        public uint targetedGuy { get; set; }
        public uint NedeedItem { get; set; }
        public SpeakQuest(uint targetNPC)
        {
            targetedGuy = targetNPC;
           

        }

        // compleatar a quest caso o NPC seja o Certo
        public bool SpeakToHim(uint generiNPC)
        {
            if(generiNPC == targetedGuy && NedeedItem == 0)
            {
                CompleteQuest();
                return true;
            }
            return true;
        }
      
        //completar a quest caso o Item entregue e o NPC estejam corretos
        public bool SpeakToHim(uint generiNPC, uint genericID)
        {
            if (generiNPC == targetedGuy && NedeedItem == genericID)
            {
                CompleteQuest();
                return true;
            }
            return true;
        }

    }
  

}
