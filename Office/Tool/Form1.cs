using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using QiHe.CodeLib;
using QiHe.Office.CompoundDocumentFormat;
using QiHe.Office.Excel;

namespace QiHe.Office.Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        CompoundDocument doc;
        bool IsOpened = false;

        private void newNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //test create xls file
            string file = "C:\\newdoc.xls";
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("First Sheet");
            worksheet.Cells[0, 1] = new Cell(1);
            worksheet.Cells[2, 0] = new Cell(2.8);
            worksheet.Cells[3, 3] = new Cell((decimal)3.45);
            worksheet.Cells[2, 2] = new Cell("Text string");
            worksheet.Cells[2, 4] = new Cell("Second string");
            worksheet.Cells[4, 0] = new Cell(32764.5, "#,###.00");
            worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY\-MM\-DD");
            worksheet.Cells.ColumnWidth[0, 1] = 3000;
            workbook.Worksheets.Add(worksheet);
            workbook.Save(file);
            //open created file
            doc = CompoundDocument.Open(file);
            IsOpened = true;
            PopulateTreeview(file);
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string file = FileSelector.BrowseFile(FileType.All);
                if (file == null) return;
                doc = CompoundDocument.Open(file);
                IsOpened = true;
                PopulateTreeview(file);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void PopulateTreeview(string file)
        {
            treeViewDoc.Nodes.Clear();
            TreeNode root = treeViewDoc.Nodes.Add(Path.GetFileName(file));
            root.Nodes.Add("Header");
            AddTreeNode(root, doc.RootStorage);
            treeViewDoc.ExpandAll();
        }

        void AddTreeNode(TreeNode node, DirectoryEntry entry)
        {
            TreeNode subnode = node.Nodes.Add(entry.Name);
            subnode.Tag = entry;
            subnode.ContextMenu = new ContextMenu();
            subnode.ContextMenu.MenuItems.Add(new MenuItem("Remove", delegate(object sender, EventArgs e)
            {
                doc.DeleteDirectoryEntry(entry);
            }));
            foreach (KeyValuePair<string, DirectoryEntry> subentry in entry.Members)
            {
                AddTreeNode(subnode, subentry.Value);
            }
        }

        private void treeViewDoc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DirectoryEntry entry = e.Node.Tag as DirectoryEntry;
            if (entry != null)
            {
                if (entry.EntryType == EntryType.Stream)
                {
                    byte[] data = entry.Data;

                    textBoxHexView.Text = Bin2Hex.Format(data);
                    textBoxShowText.Text = Encoding.Unicode.GetString(data);
                }
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (doc == null) return;
            if (e.TabPage == tabPageExcell)
            {
                //try
                {
                    LoadExcelSheets();
                }
                //catch (Exception error)
                //{
                //    MessageBox.Show(error.Message);
                //}
            }
        }

        private void LoadExcelSheets()
        {
            byte[] bookdata = doc.GetStreamData("Workbook");
            if (bookdata == null) return;
            Workbook book = WorkbookDecoder.Decode(new MemoryStream(bookdata));

            //ExtractImages(book, @"C:\Images");

            tabControlSheets.TabPages.Clear();

            foreach (Worksheet sheet in book.Worksheets)
            {
                TabPage sheetPage = new TabPage(sheet.Name);
                DataGridView dgvCells = new DataGridView();
                dgvCells.Dock = DockStyle.Fill;
                dgvCells.RowCount = sheet.Cells.LastRowIndex + 1;
                dgvCells.ColumnCount = sheet.Cells.LastColIndex + 1;

                foreach (Pair<Pair<int, int>, Cell> cell in sheet.Cells)
                {
                    dgvCells[cell.Left.Right, cell.Left.Left].Value = cell.Right.Value;
                    if (cell.Right.BackColorIndex != 64)
                    {
                        dgvCells[cell.Left.Right, cell.Left.Left].Style.BackColor = cell.Right.BackColor;
                    }
                }

                foreach (KeyValuePair<Pair<int, int>, Picture> cell in sheet.Pictures)
                {
                    int rowIndex = cell.Key.Left;
                    int colIndex = cell.Key.Right;
                    if (dgvCells.RowCount < rowIndex + 1)
                    {
                        dgvCells.RowCount = rowIndex + 1;
                    }
                    if (dgvCells.ColumnCount < colIndex + 1)
                    {
                        dgvCells.ColumnCount = colIndex + 1;
                    }
                    dgvCells[colIndex, rowIndex].Value = String.Format("<Image,{0}>", GetImageFileExtension(cell.Value.ImageFormat));
                }

                sheetPage.Controls.Add(dgvCells);
                tabControlSheets.TabPages.Add(sheetPage);
            }
        }

        void ExtractImages(Workbook book, string path)
        {
            if (book.DrawingGroup == null) return;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Worksheet sheet1 = book.Worksheets[0];
            foreach (KeyValuePair<Pair<int, int>, Picture> pic in sheet1.Pictures)
            {
                string filename = String.Format("image{0}-{1}", pic.Key.Left, pic.Key.Right)
                    + GetImageFileExtension(pic.Value.ImageFormat);
                string file = Path.Combine(path, filename);
                File.WriteAllBytes(file, pic.Value.ImageData);
            }
        }

        static string GetImageFileExtension(ushort imagetype)
        {
            switch (imagetype)
            {
                case EscherRecordType.MsofbtBlipBitmapJPEG:
                    return ".jpeg";
                case EscherRecordType.MsofbtBlipBitmapPNG:
                    return ".png";
                case EscherRecordType.MsofbtBlipBitmapDIB:
                    return ".bmp";
                case EscherRecordType.MsofbtBlipBitmapPS:
                    return ".psd";
                default:
                    return "unknown";
            }
        }

        private void exitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsOpened)
            {
                doc.Close();
            }
            this.Close();
        }

        private void saveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (doc != null)
            {
                doc.Save();
            }
        }
        Workbook workbook;
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (doc == null) return;
            string file = FileSelector.BrowseFileForSave(FileType.All);
            if (file == null) return;


            CompoundDocument newDoc = CompoundDocument.Create(file);

            foreach (string streamName in doc.RootStorage.Members.Keys)
            {
                newDoc.WriteStreamData(new string[] { streamName }, doc.GetStreamData(streamName));
            }

            byte[] bookdata = doc.GetStreamData("Workbook");
            if (bookdata != null)
            {
                if (workbook == null)
                {
                    workbook = WorkbookDecoder.Decode(new MemoryStream(bookdata));
                }
                MemoryStream stream = new MemoryStream();
                //WorkbookEncoder.Encode(workbook, stream);

                BinaryWriter writer = new BinaryWriter(stream);
                foreach (Record record in workbook.Records)
                {
                    record.Write(writer);
                }
                writer.Close();
                newDoc.WriteStreamData(new string[] { "Workbook" }, stream.ToArray());
            }
            newDoc.Save();
            newDoc.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}