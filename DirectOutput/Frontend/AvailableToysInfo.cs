using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.Cab.Toys;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace DirectOutput.Frontend
{
    public partial class AvailableToysInfo : Form
    {
        private void UpdateAvailableTypes()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Toy type", typeof(string));

            foreach (Type T in AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IToy).IsAssignableFrom(p) && !p.IsAbstract))
            {
                DT.Rows.Add(T.Name);
            }

            AvailableToys.ClearSelection();
            AvailableToys.Columns.Clear();
            AvailableToys.AutoGenerateColumns = true;
            AvailableToys.DataSource = DT;
            AvailableToys.Refresh();
            AvailableToys.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            UpdateTypeXml();
        }


        private void UpdateTypeXml()
        {
            string S = "";
            if (AvailableToys.SelectedRows.Count > 0)
            {
                string N = AvailableToys.SelectedRows[0].Cells[0].Value.ToString();
                General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IToy).IsAssignableFrom(p) && !p.IsAbstract));
                if (Types.Contains(N))
                {
                    Type T = Types[N];


                    object O = Activator.CreateInstance(T);


                    foreach (PropertyInfo PI in T.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (PI.CanWrite)
                        {
                            if (PI.PropertyType == typeof(string) && PI.Name == "Name")
                            {
                                PI.SetValue(O, "ToyName", null);
                            }
                            else if (PI.PropertyType.IsNumber())
                            {
                                //                               PI.SetValue(O, 0, null);
                            }
                            else if (PI.PropertyType == typeof(bool))
                            {
                                PI.SetValue(O, false, null);
                            }
                            else if (PI.PropertyType == typeof(string) && PI.Name.ToLower().Contains("output"))
                            {
                                PI.SetValue(O, "Name of a output", null);
                            }
                            else if (PI.PropertyType == typeof(string))
                            {
                                string V = (string)PI.GetValue(O, null);
                                if (V.IsNullOrWhiteSpace())
                                {
                                    PI.SetValue(O, "{0} value".Build(PI.Name), null);
                                }
                            }
                            else if (PI.PropertyType == typeof(DateTime))
                            {
                                PI.SetValue(O, DateTime.MaxValue, null);
                            }
                        }
                    }

                    try
                    {
                        XmlSerializerNamespaces EmptyNamepsaces = new XmlSerializerNamespaces(new[] { System.Xml.XmlQualifiedName.Empty });
                        var Serializer = new XmlSerializer(T);
                        var Settings = new System.Xml.XmlWriterSettings();
                        Settings.Indent = true;
                        Settings.OmitXmlDeclaration = true;


                        using (var Stream = new StringWriter())
                        using (var Writer = System.Xml.XmlWriter.Create(Stream, Settings))
                        {
                            Serializer.Serialize(Writer, O, EmptyNamepsaces);
                            S = Stream.ToString();
                        }

                    }
                    catch (Exception E)
                    {
                        S = "XML Serialization failed.\nException:\n{0}".Build(E.Message);
                    }

                }
                else
                {
                    S = "Type not found";
                }

            }
            TypeXml.Text = S;
        }
        public AvailableToysInfo()
        {
            InitializeComponent();
            UpdateAvailableTypes();
        }

        private void AvailableToys_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTypeXml();
        }

        private void CopyToClipboard_Click(object sender, EventArgs e)
        {
            if (!TypeXml.Text.IsNullOrWhiteSpace())
            {
                Clipboard.SetText(TypeXml.Text);
            }
        }

    }
}
