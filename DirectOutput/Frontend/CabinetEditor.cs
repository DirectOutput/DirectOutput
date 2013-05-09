using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DirectOutput.Cab;
using DirectOutput.Cab.Out;
using DirectOutput.Cab.Toys;

namespace DirectOutput.Frontend
{
    public partial class CabinetEditor : Form
    {

        public Cabinet Cabinet { get; private set; }


        private void Init()
        {

            LoadCabinet();
        }


        private void LoadCabinet()
        {
            TreeNode Root = new TreeNode("Cabinet: {0}".Build(Cabinet.Name));
            Root.Tag = Cabinet;
            Root.Name = "Cabinet";

            TreeNode OutputControllers = new TreeNode("Output controllers");
            OutputControllers.Name = "CabinetOutputControllers";
            Root.Nodes.Add(OutputControllers);

            foreach (IOutputController O in Cabinet.OutputControllers)
            {
                TreeNode OC = new TreeNode("{0} ({1})".Build(O.Name, O.GetType().Name));
                OC.ToolTipText = "{0} output controller. {1} outputs".Build(O.GetType().Name, O.Outputs.Count);
                OC.Tag = O;
                OutputControllers.Nodes.Add(OC);

                foreach (IOutput OO in O.Outputs)
                {
                    TreeNode OCO = new TreeNode(OO.Name);
                    OCO.Tag = OO;
                    OC.Nodes.Add(OCO);
                }
            }

            TreeNode Toys = new TreeNode("Toys");
            Toys.Name = "CabinetToys";
            Root.Nodes.Add(Toys);
            foreach (IToy T in Cabinet.Toys)
            {
                TreeNode Toy = new TreeNode("{0} ({1})".Build(T.Name, T.GetType().Name));
                Toy.Tag = T;
                Toys.Nodes.Add(Toy);
            }


            TreeNode Colors = new TreeNode("Colors");
            Colors.Name = "CabinetColors";
            Root.Nodes.Add(Colors);

            foreach (Color C in Cabinet.Colors)
            {
                TreeNode Color = new TreeNode(C.Name);
                Color.ToolTipText = "RGB: {0}, {1}, {2}".Build(C.BrightnessRed, C.BrightnessGreen, C.BrightnessBlue);
                Color.Tag = C;
                Colors.Nodes.Add(Color);
            }


            Root.Expand();
            CabinetParts.Nodes.Clear();
            CabinetParts.Nodes.Add(Root);
            CabinetParts.Sort();
        }




        private void PopulateCabinetContextMenu()
        {
            ToolStripItem TSI;
            bool PrevSection = false;
            CabinetContextMenuStrip.Items.Clear();

            TreeNode TN = CabinetParts.SelectedNode;

            if (TN.Tag is IOutputController || TN.Tag is IToy || TN.Tag is Color)
            {
                CabinetContextMenuStrip.Items.Add("Delete {0}".Build(TN.Text));
                PrevSection = true;
            }


            if (TN.Tag is IOutputController || TN.Tag is IOutput || TN.Tag is Cabinet || TN.Name == "CabinetOutputControllers")
            {
                if (PrevSection)
                {
                    CabinetContextMenuStrip.Items.Add("-");
                }


                foreach (Type O in AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IOutputController).IsAssignableFrom(p) && !p.IsAbstract))
                {
                    TSI = CabinetContextMenuStrip.Items.Add("Add {0}".Build(O.Name), null, AddOutputController_Click);
                    TSI.Tag = O;
                }
                PrevSection = true;
            }
            if (TN.Tag is IToy || TN.Tag is Cabinet || TN.Name == "CabinetToys")
            {
                if (PrevSection)
                {
                    CabinetContextMenuStrip.Items.Add("-");
                }



                foreach (Type T in AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IToy).IsAssignableFrom(p) && !p.IsAbstract))
                {
                    TSI = CabinetContextMenuStrip.Items.Add("Add {0}".Build(T.Name), null, AddToy_Click);
                    TSI.Tag = T;
                }
                PrevSection = true;

            }

            if (TN.Tag is Color || TN.Tag is Cabinet || TN.Name == "CabinetColors")
            {
                if (PrevSection)
                {
                    CabinetContextMenuStrip.Items.Add("-");
                }

                TSI = CabinetContextMenuStrip.Items.Add("Add Color", null, AddColor_Click);

                PrevSection = true;

            }


        }

        void AddColor_Click(object sender, EventArgs e)
        {
                int Nr = 1;
                while (Cabinet.Colors.Contains("Color {0}".Build(Nr)))
                {
                    Nr++;
                }
                Color C = new Color() { Name = "Color {0}".Build(Nr) };
                Cabinet.Colors.Add(C);


                TreeNode TN = new TreeNode(C.Name);
                TN.ToolTipText = "RGB: {0}, {1}, {2}".Build(C.BrightnessRed, C.BrightnessGreen, C.BrightnessBlue);
                TN.Tag = C;

                CabinetParts.Nodes["Cabinet"].Nodes["CabinetColors"].Nodes.Add(TN);
                CabinetParts.SelectedNode = TN;

        }

        void AddToy_Click(object sender, EventArgs e)
        {
            ToolStripItem TSI = (ToolStripItem)sender;
            if (TSI.Tag != null && TSI.Tag is Type && typeof(IToy).IsAssignableFrom(((Type)TSI.Tag)))
            {
                string NameBase = ((Type)TSI.Tag).Name;

                int Nr = 1;
                while (Cabinet.Toys.Contains("{0} {1}".Build(NameBase, Nr)))
                {
                    Nr++;
                }
               IToy Toy=(IToy)Activator.CreateInstance(((Type)TSI.Tag));
               Toy.Name = "{0} {1}".Build(NameBase, Nr);
               Cabinet.Toys.Add(Toy);

               TreeNode TN = new TreeNode("{0} ({1})".Build(Toy.Name, Toy.GetType().Name));
               TN.Tag = Toy;
               CabinetParts.Nodes["Cabinet"].Nodes["CabinetToys"].Nodes.Add(TN);
               CabinetParts.SelectedNode = TN;
            }

        }

        void AddOutputController_Click(object sender, EventArgs e)
        {
            ToolStripItem TSI = (ToolStripItem)sender;
            if (TSI.Tag != null && TSI.Tag is Type && typeof(IOutputController).IsAssignableFrom(((Type)TSI.Tag)))
            {
                string NameBase = ((Type)TSI.Tag).Name;

                int Nr = 1;
                while (Cabinet.OutputControllers.Contains("{0} {1}".Build(NameBase, Nr)))
                {
                    Nr++;
                }
                IOutputController OC = (IOutputController)Activator.CreateInstance(((Type)TSI.Tag));
                OC.Name = "{0} {1}".Build(NameBase, Nr);
                Cabinet.OutputControllers.Add(OC);

                TreeNode TN = new TreeNode("{0} ({1})".Build(OC.Name, OC.GetType().Name));
                TN.Tag = OC;
                CabinetParts.Nodes["Cabinet"].Nodes["CabinetOutputControllers"].Nodes.Add(TN);
                CabinetParts.SelectedNode = TN;
            }
        }


        #region Constructor
        public CabinetEditor()
            : this(new Cabinet())
        {
        }


        public CabinetEditor(Cabinet Cabinet)
        {
            InitializeComponent();
            this.Cabinet = Cabinet;
            Init();
        }

        #endregion

        private void CabinetParts_Click(object sender, EventArgs e)
        {

        }

        private void CabinetParts_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (CabinetParts.SelectedNode.Tag != null)
            {
                Properties.SelectedObject = CabinetParts.SelectedNode.Tag;
            }
        }

        private void CabinetContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            PopulateCabinetContextMenu();

        }

        private void CabinetParts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            ((TreeView)sender).SelectedNode = e.Node;
        }

        private void CabinetEditor_Activated(object sender, EventArgs e)
        {
            Console.WriteLine(".");
            Cab.Out.OutputNameConverter.Cabinet = Cabinet;
        }

    

    }
}
