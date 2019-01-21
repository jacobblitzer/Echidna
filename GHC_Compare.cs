using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;

using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino;
using Rhino.Geometry.Intersect;
using Rhino.Collections;
using EchidnaUtilities;

namespace Echidna
{
    public class GHC_Compare : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_Compare class.
        /// </summary>
        public GHC_Compare()
          : base("GHC_Compare", "GHC_Compare",
              "GHC_Compare",
              "Extra", "Echidna")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Signature", "Signature", "Signature", GH_ParamAccess.item);
            pManager.AddGenericParameter("Conditions", "Conditions", "Conditions", GH_ParamAccess.list);
            pManager.AddGenericParameter("CurrentTier", "CurrentTier", "CurrentTier", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Compatility", "Compatility", "Compatility", GH_ParamAccess.list);
            pManager.AddTextParameter("Options(text)", "Options(text)", "Options(text)", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Options(Index)", "Options(Index)", "Options(Index)", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Get signature
            Dictionary<string, IGH_Goo> thedictionary = new Dictionary<string, IGH_Goo>();
            DA.GetData<Dictionary<string, IGH_Goo>>(0, ref thedictionary);

           
            //Get Conditions
            List<IGH_Goo> Conditions = new List<IGH_Goo>();
            DA.GetDataList(1, Conditions);

            //GetCurrentTier
            int currenttier = -1;
            DA.GetData(2, ref currenttier);
            
            List<int> CompatibleIndices = new List<int>();
            List<string> compatiblenames = new List<string>();

            //Compare
            int count  = 0;
            List<bool> Compatability = new List<bool>();
            foreach (List<IGH_Goo> cond in Conditions)
            {
                int strategyindex = -1;
                string strategyname = "";
                bool compatible = utils.CheckCompatability(thedictionary,cond,currenttier,ref strategyname, ref strategyindex);
                Compatability.Add(compatible);
                if (compatible)
                {
                    CompatibleIndices.Add(strategyindex);
                    compatiblenames.Add(strategyname);
                }
                count++; 
            }
            DA.SetDataList(0, Compatability);
            DA.SetDataList(1, compatiblenames);
            DA.SetDataList(2, CompatibleIndices);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("840aa78f-eb3a-4934-b8d0-3c6365c7b1e8"); }
        }
    }
}