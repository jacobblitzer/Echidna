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
    public class GHC_CreateConditions : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GHC_CreateConditions()
          : base("GHC_CreateConditions", "GHC_CreateConditions",
              "GHC_CreateConditions",
              "Extra", "Echidna")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
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
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Conditions", "Conditions", "Conditions", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
          
            int closuretype = 0;
            int minfaces = 0;
            int maxfaces = 0;
            int tritype = 0;
            int  quadtype = 0;
            int ngontype = 0;
            int isconvextype = 0;
            int isdisjointtype = 0;
            int selfintersecttype = 0;
            int  isvalidtype = 0;
            int isflattype = 0;
            string strategyname = "";
            int strategyindex = 0;
            List<int> x = new List<int>();
            int randomizationweight = 0;

            DA.GetData(0, ref closuretype);
            DA.GetData(1, ref minfaces);
            DA.GetData(2, ref maxfaces);
            DA.GetData(3, ref tritype);
            DA.GetData(4, ref quadtype);
            DA.GetData(5, ref ngontype);
            DA.GetData(6, ref isconvextype);
            DA.GetData(7, ref isdisjointtype);
            DA.GetData(8, ref selfintersecttype);
            DA.GetData(9, ref isvalidtype);
            DA.GetData(10, ref isflattype);
            DA.GetData(11, ref strategyname);
            DA.GetData(12, ref strategyindex);
            DA.GetDataList<int>(13, x);
            DA.GetData(14, ref randomizationweight);

            string permittedtiers = utils.CreateIntString(x);

            IGH_Goo closuretypegoo = GH_Convert.ToGoo(closuretype);
            IGH_Goo minfacesgoo = GH_Convert.ToGoo(minfaces);
            IGH_Goo maxfacesgoo = GH_Convert.ToGoo(maxfaces);
            IGH_Goo tritypegoo = GH_Convert.ToGoo(tritype);
            IGH_Goo quadtypegoo = GH_Convert.ToGoo(quadtype);
            IGH_Goo ngontypegoo = GH_Convert.ToGoo(ngontype);
            IGH_Goo isconvextypegoo = GH_Convert.ToGoo(isconvextype);
            IGH_Goo isdisjointtypegoo = GH_Convert.ToGoo(isdisjointtype);
            IGH_Goo selfintersecttypegoo = GH_Convert.ToGoo(selfintersecttype);
            IGH_Goo isvalidtypegoo = GH_Convert.ToGoo(isvalidtype);
            IGH_Goo isflattypegoo = GH_Convert.ToGoo(isflattype);
            IGH_Goo strategynamegoo = GH_Convert.ToGoo(strategyname);
            IGH_Goo strategyindexgoo = GH_Convert.ToGoo(strategyindex);
            IGH_Goo permittedtiersgoo = GH_Convert.ToGoo(permittedtiers);
            IGH_Goo randomizationweightgoo = GH_Convert.ToGoo(randomizationweight);


            List<IGH_Goo> conditions = new List<IGH_Goo>() {closuretypegoo, minfacesgoo, maxfacesgoo, tritypegoo, quadtypegoo, ngontypegoo, isconvextypegoo, isdisjointtypegoo, selfintersecttypegoo, isvalidtypegoo,isflattypegoo};
      
            DA.SetData(0, conditions);
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
            get { return new Guid("6460d64e-5f3e-42c7-8b63-b3a792d28e0f"); }
        }
    }
}