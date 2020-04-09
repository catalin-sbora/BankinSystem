using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
   public class CardColor:DataEntity
    {
       
        public int Color { get; set; }
        public Guid CardId { get; set; }
        //public virtual Card Card { get; set; }
    }
}
