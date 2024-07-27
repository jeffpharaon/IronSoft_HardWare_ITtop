using System;
using System.Drawing;
using System.Windows.Forms;
using System.Management;

namespace IronSoft_HardWare_ITtop
{
    public partial class Form1 : Form
    {
        private string key = string.Empty;

        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
        }

        private void Form1_Load(object sender, EventArgs e) => toolStripComboBox1.SelectedIndex = 0;

        private void GetInfoHardWare(string key, ListView listView)
        {
            listView.Items.Clear();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + key);  

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    ListViewGroup group;

                    try
                    {
                        group = listView.Groups.Add(obj["Name"].ToString(), obj["Name"].ToString());
                    }
                    catch
                    {
                        group = listView.Groups.Add(obj.ToString(), obj.ToString());
                    }

                    if (obj.Properties.Count == 0) 
                    {
                        MessageBox.Show("Failed to receive information", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (PropertyData property in obj.Properties)
                    {
                        ListViewItem item = new ListViewItem(group);

                        if (listView.Items.Count % 2 != 0) item.BackColor = Color.White;
                        else item.BackColor = Color.WhiteSmoke;

                        item.Text = property.Name;

                        if (property.Value != null && !string.IsNullOrEmpty(property.Value.ToString()))
                        {
                            switch (property.Value.GetType().ToString())
                            {
                                case "System.String[]":
                                    string[] stringData = property.Value as string[];
                                    string strOne = string.Empty;

                                    foreach (string str in stringData)
                                        strOne += $"{str}";

                                    item.SubItems.Add(strOne);
                                    break;

                                case "System.UInt16[]":
                                    ushort[] ushortData = property.Value as ushort[];   
                                    string strTwo = string.Empty;

                                    foreach (var usr in ushortData)
                                        strTwo += $"{Convert.ToString(usr)}";

                                    item.SubItems.Add(strTwo);
                                    break;

                                default:
                                    item.SubItems.Add(property.Value.ToString());
                                    break;
                            }

                            listView.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolStripComboBox1.SelectedItem.ToString()) 
            {
                case "CPU":
                    key = "Win32_Processor";
                    break;

                case "GPU":
                    key = "Win32_VideoController";
                    break;

                case "Сhipset":
                    key = "Win32_IDEController";
                    break;

                case "Battery":
                    key = "Win32_Battery";
                    break;

                case "BIOS":
                    key = "Win32_BIOS";
                    break;

                case "RAM":
                    key = "Win32_PhysicalMemory";
                    break;

                case "Cache":
                    key = "Win32_CacheMemory";
                    break;

                case "USB":
                    key = "Win32_USBController";
                    break;

                case "Disk":
                    key = "Win32_DiskDrive";
                    break;

                case "Logical Disks":
                    key = "Win32_LogicalDisk";
                    break;

                case "Keyboard":
                    key = "Win32_Keyboard";
                    break;

                case "Network":
                    key = "Win32_NetworkAdapter";
                    break;

                case "Users":
                    key = "Win32_Account";
                    break;

                default:
                    key = "Win32_Processor";
                    break;
            }

            GetInfoHardWare(key, listView1);
        }
    }
}
