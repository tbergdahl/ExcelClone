namespace Homework_2.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RDI_Hash_Test1()
        {//this method tests RDI_Hash() without duplicate values
            List<int> ints = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                ints.Add(i);
            }
            Form1 form = new Form1();
            Assert.AreEqual(10000,form.RDI_Hash(ints));
        }
        [Test]
        public void RDI_Hash_Test2()
        {//this method tests RDI_Hash() with duplicate values
            List<int> ints = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                if (i % 2 == 0)
                    ints.Add(i - 1);
            }
            Form1 form = new Form1();
            Assert.AreEqual(4999, form.RDI_Hash(ints));
        }

        [Test]
        public void RDI_None_Test1()
        {//this method tests RDI_None() without duplicate values
            List<int> ints = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                ints.Add(i);
            }
            Form1 form = new Form1();
            Assert.AreEqual(10000, form.RDI_None(ints));
        }

        [Test]
        public void RDI_None_Test2()
        {//this method tests RDI_None() with duplicate values
            List<int> ints = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                if(i % 2 == 0)
                    ints.Add(i - 1);
            }
            Form1 form = new Form1();
            Assert.AreEqual(4999, form.RDI_None(ints));
        }

        [Test]
        public void RDI_Sort_Test1()
        {//this method tests RDI_Sort() without duplicate values
            List<int> ints = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                ints.Add(i);
            }
            Form1 form = new Form1();
            Assert.AreEqual(10000, form.RDI_Sort(ints));
        }


        [Test]
        public void RDI_Sort_Test2()
        {//this method tests RDI_Sort() with duplicate values
            List<int> ints = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                if (i % 2 == 0)
                    ints.Add(i - 1);
            }
            Form1 form = new Form1();
            Assert.AreEqual(4999, form.RDI_Sort(ints));
        }
    }
}