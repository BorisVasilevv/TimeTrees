using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrees.Core
{
    public class MenuSelection
    {
        //Переключение между пунктами меню 
        //Поднимаемся на один пункт меню 
        public static int ChangeToUp(int chosenId, int AmountOfMenuItems)
        {
            chosenId--;
            if (chosenId < 0)
                chosenId += AmountOfMenuItems;
            return chosenId;
        }

        //Спускаемся на один пункт меню
        public static int ChangeToDown(int chosenId, int AmountOfMenuItems)
        {
            chosenId++;
            if (chosenId >= AmountOfMenuItems)
                chosenId -= AmountOfMenuItems;
            return chosenId;
        }
    }
}
