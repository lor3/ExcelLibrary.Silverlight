using System.Linq;
using System.Windows.Controls;

namespace QiHe.CodeLib
{
    /// <summary>
    /// Represents the file type.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Txt file
        /// </summary>
        Txt,
        /// <summary>
        /// Rtf file
        /// </summary>
        Rtf,
        /// <summary>
        /// Html file
        /// </summary>
        Html,
        /// <summary>
        /// Xml file
        /// </summary>
        Xml,
        /// <summary>
        /// PDF file
        /// </summary>
        PDF,
        /// <summary>
        /// Bin file
        /// </summary>
        Bin,
        /// <summary>
        /// Zip file
        /// </summary>
        Zip,
        /// <summary>
        /// Image file
        /// </summary>
        Img,
        /// <summary>
        /// Excel 97 xls
        /// </summary>
        Excel97,
        /// <summary>
        /// Excel 2007 Open Xml Format
        /// </summary>
        Excel2007,
        /// <summary>
        /// All file type
        /// </summary>
        All
    }
    /// <summary>
    /// FileSelector
    /// </summary>
    public static class FileSelector
    {
        /// <summary>
        /// The Title of FileDialog for select a Single File
        /// </summary>
        public static string TitleSingleFile = "Please choose a file";
        /// <summary>
        /// The Title of FileDialog for select Multiple Files
        /// </summary>
        public static string TitleMultiFile = "Please choose files";
        /// <summary>
        /// Filter
        /// </summary>
        public static string Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        /// <summary>
        /// Sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        public static FileType FileExtension
        {
            set
            {
                switch (value)
                {
                    case FileType.Txt:
                        Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                        break;
                    case FileType.Rtf:
                        Filter = "Rtf files (*.rtf)|*.rtf|All files (*.*)|*.*";
                        break;
                    case FileType.Html:
                        Filter = "Html files (*.htm;*.html)|*.htm;*.html|All files (*.*)|*.*";
                        break;
                    case FileType.Xml:
                        Filter = "XML files (*.xml)|*.xml|Config files (*.config)|*.config|All files (*.*)|*.*";
                        break;
                    case FileType.PDF:
                        Filter = "PDF files (*.pdf)|*.pdf|PDF form files (*.fdf)|*.fdf|All files (*.*)|*.*";
                        break;
                    case FileType.Bin:
                        Filter = "Application files(*.exe;*.dll)|*.exe;*.dll|Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                        break;
                    case FileType.Zip:
                        Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
                        break;
                    case FileType.Img:
                        Filter = "Gif(*.gif)|*.gif|Jpeg(*.jpg)|*.jpg|Emf(*.emf)|*.emf|Bmp(*.bmp)|*.bmp|Png(*.png)|*.png";
                        break;
                    case FileType.Excel97:
                        Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                        break;
                    case FileType.Excel2007:
                        Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        break;
                    case FileType.All:
                        Filter = "All files (*.*)|*.*";
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static OpenFileDialog OFD = new OpenFileDialog();
        /// <summary>
        /// 
        /// </summary>
        public static SaveFileDialog SFD = new SaveFileDialog();


        public static string FileName
        {
            get { return OFD.File.FullName; }
        }

        /// <summary>
        /// Browses the file.
        /// </summary>
        /// <returns></returns>
        public static string BrowseFile()
        {
            OFD.Filter = Filter;
            OFD.Multiselect = false;
            if (OFD.ShowDialog().Value)
            {
                return OFD.File.FullName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Browses the files.
        /// </summary>
        /// <returns></returns>
        public static string[] BrowseFiles()
        {
            OFD.Filter = Filter;
            OFD.Multiselect = true;
            if (OFD.ShowDialog().Value)
            {
                return OFD.Files.Select(f => f.FullName).ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Browses the file for save.
        /// </summary>
        /// <returns></returns>
        public static string BrowseFileForSave()
        {
            SFD.Filter = Filter;
            if (SFD.ShowDialog().Value)
            {
                return SFD.SafeFileName;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Browses the file.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string BrowseFile(FileType type)
        {
            FileExtension = type;
            return BrowseFile();
        }
        /// <summary>
        /// Browses the file.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static string BrowseFile(string filter)
        {
            Filter = filter;
            return BrowseFile();
        }

        /// <summary>
        /// Browses the files.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string[] BrowseFiles(FileType type)
        {
            FileExtension = type;
            return BrowseFiles();
        }

        /// <summary>
        /// Browses the files.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static string[] BrowseFiles(string filter)
        {
            Filter = filter;
            return BrowseFiles();
        }

        /// <summary>
        /// Browses the file for save.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string BrowseFileForSave(FileType type)
        {
            FileExtension = type;
            return BrowseFileForSave();
        }

        /// <summary>
        /// Browses the file for save.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static string BrowseFileForSave(string filter)
        {
            Filter = filter;
            return BrowseFileForSave();
        }
    }
}
