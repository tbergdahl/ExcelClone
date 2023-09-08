using System.Runtime.CompilerServices;

namespace Homework_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RunDistinctIntegers();
        }

        private void RunDistinctIntegers()
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



            int distinct = 0;
            for (int i = 0; i < 10000; i++)
            {
                bool isDistinct = true; // Assume the current element is distinct

                for (int j = 0; j < i; j++)
                {
                    if (ints[j] == ints[i])
                    {
                        isDistinct = false; // If a duplicate is found, set isDistinct to false
                    }
                }

                if (isDistinct)
                {
                    distinct++; // If isDistinct is still true, increment the distinct count
                }
            }

            ints.Sort();
            int distinct2 = 0;
            for (int i = 0; i < 10000; i++)
            {
                distinct2++; //distinct always increments
                if (i < 9999 && ints[i] == ints[i + 1])
                {
                    distinct2--; //if we've already used element, decrement
                }
            }

           

            String str1 = "1st Algorithm:\r\n" +
                "# of Distinct Integers: " + nums.Count() +
                "\r\nThe time complexity of this algorithm is O(n) because we do not have any nested loops, " +
                "but we do have loops that are based on the size (N) of the list.\r\n\r\n";

            String str2 = "2nd Algorithm:\r\n" +
                "# of Distinct Integers: " + distinct +
                "\r\nThe time complexity of this algorithm is O(n^2) because we have nested loops, " +
                "which are based on the size (N) of the list.\r\n\r\n";

            String str3 = "3rd Algorithm:\r\n" +
                "# of Distinct Integers: " + distinct2 +
                "\r\nThe time complexity of this algorithm is O(n) because we have do not have nested loops, " +
                "and the for loop is based on the size (N) of the list.\r\n\r\n";

            textBox1.Text = str1 + str2 + str3;
        }
    }
}