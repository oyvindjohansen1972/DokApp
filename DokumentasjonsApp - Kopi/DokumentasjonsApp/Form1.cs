using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DokumentasjonsApp
{
    public partial class Form1 : Form
    {        
        private List<TreeNode> CheckedNodes = new List<TreeNode>();      

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListDirectory(treeView1, @"c:\Dokumenter");
        }                         

        private void button1_Click(object sender, EventArgs e)
        {
            RemoveAllFilesInDirectory();
            CheckedNodes.Clear();
            addCheckedNodes(treeView1.Nodes);
            foreach(var CheckedNode in CheckedNodes)
            {
                if ((CheckedNode.Text).Contains("."))
                {
                    CopyFiles(CheckedNode);
                }
                else
                {
                    MessageBox.Show("Du må velge filer, det virker som du har valgt en mappe!");
                }                
            }
        }

        private void CopyFiles(TreeNode CheckedNode)
        {
            string destPath = @"c:\ValgteDokumenter\" + CheckedNode.Text;
            File.Copy(@"c:\" + CheckedNode.FullPath, destPath, true);           
        }

        public void RemoveAllFilesInDirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(@"C:\ValgteDokumenter");
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        private void addCheckedNodes(TreeNodeCollection nodes)
        {
            foreach(TreeNode node in nodes)
            {
                if(node.Checked)
                {
                    CheckedNodes.Add(node);
                }
                else
                {
                    addCheckedNodes(node.Nodes);
                }
            }
        }

        private void ListDirectory(TreeView treeview, string path)
        {
            treeView1.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView1.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }


        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach(var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }
            foreach(var file in directoryInfo.GetFiles())
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name));
            }

            return directoryNode;
        }    

    }         
}
