using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data_concurrency
{
    [System.Serializable]
    public class PlayerStatsDataModel
    {
        public int totalPoint;

        public int currentSTR;
        public int currentINT;
        public int currentDUR;
        public int currentPER;
        public int currentVIT;

        public int strLevel;
        public int intLevel;
        public int durLevel;
        public int perLevel;
        public int vitLevel;
    }

}
