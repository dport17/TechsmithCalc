using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace TechsmithCalc
{
   class HistoryKeeper
   {
      private List<KeyValuePair<string, string>> history;

      public void AddToHistory(string exp, string result)
      {
         history.Add(new KeyValuePair<string, string>(exp, result));
      }

      public void WipeHistory()
      {
         //kill the history dictionary
      }
   }
}
