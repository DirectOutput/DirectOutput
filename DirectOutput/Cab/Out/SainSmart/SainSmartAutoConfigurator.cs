//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DirectOutput.Cab.Toys.LWEquivalent;
//using DirectOutput.Cab.Toys;

//namespace DirectOutput.Cab.Out.SainSmart
//{
//    /// <summary>
//    /// This class detects all connected SainSmartIO units and configures them.
//    /// </summary>
//    public class SainSmartIOAutoConfigurator : IAutoConfigOutputController
//    {
//        #region IAutoDetectOutputController Member

//        /// <summary>
//        /// This method detects and configures SainSmartIO controllers automatically
//       /// </summary>
//        /// <param name="Cabinet">The cabinet object to which the automatically detected IOutputController objects are added if necessary.</param>
//        public void AutoConfig(Cabinet Cabinet)
//        {
//            foreach (int Id in relayBoard.Instance.SainSmartIOGetIdList())
//            {
//                if (!Cabinet.OutputControllers.Any(oc => oc is SainSmartIO && ((SainSmartIO)oc).Id == Id))
//                {
//                    SainSmartIO PL = new SainSmartIO();
//                    PL.Id = Id;
                   
//                    if (!Cabinet.OutputControllers.Contains(PL.Name))
//                    {
//                        Cabinet.OutputControllers.Add(PL);

//                        Log.Write("Detected and added SainSmartIO Id {0} with name {1}".Build(PL.Id, PL.Name));


//                        if (!!Cabinet.Toys.Any(T => T is LedWizEquivalent && (((LedWizEquivalent)T).LedWizNumber == ((PL.Id - 1) * 2) + 20 || ((LedWizEquivalent)T).LedWizNumber == ((PL.Id - 1) * 2) + 20+1)))
//                        {
//                            LedWizEquivalent LWE = new LedWizEquivalent();
//                            LWE.LedWizNumber = (PL.Id - 1) * 2 + 20;
//                            LWE.Name = "{0} Equivalent 1".Build(PL.Name);
//                            for (int i = 1; i <= 32; i++)
//                            {
                                

//                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(PL.Name,i), LedWizEquivalentOutputNumber = i };
//                                LWE.Outputs.Add(LWEO);

//                            }
//                            if (!Cabinet.Toys.Contains(LWE.Name))
//                            {
//                                Cabinet.Toys.Add(LWE);
//                            }

//                            LWE = new LedWizEquivalent();
//                            LWE.LedWizNumber = (PL.Id - 1) * 2 + 20 + 1;
//                            LWE.Name = "{0} Equivalent 2".Build(PL.Name);
//                            for (int i = 1; i <= 32; i++)
//                            {
                                

//                                LedWizEquivalentOutput LWEO = new LedWizEquivalentOutput() { OutputName = "{0}\\{0}.{1:00}".Build(PL.Name, i+32), LedWizEquivalentOutputNumber = i };
//                                LWE.Outputs.Add(LWEO);


//                            }
//                            if (!Cabinet.Toys.Contains(LWE.Name))
//                            {
//                                Cabinet.Toys.Add(LWE);
//                                Log.Write("Added LedwizEquivalent Nr. {0} with name {1} for SainSmartIO with Id {2}".Build(LWE.LedWizNumber, LWE.Name, PL.Id));

//                            }


//                        }


//                    }
//                }


//            }

//        }

//        #endregion
//    }
//}
