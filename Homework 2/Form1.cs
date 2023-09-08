namespace Homework_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RunDistinctIntegers();
        }

        private static void RunDistinctIntegers()
        {
            Random random = new Random();

            List<int> ints = new List<int>();
          
            for (int i = 0; i < 10000; i++)
            {
                int randomNumber = random.Next(0, 20001);
                ints.Add(randomNumber);
            }

            HashSet<int> nums = new HashSet<int>();

            for (int i = 0; i < 10000; i++)
            {
                nums.Add(ints[i]);//add every int to hash map
            }

            Console.WriteLine("Distinct Integers: " + nums.Count() + "\n The time complexity of this algorithm is O(n) because we do not have any nested loops, but we do have loops that are based on the size (N) of the list.\n");

            int distinct = 0;
            for(int i = 0; i < 10000; i++) 
            {
                distinct++;
                for(int j = i; j < 10000; j++)
                {
                    if (ints[j] == ints[i] && i != j)
                    {
                        distinct--;
                    }
                }
            }

            ints.Sort();
            distinct = 0;
            int used = 0;
            for(int i = 0; i < 10000; i++)
            {
                if (ints[i] == used)
                {
                    distinct--; //if we've already used element, decrement
                }
                used = ints[i]; //update used to reflect last value in array
                distinct++; //distinct always increments
            }

        }
    }
}