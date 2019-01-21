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

namespace RawPlugins
{
    public class GHC_ReadConditions : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_ReadConditions class.
        /// </summary>
        public GHC_ReadConditions()
          : base("GHC_ReadConditions", "GHC_ReadConditions",
              "GHC_ReadConditions",
              "Extra", "Echidna")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Conditions", "Conditions", "Conditions", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("ClosureType", "ClosureType", "ClosureType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MinFaces", "MinFaces", "MinFaces", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MaxFaces", "MaxFaces", "MaxFaces", GH_ParamAccess.item);
            pManager.AddIntegerParameter("TriType", "TriType", "TriType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("QuadType", "QuadType", "QuadType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("NgonType", "NgonType", "NgonType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("IsConvexType", "IsConvexType", "IsConvexType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("IsDisjointType", "IsDisjointType", "IsDisjointType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("SelfIntersectType", "SelfIntersectType", "SelfIntersectType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("IsValidType", "IsValidType", "IsValidType", GH_ParamAccess.item);
            pManager.AddIntegerParameter("IsFlatType", "IsFlatType", "IsFlatType", GH_ParamAccess.item);
            pManager.AddTextParameter("StrategyName", "StrategyName", "StrategyName", GH_ParamAccess.item);
            pManager.AddIntegerParameter("StrategyIndex", "StrategyIndex", "StrategyIndex", GH_ParamAccess.item);
            pManager.AddIntegerParameter("PermittedTiers", "PermittedTiers", "PermittedTiers", GH_ParamAccess.list);
            pManager.AddIntegerParameter("randomizationweight", "randomizationweight", "randomizationweight", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<IGH_Goo> theconditions = new List<IGH_Goo>();
            DA.GetData<List<IGH_Goo>>(0, ref theconditions);

            List<int> candidates = new List<int>();
            String strategyname = "";
            int strategyindex = -1;
            List<int> permittedtiers = new List<int>();
            int randomizationweight = -1;
            utils.ReadConditions(theconditions, ref candidates, ref strategyname, ref strategyindex, ref permittedtiers, ref randomizationweight);
            /* Interval Facedomain = new Interval(candidates[1], candidates[2]);
            int closuretype = candidates[0];
            int minfaces = candidates[1];
            int maxfaces = candidates[2];
            int tritype = candidates[3];
            int quadtype = candidates[4];
            int ngontype = candidates[5];
            int isconvextype = candidates[6];
            int isdisjointtype = candidates[7];
            int selfintersecttype = candidates[8];
            int isvalidtype = candidates[9];
            int isflattype = candidates[10];*/

            DA.SetData(0, candidates[0]);
            DA.SetData(1, candidates[1]);
            DA.SetData(2, candidates[2]);
            DA.SetData(3, candidates[3]);
            DA.SetData(4, candidates[4]);
            DA.SetData(5, candidates[5]);
            DA.SetData(6, candidates[6]);
            DA.SetData(7, candidates[7]);
            DA.SetData(8, candidates[8]);
            DA.SetData(9, candidates[9]);
            DA.SetData(10, candidates[10]);

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
            get { return new Guid("1dda01a0-fb25-464a-a8c9-70470ab2a24e"); }
        }
    }
}