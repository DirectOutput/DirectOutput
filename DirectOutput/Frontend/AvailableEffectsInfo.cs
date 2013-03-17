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
using DirectOutput.FX;

namespace DirectOutput.Frontend
{
    public partial class AvailableEffectsInfo : Form
    {
        private void UpdateAvailableTypes()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Effect type", typeof(string));
            DT.Columns.Add("Source", typeof(string));
            Assembly A=System.Reflection.Assembly.GetExecutingAssembly();
            foreach (Type T in AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IEffect).IsAssignableFrom(p) && !p.IsAbstract))
            {
                DT.Rows.Add(T.Name,(A==T.Assembly?"Built in":"Scripted"));
            }

            AvailableEffects.ClearSelection();
            AvailableEffects.Columns.Clear();
            AvailableEffects.AutoGenerateColumns = true;
            AvailableEffects.DataSource = DT;
            AvailableEffects.Refresh();
            AvailableEffects.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            UpdateTypeXml();
        }


        private void UpdateTypeXml()
        {
            string S = "";
            if (AvailableEffects.SelectedRows.Count > 0)
            {
                string N = AvailableEffects.SelectedRows[0].Cells[0].Value.ToString();
                General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IEffect).IsAssignableFrom(p) && !p.IsAbstract));
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
                                PI.SetValue(O, "Effect name", null);
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
                            else if (PI.PropertyType == typeof(string) && PI.Name.ToLower().Contains("toy"))
                            {
                                PI.SetValue(O, "Name of a toy", null);
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
                    S = "Effect not found";
                }

            }
            TypeXml.Text = S;
        }
        public AvailableEffectsInfo()
        {
            InitializeComponent();
            UpdateAvailableTypes();
        }

     

        private void CopyToClipboard_Click(object sender, EventArgs e)
        {
            if (!TypeXml.Text.IsNullOrWhiteSpace())
            {
                Clipboard.SetText(TypeXml.Text);
            }
        }

        private void AvailableEffects_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTypeXml();
        }

    }
}
