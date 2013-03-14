using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectOutput.Scripting;
using DirectOutput.FX;
using DirectOutput.Cab.Toys;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace DirectOutput.Frontend
{
    public partial class ScriptInfo : Form
    {
        private Pinball _Pinball = new Pinball();

        public Pinball Pinball
        {
            get { return _Pinball; }
            set
            {
                _Pinball = value;
                UpdateLoadedScripts();
            }
        }


        private void UpdateScriptTypes() 
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Type", typeof(string));
            DT.Columns.Add("Interface", typeof(string));

            if (LoadedScripts.SelectedRows.Count > 0)
            {
                string N = LoadedScripts.SelectedRows[0].Cells[0].Value.ToString();

                if (Pinball.Scripts.Any(sc => sc.File.FullName == N))
                {
                    Script S = Pinball.Scripts[N];

                    foreach (Type T in S.Assembly.GetTypes().Where(t => typeof(IEffect).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        DT.Rows.Add(T.Name, "IEffect");
                    }

                    foreach (Type T in S.Assembly.GetTypes().Where(t => typeof(IToy).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        DT.Rows.Add(T.Name, "IToy");
                    }
                }
            }




            ScriptTypes.ClearSelection();
            ScriptTypes.Columns.Clear();
            ScriptTypes.AutoGenerateColumns = true;
            ScriptTypes.DataSource = DT;
            ScriptTypes.Refresh();
            ScriptTypes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            UpdateTypeXml();
        }


        private void UpdateLoadedScripts()
        {
            DataTable DT = new DataTable();
            DT.Columns.Add("Filename", typeof(string));
            DT.Columns.Add("Compilation Result", typeof(string));
            DT.Columns.Add("Effect count", typeof(int));
            DT.Columns.Add("Toy count", typeof(int));

            foreach (Script S in Pinball.Scripts)
            {
                int ToyCnt = 0;
                int EffectCnt = 0;
                string CompilationResult = "";
                if (S.Compiled)
                {
                    EffectCnt = S.Assembly.GetTypes().Count(p => typeof(IEffect).IsAssignableFrom(p) && !p.IsAbstract);
                    ToyCnt = S.Assembly.GetTypes().Count(p => typeof(IToy).IsAssignableFrom(p) && !p.IsAbstract);
                    CompilationResult = "OK";
                }
                else
                {
                    if (S.CompilationException != null)
                    {
                        CompilationResult = S.CompilationException.Message;
                    }
                    else
                    {
                        CompilationResult = "Not compiled";
                    }
                }
                DT.Rows.Add(S.File.FullName, CompilationResult, EffectCnt, ToyCnt);
            }

            LoadedScripts.ClearSelection();
            LoadedScripts.Columns.Clear();
            LoadedScripts.AutoGenerateColumns = true;
            LoadedScripts.DataSource = DT;
            LoadedScripts.Refresh();
            LoadedScripts.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            UpdateScriptTypes();
        }




        private void UpdateTypeXml()
        {
            string S = "";
            if (ScriptTypes.SelectedRows.Count > 0)
            {
                string N = ScriptTypes.SelectedRows[0].Cells[0].Value.ToString();
                General.TypeList Types = new General.TypeList(AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => (typeof(IEffect).IsAssignableFrom(p) || typeof(IToy).IsAssignableFrom(p)) && !p.IsAbstract));
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
                                if (typeof(IEffect).IsAssignableFrom(T))
                                {
                                    PI.SetValue(O, "Effect Name", null);
                                }
                                else if (typeof(IToy).IsAssignableFrom(T))
                                {
                                    PI.SetValue(O, "Toy Name", null);
                                }
                                else
                                {
                                    PI.SetValue(O, "Name", null);
                                }
                            }
                            else if (PI.PropertyType.IsNumber())
                            {
                                
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
                            Serializer.Serialize(Writer,O, EmptyNamepsaces);
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



        public ScriptInfo(Pinball Pinball)
        {
            InitializeComponent();
            this.Pinball = Pinball;
        }

        private void LoadedScripts_SelectionChanged(object sender, EventArgs e)
        {
            UpdateScriptTypes();
        }

        private void ScriptTypes_SelectionChanged(object sender, EventArgs e)
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
