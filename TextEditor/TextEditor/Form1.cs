using System.Drawing.Printing;

namespace TextEditor
{
    public partial class Form1 : Form
    {

        //global variables
        string path = ""; //store file path
        string formTitle = "Text Editor"; //title for form
        string notSaved = " *"; //indicates file not saved
        int zoomPct = 100;
        private StreamReader streamToPrint;
        Font globalFont;

        public Form1()
        {
            InitializeComponent();

            menuStrip1.Renderer = new CustomMenuStripRenderer();
            globalFont = textBox1.Font;

           zoomLabel.Alignment = ToolStripItemAlignment.Right;
           statusStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;

            countLabel.Alignment = ToolStripItemAlignment.Right;
            
        }
      
        //method used to create a new file to save under
        public void saveAs()
        {
            DialogResult saveResult = saveFileDialog1.ShowDialog();

            if (saveResult == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                try
                {
                    StreamWriter sw = new StreamWriter(path);
                    sw.Write(textBox1.Text);
                    sw.Close();
                    this.Text = formTitle;
                }
                catch (IOException ioe)
                {
                    MessageBox.Show("Error saving file: " + ioe.Message);
                }
            }
        }
        //method used to update content to the existing file
        public void save()
        {
            try
            {
                StreamReader fileOpen = new StreamReader(path);
                String content = fileOpen.ReadToEnd();
                fileOpen.Close();

                textBox1.Text = content;
                this.Text = formTitle;
            }
            catch (IOException ioe)
            {
                MessageBox.Show("Eorr opening file: " + ioe.Message);
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            DialogResult messageBoxResult = MessageBox.Show("Would you like to save before exiting?", "Save before exit", MessageBoxButtons.YesNo);
            if (messageBoxResult.ToString() == "Yes")
            {
                saveAs();
            }
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult openResult = openFileDialog1.ShowDialog();

            if (openResult == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                try
                {
                    StreamReader fileOpen = new StreamReader(path);
                    String content = fileOpen.ReadToEnd();
                    fileOpen.Close();

                    textBox1.Text = content;
                }
                catch (IOException ioe)
                {
                    MessageBox.Show("Erorr opening file: " + ioe.Message);
                }
            }
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            DialogResult messageBoxResult = MessageBox.Show("Would you like to save before creating a new file?", "Save before new file", MessageBoxButtons.YesNo);
            if (messageBoxResult.ToString() == "Yes")
            {
                saveAs();
            }
            textBox1.Clear();
            path = "";
        }
        //SAVE AS under FILE 
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAs();
        }
        //SAVE under FILE
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == String.Empty)
            {
                saveAs();
            }
            else
            {
                save();
            }
        }

        private void textHasChanged_Flag(object sender, EventArgs e)
        {
            this.Text += notSaved;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.SelectedText);
            SendKeys.Send("{DELETE}");
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.SelectedText);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste(Clipboard.GetText());
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                    streamToPrint = new StreamReader(path);
                try
                {
                    printPreviewDialog1.Document = printDocument1;
                    printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
                    DialogResult printResult = printPreviewDialog1.ShowDialog();
                       if (printResult == DialogResult.OK)
                       {
                           printDocument1.Print();
                       }
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("You need to save the file before printing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
       {
                float linesPerPage = 0;
                float yPos = 0;
                int count = 0;
                float leftMargin = e.MarginBounds.Left;
                float topMargin = e.MarginBounds.Top;
                string line = null;

            // Calculate the number of lines per page.
            linesPerPage = e.MarginBounds.Height /
                   globalFont.GetHeight(e.Graphics);

                // Print each line of the file.
                while (count < linesPerPage &&
                   ((line = streamToPrint.ReadLine()) != null))
                {
                    yPos = topMargin + (count *
                       globalFont.GetHeight(e.Graphics));
                    e.Graphics.DrawString(line, globalFont, Brushes.Black,
                       leftMargin, yPos, new StringFormat());
                    count++;
                }

                // If more lines exist, print another page.
                if (line != null)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }

        private void textWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textWrapToolStripMenuItem.Checked == true)
            {
                textBox1.WordWrap = true;
            }
            else
            {
                textBox1.WordWrap = false;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult fontResult = fontDialog1.ShowDialog();
            if(fontResult == DialogResult.OK)
            {
                textBox1.Font = fontDialog1.Font;
                globalFont = textBox1.Font;
            }
        }

        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (statusBarToolStripMenuItem.Checked == true)
            {
                statusStrip1.Visible = true;
            }
            else
            {
                statusStrip1.Visible = false;
            }
        }

        private void leftAlignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.TextAlign = HorizontalAlignment.Left;
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.TextAlign = HorizontalAlignment.Center;
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.TextAlign = HorizontalAlignment.Right;
        }
        private void zoomOut(TextBox tb)
        {
            float fontSize = tb.Font.Size;
            fontSize -= 2.0F;
            zoomPct -= 10;
            updateZoomLabel();
            tb.Font = new Font(tb.Font.Name, fontSize);
        }
        //method to give user feel of 
        private void zoomIn(TextBox tb)
        {
            float fontSize = tb.Font.Size;
            fontSize += 2.0F;
            zoomPct += 10;
            updateZoomLabel();
            tb.Font = new Font(tb.Font.Name, fontSize);
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomIn(textBox1);
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoomOut(textBox1);
        }

        private void returnToDefaultZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Font = globalFont;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm popUp = new AboutForm();
            popUp.ShowDialog();
            
        }
        private void updateZoomLabel()
        {
            zoomLabel.Text = zoomPct.ToString() + "%";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            updateZoomLabel();
        }
    }
}