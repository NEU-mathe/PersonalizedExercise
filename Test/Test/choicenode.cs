using System;

namespace Test
{
    class choicenode : IComparable<choicenode>
    {
        public int id;
        public int pid;
        public string name;

        public choicenode()
        {
           
        }

        public int CompareTo(choicenode p)
        {
                return this.id - p.id;
        }

        }
}

