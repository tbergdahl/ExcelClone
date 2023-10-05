namespace Homework_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Options_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles menu option "Load First 50 Fibonacci Numbers" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFirst50FibonacciNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader num = new FibonacciTextReader(50);
            string? line, textEntry = "";
            int i = 0;
            while ((line = num.ReadLine()) != null)
            {
                textEntry = textEntry + i++ + ": " + line +  Environment.NewLine;
            }
          
            textBox1.Text = textEntry;
        }


        /// <summary>
        /// Handles menu option "Load First 100 Fibonacci Numbers" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFirst100FibonacciNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FibonacciTextReader num = new FibonacciTextReader(100);
            string ?line, textEntry = "";
            int i = 0;
            while ((line = num.ReadLine()) != null)
            {
                textEntry = textEntry + i++ + ": " + line + Environment.NewLine;
            }

            textBox1.Text = textEntry;
        }


        /// <summary>
        /// Handles menu option "Load From File" being pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            { 
                string fileName = openFileDialog1.FileName;
                if(File.Exists(fileName)) 
                {
                    using(StreamReader sr = new StreamReader(fileName)) 
                    { 
                        LoadText(sr); 
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
        private void SaveToFileToolStripMenuItem1_Click(object sender, EventArgs e)
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
            
            string ?line, text_box_contents = "";
            while ((line = sr.ReadLine()) != null)
            {
                text_box_contents += line;
            }
            textBox1.Text = text_box_contents;
        }
    }
}     