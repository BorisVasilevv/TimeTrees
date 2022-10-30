using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrees.Core
{
    public class IdentificatorGenerator
    {
        private int _maxId;

        public int MaxId
        { 
            get 
            {
                return _maxId; 
            } 
            set
            {
                if(value>_maxId&&value>=0)
                    _maxId = value;
            }
        }

        public int GenerateId()
        {
            _maxId++;
            return _maxId;
        }
    }
}
