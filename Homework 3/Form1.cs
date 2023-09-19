namespace Homework_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Options_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles menu option "Load First 50 Fibonacci Numbers" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadFirst50FibonacciNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //implement
        }


        /// <summary>
        /// Handles menu option "Load First 100 Fibonacci Numbers" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadFirst100FibonacciNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //implement
        }


        /// <summary>
        /// Handles menu option "Load From File" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            { 
                string fileName = openFileDialog1.FileName;
                if(File.Exists(fileName)) 
                { 
                    using(StreamReader sr = new StreamReader(fileName)) 
                    {
                        string line, full_message = "";
                        while((line = sr.ReadLine()) != null)
                        {
                            full_message += line;
                        }
                        textBox1.Text = full_message;
                    }
                }
                else
                {
                    textBox1.Text = "File not found" + fileName;
                }
            }
        }



        /// <summary>
        /// Handles menu option "Save to file" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                
                using (FileStream outfile = new FileStream(filePath, FileMode.Create))
                {
                    using(StreamWriter streamWriter = new StreamWriter(outfile))
                    {
                        streamWriter.WriteLine(textBox1.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Reads all text from sr and puts it in private member textBox1
        /// </summary>
        /// <param name="sr"></param>
       private void LoadText(TextReader sr)
       {
            
            string line, text_box_contents = "";
            while ((line = sr.ReadLine()) != null)
            {
                text_box_contents += line;
            }
            textBox1.Text = text_box_contents;
        }
    }
}     